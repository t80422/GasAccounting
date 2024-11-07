''' <summary>
''' 印表機服務介面
''' </summary>
Public Interface IPrinterService
    ''' <summary>
    ''' 取得或選擇印表機
    ''' </summary>
    ''' <returns></returns>
    Function GetOrSelectPrinter() As String

    ''' <summary>
    ''' 檢查印表機是否存在
    ''' </summary>
    ''' <param name="printerName"></param>
    ''' <returns></returns>
    Function IsPrinterExists(printerName As String) As Boolean

    ''' <summary>
    ''' 儲存預設印表機設定
    ''' </summary>
    ''' <param name="printerName"></param>
    Sub SavePrinter(printerName As String)
End Interface
