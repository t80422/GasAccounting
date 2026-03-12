Public Class ScrapBarrelDetailVM
    Public Property 編號 As String
    Public Property 客戶代號 As String
    Public Property 客戶名稱 As String
    Public Property 數量50 As Integer
    Public Property 數量20 As Integer
    Public Property 數量16 As Integer
    Public Property 數量10 As Integer
    Public Property 數量4 As Integer
    Public Property 買價合計 As Decimal
    Public Property 收購合計 As Decimal

    Public Sub New(data As scrap_barrel_detail)
        編號 = data.sbd_Id
        客戶代號 = data.customer.cus_code
        客戶名稱 = data.customer.cus_name
        數量50 = data.sbd_Qty50
        數量20 = data.sbd_Qty20
        數量16 = data.sbd_Qty16
        數量10 = data.sbd_Qty10
        數量4 = data.sbd_Qty4

        Dim sb = data.scrap_barrel
        If sb IsNot Nothing Then
            買價合計 = (sb.sb_Buy50 * data.sbd_Qty50) +
                       (sb.sb_Buy20 * data.sbd_Qty20) +
                       (sb.sb_Buy16 * data.sbd_Qty16) +
                       (sb.sb_Buy10 * data.sbd_Qty10) +
                       (sb.sb_Buy4 * data.sbd_Qty4)
            收購合計 = (sb.sb_Acquisitions50 * data.sbd_Qty50) +
                       (sb.sb_Acquisitions20 * data.sbd_Qty20) +
                       (sb.sb_Acquisitions16 * data.sbd_Qty16) +
                       (sb.sb_Acquisitions10 * data.sbd_Qty10) +
                       (sb.sb_Acquisitions4 * data.sbd_Qty4)
        End If
    End Sub
End Class
