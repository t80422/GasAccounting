Public Interface ICollectionView
    Inherits ICommonView(Of collection, CollectionVM)

    ''' <summary>
    ''' 設定科目選單
    ''' </summary>
    ''' <param name="data"></param>
    Sub SetSubjectsCmb(data As List(Of ComboBoxItems))

    ''' <summary>
    ''' 設定銀行選單
    ''' </summary>
    ''' <param name="data"></param>
    Sub SetBankCmb(data As List(Of ComboBoxItems))

    ''' <summary>
    ''' 設定公司選單
    ''' </summary>
    ''' <param name="data"></param>
    Sub SetCompanyCmb(data As List(Of ComboBoxItems))

    ''' <summary>
    ''' 取得搜尋條件
    ''' </summary>
    ''' <returns></returns>
    Function GetQueryConditions() As CollectionQueryVM

    ''' <summary>
    ''' 取得支票資訊
    ''' </summary>
    ''' <returns></returns>
    Function GetChequeDatas() As cheque
End Interface
