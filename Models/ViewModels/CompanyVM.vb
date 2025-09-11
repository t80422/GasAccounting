Public Class CompanyVM
    Public Property 編號 As Integer
    Public Property 名稱 As String
    Public Property 簡稱 As String
    Public Property 統編 As String
    Public Property 備註 As String

    Public Sub New(data As company)
        編號 = data.comp_id
        名稱 = data.comp_name
        簡稱 = data.comp_short
        統編 = data.comp_tax_id
        備註 = data.comp_memo
    End Sub
End Class
