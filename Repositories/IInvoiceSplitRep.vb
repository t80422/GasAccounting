Public Interface IInvoiceSplitRep
    Inherits IRepository(Of invoice_split)

    Function Search(criteria As InvoiceSplitSearchCriteria) As List(Of invoice_split)
End Interface
