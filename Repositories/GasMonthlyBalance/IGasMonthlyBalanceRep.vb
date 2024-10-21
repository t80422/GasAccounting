Public Interface IGasMonthlyBalanceRep
    Inherits IRepository(Of gas_monthly_balances)

    Function GetByMonthAndCompany(month As Date, compId As Integer) As gas_monthly_balances
End Interface
