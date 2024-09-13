Public Interface IPurchaseView
    Sub GetUserInput()
    Function GetSearchCondition() As PurchaseCondition
    Sub SetCompanyCmb(items As List(Of SelectListItem))
    Sub SetGasVendorCmb(items As List(Of SelectListItem))
    Sub SetDriveVendorCmb(items As List(Of SelectListItem))
    Sub SetSubjectCmb(items As List(Of SelectListItem))
    Sub SetDefaultPrice(unitPrice As Single, DeliveryUnitPrice As Single)
    Sub ShowList(datas As List(Of PurchaseVM))
    Sub ClearInput()
    Sub SetDataToControls(data As purchase)
    Property CurrentPurchase As purchase
End Interface
