Public Class frmSearch_Collection
    Private ReadOnly _subjectRep As ISubjectRep
    Private ReadOnly _cusRep As ICustomerRep
    Private ReadOnly _bankRep As IBankRep

    Public Criteria As New CollectionSearchCriteria

    Public Sub New(subjectRep As ISubjectRep, cusRep As ICustomerRep, bankRep As IBankRep)

        ' 設計工具需要此呼叫。
        InitializeComponent()

        _subjectRep = subjectRep
        _cusRep = cusRep
        _bankRep = bankRep
    End Sub

    Private Async Sub frmSearch_Collection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetComboBox(cmbSubject, Await _subjectRep.GetSubjectDropdownAsync)
        SetComboBox(cmbBank, Await _bankRep.GetBankDropdownAsync)
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        With Criteria
            .CusId = If(String.IsNullOrEmpty(txtCusId.Text), Nothing, txtCusId.Text)
            .SubjectId = If(cmbSubject.SelectedValue, Nothing)
            .Type = If(cmbType.SelectedItem, Nothing)
            .ChequeNum = If(String.IsNullOrEmpty(txtChequeNum.Text), Nothing, txtChequeNum.Text)
            .IsDate = chkDate.Checked
            .StartDate = dtpStart.Value
            .EndDate = dtpEnd.Value
            .BankId = If(cmbBank.SelectedValue, Nothing)
            .AccountMonth = dtpAccountMonth.Value
            .IsAccountMonth = chkAccountMonth.Checked
        End With

        DialogResult = DialogResult.OK
    End Sub

    ' 按下Enter時,搜尋客戶資料
    Private Sub txtCusCode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim cus = _cusRep.GetByCusCode(Trim(txtCusCode.Text))

            If cus IsNot Nothing Then
                txtCusCode.Text = cus.cus_code
                txtCusName.Text = cus.cus_name
                txtCusId.Text = cus.cus_id
            Else
                MsgBox("查無此客戶資料", MsgBoxStyle.Critical, "錯誤")
            End If
        End If
    End Sub
End Class