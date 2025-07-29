''' <summary>
''' 科目平衡表
''' </summary>
Public Class AccountBalanceDTO
    Public Property DebitDatas As List(Of Dictionary(Of String, List(Of SubjectTransaction)))
    Public Property CreditDatas As List(Of Dictionary(Of String, List(Of SubjectTransaction)))
End Class

''' <summary>
''' 科目交易明細
''' </summary>
Public Class SubjectTransaction
    Public Property Day As Date
    Public Property Amount As Double
End Class
