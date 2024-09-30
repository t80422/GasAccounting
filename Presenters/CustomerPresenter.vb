Public Class CustomerPresenter
    Private ReadOnly _view As ICustomerView
    Private ReadOnly _cusRep As ICustomerRep
    Private ReadOnly _pricePlanRep As IPricePlanRep
    Private ReadOnly _compRep As ICompanyRep

    Public Sub New(view As ICustomerView, cusRep As ICustomerRep, pricePlanRep As IPricePlanRep, compRep As ICompanyRep)
        _view = view
        _cusRep = cusRep
        _pricePlanRep = pricePlanRep
        _compRep = compRep
    End Sub

    Public Async Sub InitializeAsync()
        Try
            _view.ClearInput()

            Await Task.WhenAll(
                LoadPricePlanDropdownAsync,
                LoadCompanyAsync
            )

            Await SearchAsync()
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Async Function LoadCompanyAsync() As Task
        Try
            Dim data = Await _compRep.GetCompanyDropdownAsync
            _view.SetCompanyDropdown(data)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Async Function LoadPricePlanDropdownAsync() As Task
        Try
            Dim data = Await _pricePlanRep.GetDropdownAsync
            _view.PopulatePricePlanDropdown(data)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Sub LoadPricePlanDetailsAsync(id As Integer)
        Try
            Dim data = Await _pricePlanRep.GetByIdAsync(id)
            _view.ClearPricePlan()
            _view.SetPricePlanDetails(data)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Function SearchAsync() As Task
        Try
            Dim criteria = _view.GetUserInput
            Dim data = Await _cusRep.SearchAsync(criteria)
            _view.DisplayList(data.Select(Function(x) New CustomerVM(x)).ToList)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Sub AddAsync()
        Try
            Dim data = _view.GetUserInput
            Validate(data)
            Await _cusRep.AddAsync(data)
            _view.ClearInput()
            Await SearchAsync()
            MsgBox("新增成功")
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub UpdateAsync()
        Try
            Dim data = _view.GetUserInput
            Validate(data)
            Await _cusRep.UpdateAsync(data.cus_id, data)
            _view.ClearInput()
            Await SearchAsync()
            MsgBox("修改成功")
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub DeleteAsync(id As Integer)
        Try
            Await _cusRep.DeleteAsync(id)
            _view.ClearInput()
            Await SearchAsync()
            MsgBox("刪除成功")
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub LoadDetailAsync(id As Integer)
        Try
            Dim data = Await _cusRep.GetByIdAsync(id)
            _view.ClearInput()
            _view.DisplayDetail(data)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Validate(data As customer)
        If String.IsNullOrEmpty(data.cus_code) Then Throw New Exception("請填寫代號")
        If String.IsNullOrEmpty(data.cus_name) Then Throw New Exception("請填寫名稱")
        If String.IsNullOrEmpty(data.cus_contact_person) Then Throw New Exception("請填寫聯絡人")
        If String.IsNullOrEmpty(data.cus_phone1) Then Throw New Exception("請填寫電話1")
        If String.IsNullOrEmpty(data.cus_tax_id) Then Throw New Exception("請填寫統編")
    End Sub
End Class
