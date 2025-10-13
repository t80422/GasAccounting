Imports NLog

''' <summary>
''' 日誌服務實作
''' </summary>
Public Class LoggerService
    Implements ILoggerService

    Private ReadOnly _logger As Logger

    ''' <summary>
    ''' 建構子，根據呼叫者類別名稱建立 Logger
    ''' </summary>
    Public Sub New(loggerName As String)
        _logger = LogManager.GetLogger(loggerName)
    End Sub

    ''' <summary>
    ''' 記錄一般資訊
    ''' </summary>
    Public Sub LogInfo(message As String) Implements ILoggerService.LogInfo
        _logger.Info(message)
    End Sub

    ''' <summary>
    ''' 記錄警告訊息
    ''' </summary>
    Public Sub LogWarning(message As String) Implements ILoggerService.LogWarning
        _logger.Warn(message)
    End Sub

    ''' <summary>
    ''' 記錄錯誤訊息
    ''' </summary>
    Public Sub LogError(ex As Exception, message As String) Implements ILoggerService.LogError
        _logger.Error(ex, message)
    End Sub

    ''' <summary>
    ''' 記錄除錯訊息
    ''' </summary>
    Public Sub LogDebug(message As String) Implements ILoggerService.LogDebug
        _logger.Debug(message)
    End Sub

    ''' <summary>
    ''' 記錄追蹤訊息
    ''' </summary>
    Public Sub LogTrace(message As String) Implements ILoggerService.LogTrace
        _logger.Trace(message)
    End Sub

    ''' <summary>
    ''' 記錄嚴重錯誤
    ''' </summary>
    Public Sub LogFatal(ex As Exception, message As String) Implements ILoggerService.LogFatal
        _logger.Fatal(ex, message)
    End Sub
End Class

