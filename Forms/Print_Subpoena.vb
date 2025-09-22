Public Class Print_Subpoena
    Public ReadOnly Property SelectDate As Date
        Get
            Return dtpDate.Value.Date
        End Get
    End Property

    Public Property Type As String

    Sub New(Optional transferOnly As Boolean = False)

        ' 設計工具需要此呼叫。
        InitializeComponent()

        btnCash.Visible = Not transferOnly

    End Sub

    Private Sub btnCash_Click(sender As Object, e As EventArgs) Handles btnCash.Click
        Type = "現金"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnTransfer_Click(sender As Object, e As EventArgs) Handles btnTransfer.Click
        Type = "轉帳"
        DialogResult = DialogResult.OK
    End Sub
End Class