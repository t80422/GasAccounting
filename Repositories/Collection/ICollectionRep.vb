Public Interface ICollectionRep
    Inherits IRepository(Of collection)

    Function GetList(Optional criteria As CollectionSearchCriteria = Nothing) As List(Of CollectionVM)
    Function GetByBankAndMonthAsync(bankId As Integer, month As Date) As Task(Of IEnumerable(Of collection))
    Function GetSubpoenaData(day As Date, Optional isCash As Boolean = False) As List(Of SubpoenaDTO)
End Interface