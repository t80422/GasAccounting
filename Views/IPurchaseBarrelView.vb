Public Interface IPurchaseBarrelView
    Inherits IBaseView(Of purchase_barrel, PurchaseBarrelVM)

    Event AddRequested As EventHandler
    Event UpdateRequested As EventHandler
    Event DeleteRequested As EventHandler
    Event CancelRequested As EventHandler
    Event DetailRequested As EventHandler(Of Integer)
    Event PrintRequested As EventHandler(Of Object)

    Sub SetVendorCmb(data As List(Of SelectListItem))
    Sub SetCompanyCmb(data As List(Of SelectListItem))
    Function GetSearchCriteria() As PurBarrelSC
    Sub SetButton(isSelectedRow As Boolean)
End Interface
