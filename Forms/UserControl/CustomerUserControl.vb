Public Class CustomerUserControl
    Implements ICustomerView

    Public Event CreateRequest As EventHandler Implements IFormView(Of customer, CustomerVM).CreateRequest
    Public Event DataSelectedRequest As EventHandler(Of Integer) Implements IFormView(Of customer, CustomerVM).DataSelectedRequest
    Public Event UpdateRequest As EventHandler Implements IFormView(Of customer, CustomerVM).UpdateRequest
    Public Event DeleteRequest As EventHandler Implements IFormView(Of customer, CustomerVM).DeleteRequest
    Public Event CancelRequest As EventHandler Implements IFormView(Of customer, CustomerVM).CancelRequest
    Public Event SearchRequest As EventHandler Implements IFormView(Of customer, CustomerVM).SearchRequest
    Public Event PricePlanSelectedChange As EventHandler(Of Integer) Implements ICustomerView.PricePlanSelectedChange

    ' === View ===
    Public Sub SetPricePlanDetails(data As priceplan) Implements ICustomerView.SetPricePlanDetails
        AutoMapEntityToControls(data, grpPricePlan)
    End Sub

    Public Sub PopulatePricePlanDropdown(data As List(Of SelectListItem)) Implements ICustomerView.PopulatePricePlanDropdown
        SetComboBox(cmbPricePlan, data)
    End Sub

    Public Sub ClearPricePlan() Implements ICustomerView.ClearPricePlan
        Dim exception = New List(Of String) From {cmbPricePlan.Name}
        ClearControls(grpPricePlan, exception)
    End Sub

    Public Sub SetCompanyDropdown(data As List(Of SelectListItem)) Implements ICustomerView.SetCompanyDropdown
        SetComboBox(cmbCompany_cus, data)
    End Sub

    Public Sub ShowList(data As List(Of CustomerVM)) Implements IFormView(Of customer, CustomerVM).ShowList
        dgvCustomer.DataSource = data
    End Sub

    Public Sub ShowDetail(data As customer) Implements IFormView(Of customer, CustomerVM).ShowDetail
        AutoMapEntityToControls(data, Me)
        AutoMapEntityToControls(data, grpStock)
        AutoMapEntityToControls(data, grpPricePlan)
        AutoMapEntityToControls(data, grpInsurance)
    End Sub

    Public Sub ClearInput() Implements IFormView(Of customer, CustomerVM).ClearInput
        Dim exception = New List(Of String) From {grpInsurance.Name}
        ClearControls(Me, exception)
    End Sub

    Public Sub ButtonStatus(isSelectedRow As Boolean) Implements IFormView(Of customer, CustomerVM).ButtonStatus
        SetButtonState(Me, isSelectedRow)
    End Sub

    Public Function GetInput(ByRef model As customer) As Boolean Implements IFormView(Of customer, CustomerVM).GetInput
        AutoMapControlsToEntity(model, Me)
        AutoMapControlsToEntity(model, grpStock)
        AutoMapControlsToEntity(model, grpPricePlan)
        AutoMapControlsToEntity(model, grpInsurance)
        Return True
    End Function

    Public Function GetSearchCriteria() As customer Implements ICustomerView.GetSearchCriteria
        Dim criteria As New customer

        Using frm As New frmQueryCustomer
            If frm.ShowDialog = DialogResult.OK Then
                criteria.cus_code = frm.CusCode
            End If
        End Using

        Return criteria
    End Function

    ' === 控制項事件 ===
    Private Sub CustomerUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel.PerformClick()
        ReadDataGridWidth(dgvCustomer)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        RaiseEvent CancelRequest(sender, e)
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        RaiseEvent CreateRequest(sender, e)
    End Sub

    Private Sub dgvCustomer_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCustomer.SelectionChanged, dgvCustomer.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return

        Dim id As Integer = ctrl.SelectedRows(0).Cells(0).Value
        RaiseEvent DataSelectedRequest(sender, id)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        RaiseEvent UpdateRequest(sender, e)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If MessageBox.Show("確定要刪除這筆訂單嗎？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        RaiseEvent DeleteRequest(sender, e)
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        RaiseEvent SearchRequest(sender, e)
    End Sub

    Private Sub cmbPricePlan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPricePlan.SelectedIndexChanged
        Dim cmb As ComboBox = sender

        If cmb.SelectedIndex >= 0 Then
            Dim id As Integer

            If Integer.TryParse(cmb.SelectedValue.ToString, id) Then
                RaiseEvent PricePlanSelectedChange(sender, id)
            End If
        End If
    End Sub

    Private Sub dgvCustomer_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvCustomer.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub
End Class
