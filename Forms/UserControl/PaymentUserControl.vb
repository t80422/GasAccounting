' 付款作業
Imports System.ComponentModel

Public Class PaymentUserControl
    Implements IPaymentView

    Public Event PrintRequested As EventHandler(Of Tuple(Of Date, String)) Implements IPaymentView.PrintRequested
    Public Event ManufacturerSelected As EventHandler(Of Integer) Implements IPaymentView.ManufacturerSelected
    Public Event CompanySelected As EventHandler(Of Integer) Implements IPaymentView.CompanySelected
    Public Event CreateRequest As EventHandler Implements IFormView(Of payment, PaymentListVM).CreateRequest
    Public Event DataSelectedRequest As EventHandler(Of Integer) Implements IFormView(Of payment, PaymentListVM).DataSelectedRequest
    Public Event UpdateRequest As EventHandler Implements IFormView(Of payment, PaymentListVM).UpdateRequest
    Public Event DeleteRequest As EventHandler Implements IFormView(Of payment, PaymentListVM).DeleteRequest
    Public Event CancelRequest As EventHandler Implements IFormView(Of payment, PaymentListVM).CancelRequest
    Public Event SearchRequest As EventHandler Implements IFormView(Of payment, PaymentListVM).SearchRequest

    ' === 介面實作 ===
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

    Public Sub ShowList(data As List(Of PaymentListVM)) Implements IFormView(Of payment, PaymentListVM).ShowList
        dgvPayment.DataSource = data
    End Sub

    Public Sub ShowDetail(data As payment) Implements IFormView(Of payment, PaymentListVM).ShowDetail
        AutoMapEntityToControls(data, Me)
        If data.chque_pay IsNot Nothing Then AutoMapEntityToControls(data.chque_pay, Me)
    End Sub

    Private Sub ClearInput() Implements IFormView(Of payment, PaymentListVM).ClearInput
        ClearControls(Me)
        dgvAmountDue.DataSource = Nothing
    End Sub

    Public Function GetSearchCriteria() As PaymentSearchCriteria Implements IPaymentView.GetSearchCriteria
        Using frm As New frmSearch_Payment
            If frm.ShowDialog() = DialogResult.OK Then
                Return frm.Criteria
            Else
                Return Nothing
            End If
        End Using
    End Function

    Public Function GetInput(ByRef model As payment) As Boolean Implements IFormView(Of payment, PaymentListVM).GetInput
        AutoMapControlsToEntity(model, Me)

        If Not model.p_comp_Id.HasValue Then Throw New Exception("請選擇公司")

        If String.IsNullOrEmpty(model.p_Type) Then
            Throw New Exception("請選擇付款類型")
        ElseIf model.p_Type = "銀行存款" AndAlso Not model.p_bank_Id.HasValue Then
            Throw New Exception("請選擇銀行帳號")
        End If

        If Not model.p_s_Id.HasValue Then Throw New Exception("請選擇科目")

        If model.p_Amount <= 0 Then Throw New Exception("請選擇輸入金額")

        Return True
    End Function

    Public Sub GetChequeInput(ByRef model As chque_pay) Implements IPaymentView.GetChequeInput
        AutoMapControlsToEntity(model, Me)

        If String.IsNullOrEmpty(txtCheNo_payment.Text) Then Throw New Exception("請選擇支票號碼")
    End Sub

    Public Sub ButtonStatus(isSelectedRow As Boolean) Implements IFormView(Of payment, PaymentListVM).ButtonStatus
        SetButtonState(Me, isSelectedRow)
    End Sub

    Public Function GetChequeNumbers() As BindingList(Of SelectChequeVM) Implements IPaymentView.GetChequeNumbers
        Return dgvCheque.DataSource
    End Function

    Public Sub ShowChequeList(data As BindingList(Of SelectChequeVM)) Implements IPaymentView.ShowChequeList
        dgvCheque.DataSource = New BindingList(Of SelectChequeVM)(data)
    End Sub

    ' === 控制項事件 ===
    Private Sub PaymentUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel_payment.PerformClick()
        ReadDataGridWidth(dgvPayment)
        Dim list As New BindingList(Of SelectChequeVM)
        dgvCheque.DataSource = list
        dgvCheque.Columns("編號").Visible = False
        ReadDataGridWidth(dgvCheque)
    End Sub

    Private Sub btnCancel_payment_Click(sender As Object, e As EventArgs) Handles btnCancel_payment.Click
        RaiseEvent CancelRequest(Me, EventArgs.Empty)
    End Sub

    Private Sub btnAdd_payment_Click(sender As Object, e As EventArgs) Handles btnAdd_payment.Click
        RaiseEvent CreateRequest(Me, EventArgs.Empty)
    End Sub

    Private Sub dgvPayment_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPayment.SelectionChanged, dgvPayment.CellMouseClick
        If Not dgvPayment.Focused OrElse dgvPayment.SelectedRows.Count = 0 Then Return

        Dim row = dgvPayment.SelectedRows(0)
        Dim id As Integer = row.Cells("編號").Value

        RaiseEvent DataSelectedRequest(Me, id)
    End Sub

    Private Sub btnEdit_payment_Click(sender As Object, e As EventArgs) Handles btnEdit_payment.Click
        RaiseEvent UpdateRequest(Me, EventArgs.Empty)
    End Sub

    Private Sub btnDelete_payment_Click(sender As Object, e As EventArgs) Handles btnDelete_payment.Click
        If MessageBox.Show("確定要刪除這筆資料嗎？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            RaiseEvent DeleteRequest(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub cmbPayType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPayType.SelectedIndexChanged
        ControlColumns()
        ShowDgvCheque()
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
        ShowDgvCheque()
    End Sub

    Private Sub dgvCheque_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvCheque.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        RaiseEvent SearchRequest(sender, e)
    End Sub

    ' === 方法 ===
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

    Private Sub ShowDgvCheque()
        dgvCheque.Visible = cmbSubjects_payment.Text = "應付票據" AndAlso cmbPayType.Text = "銀行存款"
    End Sub
End Class
