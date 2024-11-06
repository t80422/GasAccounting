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
        'Public Property Barrel_50KG As Integer
        'Public Property Barrel_20KG As Integer
        'Public Property Barrel_16KG As Integer
        'Public Property Barrel_10KG As Integer
        'Public Property Barrel_4KG As Integer
        'Public Property Barrel_18KG As Integer
        'Public Property Barrel_14KG As Integer
        'Public Property Barrel_5KG As Integer
        'Public Property Barrel_2KG As Integer
    End Class
End Class
