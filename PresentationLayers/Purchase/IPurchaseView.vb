Public Interface IPurchaseView
    Inherits ICommonView(Of purchase, PurchaseVM)

    ''' <summary>
    ''' 取得查詢條件
    ''' </summary>
    ''' <returns></returns>
    Function GetSearchCondition() As purchase

    ''' <summary>
    ''' 設定公司ComboBox
    ''' </summary>
    Sub SetCompanyComboBox(items As List(Of ComboBoxItems))

    ''' <summary>
    ''' 設定大氣廠商ComboBox
    ''' </summary>
    ''' <param name="items"></param>
    Sub SetGasVendorComboBox(items As List(Of ComboBoxItems))

    ''' <summary>
    ''' 設定預設價格
    ''' </summary>
    ''' <param name="productPrice">產品單價</param>
    ''' <param name="deliveryPrice">運費單價</param>
    Sub SetDefaultPrice(productPrice As Single, deliveryPrice As Single)
End Interface
