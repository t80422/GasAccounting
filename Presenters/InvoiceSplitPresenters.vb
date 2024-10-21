Public Class InvoiceSplitPresenters
    Private ReadOnly _view As IInvoiceSplitView
    Private ReadOnly _invoiceSplitRep As InvoiceSplitRep
    Private ReadOnly _compRep As CompanyRep
    Private _currentData As invoice_split

    Public Sub New(view As IInvoiceSplitView, invoiceInRep As InvoiceSplitRep, compRep As CompanyRep)
        _view = view
        _invoiceSplitRep = invoiceInRep
        _compRep = compRep
    End Sub

    Public Sub Initialize()
        _view.ClearInput()
        LoadCompanyAsync()
        LoadList()
        _currentData = Nothing
    End Sub

    Public Sub LoadList()
        Try
            Dim criteria = _view.GetSearchCriteria
            Dim data = _invoiceSplitRep.Search(criteria)
            _view.DisplayList(data.Select(Function(x) New InvoiceSplitVM(x)).ToList)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub AddAsync()
        Try
            Dim data = _view.GetUserInput
            Validate(data)
            Await _invoiceSplitRep.AddAsync(data)
            Initialize()
            MsgBox("新增成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub LoadDetailAsync(id As Integer)
        Try
            _currentData = Await _invoiceSplitRep.GetByIdAsync(id)
            _view.DisplayDetail(_currentData)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub UpdateAsync()
        Try
            Dim data = _view.GetUserInput
            Validate(data)
            Await _invoiceSplitRep.UpdateAsync(_currentData, data)
            Initialize()
            MsgBox("修改成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub DeleteAsync()
        Try
            Await _invoiceSplitRep.DeleteAsync(_currentData)
            Initialize()
            MsgBox("刪除成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Validate(data As invoice_split)
        If data.is_Amount = 0 Then Throw New Exception("請輸入金額")
        If String.IsNullOrEmpty(data.is_Name) Then Throw New Exception("請輸入品名")
        If data.is_Type = "進項" AndAlso String.IsNullOrEmpty(data.is_Number) Then Throw New Exception("請輸入發票號碼")
        If data.is_Type = "進項" AndAlso String.IsNullOrEmpty(data.is_VendorTaxId) Then Throw New Exception("請輸入廠商統編")
    End Sub

    Private Async Sub LoadCompanyAsync()
        Try
            _view.SetCompanyCmb(Await _compRep.GetCompanyDropdownAsync)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
