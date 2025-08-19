Imports System.Data.Entity

Public Class ChequePayRep
    Inherits Repository(Of chque_pay)
    Implements IChequePayRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetByChequeNumber(chequeNumber As String) As chque_pay Implements IChequePayRep.GetByChequeNumber
        Try
            Return _dbSet.FirstOrDefault(Function(x) x.cp_Number = chequeNumber)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function Search(criteria As ChequeSC) As Task(Of List(Of chque_pay)) Implements IChequePayRep.Search
        Try
            Dim query = _dbSet.AsNoTracking().AsQueryable()

            If criteria.IsDate Then query = query.Where(Function(x) x.cp_Date >= criteria.StartDate AndAlso x.cp_Date <= criteria.EndDate)
            If Not String.IsNullOrEmpty(criteria.Status) Then
                If criteria.Status = "已兌現" Then
                    query = query.Where(Function(x) x.cp_IsCashing.HasValue AndAlso x.cp_IsCashing.Value)
                End If
            End If

            Return Await query.ToListAsync()
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
