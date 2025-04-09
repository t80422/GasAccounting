Public Interface ICollectionRep
    Inherits IRepository(Of collection)

    Function Search(criteria As CollectionSearchCriteria) As List(Of CollectionVM)
    Function GetByBankAndMonthAsync(bankId As Integer, month As Date) As Task(Of IEnumerable(Of collection))
    Function GetTransferSubpoenaData(day As Date, Optional isIncome As Boolean = False) As Subpoena
End Interface