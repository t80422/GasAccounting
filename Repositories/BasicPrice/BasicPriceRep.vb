Public Class BasicPriceRep
    Inherits Repository(Of basic_price)
    Implements IBasicPriceRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetByMonth(month As Date) As IEnumerable(Of basic_price) Implements IBasicPriceRep.GetByMonth
        Dim startDate = New Date(month.Year, month.Month, 1)
        Dim endDate = startDate.AddMonths(1)
        Return _dbSet.Where(Function(x) x.bp_date >= startDate AndAlso x.bp_date < endDate).ToList
    End Function
End Class
