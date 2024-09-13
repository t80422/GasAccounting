Public Interface IReportRep
    ''' <summary>
    ''' 取得所有客戶當日氣量氣款收付明細表
    ''' </summary>
    ''' <param name="d"></param>
    ''' <returns></returns>
    Function CustomersGasDetailByDay(d As Date) As List(Of CustomersGasDetailByDay)

    ''' <summary>
    ''' 客戶提氣清冊
    ''' </summary>
    ''' <param name="d"></param>
    ''' <returns></returns>
    Function CustomersGetGasList(d As Date) As List(Of CusGetGas)

    ''' <summary>
    ''' 大氣應付明細表
    ''' </summary>
    ''' <param name="d"></param>
    ''' <returns></returns>
    Function GasPayableDetailList(d As Date, manuId As Integer) As List(Of GasPayableDetail)

    ''' <summary>
    ''' 客戶每日應收帳明細
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="cusId"></param>
    ''' <returns></returns>
    Function DailyCustomerReceivable(d As Date, cusId As Integer) As List(Of DailyCustomerReceivable)

    ''' <summary>
    ''' 提量支數統計
    ''' </summary>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GasUsageAndCylinderCount(month As Date) As List(Of GasUsageAndCylinderCount)

    ''' <summary>
    ''' 取得現金帳
    ''' </summary>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetCashAccount(month As Date) As List(Of CashAccount)

    ''' <summary>
    ''' 取得銀行帳
    ''' </summary>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetBankAccount(month As Date, bankId As Integer) As Report_BankAccount

    ''' <summary>
    ''' 取得客戶寄桶結存瓶
    ''' </summary>
    ''' <param name="cusId"></param>
    ''' <returns></returns>
    Function GetCustomerGasCylinderInventory(cusId As Integer) As Report_CustomerGasCylinderInventory
End Interface
