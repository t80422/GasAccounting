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

    Private currentCustomer As customer
    Private currentOrder As order
    Public currentCar As car

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

    Public Sub New(view As IOrderView, cusRep As ICustomerRep, carRep As ICarRep, ordRep As IOrderRep, gbRep As IGasBarrelRep, barMBService As IBarrelMonthlyBalanceService,
                   priceCalSer As IPriceCalculationService, aeSer As IAccountingEntryService, printerSer As IPrinterService, ocmSer As IOrderCollectionMappingService,
                   reportRep As IReportRep, maService As IMonthlyAccountService, unitOfWork As IUnitOfWork, logger As ILoggerService)
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

    Private Sub OnSearch()
        Try
            Dim data = _view.GetSearchCriteria
            LoadList(data)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub OnCustomerSelected(sender As Object, cusCode As String)
        Try
            LoadCustomer(cusCode)
            LoadUnitPrice()
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

    ''' <summary>
    ''' 計算結存瓶
    ''' </summary>
    Private Sub CaculateCusBarrelStock()
        Try
            If currentCustomer Is Nothing Then Exit Sub

            Dim isIn = _view.GetOrderType = "進場單"
            Dim orderType = If(isIn, "進場單", "出場單")
            Dim cusCode = currentCustomer.cus_code
            
            ' 記錄計算前的庫存
            Dim beforeStock = FormatBarrelStock(currentCustomer)
            
            Dim cusProps = currentCustomer?.GetType.GetProperties
            Dim inputInOut = If(isIn, _view.GetInInput, _view.GetOutInput)
            Dim inputProps = inputInOut.GetType.GetProperties
            Dim currentOrderProps = currentOrder?.GetType.GetProperties
            Dim cusBarrelStock As New customer

            For Each prop In cusProps.Where(Function(x) x.Name.StartsWith("cus_gas_"))
                Dim barrelType = prop.Name.Substring(8)
                Dim currentStk = prop.GetValue(currentCustomer)
                Dim barrelStk As Integer ' 計算後的瓦斯桶庫存
                Dim targetProp = cusProps.FirstOrDefault(Function(x) x.Name = $"cus_gas_{barrelType}")

                If isIn Then
                    '計算瓦斯桶庫存
                    Dim inQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_in_{barrelType}").GetValue(inputInOut)
                    Dim newInQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_new_in_{barrelType}").GetValue(inputInOut)
                    Dim inspectInQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_inspect_{barrelType}").GetValue(inputInOut)
                    Dim orderInQty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_in_{barrelType}").GetValue(currentOrder)
                    Dim orderNewInQty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_new_in_{barrelType}").GetValue(currentOrder)
                    Dim orderInspectInQty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_inspect_{barrelType}").GetValue(currentOrder)

                    barrelStk = currentStk + inQty - orderInQty + newInQty - orderNewInQty + inspectInQty - orderInspectInQty
                Else
                    '計算瓦斯桶庫存
                    Dim gasC As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_gas_c_{barrelType}").GetValue(inputInOut)
                    Dim gas As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_gas_{barrelType}").GetValue(inputInOut)
                    Dim empty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_empty_{barrelType}").GetValue(inputInOut)
                    Dim orderGasC As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_gas_c_{barrelType}").GetValue(currentOrder)
                    Dim orderGas As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_gas_{barrelType}").GetValue(currentOrder)
                    Dim orderEmpty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_empty_{barrelType}").GetValue(currentOrder)

                    barrelStk = currentStk - gasC + orderGasC - gas + orderGas - empty + orderEmpty
                End If

                targetProp.SetValue(cusBarrelStock, barrelStk)
            Next

            ' 記錄計算後的庫存
            Dim afterStock = FormatBarrelStock(cusBarrelStock)
            Dim stockDiff = FormatBarrelDiff(currentCustomer, cusBarrelStock)
            
            _logger.LogInfo($"[CaculateCusBarrelStock] 客戶[{cusCode}] {orderType} | 計算前:{beforeStock} | 計算後:{afterStock} | 變化:{stockDiff}")
            
            _view.SetCusBarrelStock(isIn, cusBarrelStock)
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
        Dim input = _view.GetOrderInput
        Dim result As Double = 0

        If currentCustomer.cus_IsInsurance Then
            result = ((input.o_gas_total + input.o_gas_c_total) + (input.o_return + input.o_return_c)) * input.o_insurance_unit_price
        End If

        _view.ShowInsurance(Math.Round(result, 2))
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

    Private Sub Add()
        Using transaction = _ordRep.BeginTransaction
            Try
                RefreshCurrentEntities()
                
                ' 記錄交易前的客戶庫存
                Dim cusCode = currentCustomer?.cus_code
                Dim beforeStock = FormatBarrelStock(currentCustomer)
                Dim orderType = _view.GetOrderType
                
                _logger.LogInfo($"[Add] 交易開始 | 客戶[{cusCode}] | 訂單類型:{orderType} | 交易前結存:{beforeStock}")
                
                Dim orderInput As New order
                _view.GetInput(orderInput)
                
                ' 記錄輸入的瓦斯桶數量
                Dim inputQty = FormatOrderBarrelQty(orderInput, orderType = "進場單")
                _logger.LogInfo($"[Add] 輸入數量 | 客戶[{cusCode}] | {inputQty}")
                
                Dim order = _ordRep.AddAsync(orderInput).Result

                _view.GetCusStkInput(currentCustomer)
                
                ' 記錄交易後的客戶庫存
                Dim afterStock = FormatBarrelStock(currentCustomer)
                Dim stockDiff = FormatBarrelDiff_FromString(beforeStock, afterStock)
                _logger.LogInfo($"[Add] 交易後結存 | 客戶[{cusCode}] | 交易後:{afterStock} | 變化:{stockDiff}")

                If currentCar IsNot Nothing Then _view.GetCarStkInput(currentCar)

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
                
                _logger.LogInfo($"[Add] 交易成功 | 訂單ID:{order.o_id} | 客戶[{cusCode}]")
                
                Initialize()
                MessageBox.Show("新增成功")
                LoadDetail(Nothing, order.o_id)
            Catch ex As Exception
                transaction.Rollback()

                Dim innerEx = ex
                While innerEx.InnerException IsNot Nothing
                    innerEx = innerEx.InnerException
                End While

                MessageBox.Show(innerEx.Message)
            End Try
        End Using
    End Sub

    Private Sub LoadDetail(sender As Object, id As Integer)
        Try
            currentOrder = _ordRep.GetByIdAsync(id).Result

            If currentOrder Is Nothing Then Throw New Exception("資料已被刪除，請刷新")

            _ordRep.Reload(currentOrder)

            _view.ClearInput()

            currentCustomer = currentOrder.customer
            currentCar = If(currentOrder.o_in_out = "進場單", currentOrder.car, currentOrder.car1)

            RefreshCurrentEntities()

            _view.ShowCustomer(currentCustomer)
            _view.ShowDetail(currentOrder)
            _view.ButtonStatus(True)

            If currentCar IsNot Nothing Then LoadCarBarrelStock(currentCar.c_id)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Update()
        Using transaction = _ordRep.BeginTransaction
            Try
                RefreshCurrentEntities(includeOrder:=True)
                
                ' 記錄修改前的資訊
                Dim cusCode = currentCustomer?.cus_code
                Dim orderId = currentOrder?.o_id
                Dim orderType = _view.GetOrderType
                Dim beforeStock = FormatBarrelStock(currentCustomer)
                Dim beforeOrderQty = FormatOrderBarrelQty(currentOrder, orderType = "進場單")
                
                _logger.LogInfo($"[Update] 修改開始 | 訂單ID:{orderId} | 客戶[{cusCode}] | 修改前結存:{beforeStock} | 修改前訂單:{beforeOrderQty}")
                
                _view.GetInput(currentOrder)
                
                ' 記錄修改後的訂單數量
                Dim afterOrderQty = FormatOrderBarrelQty(currentOrder, orderType = "進場單")
                _logger.LogInfo($"[Update] 修改後訂單 | 訂單ID:{orderId} | 客戶[{cusCode}] | {afterOrderQty}")
                
                _view.GetCusStkInput(currentCustomer)
                
                ' 記錄修改後的客戶庫存
                Dim afterStock = FormatBarrelStock(currentCustomer)
                Dim stockDiff = FormatBarrelDiff_FromString(beforeStock, afterStock)
                _logger.LogInfo($"[Update] 修改後結存 | 訂單ID:{orderId} | 客戶[{cusCode}] | 修改後:{afterStock} | 變化:{stockDiff}")
                
                If currentCar IsNot Nothing Then _view.GetCarStkInput(currentCar)

                _service.UpdateOrAddAsync(currentOrder.o_date)

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
                Dim orderType = _view.GetOrderType
                Dim beforeStock = FormatBarrelStock(currentCustomer)
                Dim orderQty = FormatOrderBarrelQty(currentOrder, orderType = "進場單")
                
                _logger.LogInfo($"[Delete] 刪除開始 | 訂單ID:{orderId} | 客戶[{cusCode}] | 刪除前結存:{beforeStock} | 訂單數量:{orderQty}")

                ' 更新客戶庫存、存氣
                If currentCustomer Is Nothing Then Throw New Exception("未取得客戶資料")

                Dim isIn = _view.GetOrderType = "進場單"
                Dim cusProps = currentCustomer?.GetType.GetProperties
                Dim inputInOut = If(isIn, _view.GetInInput, _view.GetOutInput)
                Dim inputProps = inputInOut.GetType.GetProperties

                For Each prop In cusProps.Where(Function(x) x.Name.StartsWith("cus_gas_"))
                    Dim barrelType = prop.Name.Substring(8)
                    Dim currentStk = prop.GetValue(currentCustomer)
                    Dim barrelStk As Integer ' 計算後的瓦斯桶庫存
                    Dim targetProp = cusProps.FirstOrDefault(Function(x) x.Name = $"cus_gas_{barrelType}")

                    If isIn Then
                        Dim inQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_in_{barrelType}").GetValue(inputInOut)
                        Dim newInQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_new_in_{barrelType}").GetValue(inputInOut)
                        Dim inspectInQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_inspect_{barrelType}").GetValue(inputInOut)

                        barrelStk = currentStk - (inQty + newInQty + inspectInQty)
                    Else
                        Dim gasC As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_gas_c_{barrelType}").GetValue(inputInOut)
                        Dim gas As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_gas_{barrelType}").GetValue(inputInOut)
                        Dim empty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_empty_{barrelType}").GetValue(inputInOut)

                        barrelStk = currentStk + (gasC + gas + empty)
                    End If

                    targetProp.SetValue(currentCustomer, barrelStk)
                Next

                ' 記錄刪除後的客戶庫存（應該回退）
                Dim afterStock = FormatBarrelStock(currentCustomer)
                Dim stockDiff = FormatBarrelDiff_FromString(beforeStock, afterStock)
                _logger.LogInfo($"[Delete] 回退結存 | 訂單ID:{orderId} | 客戶[{cusCode}] | 回退後:{afterStock} | 變化:{stockDiff}")

                ' 更新車輛庫存
                If currentOrder.o_delivery_type = "自運" Then
                    If currentCar Is Nothing Then Throw New Exception("未取得車輛資訊")

                    Dim carProps = currentCar?.GetType.GetProperties
                    Dim currentOrderProps = currentOrder?.GetType.GetProperties
                    Dim result As New car
                    Dim resultVaule As Integer

                    For Each prop In carProps.Where(Function(x) x.Name.StartsWith("c_deposit_"))
                        Dim barrelType = prop.Name.Substring(10)
                        Dim columnName = If(isIn, "o_deposit_in_", "o_deposit_out_") & barrelType
                        Dim qty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = columnName).GetValue(inputInOut)
                        Dim currentStock = prop.GetValue(currentCar)
                        Dim targetProp = carProps.FirstOrDefault(Function(x) x.Name = $"c_deposit_{barrelType}")

                        If isIn Then
                            resultVaule = currentStock - qty
                        Else
                            resultVaule = currentStock + qty
                        End If

                        targetProp.SetValue(currentCar, resultVaule)
                    Next
                End If


                ' 同步更新月度帳單資料
                _maService.SyncOrderToMonthlyAccount(currentOrder.o_id, False, True)

                ' 銷帳
                _ocmSer.DeleteOrder(currentOrder.o_id)

                ' 刪除訂單
                _ordRep.DeleteAsync(currentOrder)

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

    Private Sub Print(sender As Object, orderId As Integer)
        Dim templatePath As String = ""
        Dim pdfPath As String = ""
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

            MsgBox("成功")
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

            ' 同時記錄到控制台供偵錯
            Console.WriteLine(errorDetails)
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

                    For Each cus In customers
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
            Dim datas = _reportRep.CustomersGasDetailByDay(d, isMonth)

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

            '蒐集資料
            Dim datas = _unitOfWork.ReportRepository.CustomersGetGasList(d, isMonth)

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
    ''' 格式化瓦斯桶庫存變化
    ''' </summary>
    ''' <param name="before">變化前的客戶物件</param>
    ''' <param name="after">變化後的客戶物件</param>
    ''' <returns>格式化字串，例如："50Kg:10→15(+5),20Kg:5→8(+3)"</returns>
    Private Function FormatBarrelDiff(before As customer, after As customer) As String
        If before Is Nothing OrElse after Is Nothing Then Return "無資料"
        
        Dim items As New List(Of String)
        Dim barrelTypes = New String() {"50", "20", "16", "10", "4", "18", "14", "5", "2"}
        
        For Each barrelType In barrelTypes
            Dim beforeVal = CInt(before.GetType().GetProperty($"cus_gas_{barrelType}").GetValue(before))
            Dim afterVal = CInt(after.GetType().GetProperty($"cus_gas_{barrelType}").GetValue(after))
            Dim diff = afterVal - beforeVal
            
            If diff <> 0 Then
                Dim sign = If(diff > 0, "+", "")
                items.Add($"{barrelType}Kg:{beforeVal}→{afterVal}({sign}{diff})")
            End If
        Next
        
        Return If(items.Count > 0, String.Join(",", items), "無變化")
    End Function

    ''' <summary>
    ''' 從字串格式計算庫存變化（用於 Add/Update/Delete 方法）
    ''' </summary>
    ''' <param name="beforeStr">變化前的字串，例如："50Kg=10,20Kg=5"</param>
    ''' <param name="afterStr">變化後的字串，例如："50Kg=15,20Kg=8"</param>
    ''' <returns>格式化字串，例如："50Kg:10→15(+5),20Kg:5→8(+3)"</returns>
    Private Function FormatBarrelDiff_FromString(beforeStr As String, afterStr As String) As String
        If String.IsNullOrEmpty(beforeStr) OrElse String.IsNullOrEmpty(afterStr) Then Return "無資料"
        If beforeStr = "無資料" OrElse afterStr = "無資料" Then Return "無資料"
        If beforeStr = "全部為0" AndAlso afterStr = "全部為0" Then Return "無變化"
        
        ' 解析字串為 Dictionary
        Dim ParseStock = Function(str As String) As Dictionary(Of String, Integer)
            Dim result As New Dictionary(Of String, Integer)
            If str = "全部為0" Then Return result
            
            For Each item In str.Split(","c)
                Dim parts = item.Split("="c)
                If parts.Length = 2 Then
                    result(parts(0).Trim()) = Integer.Parse(parts(1).Trim())
                End If
            Next
            Return result
        End Function
        
        Dim beforeDict = ParseStock(beforeStr)
        Dim afterDict = ParseStock(afterStr)
        
        ' 收集所有的公斤數類型
        Dim allTypes As New HashSet(Of String)(beforeDict.Keys)
        For Each key In afterDict.Keys
            allTypes.Add(key)
        Next
        
        Dim items As New List(Of String)
        Dim barrelOrder = New String() {"50Kg", "20Kg", "16Kg", "10Kg", "4Kg", "18Kg", "14Kg", "5Kg", "2Kg"}
        
        For Each barrelType In barrelOrder
            If allTypes.Contains(barrelType) Then
                Dim beforeVal = If(beforeDict.ContainsKey(barrelType), beforeDict(barrelType), 0)
                Dim afterVal = If(afterDict.ContainsKey(barrelType), afterDict(barrelType), 0)
                Dim diff = afterVal - beforeVal
                
                If diff <> 0 Then
                    Dim sign = If(diff > 0, "+", "")
                    items.Add($"{barrelType}:{beforeVal}→{afterVal}({sign}{diff})")
                End If
            End If
        Next
        
        Return If(items.Count > 0, String.Join(",", items), "無變化")
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
        Dim barrelTypes = New String() {"50", "20", "16", "10", "4", "18", "14", "5", "2"}
        
        If isIn Then
            ' 進場單：收舊瓶、新瓶、檢驗
            For Each barrelType In barrelTypes
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
            For Each barrelType In barrelTypes
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
End Class