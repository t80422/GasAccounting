Public Interface IGasCheckoutView
    Function GetUserInput() As GasCheckoutUserInput
    Sub ShowList(datas As List(Of PurchaseVM))
    Sub ClearInput()
    Sub ShowMessage(message As String)
    Sub LoadVendors(datas As List(Of ComboBoxItems))
    Sub RefreshView()

    Event QueryClicked()
    Event CheckoutClicked(selectedDatas As List(Of Integer))
    Event CancelClicked()
End Interface
