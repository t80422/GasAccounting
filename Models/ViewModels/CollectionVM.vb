Public Class CollectionVM
    Public Property 編號 As Integer
    Public Property 日期 As Date
    Public Property 帳款月份 As String
    Public Property 金額 As Integer
    Public Property 收款類型 As String
    Public Property 支票號碼 As String
    Public Property 客戶名稱 As String
    Public Property 科目 As String
    Public Property 銀行名稱 As String
    Public Property 公司 As String
    Public Property 未銷帳金額 As Integer
    Public Property 備註 As String

    Public Sub New(data As collection)
        編號 = data.col_Id
        日期 = data.col_Date
        帳款月份 = data.col_AccountMonth.ToString("yyyy/MM")
        金額 = data.col_Amount
        收款類型 = data.col_Type
        支票號碼 = data.col_Cheque
        客戶名稱 = data.customer?.cus_name
        科目 = data.subject.s_name
        銀行名稱 = data.bank?.bank_name
        公司 = data.company?.comp_name
        備註 = data.col_Memo
        未銷帳金額 = data.col_UnmatchedAmount
    End Sub
End Class
