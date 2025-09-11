Public Class SurplusGasDTO
    Public Property Month As Date

    ''' <summary>
    ''' 台普
    ''' </summary>
    ''' <returns></returns>
    Public Property Platform As Integer

    ''' <summary>
    ''' 台丙
    ''' </summary>
    ''' <returns></returns>
    Public Property Platform_C As Integer

    ''' <summary>
    ''' 槽普
    ''' </summary>
    ''' <returns></returns>
    Public Property Slot As Integer

    ''' <summary>
    ''' 槽丙
    ''' </summary>
    ''' <returns></returns>
    Public Property Slot_C As Integer

    ''' <summary>
    ''' 車普
    ''' </summary>
    ''' <returns></returns>
    Public Property Car As Integer

    ''' <summary>
    ''' 車丙
    ''' </summary>
    ''' <returns></returns>
    Public Property Car_C As Integer

    ''' <summary>
    ''' 現銷
    ''' </summary>
    ''' <returns></returns>
    Public Property Sell As Integer

    ''' <summary>
    ''' 總提氣量
    ''' </summary>
    ''' <returns></returns>
    Public Property TotalOrder As Integer

    ''' <summary>
    ''' 上月結餘氣
    ''' </summary>
    ''' <returns></returns>
    Public Property LastMonthSurplus As Integer

    Public Property PurchaseDetails As List(Of PurchaseCompanyDetail)
End Class

Public Class PurchaseCompanyDetail
    Public Property CompanyName As String
    Public Property Gas As Integer
    Public Property Gas_C As Integer
End Class
