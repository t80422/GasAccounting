Imports System.Windows.Forms

''' <summary>
''' 銀行月結餘額重整工具
''' </summary>
Public Class frmBankBalanceRecalculator
    Inherits Form

    Private WithEvents btnRecalculate As Button
    Private WithEvents btnClose As Button
    Private txtResult As TextBox
    Private lblTitle As Label
    Private progressBar As ProgressBar

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Me.Text = "銀行月結餘額重整工具"
        Me.Size = New Size(600, 400)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        ' 標題
        lblTitle = New Label With {
            .Text = "此工具會重新計算所有銀行的月結餘額" & vbCrLf & "請確認是否要執行重整作業？",
            .Location = New Point(20, 20),
            .Size = New Size(540, 60),
            .Font = New Font("微軟正黑體", 12, FontStyle.Regular),
            .ForeColor = Color.DarkRed
        }

        ' 執行按鈕
        btnRecalculate = New Button With {
            .Text = "執行重整",
            .Location = New Point(200, 90),
            .Size = New Size(100, 35),
            .Font = New Font("微軟正黑體", 10, FontStyle.Bold)
        }

        ' 關閉按鈕
        btnClose = New Button With {
            .Text = "關閉",
            .Location = New Point(310, 90),
            .Size = New Size(100, 35),
            .Font = New Font("微軟正黑體", 10, FontStyle.Regular)
        }

        ' 進度條
        progressBar = New ProgressBar With {
            .Location = New Point(20, 140),
            .Size = New Size(540, 25),
            .Style = ProgressBarStyle.Marquee,
            .Visible = False
        }

        ' 結果文字框
        txtResult = New TextBox With {
            .Location = New Point(20, 180),
            .Size = New Size(540, 160),
            .Multiline = True,
            .ScrollBars = ScrollBars.Vertical,
            .ReadOnly = True,
            .Font = New Font("微軟正黑體", 10, FontStyle.Regular)
        }

        ' 加入控制項
        Me.Controls.AddRange(New Control() {lblTitle, btnRecalculate, btnClose, progressBar, txtResult})
    End Sub

    Private Async Sub btnRecalculate_Click(sender As Object, e As EventArgs) Handles btnRecalculate.Click
        Try
            ' 確認對話框
            Dim result = MessageBox.Show(
                "此操作會刪除現有的所有銀行月結餘額記錄，並重新計算。" & vbCrLf & vbCrLf &
                "建議在執行前先備份資料庫。" & vbCrLf & vbCrLf &
                "確定要繼續嗎？",
                "確認重整",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            )

            If result <> DialogResult.Yes Then
                Return
            End If

            ' 禁用按鈕，顯示進度條
            btnRecalculate.Enabled = False
            btnClose.Enabled = False
            progressBar.Visible = True
            txtResult.Text = "正在執行重整作業，請稍候..." & vbCrLf

            ' 執行重整
            Using uow As New UnitOfWork()
                uow.BeginTransaction()

                Try
                    Dim bmbService As IBankMonthlyBalanceService = DependencyContainer.Resolve(Of IBankMonthlyBalanceService)()

                    Dim resultMessage = Await bmbService.RecalculateAllBanksAsync(
                        uow.BankMonthlyBalancesRepository,
                        uow.BankRepository,
                        uow.PaymentRepository,
                        uow.CollectionRepository
                    )

                    Await uow.SaveChangesAsync()
                    uow.Commit()

                    txtResult.Text = resultMessage
                    MessageBox.Show("重整完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    uow.Rollback()
                    txtResult.Text = "重整失敗！" & vbCrLf & vbCrLf & "錯誤訊息：" & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.StackTrace
                    MessageBox.Show("重整失敗：" & ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        Catch ex As Exception
            txtResult.Text = "發生未預期的錯誤！" & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.StackTrace
            MessageBox.Show("發生錯誤：" & ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' 恢復按鈕，隱藏進度條
            btnRecalculate.Enabled = True
            btnClose.Enabled = True
            progressBar.Visible = False
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class

