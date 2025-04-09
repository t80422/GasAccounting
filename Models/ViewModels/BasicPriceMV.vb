Public Class BasicPriceMV
    Public Property 編號 As Integer
    Public Property 年月份 As String
    Public Property 普氣進氣價格 As Single
    Public Property 丙氣進氣價格 As Single
    Public Property 普氣銷售價格 As Single
    Public Property 丙氣銷售價格 As Single
    Public Property 普氣廠運價格 As Single
    Public Property 丙氣廠運價格 As Single

    Public Sub New(data As basic_price)
        編號 = data.bp_id
        年月份 = data.bp_date.ToShortDateString
        普氣進氣價格 = data.bp_normal_in
        丙氣進氣價格 = data.bp_c_in
        普氣銷售價格 = data.bp_normal_out
        丙氣銷售價格 = data.bp_c_out
        普氣廠運價格 = data.bp_Delivery_Normal
        丙氣廠運價格 = data.bp_Delivery_C
    End Sub
End Class
