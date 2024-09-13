Public Class CarVM
    Public Property 編號 As Integer
    Public Property 客戶名稱 As String
    Public Property 車號 As String
    Public Property 司機名 As String
    Public Property 寄桶結存瓶_50KG As Integer
    Public Property 寄桶結存瓶_20KG As Integer
    Public Property 寄桶結存瓶_16KG As Integer
    Public Property 寄桶結存瓶_10KG As Integer
    Public Property 寄桶結存瓶_4KG As Integer
    Public Property 寄桶結存瓶_15KG As Integer
    Public Property 寄桶結存瓶_14KG As Integer
    Public Property 寄桶結存瓶_5KG As Integer
    Public Property 寄桶結存瓶_2KG As Integer

    Public Sub New(entity As car)
        編號 = entity.c_id
        客戶名稱 = entity.customer?.cus_name
        車號 = entity.c_no
        司機名 = entity.c_driver
        寄桶結存瓶_50KG = entity.c_deposit_50
        寄桶結存瓶_20KG = entity.c_deposit_20
        寄桶結存瓶_16KG = entity.c_deposit_16
        寄桶結存瓶_10KG = entity.c_deposit_10
        寄桶結存瓶_4KG = entity.c_deposit_4
        寄桶結存瓶_15KG = entity.c_deposit_15
        寄桶結存瓶_14KG = entity.c_deposit_14
        寄桶結存瓶_5KG = entity.c_deposit_5
        寄桶結存瓶_2KG = entity.c_deposit_2
    End Sub
End Class
