Public Class frmLogin
    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        If String.IsNullOrEmpty(UsernameTextBox.Text) Then
            MsgBox("�п�J�b��")
            Return
        End If

        If String.IsNullOrEmpty(PasswordTextBox.Text) Then
            MsgBox("�п�J�K�X")
            Return
        End If

        Try
            Using db As New gas_accounting_systemEntities
                Dim user = db.employees.Where(Function(x) x.emp_acc = UsernameTextBox.Text).FirstOrDefault

                If user IsNot Nothing Then
                    If user.emp_psw = PasswordTextBox.Text Then
                        MsgBox("�n�J���\")
                        Hide()

                        frmMain.User.Id = user.emp_id
                        frmMain.User.Name = user.emp_name
                        frmMain.Show()
                    Else
                        MsgBox("�K�X���~")
                        Return
                    End If
                Else
                    MsgBox("���s�b���b��")
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

End Class
