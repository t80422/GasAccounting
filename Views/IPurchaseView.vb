Public Interface IPurchaseView
    ' 事件
    Event AddClicked As EventHandler
    Event EditClicked As EventHandler
    Event DeleteClicked As EventHandler
    Event CancelClicked As EventHandler
    Event SerchClicked As EventHandler
    Event PrintClicked As EventHandler(Of Object)
    Event GasVenderSelected As EventHandler(Of Object)
    Event RowSelected As EventHandler(Of Integer)

    Function GetInput() As purchase
    Function GetSearchCondition() As PurchaseCondition
    Sub SetCompanyCmb(items As List(Of SelectListItem))
    Sub SetGasVendorCmb(items As List(Of SelectListItem))
    Sub SetDriveVendorCmb(items As List(Of SelectListItem))
    Sub SetDefaultPrice(unitPrice As Single, DeliveryUnitPrice As Single)
    Sub ShowList(datas As List(Of PurchaseVM))
    Sub ClearInput()
    Sub SetDataToControls(data As purchase)
    Sub SetButton(isSelectRow)
End Interface
