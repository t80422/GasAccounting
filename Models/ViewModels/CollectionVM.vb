Public Class CollectionVM
    Public Property 編號 As Integer
    Public Property 日期 As Date
    Public Property 金額 As Integer
    Public Property 借方科目 As String
    Public Property 借方公司 As String
    Public Property 借方銀行 As String
    Public Property 支票號碼 As String
    Public Property 客戶名稱 As String
    Public Property 貸方科目 As String
    Public Property 帳款月份 As String
    Public Property 貸方銀行 As String
    Public Property 貸方公司 As String
    Public Property 貸方金額 As Integer
    Public Property 貸方科目2 As String
    Public Property 帳款月份2 As String
    Public Property 貸方銀行2 As String
    Public Property 貸方公司2 As String
    Public Property 貸方金額2 As Integer
    Public Property 貸方科目3 As String
    Public Property 帳款月份3 As String
    Public Property 貸方銀行3 As String
    Public Property 貸方公司3 As String
    Public Property 貸方金額3 As Integer
    Public Property 未銷帳金額 As Integer
    Public Property 備註 As String

    Public Sub New(data As collection)
        Try
            編號 = data.col_Id
            日期 = data.col_Date
            帳款月份 = data.col_AccountMonth.ToString("yyyy/MM")
            金額 = data.col_Amount
            借方科目 = data.col_Type
            支票號碼 = data.col_Cheque
            客戶名稱 = data.customer?.cus_name
            貸方科目 = data.subject1?.s_name
            借方銀行 = data.bank1?.bank_name
            借方公司 = data.company1?.comp_name
            備註 = data.col_Memo
            未銷帳金額 = data.col_UnmatchedAmount
            貸方銀行 = data.bank2?.bank_name
            貸方公司 = data.company2?.comp_name
            貸方科目2 = data.subject2?.s_name
            貸方銀行2 = data.bank3?.bank_name
            貸方公司2 = data.company3?.comp_name
            貸方科目3 = data.subject?.s_name
            貸方銀行3 = data.bank?.bank_name
            貸方公司3 = data.company?.comp_name
            貸方金額 = If(data.col_credit_amount_1, 0)
            貸方金額2 = If(data.col_credit_amount_2, 0)
            貸方金額3 = If(data.col_credit_amount_3, 0)
            帳款月份2 = data.col_AccountMonth2.Value.ToString("yyyy/MM")
            帳款月份3 = data.col_AccountMonth3.Value.ToString("yyyy/MM")
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
        End Try
    End Sub
End Class
