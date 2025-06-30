Imports System.IO
Imports ClosedXML.Excel

Public Class ReportService
    '''' <summary>
    '''' 產生轉帳傳票
    '''' </summary>
    '''' <param name="day"></param>
    'Public Sub GeneratorTransferSubpoena(day As Date, datas As Subpoena)
    '    Try
    '        Dim filePath = Path.Combine(Application.StartupPath, "Report", "轉帳傳票.xlsx")

    '        Using xml As New CloseXML_Excel(filePath)
    '            With xml
    '                .SelectWorksheet("Sheet1")

    '                .WriteToCell("A2", $"日期: {day.ToShortDateString}")

    '                ' 借方
    '                Dim debitItems = datas.DebitItems

    '                If debitItems IsNot Nothing Then
    '                    For i As Integer = 0 To debitItems.Count - 1
    '                        .WriteToCell("A", i + 4, debitItems(i).AccountName)
    '                        .WriteToCell("B", i + 4, debitItems(i).Summary)
    '                        .WriteToCell("C", i + 4, debitItems(i).Amount.ToString)
    '                    Next
    '                End If

    '                ' 貸方
    '                Dim credItems = datas.CreditItems

    '                If credItems IsNot Nothing Then
    '                    For i As Integer = 0 To credItems.Count - 1
    '                        .WriteToCell("D", i + 4, credItems(i).AccountName)
    '                        .WriteToCell("E", i + 4, credItems(i).Summary)
    '                        .WriteToCell("F", i + 4, credItems(i).Amount.ToString)
    '                    Next
    '                End If

    '                If debitItems IsNot Nothing Or credItems IsNot Nothing Then
    '                    ' 取得最大行數
    '                    Dim debitCount = If(datas.DebitItems IsNot Nothing, datas.DebitItems.Count, 0)
    '                    Dim creditCount = If(datas.CreditItems IsNot Nothing, datas.CreditItems.Count, 0)
    '                    Dim maxRow = Math.Max(debitCount, creditCount) + 4

    '                    ' 設定總計行的格式
    '                    Dim totalFormat As New CloseXML_Excel.CellFormatOptions With
    '                    {
    '                        .IsBold = True
    '                    }

    '                    .SetCustomBorders(maxRow, 1, maxRow, 6, topStyle:=XLBorderStyleValues.Thin)

    '                    ' 借方總計
    '                    .WriteToCell("A", maxRow, "合計", totalFormat)
    '                    .WriteToCell("C", maxRow, datas.TotalDebitAmount.ToString)

    '                    ' 貸方總計
    '                    .WriteToCell("D", maxRow, "合計", totalFormat)
    '                    .WriteToCell("E", maxRow, datas.TotalCreditAmount.ToString)
    '                End If

    '                .SaveExcel($"轉帳傳票_{Now.Date:yyyyMMdd}")
    '            End With
    '        End Using
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

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
