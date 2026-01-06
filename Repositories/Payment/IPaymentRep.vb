Public Interface IPaymentRep
    Inherits IRepository(Of payment)

    Function SearchPaymentAsync(Optional criteria As PaymentSearchCriteria = Nothing) As Task(Of IEnumerable(Of payment))

    ''' <summary>
    ''' 使用查詢條件與廠商編號取得資料
    ''' </summary>
    ''' <param name="criteria">查詢條件</param>
    ''' <param name="vendorIds">廠商編號</param>
    ''' <returns></returns>
    Function GetByCriteriaAndVendors(criteria As PaymentSearchCriteria, vendorIds As List(Of Integer)) As List(Of payment)
    Function GetVendorAmountDue(vendorId As Integer) As List(Of AmountDueVM)
    Function GetCashSubpoenaData(selectDate As Date) As List(Of CashSubpoenaDTO)
    Function GetTransferSubpoenaData(day As Date) As List(Of TransferSubpoenaDTO)
    Function GetBankPaymentsByDateRangeAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of IEnumerable(Of payment))
    Function GetCashToBankTransfersByDateRangeAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of IEnumerable(Of payment))

    ''' <summary>
    ''' 取得銀行帳 (存款、提款)
    ''' </summary>
    ''' <param name="bankId"></param>
    ''' <returns></returns>
    Function GetBankAccount(bankId As Integer) As IEnumerable(Of payment)

    ''' <summary>
    ''' 取得該帳款月份的現金存入銀行
    ''' </summary>
    ''' <param name="bankId"></param>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetCashToBankTransfersByAccountMonth(bankId As Integer, month As Date) As IEnumerable(Of payment)

    ''' <summary>
    ''' 取得該帳款月份的銀行存款
    ''' </summary>
    ''' <param name="bankId"></param>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetBankPaymentsByAccountMonth(bankId As Integer, month As Date) As IEnumerable(Of payment)
End Interface