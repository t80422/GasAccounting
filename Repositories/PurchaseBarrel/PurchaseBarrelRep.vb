Imports System.Data.Entity

Public Class PurchaseBarrelRep
    Inherits Repository(Of purchase_barrel)
    Implements IPurchaseBarrelRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function SearchAsync(criteria As PurBarrelSC) As Task(Of List(Of purchase_barrel)) Implements IPurchaseBarrelRep.SearchAsync
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If criteria IsNot Nothing Then
                If criteria.VendorId.HasValue Then query = query.Where(Function(x) x.pb_manu_Id = criteria.VendorId)
                If criteria.IsDate Then
                    Dim endDate = criteria.EndDate.AddDays(1)
                    query = query.Where(Function(x) x.pb_Date >= criteria.StartDate)
                    query = query.Where(Function(x) x.pb_Date < endDate)
                End If
            End If

            Return Await query.OrderByDescending(Function(x) x.pb_Date).ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetByMonthAsync(month As Date) As Task(Of List(Of purchase_barrel)) Implements IPurchaseBarrelRep.GetByMonthAsync
        Try
            Return Await _dbSet.Where(Function(x) x.pb_Date.Year = month.Year AndAlso x.pb_Date.Month = month.Month).ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
