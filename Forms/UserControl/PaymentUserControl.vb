Public Class PaymentUserControl
    Implements IPaymentView

    Private _presenter As PaymentPresenter

    Public Sub New(presenter As PaymentPresenter)
        InitializeComponent()
        _presenter = presenter
        _presenter.SetView(Me)
    End Sub

    Private Sub PaymentUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel_payment.PerformClick()
        ReadDataGridWidth(dgvPayment)
    End Sub

    Public Sub PopulateVendorDropdown(data As IReadOnlyList(Of SelectListItem)) Implements IPaymentView.PopulateVendorDropdown
        SetComboBox(cmbManu_payment, data)
    End Sub

    Public Sub PopulateSubjectDropdown(data As IReadOnlyList(Of SelectListItem)) Implements IPaymentView.PopulateSubjectDropdown
        SetComboBox(cmbSubjects_payment, data)
    End Sub

    Public Sub PopulateCompanyDropdown(data As IReadOnlyList(Of SelectListItem)) Implements IPaymentView.PopulateCompanyDropdown
        SetComboBox(cmbCompany_payment, data)
    End Sub

    Public Sub DisplayAmountDueList(data As IReadOnlyList(Of AmountDueVM)) Implements IPaymentView.DisplayAmountDueList
        dgvAmountDue.DataSource = data
    End Sub

    Public Sub ShowDetail(data As PaymentVM) Implements IPaymentView.ShowDetail
        AutoMapEntityToControls(data.Payment, Me)
    End Sub

    Public Sub ShowVendorAccount(data As String) Implements IPaymentView.ShowVendorAccount
        txtAccount_payment.Text = data
    End Sub

    Public Sub DisplayList(data As List(Of PaymentListVM)) Implements IBaseView(Of payment, PaymentListVM).DisplayList
        dgvPayment.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As payment) Implements IBaseView(Of payment, PaymentListVM).DisplayDetail
        Throw New NotImplementedException()
    End Sub

    Public Sub ClearInput() Implements IBaseView(Of payment, PaymentListVM).ClearInput
        ClearControls(Me)
        dgvAmountDue.DataSource = Nothing
    End Sub

    Public Function GetSearchCriteria() As PaymentSearchCriteria Implements IPaymentView.GetSearchCriteria
        Return New PaymentSearchCriteria With {
            .ChequeNo = txtCheNo_payment.Text.Trim,
            .CompanyId = cmbCompany_payment.SelectedItem?.Value,
            .IsSearchDate = chkIsSearchDate_payment.Checked,
            .StartDate = dtpStart_payment.Value.Date,
            .EndDate = dtpEnd_payment.Value.Date.AddDays(1),
            .VendorId = cmbManu_payment.SelectedItem?.Value
        }
    End Function

    Public Function GetInput() As PaymentVM Implements IPaymentView.GetInput
        Dim data As New payment
        AutoMapControlsToEntity(data, Me)

        If Not data.p_comp_Id.HasValue Then Throw New Exception("請選擇公司")

        If String.IsNullOrEmpty(data.p_Type) Then
            Throw New Exception("請選擇付款類型")
        ElseIf data.p_Type = "支票" AndAlso String.IsNullOrEmpty(data.p_Cheque) Then
            Throw New Exception("請選擇支票號碼")
        End If

        If Not data.p_s_Id.HasValue Then Throw New Exception("請選擇科目")

        If data.p_Amount <= 0 Then Throw New Exception("請選擇輸入金額")

        If data.p_Type <> "支票" Then data.p_CashingDate = Nothing

        Return New PaymentVM With {
            .Payment = data
        }
    End Function

    Public Function GetUserInput() As payment Implements IBaseView(Of payment, PaymentListVM).GetUserInput
        Dim data As New payment
        AutoMapControlsToEntity(data, Me)

        If Not data.p_comp_Id.HasValue Then Throw New Exception("請選擇公司")

        If String.IsNullOrEmpty(data.p_Type) Then
            Throw New Exception("請選擇付款類型")
        ElseIf data.p_Type = "銀行" AndAlso Not data.p_bank_Id.HasValue Then
            Throw New Exception("請選擇銀行帳號")
        ElseIf data.p_Type = "支票" AndAlso String.IsNullOrEmpty(data.p_Cheque) Then
            Throw New Exception("請選擇支票號碼")
        End If

        If Not data.p_s_Id.HasValue Then Throw New Exception("請選擇科目")

        If data.p_Amount <= 0 Then Throw New Exception("請選擇輸入金額")

        If data.p_Type <> "支票" Then data.p_CashingDate = Nothing

        Return data
    End Function

    ''' <summary>
    ''' 設定付款作業查詢控制項狀態
    ''' </summary>
    Private Sub SetPaymentQueryCtrlsState()
        Dim lst As New List(Of Control) From {lblPayType_payment, lblManu_payment, lblBank_payment}
        SetQueryControls(btnQuery_payment, lst)
    End Sub

    ' 取消
    Private Async Sub btnCancel_payment_Click(sender As Object, e As EventArgs) Handles btnCancel_payment.Click
        SetButtonState(sender, True)
        Await _presenter.InitializeAsync
        If btnQuery_payment.Text = "確  認" Then SetPaymentQueryCtrlsState()
    End Sub

    ' 新增
    Private Sub btnAdd_payment_Click(sender As Object, e As EventArgs) Handles btnAdd_payment.Click
        _presenter.Add()
    End Sub

    ' dgv
    Private Async Sub dgvPayment_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPayment.SelectionChanged, dgvPayment.CellMouseClick
        Dim id = DGV_SelectionChanged(sender)
        If id > 0 Then
            Await _presenter.LoadPaymentDetailAsync(id)
            _presenter.LoadVendorAmountDue(cmbManu_payment.SelectedItem.Value)
        End If
    End Sub

    ' 修改
    Private Sub btnEdit_payment_Click(sender As Object, e As EventArgs) Handles btnEdit_payment.Click
        _presenter.Update()
    End Sub

    ' 刪除
    Private Async Sub btnDelete_payment_Click(sender As Object, e As EventArgs) Handles btnDelete_payment.Click
        Await _presenter.DeleteAsync(txtId_payment.Text)
        SetButtonState(sender, True)
    End Sub

    ' 查詢
    Private Async Sub btnQuery_payment_Click(sender As Object, e As EventArgs) Handles btnQuery_payment.Click
        SetPaymentQueryCtrlsState()

        If sender.Text = "查  詢" Then
            Await _presenter.SearchPaymentsAsync()
        End If
    End Sub

    Private Sub cmbPayType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPayType.SelectedIndexChanged
        Dim cmb As ComboBox = sender
        Dim paymentType As String = cmb.Text
        Dim ctrlVisibility As New Dictionary(Of String, Control()) From {
            {"支票", {lblReq_Chuque, lblCheNo_payment, txtCheNo_payment, chkCashing, lblCashingDate_payment, dtpCashing, txtAccount_payment, lblBankRequired_payment, lblBank_payment}},
            {"銀行", {lblBankRequired_payment, lblBank_payment, txtAccount_payment}}
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
    End Sub

    ' 選擇廠商
    Private Sub cmbManu_payment_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbManu_payment.SelectionChangeCommitted
        _presenter.LoadVendorAmountDue(cmbManu_payment.SelectedValue)
        _presenter.ShowVendorAccountAsync(cmbManu_payment.SelectedValue)
    End Sub

    ' 列印傳票
    Private Sub btnPrint_pay_Click(sender As Object, e As EventArgs) Handles btnPrint_pay.Click
        Using frm As New Print_Subpoena
            If frm.ShowDialog = DialogResult.OK Then
                _presenter.Print(frm.SelectDate, frm.Type)
            End If
        End Using
    End Sub

    ' 紀錄dgv欄寬
    Private Sub dgvPayment_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvPayment.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub
End Class
