Public Interface ICollectionRep
    Inherits IRepository(Of collection)

    Function GetList(Optional criteria As CollectionSearchCriteria = Nothing) As List(Of CollectionVM)
    Function GetByBankAndMonthAsync(bankId As Integer, month As Date) As Task(Of IEnumerable(Of collection))
    Function GetTransferSubpoenaData(day As Date, Optional isIncome As Boolean = False) As Subpoena
End Interface