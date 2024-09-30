''' <summary>
''' 進銷存明細表
''' </summary>
Public Class InventoryTransactionDetail
    Public Property Company As String
    Public Property OperatorName As String
    Public Property Phone As String
    Public Property Year As String
    Public Property List As List(Of InventoryTransactionDetailList)
End Class

Public Class InventoryTransactionDetailList
    Public Property Month As Integer
    Public Property OpeningBalance As Integer
    Public Property Sale As Integer
    Public Property CloseingBalance As Integer
    Public Property PurchasesByVendor As Dictionary(Of String, Integer)
End Class