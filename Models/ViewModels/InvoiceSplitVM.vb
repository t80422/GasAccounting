Public Class InvoiceSplitVM
    Public Property 編號 As Integer
    Public Property 類型 As String
    Public Property 日期 As Date
    Public Property 發票號碼 As String
    Public Property 品名 As String
    Public Property 稅金 As Single
    Public Property 金額 As Single
    Public Property 廠商統編 As String
    Public Property 公司 As String

    Public Sub New(data As invoice_split)
        編號 = data.is_Id
        日期 = data.is_Date
        發票號碼 = data.is_Number
        品名 = data.is_Name
        稅金 = data.is_Tax
        金額 = data.is_Amount
        廠商統編 = data.is_VendorTaxId
        類型 = data.is_Type
        公司 = data.company?.comp_name
    End Sub
End Class
