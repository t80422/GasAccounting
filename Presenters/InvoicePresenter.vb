Public Class InvoicePresenter
    Private ReadOnly _view As IInvoiceView
    Private ReadOnly _cusRep As ICustomerRep
    Private ReadOnly _invoiceRep As IInvoiceRep

    Public Sub New(view As IInvoiceView, cusRep As ICustomerRep, invoiceRep As IInvoiceRep)
        _view = view
        _cusRep = cusRep
        _invoiceRep = invoiceRep
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

    Public Function CalculateAmountAndTax(kg As Integer, unitPrice As Single) As (amount As Single, tax As Single)
        Dim amount = kg * unitPrice
        Dim tax = amount - amount / 1.05
        Return (amount, tax)
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

    Private Sub Validate(data As invoice)
        If data.i_Amount = 0 Then Throw New Exception("請填寫金額")
        If data.i_cus_Id = 0 Then Throw New Exception("請選擇客戶")
        If data.i_KG = 0 Then Throw New Exception("請填寫KG")
        If String.IsNullOrEmpty(data.i_Number) Then Throw New Exception("請填寫發票號碼")
        If data.i_UnitPrice = 0 Then Throw New Exception("請填寫單價")
    End Sub
End Class
