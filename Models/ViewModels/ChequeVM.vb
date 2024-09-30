Public Class ChequeVM
    Public Property 編號 As Integer
    Public Property 收票日期 As Date?
    Public Property 支票號碼 As String
    Public Property 銀行帳號 As String
    Public Property 發票人 As String
    Public Property 金額 As Integer
    Public Property 狀態 As String
    Public Property 兌現日期 As Date?
    Public Property 代收日期 As Date?

    Public Sub New()

    End Sub

    Public Sub New(data As cheque)
        編號 = data.che_Id
        收票日期 = data.che_ReceivedDate
        支票號碼 = data.che_Number
        銀行帳號 = data.che_AccountNumber
        發票人 = data.che_IssuerName
        金額 = data.che_Amount
        狀態 = data.chu_State
        兌現日期 = data.che_CashingDate
        代收日期 = data.che_CollectionDate
    End Sub
End Class
