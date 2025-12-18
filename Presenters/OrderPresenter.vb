Imports System.Drawing.Printing
Imports System.IO
Imports ClosedXML.Excel
Imports iText.Html2pdf
Imports iText.Html2pdf.Resolver.Font
Imports iText.Kernel.Geom
Imports iText.Kernel.Pdf
Imports Path = System.IO.Path

''' <summary>
''' 銷售管理
''' </summary>
Public Class OrderPresenter
    Private ReadOnly _view As IOrderView
    Private ReadOnly _cusRep As ICustomerRep
    Private ReadOnly _carRep As ICarRep
    Private ReadOnly _gbRep As IGasBarrelRep
    Private ReadOnly _ordRep As IOrderRep
    Private ReadOnly _service As IBarrelMonthlyBalanceService
    Private ReadOnly _priceCalSer As IPriceCalculationService
    Private ReadOnly _aeSer As IAccountingEntryService
    Private ReadOnly _printerSer As IPrinterService
    Private ReadOnly _ocmSer As IOrderCollectionMappingService
    Private ReadOnly _maService As IMonthlyAccountService
    Private ReadOnly _reportRep As IReportRep
    Private ReadOnly _unitOfWork As IUnitOfWork
    Private ReadOnly _logger As ILoggerService
    Private ReadOnly _currentUserService As ICurrentUserService
    Private ReadOnly _barrelInvSer As IBarrelInventoryService

    Private currentCustomer As customer
    Private currentOrder As New order
    Private initCusStk As New GasBarrelDTO
    Public currentCar As car

    'edit by kevin 20251109
    Public InitCustStock(999)() As Integer


    ' 常量：所有瓦斯桶規格
    Private Shared ReadOnly BARREL_TYPES As String() = {"50", "20", "16", "10", "4", "18", "14", "5", "2"}

    Public ReadOnly Property View As IOrderView
        Get
            Return _view
        End Get
    End Property

    Private Sub RefreshCurrentEntities(Optional includeOrder As Boolean = False)
        If includeOrder AndAlso currentOrder IsNot Nothing Then
            _ordRep.Reload(currentOrder)
        End If

        If currentCustomer IsNot Nothing Then
            _cusRep.Reload(currentCustomer)
        End If

        If currentCar IsNot Nothing Then
            _carRep.Reload(currentCar)
        End If
    End Sub

#Region "建構函數與初始化"

    Public Sub New(view As IOrderView, cusRep As ICustomerRep, carRep As ICarRep, ordRep As IOrderRep, gbRep As IGasBarrelRep, barMBService As IBarrelMonthlyBalanceService,
                   priceCalSer As IPriceCalculationService, aeSer As IAccountingEntryService, printerSer As IPrinterService, ocmSer As IOrderCollectionMappingService,
                   reportRep As IReportRep, maService As IMonthlyAccountService, unitOfWork As IUnitOfWork, logger As ILoggerService, currentUserService As ICurrentUserService,
                   barrelInvSer As IBarrelInventoryService)
        _view = view
        _cusRep = cusRep
        _carRep = carRep
        _ordRep = ordRep
        _gbRep = gbRep
        _service = barMBService
        _priceCalSer = priceCalSer
        _aeSer = aeSer
        _printerSer = printerSer
        _ocmSer = ocmSer
        _maService = maService
        _reportRep = reportRep
        _unitOfWork = unitOfWork
        _logger = logger
        _currentUserService = currentUserService
        _barrelInvSer = barrelInvSer

        SubscribeToViewEvents()
    End Sub

    ''' <summary>
    ''' 訂閱 View 事件
    ''' </summary>
    Private Sub SubscribeToViewEvents()
        AddHandler _view.CancelRequest, AddressOf Initialize
        AddHandler _view.CustomerSelected, AddressOf OnCustomerSelected
        AddHandler _view.TransportTypeSelected, AddressOf OnTransportTypeSelected
        AddHandler _view.BarrelInInput, AddressOf OnBarrelIn
        AddHandler _view.BarrelOutInput, AddressOf OnBarrelOut
        AddHandler _view.BarrelUnitPriceInput, AddressOf CalculateBarrelAmount
        AddHandler _view.CarSelected, AddressOf OnCarSelected
        AddHandler _view.DepositInput, AddressOf CalculateDeposit
        AddHandler _view.OrderTypeChanged, AddressOf OnOrderTypeChanged
        AddHandler _view.ReturnInput, AddressOf CalculateGasAmount
        AddHandler _view.CreateRequest, AddressOf Add
        AddHandler _view.DataSelectedRequest, AddressOf LoadDetail
        AddHandler _view.UpdateRequest, AddressOf Update
        AddHandler _view.DeleteRequest, AddressOf Delete
        AddHandler _view.PrintRequest, AddressOf Print
        AddHandler _view.PrintCusStkRequest, AddressOf PrintCusStk
        AddHandler _view.CustomersGasDetailRequest, AddressOf GenerateCustomersGasDetailByDay
        AddHandler _view.CusGetGasListRequest, AddressOf GenerateCustomersGetGasList
        AddHandler _view.SearchRequest, AddressOf OnSearch
    End Sub

    Private Sub Initialize()
        Try
            _view.ClearInput()
            LoadList()
            _view.ButtonStatus(False)
            currentCustomer = Nothing
            currentOrder = Nothing
            currentCar = Nothing
            initCusStk = New GasBarrelDTO

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub LoadList(Optional criteria As OrderSearchCriteria = Nothing)
        Try
            ' 若沒有搜尋條件,預設今日的單
            If criteria Is Nothing Then
                criteria = New OrderSearchCriteria With {
                    .SearchIn = True,
                    .SearchOut = True,
                    .IsDate = True,
                    .StartDate = Today.Date,
                    .EndDate = Today.Date
                }
            End If

            Dim datas = _ordRep.SearchAsync(criteria).Result

            _view.ClearInput()

            Dim dataList As IEnumerable(Of Object) = Nothing

            If criteria.SearchOut = False Then
                dataList = datas.Select(Function(x) New OrderListInVM(x)).ToList
            ElseIf criteria.SearchIn = False Then
                dataList = datas.Select(Function(x) New OrderListOutVM(x)).ToList
            Else
                dataList = datas.Select(Function(x) New OrderListVM(x)).ToList
            End If

            _view.DisplayList(dataList)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

#End Region

#Region "搜尋與列表"

    Private Sub OnSearch()
        Try
            Dim data = _view.GetSearchCriteria
            LoadList(data)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

#End Region

#Region "事件處理"

    Private Sub OnCustomerSelected(sender As Object, cusCode As String)
        Try
            LoadCustomer(cusCode)
            LoadUnitPrice()

            Dim data As New order With {
                .o_cus_50 = initCusStk.Barrel_50,
                .o_cus_20 = initCusStk.Barrel_20,
                .o_cus_16 = initCusStk.Barrel_16,
                .o_cus_10 = initCusStk.Barrel_10,
                .o_cus_4 = initCusStk.Barrel_4,
                .o_cus_18 = initCusStk.Barrel_18,
                .o_cus_14 = initCusStk.Barrel_14,
                .o_cus_5 = initCusStk.Barrel_5,
                .o_cus_2 = initCusStk.Barrel_2
            }

            _view.ShowCusBarrelStock(data)

            CaculateCusBarrelStock()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub OnCarSelected(sender As Object, carId As Integer)
        Try
            LoadCarBarrelStock(carId)
            CalculateDeposit()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub OnOrderTypeChanged()
        Try
            CaculateCusBarrelStock()
            CalculateBarrelAmount()
            CalculateDeposit()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub OnBarrelIn()
        Try
            CaculateCusBarrelStock()
            CalculateBarrelAmount()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub OnBarrelOut()
        Try
            CaculateCusBarrelStock()
            CalculateGasAmount()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub OnTransportTypeSelected()
        LoadCar()
        LoadUnitPrice()
    End Sub

#End Region

#Region "資料載入"

    ''' <summary>
    ''' 載入客戶
    ''' </summary>
    ''' <param name="cusCode"></param>
    Private Sub LoadCustomer(cusCode As String)
        Try
            Dim customerEntity = _cusRep.GetByCusCode(cusCode)

            If customerEntity IsNot Nothing Then
                _cusRep.Reload(customerEntity)
            End If

            currentCustomer = customerEntity

            If currentCustomer Is Nothing Then
                Throw New Exception("查無此客戶")
            End If

            _view.ShowCustomer(currentCustomer)

            With initCusStk
                .Barrel_50 = currentCustomer.cus_gas_50
                .Barrel_20 = currentCustomer.cus_gas_20
                .Barrel_16 = currentCustomer.cus_gas_16
                .Barrel_10 = currentCustomer.cus_gas_10
                .Barrel_4 = currentCustomer.cus_gas_4
                .Barrel_18 = currentCustomer.cus_gas_18
                .Barrel_14 = currentCustomer.cus_gas_14
                .Barrel_5 = currentCustomer.cus_gas_5
                .Barrel_2 = currentCustomer.cus_gas_2
            End With
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' 載入單價
    ''' </summary>
    Private Sub LoadUnitPrice()
        ' 瓦斯單價
        Dim inputOrder = _view.GetOrderInput
        Dim isDelivery = inputOrder.o_delivery_type = "廠運"

        ' 瓦斯桶單價
        Dim lastOrder = _ordRep.GetLastOrder(currentCustomer.cus_id)

        Dim data As New order With {
            .o_UnitPrice = _priceCalSer.CalculateUnitPrice(currentCustomer, inputOrder.o_date, isDelivery, True),
            .o_UnitPriceC = _priceCalSer.CalculateUnitPrice(currentCustomer, inputOrder.o_date, isDelivery, False),
            .o_barrel_unit_price_10 = lastOrder?.o_barrel_unit_price_10,
            .o_barrel_unit_price_14 = lastOrder?.o_barrel_unit_price_14,
            .o_barrel_unit_price_16 = lastOrder?.o_barrel_unit_price_16,
            .o_barrel_unit_price_18 = lastOrder?.o_barrel_unit_price_18,
            .o_barrel_unit_price_2 = lastOrder?.o_barrel_unit_price_2,
            .o_barrel_unit_price_20 = lastOrder?.o_barrel_unit_price_20,
            .o_barrel_unit_price_4 = lastOrder?.o_barrel_unit_price_4,
            .o_barrel_unit_price_5 = lastOrder?.o_barrel_unit_price_5,
            .o_barrel_unit_price_50 = lastOrder?.o_barrel_unit_price_50,
            .o_insurance_unit_price = If(currentCustomer.cus_IsInsurance, currentCustomer.cus_InsurancePrice, 0) ' 保險單價
        }

        _view.ShowUnitPrice(data)
    End Sub

    Private Sub LoadCar()
        Try
            Dim cars = currentCustomer.cars.Select(Function(x) New SelectListItem With {.Display = $"{x.c_no}-{x.c_driver}", .Value = x.c_id}).ToList
            _view.SetCarDropdown(cars)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 載入寄桶結存瓶
    ''' </summary>
    Private Sub LoadCarBarrelStock(carId As Integer)
        Try
            currentCar = _carRep.GetByIdAsync(carId).Result
            _carRep.Reload(currentCar)
            _view.ShowCarBarrelStock_In(currentCar)
            _view.ShowCarBarrelStock_Out(currentCar)
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "計算方法"

    ''' <summary>
    ''' 計算結存瓶
    ''' </summary>
    Private Sub CaculateCusBarrelStock()
        If currentCustomer Is Nothing Then Return

        InitCustStock(30) = New Integer() {27, 101, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(50) = New Integer() {6, 42, 3, 2, 1, 0, 0, 0, 0}
        InitCustStock(60) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(61) = New Integer() {5, 124, 0, 0, 37, 0, 0, 0, 0}
        InitCustStock(62) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(70) = New Integer() {18, 72, 8, 4, 10, 0, 0, 0, 0}
        InitCustStock(71) = New Integer() {7, 67, 8, 2, 9, 0, 0, 0, 0}
        InitCustStock(80) = New Integer() {0, 6, 0, 0, 1, 0, 0, 0, 0}
        InitCustStock(90) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(100) = New Integer() {23, 26, 0, 1, 2, 0, 0, 0, 0}
        InitCustStock(110) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(120) = New Integer() {17, 81, 4, 2, 18, 0, 0, 0, 0}
        InitCustStock(130) = New Integer() {11, 15, 12, 2, 0, 0, 0, 0, 0}
        InitCustStock(140) = New Integer() {24, 61, 6, 7, 21, 0, 0, 0, 0}
        InitCustStock(150) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(160) = New Integer() {0, 86, 16, 11, 6, 0, 0, 0, 0}
        InitCustStock(170) = New Integer() {8, 48, 7, 5, 11, 0, 0, 0, 0}
        InitCustStock(180) = New Integer() {41, 73, 22, 13, 12, 0, 0, 0, 0}
        InitCustStock(190) = New Integer() {22, 113, 9, 12, 17, 0, 0, 0, 0}
        InitCustStock(200) = New Integer() {12, 55, 2, 1, 11, 0, 0, 0, 0}
        InitCustStock(210) = New Integer() {8, 62, 10, 8, 1, 0, 0, 0, 0}
        InitCustStock(220) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(221) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(226) = New Integer() {68, 79, 10, 4, 1, 0, 0, 0, 0}
        InitCustStock(230) = New Integer() {17, 6, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(250) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(260) = New Integer() {0, 17, 0, 0, 2, 0, 0, 0, 0}
        InitCustStock(271) = New Integer() {200, 6, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(280) = New Integer() {114, 21, 0, 1, 1, 0, 0, 0, 0}
        InitCustStock(310) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(320) = New Integer() {6, 84, 5, 12, 19, 0, 0, 0, 0}
        InitCustStock(330) = New Integer() {5, 14, 6, 0, 0, 0, 0, 0, 0}
        InitCustStock(340) = New Integer() {5, 36, 4, 1, 3, 0, 0, 0, 0}
        InitCustStock(360) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(370) = New Integer() {0, 13, 1, 0, 0, 0, 0, 0, 0}
        InitCustStock(381) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(382) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(383) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(385) = New Integer() {4, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(390) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(410) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(420) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(493) = New Integer() {79, 90, 5, 7, 17, 0, 0, 0, 0}
        InitCustStock(519) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}
        InitCustStock(999) = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0}


        Try
            Dim isIn = _view.GetOrderType = "進場單"
            Dim initProps = initCusStk.GetType.GetProperties
            Dim inputInOut = If(isIn, _view.GetInInput, _view.GetOutInput)
            Dim inputProps = inputInOut.GetType.GetProperties
            Dim currentOrderProps = currentOrder?.GetType.GetProperties
            Dim cusBarrelStock As New order
            Dim cusBarrelStockProps = cusBarrelStock.GetType.GetProperties

            'edit by kevin 20251103
            'Dim db As New gas_accounting_systemEntities
            'Dim endOfToday As DateTime = Date.Today + New TimeSpan(23, 59, 59)
            'Dim query = (From o In db.orders
            '             Where o.o_date < endOfToday AndAlso o.o_cus_Id = currentCustomer.cus_id
            '             Select o.o_in_50, o.o_in_20, o.o_in_16, o.o_in_10, o.o_in_4, o.o_in_18, o.o_in_14, o.o_in_5, o.o_in_2,
            '                    o.o_new_in_50, o.o_new_in_20, o.o_new_in_16, o.o_new_in_10, o.o_new_in_4, o.o_new_in_18, o.o_new_in_14, o.o_new_in_5, o.o_new_in_2,
            '                    o.o_inspect_50, o.o_inspect_20, o.o_inspect_16, o.o_inspect_10, o.o_inspect_4, o.o_inspect_18, o.o_inspect_14, o.o_inspect_5, o.o_inspect_2,
            '                    o.o_gas_c_50, o.o_gas_c_20, o.o_gas_c_16, o.o_gas_c_10, o.o_gas_c_4, o.o_gas_c_18, o.o_gas_c_14, o.o_gas_c_5, o.o_gas_c_2,
            '                    o.o_gas_50, o.o_gas_20, o.o_gas_16, o.o_gas_10, o.o_gas_4, o.o_gas_18, o.o_gas_14, o.o_gas_5, o.o_gas_2,
            '                    o.o_empty_50, o.o_empty_20, o.o_empty_16, o.o_empty_10, o.o_empty_4, o.o_empty_18, o.o_empty_14, o.o_empty_5, o.o_empty_2).ToList
            Dim query = _ordRep.GetCustomerStock(currentCustomer.cus_id)
            'Dim sql As String = query.ToString()
            Dim counter50to2 = 0

            For Each prop In initProps
                Dim barrelType = prop.Name.Substring(7)
                Dim currentStk = prop.GetValue(initCusStk)
                Dim barrelStk As Integer
                Dim targetProp = cusBarrelStockProps.FirstOrDefault(Function(x) x.Name = $"o_cus_{barrelType}")

                'edit by kevin 20251109 檢查庫存有異動就跳訊息
                If InitCustStock(CInt(currentCustomer.cus_code))(counter50to2) <> currentStk Then
                    '    MsgBox("庫存有變動了!!" + barrelType)
                End If
                counter50to2 = counter50to2 + 1

                '訂單收空瓶
                'Dim orderInQty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_in_{barrelType}").GetValue(currentOrder)
                Dim orderInQty As Integer = 0

                '訂單新瓶
                'Dim orderNewInQty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_new_in_{barrelType}").GetValue(currentOrder)
                Dim orderNewInQty As Integer = 0

                '訂單檢驗瓶
                'Dim orderInspectInQty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_inspect_{barrelType}").GetValue(currentOrder)
                Dim orderInspectInQty As Integer = 0

                'Dim orderGasC As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_gas_c_{barrelType}").GetValue(currentOrder)
                Dim orderGasC As Integer = 0

                'Dim orderGas As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_gas_{barrelType}").GetValue(currentOrder)
                Dim orderGas As Integer = 0

                'Dim orderEmpty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_empty_{barrelType}").GetValue(currentOrder)
                Dim orderEmpty As Integer = 0

                If currentCustomer IsNot Nothing And query.Any() Then
                    Select Case $"o_in_{barrelType}"
                        Case "o_in_50"
                            orderInQty = query.Sum(Function(x) x.o_in_50)
                        Case "o_in_20"
                            orderInQty = query.Sum(Function(x) x.o_in_20)
                        Case "o_in_16"
                            orderInQty = query.Sum(Function(x) x.o_in_16)
                        Case "o_in_10"
                            orderInQty = query.Sum(Function(x) x.o_in_10)
                        Case "o_in_4"
                            orderInQty = query.Sum(Function(x) x.o_in_4)
                        Case "o_in_18"
                            orderInQty = query.Sum(Function(x) x.o_in_18)
                        Case "o_in_14"
                            orderInQty = query.Sum(Function(x) x.o_in_14)
                        Case "o_in_5"
                            orderInQty = query.Sum(Function(x) x.o_in_5)
                        Case "o_in_2"
                            orderInQty = query.Sum(Function(x) x.o_in_2)
                    End Select
                    Select Case $"o_new_in_{barrelType}"
                        Case "o_new_in_50"
                            orderNewInQty = query.Sum(Function(x) x.o_new_in_50)
                        Case "o_new_in_20"
                            orderNewInQty = query.Sum(Function(x) x.o_new_in_20)
                        Case "o_new_in_16"
                            orderNewInQty = query.Sum(Function(x) x.o_new_in_16)
                        Case "o_new_in_10"
                            orderNewInQty = query.Sum(Function(x) x.o_new_in_10)
                        Case "o_new_in_4"
                            orderNewInQty = query.Sum(Function(x) x.o_new_in_4)
                        Case "o_new_in_18"
                            orderNewInQty = query.Sum(Function(x) x.o_new_in_18)
                        Case "o_new_in_14"
                            orderNewInQty = query.Sum(Function(x) x.o_new_in_14)
                        Case "o_new_in_5"
                            orderNewInQty = query.Sum(Function(x) x.o_new_in_5)
                        Case "o_new_in_2"
                            orderNewInQty = query.Sum(Function(x) x.o_new_in_2)
                    End Select
                    Select Case $"o_inspect_{barrelType}"
                        Case "o_inspect_50"
                            orderInspectInQty = query.Sum(Function(x) x.o_inspect_50)
                        Case "o_inspect_20"
                            orderInspectInQty = query.Sum(Function(x) x.o_inspect_20)
                        Case "o_inspect_16"
                            orderInspectInQty = query.Sum(Function(x) x.o_inspect_16)
                        Case "o_inspect_10"
                            orderInspectInQty = query.Sum(Function(x) x.o_inspect_10)
                        Case "o_inspect_4"
                            orderInspectInQty = query.Sum(Function(x) x.o_inspect_4)
                        Case "o_inspect_18"
                            orderInspectInQty = query.Sum(Function(x) x.o_inspect_18)
                        Case "o_inspect_14"
                            orderInspectInQty = query.Sum(Function(x) x.o_inspect_14)
                        Case "o_inspect_5"
                            orderInspectInQty = query.Sum(Function(x) x.o_inspect_5)
                        Case "o_inspect_2"
                            orderInspectInQty = query.Sum(Function(x) x.o_inspect_2)
                    End Select
                    Select Case $"o_gas_c_{barrelType}"
                        Case "o_gas_c_50"
                            orderGasC = query.Sum(Function(x) x.o_gas_c_50)
                        Case "o_gas_c_20"
                            orderGasC = query.Sum(Function(x) x.o_gas_c_20)
                        Case "o_gas_c_16"
                            orderGasC = query.Sum(Function(x) x.o_gas_c_16)
                        Case "o_gas_c_10"
                            orderGasC = query.Sum(Function(x) x.o_gas_c_10)
                        Case "o_gas_c_4"
                            orderGasC = query.Sum(Function(x) x.o_gas_c_4)
                        Case "o_gas_c_18"
                            orderGasC = query.Sum(Function(x) x.o_gas_c_18)
                        Case "o_gas_c_14"
                            orderGasC = query.Sum(Function(x) x.o_gas_c_14)
                        Case "o_gas_c_5"
                            orderGasC = query.Sum(Function(x) x.o_gas_c_5)
                        Case "o_gas_c_2"
                            orderGasC = query.Sum(Function(x) x.o_gas_c_2)
                    End Select
                    Select Case $"o_gas_{barrelType}"
                        Case "o_gas_50"
                            orderGas = query.Sum(Function(x) x.o_gas_50)
                        Case "o_gas_20"
                            orderGas = query.Sum(Function(x) x.o_gas_20)
                        Case "o_gas_16"
                            orderGas = query.Sum(Function(x) x.o_gas_16)
                        Case "o_gas_10"
                            orderGas = query.Sum(Function(x) x.o_gas_10)
                        Case "o_gas_4"
                            orderGas = query.Sum(Function(x) x.o_gas_4)
                        Case "o_gas_18"
                            orderGas = query.Sum(Function(x) x.o_gas_18)
                        Case "o_gas_14"
                            orderGas = query.Sum(Function(x) x.o_gas_14)
                        Case "o_gas_5"
                            orderGas = query.Sum(Function(x) x.o_gas_5)
                        Case "o_gas_2"
                            orderGas = query.Sum(Function(x) x.o_gas_2)
                    End Select
                    Select Case $"o_empty_{barrelType}"
                        Case "o_empty_50"
                            orderEmpty = query.Sum(Function(x) x.o_empty_50)
                        Case "o_empty_20"
                            orderEmpty = query.Sum(Function(x) x.o_empty_20)
                        Case "o_empty_16"
                            orderEmpty = query.Sum(Function(x) x.o_empty_16)
                        Case "o_empty_10"
                            orderEmpty = query.Sum(Function(x) x.o_empty_10)
                        Case "o_empty_4"
                            orderEmpty = query.Sum(Function(x) x.o_empty_4)
                        Case "o_empty_18"
                            orderEmpty = query.Sum(Function(x) x.o_empty_18)
                        Case "o_empty_14"
                            orderEmpty = query.Sum(Function(x) x.o_empty_14)
                        Case "o_empty_5"
                            orderEmpty = query.Sum(Function(x) x.o_empty_5)
                        Case "o_empty_2"
                            orderEmpty = query.Sum(Function(x) x.o_empty_2)
                    End Select
                End If


                If isIn Then
                    ' 計算進場單的結存：初始 + 輸入數量 - 舊訂單數量
                    '收空瓶
                    Dim inQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_in_{barrelType}").GetValue(inputInOut)
                    '新瓶
                    Dim newInQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_new_in_{barrelType}").GetValue(inputInOut)
                    '檢驗瓶
                    Dim inspectInQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_inspect_{barrelType}").GetValue(inputInOut)

                    'edit by kevin 20251104 
                    'barrelStk = currentStk + inQty - orderInQty + newInQty - orderNewInQty + inspectInQty - orderInspectInQty
                    barrelStk = currentStk + inQty + newInQty + inspectInQty + orderInQty + orderNewInQty + orderInspectInQty - orderGasC - orderGas - orderEmpty
                Else
                    ' 計算出場單的結存：初始 - 輸入數量 + 舊訂單數量
                    Dim gasC As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_gas_c_{barrelType}").GetValue(inputInOut)
                    Dim gas As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_gas_{barrelType}").GetValue(inputInOut)
                    Dim empty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_empty_{barrelType}").GetValue(inputInOut)

                    'edit by kevin 20251104
                    'barrelStk = currentStk - gasC + orderGasC - gas + orderGas - empty + orderEmpty
                    barrelStk = currentStk - gasC - gas - empty + orderInQty + orderNewInQty + orderInspectInQty - orderGasC - orderGas - orderEmpty
                End If

                targetProp.SetValue(cusBarrelStock, barrelStk)
            Next

            _view.ShowCusBarrelStock(cusBarrelStock)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' 計算瓦斯桶價格
    ''' </summary>
    Private Sub CalculateBarrelAmount()
        Try
            If currentCustomer Is Nothing Then Exit Sub

            Dim cusProps = currentCustomer?.GetType.GetProperties
            Dim userInput = _view.GetInInput
            Dim inputProps = userInput.GetType.GetProperties
            Dim barrelAmount As Integer ' 瓦斯桶金額

            For Each prop In cusProps.Where(Function(x) x.Name.StartsWith("cus_gas_"))
                Dim barrelType = prop.Name.Substring(8)
                Dim barrelUnitPrice = inputProps.FirstOrDefault(Function(x) x.Name = $"o_barrel_unit_price_{barrelType}").GetValue(userInput)
                Dim newInQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_new_in_{barrelType}").GetValue(userInput)
                barrelAmount += barrelUnitPrice * newInQty
            Next

            _view.ShowBarrelPrice(barrelAmount)
            CalculateTotalAmount()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 計算寄桶
    ''' </summary>
    Private Sub CalculateDeposit()
        Dim isIn = _view.GetOrderType = "進場單"

        If currentCar Is Nothing Then Exit Sub

        Try
            Dim carProps = currentCar?.GetType.GetProperties
            Dim input = If(isIn, _view.GetInInput, _view.GetOutInput)
            Dim inputProps = input.GetType.GetProperties
            Dim currentOrderProps = currentOrder?.GetType.GetProperties
            Dim result As New car
            Dim resultVaule As Integer

            For Each prop In carProps.Where(Function(x) x.Name.StartsWith("c_deposit_"))
                Dim barrelType = prop.Name.Substring(10)

                Dim columnName = If(isIn, "o_deposit_in_", "o_deposit_out_") & barrelType
                Dim qty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = columnName).GetValue(input)
                Dim currentStock = prop.GetValue(currentCar)
                Dim targetProp = carProps.FirstOrDefault(Function(x) x.Name = $"c_deposit_{barrelType}")
                Dim orderQty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = columnName).GetValue(currentOrder)

                If isIn Then
                    resultVaule = currentStock + qty - orderQty
                Else
                    resultVaule = currentStock - qty + orderQty
                End If

                targetProp.SetValue(result, resultVaule)
            Next

            If isIn Then
                _view.ShowCarBarrelStock_In(result)
            Else
                _view.ShowCarBarrelStock_Out(result)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 計算總氣
    ''' </summary>
    Private Sub CalculateGasAmount()
        Dim input = _view.GetOutInput
        Dim orderInput = _view.GetOrderInput
        Dim inputProps = input.GetType.GetProperties
        Dim resultGas As Integer = 0
        Dim resultGasC As Integer = 0

        ' 計算普氣
        For Each prop In inputProps.Where(Function(x) Not x.Name.StartsWith("o_gas_c_") AndAlso x.Name.StartsWith("o_gas_"))
            Dim propPart = prop.Name.Substring(6)
            Dim barrelType As Integer = 0
            If Not Integer.TryParse(propPart, barrelType) Then Continue For

            Dim qty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_gas_{barrelType}").GetValue(input)
            resultGas += barrelType * qty
        Next

        resultGas -= orderInput.o_return

        ' 計算丙氣
        For Each prop In inputProps.Where(Function(x) x.Name.StartsWith("o_gas_c_"))
            Dim propPart = prop.Name.Substring(8)
            Dim barrelType As Integer = 0

            If Not Integer.TryParse(propPart, barrelType) Then Continue For

            Dim qty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_gas_c_{barrelType}").GetValue(input)
            resultGasC += barrelType * qty
        Next

        resultGasC -= orderInput.o_return_c

        _view.ShowGasAmount(New Tuple(Of Integer, Integer)(resultGas, resultGasC))
        CalculateInsurance()
        CalculateTotalAmount()
    End Sub

    ''' <summary>
    ''' 計算保險金額
    ''' </summary>
    Private Sub CalculateInsurance()
        Try
            Dim input = _view.GetOrderInput
            Dim result As Double = 0

            If currentCustomer.cus_IsInsurance Then
                result = ((input.o_gas_total + input.o_gas_c_total) + (input.o_return + input.o_return_c)) * input.o_insurance_unit_price
            End If

            _view.ShowInsurance(Math.Round(result, 2))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 計算總金額
    ''' </summary>
    Private Sub CalculateTotalAmount()
        Dim input = _view.GetOrderInput
        Dim gasAmount As Double = input.o_gas_total * input.o_UnitPrice
        Dim gasCAmount As Double = input.o_gas_c_total * input.o_UnitPriceC

        _view.ShowTotalAmount(input.o_BarrelPrice + gasAmount + gasCAmount + input.o_Insurance - input.o_sales_allowance)
    End Sub

#End Region

#Region "CRUD 操作"

    Private Sub Add()
        Using transaction = _ordRep.BeginTransaction
            Dim sw As New Stopwatch
            sw.Start()

            Try
                RefreshCurrentEntities()

                ' 記錄交易前的客戶庫存
                Dim cusCode = currentCustomer?.cus_code
                Dim orderType = _view.GetOrderType

                Dim orderInput As New order
                _view.GetInput(orderInput)

                If _currentUserService.UserRole <> 1 AndAlso orderInput.o_date < Today.AddDays(-3) Then Throw New Exception("非管理者不能新增三天前的訂單")

                ' === 檢查庫存是否被其他使用者改變 ===
                If Not IsCustomerStockMatchInit() Then
                    ' 重新載入客戶資料
                    RefreshCurrentEntities()

                    ' 更新 initCusStk 為新的當前庫存
                    With initCusStk
                        .Barrel_50 = currentCustomer.cus_gas_50
                        .Barrel_20 = currentCustomer.cus_gas_20
                        .Barrel_16 = currentCustomer.cus_gas_16
                        .Barrel_10 = currentCustomer.cus_gas_10
                        .Barrel_4 = currentCustomer.cus_gas_4
                        .Barrel_18 = currentCustomer.cus_gas_18
                        .Barrel_14 = currentCustomer.cus_gas_14
                        .Barrel_5 = currentCustomer.cus_gas_5
                        .Barrel_2 = currentCustomer.cus_gas_2
                    End With

                    ' 重新計算並顯示庫存
                    CaculateCusBarrelStock()

                    ' 提示使用者並中止此次新增
                    Throw New Exception("庫存已被其他使用者更新，請重新確認數量後再儲存")
                End If

                ' === 計算並更新客戶庫存（用差值法）===
                UpdateCustomerStockByDiff(orderInput)

                ' 更新車輛寄桶庫存
                If currentCar IsNot Nothing Then _view.GetCarStkInput(currentCar)

                Dim order = _ordRep.AddAsync(orderInput).Result
                Dim gbRep As New GasBarrelRep(CType(_ordRep.Context, gas_accounting_systemEntities))
                _barrelInvSer.ApplyOrderIssueAsync(gbRep, orderInput).Wait()

                _service.UpdateOrAddAsync(orderInput.o_date)

                Dim entries = New List(Of accounting_entry) From {
                    New accounting_entry With {
                        .ae_TransactionId = orderInput.o_id,
                        .ae_Date = Now,
                        .ae_TransactionType = "銷售管理",
                        .ae_s_Id = 8,
                        .ae_Debit = 0,
                        .ae_Credit = orderInput.o_total_amount
                    },
                    New accounting_entry With {
                        .ae_TransactionId = orderInput.o_id,
                        .ae_Date = Now,
                        .ae_TransactionType = "銷售管理",
                        .ae_s_Id = 9,
                        .ae_Debit = orderInput.o_total_amount,
                        .ae_Credit = 0
                    }
                }

                _aeSer.AddEntries(entries)

                ' 同步更新月度帳單資料
                _maService.SyncOrderToMonthlyAccount(order.o_id, True, False)

                _cusRep.SaveChangesAsync()

                transaction.Commit()

                Initialize()
                MessageBox.Show("新增成功")
                _logger.LogInfo($"新增 {orderInput.o_id}")
                LoadDetail(Nothing, order.o_id)
            Catch ex As Exception
                transaction.Rollback()

                Dim innerEx = ex
                While innerEx.InnerException IsNot Nothing
                    innerEx = innerEx.InnerException
                End While

                MessageBox.Show(innerEx.Message)
            Finally
                sw.Stop()
                _logger.LogInfo($"Add 耗時: {sw.ElapsedMilliseconds / 1000} 秒")
            End Try
        End Using
    End Sub

    Private Sub LoadDetail(sender As Object, id As Integer)
        Dim sw As New Stopwatch
        sw.Start()

        Try
            currentOrder = _ordRep.GetByIdAsync(id).Result

            If currentOrder Is Nothing Then Throw New Exception("資料已被刪除，請刷新")

            _view.ClearInput()

            currentCustomer = currentOrder.customer

            Dim isIn = currentOrder.o_in_out = "進場單"
            currentCar = If(isIn, currentOrder.car, currentOrder.car1)

            RefreshCurrentEntities()

            ' === 載入訂單快照到 initCusStk（用於 Update/Delete 計算差值）===
            With initCusStk
                .Barrel_50 = currentOrder.o_cus_50
                .Barrel_20 = currentOrder.o_cus_20
                .Barrel_16 = currentOrder.o_cus_16
                .Barrel_10 = currentOrder.o_cus_10
                .Barrel_4 = currentOrder.o_cus_4
                .Barrel_18 = currentOrder.o_cus_18
                .Barrel_14 = currentOrder.o_cus_14
                .Barrel_5 = currentOrder.o_cus_5
                .Barrel_2 = currentOrder.o_cus_2
            End With

            _view.ShowCustomer(currentCustomer)
            _view.ShowDetail(currentOrder)
            _view.ButtonStatus(True)
            _view.SetReturnGasReadOnly(isIn)

            If currentCar IsNot Nothing Then LoadCarBarrelStock(currentCar.c_id)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            sw.Stop()
            _logger.LogInfo($"LoadDetail 耗時: {sw.ElapsedMilliseconds / 1000} 秒")
        End Try
    End Sub

    Private Sub Update()
        Using transaction = _ordRep.BeginTransaction
            Try
                RefreshCurrentEntities(includeOrder:=True)

                ' 記錄修改前的資訊
                Dim cusCode = currentCustomer?.cus_code
                Dim orderId = currentOrder?.o_id
                Dim orderType = currentOrder.o_in_out
                Dim beforeStock = FormatBarrelStock(currentCustomer)
                Dim beforeOrderQty = FormatOrderBarrelQty(currentOrder, orderType = "進場單")

                _logger.LogInfo($"[Update] 修改開始 | 訂單ID:{orderId} | 客戶[{cusCode}] | 修改前結存:{beforeStock} | 訂單快照:{FormatInitStock()}")

                ' === 取得新的訂單資料 ===
                Dim originOrderSnapshot As New order With {
                    .o_new_in_50 = currentOrder.o_new_in_50,
                    .o_new_in_20 = currentOrder.o_new_in_20,
                    .o_new_in_16 = currentOrder.o_new_in_16,
                    .o_new_in_10 = currentOrder.o_new_in_10,
                    .o_new_in_4 = currentOrder.o_new_in_4,
                    .o_new_in_18 = currentOrder.o_new_in_18,
                    .o_new_in_14 = currentOrder.o_new_in_14,
                    .o_new_in_5 = currentOrder.o_new_in_5,
                    .o_new_in_2 = currentOrder.o_new_in_2
                }

                _view.GetInput(currentOrder)

                ' 記錄修改後的訂單數量
                Dim afterOrderQty = FormatOrderBarrelQty(currentOrder, orderType = "進場單")
                _logger.LogInfo($"[Update] 修改後訂單 | 訂單ID:{orderId} | 客戶[{cusCode}] | {afterOrderQty}")

                ' === 用差值法更新客戶庫存（比較舊快照 initCusStk 與新計算的 o_cus_XX）===
                UpdateCustomerStockByDiff(currentOrder)

                ' 記錄修改後的客戶庫存
                Dim afterStock = FormatBarrelStock(currentCustomer)
                _logger.LogInfo($"[Update] 修改後結存 | 訂單ID:{orderId} | 客戶[{cusCode}] | {afterStock}")

                ' 更新車輛寄桶庫存
                If currentCar IsNot Nothing Then _view.GetCarStkInput(currentCar)

                _service.UpdateOrAddAsync(currentOrder.o_date)

                Dim gbRep As New GasBarrelRep(CType(_ordRep.Context, gas_accounting_systemEntities))
                _barrelInvSer.ApplyOrderIssueUpdateAsync(gbRep, originOrderSnapshot, currentOrder).Wait()

                ' 同步更新月度帳單資料
                _maService.SyncOrderToMonthlyAccount(currentOrder.o_id, False, False)

                _cusRep.SaveChangesAsync()
                transaction.Commit()

                _logger.LogInfo($"[Update] 修改成功 | 訂單ID:{orderId} | 客戶[{cusCode}]")

                Initialize()
                MessageBox.Show("修改成功")
            Catch ex As Exception
                transaction.Rollback()
                MessageBox.Show(ex.Message)
            End Try
        End Using
    End Sub

    Private Sub Delete()
        Using transaction = _ordRep.BeginTransaction
            Try
                RefreshCurrentEntities(includeOrder:=True)
                If currentOrder Is Nothing Then
                    Throw New Exception("沒有選擇要刪除的訂單")
                End If

                If MessageBox.Show("確定要刪除這筆訂單嗎？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return

                ' 記錄刪除前的資訊
                Dim cusCode = currentCustomer?.cus_code
                Dim orderId = currentOrder?.o_id
                Dim orderType = currentOrder.o_in_out
                Dim beforeStock = FormatBarrelStock(currentCustomer)
                Dim orderQty = FormatOrderBarrelQty(currentOrder, orderType = "進場單")

                _logger.LogInfo($"[Delete] 刪除開始 | 訂單ID:{orderId} | 客戶[{cusCode}] | 刪除前結存:{beforeStock} | 訂單快照:{FormatInitStock()}")

                ' === 回退客戶庫存（分析訂單數量並反向操作）===
                If currentCustomer Is Nothing Then Throw New Exception("未取得客戶資料")
                '20251111 edit by kevin 不用補數量回去
                'RevertCustomerStockByDiff(currentOrder)

                ' 記錄刪除後的客戶庫存
                Dim afterStock = FormatBarrelStock(currentCustomer)
                _logger.LogInfo($"[Delete] 回退結存 | 訂單ID:{orderId} | 客戶[{cusCode}] | 回退後:{afterStock}")

                ' === 回退車輛寄桶庫存 ===
                If currentOrder.o_delivery_type = "自運" AndAlso currentCar IsNot Nothing Then
                    Dim isIn = orderType = "進場單"
                    Dim carProps = currentCar.GetType.GetProperties

                    For Each barrelType In BARREL_TYPES
                        Dim columnName = If(isIn, "o_deposit_in_", "o_deposit_out_") & barrelType
                        Dim qty = CInt(currentOrder.GetType().GetProperty(columnName).GetValue(currentOrder))
                        Dim carProperty = carProps.FirstOrDefault(Function(x) x.Name = $"c_deposit_{barrelType}")
                        Dim currentStock = CInt(carProperty.GetValue(currentCar))

                        If isIn Then
                            carProperty.SetValue(currentCar, currentStock - qty)
                        Else
                            carProperty.SetValue(currentCar, currentStock + qty)
                        End If
                    Next
                End If

                ' 同步更新月度帳單資料
                _maService.SyncOrderToMonthlyAccount(currentOrder.o_id, False, True)

                ' 銷帳
                _ocmSer.DeleteOrder(currentOrder.o_id)

                ' 刪除訂單
                _ordRep.DeleteAsync(currentOrder)

                Dim gbRep As New GasBarrelRep(CType(_ordRep.Context, gas_accounting_systemEntities))
                _barrelInvSer.ApplyOrderIssueDeleteAsync(gbRep, currentOrder).Wait()

                _service.UpdateOrAddAsync(currentOrder.o_date)

                _aeSer.DeleteEntries("銷售管理", currentOrder.o_id)

                ' 保存更改
                _ordRep.SaveChangesAsync()

                transaction.Commit()

                _logger.LogInfo($"[Delete] 刪除成功 | 訂單ID:{orderId} | 客戶[{cusCode}]")

                Initialize()
                MessageBox.Show("刪除成功")
            Catch ex As Exception
                transaction.Rollback()
                MessageBox.Show("刪除失敗：" & ex.Message)
            End Try
        End Using
    End Sub

#End Region

#Region "列印與報表"

    Private Sub Print(sender As Object, orderId As Integer)
        Dim templatePath As String = ""
        Dim pdfPath As String = ""

        Dim sw As New Stopwatch
        sw.Start()

        Try
            Dim data = _ordRep.GetOrderVoucherData(orderId)
            templatePath = Path.Combine(Application.StartupPath, "Report", "客戶提氣量憑單.html")
            Dim htmlContent = FillTemplate(templatePath, data)
            pdfPath = Path.Combine(Application.StartupPath, "Report", "客戶提氣量憑單.pdf")

            ' 轉換 HTML 為 PDF
            Using pdfWriter As New PdfWriter(pdfPath)
                Using pdfDocument As New PdfDocument(pdfWriter)
                    Dim fontProvider As New DefaultFontProvider()
                    fontProvider.AddFont("c:/windows/Fonts/kaiu.ttf")
                    Dim converterProperties As New ConverterProperties()
                    converterProperties.SetFontProvider(fontProvider)

                    ' 將毫米轉換為點
                    Dim widthInPoints As Single = 216 * 2.834645
                    Dim heightInPoints As Single = 139 * 2.834645

                    ' 設置頁面大小
                    pdfDocument.SetDefaultPageSize(New PageSize(widthInPoints, heightInPoints))

                    HtmlConverter.ConvertToPdf(htmlContent, pdfDocument, converterProperties)
                End Using
            End Using

            PrintPDF(pdfPath)

            MessageBox.Show("成功")
            Initialize()
        Catch ex As Exception
            ' 詳細的錯誤診斷資訊
            Dim errorDetails = $"PDF轉換失敗 - 環境診斷資訊：{vbCrLf}{vbCrLf}" &
                             $"錯誤類型：{ex.GetType().Name}{vbCrLf}" &
                             $"錯誤訊息：{ex.Message}{vbCrLf}{vbCrLf}" &
                             $"字體檔案檢查：{vbCrLf}" &
                             $"- kaiu.ttf 存在: {File.Exists("c:/windows/Fonts/kaiu.ttf")}{vbCrLf}" &
                             $"- 模板檔案路徑: {templatePath}{vbCrLf}" &
                             $"- 模板檔案存在: {File.Exists(templatePath)}{vbCrLf}" &
                             $"- PDF輸出路徑: {pdfPath}{vbCrLf}" &
                             $"- 輸出目錄存在: {Directory.Exists(Path.GetDirectoryName(pdfPath))}{vbCrLf}{vbCrLf}" &
                             $"堆疊追蹤：{vbCrLf}{ex.StackTrace}"

            MessageBox.Show(errorDetails, "PDF轉換錯誤診斷", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            sw.Stop()
            _logger.LogInfo($"Print 耗時: {sw.ElapsedMilliseconds / 1000} 秒")
        End Try
    End Sub

    Private Async Sub PrintCusStk(sender As Object, isYesterday As Boolean)
        Try
            '取得資料
            Dim customers = Await _cusRep.GetAllAsync()

            If isYesterday Then GetYesterdayStk(customers)

            customers = customers.OrderBy(Function(x) x.cus_code).ToList

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "客戶鋼瓶結存總冊範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim printDate = If(isYesterday, Now.AddDays(-1), Now)
                    .WriteToCell(3, 10, $"印表日期:{printDate:yyyy/MM/dd}")

                    Dim rowIndex = 5
                    Dim totalSum As Integer
                    Dim db As New gas_accounting_systemEntities
                    Dim endOfToday As DateTime = Date.Today + New TimeSpan(23, 59, 59)

                    For Each cus In customers
                        Dim query = From o In db.orders Where o.o_date < endOfToday AndAlso o.o_cus_Id = cus.cus_id Select o
                        '訂單收空瓶
                        Dim orderInQty50 As Integer = 0
                        Dim orderInQty20 As Integer = 0
                        Dim orderInQty16 As Integer = 0
                        Dim orderInQty10 As Integer = 0
                        Dim orderInQty4 As Integer = 0
                        Dim orderInQty18 As Integer = 0
                        Dim orderInQty14 As Integer = 0
                        Dim orderInQty5 As Integer = 0
                        Dim orderInQty2 As Integer = 0
                        '訂單新瓶
                        Dim orderNewInQty50 As Integer = 0
                        Dim orderNewInQty20 As Integer = 0
                        Dim orderNewInQty16 As Integer = 0
                        Dim orderNewInQty10 As Integer = 0
                        Dim orderNewInQty4 As Integer = 0
                        Dim orderNewInQty18 As Integer = 0
                        Dim orderNewInQty14 As Integer = 0
                        Dim orderNewInQty5 As Integer = 0
                        Dim orderNewInQty2 As Integer = 0
                        '訂單檢驗瓶
                        Dim orderInspectInQty50 As Integer = 0
                        Dim orderInspectInQty20 As Integer = 0
                        Dim orderInspectInQty16 As Integer = 0
                        Dim orderInspectInQty10 As Integer = 0
                        Dim orderInspectInQty4 As Integer = 0
                        Dim orderInspectInQty18 As Integer = 0
                        Dim orderInspectInQty14 As Integer = 0
                        Dim orderInspectInQty5 As Integer = 0
                        Dim orderInspectInQty2 As Integer = 0

                        Dim orderGasC50 As Integer = 0
                        Dim orderGasC20 As Integer = 0
                        Dim orderGasC16 As Integer = 0
                        Dim orderGasC10 As Integer = 0
                        Dim orderGasC4 As Integer = 0
                        Dim orderGasC18 As Integer = 0
                        Dim orderGasC14 As Integer = 0
                        Dim orderGasC5 As Integer = 0
                        Dim orderGasC2 As Integer = 0

                        Dim orderGas50 As Integer = 0
                        Dim orderGas20 As Integer = 0
                        Dim orderGas16 As Integer = 0
                        Dim orderGas10 As Integer = 0
                        Dim orderGas4 As Integer = 0
                        Dim orderGas18 As Integer = 0
                        Dim orderGas14 As Integer = 0
                        Dim orderGas5 As Integer = 0
                        Dim orderGas2 As Integer = 0

                        Dim orderEmpty50 As Integer = 0
                        Dim orderEmpty20 As Integer = 0
                        Dim orderEmpty16 As Integer = 0
                        Dim orderEmpty10 As Integer = 0
                        Dim orderEmpty4 As Integer = 0
                        Dim orderEmpty18 As Integer = 0
                        Dim orderEmpty14 As Integer = 0
                        Dim orderEmpty5 As Integer = 0
                        Dim orderEmpty2 As Integer = 0

                        Try
                            orderInQty50 = query.Sum(Function(x) x.o_in_50)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInQty20 = query.Sum(Function(x) x.o_in_20)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInQty16 = query.Sum(Function(x) x.o_in_16)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInQty10 = query.Sum(Function(x) x.o_in_10)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInQty4 = query.Sum(Function(x) x.o_in_4)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInQty18 = query.Sum(Function(x) x.o_in_18)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInQty14 = query.Sum(Function(x) x.o_in_14)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInQty5 = query.Sum(Function(x) x.o_in_5)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInQty2 = query.Sum(Function(x) x.o_in_2)
                        Catch ex As Exception
                        End Try
                        Try
                            orderNewInQty50 = query.Sum(Function(x) x.o_new_in_50)
                        Catch ex As Exception
                        End Try
                        Try
                            orderNewInQty20 = query.Sum(Function(x) x.o_new_in_20)
                        Catch ex As Exception
                        End Try
                        Try
                            orderNewInQty16 = query.Sum(Function(x) x.o_new_in_16)
                        Catch ex As Exception
                        End Try
                        Try
                            orderNewInQty10 = query.Sum(Function(x) x.o_new_in_10)
                        Catch ex As Exception
                        End Try
                        Try
                            orderNewInQty4 = query.Sum(Function(x) x.o_new_in_4)
                        Catch ex As Exception
                        End Try
                        Try
                            orderNewInQty18 = query.Sum(Function(x) x.o_new_in_18)
                        Catch ex As Exception
                        End Try
                        Try
                            orderNewInQty14 = query.Sum(Function(x) x.o_new_in_14)
                        Catch ex As Exception
                        End Try
                        Try
                            orderNewInQty5 = query.Sum(Function(x) x.o_new_in_5)
                        Catch ex As Exception
                        End Try
                        Try
                            orderNewInQty2 = query.Sum(Function(x) x.o_new_in_2)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInspectInQty50 = query.Sum(Function(x) x.o_inspect_50)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInspectInQty20 = query.Sum(Function(x) x.o_inspect_20)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInspectInQty16 = query.Sum(Function(x) x.o_inspect_16)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInspectInQty10 = query.Sum(Function(x) x.o_inspect_10)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInspectInQty4 = query.Sum(Function(x) x.o_inspect_4)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInspectInQty18 = query.Sum(Function(x) x.o_inspect_18)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInspectInQty14 = query.Sum(Function(x) x.o_inspect_14)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInspectInQty5 = query.Sum(Function(x) x.o_inspect_5)
                        Catch ex As Exception
                        End Try
                        Try
                            orderInspectInQty2 = query.Sum(Function(x) x.o_inspect_2)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGasC50 = query.Sum(Function(x) x.o_gas_c_50)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGasC20 = query.Sum(Function(x) x.o_gas_c_20)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGasC16 = query.Sum(Function(x) x.o_gas_c_16)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGasC10 = query.Sum(Function(x) x.o_gas_c_10)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGasC4 = query.Sum(Function(x) x.o_gas_c_4)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGasC18 = query.Sum(Function(x) x.o_gas_c_18)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGasC14 = query.Sum(Function(x) x.o_gas_c_14)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGasC5 = query.Sum(Function(x) x.o_gas_c_5)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGasC2 = query.Sum(Function(x) x.o_gas_c_2)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGas50 = query.Sum(Function(x) x.o_gas_50)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGas20 = query.Sum(Function(x) x.o_gas_20)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGas16 = query.Sum(Function(x) x.o_gas_16)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGas10 = query.Sum(Function(x) x.o_gas_10)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGas4 = query.Sum(Function(x) x.o_gas_4)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGas18 = query.Sum(Function(x) x.o_gas_18)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGas14 = query.Sum(Function(x) x.o_gas_14)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGas5 = query.Sum(Function(x) x.o_gas_5)
                        Catch ex As Exception
                        End Try
                        Try
                            orderGas2 = query.Sum(Function(x) x.o_gas_2)
                        Catch ex As Exception
                        End Try
                        Try
                            orderEmpty50 = query.Sum(Function(x) x.o_empty_50)
                        Catch ex As Exception
                        End Try
                        Try
                            orderEmpty20 = query.Sum(Function(x) x.o_empty_20)
                        Catch ex As Exception
                        End Try
                        Try
                            orderEmpty16 = query.Sum(Function(x) x.o_empty_16)
                        Catch ex As Exception
                        End Try
                        Try
                            orderEmpty10 = query.Sum(Function(x) x.o_empty_10)
                        Catch ex As Exception
                        End Try
                        Try
                            orderEmpty4 = query.Sum(Function(x) x.o_empty_4)
                        Catch ex As Exception
                        End Try
                        Try
                            orderEmpty18 = query.Sum(Function(x) x.o_empty_18)
                        Catch ex As Exception
                        End Try
                        Try
                            orderEmpty14 = query.Sum(Function(x) x.o_empty_14)
                        Catch ex As Exception
                        End Try
                        Try
                            orderEmpty5 = query.Sum(Function(x) x.o_empty_5)
                        Catch ex As Exception
                        End Try
                        Try
                            orderEmpty2 = query.Sum(Function(x) x.o_empty_2)
                        Catch ex As Exception
                        End Try


                        cus.cus_gas_50 = cus.cus_gas_50 + orderInQty50 + orderNewInQty50 + orderInspectInQty50 - orderGasC50 - orderGas50 - orderEmpty50
                        cus.cus_gas_20 = cus.cus_gas_20 + orderInQty20 + orderNewInQty20 + orderInspectInQty20 - orderGasC20 - orderGas20 - orderEmpty20
                        cus.cus_gas_16 = cus.cus_gas_16 + orderInQty16 + orderNewInQty16 + orderInspectInQty16 - orderGasC16 - orderGas16 - orderEmpty16
                        cus.cus_gas_10 = cus.cus_gas_10 + orderInQty10 + orderNewInQty10 + orderInspectInQty10 - orderGasC10 - orderGas10 - orderEmpty10
                        cus.cus_gas_4 = cus.cus_gas_4 + orderInQty4 + orderNewInQty4 + orderInspectInQty4 - orderGasC4 - orderGas4 - orderEmpty4
                        cus.cus_gas_18 = cus.cus_gas_18 + orderInQty18 + orderNewInQty18 + orderInspectInQty18 - orderGasC18 - orderGas18 - orderEmpty18
                        cus.cus_gas_14 = cus.cus_gas_14 + orderInQty14 + orderNewInQty14 + orderInspectInQty14 - orderGasC14 - orderGas14 - orderEmpty14
                        cus.cus_gas_5 = cus.cus_gas_5 + orderInQty5 + orderNewInQty5 + orderInspectInQty5 - orderGasC5 - orderGas5 - orderEmpty5
                        cus.cus_gas_2 = cus.cus_gas_2 + orderInQty2 + orderNewInQty2 + orderInspectInQty2 - orderGasC2 - orderGas2 - orderEmpty2

                        Dim totalBarrel = cus.cus_gas_50 + cus.cus_gas_20 + cus.cus_gas_16 + cus.cus_gas_10 + cus.cus_gas_4 + cus.cus_gas_18 + cus.cus_gas_14 + cus.cus_gas_5 + cus.cus_gas_2
                        .WriteToCell(rowIndex, 1, cus.cus_code)
                        .WriteToCell(rowIndex, 2, cus.cus_name)
                        .WriteToCell(rowIndex, 3, cus.cus_gas_50.ToString)
                        .WriteToCell(rowIndex, 4, cus.cus_gas_20.ToString)
                        .WriteToCell(rowIndex, 5, cus.cus_gas_16.ToString)
                        .WriteToCell(rowIndex, 6, cus.cus_gas_10.ToString)
                        .WriteToCell(rowIndex, 7, cus.cus_gas_4.ToString)
                        .WriteToCell(rowIndex, 8, cus.cus_gas_18.ToString)
                        .WriteToCell(rowIndex, 9, cus.cus_gas_14.ToString)
                        .WriteToCell(rowIndex, 10, cus.cus_gas_5.ToString)
                        .WriteToCell(rowIndex, 11, cus.cus_gas_2.ToString)
                        .WriteToCell(rowIndex, 12, totalBarrel.ToString)
                        .InsertRow(rowIndex)
                        rowIndex += 1
                        totalSum += totalBarrel
                    Next

                    Dim totalStyle = New CloseXML_Excel.CellFormatOptions With {
                        .Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center
                    }

                    .SetCustomBorders(rowIndex, 1, rowIndex, 12, topStyle:=ClosedXML.Excel.XLBorderStyleValues.Thin)
                    .WriteToCell(rowIndex, 2, "合計", totalStyle)
                    .WriteToCell(rowIndex, 3, customers.Sum(Function(x) x.cus_gas_50).ToString)
                    .WriteToCell(rowIndex, 4, customers.Sum(Function(x) x.cus_gas_20).ToString)
                    .WriteToCell(rowIndex, 5, customers.Sum(Function(x) x.cus_gas_16).ToString)
                    .WriteToCell(rowIndex, 6, customers.Sum(Function(x) x.cus_gas_10).ToString)
                    .WriteToCell(rowIndex, 7, customers.Sum(Function(x) x.cus_gas_4).ToString)
                    .WriteToCell(rowIndex, 8, customers.Sum(Function(x) x.cus_gas_18).ToString)
                    .WriteToCell(rowIndex, 9, customers.Sum(Function(x) x.cus_gas_14).ToString)
                    .WriteToCell(rowIndex, 10, customers.Sum(Function(x) x.cus_gas_5).ToString)
                    .WriteToCell(rowIndex, 11, customers.Sum(Function(x) x.cus_gas_2).ToString)
                    .WriteToCell(rowIndex, 12, totalSum.ToString)

                    '存檔
                    Dim exportFilePath = Path.Combine(Application.StartupPath, "報表", "客戶鋼瓶結存總冊.xlsx")
                    .SaveAs(exportFilePath)

                    '取得印表機
                    Dim printerName = _printerSer.GetOrSelectPrinter
                    .Print(exportFilePath, printerName, 1, 4)

                End With
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub PrintPDF(filePath As String)
        Dim printerSettings As New PrinterSettings()
        printerSettings.DefaultPageSettings.Margins = New Margins(0, 0, 0, 0)
        printerSettings.DefaultPageSettings.PaperSize = New PaperSize("Custom", 850, 551)

        Using document As PdfiumViewer.PdfDocument = PdfiumViewer.PdfDocument.Load(filePath)
            Dim printDocument As PrintDocument = document.CreatePrintDocument()
            printDocument.PrinterSettings = printerSettings
            printDocument.PrintController = New StandardPrintController()
            printDocument.Print()
        End Using
    End Sub

    ''' <summary>
    ''' 產生氣量氣款收付明細表
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="tu"></param>
    Private Sub GenerateCustomersGasDetailByDay(sender As Object, tu As Tuple(Of Date, Boolean))
        Try
            Dim d As Date = tu.Item1
            Dim isMonth As Boolean = tu.Item2

            '蒐集資料
            Using uow As IUnitOfWork = DependencyContainer.Resolve(Of IUnitOfWork)()
                Dim datas = uow.ReportRepository.CustomersGasDetailByDay(d, isMonth)

                '套版
                Dim filePath = Path.Combine(Application.StartupPath, "Report", "氣量氣款收付明細表範本檔.xlsx")

                Using xml As New CloseXML_Excel(filePath)
                    With xml
                        .SelectWorksheet("Sheet1")

                        If isMonth Then
                            .WriteToCell(2, 1, $"{d:yyyy年MM月 氣量氣款收付明細表}")
                        Else
                            .WriteToCell(2, 1, $"{d:yyyy年MM月dd日 氣量氣款收付明細表}")
                        End If

                        .WriteToCell(3, 7, $"列印日期: {Now:yyyy/MM/dd}")

                        Dim rowIndex As Integer

                        Dim dataStyle = New CloseXML_Excel.CellFormatOptions With {
                            .Horizontal = XLAlignmentHorizontalValues.Center
                        }

                        For i As Integer = 0 To datas.Count - 1
                            rowIndex = 5 + i

                            .WriteToCell(rowIndex, 1, datas(i).客戶名稱)
                            .WriteToCell(rowIndex, 2, If(datas(i).存氣 <> Nothing, datas(i).存氣.ToString("#,##"), 0), dataStyle)
                            .WriteToCell(rowIndex, 3, If(datas(i).本日提量 <> Nothing, datas(i).本日提量.ToString("#,##"), "0"), dataStyle)
                            .WriteToCell(rowIndex, 4, If(datas(i).當月累計提量 <> Nothing, datas(i).當月累計提量.ToString("#,##"), "0"), dataStyle)
                            .WriteToCell(rowIndex, 5, If(datas(i).本日氣款 <> Nothing, datas(i).本日氣款.ToString("#,##"), "0"), dataStyle)
                            .WriteToCell(rowIndex, 6, If(datas(i).本日收款 <> Nothing, datas(i).本日收款.ToString("#,##"), "0"), dataStyle)
                            .WriteToCell(rowIndex, 7, If(datas(i).結欠 <> Nothing, datas(i).結欠.ToString("#,##"), "0"), dataStyle)
                        Next

                        .SetCustomBorders(rowIndex, 1, rowIndex, 7, bottomStyle:=XLBorderStyleValues.Thin)

                        rowIndex += 1

                        .WriteToCell(rowIndex, 1, "合計:", dataStyle)
                        .WriteToCell(rowIndex, 2, datas.Sum(Function(x) x.存氣).ToString("#,##"), dataStyle)
                        .WriteToCell(rowIndex, 3, datas.Sum(Function(x) x.本日提量).ToString("#,##"), dataStyle)
                        .WriteToCell(rowIndex, 4, datas.Sum(Function(x) x.當月累計提量).ToString("#,##"), dataStyle)
                        .WriteToCell(rowIndex, 5, datas.Sum(Function(x) x.本日氣款).ToString("#,##"), dataStyle)
                        .WriteToCell(rowIndex, 6, datas.Sum(Function(x) x.本日收款).ToString("#,##"), dataStyle)
                        .WriteToCell(rowIndex, 7, datas.Sum(Function(x) x.結欠).ToString("#,##"), dataStyle)

                        '存檔
                        If isMonth Then
                            .SaveExcel($"日氣量氣款收付明細表_{d:yyyyMM}")
                        Else
                            .SaveExcel($"日氣量氣款收付明細表_{d:yyyyMMdd}")
                        End If

                    End With
                End Using
            End Using
        Catch ex As Exception
            MsgBox("產生氣量氣款收付明細表出現錯誤:" + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生客戶提氣清冊
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="tu"></param>
    Private Sub GenerateCustomersGetGasList(sender As Object, tu As Tuple(Of Date, Boolean))
        Try
            Dim d As Date = tu.Item1
            Dim isMonth As Boolean = tu.Item2

            Using uow As New UnitOfWork
                '蒐集資料
                Dim datas = uow.ReportRepository.CustomersGetGasList(d, isMonth)

                '套版
                Dim filePath = Path.Combine(Application.StartupPath, "Report", "客戶提氣清冊範本檔.xlsx")

                Using xml As New CloseXML_Excel(filePath)
                    With xml
                        .SelectWorksheet("Sheet1")

                        ' 設定表頭
                        Dim ws = .Worksheet

                        ws.PageSetup.PrintAreas.Clear()
                        ws.PageSetup.SetRowsToRepeatAtTop(1, 5)

                        ws.PageSetup.PaperSize = XLPaperSize.A4Paper
                        ws.PageSetup.Margins.Top = 0.1
                        ws.PageSetup.Margins.Bottom = 0.5
                        ws.PageSetup.Margins.Left = 0.1
                        ws.PageSetup.Margins.Right = 0.1

                        Dim titleStyle = New CloseXML_Excel.CellFormatOptions With {
                            .FontSize = 14,
                            .IsBold = True,
                            .Horizontal = XLAlignmentHorizontalValues.Center
                        }

                        .MergeCells(1, 1, 1, 25)
                        .WriteToCell(1, 1, "豐原液化煤氣分裝場", titleStyle)

                        .MergeCells(2, 1, 2, 25)
                        .WriteToCell(2, 1, "客戶提氣量清單", titleStyle)

                        Dim dateStyle = New CloseXML_Excel.CellFormatOptions With {
                                .Horizontal = XLAlignmentHorizontalValues.Left
                            }
                        If isMonth Then
                            .WriteToCell(3, 1, $"提氣日期: {d:yyyy/MM}", dateStyle)
                        Else
                            .WriteToCell(3, 1, $"提氣日期: {d:yyyy/MM/dd}", dateStyle)
                        End If

                        Dim printDateStyle = New CloseXML_Excel.CellFormatOptions With {
                                .Horizontal = XLAlignmentHorizontalValues.Right
                            }

                        .WriteToCell(3, 25, $"列印日期: {Now:yyyy/MM/dd}", printDateStyle)

                        ws.PageSetup.Footer.Center.AddText("第 &P 頁/共 &N 頁")

                        Dim rowIndex As Integer

                        For i As Integer = 0 To datas.Count - 1
                            rowIndex = 6 + i

                            .SetRowHeight(rowIndex, 0.78)
                            .InsertRow(rowIndex)

                            .WriteToCell(rowIndex, 1, datas(i).客戶名稱)
                            .WriteToCell(rowIndex, 2, If(datas(i).普氣50Kg <> Nothing, datas(i).普氣50Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 3, If(datas(i).丙氣50Kg <> Nothing, datas(i).丙氣50Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 4, If(datas(i).普氣20Kg <> Nothing, datas(i).普氣20Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 5, If(datas(i).丙氣20Kg <> Nothing, datas(i).丙氣20Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 6, If(datas(i).普氣16Kg <> Nothing, datas(i).普氣16Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 7, If(datas(i).丙氣16Kg <> Nothing, datas(i).丙氣16Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 8, If(datas(i).普氣10Kg <> Nothing, datas(i).普氣10Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 9, If(datas(i).丙氣10Kg <> Nothing, datas(i).丙氣10Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 10, If(datas(i).普氣4Kg <> Nothing, datas(i).普氣4Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 11, If(datas(i).丙氣4Kg <> Nothing, datas(i).丙氣4Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 12, If(datas(i).普氣18Kg <> Nothing, datas(i).普氣18Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 13, If(datas(i).丙氣18Kg <> Nothing, datas(i).丙氣18Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 14, If(datas(i).普氣14Kg <> Nothing, datas(i).普氣14Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 15, If(datas(i).丙氣14Kg <> Nothing, datas(i).丙氣14Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 16, If(datas(i).普氣5Kg <> Nothing, datas(i).普氣5Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 17, If(datas(i).丙氣5Kg <> Nothing, datas(i).丙氣5Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 18, If(datas(i).普氣2Kg <> Nothing, datas(i).普氣2Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 19, If(datas(i).丙氣2Kg <> Nothing, datas(i).丙氣2Kg.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 20, If(datas(i).普氣殘氣 <> Nothing, datas(i).普氣殘氣.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 21, If(datas(i).丙氣殘氣 <> Nothing, datas(i).丙氣殘氣.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 22, If(datas(i).普氣提量 <> Nothing, datas(i).普氣提量.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 23, If(datas(i).丙氣提量 <> Nothing, datas(i).丙氣提量.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 24, If(datas(i).普氣實提量 <> Nothing, datas(i).普氣實提量.ToString("#,##"), ""))
                            .WriteToCell(rowIndex, 25, If(datas(i).丙氣實提量 <> Nothing, datas(i).丙氣實提量.ToString("#,##"), ""))
                        Next

                        '合計
                        rowIndex += 1

                        Dim totalStyle = New CloseXML_Excel.CellFormatOptions With {
                            .IsBold = True
                        }

                        .SetRowHeight(rowIndex, 0.78)

                        .WriteToCell(rowIndex, 1, "合計", totalStyle)
                        .WriteToCell(rowIndex, 2, datas.Sum(Function(x) x.普氣50Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 3, datas.Sum(Function(x) x.丙氣50Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 4, datas.Sum(Function(x) x.普氣20Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 5, datas.Sum(Function(x) x.丙氣20Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 6, datas.Sum(Function(x) x.普氣16Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 7, datas.Sum(Function(x) x.丙氣16Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 8, datas.Sum(Function(x) x.普氣10Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 9, datas.Sum(Function(x) x.丙氣10Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 10, datas.Sum(Function(x) x.普氣4Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 11, datas.Sum(Function(x) x.丙氣4Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 12, datas.Sum(Function(x) x.普氣18Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 13, datas.Sum(Function(x) x.丙氣18Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 14, datas.Sum(Function(x) x.普氣14Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 15, datas.Sum(Function(x) x.丙氣14Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 16, datas.Sum(Function(x) x.普氣5Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 17, datas.Sum(Function(x) x.丙氣5Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 18, datas.Sum(Function(x) x.普氣2Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 19, datas.Sum(Function(x) x.丙氣2Kg).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 20, datas.Sum(Function(x) x.普氣殘氣).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 21, datas.Sum(Function(x) x.丙氣殘氣).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 22, datas.Sum(Function(x) x.普氣提量).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 23, datas.Sum(Function(x) x.丙氣提量).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 24, datas.Sum(Function(x) x.普氣實提量).ToString("#,##"), totalStyle)
                        .WriteToCell(rowIndex, 25, datas.Sum(Function(x) x.丙氣實提量).ToString("#,##"), totalStyle)

                        '存檔
                        If isMonth Then
                            .SaveExcel($"客戶提氣清冊_{d:yyyyMM}")
                        Else
                            .SaveExcel($"客戶提氣清冊_{d:yyyyMMdd}")
                        End If

                    End With
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("列印失敗:" + ex.Message)
        End Try
    End Sub

    Private Sub GetYesterdayStk(ByRef customers As List(Of customer))
        Try
            For Each cus In customers
                Dim orders = _ordRep.GetByCusIdAndDate(cus.cus_id, Now)

                cus.cus_gas_50 += orders.Sum(Function(x) x.o_gas_50) + orders.Sum(Function(x) x.o_gas_c_50) + orders.Sum(Function(x) x.o_empty_50) - orders.Sum(Function(x) x.o_in_50) - orders.Sum(Function(x) x.o_new_in_50) - orders.Sum(Function(x) x.o_inspect_50)
                cus.cus_gas_20 += orders.Sum(Function(x) x.o_gas_20) + orders.Sum(Function(x) x.o_gas_c_20) + orders.Sum(Function(x) x.o_empty_20) - orders.Sum(Function(x) x.o_in_20) - orders.Sum(Function(x) x.o_new_in_20) - orders.Sum(Function(x) x.o_inspect_20)
                cus.cus_gas_18 += orders.Sum(Function(x) x.o_gas_18) + orders.Sum(Function(x) x.o_gas_c_18) + orders.Sum(Function(x) x.o_empty_18) - orders.Sum(Function(x) x.o_in_18) - orders.Sum(Function(x) x.o_new_in_18) - orders.Sum(Function(x) x.o_inspect_18)
                cus.cus_gas_16 += orders.Sum(Function(x) x.o_gas_16) + orders.Sum(Function(x) x.o_gas_c_16) + orders.Sum(Function(x) x.o_empty_16) - orders.Sum(Function(x) x.o_in_16) - orders.Sum(Function(x) x.o_new_in_16) - orders.Sum(Function(x) x.o_inspect_16)
                cus.cus_gas_14 += orders.Sum(Function(x) x.o_gas_14) + orders.Sum(Function(x) x.o_gas_c_14) + orders.Sum(Function(x) x.o_empty_14) - orders.Sum(Function(x) x.o_in_14) - orders.Sum(Function(x) x.o_new_in_14) - orders.Sum(Function(x) x.o_inspect_14)
                cus.cus_gas_10 += orders.Sum(Function(x) x.o_gas_10) + orders.Sum(Function(x) x.o_gas_c_10) + orders.Sum(Function(x) x.o_empty_10) - orders.Sum(Function(x) x.o_in_10) - orders.Sum(Function(x) x.o_new_in_10) - orders.Sum(Function(x) x.o_inspect_10)
                cus.cus_gas_5 += orders.Sum(Function(x) x.o_gas_5) + orders.Sum(Function(x) x.o_gas_c_5) + orders.Sum(Function(x) x.o_empty_5) - orders.Sum(Function(x) x.o_in_5) - orders.Sum(Function(x) x.o_new_in_5) - orders.Sum(Function(x) x.o_inspect_5)
                cus.cus_gas_4 += orders.Sum(Function(x) x.o_gas_4) + orders.Sum(Function(x) x.o_gas_c_4) + orders.Sum(Function(x) x.o_empty_4) - orders.Sum(Function(x) x.o_in_4) - orders.Sum(Function(x) x.o_new_in_4) - orders.Sum(Function(x) x.o_inspect_4)
                cus.cus_gas_2 += orders.Sum(Function(x) x.o_gas_2) + orders.Sum(Function(x) x.o_gas_c_2) + orders.Sum(Function(x) x.o_empty_2) - orders.Sum(Function(x) x.o_in_2) - orders.Sum(Function(x) x.o_new_in_2) - orders.Sum(Function(x) x.o_inspect_2)
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function FillTemplate(templatePath As String, data As OrderVoucherVM) As String
        Dim htmlContent = File.ReadAllText(templatePath)

        htmlContent = htmlContent.Replace("{{客戶名稱}}", data.客戶名稱.ToString)
        htmlContent = htmlContent.Replace("{{車號}}", If(data.車號, ""))
        htmlContent = htmlContent.Replace("{{提氣時間}}", data.提氣時間.ToString("yyyy/MM/dd HH:mm"))
        htmlContent = htmlContent.Replace("{{提單編號}}", data.提單編號.ToString("D6"))

        htmlContent = htmlContent.Replace("{{丙氣50kg}}", If(data.丙氣50kg = 0, "", data.丙氣50kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣20kg}}", If(data.丙氣20kg = 0, "", data.丙氣20kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣16kg}}", If(data.丙氣16kg = 0, "", data.丙氣16kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣10kg}}", If(data.丙氣10kg = 0, "", data.丙氣10kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣4kg}}", If(data.丙氣4kg = 0, "", data.丙氣4kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣15kg}}", If(data.丙氣18kg = 0, "", data.丙氣18kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣2kg}}", If(data.丙氣2kg = 0, "", data.丙氣2kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣14kg}}", If(data.丙氣14kg = 0, "", data.丙氣14kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣5kg}}", If(data.丙氣5kg = 0, "", data.丙氣5kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣kg數}}", If(data.丙氣kg數 = 0, "", data.丙氣kg數.ToString))

        htmlContent = htmlContent.Replace("{{普氣50kg}}", If(data.普氣50kg = 0, "", data.普氣50kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣20kg}}", If(data.普氣20kg = 0, "", data.普氣20kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣16kg}}", If(data.普氣16kg = 0, "", data.普氣16kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣10kg}}", If(data.普氣10kg = 0, "", data.普氣10kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣4kg}}", If(data.普氣4kg = 0, "", data.普氣4kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣15kg}}", If(data.普氣18kg = 0, "", data.普氣18kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣2kg}}", If(data.普氣2kg = 0, "", data.普氣2kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣14kg}}", If(data.普氣14kg = 0, "", data.普氣14kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣5kg}}", If(data.普氣5kg = 0, "", data.普氣5kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣kg數}}", If(data.普氣kg數 = 0, "", data.普氣kg數.ToString))

        htmlContent = htmlContent.Replace("{{檢驗50kg}}", If(data.檢驗50kg = 0, "", data.檢驗50kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗20kg}}", If(data.檢驗20kg = 0, "", data.檢驗20kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗16kg}}", If(data.檢驗16kg = 0, "", data.檢驗16kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗10kg}}", If(data.檢驗10kg = 0, "", data.檢驗10kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗4kg}}", If(data.檢驗4kg = 0, "", data.檢驗4kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗15kg}}", If(data.檢驗18kg = 0, "", data.檢驗18kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗2kg}}", If(data.檢驗2kg = 0, "", data.檢驗2kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗14kg}}", If(data.檢驗14kg = 0, "", data.檢驗14kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗5kg}}", If(data.檢驗5kg = 0, "", data.檢驗5kg.ToString))

        htmlContent = htmlContent.Replace("{{新瓶50kg}}", If(data.新瓶50kg = 0, "", data.新瓶50kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶20kg}}", If(data.新瓶20kg = 0, "", data.新瓶20kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶16kg}}", If(data.新瓶16kg = 0, "", data.新瓶16kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶10kg}}", If(data.新瓶10kg = 0, "", data.新瓶10kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶4kg}}", If(data.新瓶4kg = 0, "", data.新瓶4kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶15kg}}", If(data.新瓶18kg = 0, "", data.新瓶18kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶2kg}}", If(data.新瓶2kg = 0, "", data.新瓶2kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶14kg}}", If(data.新瓶14kg = 0, "", data.新瓶14kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶5kg}}", If(data.新瓶5kg = 0, "", data.新瓶5kg.ToString))

        htmlContent = htmlContent.Replace("{{收空瓶50kg}}", If(data.收空瓶50kg = 0, "", data.收空瓶50kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶20kg}}", If(data.收空瓶20kg = 0, "", data.收空瓶20kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶16kg}}", If(data.收空瓶16kg = 0, "", data.收空瓶16kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶10kg}}", If(data.收空瓶10kg = 0, "", data.收空瓶10kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶4kg}}", If(data.收空瓶4kg = 0, "", data.收空瓶4kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶15kg}}", If(data.收空瓶18kg = 0, "", data.收空瓶18kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶2kg}}", If(data.收空瓶2kg = 0, "", data.收空瓶2kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶14kg}}", If(data.收空瓶14kg = 0, "", data.收空瓶14kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶5kg}}", If(data.收空瓶5kg = 0, "", data.收空瓶5kg.ToString))

        htmlContent = htmlContent.Replace("{{退空瓶50kg}}", If(data.退空瓶50kg = 0, "", data.退空瓶50kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶20kg}}", If(data.退空瓶20kg = 0, "", data.退空瓶20kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶16kg}}", If(data.退空瓶16kg = 0, "", data.退空瓶16kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶10kg}}", If(data.退空瓶10kg = 0, "", data.退空瓶10kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶4kg}}", If(data.退空瓶4kg = 0, "", data.退空瓶4kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶15kg}}", If(data.退空瓶18kg = 0, "", data.退空瓶18kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶2kg}}", If(data.退空瓶2kg = 0, "", data.退空瓶2kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶14kg}}", If(data.退空瓶14kg = 0, "", data.退空瓶14kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶5kg}}", If(data.退空瓶5kg = 0, "", data.退空瓶5kg.ToString))

        htmlContent = htmlContent.Replace("{{結存50kg}}", If(data.結存50kg = 0, "", data.結存50kg.ToString))
        htmlContent = htmlContent.Replace("{{結存20kg}}", If(data.結存20kg = 0, "", data.結存20kg.ToString))
        htmlContent = htmlContent.Replace("{{結存16kg}}", If(data.結存16kg = 0, "", data.結存16kg.ToString))
        htmlContent = htmlContent.Replace("{{結存10kg}}", If(data.結存10kg = 0, "", data.結存10kg.ToString))
        htmlContent = htmlContent.Replace("{{結存4kg}}", If(data.結存4kg = 0, "", data.結存4kg.ToString))
        htmlContent = htmlContent.Replace("{{結存15kg}}", If(data.結存18kg = 0, "", data.結存18kg.ToString))
        htmlContent = htmlContent.Replace("{{結存2kg}}", If(data.結存2kg = 0, "", data.結存2kg.ToString))
        htmlContent = htmlContent.Replace("{{結存14kg}}", If(data.結存14kg = 0, "", data.結存14kg.ToString))
        htmlContent = htmlContent.Replace("{{結存5kg}}", If(data.結存5kg = 0, "", data.結存5kg.ToString))

        htmlContent = htmlContent.Replace("{{本日提量}}", data.本日提量.ToString)
        htmlContent = htmlContent.Replace("{{本月累計實提量}}", data.本月累計實提量.ToString)
        htmlContent = htmlContent.Replace("{{尚欠氣款}}", data.尚欠氣款.ToString)
        htmlContent = htmlContent.Replace("{{本日退氣}}", data.本日退氣.ToString)
        htmlContent = htmlContent.Replace("{{本月累計退氣}}", data.本月累計退氣.ToString)
        htmlContent = htmlContent.Replace("{{已收氣款}}", data.已收氣款.ToString)

        Return htmlContent
    End Function

#End Region

#Region "庫存管理輔助方法"

    ''' <summary>
    ''' 檢查當前客戶庫存是否與 initCusStk 相符
    ''' </summary>
    ''' <returns>True=相符, False=不符(可能被其他使用者更新)</returns>
    Private Function IsCustomerStockMatchInit() As Boolean
        If currentCustomer Is Nothing Then Return False

        ' 重新從資料庫載入最新的客戶資料
        Dim freshCustomer = _cusRep.GetByIdAsync(currentCustomer.cus_id).Result
        If freshCustomer Is Nothing Then Return False

        ' 比對所有桶型的庫存
        Return freshCustomer.cus_gas_50 = initCusStk.Barrel_50 AndAlso
               freshCustomer.cus_gas_20 = initCusStk.Barrel_20 AndAlso
               freshCustomer.cus_gas_16 = initCusStk.Barrel_16 AndAlso
               freshCustomer.cus_gas_10 = initCusStk.Barrel_10 AndAlso
               freshCustomer.cus_gas_4 = initCusStk.Barrel_4 AndAlso
               freshCustomer.cus_gas_18 = initCusStk.Barrel_18 AndAlso
               freshCustomer.cus_gas_14 = initCusStk.Barrel_14 AndAlso
               freshCustomer.cus_gas_5 = initCusStk.Barrel_5 AndAlso
               freshCustomer.cus_gas_2 = initCusStk.Barrel_2
    End Function

    ''' <summary>
    ''' 分析訂單數量並反向操作來回退客戶庫存（用於 Delete）
    ''' </summary>
    ''' <param name="ord">要刪除的訂單</param>
    Private Sub RevertCustomerStockByDiff(ord As order)
        If currentCustomer Is Nothing Then
            Throw New Exception("客戶資料不存在")
        End If

        Dim isIn = ord.o_in_out = "進場單"

        For Each barrelType In BARREL_TYPES
            Dim cusProperty = currentCustomer.GetType().GetProperty($"cus_gas_{barrelType}")
            Dim currentStock = CInt(cusProperty.GetValue(currentCustomer))
            Dim revertQty As Integer = 0

            If isIn Then
                ' 進場單：當時收瓶，刪除時要減回去
                Dim inQty = CInt(ord.GetType().GetProperty($"o_in_{barrelType}").GetValue(ord))
                Dim newInQty = CInt(ord.GetType().GetProperty($"o_new_in_{barrelType}").GetValue(ord))
                Dim inspectQty = CInt(ord.GetType().GetProperty($"o_inspect_{barrelType}").GetValue(ord))
                revertQty = -(inQty + newInQty + inspectQty)
            Else
                ' 出場單：當時出瓶，刪除時要加回去
                Dim gasQty = CInt(ord.GetType().GetProperty($"o_gas_{barrelType}").GetValue(ord))
                Dim gasCQty = CInt(ord.GetType().GetProperty($"o_gas_c_{barrelType}").GetValue(ord))
                Dim emptyQty = CInt(ord.GetType().GetProperty($"o_empty_{barrelType}").GetValue(ord))
                revertQty = gasQty + gasCQty + emptyQty
            End If

            If revertQty <> 0 Then
                cusProperty.SetValue(currentCustomer, currentStock + revertQty)
                _logger.LogInfo($"[RevertCustomerStockByDiff] {barrelType}Kg: 當前={currentStock}, 回退數量={revertQty}, 回退後={currentStock + revertQty}")
            End If
        Next
    End Sub

    ''' <summary>
    ''' 用差值法更新客戶庫存（比較 initCusStk 與 orderInput.o_cus_XX）
    ''' </summary>
    ''' <param name="orderInput">包含計算後結存瓶（o_cus_XX）的訂單物件</param>
    Private Sub UpdateCustomerStockByDiff(orderInput As order)
        If currentCustomer Is Nothing Then
            Throw New Exception("客戶資料不存在")
        End If

        Dim initProps = initCusStk.GetType().GetProperties()
        Dim orderProps = orderInput.GetType().GetProperties()

        For Each barrelType In BARREL_TYPES
            ' 取得初始庫存
            Dim initProp = initProps.FirstOrDefault(Function(x) x.Name = $"Barrel_{barrelType}")
            Dim initStock = If(initProp IsNot Nothing, CInt(initProp.GetValue(initCusStk)), 0)

            ' 取得計算後的結存瓶
            Dim orderProp = orderProps.FirstOrDefault(Function(x) x.Name = $"o_cus_{barrelType}")
            Dim finalStock = If(orderProp IsNot Nothing, CInt(orderProp.GetValue(orderInput)), 0)

            ' 計算差值並更新客戶庫存
            Dim diff = finalStock - initStock
            Dim cusProperty = currentCustomer.GetType().GetProperty($"cus_gas_{barrelType}")
            Dim currentStock = CInt(cusProperty.GetValue(currentCustomer))

            'edit by Kevin 20251103 不用回存到 customer資料表
            'cusProperty.SetValue(currentCustomer, currentStock + diff)

            If diff <> 0 Then
                _logger.LogInfo($"[UpdateCustomerStockByDiff] {barrelType}Kg: 初始={initStock}, 結存={finalStock}, 差值={diff}, 更新後={currentStock + diff}")
            End If
        Next
    End Sub

#End Region

#Region "格式化與日誌輔助方法"

    ''' <summary>
    ''' 格式化初始庫存資訊
    ''' </summary>
    ''' <returns>格式化字串</returns>
    Private Function FormatInitStock() As String
        Dim items As New List(Of String)
        If initCusStk.Barrel_50 <> 0 Then items.Add($"50Kg={initCusStk.Barrel_50}")
        If initCusStk.Barrel_20 <> 0 Then items.Add($"20Kg={initCusStk.Barrel_20}")
        If initCusStk.Barrel_16 <> 0 Then items.Add($"16Kg={initCusStk.Barrel_16}")
        If initCusStk.Barrel_10 <> 0 Then items.Add($"10Kg={initCusStk.Barrel_10}")
        If initCusStk.Barrel_4 <> 0 Then items.Add($"4Kg={initCusStk.Barrel_4}")
        If initCusStk.Barrel_18 <> 0 Then items.Add($"18Kg={initCusStk.Barrel_18}")
        If initCusStk.Barrel_14 <> 0 Then items.Add($"14Kg={initCusStk.Barrel_14}")
        If initCusStk.Barrel_5 <> 0 Then items.Add($"5Kg={initCusStk.Barrel_5}")
        If initCusStk.Barrel_2 <> 0 Then items.Add($"2Kg={initCusStk.Barrel_2}")

        Return If(items.Count > 0, String.Join(",", items), "全部為0")
    End Function

    ''' <summary>
    ''' 格式化瓦斯桶庫存資訊
    ''' </summary>
    ''' <param name="cus">客戶物件</param>
    ''' <returns>格式化字串，例如："50Kg=10,20Kg=5,16Kg=3"</returns>
    Private Function FormatBarrelStock(cus As customer) As String
        If cus Is Nothing Then Return "無資料"

        Dim items As New List(Of String)
        If cus.cus_gas_50 <> 0 Then items.Add($"50Kg={cus.cus_gas_50}")
        If cus.cus_gas_20 <> 0 Then items.Add($"20Kg={cus.cus_gas_20}")
        If cus.cus_gas_16 <> 0 Then items.Add($"16Kg={cus.cus_gas_16}")
        If cus.cus_gas_10 <> 0 Then items.Add($"10Kg={cus.cus_gas_10}")
        If cus.cus_gas_4 <> 0 Then items.Add($"4Kg={cus.cus_gas_4}")
        If cus.cus_gas_18 <> 0 Then items.Add($"18Kg={cus.cus_gas_18}")
        If cus.cus_gas_14 <> 0 Then items.Add($"14Kg={cus.cus_gas_14}")
        If cus.cus_gas_5 <> 0 Then items.Add($"5Kg={cus.cus_gas_5}")
        If cus.cus_gas_2 <> 0 Then items.Add($"2Kg={cus.cus_gas_2}")

        Return If(items.Count > 0, String.Join(",", items), "全部為0")
    End Function

    ''' <summary>
    ''' 格式化訂單中的瓦斯桶數量
    ''' </summary>
    ''' <param name="ord">訂單物件</param>
    ''' <param name="isIn">是否為進場單</param>
    ''' <returns>格式化字串</returns>
    Private Function FormatOrderBarrelQty(ord As order, isIn As Boolean) As String
        If ord Is Nothing Then Return "無訂單資料"

        Dim items As New List(Of String)

        If isIn Then
            ' 進場單：收舊瓶、新瓶、檢驗
            For Each barrelType In BARREL_TYPES
                Dim inQty = CInt(ord.GetType().GetProperty($"o_in_{barrelType}")?.GetValue(ord))
                Dim newInQty = CInt(ord.GetType().GetProperty($"o_new_in_{barrelType}")?.GetValue(ord))
                Dim inspectQty = CInt(ord.GetType().GetProperty($"o_inspect_{barrelType}")?.GetValue(ord))

                If inQty <> 0 OrElse newInQty <> 0 OrElse inspectQty <> 0 Then
                    Dim parts As New List(Of String)
                    If inQty <> 0 Then parts.Add($"收舊瓶:{inQty}")
                    If newInQty <> 0 Then parts.Add($"新瓶:{newInQty}")
                    If inspectQty <> 0 Then parts.Add($"檢驗:{inspectQty}")
                    items.Add($"{barrelType}Kg({String.Join(",", parts)})")
                End If
            Next
        Else
            ' 出場單：普氣、丙氣、空瓶
            For Each barrelType In BARREL_TYPES
                Dim gasQty = CInt(ord.GetType().GetProperty($"o_gas_{barrelType}")?.GetValue(ord))
                Dim gasCQty = CInt(ord.GetType().GetProperty($"o_gas_c_{barrelType}")?.GetValue(ord))
                Dim emptyQty = CInt(ord.GetType().GetProperty($"o_empty_{barrelType}")?.GetValue(ord))

                If gasQty <> 0 OrElse gasCQty <> 0 OrElse emptyQty <> 0 Then
                    Dim parts As New List(Of String)
                    If gasQty <> 0 Then parts.Add($"普氣:{gasQty}")
                    If gasCQty <> 0 Then parts.Add($"丙氣:{gasCQty}")
                    If emptyQty <> 0 Then parts.Add($"空瓶:{emptyQty}")
                    items.Add($"{barrelType}Kg({String.Join(",", parts)})")
                End If
            Next
        End If

        Return If(items.Count > 0, String.Join(",", items), "全部為0")
    End Function

#End Region

End Class