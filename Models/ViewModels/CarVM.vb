Public Class CarVM
    Inherits GasBarrelQtyDTO
    Public Property 編號 As Integer
    Public Property 客戶名稱 As String
    Public Property 車號 As String
    Public Property 司機名 As String
    'Public Property 寄桶結存瓶_50KG As Integer
    'Public Property 寄桶結存瓶_20KG As Integer
    'Public Property 寄桶結存瓶_16KG As Integer
    'Public Property 寄桶結存瓶_10KG As Integer
    'Public Property 寄桶結存瓶_4KG As Integer
    'Public Property 寄桶結存瓶_18KG As Integer
    'Public Property 寄桶結存瓶_14KG As Integer
    'Public Property 寄桶結存瓶_5KG As Integer
    'Public Property 寄桶結存瓶_2KG As Integer

    Public Sub New(entity As car)
        編號 = entity.c_id
        客戶名稱 = entity.customer?.cus_name
        車號 = entity.c_no
        司機名 = entity.c_driver
        瓦斯瓶50Kg = entity.c_deposit_50
        瓦斯瓶20Kg = entity.c_deposit_20
        瓦斯瓶16Kg = entity.c_deposit_16
        瓦斯瓶10KG = entity.c_deposit_10
        瓦斯瓶4Kg = entity.c_deposit_4
        瓦斯瓶18Kg = entity.c_deposit_18
        瓦斯瓶14Kg = entity.c_deposit_14
        瓦斯瓶5Kg = entity.c_deposit_5
        瓦斯瓶2Kg = entity.c_deposit_2
    End Sub
End Class
