Public Interface IMonthlyAccountService
    ''' <summary>
    ''' 初始化月度帳單資料 (將歷史訂單資料遷移到月度帳單表)
    ''' </summary>
    Function InitializeMonthlyAccounts() As Boolean

    ''' <summary>
    ''' 同步特定訂單的月度帳單資料
    ''' </summary>
    Function SyncOrderToMonthlyAccount(orderId As Integer, isNew As Boolean, isDelete As Boolean) As Boolean

    ''' <summary>
    ''' 取得客戶未結案的月度帳單資料
    ''' </summary>
    Function GetCustomerUnpaidMonths(customerId As Integer) As List(Of monthly_account)
End Interface 