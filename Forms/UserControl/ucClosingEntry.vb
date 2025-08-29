' 結轉
Public Class ucClosingEntry
    Inherits UserControl
    Implements IClosingEntryView

    Private _presenter As ClosingEntryPresenter

    Public Sub New(presenter As ClosingEntryPresenter)
        InitializeComponent()
        _presenter = presenter
    End Sub

    Public Sub SetSubjectDropdown(data As List(Of SelectListItem)) Implements IClosingEntryView.SetSubjectDropdown
        ' 設定借方科目下拉選單
        cmbDebit.DataSource = data.ToList()
        cmbDebit.DisplayMember = "Display"
        cmbDebit.ValueMember = "Value"
        cmbDebit.SelectedIndex = -1

        ' 設定貸方科目下拉選單
        cmbCredit.DataSource = data.ToList()
        cmbCredit.DisplayMember = "Display"
        cmbCredit.ValueMember = "Value"
        cmbCredit.SelectedIndex = -1
    End Sub

    Public Sub DisplayList(data As List(Of ClosingEntryVM)) Implements IBaseView(Of closing_entry, ClosingEntryVM).DisplayList
        dgvClosingEntry.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As closing_entry) Implements IBaseView(Of closing_entry, ClosingEntryVM).DisplayDetail
        dtpCE.Value = data.ce_Date
        cmbDebit.SelectedValue = data.ce_Debit
        cmbCredit.SelectedValue = data.ce_Credit
        txtAmount_credit.Text = data.ce_CreditAmount
        txtAmount_debit.Text = data.ce_DebitAmount
        txtMemo_credit.Text = data.ce_CreditMemo
        txtMemo_debit.Text = data.ce_DebitMemo
    End Sub

    Public Sub ClearInput() Implements IBaseView(Of closing_entry, ClosingEntryVM).ClearInput
        ClearControls(Me)
    End Sub

    Private Sub ucClosingEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _presenter.SetView(Me)
        btnCancel.PerformClick()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        SetButtonState(sender, True)
        _presenter.Reset()
    End Sub

    Public Function GetUserInput() As closing_entry Implements IBaseView(Of closing_entry, ClosingEntryVM).GetUserInput
        If cmbDebit.SelectedIndex < 0 Then Throw New Exception("請先選擇借方科目")
        If cmbCredit.SelectedIndex < 0 Then Throw New Exception("請先選擇貸方科目")
        If String.IsNullOrEmpty(txtMemo_credit.Text) Then Throw New Exception("請輸入貸方備註")
        If String.IsNullOrEmpty(txtMemo_debit.Text) Then Throw New Exception("請輸入借方備註")
        If String.IsNullOrEmpty(txtAmount_credit.Text) Then Throw New Exception("請輸入貸方金額")
        If String.IsNullOrEmpty(txtAmount_debit.Text) Then Throw New Exception("請輸入借方金額")

        Return New closing_entry With {
            .ce_Date = dtpCE.Value.Date,
            .ce_Debit = cmbDebit.SelectedValue,
            .ce_Credit = cmbCredit.SelectedValue,
            .ce_CreditAmount = If(IsNumeric(txtAmount_credit.Text), CDbl(txtAmount_credit.Text), 0),
            .ce_DebitAmount = If(IsNumeric(txtAmount_debit.Text), CDbl(txtAmount_debit.Text), 0),
            .ce_CreditMemo = txtMemo_credit.Text,
            .ce_DebitMemo = txtMemo_debit.Text
        }
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        _presenter.Add()
    End Sub

    Private Sub dgvClosingEntry_CellMouseClick(sender As Object, e As EventArgs) Handles dgvClosingEntry.CellMouseClick, dgvClosingEntry.SelectionChanged
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Or ctrl.SelectedRows.Count = 0 Then Return
        SetButtonState(ctrl, False)

        Dim id = ctrl.SelectedRows(0).Cells(0).Value
        _presenter.Detail(id)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        _presenter.Update()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        _presenter.Delete()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Using frm As New Search_ClosingEntry
            If frm.ShowDialog() = DialogResult.OK Then
                _presenter.LoadList(frm.Criteria)
            End If
        End Using
    End Sub
End Class
