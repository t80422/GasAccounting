Public Class frmInitializeMonthlyAccounts
    Private ReadOnly _monthlyAccountService As IMonthlyAccountService

    Public Sub New()
        InitializeComponent()
        _monthlyAccountService = New MonthlyAccountService()
    End Sub

    Private Sub btnInitialize_Click(sender As Object, e As EventArgs) Handles btnInitialize.Click
        If MessageBox.Show("確定要初始化月度帳單資料嗎？這將清空現有的月度帳單資料，並重新從訂單資料中建立。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Cursor = Cursors.WaitCursor
            lblStatus.Text = "正在初始化月度帳單資料，請稍候..."
            Application.DoEvents()

            Try
                If _monthlyAccountService.InitializeMonthlyAccounts() Then
                    lblStatus.Text = "初始化完成！"
                    MessageBox.Show("月度帳單資料初始化完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    lblStatus.Text = "初始化失敗！"
                    MessageBox.Show("月度帳單資料初始化失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Catch ex As Exception
                lblStatus.Text = "初始化失敗！"
                MessageBox.Show($"初始化過程中發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub
End Class 