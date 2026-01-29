Imports ClosedXML.Excel
Imports Microsoft.Office.Interop.Excel

Public Class CloseXML_Excel
    Implements IDisposable

    Private workbook As XLWorkbook

    Public Worksheet As IXLWorksheet

    ''' <summary>
    ''' 初始化範本檔,要先選擇sheet
    ''' </summary>
    ''' <param name="filePath">範本檔路徑</param>
    Public Sub New(Optional filePath As String = Nothing)
        Try
            If filePath IsNot Nothing Then
                workbook = New XLWorkbook(filePath)
            Else
                workbook = New XLWorkbook()
                Worksheet = workbook.Worksheets.Add("Sheet1")
            End If

            Worksheet = workbook.Worksheets.First()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' 設定指定列的欄寬。
    ''' </summary>
    ''' <param name="columnIndex">列的索引，從 1 開始。</param>
    ''' <param name="width">欄寬值。</param>
    Public Sub SetColumnWidth(columnIndex As Integer, width As Double)
        Worksheet.Column(columnIndex).Width = width
    End Sub

    ''' <summary>
    ''' 隱藏指定欄位
    ''' </summary>
    ''' <param name="columnIndex">欄位索引，從 1 開始</param>
    Public Sub HideColumn(columnIndex As Integer)
        Worksheet.Column(columnIndex).Hide()
    End Sub

    ''' <summary>
    ''' 隱藏指定範圍的欄位
    ''' </summary>
    ''' <param name="startColumn">起始欄位索引</param>
    ''' <param name="endColumn">結束欄位索引</param>
    Public Sub HideColumns(startColumn As Integer, endColumn As Integer)
        Worksheet.Columns(startColumn, endColumn).Hide()
    End Sub

    ''' <summary>
    ''' 設定指定行的列高。
    ''' </summary>
    ''' <param name="rowIndex">行的索引，從 1 開始。</param>
    ''' <param name="height_cm">列高值。</param>
    Public Sub SetRowHeight(rowIndex As Integer, height_cm As Double)
        Worksheet.Row(rowIndex).Height = CmToPoints(height_cm)
    End Sub

    Public Sub SelectWorksheet(sheetName As String)
        Try
            Worksheet = workbook.Worksheet(sheetName)
        Catch ex As Exception
            Console.WriteLine($"Sheet '{sheetName}' not found: {ex.Message}")
        End Try
    End Sub

    Public Sub WriteToCell(rowIndex As Integer, columnIndex As Integer, content As Object, Optional formatOptions As CellFormatOptions = Nothing)
        Dim cell = Worksheet.Cell(rowIndex, columnIndex)
        cell.Value = XLCellValue.FromObject(content)

        If formatOptions IsNot Nothing Then
            SetCellFormat(cell, formatOptions)
        End If
    End Sub

    Public Sub WriteToCell(cellAddress As String, content As Object, Optional formatOptions As CellFormatOptions = Nothing)
        Try
            Dim cell = Worksheet.Cell(cellAddress)
            cell.Value = XLCellValue.FromObject(content)

            If formatOptions IsNot Nothing Then
                SetCellFormat(cell, formatOptions)
            End If
        Catch ex As Exception
            Throw New Exception($"寫入單元格 {cellAddress} 時發生錯誤: {ex.Message}", ex)
        End Try
    End Sub

    Public Sub WriteToCell(columnName As String, rowIndex As Integer, content As Object, Optional formatOptions As CellFormatOptions = Nothing)
        Try
            Dim cell = Worksheet.Cell(rowIndex, columnName)
            cell.Value = XLCellValue.FromObject(content)

            If formatOptions IsNot Nothing Then
                SetCellFormat(cell, formatOptions)
            End If
        Catch ex As Exception
            Throw New Exception($"寫入單元格 {columnName}{rowIndex} 時發生錯誤: {ex.Message}", New Exception(ex.Message))
        End Try
    End Sub

    ''' <summary>
    ''' 寫入公式
    ''' </summary>
    ''' <param name="columnName">欄位名稱 (如 A, B, C...)</param>
    ''' <param name="rowIndex">列索引</param>
    ''' <param name="formula">公式內容 (例如 SUM(C4:C10))</param>
    ''' <param name="formatOptions">格式選項</param>
    Public Sub WriteFormula(columnName As String, rowIndex As Integer, formula As String, Optional formatOptions As CellFormatOptions = Nothing)
        Try
            Dim cell = Worksheet.Cell(rowIndex, columnName)
            cell.FormulaA1 = formula

            If formatOptions IsNot Nothing Then
                SetCellFormat(cell, formatOptions)
            End If
        Catch ex As Exception
            Throw New Exception($"寫入公式 {formula} 於 {columnName}{rowIndex} 時發生錯誤: {ex.Message}", ex)
        End Try
    End Sub


    Public Function SaveAs(filePath As String) As Boolean
        Try
            workbook.SaveAs(filePath)
            MsgBox("報表建立成功!")
            Return True
        Catch ex As Exception
            MsgBox($"Error saving file: {ex.Message}")
        End Try

        Return False
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        workbook?.Dispose()
    End Sub

    Public Sub SetCustomBorders(startRowIndex As Integer, startColIndex As Integer, endRowIndex As Integer, endColIndex As Integer,
                            Optional topStyle As XLBorderStyleValues? = Nothing,
                            Optional bottomStyle As XLBorderStyleValues? = Nothing,
                            Optional leftStyle As XLBorderStyleValues? = Nothing,
                            Optional rightStyle As XLBorderStyleValues? = Nothing)

        Dim range = Worksheet.Range(startRowIndex, startColIndex, endRowIndex, endColIndex)

        If topStyle.HasValue Then
            range.Style.Border.TopBorder = topStyle.Value
        End If

        If bottomStyle.HasValue Then
            range.Style.Border.BottomBorder = bottomStyle.Value
        End If

        If leftStyle.HasValue Then
            range.Style.Border.LeftBorder = leftStyle.Value
        End If

        If rightStyle.HasValue Then
            range.Style.Border.RightBorder = rightStyle.Value
        End If
    End Sub

    Public Class CellFormatOptions
        Public Property FontName As String = "新細明體"

        Public Property FontSize As Double = 11

        Public Property IsBold As Boolean = False

        ''' <summary>
        ''' 水平
        ''' </summary>
        ''' <returns></returns>
        Public Property Horizontal As XLAlignmentHorizontalValues

        ''' <summary>
        ''' 垂直置中
        ''' </summary>
        ''' <returns></returns>
        Public Property VerticalCenter As Boolean = False

        ''' <summary>
        ''' 自動換行
        ''' </summary>
        ''' <returns></returns>
        Public Property WrapText As Boolean = False

        ''' <summary>
        ''' 垂直文字
        ''' </summary>
        ''' <returns></returns>
        Public Property VerticalText As Boolean = False
    End Class

    ''' <summary>
    ''' 在指定位置插入一列
    ''' </summary>
    ''' <param name="rowIndex">要插入新列的位置,從1開始</param>
    Public Sub InsertRow(rowIndex As Integer)
        Try
            Worksheet.Row(rowIndex).InsertRowsBelow(1)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' 在指定位置下方插入多列
    ''' </summary>
    ''' <param name="rowIndex">要插入新列的位置,從1開始</param>
    ''' <param name="count">要插入的列數</param>
    Public Sub InsertRowsBelow(rowIndex As Integer, count As Integer)
        Try
            Worksheet.Row(rowIndex).InsertRowsBelow(count)
        Catch ex As Exception
            Throw New Exception($"在第 {rowIndex} 行下方插入 {count} 列時發生錯誤: {ex.Message}", ex)
        End Try
    End Sub

    ''' <summary>
    ''' 複製指定範圍到目標位置（包含格式、合併儲存格等）
    ''' </summary>
    ''' <param name="sourceRange">來源範圍，例如 "A5:G7"</param>
    ''' <param name="targetStartCell">目標起始位置，例如 "A8"</param>
    Public Sub CopyRange(sourceRange As String, targetStartCell As String)
        Try
            Dim source = Worksheet.Range(sourceRange)
            Dim target = Worksheet.Range(targetStartCell)
            source.CopyTo(target)
        Catch ex As Exception
            Throw New Exception($"複製範圍 {sourceRange} 到 {targetStartCell} 時發生錯誤: {ex.Message}", ex)
        End Try
    End Sub

    ''' <summary>
    ''' 複製指定行範圍到目標位置並插入新行
    ''' </summary>
    ''' <param name="startRow">來源起始行</param>
    ''' <param name="endRow">來源結束行</param>
    ''' <param name="targetRow">目標起始行</param>
    Public Sub CopyAndInsertRows(startRow As Integer, endRow As Integer, targetRow As Integer)
        Try
            Dim rowCount = endRow - startRow + 1
            ' 先插入足夠的行
            Worksheet.Row(targetRow).InsertRowsBelow(rowCount)
            ' 然後複製格式和內容
            Dim sourceRange = Worksheet.Range(startRow, 1, endRow, Worksheet.ColumnsUsed().Count())
            Dim targetRange = Worksheet.Range(targetRow + 1, 1, targetRow + rowCount, Worksheet.ColumnsUsed().Count())
            sourceRange.CopyTo(targetRange)
        Catch ex As Exception
            Throw New Exception($"複製並插入行 {startRow}:{endRow} 到第 {targetRow} 行時發生錯誤: {ex.Message}", ex)
        End Try
    End Sub

    Public Sub Print(filePath As String, printerName As String, Optional repeatFromRow As Integer = 0, Optional repeatToRow As Integer = 0)
        With Worksheet.PageSetup
            If repeatFromRow > 0 AndAlso repeatToRow > 0 Then
                .SetRowsToRepeatAtTop(repeatFromRow, repeatToRow)
                workbook.SaveAs(filePath)
            End If
        End With

        Dim app As Application = Nothing
        Dim wb As Workbook = Nothing

        Try
            app = New Application With {
                .DisplayAlerts = False
            }

            wb = app.Workbooks.Open(filePath)

            wb.PrintOutEx(
                Preview:=False,
                ActivePrinter:=printerName,
                Collate:=True
            )
        Catch ex As Exception
            Throw
        Finally
            If wb IsNot Nothing Then
                wb.Close(SaveChanges:=False)
                Runtime.InteropServices.Marshal.ReleaseComObject(wb)
                wb = Nothing
            End If

            If app IsNot Nothing Then
                app.Quit()
                Runtime.InteropServices.Marshal.ReleaseComObject(app)
                app = Nothing
            End If
        End Try
    End Sub

    Public Sub MergeCells(startRowIndex As Integer, startColIndex As Integer, endRowIndex As Integer, endColIndex As Integer)
        Try
            Worksheet.Range(startRowIndex, startColIndex, endRowIndex, endColIndex).Merge()
        Catch ex As Exception
            Throw New Exception($"合併儲存格時發生錯誤: {ex.Message}", ex)
        End Try
    End Sub

    ''' <summary>
    ''' 另存新檔
    ''' </summary>
    ''' <param name="fileName">檔案名稱</param>
    ''' <returns></returns>
    Public Function SaveExcel(fileName As String) As String
        Using saveDialog As New SaveFileDialog
            With saveDialog
                .Filter = "Excel檔案|*.xlsx"
                .FileName = fileName + ".xlsx"
            End With

            If saveDialog.ShowDialog = DialogResult.OK Then
                SaveAs(saveDialog.FileName)
                Return saveDialog.FileName
            End If

            Return Nothing
        End Using
    End Function

    Private Sub SetCellFormat(cell As IXLCell, formatOptions As CellFormatOptions)
        cell.Style.Font.FontName = formatOptions.FontName
        cell.Style.Font.FontSize = formatOptions.FontSize
        cell.Style.Font.Bold = formatOptions.IsBold
        cell.Style.Alignment.Horizontal = formatOptions.Horizontal
        cell.Style.Alignment.Vertical = If(formatOptions.VerticalCenter, XLAlignmentVerticalValues.Center, XLAlignmentVerticalValues.Top)
        cell.Style.Alignment.WrapText = formatOptions.WrapText

        If formatOptions.VerticalText Then cell.Style.Alignment.TextRotation = 255
    End Sub

    Private Function CmToPoints(cm As Double) As Double
        Return cm * 28.35
    End Function
End Class
