Public Class SurplusGasUserControl
    Implements ISurplusGasView

    Private _presenter As SurplusGasPresenter

    Public Event Loaded As EventHandler Implements ISurplusGasView.Loaded
    Public Event SearchClicked As EventHandler Implements ISurplusGasView.SearchClicked
    Public Event CancelClicked As EventHandler Implements ISurplusGasView.CancelClicked
    Public Event AddClicked As EventHandler Implements ISurplusGasView.AddClicked
    Public Event EditClicked As EventHandler Implements ISurplusGasView.EditClicked
    Public Event DeleteClicked As EventHandler Implements ISurplusGasView.DeleteClicked
    Public Event RowSelected As EventHandler(Of Integer) Implements ISurplusGasView.RowSelected
    Public Event PrintClicked As EventHandler Implements ISurplusGasView.PrintClicked

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

    Public Sub DisplayDetail(data As surplus_gas) Implements ISurplusGasView.DisplayDetail
        dtpStart.Value = data.sg_StartDate
        dtpEnd.Value = data.sg_EndDate
        dtpMonth.Value = data.sg_Moth
        txtId.Text = data.sg_Id
        txtPlatform.Text = data.sg_Platform
        txtPlatform_C.Text = data.sg_Platform_C
        txtSlot.Text = data.sg_Slot
        txtSlot_C.Text = data.sg_Slot_C
        txtCar.Text = data.sg_Car
        txtCar_C.Text = data.sg_Car_C
        txtSell.Text = data.sg_Sell
        txtTotal.Text = data.sg_Total
    End Sub

    Public Function GetInput() As surplus_gas Implements ISurplusGasView.GetInput
        Return New surplus_gas With {
            .sg_Car = CInt(If(String.IsNullOrEmpty(txtCar.Text), 0, txtCar.Text)),
            .sg_Car_C = CInt(If(String.IsNullOrEmpty(txtCar_C.Text), 0, txtCar_C.Text)),
            .sg_EndDate = dtpEnd.Value.Date,
            .sg_Moth = New Date(dtpMonth.Value.Year, dtpMonth.Value.Month, 1),
            .sg_StartDate = dtpStart.Value.Date,
            .sg_Platform = CInt(If(String.IsNullOrEmpty(txtPlatform.Text), 0, txtPlatform.Text)),
            .sg_Platform_C = CInt(If(String.IsNullOrEmpty(txtPlatform_C.Text), 0, txtPlatform_C.Text)),
            .sg_Slot = CInt(If(String.IsNullOrEmpty(txtSlot.Text), 0, txtSlot.Text)),
            .sg_Slot_C = CInt(If(String.IsNullOrEmpty(txtSlot_C.Text), 0, txtSlot_C.Text)),
            .sg_Sell = CInt(If(String.IsNullOrEmpty(txtSell.Text), 0, txtSell.Text)),
            .sg_Total = CInt(If(String.IsNullOrEmpty(txtTotal.Text), 0, txtTotal.Text))
        }
    End Function

    Public Sub ClearInput() Implements ISurplusGasView.ClearInput
        ClearControls(Me)
    End Sub

    Public Sub ButtonControl(isCreate As Boolean) Implements ISurplusGasView.ButtonControl
        SetButtonState(btnAdd, isCreate)
    End Sub

    Private Sub Calculate() Handles txtPlatform.KeyUp, txtPlatform_C.KeyUp, txtSlot.KeyUp, txtSlot_C.KeyUp, txtCar.KeyUp, txtCar_C.KeyUp, txtSell.KeyUp
        Dim platform As Integer = 0
        Dim platform_c As Integer = 0
        Dim slot As Integer = 0
        Dim slot_c As Integer = 0
        Dim car As Integer = 0
        Dim car_c As Integer = 0
        Dim sell As Integer = 0

        Integer.TryParse(txtPlatform.Text, platform)
        Integer.TryParse(txtPlatform_C.Text, platform_c)
        Integer.TryParse(txtSlot.Text, slot)
        Integer.TryParse(txtSlot_C.Text, slot_c)
        Integer.TryParse(txtCar.Text, car)
        Integer.TryParse(txtCar_C.Text, car_c)
        Integer.TryParse(txtSell.Text, sell)

        txtTotal.Text = (platform + platform_c + slot + slot_c + car + car_c + sell).ToString()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        RaiseEvent AddClicked(sender, EventArgs.Empty)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        RaiseEvent CancelClicked(sender, EventArgs.Empty)
    End Sub

    Private Sub dgvPurchaseBarrel_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPurchaseBarrel.SelectionChanged, dgvPurchaseBarrel.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return
        Dim id As Integer = ctrl.SelectedRows(0).Cells(0).Value
        RaiseEvent RowSelected(Me, id)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        RaiseEvent EditClicked(sender, EventArgs.Empty)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        RaiseEvent DeleteClicked(sender, EventArgs.Empty)
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        RaiseEvent PrintClicked(sender, EventArgs.Empty)
    End Sub
End Class
