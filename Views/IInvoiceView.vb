Public Interface IInvoiceView
    Inherits IBaseView(Of invoice, InvioceVM)
    Sub SetCustomer(data As customer)
    Function GetSearchCriteria() As InvoiceSearchCriteria
    Sub DisplayPrices(unitPrice As Single, tax As Single, amount As Single)
    Sub DisplayInvoiceInfo(info As InvoiceInfoVM)
End Interface
