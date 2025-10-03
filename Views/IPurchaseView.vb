Public Interface IPurchaseView
    Inherits IFormView(Of purchase, PurchaseVM)

    ' 事件
    Event PrintClicked As EventHandler(Of Object)
    Event GasVenderSelected As EventHandler(Of Object)

    Function GetSearchCondition() As PurchaseCondition
    Sub SetCompanyCmb(items As List(Of SelectListItem))
    Sub SetGasVendorCmb(items As List(Of SelectListItem))
    Sub SetDriveVendorCmb(items As List(Of SelectListItem))
    Sub SetDefaultPrice(unitPrice As Single, DeliveryUnitPrice As Single)
    Sub ShowGasUnpaidSummary(datas As List(Of PurchaseGasVendorTradeSummaryListVM))
    Sub ShowTransportationSummary(datas As List(Of PurchaseFreightTradeSummaryListVM))
End Interface
