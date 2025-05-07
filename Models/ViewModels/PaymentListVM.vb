Public Class PaymentListVM
    Public Property 編號 As Integer
    Public Property 日期 As Date
    Public Property 金額 As Integer
    Public Property 付款類型 As String
    Public Property 廠商名稱 As String
    Public Property 銀行名稱 As String
    Public Property 科目 As String
    Public Property 帳款月份 As String
    Public Property 支票號碼 As String
    Public Property 備註 As String

    Public Sub New(payment As payment)
        編號 = payment.p_Id
        日期 = payment.p_Date
        金額 = payment.p_Amount
        付款類型 = payment.p_Type
        廠商名稱 = payment.manufacturer?.manu_name
        銀行名稱 = payment.bank?.bank_name
        科目 = payment.subject?.s_name
        支票號碼 = payment.p_Cheque
        備註 = payment.p_Memo
    End Sub
End Class
