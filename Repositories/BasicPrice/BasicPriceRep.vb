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

    Public Async Function CheckDuplicateMonthAsync(month As Date) As Task(Of Boolean) Implements IBasicPriceRep.CheckDuplicateMonthAsync
        Return Await _dbSet.AnyAsync(Function(x) x.bp_date.Year = month.Year AndAlso x.bp_date.Month = month.Month)
    End Function

    Public Async Function SearchAsync(criteria As basic_price) As Task(Of IEnumerable(Of basic_price)) Implements IBasicPriceRep.SearchAsync
        Dim query = _dbSet.AsNoTracking.AsQueryable

        If criteria IsNot Nothing Then query = query.Where(Function(x) x.bp_date.Year = criteria.bp_date.Year AndAlso x.bp_date.Month = criteria.bp_date.Month)

        Return Await query.ToListAsync
    End Function
End Class
