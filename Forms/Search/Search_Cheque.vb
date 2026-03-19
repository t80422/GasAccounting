Public Class Search_Cheque
    Public Criteria As New ChequeSC

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        With Criteria
            .StartDate = dtpStart.Value.Date
            .EndDate = dtpEnd.Value.Date.AddDays(1)
            .IsDate = chkDate.Checked
            .Status = cmbStatus.SelectedItem
            .IsStatus = chkStatus.Checked
            .BankId = cmbBank.SelectedValue
            .CollectionDateStart = dtpColDateStart.Value.Date
            .CollectionDateEnd = dtpColDateEnd.Value.Date.AddDays(1)
            .IsCollectionDate = chkColDate.Checked
        End With

        DialogResult = DialogResult.OK
    End Sub

    Private Sub Search_Cheque_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Using db As New gas_accounting_systemEntities
            Dim bankRep As New BankRep(db)
            Dim banks = bankRep.GetBankDropdownAsync().Result
            SetComboBox(cmbBank, banks)
        End Using
    End Sub
End Class