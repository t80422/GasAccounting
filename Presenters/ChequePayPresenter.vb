Public Class ChequePayPresenter
    Private ReadOnly _rep As IChequePayRep
    Private _view As IChequePayView

    Public Sub New(rep As IChequePayRep)
        _rep = rep
    End Sub

    Public Sub SetView(view As IChequePayView)
        _view = view
        AddHandler _view.Loaded, AddressOf OnLoaded
        AddHandler _view.SearchClicked, AddressOf OnSearchClicked
        AddHandler _view.CancelClicked, AddressOf OnCancelClicked
        AddHandler _view.RowSelected, AddressOf OnRowSelected
    End Sub

    Private Async Sub OnLoaded(sender As Object, e As EventArgs)
        Await LoadAllAsync()
    End Sub

    Private Async Sub OnCancelClicked(sender As Object, e As EventArgs)
        _view.ClearInput()
        Await LoadAllAsync()
    End Sub

    Private Async Sub OnSearchClicked(sender As Object, e As EventArgs)
        Dim criteria = _view.GetSearchCriteria()
        If criteria IsNot Nothing Then
            Try
                Dim items = Await _rep.Search(criteria)
                _view.DisplayList(ToViewModel(items))
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Async Sub OnRowSelected(sender As Object, id As Integer)
        Try
            Dim entity = Await _rep.GetByIdAsync(id)
            If entity IsNot Nothing Then
                _view.DisplayDetail(entity)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Async Function LoadAllAsync() As Task
        Try
            Dim items = Await _rep.GetAllAsync()
            _view.DisplayList(ToViewModel(items))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Function ToViewModel(items As IEnumerable(Of chque_pay)) As List(Of ChequePayVM)
        Return items.Select(Function(x) New ChequePayVM With {
            .編號 = x.cp_Id,
            .給票日期 = x.cp_Date.Value.ToString("yyyy/MM/dd"),
            .支票號碼 = x.cp_Number,
            .金額 = x.cp_Amount,
            .兌現日期 = If(x.cp_CashingDate.HasValue, x.cp_CashingDate.Value.ToString("yyyy/MM/dd"), Nothing),
            .是否兌現 = If(x.cp_IsCashing, False)
        }).ToList()
    End Function
End Class


