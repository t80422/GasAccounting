Public Class BankAccount
    Public Property 年月 As String
    Public Property List As List(Of BankAccountList)
End Class

Public Class BankAccountList
    Public Property 日期 As Integer
    Public Property 科目 As String
    Public Property 摘要 As String
    Public Property 借方 As Integer
    Public Property 貸方 As Integer
    Public Property 餘額 As Integer
End Class
