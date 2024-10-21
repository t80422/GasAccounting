Public Class IncomeStatement
    Public Property CompanyName As String
    Public Property DateRange As String
    Public Property List As List(Of IncomeStatementList)
End Class

Public Class IncomeStatementList
    Public Property SubjectType As String
    Public Property Subject As String
    Public Property Amount As Single
End Class