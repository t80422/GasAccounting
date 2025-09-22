' 付款作業
Public Class PaymentUserControl
    Implements IPaymentView

    Public Event AddRequested As EventHandler Implements IPaymentView.AddRequested
    Public Event UpdateRequested As EventHandler Implements IPaymentView.UpdateRequested
    Public Event DeleteRequested As EventHandler Implements IPaymentView.DeleteRequested
    Public Event CancelRequested As EventHandler Implements IPaymentView.CancelRequested
    Public Event DetailRequested As EventHandler(Of Integer) Implements IPaymentView.DetailRequested
    Public Event PrintRequested As EventHandler(Of Tuple(Of Date, String)) Implements IPaymentView.PrintRequested
    Public Event ManufacturerSelected As EventHandler(Of Integer) Implements IPaymentView.ManufacturerSelected
    Public Event CompanySelected As EventHandler(Of Integer) Implements IPaymentView.CompanySelected

    ' 互動
    Public Sub PopulateVendorDropdown(data As IReadOnlyList(Of SelectListItem)) Implements IPaymentView.PopulateVendorDropdown
        SetComboBox(cmbManu_payment, data)
    End Sub

    Public Sub PopulateSubjectDropdown(data As IReadOnlyList(Of SelectListItem)) Implements IPaymentView.PopulateSubjectDropdown
        SetComboBox(cmbSubjects_payment, data)
    End Sub

    Public Sub PopulateCompanyDropdown(data As IReadOnlyList(Of SelectListItem)) Implements IPaymentView.PopulateCompanyDropdown
        SetComboBox(cmbCompany_payment, data)
    End Sub

    Public Sub PopulateBankDropdown(data As IReadOnlyList(Of SelectListItem)) Implements IPaymentView.PopulateBankDropdown
        SetComboBox(cmbBank, data)
    End Sub

    Public Sub DisplayAmountDueList(data As IReadOnlyList(Of AmountDueVM)) Implements IPaymentView.DisplayAmountDueList
        dgvAmountDue.DataSource = data
    End Sub

    Public Sub ShowVendorAccount(data As String) Implements IPaymentView.ShowVendorAccount
        txtVendorAccount_payment.Text = data
    End Sub

    Public Sub DisplayList(data As List(Of PaymentListVM)) Implements IBaseView(Of payment, PaymentListVM).DisplayList
        dgvPayment.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As payment) Implements IBaseView(Of payment, PaymentListVM).DisplayDetail
        AutoMapEntityToControls(data, Me)
    End Sub

    Public Sub ClearInput() Implements IBaseView(Of payment, PaymentListVM).ClearInput
        ClearControls(Me)
        dgvAmountDue.DataSource = Nothing
    End Sub

    Public Function GetSearchCriteria() As PaymentSearchCriteria Implements IPaymentView.GetSearchCriteria
        Return Nothing
    End Function

    Public Function GetUserInput() As payment Implements IBaseView(Of payment, PaymentListVM).GetUserInput
        Dim data As New payment
        AutoMapControlsToEntity(data, Me)

        If Not data.p_comp_Id.HasValue Then Throw New Exception("請選擇公司")

        If String.IsNullOrEmpty(data.p_Type) Then
            Throw New Exception("請選擇付款類型")
        ElseIf data.p_Type = "銀行存款" AndAlso Not data.p_bank_Id.HasValue Then
            Throw New Exception("請選擇銀行帳號")
        ElseIf data.p_Type = "應付票據" AndAlso String.IsNullOrEmpty(data.p_Cheque) Then
            Throw New Exception("請選擇支票號碼")
        End If

        If Not data.p_s_Id.HasValue Then Throw New Exception("請選擇科目")

        If data.p_Amount <= 0 Then Throw New Exception("請選擇輸入金額")

        If data.p_Type <> "應付票據" Then data.p_CashingDate = Nothing

        Return data
    End Function

    Public Sub SetButton(isSelectedRow As Boolean) Implements IPaymentView.SetButton
        SetButtonState(Me, isSelectedRow)
    End Sub

    ' 事件
    Private Sub PaymentUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel_payment.PerformClick()
        ReadDataGridWidth(dgvPayment)
    End Sub

    Private Sub btnCancel_payment_Click(sender As Object, e As EventArgs) Handles btnCancel_payment.Click
        RaiseEvent CancelRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub btnAdd_payment_Click(sender As Object, e As EventArgs) Handles btnAdd_payment.Click
        RaiseEvent AddRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub dgvPayment_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPayment.SelectionChanged, dgvPayment.CellMouseClick
        If Not dgvPayment.Focused OrElse dgvPayment.SelectedRows.Count = 0 Then Return

        Dim row = dgvPayment.SelectedRows(0)
        Dim id As Integer = row.Cells("編號").Value

        RaiseEvent DetailRequested(Me, id)
    End Sub

    Private Sub btnEdit_payment_Click(sender As Object, e As EventArgs) Handles btnEdit_payment.Click
        RaiseEvent UpdateRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub btnDelete_payment_Click(sender As Object, e As EventArgs) Handles btnDelete_payment.Click
        If MessageBox.Show("確定要刪除這筆資料嗎？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            RaiseEvent DeleteRequested(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub btnQuery_payment_Click(sender As Object, e As EventArgs) Handles btnQuery_payment.Click

    End Sub

    Private Sub cmbPayType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPayType.SelectedIndexChanged
        ControlColumns()
    End Sub

    Private Sub cmbManu_payment_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbManu_payment.SelectionChangeCommitted
        RaiseEvent ManufacturerSelected(Me, cmbManu_payment.SelectedValue)
    End Sub

    Private Sub btnPrint_pay_Click(sender As Object, e As EventArgs) Handles btnPrint_pay.Click
        Using frm As New Print_Subpoena
            If frm.ShowDialog = DialogResult.OK Then
                RaiseEvent PrintRequested(Me, Tuple.Create(frm.SelectDate, frm.Type))
            End If
        End Using
    End Sub

    ' 紀錄dgv欄寬
    Private Sub dgvPayment_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvPayment.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub

    ' 選擇公司
    Private Sub cmbCompany_payment_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbCompany_payment.SelectionChangeCommitted
        RaiseEvent CompanySelected(Me, cmbCompany_payment.SelectedValue)
    End Sub

    Private Sub cmbSubjects_payment_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSubjects_payment.SelectedIndexChanged
        ControlColumns()
    End Sub

    ' 方法
    Private Sub ControlColumns()
        Dim paymentType As String = cmbPayType.Text
        Dim debitType As String = cmbSubjects_payment.Text
        Dim ctrlVisibility As New Dictionary(Of String, Control()) From {
            {"應付票據", {
                lblReq_Chuque,
                lblCheNo_payment,
                txtCheNo_payment,
                chkCashing,
                lblCashingDate_payment,
                dtpCashing,
                txtVendorAccount_payment,
                lblVendorBankRequired_payment,
                lblVendorBank_payment,
                lblBankReq,
                lblBank,
                cmbBank
            }},
            {"銀行存款", {lblVendorBankRequired_payment, lblVendorBank_payment, txtVendorAccount_payment, lblBankReq, lblBank, cmbBank}}
        }

        ' 先隱藏所有相關控制項
        For Each arr In ctrlVisibility.Values
            For Each ctrl In arr
                ctrl.Visible = False
            Next
        Next

        ' 再依照 paymentType 顯示對應控制項
        If ctrlVisibility.ContainsKey(paymentType) Then
            For Each ctrl In ctrlVisibility(paymentType)
                ctrl.Visible = True
            Next
        End If

        If debitType = "銀行存款" Then
            lblBankReq.Visible = True
            lblBank.Visible = True
            cmbBank.Visible = True
        End If
    End Sub
End Class
