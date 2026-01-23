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

        ' 模擬 Find 方法 (支援單一主鍵的情況)
        mockSet.Setup(Function(m) m.Find(It.IsAny(Of Object()))).Returns(
            Function(keyValues As Object())
                Dim targetId = keyValues(0)
                Dim className = GetType(T).Name.ToLower()
                ' 優先尋找 [類名]_id 或 id，若找不到才找結尾為 id 的屬性
                Dim idProp = If(GetType(T).GetProperties().FirstOrDefault(Function(p)
                                                                              Dim pName = p.Name.ToLower()
                                                                              Return pName = className & "_id" OrElse pName = className & "id" OrElse pName = "id"
                                                                          End Function), GetType(T).GetProperties().FirstOrDefault(Function(p) p.Name.ToLower().EndsWith("id")))

                If idProp IsNot Nothing Then
                    ' 使用字串比較以避免 Integer/Object 類型不匹配問題
                    Return data.FirstOrDefault(Function(x)
                                                   Dim val = idProp.GetValue(x)
                                                   Return val IsNot Nothing AndAlso val.ToString() = targetId.ToString()
                                               End Function)
                End If
                Return Nothing
            End Function)

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
        Dim collections As New List(Of collection)

        Dim mockCustomers = CreateMockDbSet(customers)
        Dim mockOrders = CreateMockDbSet(orders)
        Dim mockCollections = CreateMockDbSet(collections)

        _mockContext.Setup(Function(c) c.customers).Returns(mockCustomers.Object)
        _mockContext.Setup(Function(c) c.orders).Returns(mockOrders.Object)
        _mockContext.Setup(Function(x) x.collections).Returns(mockCollections.Object)

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
        Dim collections As New List(Of collection)

        Dim mockCustomers = CreateMockDbSet(customers)
        Dim mockOrders = CreateMockDbSet(orders)
        Dim mockCollections = CreateMockDbSet(collections)

        _mockContext.Setup(Function(c) c.customers).Returns(mockCustomers.Object)
        _mockContext.Setup(Function(c) c.orders).Returns(mockOrders.Object)
        _mockContext.Setup(Function(x) x.collections).Returns(mockCollections.Object)

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

    <TestMethod("銀行帳-自訂測試資料")>
    Public Sub GetBankAccount_CustomData()
        ' Arrange
        Dim targetMonth = New Date(2024, 5, 1) ' 請設定測試月份

        ' 1. 準備科目與客戶/廠商資料 (視需要自行修改)
        Dim bankSubject = New subject With {.s_name = "銀行存款"}
        Dim incomeSubject = New subject With {.s_name = "銷貨收入"}
        Dim paymentSubject1 = New subject With {.s_name = "進貨"}
        Dim paymentSubject2 = New subject With {.s_name = "郵電費"}
        Dim customer1 = New customer With {.cus_id = 1, .cus_code = "C001", .cus_name = "測試客戶"}
        Dim company1 = New company With {.comp_id = 1, .comp_name = "台灣中油股份有限公司"}
        Dim bank = New bank With {.bank_id = 1, .bank_name = "測試銀行"}
        Dim banks As New List(Of bank) From {
            bank
        }


        ' 2. 準備上期銀行月結餘額 (若無則維持 List 為空)
        Dim bankMonthlyBalances = New List(Of bank_monthly_balances) From {
             New bank_monthly_balances With {
                 .bm_Id = 1,
                 .bm_bank_Id = bank.bank_id,
                 .bm_Month = New Date(2024, 4, 1),
                 .bm_ClosingBalance = 10000,
                 .bank = bank
             }
        }

        ' 3. 準備收款資料 (Collections)
        Dim collections = New List(Of collection) From {
             New collection With {
                 .col_Id = 1,
                 .col_Date = New Date(2024, 5, 5),
                 .col_Type = "銀行存款",
                 .col_bank_Id = bank.bank_id,
                 .col_Amount = 5000,
                 .col_Memo = "收到貨款",
                 .subject = incomeSubject,
                 .customer = customer1,
                 .bank = bank
             },
             New collection With {
                 .col_Id = 2,
                 .col_Date = New Date(2024, 5, 6),
                 .col_Type = "現金",
                 .col_Amount = 2000,
                 .col_credit_bank_id_2 = bank.bank_id,  ' Split 2 is Bank
                 .col_credit_amount_2 = 1500,           ' Amount for Bank
                 .col_Memo = "拆分提款",
                 .subject2 = bankSubject,               ' Subject is Bank (Withdrawal)
                 .customer = customer1
             }
        }

        ' 4. 準備付款資料 (Payments)
        Dim payments = New List(Of payment) From {
             New payment With {
                 .p_Id = 1,
                 .p_Date = New Date(2024, 5, 10),
                 .p_Type = "銀行存款",
                 .p_bank_Id = bank.bank_id,
                 .p_Amount = 1000,
                 .subject = paymentSubject1,
                 .company = company1,
                 .bank = bank,
                 .p_Memo = "一般付款"
             },
             New payment With {
                 .p_Id = 2,
                 .p_Date = New Date(2024, 5, 11),
                 .p_Type = "現金",
                 .p_Amount = 3000,
                 .p_debit_bank_id_2 = bank.bank_id,    ' Split 2 is Bank
                 .p_debit_amount_2 = 2500,             ' Amount for Bank
                 .subject2 = bankSubject,              ' Subject is Bank (Deposit)
                 .company = company1,
                 .p_Memo = "拆分存款"
             }
        }

        ' 建立 Mock (不用修改)
        Dim mockCollections = CreateMockDbSet(collections)
        Dim mockPayments = CreateMockDbSet(payments)
        Dim mockBankMonthlyBalances = CreateMockDbSet(bankMonthlyBalances)
        Dim mockBank = CreateMockDbSet(banks)

        SetupAsNoTracking(mockCollections)
        SetupAsNoTracking(mockPayments)
        SetupAsNoTracking(mockBankMonthlyBalances)
        SetupAsNoTracking(mockBank)

        _mockContext.Setup(Function(c) c.collections).Returns(mockCollections.Object)
        _mockContext.Setup(Function(c) c.payments).Returns(mockPayments.Object)
        _mockContext.Setup(Function(c) c.bank_monthly_balances).Returns(mockBankMonthlyBalances.Object)
        _mockContext.Setup(Function(x) x.banks).Returns(mockBank.Object)

        ' 補足其他必要的實體 Mock
        Dim mockCustomers = CreateMockDbSet(New List(Of customer) From {customer1})
        Dim mockCompanies = CreateMockDbSet(New List(Of company) From {company1})
        Dim mockSubjects = CreateMockDbSet(New List(Of subject) From {bankSubject, incomeSubject, paymentSubject1, paymentSubject2})
        _mockContext.Setup(Function(c) c.customers).Returns(mockCustomers.Object)
        _mockContext.Setup(Function(c) c.companies).Returns(mockCompanies.Object)
        _mockContext.Setup(Function(c) c.subjects).Returns(mockSubjects.Object)

        ' Act
        Dim result = _reportRep.GetBankAccount(targetMonth, bank.bank_id)

        ' Assert
        ' 您可以在此加入斷言來驗證結果，或使用中斷點查看 result
        Assert.IsNotNull(result)
        Console.WriteLine($"日期: {result.日期}")
        For Each item In result.List
            Console.WriteLine($"日期: {item.日期}, 摘要: {item.摘要}, 借貸: {item.借方}/{item.貸方}, 餘額: {item.餘額}")
        Next
    End Sub

    <TestMethod("月對帳單-資料正確性")>
    Public Sub GetMonthlyStatement_DataCorrect()
        ' Arrange
        Dim testDate = New Date(2025, 12, 17)
        Dim testCusCode = "C001"

        Dim cus1 As New customer With {
            .cus_id = 1,
            .cus_code = "C001",
            .cus_name = "測試客戶一",
            .cus_InsurancePrice = 1
        }

        Dim orders = New List(Of order) From {
             New order With {
                .o_date = New Date(2025, 12, 1),
                .o_gas_total = 100,
                .o_UnitPrice = 10,
                .o_gas_c_total = 200,
                .o_UnitPriceC = 20,
                .o_BarrelPrice = 10,
                .o_delivery_type = "自運",
                .customer = cus1
             },
            New order With {
                .o_date = New Date(2025, 12, 1),
                .o_gas_total = 100,
                .o_UnitPrice = 10,
                .o_gas_c_total = 200,
                .o_UnitPriceC = 20,
                .o_BarrelPrice = 10,
                .o_delivery_type = "廠運",
                .customer = cus1
             }
        }

        Dim collections = New List(Of collection) From {
            New collection With { ' 當月帳款
                .col_AccountMonth = New Date(2025, 12, 1),
                .col_cus_Id = 1,
                .col_Amount = 100
            },
            New collection With { ' 非當月帳款
                .col_AccountMonth = New Date(2025, 11, 1),
                .col_cus_Id = 1,
                .col_Amount = 100
            }
        }

        Dim scrapBarrels = New List(Of scrap_barrel)
        Dim scrapBarrelDetails = New List(Of scrap_barrel_detail)

        Dim mockOrders = CreateMockDbSet(orders)
        Dim mockCollections = CreateMockDbSet(collections)
        Dim mockSBs = CreateMockDbSet(scrapBarrels)
        Dim mockSBDs = CreateMockDbSet(scrapBarrelDetails)

        SetupAsNoTracking(mockOrders)
        SetupAsNoTracking(mockCollections)

        _mockContext.Setup(Function(x) x.collections).Returns(mockCollections.Object)
        _mockContext.Setup(Function(x) x.orders).Returns(mockOrders.Object)
        _mockContext.Setup(Function(x) x.scrap_barrel).Returns(mockSBs.Object)
        _mockContext.Setup(Function(x) x.scrap_barrel_detail).Returns(mockSBDs.Object)

        ' Act
        Dim result = _reportRep.GetMonthlyStatement(testCusCode, testDate)

        ' Assert
        ' 期待結果為 100，因為只應包含當月帳款。若包含非當月帳款，總額會變成 200。
        Assert.AreEqual(100, result.GasAccountsReceived, "應只包含指定帳款月份的收款")
    End Sub

    <TestCleanup()>
    Public Sub Cleanup()
        _mockContext = Nothing
        _reportRep = Nothing
    End Sub
End Class