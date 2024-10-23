Public Interface IAccountingEntryService
    Sub AddEntries(entries As List(Of accounting_entry))
    Sub UpdateEntries(entries As List(Of accounting_entry))
    Sub DeleteEntries(transactionType As String, transactionId As Integer)
End Interface
