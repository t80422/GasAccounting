Imports System.IO

Public Class ScrapBarrelPresenter
    Private _view As IScrapBarrelView
    Private _scrapBarrelRep As IScrapBarrelRep
    Private _currentData As scrap_barrel

    Public Sub New(scrapBarrelRep As IScrapBarrelRep)
        _scrapBarrelRep = scrapBarrelRep
    End Sub

    Public Sub SetView(view As IScrapBarrelView)
        _view = view
    End Sub

    Public Sub Reset()
        Try
            _view.ClearInput()
            LoadList()
            _currentData = Nothing
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub LoadList()
        Try
            Dim data = _scrapBarrelRep.GetAllAsync().Result
            Dim result = data.OrderByDescending(Function(x) x.sb_Month).
                              Select(Function(x) New ScrapBarrelVM(x)).ToList()
            _view.ShowList(result)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Add()
        Try
            Dim data = _view.GetInput
            _scrapBarrelRep.AddAsync(data)
            Reset()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub LoadDetail(id As Integer)
        Try
            Dim data = _scrapBarrelRep.GetByIdAsync(id).Result
            _currentData = data
            _view.ClearInput()
            _view.ShowDetail(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Update()
        Try
            If _currentData Is Nothing Then
                MsgBox("No data selected for update.")
                Return
            End If
            Dim updatedData = _view.GetInput()
            updatedData.sb_Id = _currentData.sb_Id ' Ensure the ID remains the same
            _scrapBarrelRep.UpdateAsync(_currentData.sb_Id, updatedData)
            Reset()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Delete()
        Try
            If _currentData Is Nothing Then
                MsgBox("No data selected for deletion.")
                Return
            End If
            _scrapBarrelRep.DeleteAsync(_currentData.sb_Id)
            Reset()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Print(sbId As Integer)
        Try
            If _currentData Is Nothing Then
                MsgBox("請先選擇一條記錄")
                Return
            End If

            '取得資料
            Dim data = _scrapBarrelRep.GetByIdAsync(_currentData.sb_Id).Result

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "報廢桶範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    ' 標題
                    Dim title = $"{data.sb_Month.Value:yyyy年MM月報廢桶}"
                    .WriteToCell("A1", title)

                    ' 單價
                    .WriteToCell("C3", data.sb_Acquisitions50.ToString)
                    .WriteToCell("C4", data.sb_Buy50.ToString)
                    .WriteToCell("D3", data.sb_Acquisitions20.ToString)
                    .WriteToCell("D4", data.sb_Buy20.ToString)
                    .WriteToCell("E3", data.sb_Acquisitions16.ToString)
                    .WriteToCell("E4", data.sb_Buy16.ToString)
                    .WriteToCell("F3", data.sb_Acquisitions10.ToString)
                    .WriteToCell("F4", data.sb_Buy10.ToString)
                    .WriteToCell("G3", data.sb_Acquisitions4.ToString)
                    .WriteToCell("G4", data.sb_Buy4.ToString)

                    ' 客戶資料動態插入（複製範本行）
                    Dim templateStartRow As Integer = 5 ' 範本行起始位置
                    Dim templateEndRow As Integer = 7   ' 範本行結束位置
                    Dim currentRow As Integer = templateStartRow

                    For i As Integer = 0 To data.scrap_barrel_detail.Count - 1
                        Dim sbd = data.scrap_barrel_detail(i)

                        ' 如果不是第一個客戶，需要複製範本行
                        If i > 0 Then
                            ' 複製範本行（A5:H7）到新位置
                            .CopyAndInsertRows(templateStartRow, templateEndRow, currentRow - 1)
                        End If

                        ' 寫入客戶資料
                        .WriteToCell($"A{currentRow}", sbd.customer.cus_name)

                        ' 數量行
                        .WriteToCell($"C{currentRow}", sbd.sbd_Qty50.ToString)
                        .WriteToCell($"D{currentRow}", sbd.sbd_Qty20.ToString)
                        .WriteToCell($"E{currentRow}", sbd.sbd_Qty16.ToString)
                        .WriteToCell($"F{currentRow}", sbd.sbd_Qty10.ToString)
                        .WriteToCell($"G{currentRow}", sbd.sbd_Qty4.ToString)
                        .WriteToCell($"H{currentRow}", (sbd.sbd_Qty50 + sbd.sbd_Qty20 + sbd.sbd_Qty16 + sbd.sbd_Qty10 + sbd.sbd_Qty4).ToString)

                        ' 買價金額行
                        Dim buy50 = sbd.sbd_Qty50 * data.sb_Buy50
                        Dim buy20 = sbd.sbd_Qty20 * data.sb_Buy20
                        Dim buy16 = sbd.sbd_Qty16 * data.sb_Buy16
                        Dim buy10 = sbd.sbd_Qty10 * data.sb_Buy10
                        Dim buy4 = sbd.sbd_Qty4 * data.sb_Buy4
                        .WriteToCell($"C{currentRow + 1}", buy50.ToString)
                        .WriteToCell($"D{currentRow + 1}", buy20.ToString)
                        .WriteToCell($"E{currentRow + 1}", buy16.ToString)
                        .WriteToCell($"F{currentRow + 1}", buy10.ToString)
                        .WriteToCell($"G{currentRow + 1}", buy4.ToString)
                        .WriteToCell($"H{currentRow + 1}", (buy50 + buy20 + buy16 + buy10 + buy4).ToString)

                        ' 收購價金額行
                        Dim acq50 = sbd.sbd_Qty50 * data.sb_Acquisitions50
                        Dim acq20 = sbd.sbd_Qty20 * data.sb_Acquisitions20
                        Dim acq16 = sbd.sbd_Qty16 * data.sb_Acquisitions16
                        Dim acq10 = sbd.sbd_Qty10 * data.sb_Acquisitions10
                        Dim acq4 = sbd.sbd_Qty4 * data.sb_Acquisitions4
                        .WriteToCell($"C{currentRow + 2}", acq50.ToString)
                        .WriteToCell($"D{currentRow + 2}", acq20.ToString)
                        .WriteToCell($"E{currentRow + 2}", acq16.ToString)
                        .WriteToCell($"F{currentRow + 2}", acq10.ToString)
                        .WriteToCell($"G{currentRow + 2}", acq4.ToString)
                        .WriteToCell($"H{currentRow + 2}", (acq50 + acq20 + acq16 + acq10 + acq4).ToString)

                        ' 移動到下一個客戶的起始行
                        currentRow += 3
                    Next



                    '存檔
                    .SaveExcel(title)
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
