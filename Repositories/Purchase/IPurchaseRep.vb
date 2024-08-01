Public Interface IPurchaseRep
    Inherits IRepository(Of purchase)

    Function GetForCheckout(data As GasCheckoutUserInput) As List(Of PurchaseVM)
    Function UpdateCheckoutStatusAsync(datas As List(Of Integer)) As Task
End Interface