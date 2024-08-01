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

    ''' <summary>
    ''' 設定運輸公司Combobox
    ''' </summary>
    ''' <param name="items"></param>
    Sub SetDriveVendorCmb(items As List(Of ComboBoxItems))

    ''' <summary>
    ''' 設定科目Combobox
    ''' </summary>
    ''' <param name="items"></param>
    Sub SetSubjectCmb(items As List(Of ComboBoxItems))
End Interface
