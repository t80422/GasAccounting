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
                    .WriteToCell("B2", $"日期: {day:yyyy年MM月dd日}")
                    .WriteToCell("A3", If(isCollection, "貸方科目", "借方科目"))

                    For i As Integer = 0 To datas.Count - 1
                        .WriteToCell("A", i + 4, datas(i).SubjectName)
                        .WriteToCell("B", i + 4, $"{datas(i).Code}  {datas(i).Summary}")
                        .WriteToCell("C", i + 4, datas(i).Amount.ToString)
                        .SetCustomBorders(i + 4, 1, i + 4, 1, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                        .SetCustomBorders(i + 4, 2, i + 4, 2, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                        .SetCustomBorders(i + 4, 3, i + 4, 3, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                    Next

                    ' 頁尾
                    Dim rowIndex = datas.Count + 4

                    ' 總計
                    .WriteToCell("A", rowIndex, "合計")
                    .WriteToCell("C", rowIndex, datas.Sum(Function(x) x.Amount).ToString)
                    .SetCustomBorders(rowIndex, 1, rowIndex, 1, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                    .SetCustomBorders(rowIndex, 2, rowIndex, 2, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                    .SetCustomBorders(rowIndex, 3, rowIndex, 3, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)

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
                    .WriteToCell("A2", $"日期: {day:yyyy年MM月dd日}")

                    datas = datas.OrderBy(Function(x) x.Id).ToList()

                    Dim currentStartRow As Integer = 4
                    Dim currentColId As Integer = -1

                    For i As Integer = 0 To datas.Count - 1
                        Dim currentRow = i + 4
                        Dim data = datas(i)

                        .WriteToCell("A", currentRow, data.DebitSubjectName, New CloseXML_Excel.CellFormatOptions With {.VerticalCenter = True})
                        .WriteToCell("B", currentRow, data.DebitSummary, New CloseXML_Excel.CellFormatOptions With {.VerticalCenter = True})

                        ' 如果是收款(isCollection=True)，借方是總額，只需在第一列顯示
                        Dim debitAmountVal As Object = If(isCollection AndAlso i > 0 AndAlso data.Id = datas(i - 1).Id, String.Empty, data.DebitAmount)
                        .WriteToCell("C", currentRow, debitAmountVal, New CloseXML_Excel.CellFormatOptions With {.VerticalCenter = True})

                        .SetCustomBorders(currentRow, 1, currentRow, 2, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                        .SetCustomBorders(currentRow, 3, currentRow, 3, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Double)

                        .WriteToCell("D", currentRow, data.CreditSubjectName, New CloseXML_Excel.CellFormatOptions With {.VerticalCenter = True})
                        .WriteToCell("E", currentRow, data.CreditSummary, New CloseXML_Excel.CellFormatOptions With {.VerticalCenter = True})

                        ' 如果是付款(isCollection=False)，貸方是總額，只需在第一列顯示
                        Dim creditAmountVal As Object = If(Not isCollection AndAlso i > 0 AndAlso data.Id = datas(i - 1).Id, String.Empty, data.CreditAmount)
                        .WriteToCell("F", currentRow, creditAmountVal, New CloseXML_Excel.CellFormatOptions With {.VerticalCenter = True})

                        .SetCustomBorders(currentRow, 4, currentRow, 4, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Double, XLBorderStyleValues.Thin)
                        .SetCustomBorders(currentRow, 5, currentRow, 6, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)

                        ' 檢查是否需要合併 (當 Id 改變)
                        If data.Id <> currentColId Then
                            If i > 0 AndAlso currentRow - 1 > currentStartRow Then
                                ' 直接合併借方 A、B、C 欄
                                .MergeCells(currentStartRow, 1, currentRow - 1, 1)
                                .MergeCells(currentStartRow, 2, currentRow - 1, 2)
                                .MergeCells(currentStartRow, 3, currentRow - 1, 3)
                            End If
                            currentStartRow = currentRow
                            currentColId = data.Id
                        End If

                        ' 最後一筆資料的特殊處理
                        If i = datas.Count - 1 AndAlso currentRow > currentStartRow Then
                            ' 直接合併借方 A、B、C 欄
                            .MergeCells(currentStartRow, 1, currentRow, 1)
                            .MergeCells(currentStartRow, 2, currentRow, 2)
                            .MergeCells(currentStartRow, 3, currentRow, 3)
                        End If
                    Next

                    ' 頁尾
                    Dim rowIndex = datas.Count + 4

                    .SetCustomBorders(rowIndex, 1, rowIndex, 6, topStyle:=XLBorderStyleValues.Medium)

                    ' 總計
                    .WriteToCell("A", rowIndex, "合計")
                    ' 使用 Excel SUM 公式從第 4 列加總到最後一筆資料列
                    .WriteFormula("C", rowIndex, $"SUM(C4:C{rowIndex - 1})")

                    .SetCustomBorders(rowIndex, 1, rowIndex, 2, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                    .SetCustomBorders(rowIndex, 3, rowIndex, 3, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Double)

                    .WriteToCell("D", rowIndex, "合計")
                    .WriteFormula("F", rowIndex, $"SUM(F4:F{rowIndex - 1})")

                    .SetCustomBorders(rowIndex, 4, rowIndex, 4, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Double, XLBorderStyleValues.Thin)
                    .SetCustomBorders(rowIndex, 5, rowIndex, 6, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)

                    .SaveExcel($"{title}_{day:yyyyMMdd}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
