Imports System.Data.Entity

Public Class CarMV
    Public Property 編號 As Integer
    Public Property 客戶名稱 As String
    Public Property 車號 As String
    Public Property 司機名 As String
    Public Property 寄桶結存瓶_50KG As String
    Public Property 寄桶結存瓶_20KG As String
    Public Property 寄桶結存瓶_16KG As String
    Public Property 寄桶結存瓶_10KG As String
    Public Property 寄桶結存瓶_4KG As String
    Public Property 寄桶結存瓶_15KG As String
    Public Property 寄桶結存瓶_14KG As String
    Public Property 寄桶結存瓶_5KG As String
    Public Property 寄桶結存瓶_2KG As String

    Public Shared Function GetCarList(data As IQueryable(Of car)) As List(Of CarMV)
        Dim carList = data.Select(Function(car) New CarMV With {
            .編號 = car.c_id,
            .客戶名稱 = car.customer.cus_name,
            .車號 = car.c_no,
            .司機名 = car.c_driver,
            .寄桶結存瓶_50KG = car.c_deposit_50,
            .寄桶結存瓶_20KG = car.c_deposit_20,
            .寄桶結存瓶_16KG = car.c_deposit_16,
            .寄桶結存瓶_10KG = car.c_deposit_10,
            .寄桶結存瓶_4KG = car.c_deposit_4,
            .寄桶結存瓶_15KG = car.c_deposit_15,
            .寄桶結存瓶_14KG = car.c_deposit_14,
            .寄桶結存瓶_5KG = car.c_deposit_5,
            .寄桶結存瓶_2KG = car.c_deposit_2
        })
        Return carList.ToList
    End Function
End Class
