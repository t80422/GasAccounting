''' <summary>
''' 科目平衡表
''' </summary>
Public Class AccountBalanceDTO
    Public Property SubjectName As String

    ''' <summary>
    ''' 借方明細集合
    ''' </summary>
    ''' <returns></returns>
    Public Property DebitList As List(Of SubjectTransaction)

    ''' <summary>
    ''' 貸方明細集合
    ''' </summary>
    ''' <returns></returns>
    Public Property CreditList As List(Of SubjectTransaction)
End Class


''' <summary>
''' 科目交易明細
''' </summary>
Public Class SubjectTransaction
    Public Property Day As Date
    Public Property Amount As Double
End Class
