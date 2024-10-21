Public Interface IPurchaseBarrelView
    Inherits IBaseView(Of purchase_barrel, PurchaseBarrelVM)

    Sub SetVendorCmb(data As List(Of SelectListItem))
    Sub SetCompanyCmb(data As List(Of SelectListItem))
    Function GetSearchCriteria() As PurBarrelSC
End Interface
