Public Interface IInvoiceRep
    Inherits IRepository(Of invoice)

    Function SearchAsync(criteria As InvoiceSearchCriteria) As Task(Of List(Of invoice))
    Function GetByMonth(month As Date) As List(Of invoice)
End Interface
