Public Class ScrapBarrelVM
    Public Property 編號 As String
    Public Property 月份 As String
    Public Property 收購價50 As Integer
    Public Property 買價50 As Integer
    Public Property 收購價20 As Integer
    Public Property 買價20 As Integer
    Public Property 收購價16 As Integer
    Public Property 買價16 As Integer
    Public Property 收購價10 As Integer
    Public Property 買價10 As Integer
    Public Property 收購價4 As Integer
    Public Property 買價4 As Integer

    Public Sub New(data As scrap_barrel)
        編號 = data.sb_Id
        月份 = data.sb_Month.Value.ToString("yyyy/MM")
        收購價50 = data.sb_Acquisitions50
        買價50 = data.sb_Buy50
        收購價20 = data.sb_Acquisitions20
        買價20 = data.sb_Buy20
        收購價16 = data.sb_Acquisitions16
        買價16 = data.sb_Buy16
        收購價10 = data.sb_Acquisitions10
        買價10 = data.sb_Buy10
        收購價4 = data.sb_Acquisitions4
        買價4 = data.sb_Buy4
    End Sub
End Class
