Imports System.IO
Imports ClosedXML.Excel

Public Class SurplusGasPresenter
    Private _view As ISurplusGasView

    Private ReadOnly _sgRep As ISurplusGasRep
    Private ReadOnly _reportRep As IReportRep
    Private currentData As surplus_gas

    Public Sub New(sgRep As ISurplusGasRep, reportRep As IReportRep)
        _sgRep = sgRep
        _reportRep = reportRep
    End Sub

    Public Sub SetView(view As ISurplusGasView)
        _view = view
        AddHandler _view.Loaded, AddressOf OnLoaded
        AddHandler _view.SearchClicked, AddressOf OnSearchClicked
        AddHandler _view.CancelClicked, AddressOf Reset
        AddHandler _view.AddClicked, AddressOf OnAddClicked
        AddHandler _view.EditClicked, AddressOf OnEditClicked
        AddHandler _view.DeleteClicked, AddressOf OnDeleteClicked
        AddHandler _view.RowSelected, AddressOf OnRowSelected
        AddHandler _view.PrintClicked, AddressOf OnPrintClicked
    End Sub

    Private Sub OnLoaded()
        LoadList()
        _view.ButtonControl(True)
    End Sub

    Private Sub OnSearchClicked()
        Dim criteria = _view.GetSearchCriteria
        LoadList(criteria)
    End Sub

    Private Sub Reset()
        _view.ClearInput()
        LoadList()
        _view.ButtonControl(True)
        currentData = Nothing
    End Sub

    Private Sub LoadList(Optional criteria As Object = Nothing)
        Try
            Dim data = _sgRep.GetList(criteria)
            _view.DisplayList(data)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Async Sub OnAddClicked()
        Try
            Dim data = _view.GetInput
            Await _sgRep.AddAsync(data)
            Reset()
            MessageBox.Show("新增成功")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub OnRowSelected(sender As Object, id As Integer)
        Try
            Dim data = _sgRep.GetByIdAsync(id).Result
            If data IsNot Nothing Then
                _view.DisplayDetail(data)
                currentData = data
                _view.ButtonControl(False)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub OnEditClicked()
        If currentData Is Nothing Then
            MessageBox.Show("請選擇一筆資料進行修改")
            Return
        End If

        Try
            Dim inputData = _view.GetInput()
            inputData.sg_Id = currentData.sg_Id ' 保持ID不變
            _sgRep.UpdateAsync(currentData, inputData)
            Reset()
            MessageBox.Show("修改成功")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub OnDeleteClicked()
        If currentData Is Nothing Then
            MessageBox.Show("請選擇一筆資料進行刪除")
            Return
        End If

        Dim result = MessageBox.Show("確定要刪除這筆資料嗎？", "確認刪除", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Try
                _sgRep.DeleteAsync(currentData.sg_Id)
                Reset()
                MessageBox.Show("刪除成功")
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub

    Private Sub OnPrintClicked()
        Try
            If currentData Is Nothing Then
                MessageBox.Show("請選擇一筆資料進行列印")
                Return
            End If

            ' 取得資料
            Dim datas = _reportRep.GetSurplusGasReport(currentData)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "結餘氣範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    .WriteToCell("A1", datas.Month.ToString("MMM份"))

                    ' 寫入進貨公司
                    Dim purchaseRowIndex = 2
                    For Each item In datas.PurchaseDetails
                        .WriteToCell($"B{purchaseRowIndex}", item.CompanyName, New CloseXML_Excel.CellFormatOptions With {
                                     .Horizontal = XLAlignmentHorizontalValues.Center,
                                     .VerticalCenter = True
                        })
                        .MergeCells(purchaseRowIndex, 2, purchaseRowIndex + 1, 2)
                        .WriteToCell($"C{purchaseRowIndex}", "普")
                        .WriteToCell($"D{purchaseRowIndex}", item.Gas)
                        .WriteToCell($"C{purchaseRowIndex + 1}", "丙")
                        .WriteToCell($"D{purchaseRowIndex + 1}", item.Gas_C)
                        purchaseRowIndex += 2
                    Next

                    ' 寫進當月庫存
                    .WriteToCell("G2", datas.Platform)
                    .WriteToCell("G3", datas.Platform_C)
                    .WriteToCell("G4", datas.Slot)
                    .WriteToCell("G5", datas.Slot_C)
                    .WriteToCell("G6", datas.Car)
                    .WriteToCell("G7", datas.Car_C)
                    .WriteToCell("G8", datas.Sell)

                    ' 寫入總計
                    Dim totalRowIndex = 9
                    If purchaseRowIndex > totalRowIndex Then totalRowIndex = purchaseRowIndex

                    ' 畫線
                    .SetCustomBorders(2, 2, totalRowIndex - 1, 8, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                    .SetCustomBorders(2, 2, totalRowIndex - 1, 2, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin)
                    .SetCustomBorders(2, 6, totalRowIndex - 1, 6, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin)
                    .SetCustomBorders(2, 8, totalRowIndex - 1, 8, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Medium)
                    .SetCustomBorders(1, 2, 1, 2, XLBorderStyleValues.Thin, XLBorderStyleValues.Medium, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin)
                    .SetCustomBorders(1, 5, 1, 5, XLBorderStyleValues.Thin, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Medium)
                    .SetCustomBorders(1, 6, 1, 6, XLBorderStyleValues.Thin, XLBorderStyleValues.Medium, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin)
                    .SetCustomBorders(1, 7, 1, 7, XLBorderStyleValues.Thin, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                    .SetCustomBorders(1, 8, 1, 8, XLBorderStyleValues.Thin, XLBorderStyleValues.Medium, XLBorderStyleValues.Thin, XLBorderStyleValues.Medium)

                    .WriteToCell($"B{totalRowIndex}", "合計")
                    .MergeCells(totalRowIndex, 2, totalRowIndex, 3)
                    Dim totalPurchase = datas.PurchaseDetails.Sum(Function(x) x.Gas + x.Gas_C)
                    .WriteToCell($"D{totalRowIndex}", totalPurchase)
                    .WriteToCell($"B{totalRowIndex + 1}", "提氣")
                    .MergeCells(totalRowIndex + 1, 2, totalRowIndex + 1, 3)
                    .WriteToCell($"D{totalRowIndex + 1}", datas.TotalOrder)
                    .WriteToCell($"B{totalRowIndex + 2}", "進出餘氣")
                    .MergeCells(totalRowIndex + 2, 2, totalRowIndex + 2, 3)
                    .WriteToCell($"D{totalRowIndex + 2}", totalPurchase - datas.TotalOrder)
                    .WriteToCell($"F{totalRowIndex + 2}", "合計")
                    Dim totalStock = datas.Platform + datas.Platform_C + datas.Slot + datas.Slot_C + datas.Car + datas.Car_C + datas.Sell
                    .WriteToCell($"G{totalRowIndex + 2}", totalStock)
                    .WriteToCell($"B{totalRowIndex + 3}", "結餘氣")
                    .MergeCells(totalRowIndex + 3, 2, totalRowIndex + 3, 3)
                    .WriteToCell($"D{totalRowIndex + 3}", datas.LastMonthSurplus + totalPurchase - datas.TotalOrder - totalStock)

                    ' 畫線
                    .SetCustomBorders(totalRowIndex, 2, totalRowIndex + 3, 8, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                    .SetCustomBorders(totalRowIndex, 2, totalRowIndex, 5, XLBorderStyleValues.Medium)
                    .SetCustomBorders(totalRowIndex, 2, totalRowIndex + 3, 2, leftStyle:=XLBorderStyleValues.Medium)
                    .SetCustomBorders(totalRowIndex, 6, totalRowIndex + 3, 6, leftStyle:=XLBorderStyleValues.Medium)
                    .SetCustomBorders(totalRowIndex, 8, totalRowIndex + 3, 8, rightStyle:=XLBorderStyleValues.Medium)
                    .SaveExcel($"{datas.Month:MMM份}結餘氣")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
