Public Interface IPurchaseView
    Sub GetUserInput()
    Function GetSearchCondition() As PurchaseCondition
    Sub SetCompanyCmb(items As List(Of ComboBoxItems))
    Sub SetGasVendorCmb(items As List(Of ComboBoxItems))
    Sub SetDriveVendorCmb(items As List(Of ComboBoxItems))
    Sub SetSubjectCmb(items As List(Of ComboBoxItems))
    Sub SetDefaultPrice(unitPrice As Single, DeliveryUnitPrice As Single)
    Sub ShowList(datas As List(Of PurchaseVM))
    Sub ClearInput()
    Sub SetDataToControls(data As purchase)
    Property CurrentPurchase As purchase
End Interface
