Public Class GasBarrelVM
    Public Property 編號 As Integer
    Public Property 名稱 As String
    Public Property 初始庫存 As Integer
    Public Property 庫存 As Integer
    Public Property 銷售價格 As Integer

    Public Sub New(data As gas_barrel)
        編號 = data.gb_Id
        名稱 = data.gb_Name
        初始庫存 = data.gb_InitialInventory
        庫存 = data.gb_Inventory
        銷售價格 = data.gb_SalesPrice
    End Sub
End Class
