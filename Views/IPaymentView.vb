Public Interface IPaymentView
    Inherits IBaseView(Of payment, PaymentVM)

    ''' <summary>
    ''' 設定廠商下拉選單
    ''' </summary>
    Sub PopulateVendorDropdown(data As IReadOnlyList(Of SelectListItem))

    ''' <summary>
    ''' 設定銀行下拉選單
    ''' </summary>
    Sub PopulateBankDropdown(data As IReadOnlyList(Of SelectListItem))

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
    ''' 設定應付未付列表
    ''' </summary>
    ''' <param name="data"></param>
    Sub DisplayAmountDueList(data As IReadOnlyList(Of AmountDueVM))

    ''' <summary>
    ''' 取得搜尋條件
    ''' </summary>
    ''' <returns></returns>
    Function GetSearchCriteria() As PaymentSearchCriteria
End Interface
