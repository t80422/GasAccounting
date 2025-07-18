Public Class ucScrapBarrel

    Private _presenter As InspectionPresenter


    Sub New()
        ' 設計工具需要此呼叫。
        InitializeComponent()
    End Sub


    Private Sub btnCus_ins_Click(sender As Object, e As EventArgs) Handles btnCus_ins.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusId_ins.Text = searchForm.CusId
                txtCusCode_ins.Text = searchForm.CusCode
                txtCusName_ins.Text = searchForm.CusName
            End If
        End Using
    End Sub

    Private Sub txtCusCode_ins_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_ins.KeyDown
        If e.KeyCode = Keys.Enter Then
            _presenter.GetCustomerByCusCode(txtCusCode_ins.Text)
        End If
    End Sub
End Class
