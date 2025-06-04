Public Class Search_Cheque
    Public Criteria As New ChequeSC

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        With Criteria
            .StartDate = dtpStart.Value.Date
            .EndDate = dtpEnd.Value.Date.AddDays(1)
            .IsDate = chkDate.Checked
            .Status = cmbStatus.SelectedItem
        End With

        DialogResult = DialogResult.OK
    End Sub
End Class