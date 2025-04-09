
''' <summary>
''' 傳票
''' </summary>
Public Class Subpoena
    ''' <summary>
    ''' 借方明細項目集合
    ''' </summary>
    ''' <returns></returns>
    Public Property DebitItems As List(Of TransactionItem)

    ''' <summary>
    ''' 貸方明細項目集合
    ''' </summary>
    ''' <returns></returns>
    Public Property CreditItems As List(Of TransactionItem)

    Public ReadOnly Property TotalDebitAmount As Integer
        Get
            Return If(DebitItems IsNot Nothing, DebitItems.Sum(Function(x) x.Amount), 0)
        End Get
    End Property

    Public ReadOnly Property TotalCreditAmount As Integer
        Get
            Return If(CreditItems IsNot Nothing, CreditItems.Sum(Function(x) x.Amount), 0)
        End Get
    End Property
End Class