Public Class AccountingEntryService
    Implements IAccountingEntryService

    Private ReadOnly _aeRep As IAccountingEntryRep

    Public Sub New(aeRep As IAccountingEntryRep)
        _aeRep = aeRep
    End Sub

    Public Sub AddEntries(entries As List(Of accounting_entry)) Implements IAccountingEntryService.AddEntries
        Try
            '驗證借貸是否平衡
            Dim totalDebit = entries.Sum(Function(x) x.ae_Debit)
            Dim totalCredit = entries.Sum(Function(x) x.ae_Credit)

            If totalCredit <> totalDebit Then Throw New Exception($"借貸不平衡:借方合計{totalDebit},貸方合計{totalCredit}")

            entries.ForEach(Sub(x) _aeRep.AddAsync(x))
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Sub

    Public Sub UpdateEntries(entries As List(Of accounting_entry)) Implements IAccountingEntryService.UpdateEntries
        Try
            Dim firstEntry = entries.First
            Dim transactionType = firstEntry.ae_TransactionType
            Dim transactionId = firstEntry.ae_TransactionId

            '驗證借貸是否平衡
            Dim totalDebit = entries.Sum(Function(x) x.ae_Debit)
            Dim totalCredit = entries.Sum(Function(x) x.ae_Credit)

            If totalCredit <> totalDebit Then Throw New Exception($"借貸不平衡:借方合計{totalDebit},貸方合計{totalCredit}")

            _aeRep.DeleteByTransaction(transactionType, transactionId)

            entries.ForEach(Sub(x) _aeRep.AddAsync(x))
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Sub

    Public Sub DeleteEntries(transactionType As String, transactionId As Integer) Implements IAccountingEntryService.DeleteEntries
        Try
            _aeRep.DeleteByTransaction(transactionType, transactionId)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Sub

    ' === 新方法（接受 Repository 參數，給 UnitOfWork 使用） ===

    Public Sub AddEntries(aeRep As IAccountingEntryRep, entries As List(Of accounting_entry)) Implements IAccountingEntryService.AddEntries
        Try
            '驗證借貸是否平衡
            Dim totalDebit = entries.Sum(Function(x) x.ae_Debit)
            Dim totalCredit = entries.Sum(Function(x) x.ae_Credit)

            If totalCredit <> totalDebit Then Throw New Exception($"借貸不平衡:借方合計{totalDebit},貸方合計{totalCredit}")

            entries.ForEach(Sub(x) aeRep.AddAsync(x))
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Sub

    Public Sub UpdateEntries(aeRep As IAccountingEntryRep, entries As List(Of accounting_entry)) Implements IAccountingEntryService.UpdateEntries
        Try
            Dim firstEntry = entries.First
            Dim transactionType = firstEntry.ae_TransactionType
            Dim transactionId = firstEntry.ae_TransactionId

            '驗證借貸是否平衡
            Dim totalDebit = entries.Sum(Function(x) x.ae_Debit)
            Dim totalCredit = entries.Sum(Function(x) x.ae_Credit)

            If totalCredit <> totalDebit Then Throw New Exception($"借貸不平衡:借方合計{totalDebit},貸方合計{totalCredit}")

            aeRep.DeleteByTransaction(transactionType, transactionId)

            entries.ForEach(Sub(x) aeRep.AddAsync(x))
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Sub

    Public Sub DeleteEntries(aeRep As IAccountingEntryRep, transactionType As String, transactionId As Integer) Implements IAccountingEntryService.DeleteEntries
        Try
            aeRep.DeleteByTransaction(transactionType, transactionId)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Sub
End Class
