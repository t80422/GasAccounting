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
    Function DailyCustomerReceivable(d As Date, cusId As String) As List(Of DailyCustomerReceivable)

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
    Function GetBankAccount(month As Date, bankId As Integer) As BankAccount

    ''' <summary>
    ''' 取得客戶寄桶結存瓶
    ''' </summary>
    ''' <param name="cusId"></param>
    ''' <returns></returns>
    Function GetCustomerGasCylinderInventory(cusId As Integer) As CustomerGasCylinderInventory

    ''' <summary>
    ''' 取得新桶明細
    ''' </summary>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetNewBarrelDetails(month As Date) As NewBarrelDetails

    ''' <summary>
    ''' 取得應收票據
    ''' </summary>
    ''' <param name="companyId"></param>
    ''' <param name="bankId"></param>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetBillsReceivable(companyId As Integer, bankId As Integer, month As Date) As BillsReceivable

    ''' <summary>
    ''' 取得發票
    ''' </summary>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetInvoice(month As Date) As List(Of Report_Invoice)

    ''' <summary>
    ''' 取得月應收帳明細
    ''' </summary>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetMonthlyAccountsReceivable(month As Date) As MonthlyAccountsReceivable

    ''' <summary>
    ''' 取得進銷存明細
    ''' </summary>
    ''' <param name="month"></param>
    ''' <param name="compId"></param>
    ''' <returns></returns>
    Function GetInventoryTransactionDetail(year As Date, compId As Integer, empId As Integer) As InventoryTransactionDetail

    ''' <summary>
    ''' 取得應付票據
    ''' </summary>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetPayableCheck(month As Date) As List(Of PayableCheck)

    ''' <summary>
    ''' 取得財稅
    ''' </summary>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetTax(month As Date) As Tax

    ''' <summary>
    ''' 取得能源局
    ''' </summary>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetBureau(month As Date) As List(Of EnergyBureau)

    ''' <summary>
    ''' 取得月對帳單
    ''' </summary>
    ''' <param name="compId"></param>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetMonthlyStatement(cusCode As String, month As Date) As MonthlyStatement

    ''' <summary>
    ''' 取得保險
    ''' </summary>
    ''' <param name="compId"></param>
    ''' <param name="month"></param>
    ''' <returns></returns>
    Function GetInsurance(compId As Integer, month As Date) As Insurance

    ''' <summary>
    ''' 取得損益表
    ''' </summary>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    ''' <param name="compId"></param>
    ''' <returns></returns>
    Function GetIncomeStatement(startDate As Date, endDate As Date, compId As Integer) As IncomeStatement

    ''' <summary>
    ''' 取得銷項
    ''' </summary>
    ''' <param name="year"></param>
    ''' <param name="months"></param>
    ''' <returns></returns>
    Function GetOutInvoice(year As Integer, months As String) As OutInvoice

    ''' <summary>
    ''' 取得分裝場進銷項
    ''' </summary>
    ''' <param name="year"></param>
    ''' <param name="months"></param>
    ''' <returns></returns>
    Function GetSplitCompany(year As Integer, months As String) As SplitCompanyInvoice
End Interface