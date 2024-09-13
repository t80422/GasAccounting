Public Class QueryCusPresenter
    Private _view As IQueryCusView
    Private _repository As ICustomerRep

    Public Sub New(view As IQueryCusView, repository As ICustomerRep)
        _view = view
        _repository = repository
    End Sub

    ''' <summary>
    ''' 載入列表
    ''' </summary>
    Public Async Sub LoadList()
        Dim condition = _view.GetSearchCondition
        Dim custromers = Await _repository.SearchAsync(condition)
        _view.ShowList(custromers.Select(Function(x) New QueryCusVM(x)).ToList())
    End Sub

    ''' <summary>
    ''' 載入細節
    ''' </summary>
    Public Sub LoadDetails(id As Integer)
        _view.Reset()
        Dim data = _repository.GetCustomerById(id)
        _view.ShowDetails(data)
    End Sub

    Public Sub Reset()
        _view.Reset()
    End Sub
End Class