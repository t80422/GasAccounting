Imports System.Data.Entity

Public Class ClosingEntryRep
    Inherits Repository(Of closing_entry)
    Implements IClosingEntryRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function Search(Optional criteria As ClosingEntrySC = Nothing) As List(Of ClosingEntryVM) Implements IClosingEntryRep.Search
        Dim query = _dbSet.AsNoTracking.AsQueryable()

        If criteria IsNot Nothing Then
            If criteria.IsDate Then query = query.Where(Function(x) x.ce_Date.Value >= criteria.StartDate.Value And x.ce_Date < criteria.EndDate.Value)
            If criteria.SubjectId <> 0 Then query = query.Where(Function(x) x.ce_Credit = criteria.SubjectId Or x.ce_Debit = criteria.SubjectId)
        End If

        Dim result = query.OrderByDescending(Function(x) x.ce_Date).ToList

        Return result.Select(Function(x) New ClosingEntryVM(x)).ToList()
    End Function
End Class
