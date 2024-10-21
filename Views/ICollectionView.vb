Public Interface ICollectionView
    Inherits ICommonView_old(Of collection, CollectionVM)

    ''' <summary>
    ''' 設定科目選單
    ''' </summary>
    ''' <param name="data"></param>
    Sub SetSubjectsCmb(data As IReadOnlyList(Of SelectListItem))

    ''' <summary>
    ''' 設定銀行選單
    ''' </summary>
    ''' <param name="data"></param>
    Sub SetBankCmb(data As List(Of SelectListItem))

    ''' <summary>
    ''' 設定公司選單
    ''' </summary>
    ''' <param name="data"></param>
    Sub ICollectionView_SetCompanyCmb(data As List(Of SelectListItem))

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

    ''' <summary>
    ''' 取得傳票資訊
    ''' </summary>
    ''' <returns></returns>
    Function GetJournalDatas() As journal

    Sub Reset()

    Overloads Sub SetDataToControl(col As collection, Optional che As cheque = Nothing)
End Interface
