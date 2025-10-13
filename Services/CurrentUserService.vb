''' <summary>
''' 目前登入使用者服務實作
''' 管理整個應用程式的使用者會話資訊
''' </summary>
Public Class CurrentUserService
    Implements ICurrentUserService

    Private _userId As Integer?
    Private _userName As String
    Private _userRole As Integer?

    Public Sub New()
        _userId = Nothing
        _userName = String.Empty
        _userRole = Nothing
    End Sub

    ''' <summary>
    ''' 設定目前登入的使用者資訊
    ''' </summary>
    Public Sub SetCurrentUser(userId As Integer, userName As String, roleId As Integer) Implements ICurrentUserService.SetCurrentUser
        _userId = userId
        _userName = userName
        _userRole = roleId
    End Sub

    ''' <summary>
    ''' 取得目前使用者 ID
    ''' </summary>
    Public ReadOnly Property UserId As Integer? Implements ICurrentUserService.UserId
        Get
            Return _userId
        End Get
    End Property

    ''' <summary>
    ''' 取得目前使用者名稱
    ''' </summary>
    Public ReadOnly Property UserName As String Implements ICurrentUserService.UserName
        Get
            Return If(_userName, String.Empty)
        End Get
    End Property

    ''' <summary>
    ''' 檢查是否已登入
    ''' </summary>
    Public ReadOnly Property IsAuthenticated As Boolean Implements ICurrentUserService.IsAuthenticated
        Get
            Return _userId.HasValue
        End Get
    End Property

    Public ReadOnly Property UserRole As Integer Implements ICurrentUserService.UserRole
        Get
            Return _userRole
        End Get
    End Property

    ''' <summary>
    ''' 清除使用者資訊（登出時使用）
    ''' </summary>
    Public Sub ClearCurrentUser() Implements ICurrentUserService.ClearCurrentUser
        _userId = Nothing
        _userName = String.Empty
    End Sub
End Class

