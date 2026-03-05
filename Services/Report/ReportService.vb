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

    Private Sub IReportService_GeneratorTransferSubpoena(request As TransferSubpoenaReportRequest) Implements IReportService.GeneratorTransferSubpoena
        If request Is Nothing Then Throw New ArgumentNullException(NameOf(request))
        If request.Groups Is Nothing Then request.Groups = New List(Of TransferSubpoenaGroup)
        Try
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "轉帳傳票範本檔.xlsx")
            Dim isIncome = (request.VoucherType = TransferVoucherType.Income)
            Dim opts = New CloseXML_Excel.CellFormatOptions With {.VerticalCenter = True}

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    Dim title = "轉帳" + If(isIncome, "收入傳票", "支出傳票")
                    .WriteToCell("A1", title)
                    .WriteToCell("A2", $"日期: {request.Day:yyyy年MM月dd日}")

                    Dim currentRow As Integer = 4
                    For Each grp In request.Groups
                        If grp.Details Is Nothing OrElse grp.Details.Count = 0 Then Continue For
                        Dim startRow As Integer = currentRow
                        For Each d In grp.Details
                            If isIncome Then
                                .WriteToCell("A", currentRow, grp.SubjectName, opts)
                                .WriteToCell("B", currentRow, grp.Summary, opts)
                                .WriteToCell("C", currentRow, grp.Amount, opts)
                                .WriteToCell("D", currentRow, d.SubjectName, opts)
                                .WriteToCell("E", currentRow, d.Summary, opts)
                                .WriteToCell("F", currentRow, d.Amount, opts)
                            Else
                                .WriteToCell("A", currentRow, d.SubjectName, opts)
                                .WriteToCell("B", currentRow, d.Summary, opts)
                                .WriteToCell("C", currentRow, d.Amount, opts)
                                .WriteToCell("D", currentRow, grp.SubjectName, opts)
                                .WriteToCell("E", currentRow, grp.Summary, opts)
                                .WriteToCell("F", currentRow, grp.Amount, opts)
                            End If
                            .SetCustomBorders(currentRow, 1, currentRow, 2, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                            .SetCustomBorders(currentRow, 3, currentRow, 3, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Double)
                            .SetCustomBorders(currentRow, 4, currentRow, 4, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Double, XLBorderStyleValues.Thin)
                            .SetCustomBorders(currentRow, 5, currentRow, 6, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                            currentRow += 1
                        Next
                        Dim endRow As Integer = currentRow - 1
                        If grp.Details.Count > 1 Then
                            If isIncome Then
                                .MergeCells(startRow, 1, endRow, 1)
                                .MergeCells(startRow, 2, endRow, 2)
                                .MergeCells(startRow, 3, endRow, 3)
                            Else
                                .MergeCells(startRow, 4, endRow, 4)
                                .MergeCells(startRow, 5, endRow, 5)
                                .MergeCells(startRow, 6, endRow, 6)
                            End If
                        End If
                    Next

                    Dim rowIndex As Integer = currentRow
                    .SetCustomBorders(rowIndex, 1, rowIndex, 6, topStyle:=XLBorderStyleValues.Medium)
                    .WriteToCell("A", rowIndex, "合計")
                    .WriteFormula("C", rowIndex, $"SUM(C4:C{rowIndex - 1})")
                    .SetCustomBorders(rowIndex, 1, rowIndex, 2, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                    .SetCustomBorders(rowIndex, 3, rowIndex, 3, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Double)
                    .WriteToCell("D", rowIndex, "合計")
                    .WriteFormula("F", rowIndex, $"SUM(F4:F{rowIndex - 1})")
                    .SetCustomBorders(rowIndex, 4, rowIndex, 4, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Double, XLBorderStyleValues.Thin)
                    .SetCustomBorders(rowIndex, 5, rowIndex, 6, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)

                    .SaveExcel($"{title}_{request.Day:yyyyMMdd}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
