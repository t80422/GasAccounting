Public Interface IAccountingEntryService
    ' 舊方法（使用構造函數注入的 Repository）
    Sub AddEntries(entries As List(Of accounting_entry))
    Sub UpdateEntries(entries As List(Of accounting_entry))
    Sub DeleteEntries(transactionType As String, transactionId As Integer)

    ' 新方法（接受 Repository 參數，給 UnitOfWork 使用）
    Sub AddEntries(aeRep As IAccountingEntryRep, entries As List(Of accounting_entry))
    Sub UpdateEntries(aeRep As IAccountingEntryRep, entries As List(Of accounting_entry))
    Sub DeleteEntries(aeRep As IAccountingEntryRep, transactionType As String, transactionId As Integer)
End Interface
