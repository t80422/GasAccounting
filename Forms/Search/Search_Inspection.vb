Public Class Search_Inspection
    Public Criteria As New InspectionSC
    Private _cusRep As New CustomerRep(New gas_accounting_systemEntities)

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        With Criteria
            .CusId = If(txtCusId.Text = "", Nothing, Integer.Parse(txtCusId.Text))
            .IsDate = chkDate.Checked
            .Month = dtpDate.Value.Date
        End With

        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCus_Click(sender As Object, e As EventArgs) Handles btnCus.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusId.Text = searchForm.CusId
                txtCusCode.Text = searchForm.CusCode
                txtCusName.Text = searchForm.CusName
            End If
        End Using
    End Sub

    Private Sub txtCusCode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim cus = _cusRep.GetByCusCode(txtCusCode.Text)
            If cus IsNot Nothing Then
                txtCusId.Text = cus.cus_id
                txtCusCode.Text = cus.cus_code
                txtCusName.Text = cus.cus_name
            Else
                MsgBox("找不到該客戶")
            End If
        End If
    End Sub
End Class