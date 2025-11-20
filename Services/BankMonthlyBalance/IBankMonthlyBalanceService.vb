Public Interface IBankMonthlyBalanceService
    ' 舊方法（使用構造函數注入的 Repository）
    Function UpdateMonthBalanceAsync(bankId As Integer, inputMonth As Date) As Task
    
    ' 新方法（接受 Repository 參數，給 UnitOfWork 使用）
    Function UpdateMonthBalanceAsync(bmbRep As IBankMonthlyBalancesRep, bankRep As IBankRep, paymentRep As IPaymentRep, collectionRep As ICollectionRep, bankId As Integer, inputMonth As Date) As Task
End Interface