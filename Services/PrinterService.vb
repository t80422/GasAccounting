Imports System.Drawing.Printing
Imports System.IO

Public Class PrinterService
    Implements IPrinterService

    Private ReadOnly _settingsPath As String

    Public Sub New()
        '設定檔存放在應用程式目錄下的Settings資料夾
        _settingsPath = Path.Combine(Application.StartupPath, "Settings", "printer.txt")

        '確保目錄存在
        Dim dirPath = Path.GetDirectoryName(_settingsPath)
        If Not Directory.Exists(dirPath) Then Directory.CreateDirectory(dirPath)
    End Sub

    Public Sub SavePrinter(printerName As String) Implements IPrinterService.SavePrinter
        Try
            File.WriteAllText(_settingsPath, printerName)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function GetOrSelectPrinter() As String Implements IPrinterService.GetOrSelectPrinter
        '先取得儲存的印表機
        Dim savedPrinter = GetSavedPrinter()

        '如果有儲存的印表機且該印表機存在,直接返回
        If Not String.IsNullOrEmpty(savedPrinter) AndAlso IsPrinterExists(savedPrinter) Then Return savedPrinter

        '否則讓使用者選擇印表機
        Using pd As New PrintDialog
            If pd.ShowDialog = DialogResult.OK Then
                Dim selectedPrinter = pd.PrinterSettings.PrinterName
                SavePrinter(selectedPrinter)
                Return selectedPrinter
            End If
        End Using

        Return String.Empty
    End Function

    Public Function IsPrinterExists(printerName As String) As Boolean Implements IPrinterService.IsPrinterExists
        Return PrinterSettings.InstalledPrinters.Cast(Of String).Any(Function(x) x = printerName)
    End Function

    ''' <summary>
    ''' 取得儲存的印表機
    ''' </summary>
    ''' <returns></returns>
    Private Function GetSavedPrinter() As String
        Try
            If File.Exists(_settingsPath) Then Return File.ReadAllText(_settingsPath).Trim
        Catch ex As Exception
            Throw
        End Try

        Return String.Empty
    End Function
End Class
