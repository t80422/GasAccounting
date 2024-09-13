Public Interface IBankMonthlyBalanceService
    Function UpdateMonthBalanceAsync(bankId As Integer, inputMonth As Date) As Task
End Interface