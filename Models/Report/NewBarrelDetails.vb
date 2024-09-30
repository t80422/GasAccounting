Public Class NewBarrelDetails
    Public Property List As New List(Of NewBarrelDetailsList)
    Public Property PayUnitPrice50 As Integer
    Public Property PayUnitPrice20 As Integer
    Public Property PayUnitPrice16 As Integer
    Public Property PayUnitPrice10 As Integer
    Public Property PayUnitPrice4 As Integer
    Public Property IncomeUnitPrice50 As Integer
    Public Property IncomeUnitPrice20 As Integer
    Public Property IncomeUnitPrice16 As Integer
    Public Property IncomeUnitPrice10 As Integer
    Public Property IncomeUnitPrice4 As Integer
    Public Property Last50 As Integer
    Public Property Last20 As Integer
    Public Property Last16 As Integer
    Public Property Last10 As Integer
    Public Property Last4 As Integer
End Class

Public Class NewBarrelDetailsList
    Public Property Day As String
    Public Property In50 As Integer?
    Public Property In20 As Integer?
    Public Property In16 As Integer?
    Public Property In10 As Integer?
    Public Property In4 As Integer?
    Public Property Out50 As Integer?
    Public Property Out20 As Integer?
    Public Property Out16 As Integer?
    Public Property Out10 As Integer?
    Public Property Out4 As Integer?
    Public Property Memo As String
    Public Property PayDate As String
End Class
