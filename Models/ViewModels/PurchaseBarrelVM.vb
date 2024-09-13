Public Class PurchaseBarrelVM
    Public Property 編號 As Integer
    Public Property 日期 As String
    Public Property 廠商名稱 As String
    Public Property 總計 As Integer
    Public Property 數量_50 As Integer
    Public Property 單價_50 As Integer
    Public Property 數量_20 As Integer
    Public Property 單價_20 As Integer
    Public Property 數量_16 As Integer
    Public Property 單價_16 As Integer
    Public Property 數量_10 As Integer
    Public Property 單價_10 As Integer
    Public Property 數量_4 As Integer
    Public Property 單價_4 As Integer
    Public Property 數量_15 As Integer
    Public Property 單價_15 As Integer
    Public Property 數量_14 As Integer
    Public Property 單價_14 As Integer
    Public Property 數量_5 As Integer
    Public Property 單價_5 As Integer
    Public Property 數量_2 As Integer
    Public Property 單價_2 As Integer

    Public Sub New(data As purchase_barrel)
        編號 = data.pb_Id
        日期 = data.pb_Date
        廠商名稱 = data.manufacturer.manu_name
        總計 = data.pb_Amount
        數量_50 = data.pb_Qty_50
        單價_50 = data.pb_UnitPrice_50
        數量_20 = data.pb_Qty_20
        單價_20 = data.pb_UnitPrice_20
        數量_16 = data.pb_Qty_16
        單價_16 = data.pb_UnitPrice_16
        數量_10 = data.pb_Qty_10
        單價_10 = data.pb_UnitPrice_10
        數量_4 = data.pb_Qty_4
        單價_4 = data.pb_UnitPrice_4
        數量_15 = data.pb_Qty_15
        單價_15 = data.pb_UnitPrice_15
        數量_14 = data.pb_Qty_14
        單價_14 = data.pb_UnitPrice_14
        數量_5 = data.pb_Qty_5
        單價_5 = data.pb_UnitPrice_5
        數量_2 = data.pb_Qty_2
        單價_2 = data.pb_UnitPrice_2
    End Sub
End Class
