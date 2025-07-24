Imports System.IO
Imports ClosedXML.Excel

Public Class ReportService
    Implements IReportService

    Private Sub IReportService_GeneratorCashSubpoena(day As Date, datas As List(Of CashSubpoenaDTO), isCollection As Boolean) Implements IReportService.GeneratorCashSubpoena
        Try
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "現金傳票範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim title = "現金" + If(isCollection, "收入傳票", "支出傳票")

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

    Private Sub IReportService_GeneratorTransferSubpoena(day As Date, datas As List(Of TransferSubpoenaDTO), isCollection As Boolean) Implements IReportService.GeneratorTransferSubpoena
        Try
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "轉帳傳票範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim title = "轉帳" + If(isCollection, "收入傳票", "支出傳票")

                    .WriteToCell("A1", title)
                    .WriteToCell("B2", $"日期: {day.ToShortDateString}")

                    For i As Integer = 0 To datas.Count - 1
                        .WriteToCell("A", i + 4, datas(i).DebitSubjectName)
                        .WriteToCell("B", i + 4, datas(i).DebitSummary)
                        .WriteToCell("C", i + 4, datas(i).DebitAmount.ToString)
                        .WriteToCell("D", i + 4, datas(i).CreditSubjectName)
                        .WriteToCell("E", i + 4, datas(i).CreditSummary)
                        .WriteToCell("F", i + 4, datas(i).CreditAmount.ToString)
                    Next

                    ' 頁尾
                    Dim rowIndex = datas.Count + 4

                    .SetCustomBorders(rowIndex, 1, rowIndex, 6, topStyle:=XLBorderStyleValues.Thin)

                    ' 總計
                    .WriteToCell("B", rowIndex, "合計")
                    .WriteToCell("C", rowIndex, datas.Sum(Function(x) x.DebitAmount).ToString)
                    .WriteToCell("F", rowIndex, datas.Sum(Function(x) x.CreditAmount).ToString)

                    .SaveExcel($"{title}_{day:yyyyMMdd}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
