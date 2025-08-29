Public Class SurplusGasPresenter
    Private _view As ISurplusGasView

    Public Sub SetView(view As ISurplusGasView)
        _view = view
        AddHandler _view.Loaded, AddressOf OnLoaded
        AddHandler _view.SearchClicked, AddressOf OnSearchClicked
        AddHandler _view.CancelClicked, AddressOf OnCancelClicked
    End Sub

    Private Sub OnLoaded(sender As Object, e As EventArgs)
        LoadList()
    End Sub

    Private Sub OnSearchClicked(sender As Object, e As EventArgs)
        Dim criteria = _view.GetSearchCriteria
        LoadList(criteria)
    End Sub

    Private Sub OnCancelClicked(sender As Object, e As EventArgs)
        LoadList()
    End Sub

    Private Sub LoadList(Optional criteria As Object = Nothing)
        Dim data
        _view.DisplayList(data)
    End Sub
End Class
