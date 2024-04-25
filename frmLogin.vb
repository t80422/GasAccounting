Public Class frmLogin
    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        If String.IsNullOrEmpty(UsernameTextBox.Text) Then
            MsgBox("請輸入帳號")
            Return
        End If

        If String.IsNullOrEmpty(PasswordTextBox.Text) Then
            MsgBox("請輸入密碼")
            Return
        End If

        Try
            Using db As New gas_accounting_systemEntities
                Dim user = db.employees.Where(Function(x) x.emp_acc = UsernameTextBox.Text).FirstOrDefault

                If user IsNot Nothing Then
                    If user.emp_psw = PasswordTextBox.Text Then
                        Hide()

                        frmMain.User.Id = user.emp_id
                        frmMain.User.Name = user.emp_name
                        frmMain.Show()
                    Else
                        MsgBox("密碼錯誤")
                        Return
                    End If
                Else
                    MsgBox("不存在的帳號")
                    Return
                End If
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
