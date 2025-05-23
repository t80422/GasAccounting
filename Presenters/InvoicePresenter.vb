Imports NLog

''' <summary>
''' 發票管理
''' </summary>
Public Class InvoicePresenter
    Private ReadOnly _view As IInvoiceView
    Private ReadOnly _cusRep As ICustomerRep
    Private ReadOnly _invoiceRep As IInvoiceRep
    Private ReadOnly _priceCalSer As IPriceCalculationService
    Private ReadOnly _orderRep As IOrderRep
    Private Shared ReadOnly logger As Logger = LogManager.GetCurrentClassLogger()

    Public Sub New(view As IInvoiceView, cusRep As ICustomerRep, invoiceRep As IInvoiceRep, priceCalSer As IPriceCalculationService, orderRep As IOrderRep)
        _view = view
        _cusRep = cusRep
        _invoiceRep = invoiceRep
        _priceCalSer = priceCalSer
        _orderRep = orderRep
    End Sub

    Public Sub GetCustomerByCusCode(cusCode As String)
        Try
            Dim data = _cusRep.GetByCusCode(cusCode)

            If data Is Nothing Then
                MsgBox("查無此客戶")
                Return
            End If

            _view.SetCustomer(data)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Function LoadList() As Task
        Try
            Dim criteria = _view.GetSearchCriteria
            Dim datas = Await _invoiceRep.SearchAsync(criteria)
            _view.ClearInput()
            _view.DisplayList(datas.Select(Function(x) New InvioceVM(x)).ToList)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Sub AddAsync()
        Try
            Dim data = _view.GetUserInput
            Validate(data)
            Await _invoiceRep.AddAsync(data)
            _view.ClearInput()
            Await LoadList()
            MsgBox("新增成功")
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub LoadDetail(id As Integer)
        Try
            Dim data = Await _invoiceRep.GetByIdAsync(id)
            _view.ClearInput()
            _view.DisplayDetail(data)
            LoadInvoiceInfo(data.i_cus_Id, data.i_Date)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Update()
        Try
            Dim data = _view.GetUserInput
            Validate(data)
            Await _invoiceRep.UpdateAsync(data.i_Id, data)
            _view.ClearInput()
            Await LoadList()
            MsgBox("修改成功")
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Delete()
        Try
            Dim data = _view.GetUserInput
            Await _invoiceRep.DeleteAsync(data.i_Id)
            _view.ClearInput()
            Await LoadList()
            MsgBox("刪除成功")
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub CalculatePrices(type As String)
        Try
            Dim input = _view.GetUserInput
            If String.IsNullOrEmpty(type) OrElse input.i_cus_Id = 0 Then Return

            Dim cus = Await _cusRep.GetByIdAsync(input.i_cus_Id)
            Dim unitPrice As Single

            If type.Contains("廠運") Then
                If type.Contains("普氣") Then
                    unitPrice = _priceCalSer.CalculateUnitPrice(cus, input.i_Date, True, True)
                Else
                    unitPrice = _priceCalSer.CalculateUnitPrice(cus, input.i_Date, True, False)
                End If
            Else
                If type.Contains("普氣") Then
                    unitPrice = _priceCalSer.CalculateUnitPrice(cus, input.i_Date, False, True)
                Else
                    unitPrice = _priceCalSer.CalculateUnitPrice(cus, input.i_Date, False, False)
                End If
            End If

            Dim amount = unitPrice * input.i_KG
            Dim tax = amount - amount / 1.05

            _view.DisplayPrices(unitPrice, tax.ToString("f2"), amount)
        Catch ex As Exception
            MsgBox("計算價格失敗")
            logger.Error(ex, "計算價格失敗")
        End Try
    End Sub

    Public Sub LoadInvoiceInfo(cusId As Integer, d As Date)
        Try
            If cusId = 0 Then Return

            Dim orders = _orderRep.GetByMonth(d).Where(Function(x) x.o_cus_Id = cusId)
            Dim invoices = _invoiceRep.GetByMonth(d).Where(Function(x) x.i_cus_Id = cusId)
            Dim result As (Integer, Integer)
            result.Item1 = orders.Sum(Function(x) x.o_gas_total)
            result.Item2 = orders.Sum(Function(x) x.o_gas_c_total)

            _view.DisplayInvoiceInfo(result)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function GetInvoiceDefaultNumber(invoiceType As String) As String
        Try
            If Not String.IsNullOrEmpty(invoiceType) Then
                Dim lastNumber = _invoiceRep.GetLastInvoiceNumberByType(invoiceType)

                If Not String.IsNullOrEmpty(lastNumber) Then

                    '將數字部分轉換為整數並加1
                    Return (Integer.Parse(lastNumber) + 1).ToString("D8")
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return Nothing
    End Function

    Private Sub Validate(data As invoice)
        If String.IsNullOrEmpty(data.i_Number) Then Throw New Exception("請填寫發票號碼")

        If data.i_cus_Id <> 0 Then
            If String.IsNullOrEmpty(data.i_Type) Then Throw New Exception("請選擇種類")

            If data.i_KG = 0 Then Throw New Exception("請填寫KG")
        End If
    End Sub
End Class
