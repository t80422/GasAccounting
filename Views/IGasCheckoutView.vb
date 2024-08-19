Public Interface IGasCheckoutView
    Function GetUserInput() As PurchaseCondition
    Sub ShowList(datas As List(Of PurchaseVM))
    Sub ClearInput()
    Sub ShowMessage(message As String)
    Sub LoadVendors(datas As List(Of ComboBoxItems))
    Sub RefreshView()
    Function GetSelectedIds() As List(Of Integer)

    Event QueryClicked()
    Event CheckoutClicked()
    Event CancelClicked()
End Interface
