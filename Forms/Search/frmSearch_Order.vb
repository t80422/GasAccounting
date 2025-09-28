Public Class frmSearch_Order
    Public Criteria As New OrderSearchCriteria
    Private _cusRep As New CustomerRep(New gas_accounting_systemEntities)

    Private Sub btnSearchCus_Click(sender As Object, e As EventArgs) Handles btnSearchCus.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusID.Text = searchForm.CusId
                txtCusCode.Text = searchForm.CusCode
                txtCusName.Text = searchForm.CusName
            End If
        End Using
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        With Criteria
            .CusId = If(txtCusID.Text = "", Nothing, Integer.Parse(txtCusID.Text))
            .IsDate = chkIsDate.Checked
            .StartDate = dtpStart.Value
            .EndDate = dtpEnd.Value
            .SearchIn = chkIn.Checked
            .SearchOut = chkOut.Checked
        End With

        DialogResult = DialogResult.OK
    End Sub

    Private Sub txtCusCode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim cus = _cusRep.GetByCusCode(txtCusCode.Text)
            If cus IsNot Nothing Then
                txtCusID.Text = cus.cus_id
                txtCusCode.Text = cus.cus_code
                txtCusName.Text = cus.cus_name
            Else
                MsgBox("找不到該客戶")
            End If
        End If
    End Sub
End Class