''' <summary>
''' 客戶寄桶結存瓶
''' </summary>
Public Class CustomerGasCylinderInventory
    Public Property CustomerName As String
    Public Property List As List(Of DepositList)

    Public Class DepositList
        Inherits GasBarrelQtyDTO
        Public Property CarNo As String
        Public Property DriverName As String
    End Class
End Class
