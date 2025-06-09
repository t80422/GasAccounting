Public Class Print_Subpoena
    Private _SelectDate As Object

    Public ReadOnly Property SelectDate
        Get
            Return dtpDate.Value.Date
        End Get
    End Property

    Public Property Type As String

    Private Sub btnCash_Click(sender As Object, e As EventArgs) Handles btnCash.Click
        Type = "現金"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnTransfer_Click(sender As Object, e As EventArgs) Handles btnTransfer.Click
        Type = "轉帳"
        DialogResult = DialogResult.OK
    End Sub
End Class