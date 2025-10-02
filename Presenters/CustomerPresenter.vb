Imports System.Data.Entity.Core.Mapping

Public Class CustomerPresenter
    Private ReadOnly _view As ICustomerView
    Private ReadOnly _cusRep As ICustomerRep
    Private ReadOnly _pricePlanRep As IPricePlanRep
    Private ReadOnly _compRep As ICompanyRep

    Private currentData As customer

    Public ReadOnly Property View As ICustomerView
        Get
            Return _view
        End Get
    End Property

    Public Sub New(view As ICustomerView, cusRep As ICustomerRep, pricePlanRep As IPricePlanRep, compRep As ICompanyRep)
        _view = view
        _cusRep = cusRep
        _pricePlanRep = pricePlanRep
        _compRep = compRep

        SubscribeToViewEvents()
    End Sub

    ''' <summary>
    ''' 訂閱 View 事件
    ''' </summary>
    Private Sub SubscribeToViewEvents()
        AddHandler _view.CancelRequest, AddressOf InitializeAsync
        AddHandler _view.PricePlanSelectedChange, AddressOf LoadPricePlanDetailsAsync
        AddHandler _view.CreateRequest, AddressOf AddAsync
        AddHandler _view.DataSelectedRequest, AddressOf LoadDetail
        AddHandler _view.UpdateRequest, AddressOf UpdateAsync
        AddHandler _view.DeleteRequest, AddressOf DeleteAsync
        AddHandler _view.SearchRequest, AddressOf Search
    End Sub

    Private Async Sub InitializeAsync()
        Try
            _view.ClearInput()

            Await Task.WhenAll(
                LoadPricePlanDropdownAsync,
                LoadCompanyAsync
            )

            LoadList()
            _view.ButtonStatus(False)
            currentData = Nothing
        Catch ex As Exception
            MessageBox.Show(ex.Message)
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

    Private Sub LoadList(Optional criteria As customer = Nothing)
        Try
            Dim data = _cusRep.SearchAsync(criteria).Result
            _view.ClearInput()
            _view.ShowList(data.Select(Function(x) New CustomerVM(x)).ToList)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Async Sub LoadPricePlanDetailsAsync(sender As Object, id As Integer)
        Try
            Dim data = Await _pricePlanRep.GetByIdAsync(id)
            _view.ClearPricePlan()
            _view.SetPricePlanDetails(data)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Async Sub AddAsync()
        Try
            Dim data As New customer
            _view.GetInput(data)
            Validate(data)
            Await _cusRep.AddAsync(data)
            MessageBox.Show("新增成功")
            InitializeAsync()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub LoadDetail(sender As Object, id As Integer)
        Try
            currentData = _cusRep.GetByIdAsync(id).Result
            _view.ClearInput()
            _view.ShowDetail(currentData)
            _view.ButtonStatus(True)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Async Sub UpdateAsync()
        Try
            _view.GetInput(currentData)
            Validate(currentData)
            Await _cusRep.SaveChangesAsync
            MessageBox.Show("修改成功")
            InitializeAsync()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Async Sub DeleteAsync()
        Try
            Await _cusRep.DeleteAsync(currentData)
            Await _cusRep.SaveChangesAsync
            MessageBox.Show("刪除成功")
            InitializeAsync()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Search()
        Try
            Dim criteria = _view.GetSearchCriteria
            LoadList(criteria)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
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
