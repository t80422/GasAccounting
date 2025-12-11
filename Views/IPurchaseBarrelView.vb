Public Interface IPurchaseBarrelView
    Inherits IFormView(Of purchase_barrel, PurchaseBarrelVM)

    Event PrintRequested As EventHandler(Of Object)
    Sub SetVendorCmb(data As List(Of SelectListItem))
    Sub SetCompanyCmb(data As List(Of SelectListItem))
    Sub SetBarrelInventory(data As GasBarrelDTO)
    Function GetSearchCriteria() As PurBarrelSC
End Interface
