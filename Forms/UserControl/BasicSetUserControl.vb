Public Class BasicSetUserControl
    Implements IBasicSetView

    Private _presenter As BasicSetPresenter

    Public Event Loaded As EventHandler Implements IBasicSetView.Loaded
    Public Event SaveClicked As EventHandler Implements IBasicSetView.SaveClicked

    Public Sub DisplayDetail(data As basic_set) Implements IBasicSetView.DisplayDetail
        txtInitCash.Text = data.bs_Cash
        txtInitGas.Text = data.bs_Gas
        txtId.Text = data.bs_Id
    End Sub

    Public Function GetInput() As basic_set Implements IBasicSetView.GetInput
        If String.IsNullOrEmpty(txtInitCash.Text) Then Throw New Exception("請填寫期初現金")
        If String.IsNullOrEmpty(txtInitGas.Text) Then Throw New Exception("請填寫期初瓦斯")

        Return New basic_set With {
            .bs_Cash = txtInitCash.Text,
            .bs_Gas = txtInitGas.Text,
            .bs_Id = txtId.Text
        }
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        RaiseEvent SaveClicked(Me, EventArgs.Empty)
    End Sub

    Private Sub BasicSetUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _presenter = DependencyContainer.Resolve(Of BasicSetPresenter)()
        _presenter.SetVeiw(Me)
        RaiseEvent Loaded(Me, EventArgs.Empty)
    End Sub
End Class
