Public Interface IInvoiceView
    Inherits IBaseView(Of invoice, InvioceVM)
    Sub SetCustomer(data As customer)
    Function GetSearchCriteria() As InvoiceSearchCriteria
End Interface
