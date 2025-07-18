Imports System.IO
Imports ClosedXML.Excel

Public Class ReportService
    ''' <summary>
    ''' 產生傳票
    ''' </summary>
    ''' <param name="day"></param>
    ''' <param name="datas"></param>
    ''' <param name="isCash">true:現金 false:轉帳</param>
    ''' <param name="isIncome">true:收入 false:支出</param>
    Public Sub GeneratorSubpoena(day As Date, datas As List(Of SubpoenaDTO), isCash As Boolean, isIncome As Boolean)
        Try
            Dim templateFileName = "現金傳票範本檔.xlsx"
            Dim filePath = Path.Combine(Application.StartupPath, "Report", templateFileName)

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim title As String = ""

                    If isCash Then
                        title = "現金"
                    Else
                        title = "轉帳"
                    End If

                    If isIncome Then
                        title += "收入"
                    Else
                        title += "支出"
                    End If

                    title += "傳票"

                    .WriteToCell("A1", title)
                    .WriteToCell("B2", $"日期: {day.ToShortDateString}")

                    For i As Integer = 0 To datas.Count - 1
                        .WriteToCell("A", i + 4, datas(i).SubjectName)
                        .WriteToCell("B", i + 4, $"{datas(i).Code}  {datas(i).Summary}")
                        .WriteToCell("C", i + 4, datas(i).Amount.ToString)
                    Next

                    ' 頁尾
                    Dim rowIndex = datas.Count + 4

                    .SetCustomBorders(rowIndex, 1, rowIndex, 3, topStyle:=XLBorderStyleValues.Thin)

                    ' 總計
                    .WriteToCell("B", rowIndex, "合計")
                    .WriteToCell("C", rowIndex, datas.Sum(Function(x) x.Amount).ToString)

                    .SaveExcel($"{title}_{day:yyyyMMdd}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
