Public Interface ICustomer
    Inherits ICommonView(Of customer, CustomerVM)

    ''' <summary>
    ''' 取得搜尋條件
    ''' </summary>
    ''' <returns></returns>
    Function GetSearchConditions() As customer

    ''' <summary>
    ''' 設定價格方案ComboBox
    ''' </summary>
    Sub SetPricePlan_Cmb(data As List(Of ComboBoxItems))

    Sub SetPricePlanDetails(data As priceplan)
End Interface
