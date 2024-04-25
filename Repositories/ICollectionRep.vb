Public Interface ICollectionRep
    Sub Add(collection As collection, Optional journal As journal = Nothing, Optional cheque As cheque = Nothing)
    Sub Edit(col As collection, jour As journal, che As cheque)
    Sub Delete(id As Integer)
    Sub UpdateCheque(colId As Integer)
End Interface
