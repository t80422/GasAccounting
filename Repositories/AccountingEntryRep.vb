Public Class AccountingEntryRep
    Inherits Repository(Of accounting_entry)
    Implements IAccountingEntryRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Sub DeleteByTransaction(transactionType As String, transactionId As Integer) Implements IAccountingEntryRep.DeleteByTransaction
        Try
            Dim datas = _dbSet.Where(Function(x) x.ae_TransactionType = transactionType AndAlso x.ae_TransactionId = transactionId)
            _dbSet.RemoveRange(datas)
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class
