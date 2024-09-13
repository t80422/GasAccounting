Public Interface IInvoiceRep
    Inherits IRepository(Of invoice)

    Function SearchAsync(criteria As InvoiceSearchCriteria) As Task(Of List(Of invoice))
End Interface
