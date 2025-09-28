' 大氣採購
Imports System.Runtime.InteropServices.ComTypes

Public Class GasPurchaseUserControl
    Implements IPurchaseView

    Public Event AddClicked As EventHandler Implements IPurchaseView.AddClicked
    Public Event EditClicked As EventHandler Implements IPurchaseView.EditClicked
    Public Event DeleteClicked As EventHandler Implements IPurchaseView.DeleteClicked
    Public Event CancelClicked As EventHandler Implements IPurchaseView.CancelClicked
    Public Event SerchClicked As EventHandler Implements IPurchaseView.SerchClicked
    Public Event GasVenderSelected As EventHandler(Of Object) Implements IPurchaseView.GasVenderSelected
    Public Event PrintClicked As EventHandler(Of Object) Implements IPurchaseView.PrintClicked
    Public Event RowSelected As EventHandler(Of Integer) Implements IPurchaseView.RowSelected

    ' 介面
    Public Sub SetCompanyCmb(items As List(Of SelectListItem)) Implements IPurchaseView.SetCompanyCmb
        SetComboBox(cmbCompany_pur, items)
    End Sub

    Public Sub SetGasVendorCmb(items As List(Of SelectListItem)) Implements IPurchaseView.SetGasVendorCmb
        SetComboBox(cmbGasVendor, items)
    End Sub

    Public Sub SetDriveVendorCmb(items As List(Of SelectListItem)) Implements IPurchaseView.SetDriveVendorCmb
        SetComboBox(cmbDriveCmp, items)

        ' 預設選取「萬和汽車貨運」
        Dim index = items.FindIndex(Function(x) x.Display.Contains("萬和汽車貨運"))
        If cmbDriveCmp.Items.Count > 0 AndAlso index >= 0 Then
            cmbDriveCmp.SelectedIndex = index
        End If
    End Sub

    Public Sub SetDefaultPrice(unitPrice As Single, DeliveryUnitPrice As Single) Implements IPurchaseView.SetDefaultPrice
        txtUnitPrice_pur.Text = unitPrice
        txtDeliUnitPrice.Text = DeliveryUnitPrice
    End Sub

    Public Sub ShowList(datas As List(Of PurchaseVM)) Implements IPurchaseView.ShowList
        dgvPurchase.DataSource = datas
    End Sub

    Public Sub ClearInput() Implements IPurchaseView.ClearInput
        ClearControls(Me)
    End Sub

    Public Sub SetDataToControls(data As purchase) Implements IPurchaseView.SetDataToControls
        AutoMapEntityToControls(data, Me)
    End Sub

    Public Function GetSearchCondition() As PurchaseCondition Implements IPurchaseView.GetSearchCondition
        Using frm = DependencyContainer.Resolve(Of Search_GasPurchase)()
            If frm.ShowDialog() = DialogResult.OK Then
                Return frm.Criteria
            End If
        End Using

        Return Nothing
    End Function

    Public Function GetInput() As purchase Implements IPurchaseView.GetInput
        If cmbCompany_pur.SelectedIndex = -1 Then Throw New Exception("請選擇公司")
        If cmbProduct.SelectedIndex = -1 Then Throw New Exception("請選擇產品")
        If cmbGasVendor.SelectedIndex = -1 Then Throw New Exception("請選擇大氣廠商")
        If String.IsNullOrEmpty(txtWeight_pur.Text) Then Throw New Exception("請輸入重量")

        Dim data As New purchase With {
            .pur_id = If(String.IsNullOrEmpty(txtId_pur.Text), 0, txtId_pur.Text),
            .pur_comp_id = cmbCompany_pur.SelectedValue,
            .pur_date = dtpDate_pur.Value.Date,
            .pur_delivery_fee = If(String.IsNullOrEmpty(txtFreight.Text), 0, Double.Parse(txtFreight.Text)),
            .pur_deli_unit_price = If(String.IsNullOrEmpty(txtDeliUnitPrice.Text), 0, Double.Parse(txtDeliUnitPrice.Text)),
            .pur_DriveCmpId = cmbDriveCmp.SelectedValue,
            .pur_manu_id = cmbGasVendor.SelectedValue,
            .pur_Memo = txtMemo_pur.Text,
            .pur_price = If(String.IsNullOrEmpty(txtSum_pur.Text), 0, Double.Parse(txtSum_pur.Text)),
            .pur_product = cmbProduct.SelectedItem,
            .pur_quantity = txtWeight_pur.Text,
            .pur_SpecialDUP = chkSp.Checked,
            .pur_SpecialUP = chkSpecial.Checked,
            .pur_unit_price = If(String.IsNullOrEmpty(txtUnitPrice_pur.Text), 0, Double.Parse(txtUnitPrice_pur.Text))
        }

        Return data
    End Function

    Public Sub SetButton(isSelectRow As Object) Implements IPurchaseView.SetButton
        SetButtonState(Me, isSelectRow)
    End Sub

    Public Sub ShowGasUnpaidSummary(datas As List(Of PurchaseGasVendorTradeSummaryListVM)) Implements IPurchaseView.ShowGasUnpaidSummary
        dgvGasUnpaid.DataSource = datas
        ReadDataGridWidth(dgvGasUnpaid)
    End Sub

    Public Sub ShowTransportationSummary(datas As List(Of PurchaseFreightTradeSummaryListVM)) Implements IPurchaseView.ShowTransportationSummary
        dgvFreightUnpaid.DataSource = datas
        ReadDataGridWidth(dgvFreightUnpaid)
    End Sub

    ' 事件
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        RaiseEvent CancelClicked(sender, EventArgs.Empty)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        RaiseEvent AddClicked(sender, EventArgs.Empty)
    End Sub

    Private Sub dgvPurchase_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPurchase.SelectionChanged, dgvPurchase.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused OrElse ctrl.SelectedRows.Count = 0 Then Return

        Dim id As Integer = ctrl.SelectedRows(0).Cells("編號").Value
        RaiseEvent RowSelected(sender, id)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        RaiseEvent EditClicked(sender, e)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        RaiseEvent DeleteClicked(sender, e)
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        RaiseEvent SerchClicked(sender, e)
    End Sub

    Private Sub cmbGasVendor_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbGasVendor.SelectionChangeCommitted, cmbProduct.SelectionChangeCommitted
        If cmbGasVendor.SelectedIndex > -1 AndAlso cmbProduct.SelectedIndex > -1 Then
            RaiseEvent GasVenderSelected(sender, Tuple.Create(cmbGasVendor.SelectedValue, cmbProduct.SelectedItem))
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        RaiseEvent PrintClicked(sender, dgvPurchase.DataSource)
    End Sub

    Private Sub GasPurchaseUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel.PerformClick()
        ReadDataGridWidth(dgvPurchase)
    End Sub

    Private Sub dgv_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvPurchase.ColumnWidthChanged, dgvGasUnpaid.ColumnWidthChanged, dgvFreightUnpaid.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub

    ' 方法
    ''' <summary>
    ''' 計算運費
    ''' </summary>
    Private Sub CalculateFreight() Handles txtWeight_pur.TextChanged, txtDeliUnitPrice.TextChanged
        Dim unitPrice As Single = If(String.IsNullOrEmpty(txtDeliUnitPrice.Text), 0, txtDeliUnitPrice.Text)
        Dim weight As Integer = If(String.IsNullOrEmpty(txtWeight_pur.Text), 0, txtWeight_pur.Text)
        txtFreight.Text = unitPrice * weight
    End Sub

    ''' <summary>
    ''' 計算大氣採購總金額
    ''' </summary>
    Private Sub CalculateSum_Purchase() Handles txtWeight_pur.TextChanged, txtUnitPrice_pur.TextChanged
        Dim weight As Integer = If(String.IsNullOrEmpty(txtWeight_pur.Text), 0, txtWeight_pur.Text)
        Dim unitPrice As Double = If(String.IsNullOrEmpty(txtUnitPrice_pur.Text), 0, txtUnitPrice_pur.Text)
        txtSum_pur.Text = Math.Round(weight * unitPrice, 0)
    End Sub
End Class
