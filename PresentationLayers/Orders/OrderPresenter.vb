Imports System.Drawing.Printing
Imports System.IO
Imports iText.Html2pdf
Imports iText.Html2pdf.Resolver.Font
Imports iText.Kernel.Geom
Imports iText.Kernel.Pdf
Imports Path = System.IO.Path

Public Class OrderPresenter
    Private _view As IOrderView
    Private _ordRep As IOrderRepository = New OrderRepository
    Private _cusRep As ICustomerRepository = New CustomerRepository
    Private _carRep As ICarRepository = New CarRepository

    Public Property StockValues As New Dictionary(Of String, Object) '儲存客戶瓦斯桶庫存
    Public Property DepositStockValues As New Dictionary(Of String, Object) '儲存司機寄瓶庫存
    Public Property GasValues As New Dictionary(Of String, Integer) '用於存儲 txtGas_、txtGas_c_ 開頭的 TextBox 的初始值
    Public Property DepositValues As New Dictionary(Of String, Object) '用於存儲TextBox.Tag = o_deposit開頭的初始值

    Public Sub New(view As IOrderView)
        _view = view
    End Sub

    Public Sub LoadList(isQuery As Boolean)
        Dim condition = _view.GetQueryCondition
        Dim list = _ordRep.QueryOrders(condition, isQuery)
        _view.ShowList(list.Select(Function(x) New OrderVM(x)).ToList)
    End Sub

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
    ''' 載入車號選單
    ''' </summary>
    ''' <param name="cusId"></param>
    Public Sub LoadCmbCar(cusId As Integer)
        Dim data = _carRep.GetCmbByCusId(cusId)
        If data IsNot Nothing Then _view.SetCmbCar(data)
    End Sub

    Public Function LoadCar(carId As Integer) As car
        Dim car = _carRep.GetCarById(carId)

        If car IsNot Nothing Then
            DepositStockValues = GetEntityFieldsByPrefix(car, "c_deposit_")
            Return car
        End If

        Return Nothing
    End Function

    Public Sub Add(container As Control)
        Try
            Dim ord As New order
            Dim car As New car
            Dim cus As New customer

            If GetData(ord, car, cus, container) Then
                _ordRep.Add(ord, car, cus)
                _view.Reset()
                LoadList(False)
                MsgBox("新增成功")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub GetDetail(id As Integer)
        Try
            _view.ShowDetails(_ordRep.GetOrderById(id))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Edit(container As Control)
        Dim ord As New order
        Dim car As New car
        Dim cus As New customer

        Try
            If GetData(ord, car, cus, container) Then
                _ordRep.Edit(ord, car, cus)
                _view.Reset()
                LoadList(False)
                MsgBox("修改成功")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Delete(orderId As Integer, orderName As String)
        Dim ord = _ordRep.GetOrderById(orderId)

        If ord Is Nothing Then Exit Sub

        Dim carId = ord.o_c_id
        Dim car = _carRep.GetCarById(carId)
        Dim cusId = car.c_cus_id
        Dim cus = _cusRep.GetCustomerById(cusId)

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

        Try
            _ordRep.Delete(ord.o_id, car, cus)
            _view.Reset()
            LoadList(False)
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

    Private Function GetData(ByRef ord As order, ByRef car As car, ByRef cus As customer, container As Control) As Boolean
        ord = _view.GetUserInput_ord()
        If ord Is Nothing Then Return False

        car = _carRep.GetCarById(ord.o_c_id)
        _view.GetUserInput_car(car, container)
        cus = _cusRep.GetCustomerById(car.c_cus_id)
        _view.GetUserInput_cus(cus, container)

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