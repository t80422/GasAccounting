Public Interface IBarrelInventoryService
    Function ApplyPurchaseAsync(gasBarrelRep As IGasBarrelRep, purchase As purchase_barrel) As Task
    Function ApplyUpdateAsync(gasBarrelRep As IGasBarrelRep, originalPurchase As purchase_barrel, updatedPurchase As purchase_barrel) As Task
    Function ApplyDeleteAsync(gasBarrelRep As IGasBarrelRep, purchase As purchase_barrel) As Task

    ' 訂單新瓶出庫：扣除庫存
    Function ApplyOrderIssueAsync(gasBarrelRep As IGasBarrelRep, ord As order) As Task
    Function ApplyOrderIssueUpdateAsync(gasBarrelRep As IGasBarrelRep, originalOrder As order, updatedOrder As order) As Task
    Function ApplyOrderIssueDeleteAsync(gasBarrelRep As IGasBarrelRep, ord As order) As Task
End Interface

