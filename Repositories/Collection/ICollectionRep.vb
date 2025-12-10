Public Interface ICollectionRep
    Inherits IRepository(Of collection)

    Function GetList(Optional criteria As CollectionSearchCriteria = Nothing) As List(Of CollectionVM)
    Function GetByBankAndMonthAsync(bankId As Integer, month As Date) As Task(Of IEnumerable(Of collection))
    Function GetCashSubpoenaData(day As Date) As List(Of CashSubpoenaDTO)
    Function GetTarnsferSubpoenaData(day As Date) As List(Of TransferSubpoenaDTO)
    Function GetBankDepositsByDateRangeAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of IEnumerable(Of collection))
    Function GetCashToBankTransfersByDateRangeAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of IEnumerable(Of collection))
End Interface