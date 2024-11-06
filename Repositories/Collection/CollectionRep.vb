Imports System.Data.Entity

Public Class CollectionRep
    Inherits Repository(Of collection)
    Implements ICollectionRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function Search(criteria As CollectionSearchCriteria) As List(Of CollectionVM) Implements ICollectionRep.Search
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If criteria.IsDate Then
                Dim startDate = criteria.StartDate.Value.Date
                Dim endDate = criteria.EndDate.Value.Date.AddDays(1)
                query = query.Where(Function(x) x.col_Date >= startDate AndAlso x.col_Date < endDate)
            End If

            If criteria.SubjectId.HasValue Then query = query.Where(Function(x) x.col_s_Id = criteria.SubjectId)
            If criteria.CompanyId.HasValue Then query = query.Where(Function(x) x.col_comp_Id = criteria.CompanyId)
            If criteria.BankId.HasValue Then query = query.Where(Function(x) x.col_bank_Id = criteria.BankId)
            If criteria.CusId <> 0 Then query = query.Where(Function(x) x.col_cus_Id = criteria.CusId)
            If Not String.IsNullOrEmpty(criteria.Type) Then query = query.Where(Function(x) x.col_Type = criteria.Type)
            If Not String.IsNullOrEmpty(criteria.Cheque) Then query = query.Where(Function(x) x.col_Cheque = criteria.Cheque)

            Return query.OrderByDescending(Function(x) x.col_Date).
                         ThenByDescending(Function(x) x.col_Id).ToList.
                         Select(Function(x) New CollectionVM(x)).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetByBankAndMonthAsync(bankId As Integer, month As Date) As Task(Of IEnumerable(Of collection)) Implements ICollectionRep.GetByBankAndMonthAsync
        Try
            Return Await _dbSet.AsNoTracking.Where(Function(x) x.col_AccountMonth.Year = month.Year AndAlso x.col_AccountMonth.Month = month.Month AndAlso x.col_bank_Id = bankId).
                                             ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class