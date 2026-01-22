Public Class frmSearch_Order
    Public Criteria As New OrderSearchCriteria
    Private _cusRep As New CustomerRep(New gas_accounting_systemEntities)

    Private Sub btnSearchCus_Click(sender As Object, e As EventArgs) Handles btnSearchCus.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusCode.Text = searchForm.CusCode
                txtCusName.Text = searchForm.CusName
            End If
        End Using
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        With Criteria
            .CusCode = txtCusCode.Text
            .IsDate = chkIsDate.Checked
            .StartDate = dtpStart.Value
            .EndDate = dtpEnd.Value
            .SearchIn = chkIn.Checked
            .SearchOut = chkOut.Checked
        End With

        DialogResult = DialogResult.OK
    End Sub
End Class