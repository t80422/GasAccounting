Imports System.Drawing.Printing
Imports System.IO
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
    Private ReadOnly _bpRep As IBasicPriceRep
    Private ReadOnly _compRep As ICompanyRep

    Private currentCustomer As customer
    Private currentOrder As order

    Public CurrentCarIn As car
    Public CurrentCarOut As car




    Private _service As ICusOrdByCarService
    Private _ordRep As IOrderRep

    Private gasReturn As New Dictionary(Of String, Integer) '儲存客戶訂單瓦斯退氣量

    Public OrgCarStk As car
    Public Property GasBarrel As New Dictionary(Of String, Object) '儲存客戶訂單瓦斯桶數量
    Public Property InspectBarrel As New Dictionary(Of String, Object) '儲存客戶訂單檢驗瓶數量
    Public Property StockValues As New Dictionary(Of String, Object) '儲存客戶瓦斯桶庫存
    Public Property GasValues As New Dictionary(Of String, Integer) '用於存儲 txtGas_、txtGas_c_ 開頭的 TextBox 的初始值
    Public Property DepositValues As New Dictionary(Of String, Object) '用於存儲TextBox.Tag = o_deposit開頭的初始值

    Public Sub New(view As IOrderView, cusRep As ICustomerRep, carRep As ICarRep, compRep As ICompanyRep, ordRep As IOrderRep, gbRep As IGasBarrelRep, bpRep As IBasicPriceRep)
        _view = view
        _cusRep = cusRep
        _carRep = carRep
        _ordRep = ordRep
        _compRep = compRep
        _gbRep = gbRep
        _bpRep = bpRep
    End Sub

    Public Async Function InitializeAsync() As Task
        Try
            Await LoadCompanyAsync()
            _view.ClearInput()
            Await LoadList()
            currentOrder = Nothing
            currentCustomer = Nothing
            CurrentCarIn = Nothing
            CurrentCarOut = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Function LoadList() As Task
        Try
            Dim criteria = _view.GetSearchCriteria()
            Dim datas = Await _ordRep.SearchAsync(criteria)

            _view.ClearInput()
            _view.DisplayList(datas.Select(Function(x) New OrderVM(x)).ToList)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function LoadCustomerByCusCode(cusCode As String) As Boolean
        Try
            currentCustomer = _cusRep.GetByCusCode(cusCode)

            If currentCustomer Is Nothing Then
                MsgBox("查無此客戶")
                Return False
            End If

            _view.DisplayCustomer(currentCustomer)
            _view.DisplayCusStk(currentCustomer, True)
            _view.DisplayCusStk(currentCustomer, False)
            Return True
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try

        Return False
    End Function

    Public Sub LoadCar()
        Try
            If currentCustomer Is Nothing Then Return

            '設定車選單
            Dim carsDropdown = currentCustomer.cars.Select(Function(x) New SelectListItem With {.Display = $"{x.c_no}-{x.c_driver}", .Value = x.c_id}).ToList
            _view.SetCarDropdown(carsDropdown)

        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub LoadCarStk_In(carId As Integer)
        Try
            CurrentCarIn = Await _carRep.GetByIdAsync(carId)
            _view.DisplayCarStk(CurrentCarIn, True)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub LoadCarStk_Out(carId As Integer)
        Try
            currentCarOut = Await _carRep.GetByIdAsync(carId)
            _view.DisplayCarStk(currentCarOut, False)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub CalculateStkAndPrice(isIn As Boolean)
        Try
            If currentCustomer Is Nothing Then
                MsgBox("請先選擇客戶")
                Return
            End If

            Dim result As New customer
            Dim inputInOut = If(isIn, _view.GetInInput, _view.GetOutInput)
            Dim inputOrder = _view.GetOrderInput
            Dim cusProps = currentCustomer.GetType.GetProperties
            Dim inputProps = inputInOut.GetType.GetProperties
            Dim totalBarrelPrice As Integer '總瓦斯瓶價格
            Dim totalGas As Integer '總普氣
            Dim totalGasC As Integer '總丙氣
            Dim currentOrderProps = currentOrder?.GetType.GetProperties

            '計算瓦斯桶庫存
            For Each prop In cusProps.Where(Function(x) x.Name.StartsWith("cus_gas_"))
                Dim barrelType = prop.Name.Substring(8)
                Dim currentStk = prop.GetValue(currentCustomer)
                Dim newStk As Integer

                If isIn Then
                    Dim inQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_in_{barrelType}").GetValue(inputInOut)
                    Dim newInQty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_new_in_{barrelType}").GetValue(inputInOut)
                    Dim orderInQty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_in_{barrelType}").GetValue(currentOrder)
                    Dim ordernewInQty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_new_in_{barrelType}").GetValue(currentOrder)
                    newStk = currentStk + inQty - orderInQty + newInQty - ordernewInQty

                    Dim barrel = Await _gbRep.GetByIdAsync(Await _gbRep.GetIdByKgAsync(barrelType))
                    totalBarrelPrice += newInQty * barrel.gb_SalesPrice
                Else
                    Dim gasC As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_gas_c_{barrelType}").GetValue(inputInOut)
                    Dim gas As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_gas_{barrelType}").GetValue(inputInOut)
                    Dim empty As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_empty_{barrelType}").GetValue(inputInOut)
                    Dim orderGasC As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_gas_c_{barrelType}").GetValue(currentOrder)
                    Dim orderGas As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_gas_{barrelType}").GetValue(currentOrder)
                    Dim orderEmpty As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_empty_{barrelType}").GetValue(currentOrder)

                    newStk = currentStk - gasC + orderGasC - gas + orderGas - empty + orderEmpty
                    totalGas += gas * CInt(barrelType)
                    totalGasC += gasC * CInt(barrelType)
                End If

                prop.SetValue(result, newStk)
            Next

            '計算檢驗桶庫存
            If isIn Then
                For Each prop In cusProps.Where(Function(x) x.Name.StartsWith("cus_inspect_"))
                    Dim barrelType = prop.Name.Substring(12)
                    Dim currentStk = prop.GetValue(currentCustomer)
                    Dim inValue As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_inspect_{barrelType}").GetValue(inputInOut)
                    Dim orderInValue As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_inspect_{barrelType}").GetValue(currentOrder)
                    Dim newStk As Integer = currentStk + inValue - orderInValue
                    prop.SetValue(result, newStk)
                Next
            End If

            _view.DisplayCusStk(result, isIn)

            '計算瓦斯金額
            Dim insurance = If(Not currentCustomer.cus_IsInsurance, (totalGas + totalGasC) * currentCustomer.cus_InsurancePrice, 0)
            Dim basePrice = _bpRep.GetByMonth(inputOrder.o_date)
            Dim cusGasUnitPrice As Single '客戶普氣單價
            Dim cusGasCUnitPrice As Single '客戶丙氣單價

            If inputOrder.o_delivery_type = "自運" Then
                cusGasUnitPrice = basePrice.bp_normal_out + If(currentCustomer.priceplan IsNot Nothing, currentCustomer.priceplan.pp_Gas, 0)
                cusGasCUnitPrice = basePrice.bp_c_out + If(currentCustomer.priceplan IsNot Nothing, currentCustomer.priceplan.pp_Gas_c, 0)
            Else
                cusGasUnitPrice = basePrice.bp_Delivery_Normal + If(currentCustomer.priceplan IsNot Nothing, currentCustomer.priceplan.pp_GasDelivery, 0)
                cusGasCUnitPrice = basePrice.bp_Delivery_C + If(currentCustomer.priceplan IsNot Nothing, currentCustomer.priceplan.pp_GasDelivery_c, 0)
            End If

            Dim gasPrice = cusGasUnitPrice * totalGas '普氣金額
            Dim gasCPrice = cusGasCUnitPrice * totalGasC '丙氣金額

            '計算退氣
            Dim returnGas = cusGasUnitPrice * inputOrder.o_return
            Dim returnGasC = cusGasCUnitPrice * inputOrder.o_return_c

            '計算總計
            Dim amount As Single = totalBarrelPrice + gasPrice + gasCPrice + insurance - inputOrder.o_sales_allowance - returnGas - returnGasC

            _view.DisplayGasAndPrice(totalGas, totalGasC, amount, insurance)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub CalculateCarStk(isIn As Boolean)
        Dim result As New car
        Dim carData = If(isIn, CurrentCarIn, CurrentCarOut)

        If carData Is Nothing Then Return

        Dim input = If(isIn, _view.GetInInput, _view.GetOutInput)
        Dim inputProps = input.GetType.GetProperties
        Dim currentOrderProps = currentOrder?.GetType.GetProperties

        For Each prop In carData.GetType.GetProperties.Where(Function(x) x.Name.StartsWith("c_deposit_"))
            Dim barrelType = prop.Name.Substring(10)
            Dim currentStk = prop.GetValue(carData)
            Dim newStk As Integer

            If isIn Then
                Dim inValue As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_deposit_in_{barrelType}").GetValue(input)
                Dim orderInValue As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_deposit_in_{barrelType}").GetValue(currentOrder)
                newStk = currentStk + inValue - orderInValue
            Else
                Dim outValue As Integer = inputProps.FirstOrDefault(Function(x) x.Name = $"o_deposit_out_{barrelType}").GetValue(input)
                Dim orderOutValue As Integer = currentOrderProps?.FirstOrDefault(Function(x) x.Name = $"o_deposit_out_{barrelType}").GetValue(currentOrder)
                newStk = currentStk - outValue + orderOutValue
            End If

            prop.SetValue(result, newStk)
        Next

        _view.DisplayCarStk(result, isIn)
    End Sub

    Public Async Sub Add()
        Using transaction = _ordRep.BeginTransaction
            Try
                Dim orderInput = _view.GetUserInput()
                Validate(orderInput)
                Await _ordRep.AddAsync(orderInput)

                _view.GetCusStkInput(currentCustomer)

                If orderInput.o_delivery_type = "自運" Then
                    If orderInput.o_in_out = "進場單" Then
                        _view.GetCarStkInput(CurrentCarIn)
                    Else
                        _view.GetCarStkInput(CurrentCarOut)
                    End If
                End If

                Await _cusRep.SaveChangesAsync

                transaction.Commit()
                _view.ClearInput()
                Await LoadList()
                MsgBox("新增成功")
            Catch ex As Exception
                transaction.Rollback()
                Console.WriteLine(ex.StackTrace)
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub LoadDetail(id As Integer)
        Try
            Dim order = Await _ordRep.GetByIdAsync(id)
            _view.ClearInput()

            currentOrder = order
            currentCustomer = order.customer
            CurrentCarIn = order.car
            currentCarOut = order.car1

            Dim isIn = order.o_in_out = "進場單"

            _view.DisplayDetail(currentOrder)
            _view.DisplayCusStk(currentCustomer, isIn)

        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Delete()
        Using transaction = _ordRep.BeginTransaction
            Try
                If currentOrder Is Nothing Then
                    Throw New Exception("沒有選擇要刪除的訂單")
                End If

                ' 更新客戶庫存
                UpdateCustomerStock(currentOrder, currentCustomer)

                ' 更新車輛庫存
                If currentOrder.o_delivery_type = "自運" Then
                    If currentOrder.o_in_out = "進場單" Then
                        UpdateCarStock(currentOrder, CurrentCarIn, True)
                    Else
                        UpdateCarStock(currentOrder, CurrentCarOut, False)
                    End If
                End If

                ' 刪除訂單
                Await _ordRep.DeleteAsync(currentOrder.o_id)

                ' 保存更改
                Await _ordRep.SaveChangesAsync()

                transaction.Commit()
                _view.ClearInput()
                Await LoadList()
                MsgBox("刪除成功")

                ' 重置當前訂單和相關對象
                currentOrder = Nothing
                currentCustomer = Nothing
                CurrentCarIn = Nothing
                CurrentCarOut = Nothing

            Catch ex As Exception
                transaction.Rollback()
                Console.WriteLine(ex.StackTrace)
                MsgBox("刪除失敗：" & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub Validate(input As order)
        If input.o_cus_Id Is Nothing Then Throw New Exception("請輸入客戶")
    End Sub

    Private Async Function LoadCompanyAsync() As Task
        Try
            Dim datas = Await _compRep.GetCompanyDropdownAsync
            _view.SetCompanyDropdown(datas)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub UpdateCustomerStock(order As order, customer As customer)
        Dim orderProps = order.GetType().GetProperties()
        Dim customerProps = customer.GetType().GetProperties()

        If order.o_in_out = "進場單" Then
            UpdateStockDynamic(order, customer, "o_in_", "cus_gas_", -1)
            UpdateStockDynamic(order, customer, "o_new_in_", "cus_gas_", -1)
            UpdateStockDynamic(order, customer, "o_inspect_", "cus_inspect_", -1)
        Else
            UpdateStockDynamic(order, customer, "o_gas_", "cus_gas_", 1)
            UpdateStockDynamic(order, customer, "o_gas_c_", "cus_gas_", 1)
            UpdateStockDynamic(order, customer, "o_empty_", "cus_gas_", 1)
        End If

        customer.cus_GasStock += order.o_return
        customer.cus_GasCStock += order.o_return_c
    End Sub

    Private Sub UpdateCarStock(order As order, car As car, isIn As Boolean)
        Dim prefix As String = If(isIn, "o_deposit_in_", "o_deposit_out_")
        Dim factor As Integer = If(isIn, -1, 1)
        UpdateStockDynamic(order, car, prefix, "c_deposit_", factor)
    End Sub

    Private Sub UpdateStockDynamic(source As Object, target As Object, sourcePrefix As String, targetPrefix As String, factor As Integer)
        Dim sourceProps = source.GetType().GetProperties()
        Dim targetProps = target.GetType().GetProperties()

        For Each sourceProp In sourceProps
            If sourceProp.Name.StartsWith(sourcePrefix) Then
                Dim suffix = sourceProp.Name.Substring(sourcePrefix.Length)
                Dim targetPropName = targetPrefix & suffix
                Dim targetProp = targetProps.FirstOrDefault(Function(p) p.Name = targetPropName)

                If targetProp IsNot Nothing Then
                    Dim sourceValue = CInt(sourceProp.GetValue(source))
                    Dim targetValue = CInt(targetProp.GetValue(target))
                    targetProp.SetValue(target, targetValue + (sourceValue * factor))
                End If
            End If
        Next
    End Sub






    Public Sub Print(orderId As Integer)
        Dim data = _ordRep.GetOrderVoucherData(orderId)
        Dim templatePath = Path.Combine(Application.StartupPath, "Report", "客戶提氣量憑單.html")
        Dim htmlContent = FillTemplate(templatePath, data)
        Dim pdfPath = Path.Combine(Application.StartupPath, "Report", "客戶提氣量憑單.pdf")

        Try
            ' 轉換 HTML 為 PDF
            Using pdfWriter As New PdfWriter(pdfPath)
                Using pdfDocument As New PdfDocument(pdfWriter)
                    Dim fontProvider As New DefaultFontProvider()
                    fontProvider.AddFont("c:/windows/Fonts/MSMINCHO.TTF")
                    fontProvider.AddFont("c:/windows/Fonts/STSONG.TTF")
                    Dim converterProperties As New ConverterProperties()
                    converterProperties.SetFontProvider(fontProvider)

                    ' 將毫米轉換為點
                    Dim widthInPoints As Single = 215 * 2.834645
                    Dim heightInPoints As Single = 140 * 2.834645

                    ' 設置頁面大小
                    pdfDocument.SetDefaultPageSize(New PageSize(widthInPoints, heightInPoints))

                    HtmlConverter.ConvertToPdf(htmlContent, pdfDocument, converterProperties)
                End Using
            End Using

            PrintPDF(pdfPath)

            MsgBox("成功")

        Catch ex As Exception
            MessageBox.Show("轉換失敗：" & ex.Message)
        End Try
    End Sub

    Public Sub PrintPDF(filePath As String)
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
    ''' 取得訂單明細
    ''' </summary>
    ''' <param name="ord"></param>
    ''' <param name="group"></param>
    ''' <param name="isIn"></param>
    ''' <returns></returns>
    Private Function GetOrderValue(ord As order, group As String, isIn As Boolean) As Integer
        If isIn Then
            Return ord.GetType.GetProperties.
                Where(Function(p) (p.Name.StartsWith("o_in_") Or p.Name.StartsWith("o_new_in_") Or p.Name.StartsWith("o_inspect_")) And p.Name.EndsWith(group)).
                Sum(Function(x) x.GetValue(ord))
        Else
            Return ord.GetType.GetProperties.
                Where(Function(p) (p.Name.StartsWith("o_gas_") Or p.Name.StartsWith("o_gas_c_") Or p.Name.StartsWith("o_empty_")) And p.Name.EndsWith(group)).
                Sum(Function(x) x.GetValue(ord))
        End If
    End Function

    ''' <summary>
    ''' 取得寄瓶明細
    ''' </summary>
    ''' <param name="ord"></param>
    ''' <param name="group"></param>
    ''' <param name="isIn"></param>
    ''' <returns></returns>
    Private Function GetDepositValue(ord As order, group As String, isIn As Boolean) As Integer
        If isIn Then
            Return ord.GetType.GetProperties.
                Where(Function(p) p.Name.StartsWith("o_deposit_in_") And p.Name.EndsWith(group)).
                Sum(Function(x) x.GetValue(ord))
        Else
            Return ord.GetType.GetProperties.
                Where(Function(p) p.Name.StartsWith("o_deposit_out_") And p.Name.EndsWith(group)).
                Sum(Function(x) x.GetValue(ord))
        End If
    End Function

    Private Function FillTemplate(templatePath As String, data As OrderVoucherVM) As String
        Dim htmlContent = File.ReadAllText(templatePath)

        htmlContent = htmlContent.Replace("{{客戶名稱}}", data.客戶名稱.ToString)
        htmlContent = htmlContent.Replace("{{車號}}", data.車號.ToString)
        htmlContent = htmlContent.Replace("{{提氣時間}}", data.提氣時間.ToString)
        htmlContent = htmlContent.Replace("{{提單編號}}", data.提單編號.ToString)

        htmlContent = htmlContent.Replace("{{丙氣50kg}}", If(data.丙氣50kg = 0, "", data.丙氣50kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣20kg}}", If(data.丙氣20kg = 0, "", data.丙氣20kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣16kg}}", If(data.丙氣16kg = 0, "", data.丙氣16kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣10kg}}", If(data.丙氣10kg = 0, "", data.丙氣10kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣4kg}}", If(data.丙氣4kg = 0, "", data.丙氣4kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣15kg}}", If(data.丙氣15kg = 0, "", data.丙氣15kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣2kg}}", If(data.丙氣2kg = 0, "", data.丙氣2kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣14kg}}", If(data.丙氣14kg = 0, "", data.丙氣14kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣5kg}}", If(data.丙氣5kg = 0, "", data.丙氣5kg.ToString))
        htmlContent = htmlContent.Replace("{{丙氣kg數}}", If(data.丙氣kg數 = 0, "", data.丙氣kg數.ToString))

        htmlContent = htmlContent.Replace("{{普氣50kg}}", If(data.普氣50kg = 0, "", data.普氣50kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣20kg}}", If(data.普氣20kg = 0, "", data.普氣20kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣16kg}}", If(data.普氣16kg = 0, "", data.普氣16kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣10kg}}", If(data.普氣10kg = 0, "", data.普氣10kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣4kg}}", If(data.普氣4kg = 0, "", data.普氣4kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣15kg}}", If(data.普氣15kg = 0, "", data.普氣15kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣2kg}}", If(data.普氣2kg = 0, "", data.普氣2kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣14kg}}", If(data.普氣14kg = 0, "", data.普氣14kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣5kg}}", If(data.普氣5kg = 0, "", data.普氣5kg.ToString))
        htmlContent = htmlContent.Replace("{{普氣kg數}}", If(data.普氣kg數 = 0, "", data.普氣kg數.ToString))

        htmlContent = htmlContent.Replace("{{檢驗50kg}}", If(data.檢驗50kg = 0, "", data.檢驗50kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗20kg}}", If(data.檢驗20kg = 0, "", data.檢驗20kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗16kg}}", If(data.檢驗16kg = 0, "", data.檢驗16kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗10kg}}", If(data.檢驗10kg = 0, "", data.檢驗10kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗4kg}}", If(data.檢驗4kg = 0, "", data.檢驗4kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗15kg}}", If(data.檢驗15kg = 0, "", data.檢驗15kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗2kg}}", If(data.檢驗2kg = 0, "", data.檢驗2kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗14kg}}", If(data.檢驗14kg = 0, "", data.檢驗14kg.ToString))
        htmlContent = htmlContent.Replace("{{檢驗5kg}}", If(data.檢驗5kg = 0, "", data.檢驗5kg.ToString))

        htmlContent = htmlContent.Replace("{{新瓶50kg}}", If(data.收空瓶50kg = 0, "", data.收空瓶50kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶20kg}}", If(data.收空瓶20kg = 0, "", data.收空瓶20kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶16kg}}", If(data.收空瓶16kg = 0, "", data.收空瓶16kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶10kg}}", If(data.收空瓶10kg = 0, "", data.收空瓶10kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶4kg}}", If(data.收空瓶4kg = 0, "", data.收空瓶4kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶15kg}}", If(data.收空瓶15kg = 0, "", data.收空瓶15kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶2kg}}", If(data.收空瓶2kg = 0, "", data.收空瓶2kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶14kg}}", If(data.收空瓶14kg = 0, "", data.收空瓶14kg.ToString))
        htmlContent = htmlContent.Replace("{{新瓶5kg}}", If(data.收空瓶5kg = 0, "", data.收空瓶5kg.ToString))

        htmlContent = htmlContent.Replace("{{收空瓶50kg}}", If(data.收空瓶50kg = 0, "", data.收空瓶50kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶20kg}}", If(data.收空瓶20kg = 0, "", data.收空瓶20kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶16kg}}", If(data.收空瓶16kg = 0, "", data.收空瓶16kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶10kg}}", If(data.收空瓶10kg = 0, "", data.收空瓶10kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶4kg}}", If(data.收空瓶4kg = 0, "", data.收空瓶4kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶15kg}}", If(data.收空瓶15kg = 0, "", data.收空瓶15kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶2kg}}", If(data.收空瓶2kg = 0, "", data.收空瓶2kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶14kg}}", If(data.收空瓶14kg = 0, "", data.收空瓶14kg.ToString))
        htmlContent = htmlContent.Replace("{{收空瓶5kg}}", If(data.收空瓶5kg = 0, "", data.收空瓶5kg.ToString))

        htmlContent = htmlContent.Replace("{{退空瓶50kg}}", If(data.退空瓶50kg = 0, "", data.退空瓶50kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶20kg}}", If(data.退空瓶20kg = 0, "", data.退空瓶20kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶16kg}}", If(data.退空瓶16kg = 0, "", data.退空瓶16kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶10kg}}", If(data.退空瓶10kg = 0, "", data.退空瓶10kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶4kg}}", If(data.退空瓶4kg = 0, "", data.退空瓶4kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶15kg}}", If(data.退空瓶15kg = 0, "", data.退空瓶15kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶2kg}}", If(data.退空瓶2kg = 0, "", data.退空瓶2kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶14kg}}", If(data.退空瓶14kg = 0, "", data.退空瓶14kg.ToString))
        htmlContent = htmlContent.Replace("{{退空瓶5kg}}", If(data.退空瓶5kg = 0, "", data.退空瓶5kg.ToString))

        htmlContent = htmlContent.Replace("{{結存50kg}}", If(data.結存50kg = 0, "", data.結存50kg.ToString))
        htmlContent = htmlContent.Replace("{{結存20kg}}", If(data.結存20kg = 0, "", data.結存20kg.ToString))
        htmlContent = htmlContent.Replace("{{結存16kg}}", If(data.結存16kg = 0, "", data.結存16kg.ToString))
        htmlContent = htmlContent.Replace("{{結存10kg}}", If(data.結存10kg = 0, "", data.結存10kg.ToString))
        htmlContent = htmlContent.Replace("{{結存4kg}}", If(data.結存4kg = 0, "", data.結存4kg.ToString))
        htmlContent = htmlContent.Replace("{{結存15kg}}", If(data.結存15kg = 0, "", data.結存15kg.ToString))
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
End Class