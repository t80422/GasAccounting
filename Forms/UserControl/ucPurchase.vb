' 大氣採購
Public Class ucPurchase
    Implements IPurchaseView

    Public Event GasVenderSelected As EventHandler(Of Object) Implements IPurchaseView.GasVenderSelected
    Public Event PrintClicked As EventHandler(Of Object) Implements IPurchaseView.PrintClicked
    Public Event CreateRequest As EventHandler Implements IFormView(Of purchase, PurchaseVM).CreateRequest
    Public Event DataSelectedRequest As EventHandler(Of Integer) Implements IFormView(Of purchase, PurchaseVM).DataSelectedRequest
    Public Event UpdateRequest As EventHandler Implements IFormView(Of purchase, PurchaseVM).UpdateRequest
    Public Event DeleteRequest As EventHandler Implements IFormView(Of purchase, PurchaseVM).DeleteRequest
    Public Event CancelRequest As EventHandler Implements IFormView(Of purchase, PurchaseVM).CancelRequest
    Public Event SearchRequest As EventHandler Implements IFormView(Of purchase, PurchaseVM).SearchRequest

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

    Public Sub ShowDetail(data As purchase) Implements IFormView(Of purchase, PurchaseVM).ShowDetail
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

    Public Function GetInput(ByRef model As purchase) As Boolean Implements IFormView(Of purchase, PurchaseVM).GetInput
        If cmbCompany_pur.SelectedIndex = -1 Then Throw New Exception("請選擇公司")
        If cmbProduct.SelectedIndex = -1 Then Throw New Exception("請選擇產品")
        If String.IsNullOrEmpty(txtWeight_pur.Text) Then Throw New Exception("請輸入重量")

        AutoMapControlsToEntity(model, Me)

        Return True
    End Function

    Public Sub ButtonStatus(isSelectedRow As Boolean) Implements IFormView(Of purchase, PurchaseVM).ButtonStatus
        SetButtonState(Me, isSelectedRow)
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
        RaiseEvent CancelRequest(sender, EventArgs.Empty)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        RaiseEvent CreateRequest(sender, EventArgs.Empty)
    End Sub

    Private Sub dgvPurchase_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPurchase.SelectionChanged, dgvPurchase.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused OrElse ctrl.SelectedRows.Count = 0 Then Return

        Dim id As Integer = ctrl.SelectedRows(0).Cells("編號").Value
        RaiseEvent DataSelectedRequest(sender, id)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        RaiseEvent UpdateRequest(sender, e)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        RaiseEvent DeleteRequest(sender, e)
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        RaiseEvent SearchRequest(sender, e)
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
        txtFreight.Text = Math.Round(unitPrice * weight)
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
