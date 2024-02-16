Public Interface IPresenter(Of TEntity, TViewModel)
    ''' <summary>
    ''' 設定查詢條件
    ''' </summary>
    ''' <param name="query"></param>
    ''' <param name="conditions"></param>
    ''' <returns>沒有查詢就回傳nothing</returns>
    Function SetSearchConditions(query As IQueryable(Of TEntity), conditions As Object) As IQueryable(Of TEntity)

    ''' <summary>
    ''' 設定列表的model
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <returns></returns>
    Function SetListViewModel(query As IQueryable(Of TEntity)) As List(Of TViewModel)
End Interface
