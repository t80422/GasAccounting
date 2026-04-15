Public Interface IBarrelMonthlyBalanceService
    Function UpdateOrAddAsync(month As Date) As Task
    Function RecalculateAllMonthsAsync(pbRep As IPurchaseBarrelRep, orderRep As IOrderRep) As Task
End Interface
