Imports System.Drawing.Printing
Imports System.IO
Imports iText.Html2pdf
Imports iText.Html2pdf.Resolver.Font
Imports iText.Kernel.Geom
Imports iText.Kernel.Pdf
Imports Path = System.IO.Path

Public Class OrderPresenter
    Private _view As IOrderView
    Private _service As ICusOrdByCarService
    Private _ordRep As IOrderRepository
    Private _cusRep As ICustomerRepository
    Private _carRep As ICarRepository
    Private _bpRep As IBasicPriceRep

    Private gasReturn As New Dictionary(Of String, Integer) '儲存客戶訂單瓦斯退氣量

    Private tempOrd As order
    Public OrgCarStk As car

    Private tempCarStk As car
    Public Property GasBarrel As New Dictionary(Of String, Object) '儲存客戶訂單瓦斯桶數量
    'Public Property GasDepoitBarrel As New Dictionary(Of String, Object) '儲存車訂單寄桶數量
    Public Property StockValues As New Dictionary(Of String, Object) '儲存客戶瓦斯桶庫存
    'Public Property DepositStockValues As New Dictionary(Of String, Object) '儲存司機寄瓶庫存
    'Public Property OrgCarId As Integer '紀錄訂單原始CarId
    Public Property GasValues As New Dictionary(Of String, Integer) '用於存儲 txtGas_、txtGas_c_ 開頭的 TextBox 的初始值
    Public Property DepositValues As New Dictionary(Of String, Object) '用於存儲TextBox.Tag = o_deposit開頭的初始值

    Public Sub New(view As IOrderView, service As ICusOrdByCarService)
        _view = view
        _service = service
        _ordRep = service.OrdRep
        _cusRep = service.CusRep
        _carRep = service.CarRep
        _bpRep = service.BPRep
    End Sub

    'Public Sub LoadList(isQuery As Boolean)
    '    Dim condition = _view.GetQueryCondition
    '    Dim list = _ordRep.QueryOrders(condition, isQuery)
    '    _view.ShowList(list.Select(Function(x) New OrderVM(x)).ToList)
    'End Sub

    ''' <summary>
    ''' 取得客戶庫存
    ''' </summary>
    ''' <param name="cusId"></param>
    Public Sub GetCusStk(cusId As Integer)
        Dim data = _cusRep.GetCustomerById(cusId)

        If data IsNot Nothing Then
            StockValues = GetEntityFieldsByPrefix(data, "cus_gas")
            _view.ShowCusStk(data)
        End If
    End Sub

    ''' <summary>
    ''' 取得車輛寄瓶
    ''' </summary>
    ''' <param name="carId"></param>
    Public Sub GetCarStk(carId As Integer)
        Dim data = _carRep.GetById(carId)
        If data IsNot Nothing Then
            'DepositStockValues = GetEntityFieldsByPrefix(data, "c_deposit_")
            tempCarStk = data
            _view.ShowCarStk(data)
        End If
    End Sub

    ''' <summary>
    ''' 載入車號選單
    ''' </summary>
    ''' <param name="cusId"></param>
    Public Sub LoadCmbCar(cusId As Integer)
        Dim data = _carRep.GetCmbByCusId(cusId)
        If data IsNot Nothing Then _view.SetCmbCar(data)
    End Sub

    'Public Function LoadCar(carId As Integer) As car
    '    Dim car = _carRep.GetCarById(carId)

    '    If car IsNot Nothing Then
    '        DepositStockValues = GetEntityFieldsByPrefix(car, "c_deposit_")
    '        Return car
    '    End If

    '    Return Nothing
    'End Function

    Public Sub Insert(container As Control)
        Try
            Dim ord As New order
            Dim car As New car
            Dim cus As New customer

            If GetData(ord, car, cus, container) Then
                '新增退氣
                cus.cus_GasStock += ord.o_return
                cus.cus_GasCStock += ord.o_return_c

                '=====
                '_ordRep.Add(ord, car, cus)
                _service.Insert(ord, cus, car)
                '=====

                _view.Reset()
                'LoadList(False)
                SearchOrders()
                MsgBox("新增成功")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Public Sub GetDetail(id As Integer)
        Try
            Dim order = _ordRep.GetById(id)

            '保存客戶訂單瓦斯退氣量
            gasReturn("Gas") = order.o_return
            gasReturn("GasC") = order.o_return_c

            GetCusStk(order.car.c_cus_id)
            GetCarStk(order.car.c_id)

            tempOrd = order
            OrgCarStk = order.car

            _view.ShowDetails(order)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Update(container As Control, ordId As Integer)
        Dim ord = _ordRep.GetById(ordId)
        Dim car = ord.car
        Dim cus = ord.car.customer

        Try
            If GetData(ord, car, cus, container) Then
                '更新瓦斯存量
                cus.cus_GasStock += ord.o_return - gasReturn("Gas")
                cus.cus_GasCStock += ord.o_return_c - gasReturn("GasC")

                '_ordRep.Edit(ord, car, cus)
                _service.Update(ord, cus, car, OrgCarStk, tempOrd)

                'If car.c_id <> OrgCarStk.c_id Then
                '    Dim orgCar = _carRep.GetById(OrgCarStk.c_id)

                '    If ord.o_in_out = "進場單" Then
                '        orgCar.c_deposit_50 -= tempOrd.o_deposit_in_50
                '        orgCar.c_deposit_20 -= tempOrd.o_deposit_in_20
                '        orgCar.c_deposit_16 -= tempOrd.o_deposit_in_16
                '        orgCar.c_deposit_10 -= tempOrd.o_deposit_in_10
                '        orgCar.c_deposit_4 -= tempOrd.o_deposit_in_4
                '        orgCar.c_deposit_15 -= tempOrd.o_deposit_in_15
                '        orgCar.c_deposit_14 -= tempOrd.o_deposit_in_14
                '        orgCar.c_deposit_5 -= tempOrd.o_deposit_in_5
                '        orgCar.c_deposit_2 -= tempOrd.o_deposit_in_2
                '    Else
                '        orgCar.c_deposit_50 += tempOrd.o_deposit_out_50
                '        orgCar.c_deposit_20 += tempOrd.o_deposit_out_20
                '        orgCar.c_deposit_16 += tempOrd.o_deposit_out_16
                '        orgCar.c_deposit_10 += tempOrd.o_deposit_out_10
                '        orgCar.c_deposit_4 += tempOrd.o_deposit_out_4
                '        orgCar.c_deposit_15 += tempOrd.o_deposit_out_15
                '        orgCar.c_deposit_14 += tempOrd.o_deposit_out_14
                '        orgCar.c_deposit_5 += tempOrd.o_deposit_out_5
                '        orgCar.c_deposit_2 += tempOrd.o_deposit_out_2
                '    End If
                '    _carRep.Save()
                'End If

                _view.Reset()
                'LoadList(False)
                SearchOrders()
                MsgBox("修改成功")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Delete(orderId As Integer, orderName As String)
        Dim ord = _ordRep.GetById(orderId)

        If ord Is Nothing Then Exit Sub

        Dim carId = ord.o_c_id
        Dim car = _carRep.GetById(carId)
        Dim cusId = car.c_cus_id
        Dim cus = _cusRep.GetById(cusId)

        If orderName = "進場單" Then
            cus.cus_gas_50 -= GetOrderValue(ord, "50", True)
            cus.cus_gas_20 -= GetOrderValue(ord, "20", True)
            cus.cus_gas_16 -= GetOrderValue(ord, "16", True)
            cus.cus_gas_10 -= GetOrderValue(ord, "10", True)
            cus.cus_gas_4 -= GetOrderValue(ord, "4", True)
            cus.cus_gas_15 -= GetOrderValue(ord, "15", True)
            cus.cus_gas_14 -= GetOrderValue(ord, "14", True)
            cus.cus_gas_5 -= GetOrderValue(ord, "5", True)
            cus.cus_gas_2 -= GetOrderValue(ord, "2", True)

            '取得寄瓶庫存
            car.c_deposit_50 -= GetDepositValue(ord, "50", True)
            car.c_deposit_20 -= GetDepositValue(ord, "20", True)
            car.c_deposit_16 -= GetDepositValue(ord, "16", True)
            car.c_deposit_10 -= GetDepositValue(ord, "10", True)
            car.c_deposit_4 -= GetDepositValue(ord, "4", True)
            car.c_deposit_15 -= GetDepositValue(ord, "15", True)
            car.c_deposit_14 -= GetDepositValue(ord, "14", True)
            car.c_deposit_5 -= GetDepositValue(ord, "5", True)
            car.c_deposit_2 -= GetDepositValue(ord, "2", True)
        Else
            cus.cus_gas_50 += GetOrderValue(ord, "50", False)
            cus.cus_gas_20 += GetOrderValue(ord, "20", False)
            cus.cus_gas_16 += GetOrderValue(ord, "16", False)
            cus.cus_gas_10 += GetOrderValue(ord, "10", False)
            cus.cus_gas_4 += GetOrderValue(ord, "4", False)
            cus.cus_gas_15 += GetOrderValue(ord, "15", False)
            cus.cus_gas_14 += GetOrderValue(ord, "14", False)
            cus.cus_gas_5 += GetOrderValue(ord, "5", False)
            cus.cus_gas_2 += GetOrderValue(ord, "2", False)

            '取得寄瓶庫存
            car.c_deposit_50 -= GetDepositValue(ord, "50", False)
            car.c_deposit_20 -= GetDepositValue(ord, "20", False)
            car.c_deposit_16 -= GetDepositValue(ord, "16", False)
            car.c_deposit_10 -= GetDepositValue(ord, "10", False)
            car.c_deposit_4 -= GetDepositValue(ord, "4", False)
            car.c_deposit_15 -= GetDepositValue(ord, "15", False)
            car.c_deposit_14 -= GetDepositValue(ord, "14", False)
            car.c_deposit_5 -= GetDepositValue(ord, "5", False)
            car.c_deposit_2 -= GetDepositValue(ord, "2", False)
        End If

        '更新瓦斯存量
        cus.cus_GasStock -= ord.o_return
        cus.cus_GasCStock -= ord.o_return_c

        Try
            '_ordRep.Delete(ord.o_id, car, cus)
            _service.Delete(ord, car, cus)
            _view.Reset()
            'LoadList(False)
            SearchOrders()
            MsgBox("刪除成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function GetCusDataByCusCode(cusCode As String) As customer
        Return _cusRep.GetCusByCusCode(cusCode)
    End Function

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
    ''' 計算客戶庫存
    ''' </summary>
    ''' <param name="products"></param>
    ''' <param name="orgStk"></param>
    ''' <param name="isIn"></param>
    ''' <returns></returns>
    Public Function CalculateCusStock(products As List(Of TextBox), orgStk As Integer, isIn As Boolean) As Integer
        '計算商品群組與暫存的差
        Dim sum As Integer

        For Each txt In products
            Dim barrelValue As Integer
            Dim txtValue = If(String.IsNullOrEmpty(txt.Text), 0, txt.Text)

            If GasBarrel.TryGetValue(txt.Tag, barrelValue) Then
                If isIn Then
                    '入場單計算
                    sum += txtValue - barrelValue
                Else
                    '出場單計算
                    sum -= txtValue - barrelValue
                End If
            Else
                If isIn Then
                    '入場單計算
                    sum += txtValue
                Else
                    '出場單計算
                    sum -= txtValue
                End If
            End If

            GasBarrel(txt.Tag.ToString) = If(String.IsNullOrEmpty(txt.Text), 0, txt.Text)
        Next

        Return orgStk + sum
    End Function

    'Public Function CalculateCarStk(products As List(Of TextBox), orgStk As Integer, isIn As Boolean) As Integer
    '    Dim sum As Integer

    '    For Each txt In products
    '        Dim barrelValue As Integer
    '        Dim txtValue = If(String.IsNullOrEmpty(txt.Text), 0, txt.Text)

    '        If GasDepoitBarrel.TryGetValue(txt.Tag, barrelValue) Then
    '            If isIn Then
    '                '入場單計算
    '                sum += txtValue - barrelValue
    '            Else
    '                '出場單計算
    '                sum -= txtValue - barrelValue
    '            End If
    '        Else
    '            If isIn Then
    '                '入場單計算
    '                sum += txtValue
    '            Else
    '                '出場單計算
    '                sum -= txtValue
    '            End If
    '        End If
    '    Next

    '    Return orgStk + sum
    'End Function

    Public Function CalculateCarStk(products As List(Of TextBox), group As Integer, isIn As Boolean, Optional firstCount As Boolean = False) As Integer
        Dim sum As Integer

        For Each txt In products
            Dim barrelValue As Integer = 0

            If tempOrd IsNot Nothing And firstCount = False Then
                barrelValue = GetEntityFieldsByPrefix(tempOrd, txt.Tag).First.Value
            End If

            Dim txtValue = If(String.IsNullOrEmpty(txt.Text), 0, txt.Text)

            If isIn Then
                '入場單計算
                sum += txtValue - barrelValue
            Else
                '出場單計算
                sum -= txtValue - barrelValue
            End If
        Next

        Dim orgValue As Integer = 0

        If tempCarStk IsNot Nothing Then
            orgValue = GetEntityFields(tempCarStk, $"c_deposit_{group}").First.Value
        End If

        Return orgValue + sum
    End Function

    Public Function CalculateTotalPrice(gas As Integer, gasC As Integer, cusId As Integer, type As String, month As Date) As Integer
        Dim cus = _cusRep.GetById(cusId)
        Dim unitPrice = GetUnitPrice(cus, type, month)

        Return (unitPrice.丙氣單價) * gasC + (unitPrice.普氣單價 * gas)
    End Function

    ''' <summary>
    ''' 搜尋
    ''' </summary>
    ''' <param name="criteria"></param>
    Public Sub SearchOrders(Optional criteria As OrderSearchCriteria = Nothing)
        Try
            Dim orders = _service.SearchOrders(criteria)
            Dim orderVMs As IEnumerable(Of Object)

            If criteria IsNot Nothing AndAlso Not String.IsNullOrEmpty(criteria.InOut) Then
                If criteria.InOut = "進場單" Then
                    orderVMs = orders.Select(Function(o) New OrderInVM(o)).ToList()
                Else
                    orderVMs = orders.Select(Function(o) New OrderOutVM(o)).ToList()
                End If
            Else
                orderVMs = orders.Select(Function(o) New OrderVM(o)).ToList()
            End If

            _view.ShowOrderList(orderVMs)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 取得單價
    ''' </summary>
    ''' <param name="cus"></param>
    ''' <param name="type"></param>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Private Function GetUnitPrice(cus As customer, type As String, month As Date) As OrderUnitPrice
        '取得基礎價格
        Dim basicUnitPrice = _bpRep.GetByMonth(month).LastOrDefault

        If basicUnitPrice Is Nothing Then
            Throw New Exception("找不到對應月份的基礎價格")
            Return New OrderUnitPrice
        End If

        Dim gasBasicUnitPrice = basicUnitPrice.bp_normal_out
        Dim gasCBasicUnitPrice = basicUnitPrice.bp_c_out

        ' 取得價格方案
        Dim pricePlan = cus.priceplan
        Dim ppGas As Single = 0
        Dim ppGasC As Single = 0

        If pricePlan IsNot Nothing Then
            Select Case type
                Case "廠運"
                    ppGas = pricePlan.pp_GasDelivery
                    ppGasC = pricePlan.pp_GasDelivery_c
                Case "自運"
                    ppGas = pricePlan.pp_Gas
                    ppGasC = pricePlan.pp_Gas_c
            End Select
        End If


        Return New OrderUnitPrice With {
            .丙氣單價 = gasCBasicUnitPrice + ppGasC,
            .普氣單價 = gasBasicUnitPrice + ppGas
        }
    End Function

    Private Function GetData(ByRef ord As order, ByRef car As car, ByRef cus As customer, container As Control) As Boolean
        _view.GetUserInput_ord(ord)
        If ord Is Nothing Then Return False

        car = _carRep.GetById(ord.o_c_id)
        'car = ord.car
        _view.GetUserInput_car(car, container)

        'cus = _cusRep.GetById(car.c_cus_id)
        cus = car.customer
        _view.GetUserInput_cus(cus, container)

        '更新單價
        Dim unitPrice = GetUnitPrice(cus, ord.o_delivery_type, ord.o_date)
        ord.o_UnitPrice = unitPrice.普氣單價
        ord.o_UnitPriceC = unitPrice.丙氣單價

        Return True
    End Function

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