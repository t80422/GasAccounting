Public Class SurplusGasUserControl
    Implements ISurplusGasView

    Private _presenter As SurplusGasPresenter

    Public Event Loaded As EventHandler Implements ISurplusGasView.Loaded
    Public Event SearchClicked As EventHandler Implements ISurplusGasView.SearchClicked
    Public Event CancelClicked As EventHandler Implements ISurplusGasView.CancelClicked

    Public Sub DisplayList(data As List(Of SurplusGasListVM)) Implements ISurplusGasView.DisplayList
        dgvPurchaseBarrel.DataSource = data
    End Sub

    Public Function GetSearchCriteria() As Object Implements ISurplusGasView.GetSearchCriteria
        Throw New NotImplementedException()
    End Function

    Private Sub SurplusGasUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _presenter = DependencyContainer.Resolve(Of SurplusGasPresenter)
        _presenter.SetView(Me)
        RaiseEvent Loaded(sender, EventArgs.Empty)
    End Sub
End Class
