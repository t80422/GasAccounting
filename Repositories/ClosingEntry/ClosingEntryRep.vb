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

    Public Function GetTarnsferSubpoenaData(day As Date) As List(Of TransferSubpoenaDTO) Implements IClosingEntryRep.GetTarnsferSubpoenaData
        Try
            Dim query = _dbSet.Where(Function(x) x.ce_Date = day).ToList

            Dim result = query.
                Select(Function(x) New TransferSubpoenaDTO With {
                    .CreditAmount = x.ce_CreditAmount,
                    .CreditSubjectName = x.subject.s_name,
                    .CreditSummary = x.ce_CreditMemo,
                    .DebitAmount = x.ce_DebitAmount,
                    .DebitSubjectName = x.subject1.s_name,
                    .DebitSummary = x.ce_DebitMemo
                }).ToList
            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
