Imports System.Data.Entity

Public Class BasicPriceRep
    Inherits Repository(Of basic_price)
    Implements IBasicPriceRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetByMonth(month As Date) As basic_price Implements IBasicPriceRep.GetByMonth
        Return _dbSet.FirstOrDefault(Function(x) x.bp_date.Year = month.Year AndAlso x.bp_date.Month = month.Month)
    End Function

    Public Function GetByNearestDate(day As Date) As basic_price Implements IBasicPriceRep.GetByNearestDate
        Return _dbSet.AsNoTracking.Where(Function(x) x.bp_date <= day).OrderByDescending(Function(x) x.bp_date).FirstOrDefault()
    End Function

    Public Async Function SearchAsync(criteria As basic_price) As Task(Of IEnumerable(Of basic_price)) Implements IBasicPriceRep.SearchAsync
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If criteria IsNot Nothing Then query = query.Where(Function(x) x.bp_date = criteria.bp_date)

            Return Await query.ToListAsync
        Catch
            Throw
        End Try
    End Function

    Public Async Function CheckDuplicateDateAsync(day As Date) As Task(Of Boolean) Implements IBasicPriceRep.CheckDuplicateDateAsync
        Try
            Return Await _dbSet.AnyAsync(Function(x) x.bp_date = day.Date)
        Catch
            Throw
        End Try
    End Function
End Class
