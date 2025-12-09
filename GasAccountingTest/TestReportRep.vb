Imports System.Data.Entity
Imports GasAccounting
Imports Moq

<TestClass()>
Public Class TestReportRep
    Private _mockContext As Mock(Of gas_accounting_systemEntities)
    Private _reportRep As ReportRep

    <TestInitialize()>
    Public Sub Setup()
        _mockContext = New Mock(Of gas_accounting_systemEntities)()
        _reportRep = New ReportRep(_mockContext.Object)
    End Sub

    ''' <summary>
    ''' 建立 Mock DbSet 的輔助方法 (適用於 EF6)
    ''' </summary>
    Private Function CreateMockDbSet(Of T As Class)(data As List(Of T)) As Mock(Of DbSet(Of T))
        Dim queryable = data.AsQueryable()
        Dim mockSet = New Mock(Of DbSet(Of T))()

        ' 設定 IQueryable 的行為
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.Provider).Returns(queryable.Provider)
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.Expression).Returns(queryable.Expression)
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.ElementType).Returns(queryable.ElementType)
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.GetEnumerator()).Returns(queryable.GetEnumerator())

        Return mockSet
    End Function

    ''' <summary>
    ''' 設定 AsNoTracking 回傳同一個 DbSet，以便模擬查詢行為
    ''' </summary>
    Private Sub SetupAsNoTracking(Of T As Class)(mockSet As Mock(Of DbSet(Of T)))
        mockSet.Setup(Function(m) m.AsNoTracking()).Returns(mockSet.Object)
    End Sub

    <TestMethod("單一客戶每月應收帳-客戶代號不存在")>
    Public Sub MonthlyCustomerReceivable_WithoutCusCode()
        ' Arrange
        Dim testDate = New Date(2024, 1, 15)
        Dim invalidCusCode = "INVALID"

        ' 準備空的客戶資料
        Dim customers = New List(Of customer)()
        Dim mockCustomers = CreateMockDbSet(customers)
        _mockContext.Setup(Function(c) c.customers).Returns(mockCustomers.Object)

        ' Act & Assert
        Dim ex = Assert.ThrowsException(Of Exception)(
            Sub()
                _reportRep.MonthlyCustomerReceivable(testDate, invalidCusCode)
            End Sub
        )

        Assert.AreEqual("查無此客戶代號", ex.Message)
    End Sub

    <TestMethod("單一客戶每月應收帳-無訂單資料")>
    Public Sub MonthlyCustomerReceivable_WithoutOrder()
        ' Arrange
        Dim testDate = New Date(2024, 1, 15)
        Dim cusCode = "C001"

        Dim testCustomer = New customer With {
            .cus_id = 1,
            .cus_code = "C001",
            .cus_name = "測試客戶"
        }

        Dim customers = New List(Of customer) From {testCustomer}
        Dim orders = New List(Of order)() ' 空訂單

        Dim mockCustomers = CreateMockDbSet(customers)
        Dim mockOrders = CreateMockDbSet(orders)

        _mockContext.Setup(Function(c) c.customers).Returns(mockCustomers.Object)
        _mockContext.Setup(Function(c) c.orders).Returns(mockOrders.Object)

        ' Act
        Dim result = _reportRep.MonthlyCustomerReceivable(testDate, cusCode)

        ' Assert
        Assert.IsNotNull(result)
        Assert.AreEqual(31, result.Count, "一月份應該有 31 天")

        ' 所有日期的總提氣應該都是 0
        For Each item In result
            Assert.AreEqual(0, item.總提氣, $"{item.日期} 的總提氣應該是 0")
            Assert.AreEqual(0, item.總額, $"{item.日期} 的總額應該是 0")
        Next
    End Sub

    <TestMethod("單一客戶每月應收帳-正確計算單價和金額")>
    Public Sub MonthlyCustomerReceivable_CorrectPrice()
        ' Arrange
        Dim testDate = New Date(2024, 2, 1) ' 使用2月測試不同天數
        Dim cusCode = "C002"

        Dim testCustomer = New customer With {
            .cus_id = 2,
            .cus_code = "C002",
            .cus_name = "單價測試客戶"
        }

        Dim customers = New List(Of customer) From {testCustomer}

        ' 準備一筆訂單用於測試單價計算
        Dim orders = New List(Of order) From {
            New order With {
                .o_id = 1,
                .o_cus_Id = 2,
                .o_date = New Date(2024, 2, 10),
                .o_in_out = "出場單",
                .o_delivery_type = "廠運",
                .o_gas_total = 200,
                .o_gas_c_total = 100,
                .o_return = 10,
                .o_return_c = 5,
                .o_UnitPrice = 15,
                .o_UnitPriceC = 18,
                .customer = testCustomer
            }
        }

        Dim mockCustomers = CreateMockDbSet(customers)
        Dim mockOrders = CreateMockDbSet(orders)

        _mockContext.Setup(Function(c) c.customers).Returns(mockCustomers.Object)
        _mockContext.Setup(Function(c) c.orders).Returns(mockOrders.Object)

        ' Act
        Dim result = _reportRep.MonthlyCustomerReceivable(testDate, cusCode)

        ' Assert
        Dim day10 = result.FirstOrDefault(Function(x) x.日期 = "02月10日")
        Assert.IsNotNull(day10)

        ' 驗證數量計算
        Assert.AreEqual(210, day10.廠運普氣, "廠運普氣 = 200 + 10")
        Assert.AreEqual(105, day10.廠運丙氣, "廠運丙氣 = 100 + 5")

        ' 驗證金額計算
        Assert.AreEqual(3000, day10.廠運普氣金額, "廠運普氣金額 = 200 * 15")
        Assert.AreEqual(1800, day10.廠運丙氣金額, "廠運丙氣金額 = 100 * 18")

        ' 驗證總計
        Assert.AreEqual(315, day10.廠運總提氣, "廠運總提氣 = 210 + 105")
        Assert.AreEqual(4800, day10.總額, "總額 = 3000 + 1800")
        Assert.AreEqual(4800, day10.掛帳, "掛帳應該等於總額")
    End Sub

    <TestMethod("銀行帳-包含現金轉入銀行")>
    Public Sub GetBankAccount_ShouldIncludeCashToBank()
        ' Arrange
        Dim targetMonth = New Date(2024, 5, 1)
        Dim bankId = 1

        ' 科目與相關資料
        Dim bankSubject = New subject With {.s_name = "銀行存款"}
        Dim incomeSubject = New subject With {.s_name = "銷貨收入"}
        Dim paymentSubject = New subject With {.s_name = "進貨支出"}
        Dim customer1 = New customer With {.cus_id = 1, .cus_code = "C001", .cus_name = "客戶A"}
        Dim company1 = New company With {.comp_id = 1, .comp_name = "廠商A"}

        ' 上期月結餘額
        Dim lastMonthlyBalance = New bank_monthly_balances With {
            .bm_Id = 1,
            .bm_bank_Id = bankId,
            .bm_Month = New Date(2024, 4, 1),
            .bm_ClosingBalance = 1000
        }

        ' 本期交易
        Dim collections = New List(Of collection) From {
            New collection With {
                .col_Id = 1,
                .col_Date = New Date(2024, 5, 5),
                .col_Type = "銀行存款",
                .col_bank_Id = bankId,
                .col_Amount = 200,
                .col_Memo = "銀行收入",
                .subject = incomeSubject,
                .customer = customer1
            },
            New collection With {' 現金轉入銀行：col_Type=現金，subject=銀行存款
                .col_Id = 2,
                .col_Date = New Date(2024, 5, 10),
                .col_Type = "現金",
                .col_bank_Id = bankId,
                .col_Amount = 300,
                .col_Memo = "現金轉存",
                .subject = bankSubject,
                .customer = customer1
            }
        }

        Dim payments = New List(Of payment) From {
            New payment With {
                .p_Id = 1,
                .p_Date = New Date(2024, 5, 15),
                .p_Type = "銀行存款",
                .p_bank_Id = bankId,
                .p_Amount = 150,
                .p_Memo = "銀行支出",
                .subject = paymentSubject,
                .company = company1
            }
        }

        Dim bankMonthlyBalances = New List(Of bank_monthly_balances) From {lastMonthlyBalance}

        ' Mock DbSet
        Dim mockCollections = CreateMockDbSet(collections)
        Dim mockPayments = CreateMockDbSet(payments)
        Dim mockBankMonthlyBalances = CreateMockDbSet(bankMonthlyBalances)

        SetupAsNoTracking(mockCollections)
        SetupAsNoTracking(mockPayments)
        SetupAsNoTracking(mockBankMonthlyBalances)

        _mockContext.Setup(Function(c) c.collections).Returns(mockCollections.Object)
        _mockContext.Setup(Function(c) c.payments).Returns(mockPayments.Object)
        _mockContext.Setup(Function(c) c.bank_monthly_balances).Returns(mockBankMonthlyBalances.Object)

        ' Act
        Dim result = _reportRep.GetBankAccount(targetMonth, bankId)

        ' Assert
        Assert.IsNotNull(result)
        Assert.AreEqual("2024年05月", result.日期)
        Assert.IsNotNull(result.List)
        Assert.AreEqual(4, result.List.Count, "上期結餘 + 2 收入 + 1 支出")

        ' 逐筆檢查
        Dim initial = result.List(0)
        Assert.AreEqual("上期結餘", initial.摘要)
        Assert.AreEqual(1000, initial.餘額)

        Dim income1 = result.List(1)
        Assert.AreEqual("銷貨收入", income1.科目)
        Assert.AreEqual(200, income1.借方)
        Assert.AreEqual(0, income1.貸方)
        Assert.AreEqual(1200, income1.餘額)

        Dim cashToBank = result.List(2)
        Assert.AreEqual("現金", cashToBank.科目, "現金轉入銀行時科目應為 col_Type")
        Assert.AreEqual(300, cashToBank.借方)
        Assert.AreEqual(0, cashToBank.貸方)
        Assert.AreEqual(1500, cashToBank.餘額, "應累加借方金額")

        Dim payment1 = result.List(3)
        Assert.AreEqual("進貨支出", payment1.科目)
        Assert.AreEqual(0, payment1.借方)
        Assert.AreEqual(150, payment1.貸方)
        Assert.AreEqual(1350, payment1.餘額, "支出應扣減餘額")
    End Sub

    <TestCleanup()>
    Public Sub Cleanup()
        _mockContext = Nothing
        _reportRep = Nothing
    End Sub
End Class