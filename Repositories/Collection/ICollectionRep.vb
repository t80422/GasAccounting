Public Interface ICollectionRep
    Inherits IRepository(Of collection)

    Sub Add(collection As collection, Optional cheque As cheque = Nothing)
    Sub Edit(col As collection, che As cheque)
    Sub UpdateCheque(colId As Integer)
    Function GetByBankAndMonthAsync(bankId As Integer, month As Date) As Task(Of IEnumerable(Of collection))
End Interface
