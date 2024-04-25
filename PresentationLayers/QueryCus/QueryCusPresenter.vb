Public Class QueryCusPresenter
    Private _view As IQueryCusView
    Private _repository As ICustomerRepository

    Public Sub New(view As IQueryCusView, repository As ICustomerRepository)
        _view = view
        _repository = repository
    End Sub

    ''' <summary>
    ''' 載入列表
    ''' </summary>
    Public Sub LoadList()
        Dim condition = _view.GetSearchCondition
        Dim custromers = _repository.SearchCustomers(condition)
        _view.ShowList(custromers.Select(Function(x) New QueryCusVM(x)).ToList())
    End Sub

    ''' <summary>
    ''' 載入細節
    ''' </summary>
    Public Sub LoadDetails(id As Integer)
        _view.Reset()
        _view.ShowDetails(_repository.GetCustomerById(id))
    End Sub

    Public Sub Reset()
        _view.Reset()
    End Sub
End Class