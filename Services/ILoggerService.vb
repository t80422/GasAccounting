''' <summary>
''' 日誌服務介面
''' </summary>
Public Interface ILoggerService
    ''' <summary>
    ''' 記錄一般資訊
    ''' </summary>
    Sub LogInfo(message As String)

    ''' <summary>
    ''' 記錄警告訊息
    ''' </summary>
    Sub LogWarning(message As String)

    ''' <summary>
    ''' 記錄錯誤訊息
    ''' </summary>
    Sub LogError(ex As Exception, message As String)

    ''' <summary>
    ''' 記錄除錯訊息
    ''' </summary>
    Sub LogDebug(message As String)

    ''' <summary>
    ''' 記錄追蹤訊息
    ''' </summary>
    Sub LogTrace(message As String)

    ''' <summary>
    ''' 記錄嚴重錯誤
    ''' </summary>
    Sub LogFatal(ex As Exception, message As String)
End Interface

