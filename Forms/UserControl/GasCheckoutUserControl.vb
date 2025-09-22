Public Class GasCheckoutUserControl
    Implements IGasCheckoutView

    Public Event QueryClicked() Implements IGasCheckoutView.QueryClicked
    Public Event CheckoutClicked() Implements IGasCheckoutView.CheckoutClicked
    Public Event CancelClicked() Implements IGasCheckoutView.CancelClicked

    ' 介面
    Public Sub ShowList(datas As List(Of PurchaseVM)) Implements IGasCheckoutView.ShowList
        dgvGasCheckout.DataSource = datas
    End Sub

    Public Sub ClearInput() Implements IGasCheckoutView.ClearInput
        ClearControls(Me)
    End Sub

    Public Sub LoadVendors(datas As List(Of SelectListItem)) Implements IGasCheckoutView.LoadVendors
        SetComboBox(cmbVendor, datas)
    End Sub

    Public Function GetUserInput() As PurchaseCondition Implements IGasCheckoutView.GetUserInput
        If chkDate.Checked AndAlso dtpStart.Value.Date > dtpEnd.Value.Date Then
            Throw New Exception("日期起不能大於日期迄")
        End If

        Return New PurchaseCondition With {
            .StartDate = dtpStart.Value.Date,
            .EndDate = dtpEnd.Value.Date,
            .ManufacturerId = If(cmbVendor.SelectedValue IsNot Nothing, CInt(cmbVendor.SelectedValue), 0),
            .IsDateSearch = chkDate.Checked
        }
    End Function

    Public Function GetSelectedIds() As List(Of Integer) Implements IGasCheckoutView.GetSelectedIds
        Return dgvGasCheckout.SelectedRows.Cast(Of DataGridViewRow).Select(Function(x) CInt(x.Cells("編號").Value)).ToList
    End Function

    ' 事件
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        RaiseEvent QueryClicked()
    End Sub

    Private Sub btnCheckout_Click(sender As Object, e As EventArgs) Handles btnCheckout.Click
        RaiseEvent CheckoutClicked()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        RaiseEvent CancelClicked()
    End Sub

    Private Sub GasCheckoutUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel.PerformClick()
    End Sub
End Class
