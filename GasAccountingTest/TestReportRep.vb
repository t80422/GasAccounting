Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Moq
Imports System.Data.Entity
Imports GasAccounting

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

    <TestMethod("單一客戶每月應收帳-返回整月客戶應收帳明細")>
    Public Sub MonthlyCustomerReceivable_correct()
        ' Arrange - 準備測試資料
        Dim testDate = New Date(2024, 1, 15)
        Dim cusCode = "C001"

        ' 準備客戶資料
        Dim testCustomer = New customer With {
            .cus_id = 1,
            .cus_code = "C001",
            .cus_name = "測試客戶"
        }

        Dim customers = New List(Of customer) From {testCustomer}

        ' 準備訂單資料 - 包含廠運和自運的訂單
        Dim orders = New List(Of order) From {
            New order With { ' 1月5日 - 廠運訂單
                .o_id = 1,
                .o_cus_Id = 1,
                .o_date = New Date(2024, 1, 5),
                .o_in_out = "出場單",
                .o_delivery_type = "廠運",
                .o_gas_total = 100,
                .o_gas_c_total = 50,
                .o_return = 5,
                .o_return_c = 3,
                .o_UnitPrice = 10,
                .o_UnitPriceC = 12,
                .customer = testCustomer
            },
            New order With {' 1月10日 - 自運訂單
                .o_id = 2,
                .o_cus_Id = 1,
                .o_date = New Date(2024, 1, 10),
                .o_in_out = "出場單",
                .o_delivery_type = "自運",
                .o_gas_total = 80,
                .o_gas_c_total = 40,
                .o_return = 4,
                .o_return_c = 2,
                .o_UnitPrice = 9,
                .o_UnitPriceC = 11,
                .customer = testCustomer
            },
            New order With {' 1月15日 - 混合訂單（廠運+自運）
                .o_id = 3,
                .o_cus_Id = 1,
                .o_date = New Date(2024, 1, 15),
                .o_in_out = "出場單",
                .o_delivery_type = "廠運",
                .o_gas_total = 60,
                .o_gas_c_total = 30,
                .o_return = 3,
                .o_return_c = 2,
                .o_UnitPrice = 10,
                .o_UnitPriceC = 12,
                .customer = testCustomer
            }
        }

        ' 設定 Mock DbSet
        Dim mockCustomers = CreateMockDbSet(customers)
        Dim mockOrders = CreateMockDbSet(orders)

        _mockContext.Setup(Function(c) c.customers).Returns(mockCustomers.Object)
        _mockContext.Setup(Function(c) c.orders).Returns(mockOrders.Object)

        ' Act - 執行測試
        Dim result = _reportRep.MonthlyCustomerReceivable(testDate, cusCode)

        ' Assert - 驗證結果
        Assert.IsNotNull(result, "結果不應該是 Nothing")
        Assert.AreEqual(31, result.Count, "一月份應該有 31 天的資料")

        ' 驗證第一筆資料（1月1日）- 應該沒有訂單
        Dim firstDay = result.FirstOrDefault(Function(x) x.日期 = "01月01日")
        Assert.IsNotNull(firstDay)
        Assert.AreEqual("測試客戶", firstDay.客戶名稱)
        Assert.AreEqual(0, firstDay.總提氣)

        ' 驗證1月5日的資料 - 有廠運訂單
        Dim day5 = result.FirstOrDefault(Function(x) x.日期 = "01月05日")
        Assert.IsNotNull(day5)
        Assert.AreEqual(105, day5.廠運普氣, "廠運普氣 = 100 + 5(退氣)")
        Assert.AreEqual(53, day5.廠運丙氣, "廠運丙氣 = 50 + 3(退氣)")
        Assert.AreEqual(10.0, day5.廠運普氣單價)
        Assert.AreEqual(12.0, day5.廠運丙氣單價)
        Assert.AreEqual(1050 + 636, day5.廠運普氣金額 + day5.廠運丙氣金額, "廠運總金額")

        ' 驗證1月10日的資料 - 有自運訂單
        Dim day10 = result.FirstOrDefault(Function(x) x.日期 = "01月10日")
        Assert.IsNotNull(day10)
        Assert.AreEqual(84, day10.自運普氣, "自運普氣 = 80 + 4(退氣)")
        Assert.AreEqual(42, day10.自運丙氣, "自運丙氣 = 40 + 2(退氣)")
        Assert.AreEqual(9.0, day10.自運普氣單價)
        Assert.AreEqual(11.0, day10.自運丙氣單價)

        ' 驗證累計金額是否正確遞增
        Dim day5Total = day5.總額
        Dim day10Total = day10.總額
        Assert.IsTrue(day10.累計 > day5.累計, "累計金額應該遞增")
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
        Assert.AreEqual(3150, day10.廠運普氣金額, "廠運普氣金額 = 210 * 15")
        Assert.AreEqual(1890, day10.廠運丙氣金額, "廠運丙氣金額 = 105 * 18")

        ' 驗證總計
        Assert.AreEqual(315, day10.廠運總提氣, "廠運總提氣 = 210 + 105")
        Assert.AreEqual(5040, day10.總額, "總額 = 3150 + 1890")
        Assert.AreEqual(5040, day10.掛帳, "掛帳應該等於總額")
    End Sub

    <TestCleanup()>
    Public Sub Cleanup()
        _mockContext = Nothing
        _reportRep = Nothing
    End Sub
End Class