Public Interface ICollectionRep
    Inherits IRepository(Of collection)

    Function GetList(Optional criteria As CollectionSearchCriteria = Nothing) As List(Of CollectionVM)
    Function GetCashSubpoenaData(day As Date) As List(Of CashSubpoenaDTO)
    Function GetTarnsferSubpoenaData(day As Date) As List(Of TransferSubpoenaDTO)
    Function GetBankDepositsByDateRangeAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of IEnumerable(Of collection))
    Function GetCashToBankTransfersByDateRangeAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of IEnumerable(Of collection))

    ''' <summary>
    ''' 取得銀行帳 (存、取)
    ''' </summary>
    ''' <param name="bankId"></param>
    ''' <returns></returns>
    Function GetBankAccount(bankId As Integer) As IEnumerable(Of collection)

    ''' <summary>
    ''' 取得該帳款月份的銀行存款
    ''' </summary>
    ''' <param name="bankId"></param>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetBankDepositsByAccountMonth(bankId As Integer, month As Date) As IEnumerable(Of collection)

    ''' <summary>
    ''' 取得該帳款月份的銀行提款
    ''' </summary>
    ''' <param name="bankId"></param>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetCashToBankTransfersByAccountMonth(bankId As Integer, month As Date) As IEnumerable(Of collection)
End Interface