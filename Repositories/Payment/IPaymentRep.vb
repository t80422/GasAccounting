Public Interface IPaymentRep
    Inherits IRepository(Of payment)

    Function SearchPaymentAsync(criteria As PaymentSearchCriteria) As Task(Of IEnumerable(Of payment))
    Function GetVendorAmountDue(vendorId As Integer) As List(Of AmountDueVM)
    Function GetByBankAndMonthAsync(bankId As Integer, month As Date) As Task(Of IEnumerable(Of payment))
End Interface