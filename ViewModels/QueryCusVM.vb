Public Class QueryCusVM
    Public Property 代號 As String
    Public Property 名稱 As String
    Public Property 編號 As Integer

    Public Sub New(customer As customer)
        代號 = customer.cus_code
        名稱 = customer.cus_name
        編號 = customer.cus_id
    End Sub
End Class