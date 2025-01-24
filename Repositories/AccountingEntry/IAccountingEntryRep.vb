Public Interface IAccountingEntryRep
    Inherits IRepository(Of accounting_entry)

    Sub DeleteByTransaction(transactionType As String, transactionId As Integer)
End Interface
