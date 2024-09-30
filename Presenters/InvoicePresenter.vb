Public Class InvoicePresenter
    Private ReadOnly _view As IInvoiceView
    Private ReadOnly _cusRep As ICustomerRep
    Private ReadOnly _invoiceRep As IInvoiceRep
    Private ReadOnly _priceCalSer As IPriceCalculationService
    Private ReadOnly _orderRep As IOrderRep

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
                    unitPrice = _priceCalSer.CalculateUnitPrice(cus, input.i_Month, True, True)
                Else
                    unitPrice = _priceCalSer.CalculateUnitPrice(cus, input.i_Month, True, False)
                End If
            Else
                If type.Contains("普氣") Then
                    unitPrice = _priceCalSer.CalculateUnitPrice(cus, input.i_Month, False, True)
                Else
                    unitPrice = _priceCalSer.CalculateUnitPrice(cus, input.i_Month, False, False)
                End If
            End If

            Dim amount = unitPrice * input.i_KG
            Dim tax = amount - amount / 1.05

            _view.DisplayPrices(unitPrice, tax.ToString("f2"), amount)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub LoadInvoiceInfo(cusId As Integer, d As Date)
        Try
            If cusId = 0 Then Return

            Dim orders = _orderRep.GetByMonth(d).Where(Function(x) x.o_cus_Id = cusId)
            Dim invoices = _invoiceRep.GetByMonth(d).Where(Function(x) x.i_cus_Id = cusId)
            Dim result = New InvoiceInfoVM With {
                .DeliNormTotal = orders.Where(Function(x) x.o_delivery_type = "廠運").Sum(Function(x) x.o_gas_total),
                .DeliCTotal = orders.Where(Function(x) x.o_delivery_type = "廠運").Sum(Function(x) x.o_gas_c_total),
                .PickNormTotal = orders.Where(Function(x) x.o_delivery_type = "自運").Sum(Function(x) x.o_gas_total),
                .PickCTotal = orders.Where(Function(x) x.o_delivery_type = "自運").Sum(Function(x) x.o_gas_c_total),
                .DeliNormInvoice = invoices.Where(Function(x) x.i_Type = "廠運普氣").Sum(Function(x) x.i_KG),
                .DeliCInvoice = invoices.Where(Function(x) x.i_Type = "廠運丙氣").Sum(Function(x) x.i_KG),
                .PickNormInvoice = invoices.Where(Function(x) x.i_Type = "廠運普氣").Sum(Function(x) x.i_KG),
                .PickCInvoice = invoices.Where(Function(x) x.i_Type = "廠運丙氣").Sum(Function(x) x.i_KG)
            }

            _view.DisplayInvoiceInfo(result)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Validate(data As invoice)
        If data.i_cus_Id = 0 Then Throw New Exception("請選擇客戶")
        If String.IsNullOrEmpty(data.i_Type) Then Throw New Exception("請選擇種類")
        If data.i_KG = 0 Then Throw New Exception("請填寫KG")
        If String.IsNullOrEmpty(data.i_Number) Then Throw New Exception("請填寫發票號碼")
    End Sub
End Class
