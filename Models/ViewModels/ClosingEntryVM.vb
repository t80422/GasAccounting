Public Class ClosingEntryVM
    Public Property 編號 As Integer
    Public Property 日期 As String
    Public Property 借方科目 As String
    Public Property 借方備註 As String
    Public Property 借方金額 As Double
    Public Property 貸方科目 As String
    Public Property 貸方備註 As String
    Public Property 貸方金額 As Double

    Public Sub New(data As closing_entry)
        編號 = data.ce_Id
        日期 = data.ce_Date.Value.ToString("yyyy/MM/dd")
        借方科目 = data.subject1.s_name
        借方備註 = data.ce_DebitMemo
        借方金額 = data.ce_DebitAmount.Value
        貸方科目 = data.subject.s_name
        貸方備註 = data.ce_CreditMemo
        貸方金額 = data.ce_CreditAmount
    End Sub
End Class
