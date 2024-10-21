Public Class Insurance
    Public Property CompanyName As String
    Public Property Month As Date
    Public Property List As List(Of InsuranceList)
End Class

Public Class InsuranceList
    Public Property CusCode As String
    Public Property CusName As String
    Public Property TaxId As String
    Public Property Amount As Single
End Class