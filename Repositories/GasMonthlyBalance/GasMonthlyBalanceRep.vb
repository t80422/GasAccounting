Public Class GasMonthlyBalanceRep
    Inherits Repository(Of gas_monthly_balances)
    Implements IGasMonthlyBalanceRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetByMonthAndCompany(month As Date, compId As Integer) As gas_monthly_balances Implements IGasMonthlyBalanceRep.GetByMonthAndCompany
        Try
            Return _dbSet.FirstOrDefault(Function(x) x.gmb_Month.Year = month.Year AndAlso x.gmb_Month.Month = month.Month And x.gmb_comp_Id = compId)
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
