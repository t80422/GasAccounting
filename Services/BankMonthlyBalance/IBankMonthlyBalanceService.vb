Public Interface IBankMonthlyBalanceService
    ' 新方法（接受 Repository 參數，給 UnitOfWork 使用）
    Function UpdateMonthBalanceAsync(bmbRep As IBankMonthlyBalancesRep, bankRep As IBankRep, paymentRep As IPaymentRep, collectionRep As ICollectionRep, bankId As Integer, inputMonth As Date) As Task

    ''' <summary>
    ''' 增量更新銀行月結餘額（Delta Update）
    ''' </summary>
    ''' <param name="bmbRep">銀行月結餘額 Repository</param>
    ''' <param name="bankRep">銀行 Repository</param>
    ''' <param name="bankId">銀行 ID</param>
    ''' <param name="transactionMonth">交易月份</param>
    ''' <param name="creditDelta">支出變化量（payment 用，正數表示增加支出，負數表示減少支出）</param>
    ''' <param name="debitDelta">收入變化量（collection 用，正數表示增加收入，負數表示減少收入）</param>
    ''' <returns></returns>
    Function UpdateMonthBalanceIncrementalAsync(bmbRep As IBankMonthlyBalancesRep, bankRep As IBankRep, bankId As Integer, transactionMonth As Date, creditDelta As Decimal, debitDelta As Decimal) As Task

    ''' <summary>
    ''' 重新計算指定銀行的所有月結餘額（從頭開始全量計算）
    ''' </summary>
    ''' <param name="bmbRep">銀行月結餘額 Repository</param>
    ''' <param name="bankRep">銀行 Repository</param>
    ''' <param name="paymentRep">付款 Repository</param>
    ''' <param name="collectionRep">收款 Repository</param>
    ''' <param name="bankId">銀行 ID</param>
    ''' <returns></returns>
    Function RecalculateBankBalancesAsync(bmbRep As IBankMonthlyBalancesRep, bankRep As IBankRep, paymentRep As IPaymentRep, collectionRep As ICollectionRep, bankId As Integer) As Task

    ''' <summary>
    ''' 重新計算所有銀行的月結餘額（維護工具）
    ''' </summary>
    ''' <param name="bmbRep">銀行月結餘額 Repository</param>
    ''' <param name="bankRep">銀行 Repository</param>
    ''' <param name="paymentRep">付款 Repository</param>
    ''' <param name="collectionRep">收款 Repository</param>
    ''' <returns>處理結果訊息</returns>
    Function RecalculateAllBanksAsync(bmbRep As IBankMonthlyBalancesRep, bankRep As IBankRep, paymentRep As IPaymentRep, collectionRep As ICollectionRep) As Task(Of String)
End Interface