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
    Private ReadOnly _ordRep As IOrderRep
    Private ReadOnly _service As IBarrelMonthlyBalanceService
    Private ReadOnly _priceCalSer As IPriceCalculationService
    Private ReadOnly _aeSer As IAccountingEntryService
    Private ReadOnly _printerSer As IPrinterService
    Private ReadOnly _ocmSer As IOrderCollectionMappingService

    Private currentCustomer As customer
    Private currentOrder As order

    Public CurrentCarIn As car
    Public CurrentCarOut As car

    Public Sub New(view As IOrderView, cusRep As ICustomerRep, carRep As ICarRep, ordRep As IOrderRep, gbRep As IGasBarrelRep, barMBService As IBarrelMonthlyBalanceService,
                   priceCalSer As IPriceCalculationService, aeSer As IAccountingEntryService, printerSer As IPrinterService, ocmSer As IOrderCollectionMappingService)
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
    End Sub

    Public Async Function InitializeAsync() As Task
        Try
            _view.ClearInput()
            Await LoadList(False)
            currentOrder = Nothing
            currentCustomer = Nothing
            CurrentCarIn = Nothing
            CurrentCarOut = Nothing
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Public Async Function LoadList(isSearch As Boolean) As Task
        Try
            Dim criteria = If(isSearch,
                _view.GetSearchCriteria(),
                New OrderSearchCriteria With {
                    .SearchIn = True,
                    .SearchOut = True,
                    .IsDate = True,
                    .StartDate = Today,
                    .EndDate = Today
                })

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
            CurrentCarOut = Await _carRep.GetByIdAsync(carId)
            _view.DisplayCarStk(CurrentCarOut, False)
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
            Dim cusGasUnitPrice As Single '客戶普氣單價
            Dim cusGasCUnitPrice As Single '客戶丙氣單價

            If inputOrder.o_delivery_type = "自運" Then
                cusGasUnitPrice = _priceCalSer.CalculateUnitPrice(currentCustomer, inputOrder.o_date, False, True)
                cusGasCUnitPrice = _priceCalSer.CalculateUnitPrice(currentCustomer, inputOrder.o_date, False, False)
            Else
                cusGasUnitPrice = _priceCalSer.CalculateUnitPrice(currentCustomer, inputOrder.o_date, True, True)
                cusGasCUnitPrice = _priceCalSer.CalculateUnitPrice(currentCustomer, inputOrder.o_date, True, False)
            End If

            Dim gasPrice = cusGasUnitPrice * totalGas '普氣金額
            Dim gasCPrice = cusGasCUnitPrice * totalGasC '丙氣金額

            '計算退氣
            Dim returnGas = cusGasUnitPrice * inputOrder.o_return
            Dim returnGasC = cusGasCUnitPrice * inputOrder.o_return_c

            '計算總計
            Dim amount As Single = totalBarrelPrice + gasPrice + gasCPrice + insurance - inputOrder.o_sales_allowance - returnGas - returnGasC

            ' 未收款金額
            Dim paid As Integer = 0

            If currentOrder IsNot Nothing Then
                paid = _ocmSer.CalculateOrderUnPaid(currentOrder.o_id)
            End If

            Dim unpaid = amount - paid
            _view.DisplayGasAndPrice(totalGas, totalGasC, amount, insurance, totalBarrelPrice, cusGasUnitPrice, cusGasCUnitPrice, unpaid)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
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

    Public Async Function Add() As Task(Of Integer)
        Using transaction = _ordRep.BeginTransaction
            Try
                Dim orderInput = _view.GetUserInput()
                Validate(orderInput)
                Dim insert = Await _ordRep.AddAsync(orderInput)

                _view.GetCusStkInput(currentCustomer)

                If orderInput.o_delivery_type = "自運" Then
                    If orderInput.o_in_out = "進場單" Then
                        _view.GetCarStkInput(CurrentCarIn)
                    Else
                        _view.GetCarStkInput(CurrentCarOut)
                    End If
                End If

                Await _service.UpdateOrAddAsync(orderInput.o_date)

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

                Await _cusRep.SaveChangesAsync

                transaction.Commit()
                _view.ClearInput()
                Await LoadList(False)
                MsgBox("新增成功")

                Return insert.o_id
            Catch ex As Exception
                transaction.Rollback()
                Console.WriteLine(ex.StackTrace)

                Dim innerEx = ex
                While innerEx.InnerException IsNot Nothing
                    innerEx = innerEx.InnerException
                End While

                MsgBox(innerEx.Message)
                Return 0
            End Try
        End Using
    End Function

    Public Async Sub LoadDetail(id As Integer)
        Try
            Dim order = Await _ordRep.GetByIdAsync(id)
            _view.ClearInput()

            currentOrder = order
            currentCustomer = order.customer
            CurrentCarIn = order.car
            CurrentCarOut = order.car1

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

                ' 銷帳
                _ocmSer.DeleteOrder(currentOrder.o_id)

                ' 刪除訂單
                Await _ordRep.DeleteAsync(currentOrder.o_id)

                Await _service.UpdateOrAddAsync(currentOrder.o_date)

                _aeSer.DeleteEntries("銷售管理", currentOrder.o_id)

                ' 保存更改
                Await _ordRep.SaveChangesAsync()

                transaction.Commit()
                _view.ClearInput()
                Await LoadList(False)
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
        If input.o_delivery_type = "自運" AndAlso input.o_c_id Is Nothing Then Throw New Exception("請選擇車號")
    End Sub

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

    Public Async Sub Update()
        Using transaction = _ordRep.BeginTransaction
            Try
                Dim orderInput = _view.GetUserInput()
                Validate(orderInput)
                Await _ordRep.UpdateAsync(currentOrder, orderInput)

                _view.GetCusStkInput(currentCustomer)

                If orderInput.o_delivery_type = "自運" Then
                    If orderInput.o_in_out = "進場單" Then
                        _view.GetCarStkInput(CurrentCarIn)
                    Else
                        _view.GetCarStkInput(CurrentCarOut)
                    End If
                End If

                Await _service.UpdateOrAddAsync(orderInput.o_date)

                Dim entries = New List(Of accounting_entry) From {
                    New accounting_entry With {
                        .ae_TransactionId = orderInput.o_id,
                        .ae_Date = orderInput.o_date,
                        .ae_TransactionType = "銷售管理",
                        .ae_s_Id = 8,
                        .ae_Debit = 0,
                        .ae_Credit = orderInput.o_total_amount
                    },
                    New accounting_entry With {
                        .ae_TransactionId = orderInput.o_id,
                        .ae_Date = orderInput.o_date,
                        .ae_TransactionType = "銷售管理",
                        .ae_s_Id = 9,
                        .ae_Debit = orderInput.o_total_amount,
                        .ae_Credit = 0
                    }
                }

                _aeSer.UpdateEntries(entries)

                Await _cusRep.SaveChangesAsync

                transaction.Commit()
                _view.ClearInput()
                Await LoadList(False)
                MsgBox("修改成功")
            Catch ex As Exception
                transaction.Rollback()
                Console.WriteLine(ex.StackTrace)
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub PrintCusStk(isYesterday As Boolean)
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
                        .WriteToCell(rowIndex, 3, cus.cus_gas_50)
                        .WriteToCell(rowIndex, 4, cus.cus_gas_20)
                        .WriteToCell(rowIndex, 5, cus.cus_gas_16)
                        .WriteToCell(rowIndex, 6, cus.cus_gas_10)
                        .WriteToCell(rowIndex, 7, cus.cus_gas_4)
                        .WriteToCell(rowIndex, 8, cus.cus_gas_18)
                        .WriteToCell(rowIndex, 9, cus.cus_gas_14)
                        .WriteToCell(rowIndex, 10, cus.cus_gas_5)
                        .WriteToCell(rowIndex, 11, cus.cus_gas_2)
                        .WriteToCell(rowIndex, 12, totalBarrel)
                        .InsertRow(rowIndex)
                        rowIndex += 1
                        totalSum += totalBarrel
                    Next

                    Dim totalStyle = New CloseXML_Excel.CellFormatOptions With {
                        .Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center
                    }

                    .SetCustomBorders(rowIndex, 1, rowIndex, 12, topStyle:=ClosedXML.Excel.XLBorderStyleValues.Thin)
                    .WriteToCell(rowIndex, 2, "合計", totalStyle)
                    .WriteToCell(rowIndex, 3, customers.Sum(Function(x) x.cus_gas_50))
                    .WriteToCell(rowIndex, 4, customers.Sum(Function(x) x.cus_gas_20))
                    .WriteToCell(rowIndex, 5, customers.Sum(Function(x) x.cus_gas_16))
                    .WriteToCell(rowIndex, 6, customers.Sum(Function(x) x.cus_gas_10))
                    .WriteToCell(rowIndex, 7, customers.Sum(Function(x) x.cus_gas_4))
                    .WriteToCell(rowIndex, 8, customers.Sum(Function(x) x.cus_gas_18))
                    .WriteToCell(rowIndex, 9, customers.Sum(Function(x) x.cus_gas_14))
                    .WriteToCell(rowIndex, 10, customers.Sum(Function(x) x.cus_gas_5))
                    .WriteToCell(rowIndex, 11, customers.Sum(Function(x) x.cus_gas_2))
                    .WriteToCell(rowIndex, 12, totalSum)

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
End Class