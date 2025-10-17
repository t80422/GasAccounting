Public Class Search_ChequePay
    Public Criteria As New ChequeSC
    Private Sub Search_ChequePay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Using db As New gas_accounting_systemEntities
            Dim compRep As New CompanyRep(db)
            Dim companise = compRep.GetCompanyDropdownAsync().Result
            SetComboBox(cmbCompany, companise)
        End Using
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        With Criteria
            .StartDate = dtpStart.Value.Date
            .EndDate = dtpEnd.Value.Date.AddDays(1)
            .IsDate = chkDate.Checked
            .Status = cmbStatus.SelectedItem
            .IsStatus = chkStatus.Checked
            .CompanyId = cmbCompany.SelectedValue
        End With

        DialogResult = DialogResult.OK
    End Sub
End Class