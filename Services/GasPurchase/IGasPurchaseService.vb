Public Interface IGasPurchaseService
    ' 舊方法（使用構造函數注入的 Repository）
    Function GetPurchaseTradeSummary(datas As List(Of purchase), criteria As PurchaseCondition) As Tuple(Of List(Of PurchaseGasVendorTradeSummaryListVM), List(Of PurchaseFreightTradeSummaryListVM))

    ' 新方法（接受 Repository 參數，給 UnitOfWork 使用）
    Function GetPurchaseTradeSummary(paymentRep As IPaymentRep, datas As List(Of purchase), criteria As PurchaseCondition) As Tuple(Of List(Of PurchaseGasVendorTradeSummaryListVM), List(Of PurchaseFreightTradeSummaryListVM))
End Interface
