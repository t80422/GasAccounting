Public Interface IPaymentView
    Inherits IBaseView(Of payment, PaymentListVM)

    ' 事件
    Event AddRequested As EventHandler
    Event UpdateRequested As EventHandler
    Event DeleteRequested As EventHandler
    Event CancelRequested As EventHandler
    Event DetailRequested As EventHandler(Of Integer)
    Event PrintRequested As EventHandler(Of Tuple(Of Date, String))
    Event ManufacturerSelected As EventHandler(Of Integer)
    Event CompanySelected As EventHandler(Of Integer)

    ''' <summary>
    ''' 設定廠商下拉選單
    ''' </summary>
    Sub PopulateVendorDropdown(data As IReadOnlyList(Of SelectListItem))

    ''' <summary>
    ''' 設定科目下拉選單
    ''' </summary>
    Sub PopulateSubjectDropdown(data As IReadOnlyList(Of SelectListItem))

    ''' <summary>
    ''' 設定公司下拉選單
    ''' </summary>
    ''' <param name="data"></param>
    Sub PopulateCompanyDropdown(data As IReadOnlyList(Of SelectListItem))

    ''' <summary>
    ''' 設定銀行下拉選單
    ''' </summary>
    ''' <param name="data"></param>
    Sub PopulateBankDropdown(data As IReadOnlyList(Of SelectListItem))

    ''' <summary>
    ''' 設定應付未付列表
    ''' </summary>
    ''' <param name="data"></param>
    Sub DisplayAmountDueList(data As IReadOnlyList(Of AmountDueVM))

    ''' <summary>
    ''' 取得搜尋條件
    ''' </summary>
    ''' <returns></returns>
    Function GetSearchCriteria() As PaymentSearchCriteria

    ''' <summary>
    ''' 顯示廠商帳號
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    Sub ShowVendorAccount(data As String)

    Sub SetButton(isSelectedRow As Boolean)
End Interface
