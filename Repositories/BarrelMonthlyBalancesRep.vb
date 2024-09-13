Imports System.Data.Entity

Public Class BarrelMonthlyBalancesRep
    Inherits Repository(Of barrel_monthly_balances)
    Implements IBarrelMonthlyBalancesRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function GetLastClosingBalance(month As Date, gbId As Integer) As Task(Of Integer?) Implements IBarrelMonthlyBalancesRep.GetLastClosingBalance
        Try
            Dim result As Integer?
            Dim data = Await _dbSet.FirstOrDefaultAsync(Function(x) x.barmb_gb_Id = gbId AndAlso x.barmb_Month < month)
            If data IsNot Nothing Then result = data.barmb_ClosingBalance
            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetByMonthAndGbIdAsync(month As Date, gbId As Integer) As Task(Of barrel_monthly_balances) Implements IBarrelMonthlyBalancesRep.GetByMonthAndGbIdAsync
        Try
            Return Await _dbSet.FirstOrDefaultAsync(Function(x) x.barmb_Month.Year = month.Year AndAlso
                                                                x.barmb_Month.Month = month.Month AndAlso
                                                                x.barmb_gb_Id = gbId)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetAfterByMonthAndGbIdAsync(month As Date, gbId As Integer) As Task(Of List(Of barrel_monthly_balances)) Implements IBarrelMonthlyBalancesRep.GetAfterByMonthAndGbIdAsync
        Try
            Dim thisMonth = New Date(month.Year, month.Month, 1).AddMonths(1)
            Return Await _dbSet.Where(Function(x) x.barmb_Month >= thisMonth AndAlso x.barmb_gb_Id = gbId).OrderBy(Function(x) x.barmb_Month).ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
