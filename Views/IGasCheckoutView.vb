Public Interface IGasCheckoutView
    Function GetUserInput() As PurchaseCondition
    Sub ShowList(datas As List(Of PurchaseVM))
    Sub ClearInput()
    Sub LoadVendors(datas As List(Of SelectListItem))
    Function GetSelectedIds() As List(Of Integer)

    Event QueryClicked()
    Event CheckoutClicked()
    Event CancelClicked()
End Interface
