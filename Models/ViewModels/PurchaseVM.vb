Public Class PurchaseVM
    Public Property 編號 As Integer
    Public Property 廠商 As String
    Public Property 日期 As Date
    Public Property 產品 As String
    Public Property 重量 As Integer
    Public Property 單價 As Single
    Public Property 金額 As Single
    Public Property 公司 As String
    Public Property 運費單價 As Single
    Public Property 運費 As Single
    Public Property 特殊單價 As Boolean
    Public Property 特殊運費 As Boolean
    Public Property 運輸公司 As String

    Public Sub New(data As purchase)
        編號 = data.pur_id
        廠商 = data.manufacturer?.manu_name
        日期 = data.pur_date
        產品 = data.pur_product
        重量 = data.pur_quantity
        單價 = data.pur_unit_price
        金額 = data.pur_price
        公司 = data.company?.comp_name
        運費單價 = data.pur_deli_unit_price
        運費 = data.pur_delivery_fee
        特殊單價 = data.pur_SpecialUP
        特殊運費 = data.pur_SpecialDUP
        運輸公司 = data.manufacturer1?.manu_name
    End Sub

    Public Sub New()

    End Sub
End Class