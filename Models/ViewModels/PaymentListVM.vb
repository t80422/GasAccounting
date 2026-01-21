Public Class PaymentListVM
    Public Property 編號 As Integer
    Public Property 日期 As Date
    Public Property 帳款月份 As String
    Public Property 金額 As Integer
    Public Property 貸方科目 As String
    Public Property 廠商名稱 As String
    Public Property 支票號碼 As String
    Public Property 借方科目 As String
    Public Property 借方公司 As String
    Public Property 借方銀行 As String
    Public Property 借方金額 As Integer
    Public Property 借方科目2 As String
    Public Property 借方公司2 As String
    Public Property 借方銀行2 As String
    Public Property 借方金額2 As Integer
    Public Property 借方科目3 As String
    Public Property 借方公司3 As String
    Public Property 借方銀行3 As String
    Public Property 借方金額3 As Integer
    Public Property 備註 As String


    Public Sub New(payment As payment)
        編號 = payment.p_Id
        日期 = payment.p_Date
        金額 = payment.p_Amount
        貸方科目 = payment.p_Type
        廠商名稱 = payment.manufacturer?.manu_name
        借方科目 = payment.subject1?.s_name
        支票號碼 = payment.chque_pay?.cp_Number
        備註 = payment.p_Memo
        帳款月份 = payment.p_AccountMonth?.ToString("yyyy年MM月")
        借方公司 = payment.company1?.comp_name
        借方銀行 = payment.bank1?.bank_name
        借方金額 = If(payment.p_debit_amount_1, 0)
        借方科目2 = payment.subject2?.s_name
        借方公司2 = payment.company2?.comp_name
        借方銀行2 = payment.bank2?.bank_name
        借方金額2 = If(payment.p_debit_amount_2, 0)
        借方科目3 = payment.subject?.s_name
        借方公司3 = payment.company?.comp_name
        借方銀行3 = payment.bank?.bank_name
        借方金額3 = If(payment.p_debit_amount_3, 0)
    End Sub
End Class
