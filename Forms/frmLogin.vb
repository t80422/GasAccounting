Imports System.Configuration

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
                        OpenMain(user)
                    Else
                        MsgBox("密碼錯誤")
                        Return
                    End If
                Else
                    MsgBox("不存在的帳號")
                    Return
                End If
            End Using
        Catch entityEx As Entity.Core.EntityException
            Dim mysqlEx As MySql.Data.MySqlClient.MySqlException = TryCast(entityEx.InnerException, MySql.Data.MySqlClient.MySqlException)
            If mysqlEx IsNot Nothing Then
                MsgBox("無法連線到資料庫")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Close()
    End Sub

    Private Sub OpenMain(user As employee)
        Hide()
        Dim permissions = GetUserPermissions(user.emp_id)
        Dim mainForm = New frmMain(user, permissions)
        mainForm.Show()
    End Sub

    Private Sub frmLogin_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If (ConfigurationManager.AppSettings("debug") = "T") Then
            Dim user = New employee
            user.emp_name = "test"
            user.emp_id = 0

            OpenMain(user)
        End If
    End Sub

    Private Function GetUserPermissions(empId As Integer) As List(Of String)
        Using db As New gas_accounting_systemEntities
            Return db.rolepermissions.Where(Function(x) x.role.employees.Any(Function(e) e.emp_id = empId)).
                                      Select(Function(x) x.permission.per_TabPageName).
                                      ToList
        End Using
    End Function
End Class
