''' <summary>
''' 目前登入使用者服務介面
''' </summary>
Public Interface ICurrentUserService
    ''' <summary>
    ''' 設定目前登入的使用者資訊
    ''' </summary>
    ''' <param name="userId">使用者 ID</param>
    ''' <param name="userName">使用者名稱</param>
    Sub SetCurrentUser(userId As Integer, userName As String, roleId As Integer)

    ''' <summary>
    ''' 取得目前使用者 ID
    ''' </summary>
    ''' <returns>使用者 ID，若未登入則回傳 Nothing</returns>
    ReadOnly Property UserId As Integer?

    ''' <summary>
    ''' 取得目前使用者名稱
    ''' </summary>
    ''' <returns>使用者名稱，若未登入則回傳 空字串</returns>
    ReadOnly Property UserName As String

    ''' <summary>
    ''' 角色編號
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property UserRole As Integer

    ''' <summary>
    ''' 檢查是否已登入
    ''' </summary>
    ''' <returns>已登入回傳 True，否則回傳 False</returns>
    ReadOnly Property IsAuthenticated As Boolean

    ''' <summary>
    ''' 清除使用者資訊（登出時使用）
    ''' </summary>
    Sub ClearCurrentUser()
End Interface

