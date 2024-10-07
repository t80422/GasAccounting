Public Class Tax
    Public Property Month As Date
    Public Property List As List(Of TaxList)
End Class

Public Class TaxList
    Public Property Day As Date
    Public Property InvoiceNum As String
    Public Property TaxId As String
    Public Property UnitPrice As Single
    Public Property Quantity As Integer
    Public Property Tax As Single
    Public Property Amount As Single
    Public Property Memo As String
End Class
