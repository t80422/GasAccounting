Public Interface IInvoiceSplitView
    Inherits IBaseView(Of invoice_split, InvoiceSplitVM)

    Function GetSearchCriteria() As InvoiceSplitSearchCriteria

    Sub SetCompanyCmb(datas As List(Of SelectListItem))
End Interface
