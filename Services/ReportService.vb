Imports System.IO
Imports ClosedXML.Excel

Public Class ReportService
    ''' <summary>
    ''' 產生轉帳傳票
    ''' </summary>
    ''' <param name="day"></param>
    Public Sub GeneratorTransferSubpoena(day As Date, datas As Subpoena)
        Try
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "轉帳傳票.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    .WriteToCell("A2", $"日期: {day.ToShortDateString}")

                    ' 借方
                    Dim debitItems = datas.DebitItems

                    If debitItems IsNot Nothing Then
                        For i As Integer = 0 To debitItems.Count - 1
                            .WriteToCell("A", i + 4, debitItems(i).AccountName)
                            .WriteToCell("B", i + 4, debitItems(i).Summary)
                            .WriteToCell("C", i + 4, debitItems(i).Amount.ToString)
                        Next
                    End If

                    ' 貸方
                    Dim credItems = datas.CreditItems

                    If credItems IsNot Nothing Then
                        For i As Integer = 0 To credItems.Count - 1
                            .WriteToCell("D", i + 4, credItems(i).AccountName)
                            .WriteToCell("E", i + 4, credItems(i).Summary)
                            .WriteToCell("F", i + 4, credItems(i).Amount.ToString)
                        Next
                    End If

                    If debitItems IsNot Nothing Or credItems IsNot Nothing Then
                        ' 取得最大行數
                        Dim debitCount = If(datas.DebitItems IsNot Nothing, datas.DebitItems.Count, 0)
                        Dim creditCount = If(datas.CreditItems IsNot Nothing, datas.CreditItems.Count, 0)
                        Dim maxRow = Math.Max(debitCount, creditCount) + 4

                        ' 設定總計行的格式
                        Dim totalFormat As New CloseXML_Excel.CellFormatOptions With
                        {
                            .IsBold = True
                        }

                        .SetCustomBorders(maxRow, 1, maxRow, 6, topStyle:=XLBorderStyleValues.Thin)

                        ' 借方總計
                        .WriteToCell("A", maxRow, "合計", totalFormat)
                        .WriteToCell("C", maxRow, datas.TotalDebitAmount.ToString)

                        ' 貸方總計
                        .WriteToCell("D", maxRow, "合計", totalFormat)
                        .WriteToCell("E", maxRow, datas.TotalCreditAmount.ToString)
                    End If

                    .SaveExcel($"轉帳傳票_{Now.Date:yyyyMMdd}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生現金傳票
    ''' </summary>
    ''' <param name="day"></param>
    ''' <param name="datas"></param>
    ''' <param name="isIncome">true:收入 false:支出</param>
    Public Sub GeneratorCashIncomeSubpoena(day As Date, datas As Subpoena, isIncome As Boolean)
        Try
            Dim templateFileName = If(isIncome, "現金收入傳票.xlsx", "現金支出傳票.xlsx")
            Dim filePath = Path.Combine(Application.StartupPath, "Report", templateFileName)

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    .WriteToCell("A2", $"日期: {day.ToShortDateString}")

                    ' 借方
                    Dim items = If(datas.DebitItems, datas.CreditItems)

                    If items IsNot Nothing Then
                        For i As Integer = 0 To items.Count - 1
                            .WriteToCell("A", i + 4, items(i).AccountName)
                            .WriteToCell("B", i + 4, items(i).Summary)
                            .WriteToCell("C", i + 4, items(i).Amount.ToString)
                        Next
                    End If

                    If items IsNot Nothing Then
                        ' 取得最大行數
                        Dim count = items.Count
                        Dim maxRow = count + 4

                        ' 設定總計行的格式
                        Dim totalFormat As New CloseXML_Excel.CellFormatOptions With
                        {
                            .IsBold = True
                        }

                        .SetCustomBorders(maxRow, 1, maxRow, 3, topStyle:=XLBorderStyleValues.Thin)

                        ' 總計
                        .WriteToCell("A", maxRow, "合計", totalFormat)

                        Dim amount As String = If(isIncome, datas.TotalDebitAmount, datas.TotalCreditAmount)
                        .WriteToCell("C", maxRow, amount)
                    End If

                    Dim fileName = If(isIncome, "現金收入傳票", "現金支出傳票")
                    .SaveExcel($"{fileName}_{Now.Date:yyyyMMdd}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
