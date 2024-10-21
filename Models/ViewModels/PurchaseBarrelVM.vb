Public Class PurchaseBarrelVM
    Public Property 編號 As Integer
    Public Property 日期 As String
    Public Property 廠商名稱 As String
    Public Property 公司 As String
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
    Public Property 數量_18 As Integer
    Public Property 單價_18 As Integer
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
        公司 = data.company?.comp_name
        總計 = data.pb_Amount
        數量_50 = If(data.pb_Qty_50, 0)
        單價_50 = If(data.pb_UnitPrice_50, 0)
        數量_20 = If(data.pb_Qty_20, 0)
        單價_20 = If(data.pb_UnitPrice_20, 0)
        數量_16 = If(data.pb_Qty_16, 0)
        單價_16 = If(data.pb_UnitPrice_16, 0)
        數量_10 = If(data.pb_Qty_10, 0)
        單價_10 = If(data.pb_UnitPrice_10, 0)
        數量_4 = If(data.pb_Qty_4, 0)
        單價_4 = If(data.pb_UnitPrice_4, 0)
        數量_18 = If(data.pb_Qty_18, 0)
        單價_18 = If(data.pb_UnitPrice_18, 0)
        數量_14 = If(data.pb_Qty_14, 0)
        單價_14 = If(data.pb_UnitPrice_14, 0)
        數量_5 = If(data.pb_Qty_5, 0)
        單價_5 = If(data.pb_UnitPrice_5, 0)
        數量_2 = If(data.pb_Qty_2, 0)
        單價_2 = If(data.pb_UnitPrice_2, 0)
    End Sub
End Class
