Public Class SurplusGasListVM
    Public Property 編號 As Integer
    Public Property 起始日期 As Date
    Public Property 結束日期 As Date
    Public Property 月份 As String
    Public Property 台普 As Integer
    Public Property 台丙 As Integer
    Public Property 槽普 As Integer
    Public Property 槽丙 As Integer
    Public Property 車普 As Integer
    Public Property 車丙 As Integer
    Public Property 現銷 As Integer
    Public Property 總庫存 As Integer

    Public Sub New(data As surplus_gas)
        編號 = data.sg_Id
        起始日期 = data.sg_StartDate
        結束日期 = data.sg_EndDate
        月份 = data.sg_Moth.Value.ToString("yyyy/MM")
        台普 = data.sg_Platform
        台丙 = data.sg_Platform_C
        槽普 = data.sg_Slot
        槽丙 = data.sg_Slot_C
        車普 = data.sg_Car
        車丙 = data.sg_Car_C
        現銷 = data.sg_Sell
        總庫存 = data.sg_Total
    End Sub
End Class
