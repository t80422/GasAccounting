Public Interface ICollectionRep
    Inherits IRepository(Of collection)

    Function GetList(Optional criteria As CollectionSearchCriteria = Nothing) As List(Of CollectionVM)
    Function GetByBankAndMonthAsync(bankId As Integer, month As Date) As Task(Of IEnumerable(Of collection))
    Function GetCashSubpoenaData(day As Date) As List(Of CashSubpoenaDTO)
    Function GetTarnsferSubpoenaData(day As Date) As List(Of TransferSubpoenaDTO)
End Interface