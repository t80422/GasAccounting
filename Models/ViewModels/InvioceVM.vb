Public Class InvioceVM
    Public Property 編號 As Integer
    Public Property 日期 As String
    Public Property 客戶代號 As String
    Public Property 客戶名稱 As String
    Public Property 發票號碼 As String
    Public Property KG As Integer
    Public Property 單價 As Single
    Public Property 稅 As Single
    Public Property 金額 As Single
    Public Property 作廢 As Boolean

    Public Sub New(data As invoice)
        編號 = data.i_Id
        客戶代號 = data.customer?.cus_code
        客戶名稱 = data.customer?.cus_name
        發票號碼 = data.i_Number
        日期 = data.i_Date.ToString("yyyy年MM月dd日")
        KG = data.i_KG
        單價 = data.i_UnitPrice
        稅 = data.i_Tax
        金額 = data.i_Amount
        作廢 = data.i_IsInvalid
    End Sub
End Class
