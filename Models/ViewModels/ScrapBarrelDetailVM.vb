Public Class ScrapBarrelDetailVM
    Public Property 編號 As String
    Public Property 客戶代號 As String
    Public Property 客戶名稱 As String
    Public Property 數量50 As Integer
    Public Property 數量20 As Integer
    Public Property 數量16 As Integer
    Public Property 數量10 As Integer
    Public Property 數量4 As Integer

    Public Sub New(data As scrap_barrel_detail)
        編號 = data.sbd_Id
        客戶代號 = data.customer.cus_code
        客戶名稱 = data.customer.cus_name
        數量50 = data.sbd_Qty50
        數量20 = data.sbd_Qty20
        數量16 = data.sbd_Qty16
        數量10 = data.sbd_Qty10
        數量4 = data.sbd_Qty4
    End Sub
End Class
