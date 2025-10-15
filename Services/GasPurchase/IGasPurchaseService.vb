Public Interface IGasPurchaseService
    Function GetPurchaseTradeSummary(datas As List(Of purchase), criteria As PurchaseCondition) As Tuple(Of List(Of PurchaseGasVendorTradeSummaryListVM), List(Of PurchaseFreightTradeSummaryListVM))
End Interface
