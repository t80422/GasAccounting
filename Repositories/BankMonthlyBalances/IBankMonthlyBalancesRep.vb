Public Interface IBankMonthlyBalancesRep
    Inherits IRepository(Of bank_monthly_balances)

    Function UpdateBankMonthlyBalancesAsync(bmb As bank_monthly_balances) As Task

    Function GetByMonthAndBankAsync(month As Date, bankId As Integer) As Task(Of bank_monthly_balances)

    ''' <summary>
    ''' 取得指定月份之後的月份餘額
    ''' </summary>
    ''' <param name="month"></param>
    ''' <param name="bankId"></param>
    ''' <returns></returns>
    Function GetSubsequentMonthAsync(month As Date, bankId As Integer) As Task(Of IEnumerable(Of bank_monthly_balances))

    ''' <summary>
    ''' 取得上期結餘
    ''' </summary>
    ''' <param name="startMonth"></param>
    ''' <param name="bankId"></param>
    ''' <returns></returns>
    Function GetLastBalanceBeforeMonthAsync(startMonth As Date, bankId As Integer) As Task(Of bank_monthly_balances)

    ''' <summary>
    ''' 取得指定銀行的所有月結餘額記錄
    ''' </summary>
    ''' <param name="bankId"></param>
    ''' <returns></returns>
    Function GetAllByBankAsync(bankId As Integer) As Task(Of IEnumerable(Of bank_monthly_balances))
End Interface
