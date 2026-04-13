Imports System.Diagnostics

Public Class DebugTimingTracker
    Private _sw As New Stopwatch
    Private _lastMs As Long = 0
    Private _lines As New List(Of String)

    Public Sub StartTracking()
        _sw.Restart()
        _lastMs = 0
        _lines.Clear()
        _lines.Add($"[開始] {DateTime.Now:HH:mm:ss.fff}")
    End Sub

    Public Sub Mark(stepName As String)
        Dim total = _sw.ElapsedMilliseconds
        Dim interval = total - _lastMs
        _lines.Add($"{stepName}: 累計 {total}ms  /  區間 {interval}ms")
        _lastMs = total
    End Sub

    Public Function GetReport() As String
        Dim total = _sw.ElapsedMilliseconds
        _lines.Add($"[結束] 總耗時 {total}ms")
        Return String.Join(Environment.NewLine, _lines)
    End Function
End Class
