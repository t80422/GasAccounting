Public Class CusGetGas
    Public Property 客戶名稱 As String
    Public Property 普氣50Kg As Integer
    Public Property 普氣20Kg As Integer
    Public Property 普氣16Kg As Integer
    Public Property 普氣10Kg As Integer
    Public Property 普氣4Kg As Integer
    Public Property 普氣18Kg As Integer
    Public Property 普氣14Kg As Integer
    Public Property 普氣5Kg As Integer
    Public Property 普氣2Kg As Integer
    Public Property 普氣殘氣 As Integer
    Public Property 普氣提量 As Integer

    Public ReadOnly Property 普氣實提量 As Integer
        Get
            Return 普氣提量 - 普氣殘氣
        End Get
    End Property


    Public Property 丙氣50Kg As Integer
    Public Property 丙氣20Kg As Integer
    Public Property 丙氣16Kg As Integer
    Public Property 丙氣10Kg As Integer
    Public Property 丙氣4Kg As Integer
    Public Property 丙氣18Kg As Integer
    Public Property 丙氣14Kg As Integer
    Public Property 丙氣5Kg As Integer
    Public Property 丙氣2Kg As Integer
    Public Property 丙氣殘氣 As Integer
    Public Property 丙氣提量 As Integer

    Public ReadOnly Property 丙氣實提量 As Integer
        Get
            Return 丙氣提量 - 丙氣殘氣
        End Get
    End Property
End Class
