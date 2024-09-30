Public Interface IBarrelMonthlyBalancesRep
    Inherits IRepository(Of barrel_monthly_balances)

    Function GetLastClosingBalance(month As Date, gbId As Integer) As Task(Of Integer?)
    Function GetByMonthAndGbIdAsync(month As Date, gbId As Integer) As Task(Of barrel_monthly_balances)
    Function GetAfterByMonthAndGbIdAsync(month As Date, gbId As Integer) As Task(Of List(Of barrel_monthly_balances))
End Interface