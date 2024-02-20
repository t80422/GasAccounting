''' <summary>
''' 通用的View接口
''' </summary>
''' <typeparam name="TEntity">實體模型</typeparam>
''' <typeparam name="TViewModel">列表用</typeparam>
Public Interface ICommonView(Of TEntity, TViewModel)
    ''' <summary>
    ''' 顯示列表
    ''' </summary>
    ''' <param name="data"></param>
    Sub ShowList(data As List(Of TViewModel))
    ''' <summary>
    ''' 設定取得的資料到控制項
    ''' </summary>
    ''' <param name="data"></param>
    Sub SetDataToControl(data As TEntity)
    ''' <summary>
    ''' 取得用戶輸入資料
    ''' </summary>
    ''' <returns></returns>
    Function GetUserInput() As TEntity
    ''' <summary>
    ''' 重置用戶輸入
    ''' </summary>
    Sub ClearInput()
    ''' <summary>
    ''' 檢查必填欄位
    ''' </summary>
    ''' <returns></returns>
    Function CheckDataRequired() As Boolean
End Interface
