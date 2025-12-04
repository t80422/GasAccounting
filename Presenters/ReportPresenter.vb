Imports System.IO
Imports System.Security.AccessControl
Imports System.Windows.Interop
Imports ClosedXML.Excel

Public Class ReportPresenter
    Property _view As IReportView
    Private ReadOnly _rep As IReportRep
    Private ReadOnly _manuSer As IManufacturerService
    Private ReadOnly _bankRep As IBankRep
    Private ReadOnly _compRep As ICompanyRep
    Private ReadOnly _colRep As ICollectionRep
    Private ReadOnly _printerSer As IPrinterService
    Private ReadOnly _cusRep As ICustomerRep

    Private tempDate As String ' 暫存日期,報表出現單一日期用

    Public Sub New(reportRep As IReportRep, bankRep As IBankRep, compRep As ICompanyRep, printerSer As IPrinterService, colRep As ICollectionRep,
                   manuSer As IManufacturerService, cusRep As ICustomerRep)
        _rep = reportRep
        _bankRep = bankRep
        _compRep = compRep
        _printerSer = printerSer
        _colRep = colRep
        _manuSer = manuSer
        _cusRep = cusRep
    End Sub

    Public Sub SetView(view As IReportView)
        _view = view
    End Sub

    Public Async Sub LoadBankAccount()
        Try
            Dim items = Await _bankRep.GetBankDropdownAsync
            _view.SetBankAccountCmb(items)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub LoadCompany()
        Try
            Dim items = Await _compRep.GetCompanyDropdownAsync
            _view.SetCompanyCmb(items)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub GetManuCmb()
        _view.SetGasVendorCmb(_manuSer.GetGasVendorCmbItems())
    End Sub

    ''' <summary>
    ''' 產生客戶提氣清冊
    ''' </summary>
    ''' <param name="d"></param>
    Public Sub GenerateCustomersGetGasList(d As Date, isMonth As Boolean)
        Try
            '蒐集資料
            Dim datas = _rep.CustomersGetGasList(d, isMonth)

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "客戶提氣清冊範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    ' 設定表頭
                    Dim ws = .Worksheet

                    ws.PageSetup.PrintAreas.Clear()
                    ws.PageSetup.SetRowsToRepeatAtTop(1, 5)

                    ws.PageSetup.PaperSize = XLPaperSize.A4Paper
                    ws.PageSetup.Margins.Top = 0.1
                    ws.PageSetup.Margins.Bottom = 0.5
                    ws.PageSetup.Margins.Left = 0.1
                    ws.PageSetup.Margins.Right = 0.1

                    Dim titleStyle = New CloseXML_Excel.CellFormatOptions With {
                        .FontSize = 14,
                        .IsBold = True,
                        .Horizontal = XLAlignmentHorizontalValues.Center
                    }

                    .MergeCells(1, 1, 1, 25)
                    .WriteToCell(1, 1, "豐原液化煤氣分裝場", titleStyle)

                    .MergeCells(2, 1, 2, 25)
                    .WriteToCell(2, 1, "客戶提氣量清單", titleStyle)

                    Dim dateStyle = New CloseXML_Excel.CellFormatOptions With {
                            .Horizontal = XLAlignmentHorizontalValues.Left
                        }
                    If isMonth Then
                        .WriteToCell(3, 1, $"提氣日期: {d:yyyy/MM}", dateStyle)
                    Else
                        .WriteToCell(3, 1, $"提氣日期: {d:yyyy/MM/dd}", dateStyle)
                    End If

                    Dim printDateStyle = New CloseXML_Excel.CellFormatOptions With {
                            .Horizontal = XLAlignmentHorizontalValues.Right
                        }

                    .WriteToCell(3, 25, $"列印日期: {Now:yyyy/MM/dd}", printDateStyle)

                    ws.PageSetup.Footer.Center.AddText("第 &P 頁/共 &N 頁")

                    Dim rowIndex As Integer

                    For i As Integer = 0 To datas.Count - 1
                        rowIndex = 6 + i

                        .SetRowHeight(rowIndex, 0.78)
                        .InsertRow(rowIndex)

                        .WriteToCell(rowIndex, 1, datas(i).客戶名稱)
                        .WriteToCell(rowIndex, 2, If(datas(i).普氣50Kg <> Nothing, datas(i).普氣50Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 3, If(datas(i).丙氣50Kg <> Nothing, datas(i).丙氣50Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 4, If(datas(i).普氣20Kg <> Nothing, datas(i).普氣20Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 5, If(datas(i).丙氣20Kg <> Nothing, datas(i).丙氣20Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 6, If(datas(i).普氣16Kg <> Nothing, datas(i).普氣16Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 7, If(datas(i).丙氣16Kg <> Nothing, datas(i).丙氣16Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 8, If(datas(i).普氣10Kg <> Nothing, datas(i).普氣10Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 9, If(datas(i).丙氣10Kg <> Nothing, datas(i).丙氣10Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 10, If(datas(i).普氣4Kg <> Nothing, datas(i).普氣4Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 11, If(datas(i).丙氣4Kg <> Nothing, datas(i).丙氣4Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 12, If(datas(i).普氣18Kg <> Nothing, datas(i).普氣18Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 13, If(datas(i).丙氣18Kg <> Nothing, datas(i).丙氣18Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 14, If(datas(i).普氣14Kg <> Nothing, datas(i).普氣14Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 15, If(datas(i).丙氣14Kg <> Nothing, datas(i).丙氣14Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 16, If(datas(i).普氣5Kg <> Nothing, datas(i).普氣5Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 17, If(datas(i).丙氣5Kg <> Nothing, datas(i).丙氣5Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 18, If(datas(i).普氣2Kg <> Nothing, datas(i).普氣2Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 19, If(datas(i).丙氣2Kg <> Nothing, datas(i).丙氣2Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 20, If(datas(i).普氣殘氣 <> Nothing, datas(i).普氣殘氣.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 21, If(datas(i).丙氣殘氣 <> Nothing, datas(i).丙氣殘氣.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 22, If(datas(i).普氣提量 <> Nothing, datas(i).普氣提量.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 23, If(datas(i).丙氣提量 <> Nothing, datas(i).丙氣提量.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 24, If(datas(i).普氣實提量 <> Nothing, datas(i).普氣實提量.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 25, If(datas(i).丙氣實提量 <> Nothing, datas(i).丙氣實提量.ToString("#,##"), ""))
                    Next

                    '合計
                    rowIndex += 1

                    Dim totalStyle = New CloseXML_Excel.CellFormatOptions With {
                        .IsBold = True
                    }

                    .SetRowHeight(rowIndex, 0.78)

                    .WriteToCell(rowIndex, 1, "合計", totalStyle)
                    .WriteToCell(rowIndex, 2, datas.Sum(Function(x) x.普氣50Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 3, datas.Sum(Function(x) x.丙氣50Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 4, datas.Sum(Function(x) x.普氣20Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 5, datas.Sum(Function(x) x.丙氣20Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 6, datas.Sum(Function(x) x.普氣16Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 7, datas.Sum(Function(x) x.丙氣16Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 8, datas.Sum(Function(x) x.普氣10Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 9, datas.Sum(Function(x) x.丙氣10Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 10, datas.Sum(Function(x) x.普氣4Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 11, datas.Sum(Function(x) x.丙氣4Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 12, datas.Sum(Function(x) x.普氣18Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 13, datas.Sum(Function(x) x.丙氣18Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 14, datas.Sum(Function(x) x.普氣14Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 15, datas.Sum(Function(x) x.丙氣14Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 16, datas.Sum(Function(x) x.普氣5Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 17, datas.Sum(Function(x) x.丙氣5Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 18, datas.Sum(Function(x) x.普氣2Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 19, datas.Sum(Function(x) x.丙氣2Kg).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 20, datas.Sum(Function(x) x.普氣殘氣).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 21, datas.Sum(Function(x) x.丙氣殘氣).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 22, datas.Sum(Function(x) x.普氣提量).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 23, datas.Sum(Function(x) x.丙氣提量).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 24, datas.Sum(Function(x) x.普氣實提量).ToString("#,##"), totalStyle)
                    .WriteToCell(rowIndex, 25, datas.Sum(Function(x) x.丙氣實提量).ToString("#,##"), totalStyle)

                    '存檔
                    If isMonth Then
                        .SaveExcel($"客戶提氣清冊_{d:yyyyMM}")
                    Else
                        .SaveExcel($"客戶提氣清冊_{d:yyyyMMdd}")
                    End If

                End With
            End Using
        Catch ex As Exception
            MsgBox("列印失敗:" + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生大氣進貨明細
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="manuId"></param>
    Public Sub GenerateGasPayableDetail(d As Date, manuId As Integer)
        Try
            '蒐集資料
            Dim datas = _rep.GasPayableDetailList(d, manuId)

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "大氣進貨明細範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim vendorName = datas.FirstOrDefault().廠商
                    .WriteToCell(1, 1, $"{d:yyyy}年豐原液化煤氣分裝場應付明細帳")
                    .WriteToCell(3, 1, vendorName)

                    Dim rowIndex As Integer

                    For i As Integer = 0 To datas.Count - 1
                        rowIndex = 3 + i

                        .WriteToCell(rowIndex, 2, datas(i).日期)
                        .WriteToCell(rowIndex, 3, If(datas(i).普氣 <> Nothing, datas(i).普氣, ""))
                        .WriteToCell(rowIndex, 4, If(datas(i).普氣單價 <> Nothing, datas(i).普氣單價.ToString("N2"), ""))
                        .WriteToCell(rowIndex, 5, If(datas(i).普氣金額 <> Nothing, datas(i).普氣金額, ""))
                        .WriteToCell(rowIndex, 6, If(datas(i).丙氣 <> Nothing, datas(i).丙氣, ""))
                        .WriteToCell(rowIndex, 7, If(datas(i).丙氣單價 <> Nothing, datas(i).丙氣單價.ToString("N2"), ""))
                        .WriteToCell(rowIndex, 8, If(datas(i).丙氣金額 <> Nothing, datas(i).丙氣金額, ""))
                        .WriteToCell(rowIndex, 9, If(datas(i).總計 <> Nothing, datas(i).總計, ""))
                        .WriteToCell(rowIndex, 10, If(datas(i).餘額 <> Nothing, datas(i).餘額, ""))
                        .WriteToCell(rowIndex, 11, If(datas(i).累計 <> Nothing, datas(i).累計, ""))
                    Next
                    .SetCustomBorders(rowIndex, 1, rowIndex, 11, XLBorderStyleValues.Thin)

                    rowIndex += 1
                    .WriteToCell(rowIndex, 2, "總計:")
                    .WriteToCell(rowIndex, 3, datas.Sum(Function(x) x.普氣).ToString)
                    .WriteToCell(rowIndex, 4, datas.Average(Function(x) x.普氣單價.ToString("N2")).ToString)
                    .WriteToCell(rowIndex, 5, datas.Sum(Function(x) x.普氣金額).ToString)
                    .WriteToCell(rowIndex, 6, datas.Sum(Function(x) x.丙氣).ToString)
                    .WriteToCell(rowIndex, 7, datas.Average(Function(x) x.丙氣單價.ToString("N2")).ToString)
                    .WriteToCell(rowIndex, 8, datas.Sum(Function(x) x.丙氣金額).ToString)
                    .WriteToCell(rowIndex, 9, datas.Sum(Function(x) x.總計).ToString)
                    .WriteToCell(rowIndex, 10, datas.Sum(Function(x) x.餘額).ToString)
                    .WriteToCell(rowIndex, 11, datas.Last().累計.ToString)

                    '存檔
                    .SaveExcel($"大氣進貨明細_{vendorName}_{d:yyyyMMdd}")

                End With
            End Using
        Catch ex As Exception
            MsgBox("列印失敗:" + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生客戶每日應收帳明細表
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="cusCode"></param>
    Public Sub GenerateMonthlyCustomerReceivable(d As Date, cusCode As String)
        Try
            '蒐集資料
            Dim datas As List(Of DailyCustomerReceivable)

            Using uow As New UnitOfWork
                datas = uow.ReportRepository.MonthlyCustomerReceivable(d, cusCode)
            End Using

            If datas Is Nothing OrElse datas.Count = 0 Then
                MessageBox.Show("查無資料")
                Return
            End If

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "單一客戶每月的應收帳明細表範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim cusName = datas.FirstOrDefault().客戶名稱
                    .WriteToCell(1, 1, $"{d:yyyy}年豐原液化煤氣分裝場氣款應收帳")
                    .WriteToCell("A2", cusName)

                    Dim rowIndex As Integer

                    For i As Integer = 0 To datas.Count - 1
                        '資料開始列
                        rowIndex = 4 + i

                        .InsertRow(rowIndex)

                        .WriteToCell(rowIndex, 1, datas(i).日期)
                        .WriteToCell(rowIndex, 2, datas(i).總提氣.ToString)

                        .WriteToCell(rowIndex, 3, datas(i).廠運總提氣.ToString)
                        .WriteToCell(rowIndex, 4, datas(i).廠運普氣.ToString)
                        .WriteToCell(rowIndex, 5, datas(i).廠運普氣退氣.ToString)
                        .WriteToCell(rowIndex, 6, datas(i).廠運普氣單價.ToString("N2"))
                        .WriteToCell(rowIndex, 7, datas(i).廠運普氣金額.ToString)
                        .WriteToCell(rowIndex, 8, datas(i).廠運丙氣.ToString)
                        .WriteToCell(rowIndex, 9, datas(i).廠運丙氣退氣.ToString)
                        .WriteToCell(rowIndex, 10, datas(i).廠運丙氣單價.ToString("N2"))
                        .WriteToCell(rowIndex, 11, datas(i).廠運丙氣金額.ToString)

                        .WriteToCell(rowIndex, 12, datas(i).自運總提氣.ToString)
                        .WriteToCell(rowIndex, 13, datas(i).自運普氣.ToString)
                        .WriteToCell(rowIndex, 14, datas(i).自運普氣退氣.ToString)
                        .WriteToCell(rowIndex, 15, datas(i).自運普氣單價.ToString("N2"))
                        .WriteToCell(rowIndex, 16, datas(i).自運普氣金額.ToString)
                        .WriteToCell(rowIndex, 17, datas(i).自運丙氣.ToString)
                        .WriteToCell(rowIndex, 18, datas(i).自運丙氣退氣.ToString)
                        .WriteToCell(rowIndex, 19, datas(i).自運丙氣單價.ToString("N2"))
                        .WriteToCell(rowIndex, 20, datas(i).自運丙氣金額.ToString)

                        .WriteToCell(rowIndex, 21, datas(i).總額.ToString)
                        .WriteToCell(rowIndex, 22, datas(i).現金.ToString)
                        .WriteToCell(rowIndex, 23, datas(i).票據.ToString)
                        .WriteToCell(rowIndex, 24, datas(i).掛帳.ToString)
                        .WriteToCell(rowIndex, 25, datas(i).累計.ToString)
                    Next

                    rowIndex += 1
                    Dim deliveryGas = datas.Sum(Function(x) x.廠運總提氣).ToString

                    .WriteToCell("A", rowIndex, "合計")
                    .WriteToCell("B", rowIndex, datas.Sum(Function(x) x.總提氣).ToString)
                    .WriteToCell("C", rowIndex, deliveryGas)
                    .WriteToCell("D", rowIndex, datas.Sum(Function(x) x.廠運普氣).ToString)
                    .WriteToCell("E", rowIndex, datas.Sum(Function(x) x.廠運普氣退氣).ToString)
                    .WriteToCell("F", rowIndex, datas.Average(Function(x) x.廠運普氣單價).ToString("N2"))
                    .WriteToCell("G", rowIndex, datas.Sum(Function(x) x.廠運普氣金額).ToString)
                    .WriteToCell("H", rowIndex, datas.Sum(Function(x) x.廠運丙氣).ToString)
                    .WriteToCell("I", rowIndex, datas.Sum(Function(x) x.廠運丙氣退氣).ToString)
                    .WriteToCell("J", rowIndex, datas.Average(Function(x) x.廠運丙氣單價).ToString("N2"))
                    .WriteToCell("K", rowIndex, datas.Sum(Function(x) x.廠運丙氣金額).ToString)

                    .WriteToCell("L", rowIndex, datas.Sum(Function(x) x.自運總提氣).ToString)
                    .WriteToCell("M", rowIndex, datas.Sum(Function(x) x.自運普氣).ToString)
                    .WriteToCell("N", rowIndex, datas.Sum(Function(x) x.自運普氣退氣).ToString)
                    .WriteToCell("O", rowIndex, datas.Average(Function(x) x.自運普氣單價).ToString("N2"))
                    .WriteToCell("P", rowIndex, datas.Sum(Function(x) x.自運普氣金額).ToString)
                    .WriteToCell("Q", rowIndex, datas.Sum(Function(x) x.自運丙氣).ToString)
                    .WriteToCell("R", rowIndex, datas.Sum(Function(x) x.自運丙氣退氣).ToString)
                    .WriteToCell("S", rowIndex, datas.Average(Function(x) x.自運丙氣單價).ToString("N2"))
                    .WriteToCell("T", rowIndex, datas.Sum(Function(x) x.自運丙氣金額).ToString)

                    .WriteToCell("U", rowIndex, datas.Sum(Function(x) x.總額).ToString)
                    .WriteToCell("V", rowIndex, datas.Sum(Function(x) x.現金).ToString)
                    .WriteToCell("W", rowIndex, datas.Sum(Function(x) x.票據).ToString)
                    .WriteToCell("X", rowIndex, datas.Sum(Function(x) x.掛帳).ToString)
                    .WriteToCell("Y", rowIndex, datas.Sum(Function(x) x.累計).ToString)

                    .SetCustomBorders(rowIndex, 1, rowIndex, 25, XLBorderStyleValues.Medium, XLBorderStyleValues.Medium)

                    ' 若沒有廠運就隱藏
                    If deliveryGas = 0 Then .HideColumns(3, 11)

                    '存檔
                    .SaveExcel($"單一客戶每月的應收帳明細表_{cusName}_{d:yyyyMM}")
                End With
            End Using
        Catch ex As Exception
            MessageBox.Show("產生客戶每日應收帳明細表 錯誤:" + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生提量支數統計
    ''' </summary>
    ''' <param name="month"></param>
    Public Sub GenerateGasUsageAndCylinderCount(month As Date)
        Try
            '蒐集資料
            Dim datas = _rep.GasUsageAndCylinderCount(month)

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "提氣支數統計範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    .WriteToCell("A1", $"{month:yyyy}年")

                    Dim rowIndex As Integer

                    For i As Integer = 0 To datas.Count - 1
                        '資料開始列
                        rowIndex = 3 + i

                        .WriteToCell($"A{rowIndex}", datas(i).日期)
                        .WriteToCell($"B{rowIndex}", datas(i).退氣.ToString)
                        .WriteToCell($"D{rowIndex}", datas(i).提氣量.ToString)
                        .WriteToCell($"H{rowIndex}", datas(i).瓦斯瓶50Kg.ToString)
                        .WriteToCell($"I{rowIndex}", datas(i).瓦斯瓶20Kg.ToString)
                        .WriteToCell($"J{rowIndex}", datas(i).瓦斯瓶16Kg.ToString)
                        .WriteToCell($"K{rowIndex}", datas(i).瓦斯瓶10Kg.ToString)
                        .WriteToCell($"L{rowIndex}", datas(i).瓦斯瓶4Kg.ToString)
                    Next

                    '存檔
                    .SaveExcel($"提氣支數統計_{month:yyyy年MM月}")
                End With
            End Using
        Catch ex As Exception
            MsgBox("產生提量支數統計 失敗:" + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生現金帳
    ''' </summary>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    Public Sub GenerateCashAccount(startDate As Date, endDate As Date)
        Try
            '取得資料
            Dim data = _rep.GetCashAccount(startDate, endDate)

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "現金帳範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    .WriteToCell(1, 1, $"{startDate:yyyy/MM/dd}~{endDate:yyyy/MM/dd}")

                    Dim rowIndex = 3

                    For i As Integer = 0 To data.Count - 1
                        .WriteToCell(rowIndex + i, 1, data(i).日期)
                        .WriteToCell(rowIndex + i, 2, data(i).科目)
                        .WriteToCell(rowIndex + i, 3, data(i).摘要)
                        .WriteToCell(rowIndex + i, 4, data(i).收入金額.ToString)
                        .WriteToCell(rowIndex + i, 5, data(i).支出金額.ToString)
                        .WriteToCell(rowIndex + i, 6, data(i).餘額.ToString)
                    Next

                    '存檔
                    Dim exportFilePath = .SaveExcel($"現金帳_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}")
                End With
            End Using
        Catch ex As Exception
            MsgBox("產生現金帳 失敗:" + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生銀行帳
    ''' </summary>
    ''' <param name="month"></param>
    ''' <param name="bankId"></param>
    Public Sub GenerateBankAccount(month As Date, bankId As Integer)
        Try
            '取得資料
            Dim data = _rep.GetBankAccount(month, bankId)

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "銀行帳範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    .WriteToCell(1, 1, $"{data.日期}")

                    Dim rowIndex = 3

                    For Each bankAccount In data.List
                        .WriteToCell(rowIndex, 1, bankAccount.日期.ToString("MM/dd"))
                        .WriteToCell(rowIndex, 2, bankAccount.科目)
                        .WriteToCell(rowIndex, 3, bankAccount.摘要)
                        .WriteToCell(rowIndex, 4, bankAccount.借方.ToString)
                        .WriteToCell(rowIndex, 5, bankAccount.貸方.ToString)
                        .WriteToCell(rowIndex, 6, bankAccount.餘額.ToString)
                        .SetCustomBorders(rowIndex, 1, rowIndex, 6, bottomStyle:=XLBorderStyleValues.Thin)
                        rowIndex += 1
                    Next

                    Dim totalDebit = data.List.Sum(Function(x) x.借方)
                    Dim totalCredit = data.List.Sum(Function(x) x.貸方)

                    .WriteToCell(rowIndex, 3, "合計")
                    .WriteToCell(rowIndex, 4, totalDebit.ToString)
                    .WriteToCell(rowIndex, 5, totalCredit.ToString)

                    '存檔
                    .SaveExcel($"銀行帳_{month:yyyy年MM月}")

                End With
            End Using
        Catch ex As Exception
            MsgBox("產生銀行帳發生錯誤:" + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生客戶寄桶結存瓶
    ''' </summary>
    ''' <param name="cusId"></param>
    Public Sub GenerateCustomerGasCylinderInventory(cusId As Integer)
        Try
            '取得資料
            Dim data = _rep.GetCustomerGasCylinderInventory(cusId)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "客戶寄桶結存瓶範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    .WriteToCell(1, 1, $"{data.CustomerName} 寄桶結存瓶")

                    Dim rowIndex = 3

                    For Each bankAccount In data.List
                        .WriteToCell(rowIndex, 1, bankAccount.CarNo)
                        .WriteToCell(rowIndex, 2, bankAccount.DriverName)
                        .WriteToCell(rowIndex, 3, bankAccount.瓦斯瓶50Kg.ToString)
                        .WriteToCell(rowIndex, 4, bankAccount.瓦斯瓶20Kg.ToString)
                        .WriteToCell(rowIndex, 5, bankAccount.瓦斯瓶16Kg.ToString)
                        .WriteToCell(rowIndex, 6, bankAccount.瓦斯瓶10Kg.ToString)
                        .WriteToCell(rowIndex, 7, bankAccount.瓦斯瓶4Kg.ToString)
                        .WriteToCell(rowIndex, 8, bankAccount.瓦斯瓶18Kg.ToString)
                        .WriteToCell(rowIndex, 9, bankAccount.瓦斯瓶14Kg.ToString)
                        .WriteToCell(rowIndex, 10, bankAccount.瓦斯瓶5Kg.ToString)
                        .WriteToCell(rowIndex, 11, bankAccount.瓦斯瓶2Kg.ToString)
                        .SetCustomBorders(rowIndex, 1, rowIndex, 11, bottomStyle:=XLBorderStyleValues.Thin)
                        rowIndex += 1
                    Next

                    '存檔
                    .SaveExcel($"客戶寄桶結存瓶_{data.CustomerName}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生新桶明細
    ''' </summary>
    ''' <param name="month"></param>
    Public Sub GenerateNewBarrelDetails(month As Date)
        Try
            Dim formatMonth = New Date(month.Year, month.Month, 1)

            '取得資料
            Dim data = _rep.GetNewBarrelDetails(formatMonth)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "新桶明細範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim total50 = data.Last50
                    Dim total20 = data.Last20
                    Dim total16 = data.Last16
                    Dim total10 = data.Last10
                    Dim total4 = data.Last4

                    '上期結餘
                    .WriteToCell(3, 12, total50.ToString)
                    .WriteToCell(3, 13, total20.ToString)
                    .WriteToCell(3, 14, total16.ToString)
                    .WriteToCell(3, 15, total10.ToString)
                    .WriteToCell(3, 16, total4.ToString)
                    .InsertRow(3)

                    '明細
                    Dim rowIndex = 4
                    Dim payAmount50 As Integer
                    Dim payAmount20 As Integer
                    Dim payAmount16 As Integer
                    Dim payAmount10 As Integer
                    Dim payAmount4 As Integer
                    Dim incomingAmount50 As Integer
                    Dim incomingAmount20 As Integer
                    Dim incomingAmount16 As Integer
                    Dim incomingAmount10 As Integer
                    Dim incomingAmount4 As Integer

                    For Each detail In data.List
                        Dim in50 = If(detail.In50.HasValue, detail.In50, 0)
                        Dim in20 = If(detail.In20.HasValue, detail.In20, 0)
                        Dim in16 = If(detail.In16.HasValue, detail.In16, 0)
                        Dim in10 = If(detail.In10.HasValue, detail.In10, 0)
                        Dim in4 = If(detail.In4.HasValue, detail.In4, 0)
                        Dim out50 = If(detail.Out50.HasValue, detail.Out50, 0)
                        Dim out20 = If(detail.Out20.HasValue, detail.Out20, 0)
                        Dim out16 = If(detail.Out16.HasValue, detail.Out16, 0)
                        Dim out10 = If(detail.Out10.HasValue, detail.Out10, 0)
                        Dim out4 = If(detail.Out4.HasValue, detail.Out4, 0)

                        .WriteToCell(rowIndex, 1, detail.Day)

                        '收入
                        .WriteToCell(rowIndex, 2, in50.ToString)
                        .WriteToCell(rowIndex, 3, in20.ToString)
                        .WriteToCell(rowIndex, 4, in16.ToString)
                        .WriteToCell(rowIndex, 5, in10.ToString)
                        .WriteToCell(rowIndex, 6, in4.ToString)

                        '支出
                        .WriteToCell(rowIndex, 7, out50.ToString)
                        .WriteToCell(rowIndex, 8, out20.ToString)
                        .WriteToCell(rowIndex, 9, out16.ToString)
                        .WriteToCell(rowIndex, 10, out10.ToString)
                        .WriteToCell(rowIndex, 11, out4.ToString)

                        '結餘
                        total50 += in50 - out50
                        total20 += in20 - out20
                        total16 += in16 - out16
                        total10 += in10 - out10
                        total4 += in4 - out4

                        .WriteToCell(rowIndex, 12, (in50 - out50).ToString)
                        .WriteToCell(rowIndex, 13, (in20 - out20).ToString)
                        .WriteToCell(rowIndex, 14, (in16 - out16).ToString)
                        .WriteToCell(rowIndex, 15, (in10 - out10).ToString)
                        .WriteToCell(rowIndex, 16, (in4 - out4).ToString)

                        .WriteToCell(rowIndex, 17, detail.Memo)
                        .WriteToCell(rowIndex, 18, detail.PayDate)

                        If detail.In50 <> 0 Then
                            .WriteToCell(rowIndex, 19, data.PayUnitPrice50.ToString)

                            Dim amount = data.PayUnitPrice50 * in50
                            .WriteToCell(rowIndex, 20, amount.ToString)
                            payAmount50 += amount
                        End If

                        If detail.In20 <> 0 Then
                            .WriteToCell(rowIndex, 21, data.PayUnitPrice20.ToString)

                            Dim amount = data.PayUnitPrice20 * in20
                            .WriteToCell(rowIndex, 22, amount.ToString)
                            payAmount20 += amount
                        End If

                        If detail.In16 <> 0 Then
                            .WriteToCell(rowIndex, 23, data.PayUnitPrice16.ToString)

                            Dim amount = data.PayUnitPrice16 * in16
                            .WriteToCell(rowIndex, 24, amount.ToString)
                            payAmount16 += amount
                        End If

                        If detail.In10 <> 0 Then
                            .WriteToCell(rowIndex, 25, data.PayUnitPrice10.ToString)

                            Dim amount = data.PayUnitPrice10 * in10
                            .WriteToCell(rowIndex, 26, amount.ToString)
                            payAmount10 += amount
                        End If

                        If detail.In4 <> 0 Then
                            .WriteToCell(rowIndex, 27, data.PayUnitPrice4.ToString)

                            Dim amount = data.PayUnitPrice4 * in4
                            .WriteToCell(rowIndex, 28, amount.ToString)
                            payAmount4 += amount
                        End If

                        If detail.Out50 <> 0 Then
                            .WriteToCell(rowIndex, 29, data.IncomeUnitPrice50.ToString)

                            Dim amount = data.IncomeUnitPrice50 * out50
                            .WriteToCell(rowIndex, 30, amount.ToString)
                            incomingAmount50 += amount
                        End If

                        If detail.Out20 <> 0 Then
                            .WriteToCell(rowIndex, 31, data.IncomeUnitPrice20.ToString)

                            Dim amount = data.IncomeUnitPrice20 * out20
                            .WriteToCell(rowIndex, 32, amount.ToString)
                            incomingAmount20 += amount
                        End If

                        If detail.Out16 <> 0 Then
                            .WriteToCell(rowIndex, 33, data.IncomeUnitPrice16.ToString)

                            Dim amount = data.IncomeUnitPrice16 * out16
                            .WriteToCell(rowIndex, 34, amount.ToString)
                            incomingAmount16 += amount
                        End If

                        If detail.Out10 <> 0 Then
                            .WriteToCell(rowIndex, 35, data.IncomeUnitPrice10.ToString)

                            Dim amount = data.IncomeUnitPrice10 * out10
                            .WriteToCell(rowIndex, 36, amount.ToString)
                            incomingAmount10 += amount
                        End If

                        If detail.Out4 <> 0 Then
                            .WriteToCell(rowIndex, 37, data.IncomeUnitPrice4.ToString)

                            Dim amount = data.IncomeUnitPrice4 * out4
                            .WriteToCell(rowIndex, 38, amount.ToString)
                            incomingAmount4 += amount
                        End If

                        .InsertRow(rowIndex)
                        rowIndex += 1
                    Next

                    '總計
                    .WriteToCell(rowIndex, 1, "總計")
                    .WriteToCell(rowIndex, 2, data.List.Sum(Function(x) x.In50).ToString)
                    .WriteToCell(rowIndex, 3, data.List.Sum(Function(x) x.In20).ToString)
                    .WriteToCell(rowIndex, 4, data.List.Sum(Function(x) x.In16).ToString)
                    .WriteToCell(rowIndex, 5, data.List.Sum(Function(x) x.In10).ToString)
                    .WriteToCell(rowIndex, 6, data.List.Sum(Function(x) x.In4).ToString)
                    .WriteToCell(rowIndex, 7, data.List.Sum(Function(x) x.Out50).ToString)
                    .WriteToCell(rowIndex, 8, data.List.Sum(Function(x) x.Out20).ToString)
                    .WriteToCell(rowIndex, 9, data.List.Sum(Function(x) x.Out16).ToString)
                    .WriteToCell(rowIndex, 10, data.List.Sum(Function(x) x.Out10).ToString)
                    .WriteToCell(rowIndex, 11, data.List.Sum(Function(x) x.Out4).ToString)

                    .WriteToCell(rowIndex, 12, total50.ToString)
                    .WriteToCell(rowIndex, 13, total20.ToString)
                    .WriteToCell(rowIndex, 14, total16.ToString)
                    .WriteToCell(rowIndex, 15, total10.ToString)
                    .WriteToCell(rowIndex, 16, total4.ToString)

                    .WriteToCell(rowIndex, 20, payAmount50.ToString)
                    .WriteToCell(rowIndex, 22, payAmount20.ToString)
                    .WriteToCell(rowIndex, 24, payAmount16.ToString)
                    .WriteToCell(rowIndex, 26, payAmount10.ToString)
                    .WriteToCell(rowIndex, 28, payAmount4.ToString)

                    .WriteToCell(rowIndex, 30, incomingAmount50.ToString)
                    .WriteToCell(rowIndex, 32, incomingAmount20.ToString)
                    .WriteToCell(rowIndex, 34, incomingAmount16.ToString)
                    .WriteToCell(rowIndex, 36, incomingAmount10.ToString)
                    .WriteToCell(rowIndex, 38, incomingAmount4.ToString)

                    '存檔
                    .SaveExcel($"新桶明細_{month:yyyyMM}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生發票
    ''' </summary>
    ''' <param name="month"></param>
    Public Sub GenerateInvoice(month As Date)
        Try
            '取得資料
            Dim data = _rep.GetInvoice(month)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "客戶發票明細範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim rowIndex = 2
                    Dim totalNotInoice As Integer

                    For Each item In data

                        .WriteToCell(rowIndex, 1, item.CusCode)
                        .WriteToCell(rowIndex, 2, item.TaxId)
                        .WriteToCell(rowIndex, 3, item.Amount.ToString)
                        .WriteToCell(rowIndex, 4, item.IsInvoice.ToString)
                        Dim notInvoice = item.Amount - item.IsInvoice
                        .WriteToCell(rowIndex, 5, (item.Amount - item.IsInvoice).ToString)
                        .WriteToCell(rowIndex, 6, item.CusName)

                        totalNotInoice += notInvoice
                        .InsertRow(rowIndex)
                        rowIndex += 1
                    Next

                    .WriteToCell(rowIndex, 2, "總計")
                    .WriteToCell(rowIndex, 3, data.Sum(Function(x) x.Amount).ToString)
                    .WriteToCell(rowIndex, 4, data.Sum(Function(x) x.IsInvoice).ToString)
                    .WriteToCell(rowIndex, 5, totalNotInoice.ToString)

                    '存檔
                    .SaveExcel($"客戶發票明細_{month:yyyyMM}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生月應收帳明細
    ''' </summary>
    ''' <param name="month"></param>
    Public Sub GenerateMonthlyAccountsReceivable(month As Date)
        Try
            '取得資料
            Dim data = _rep.GetMonthlyAccountsReceivable(month)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "月應收帳明細範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    .WriteToCell(1, 1, data.Month)

                    Dim rowIndex = 3
                    Dim totalOver As Single
                    Dim totalNotCollect As Single

                    For Each item In data.List

                        .WriteToCell(rowIndex, 1, item.CusCode)
                        .WriteToCell(rowIndex, 2, item.AccountsReceivable.ToString)
                        .WriteToCell(rowIndex, 3, item.AccountsReceived.ToString)

                        If item.AccountsReceivable < item.AccountsReceived Then
                            Dim over = item.AccountsReceived - item.AccountsReceivable
                            totalOver += over
                            .WriteToCell(rowIndex, 4, over.ToString)
                            .WriteToCell(rowIndex, 5, "0")
                        Else
                            Dim notCollect = item.AccountsReceivable - item.AccountsReceived
                            totalNotCollect += notCollect
                            .WriteToCell(rowIndex, 4, "0")
                            .WriteToCell(rowIndex, 5, notCollect.ToString)
                        End If

                        .WriteToCell(rowIndex, 6, item.Discount.ToString)

                        .InsertRow(rowIndex)
                        rowIndex += 1
                    Next

                    .WriteToCell(rowIndex, 1, "合計")
                    .WriteToCell(rowIndex, 2, data.List.Sum(Function(x) x.AccountsReceivable).ToString)
                    .WriteToCell(rowIndex, 3, data.List.Sum(Function(x) x.AccountsReceived).ToString)
                    .WriteToCell(rowIndex, 4, totalOver.ToString)
                    .WriteToCell(rowIndex, 5, totalNotCollect.ToString)
                    .WriteToCell(rowIndex, 6, data.List.Sum(Function(x) x.Discount).ToString)

                    '存檔
                    .SaveExcel($"月應收帳明細_{month:yyyyMM}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生進銷存明細
    ''' </summary>
    ''' <param name="year"></param>
    ''' <param name="compId"></param>
    ''' <param name="empId"></param>
    Public Sub GenerateInventoryTransactionDetail(year As Date, compId As Integer, empId As Integer)
        Try
            '取得資料
            Dim data = _rep.GetInventoryTransactionDetail(year, compId, empId)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "進銷存明細表範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    .WriteToCell(2, 1, "公司名稱:" + data.Company)
                    .WriteToCell(2, 3, "填表人:" + data.OperatorName)
                    .WriteToCell(2, 12, "連絡電話:" + data.Phone)
                    .WriteToCell(3, 1, data.Year)

                    '取得所有廠商
                    Dim allVendors = data.List.SelectMany(Function(x) x.PurchasesByVendor.Keys).
                                               Distinct.
                                               OrderBy(Function(x) x).
                                               ToList

                    '填寫廠商名稱
                    For i As Integer = 0 To allVendors.Count - 1
                        .WriteToCell(6 + i, 2, allVendors(i))
                    Next

                    '填寫每月數據
                    For Each monthData In data.List
                        Dim colIndex = monthData.Month + 2

                        '期初存量
                        .WriteToCell(5, colIndex, monthData.OpeningBalance.ToString)

                        '各廠商的進貨量
                        For i As Integer = 0 To allVendors.Count - 1
                            Dim vendor = allVendors(i)

                            If monthData.PurchasesByVendor.ContainsKey(vendor) Then .WriteToCell(6 + i, colIndex, monthData.PurchasesByVendor(vendor).ToString)
                        Next

                        '進貨總數
                        Dim totalMonthlyPurchase = monthData.PurchasesByVendor.Sum(Function(x) x.Value)
                        .WriteToCell(19, colIndex, totalMonthlyPurchase.ToString)

                        '銷售總數
                        .WriteToCell(20, colIndex, monthData.Sale.ToString)

                        '期末存量
                        .WriteToCell(21, colIndex, monthData.CloseingBalance.ToString)

                        '差異
                        .WriteToCell(22, colIndex, (monthData.OpeningBalance + totalMonthlyPurchase - monthData.Sale - monthData.CloseingBalance).ToString)
                    Next

                    '存檔
                    .SaveExcel($"進銷存明細表_{data.Company}_{year:yyyy}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生財稅
    ''' </summary>
    ''' <param name="month"></param>
    Public Sub GenerateTax(month As Date)
        Try
            '取得資料
            Dim data As Tax = _rep.GetTax(month)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "發票明細範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    .WriteToCell(1, 1, data.Month.ToString("yyyy年MM月") + " 發票明細")

                    Dim rowIndex = 3

                    For Each item In data.List
                        .WriteToCell(rowIndex, 1, item.Day.ToString("MM/dd"))
                        .WriteToCell(rowIndex, 2, item.InvoiceNum)
                        .WriteToCell(rowIndex, 3, item.TaxId)
                        .WriteToCell(rowIndex, 4, item.UnitPrice.ToString)
                        .WriteToCell(rowIndex, 5, item.Quantity.ToString)
                        .WriteToCell(rowIndex, 6, item.Tax.ToString)
                        .WriteToCell(rowIndex, 7, item.Amount.ToString)
                        .WriteToCell(rowIndex, 8, item.Memo)

                        rowIndex += 1
                    Next

                    .SetCustomBorders(rowIndex - 1, 1, rowIndex - 1, 8, XLBorderStyleValues.Thin)
                    .WriteToCell(rowIndex, 4, "合計")
                    .WriteToCell(rowIndex, 5, data.List.Sum(Function(x) x.Quantity).ToString)
                    .WriteToCell(rowIndex, 6, data.List.Sum(Function(x) x.Tax).ToString)
                    .WriteToCell(rowIndex, 7, data.List.Sum(Function(x) x.Amount).ToString)

                    '存檔
                    .SaveExcel($"發票明細_{month:yyyyMM}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生能源局
    ''' </summary>
    ''' <param name="month"></param>
    Public Sub GenerateEnergyBureau(month As Date)
        Try
            '取得資料
            Dim data As List(Of EnergyBureau) = _rep.GetBureau(month)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "能源局範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim leftColStart = 1
                    Dim rightColStart = 5
                    Dim currentRow = 1
                    Dim rowsPerPage = 57
                    Dim dataRowsPerPage = 55
                    Dim currentPage = 1
                    Dim dataIndex = 0

                    While dataIndex < data.Count
                        Dim leftSideTotal As Integer = 0
                        Dim rightSideTotal As Integer = 0
                        Dim leftSideRowCount = 0

                        '添加標題行
                        AddHeaderRow(xml, currentRow, leftColStart)
                        AddHeaderRow(xml, currentRow, rightColStart)
                        currentRow += 1

                        '填充左側數據
                        For i = 1 To dataRowsPerPage
                            If dataIndex >= data.Count Then Exit For

                            WriteItemToSheet(xml, currentRow, leftColStart, data(dataIndex))
                            leftSideTotal += data(dataIndex).Quantity
                            dataIndex += 1
                            currentRow += 1
                            leftSideRowCount += 1
                        Next

                        '重置行數以填充右側數據
                        currentRow -= leftSideRowCount

                        '填充右側數據
                        For i = 1 To dataRowsPerPage
                            If dataIndex >= data.Count Then Exit For
                            WriteItemToSheet(xml, currentRow, rightColStart, data(dataIndex))
                            rightSideTotal += data(dataIndex).Quantity
                            dataIndex += 1
                            currentRow += 1
                        Next

                        '確保currentRow到頁面底部
                        currentRow = currentPage * rowsPerPage

                        '添加總計行
                        AddTotalRow(xml, currentRow, leftColStart, currentPage, leftSideTotal)
                        AddTotalRow(xml, currentRow, rightColStart, currentPage, rightSideTotal)

                        currentRow += 1
                        currentPage += 1
                    End While

                    '存檔
                    .SaveExcel($"能源局_{month:yyyyMM}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            tempDate = String.Empty
        End Try
    End Sub

    Private Sub AddHeaderRow(xml As CloseXML_Excel, rowIndex As Integer, colStart As Integer)
        Dim headers() As String = {"日期", "發票號碼", "統一編號", "KG"}
        For i As Integer = 0 To headers.Length - 1
            xml.WriteToCell(rowIndex, colStart + i, headers(i))
        Next

        xml.SetCustomBorders(rowIndex, 1, rowIndex, 8, bottomStyle:=XLBorderStyleValues.Thin)
    End Sub

    Private Sub WriteItemToSheet(xml As CloseXML_Excel, rowIndex As Integer, startCol As Integer, item As EnergyBureau)
        With xml
            If item.Day.ToString("MM/dd") <> tempDate Then
                .WriteToCell(rowIndex, startCol, item.Day.ToString("MM/dd"))
                tempDate = item.Day.ToString("MM/dd")
            End If

            .WriteToCell(rowIndex, startCol + 1, item.InvoiceNum)
            .WriteToCell(rowIndex, startCol + 2, item.TaxId)
            .WriteToCell(rowIndex, startCol + 3, item.Quantity.ToString)
        End With
    End Sub

    Private Sub AddTotalRow(xml As CloseXML_Excel, rowIndex As Integer, colStart As Integer, page As Integer, total As Integer)
        With xml
            .WriteToCell(rowIndex, colStart, "總計")
            .WriteToCell(rowIndex, colStart + 3, total.ToString)
            .SetCustomBorders(rowIndex, 1, rowIndex, 8, topStyle:=XLBorderStyleValues.Thin)
        End With
    End Sub

    ''' <summary>
    ''' 產生月結帳單
    ''' </summary>
    ''' <param name="cusCode"></param>
    ''' <param name="month"></param>
    Public Sub GenerateMonthlyStatement(cusCode As String, month As Date)
        Try
            '取得資料
            Dim data As MonthlyStatement = _rep.GetMonthlyStatement(cusCode, month)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "月對帳單範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    '標題
                    .WriteToCell("A1", $" {month.Year}年{month.Month}月對帳單:")
                    .WriteToCell("B1", data.CusName)
                    .WriteToCell("D1", data.CusCode)

                    '家用瓦斯(變動前)
                    .WriteToCell("B3", data.GasNormalQuantity_First.ToString)
                    .WriteToCell("C3", data.GasNormalUnitPrice_First.ToString)

                    Dim gasAmount_1 = data.GasNormalQuantity_First * data.GasNormalUnitPrice_First
                    .WriteToCell("D3", gasAmount_1.ToString)

                    '工業氣(變動前)
                    .WriteToCell("B4", data.GasCQuantity_First.ToString)
                    .WriteToCell("C4", data.GasCUnitPrice_First.ToString)

                    Dim gasCAmount_1 = data.GasCQuantity_First * data.GasCUnitPrice_First
                    .WriteToCell("D4", gasCAmount_1.ToString)

                    '家用瓦斯(變動後)
                    .WriteToCell("B5", data.GasNormalQuantity.ToString)
                    .WriteToCell("C5", data.GasNormalUnitPrice.ToString)

                    Dim gasAmount = data.GasNormalQuantity * data.GasNormalUnitPrice
                    .WriteToCell("D5", gasAmount.ToString)

                    '工業氣(變動後)
                    .WriteToCell("B6", data.GasCQuantity.ToString)
                    .WriteToCell("C6", data.GasCUnitPrice.ToString)

                    Dim gasCAmount = data.GasCQuantity * data.GasCUnitPrice
                    .WriteToCell("D6", gasCAmount.ToString)

                    '保險
                    Dim totalQty = data.GasNormalQuantity_First + data.GasCQuantity_First + data.GasNormalQuantity + data.GasCQuantity
                    .WriteToCell("B7", totalQty.ToString)
                    .WriteToCell("C7", data.InsuranceUnitPrice.ToString)

                    Dim insuranceAmount = totalQty * data.InsuranceUnitPrice
                    .WriteToCell("D7", insuranceAmount.ToString)

                    ' 本月應收氣款
                    Dim gasAccountsRecievable = gasAmount_1 + gasCAmount_1 + gasAmount + gasCAmount
                    If Not data.IsInsurance Then gasAccountsRecievable += insuranceAmount
                    .WriteToCell("B8", gasAccountsRecievable.ToString)

                    ' 本月已收氣款
                    .WriteToCell("B9", data.GasAccountsReceived.ToString)
                    ' 新桶
                    .WriteToCell("B10", data.NewBerralAccountsReceivable.ToString)
                    ' 報廢桶
                    .WriteToCell("B11", data.ScrapBarrel.ToString)
                    ' 本月欠款
                    .WriteToCell("B12", (gasAccountsRecievable + data.NewBerralAccountsReceivable - data.GasAccountsReceived).ToString)

                    '存檔
                    .SaveExcel($"月對帳單_{data.CusCode}_{month:yyyyMM}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生保險
    ''' </summary>
    ''' <param name="compId"></param>
    ''' <param name="month"></param>
    Public Sub GenerateInsurance(compId As Integer, month As Date)
        Try
            '取得資料
            Dim data As Insurance = _rep.GetInsurance(compId, month)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "保險範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    '標題
                    .WriteToCell(1, 2, data.CompanyName)
                    .WriteToCell(1, 4, data.Month.ToString("yyyy年MM月"))

                    Dim rowIndex = 3

                    For Each item In data.List
                        .WriteToCell(rowIndex, 1, item.CusCode)
                        .WriteToCell(rowIndex, 2, item.CusName)
                        .WriteToCell(rowIndex, 3, item.TaxId)
                        .WriteToCell(rowIndex, 4, item.Amount.ToString)

                        rowIndex += 1
                    Next

                    .WriteToCell(rowIndex, 3, "合計")
                    .WriteToCell(rowIndex, 4, data.List.Sum(Function(x) x.Amount).ToString)
                    .SetCustomBorders(rowIndex, 1, rowIndex, 4, topStyle:=XLBorderStyleValues.Thin)

                    '存檔
                    .SaveExcel($"保險_{data.CompanyName}_{month:yyyyMM}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生損益表
    ''' </summary>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    ''' <param name="compId"></param>
    Public Sub GenerateIncomeStatement(startDate As Date, endDate As Date, compId As Integer)
        Try
            '取得資料
            Dim data As IncomeStatementModel = _rep.GetIncomeStatement(startDate, endDate, compId)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "損益表範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    '標題
                    .WriteToCell(1, 1, data.CompanyName)
                    .WriteToCell(3, 1, $"起訖年月日 {data.DateRange}")

                    Dim totalRevenue As Single = 0
                    Dim totalCostOfSales As Single = 0
                    Dim totalOperatingExpenses As Single = 0
                    Dim totalNonOperatingIncome As Single = 0

                    ' 營業收入
                    .WriteToCell(6, 3, data.GasIncome.ToString)
                    .WriteToCell(7, 3, data.SalesDiscount.ToString)

                    ' 營業收入
                    .WriteToCell(5, 4, data.OperatingIncome.ToString)

                    ' 營業費用(上)
                    .WriteToCell(9, 3, data.Income.ToString)
                    .WriteToCell(8, 4, data.Income.ToString)

                    ' 銷貨毛利
                    Dim grossProfit = data.OperatingIncome - data.Income
                    .WriteToCell(10, 4, grossProfit.ToString)

                    ' 營業費用(下)
                    totalOperatingExpenses = data.PaymentList.Sum(Function(x) x.Amount)
                    .WriteToCell(11, 4, totalOperatingExpenses.ToString)

                    Dim rowIndex = 12

                    For Each item In data.PaymentList
                        .WriteToCell(rowIndex, 2, item.Subject)
                        .WriteToCell(rowIndex, 3, item.Amount.ToString)
                        rowIndex += 1
                    Next

                    ' 營業外收益
                    totalNonOperatingIncome = data.CollectionsList.Sum(Function(x) x.Amount)
                    .WriteToCell(rowIndex, 1, "營業外收益", New CloseXML_Excel.CellFormatOptions With {.IsBold = True, .Horizontal = XLAlignmentHorizontalValues.Left})


                    .WriteToCell(rowIndex, 4, totalNonOperatingIncome.ToString)
                    rowIndex += 1

                    For Each item In data.CollectionsList
                        .WriteToCell(rowIndex, 2, item.Subject)
                        .WriteToCell(rowIndex, 3, item.Amount.ToString)
                        rowIndex += 1
                    Next

                    .SetCustomBorders(rowIndex, 1, rowIndex, 4, topStyle:=XLBorderStyleValues.Thin)
                    .WriteToCell(rowIndex, 1, "存貨")
                    rowIndex += 1

                    Dim netIncome As Single = grossProfit - totalOperatingExpenses + totalNonOperatingIncome
                    .WriteToCell(rowIndex, 1, "本期損益", New CloseXML_Excel.CellFormatOptions With {.IsBold = True, .Horizontal = XLAlignmentHorizontalValues.Left})
                    .WriteToCell(rowIndex, 4, netIncome.ToString)

                    '存檔
                    .SaveExcel($"損益表_{data.CompanyName}_{startDate:yyyyMMdd}-{endDate:yyyyMMdd}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生進項銷項
    ''' </summary>
    ''' <param name="year"></param>
    ''' <param name="month"></param>
    Public Sub GenerateInOut(year As Date, month As String)
        Try
            '取得資料
            Dim outData As OutInvoice = _rep.GetOutInvoice(year.Year, month)
            Dim splitCompanyData As SplitCompanyInvoice = _rep.GetSplitCompany(year.Year, month)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "進項銷項範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    .WriteToCell(1, 1, $"{year.Year}年{month}月")

                    '處理兩家公司的數據
                    For i As Integer = 0 To Math.Min(1, outData.Companies.Count - 1)
                        Dim company = outData.Companies(i)
                        Dim startColumn = If(i = 0, 1, 6) ' 第一家公司從第1列開始，第二家從第6列開始
                        Dim rowIndex As Integer = 4
                        Dim kgTotal As Integer = 0
                        Dim taxAmountTotal As Single = 0
                        Dim amountTotal As Single = 0

                        For Each monthData In company.MonthlyData
                            For Each group In monthData.RegularInvoices
                                .WriteToCell(rowIndex, startColumn, $"{monthData.Month}/{group.GroupNumber}")
                                .WriteToCell(rowIndex, startColumn + 1, group.Qty.ToString)
                                .WriteToCell(rowIndex, startColumn + 2, group.TaxAmount.ToString)
                                .WriteToCell(rowIndex, startColumn + 3, group.Amount.ToString)
                                rowIndex += 1
                            Next

                            rowIndex += 5 - monthData.RegularInvoices.Count

                            Dim kgSubTotal = monthData.RegularInvoices.Sum(Function(x) x.Qty)
                            Dim taxAmountSubTotal = monthData.RegularInvoices.Sum(Function(x) x.TaxAmount)
                            Dim amountSubTotal = monthData.RegularInvoices.Sum(Function(x) x.Amount)

                            Dim twoPartMachine = monthData.SpecialInvoices.TwoPartMachine
                            .WriteToCell(rowIndex, startColumn + 1, twoPartMachine.Qty.ToString)
                            kgSubTotal += twoPartMachine.Qty
                            .WriteToCell(rowIndex, startColumn + 2, twoPartMachine.TaxAmount.ToString)
                            taxAmountSubTotal += twoPartMachine.TaxAmount
                            .WriteToCell(rowIndex, startColumn + 3, twoPartMachine.Amount.ToString)
                            amountSubTotal += twoPartMachine.Amount
                            rowIndex += 1

                            Dim threePartHandwritten = monthData.SpecialInvoices.ThreePartHandwritten
                            .WriteToCell(rowIndex, startColumn + 1, threePartHandwritten.Qty.ToString)
                            kgSubTotal += threePartHandwritten.Qty
                            .WriteToCell(rowIndex, startColumn + 2, threePartHandwritten.TaxAmount.ToString)
                            taxAmountSubTotal += threePartHandwritten.TaxAmount
                            .WriteToCell(rowIndex, startColumn + 3, threePartHandwritten.Amount.ToString)
                            amountSubTotal += threePartHandwritten.Amount
                            rowIndex += 1

                            Dim twoPartHandwritten = monthData.SpecialInvoices.TwoPartHandwritten
                            .WriteToCell(rowIndex, startColumn + 1, twoPartHandwritten.Qty.ToString)
                            kgSubTotal += twoPartHandwritten.Qty
                            .WriteToCell(rowIndex, startColumn + 2, twoPartHandwritten.TaxAmount.ToString)
                            taxAmountSubTotal += twoPartHandwritten.TaxAmount
                            .WriteToCell(rowIndex, startColumn + 3, twoPartHandwritten.Amount.ToString)
                            amountSubTotal += twoPartHandwritten.Amount
                            rowIndex += 1

                            '小計
                            .WriteToCell(rowIndex, startColumn + 1, kgSubTotal.ToString)
                            kgTotal += kgSubTotal
                            .WriteToCell(rowIndex, startColumn + 2, taxAmountSubTotal.ToString)
                            taxAmountTotal += taxAmountSubTotal
                            .WriteToCell(rowIndex, startColumn + 3, amountSubTotal.ToString)
                            amountTotal += amountSubTotal

                            rowIndex += 2
                        Next

                        '合計
                        .WriteToCell(rowIndex - 1, startColumn + 1, kgTotal.ToString)
                        .WriteToCell(rowIndex - 1, startColumn + 2, taxAmountTotal.ToString)
                        .WriteToCell(rowIndex - 1, startColumn + 3, amountTotal.ToString)
                    Next

                    '分裝場
                    Dim taxSubTotal As Single = 0
                    Dim amountSubTotal_split As Single = 0

                    .WriteToCell(4, 11, splitCompanyData.FrontDate1)
                    .WriteToCell(4, 14, splitCompanyData.FrontTax1.ToString)
                    taxSubTotal += splitCompanyData.FrontTax1
                    .WriteToCell(4, 15, splitCompanyData.FrontAmount1.ToString)
                    amountSubTotal_split += splitCompanyData.FrontAmount1
                    .WriteToCell(4, 16, splitCompanyData.FrontVendorTaxId1)

                    .WriteToCell(5, 11, splitCompanyData.FrontDate2)
                    .WriteToCell(5, 14, splitCompanyData.FrontTax2.ToString)
                    taxSubTotal += splitCompanyData.FrontTax2
                    .WriteToCell(5, 15, splitCompanyData.FrontAmount2.ToString)
                    amountSubTotal_split += splitCompanyData.FrontAmount2
                    .WriteToCell(5, 16, splitCompanyData.FrontVendorTaxId2)

                    .WriteToCell(6, 11, splitCompanyData.EndDate1)
                    .WriteToCell(6, 14, splitCompanyData.EndTax1.ToString)
                    taxSubTotal += splitCompanyData.EndTax1
                    .WriteToCell(6, 15, splitCompanyData.EndAmount1.ToString)
                    amountSubTotal_split += splitCompanyData.EndAmount1
                    .WriteToCell(6, 16, splitCompanyData.EndVendorTaxId1)

                    .WriteToCell(7, 11, splitCompanyData.EndDate2)
                    .WriteToCell(7, 14, splitCompanyData.EndTax2.ToString)
                    taxSubTotal += splitCompanyData.EndTax2
                    .WriteToCell(7, 15, splitCompanyData.EndAmount2.ToString)
                    amountSubTotal_split += splitCompanyData.EndAmount2
                    .WriteToCell(7, 16, splitCompanyData.EndVendorTaxId2)

                    '小計
                    .WriteToCell(8, 14, taxSubTotal.ToString)
                    .WriteToCell(8, 15, amountSubTotal_split.ToString)

                    Dim rowIndex_split As Integer = 10

                    For Each item In splitCompanyData.InList
                        .WriteToCell(rowIndex_split, 11, item.Day)
                        .WriteToCell(rowIndex_split, 12, item.InvoiceNum)
                        .WriteToCell(rowIndex_split, 13, item.Name)
                        .WriteToCell(rowIndex_split, 14, item.Tax.ToString)
                        .WriteToCell(rowIndex_split, 15, item.Amount.ToString)
                        .WriteToCell(rowIndex_split, 16, item.VendorTaxId)
                        .SetCustomBorders(rowIndex_split, 11, rowIndex_split, 16, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin,
                                          XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)
                        rowIndex_split += 1
                    Next

                    '存檔
                    .SaveExcel($"進項銷項_{year:yyyy}_{month}")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生每日科目匯總表
    ''' </summary>
    Public Sub GenerateDailySubjectSummary(day As Date)
        Try
            '取得資料
            Dim data As List(Of DailySubjectSummary) = _rep.GetDailySubjectSummary(day)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "每日科目匯總表範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    Dim title = day.ToString("yyyy年MM月dd日") + " 科目匯總表"

                    .WriteToCell("A1", title)

                    Dim rowIndex As Integer = 3
                    Dim totalBalance As Double = 0

                    For i As Integer = 0 To data.Count - 1
                        Dim item = data(i)
                        .WriteToCell("A", rowIndex, item.Subject)
                        .WriteToCell("C", rowIndex, item.Debit.ToString)
                        .WriteToCell("D", rowIndex, item.Credit.ToString)
                        Dim balance = item.Debit - item.Credit
                        .WriteToCell("E", rowIndex, (balance).ToString)
                        totalBalance += balance
                        rowIndex += 1
                    Next

                    .SetCustomBorders(rowIndex, 1, rowIndex, 5, XLBorderStyleValues.Thin)
                    .WriteToCell("A", rowIndex, "合計")
                    .WriteToCell("C", rowIndex, data.Sum(Function(x) x.Debit).ToString)
                    .WriteToCell("D", rowIndex, data.Sum(Function(x) x.Credit).ToString)
                    .WriteToCell("E", rowIndex, totalBalance.ToString)

                    '存檔
                    .SaveExcel(title)
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生科目平衡表 - 橫向多科目並列格式
    ''' </summary>
    ''' <param name="month">查詢月份</param>
    Public Sub GenerateAccountBalance(month As Date)
        Try
            '取得資料
            Dim data = _rep.GetAccountBalance(month)

            '套版
            Using xml As New CloseXML_Excel()
                With xml
                    .SelectWorksheet("Sheet1")

                    '設定標題
                    Dim titleStyle = New CloseXML_Excel.CellFormatOptions With {
                        .FontSize = 14,
                        .IsBold = True,
                        .Horizontal = XLAlignmentHorizontalValues.Left
                    }

                    .WriteToCell(1, 1, $"{month:yyyy年MM月} 科目平衡表", titleStyle)

                    '設定表頭樣式
                    Dim headerStyle = New CloseXML_Excel.CellFormatOptions With {
                        .IsBold = True,
                        .Horizontal = XLAlignmentHorizontalValues.Center
                    }

                    '設定金額樣式
                    Dim amountStyle = New CloseXML_Excel.CellFormatOptions With {
                        .Horizontal = XLAlignmentHorizontalValues.Right
                    }

                    '設定總計樣式
                    Dim totalStyle = New CloseXML_Excel.CellFormatOptions With {
                        .IsBold = True
                    }

                    ' 取得最高列
                    Dim maxRow = data.Max(Function(x) Math.Max(x.DebitList.Count, x.CreditList.Count))

                    ' 寫入資料
                    Dim colIndex As Integer = 1
                    For Each subject In data

                        ' 畫格子
                        .SetCustomBorders(2, colIndex, maxRow + 3, colIndex + 3,
                                          XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin, XLBorderStyleValues.Thin)

                        ' 寫入表頭
                        .WriteToCell(2, colIndex, subject.SubjectName, headerStyle)

                        '合併科目名稱欄位
                        .MergeCells(2, colIndex, 2, colIndex + 3)

                        ' 寫入明細
                        ' 借方明細
                        Dim debitRowIndex As Integer = 3
                        For Each item In subject.DebitList
                            .WriteToCell(debitRowIndex, colIndex, item.Day.ToString("MM月dd日"))
                            .WriteToCell(debitRowIndex, colIndex + 1, item.Amount.ToString("#,##0"), amountStyle)
                            debitRowIndex += 1
                        Next

                        ' 貸方明細
                        Dim creditRowIndex As Integer = 3
                        For Each item In subject.CreditList
                            .WriteToCell(creditRowIndex, colIndex + 2, item.Amount.ToString("#,##0"), amountStyle)
                            .WriteToCell(creditRowIndex, colIndex + 3, item.Day.ToString("MM月dd日"))

                            creditRowIndex += 1
                        Next

                        ' 劃科目隔線
                        .SetCustomBorders(2, colIndex + 3, maxRow + 3, colIndex + 3, rightStyle:=XLBorderStyleValues.Medium)

                        ' 總計
                        .WriteToCell(maxRow + 3, colIndex, "總計", totalStyle)
                        .WriteToCell(maxRow + 3, colIndex + 1, subject.DebitList.Sum(Function(x) x.Amount).ToString("#,##0"), amountStyle)
                        .WriteToCell(maxRow + 3, colIndex + 2, subject.CreditList.Sum(Function(x) x.Amount).ToString("#,##0"), amountStyle)

                        colIndex += 4
                    Next

                    '存檔
                    .SaveExcel($"科目平衡表_{month:yyyyMM}")
                End With
            End Using
        Catch ex As Exception
            MsgBox("產生科目平衡表失敗:" + ex.Message)
        End Try
    End Sub

    Public Function GetCusInfo(cusCode As String) As customer
        Try
            Return _cusRep.GetByCusCode(cusCode)
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
End Class