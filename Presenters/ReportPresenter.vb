Imports System.IO
Imports ClosedXML.Excel

Public Class ReportPresenter
    Private ReadOnly _view As IReportView
    Private _rep As IReportRep
    Private _manuRep As IManufacturerService = New ManufacturerService
    Private ReadOnly _bankRep As IBankRep
    Private ReadOnly _compRep As ICompanyRep
    Private ReadOnly _printerSer As IPrinterService

    Public Sub New(view As IReportView, reportRep As IReportRep, bankRep As IBankRep, compRep As ICompanyRep, printerSer As IPrinterService)
        _view = view
        _rep = reportRep
        _bankRep = bankRep
        _compRep = compRep
        _printerSer = printerSer
    End Sub

    ''' <summary>
    ''' 產生氣量氣款收付明細表
    ''' </summary>
    ''' <param name="d"></param>
    Public Sub GenerateCustomersGasDetailByDay(d As Date)
        Try
            '蒐集資料
            Dim datas = _rep.CustomersGasDetailByDay(d)

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "氣量氣款收付明細表範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    .WriteToCell(2, 1, $"{d:yyyy年MM月dd日 氣量氣款收付明細表}")
                    .WriteToCell(3, 7, $"列印日期: {Now:yyyy/MM/dd}")

                    Dim rowIndex As Integer

                    Dim dataStyle = New CloseXML_Excel.CellFormatOptions With {
                        .Horizontal = XLAlignmentHorizontalValues.Center
                    }

                    For i As Integer = 0 To datas.Count - 1
                        rowIndex = 5 + i

                        .WriteToCell(rowIndex, 1, datas(i).客戶名稱)
                        .WriteToCell(rowIndex, 2, If(datas(i).存氣 <> Nothing, datas(i).存氣.ToString("#,##"), 0), dataStyle)
                        .WriteToCell(rowIndex, 3, If(datas(i).本日提量 <> Nothing, datas(i).本日提量.ToString("#,##"), "0"), dataStyle)
                        .WriteToCell(rowIndex, 4, If(datas(i).當月累計提量 <> Nothing, datas(i).當月累計提量.ToString("#,##"), "0"), dataStyle)
                        .WriteToCell(rowIndex, 5, If(datas(i).本日氣款 <> Nothing, datas(i).本日氣款.ToString("#,##"), "0"), dataStyle)
                        .WriteToCell(rowIndex, 6, If(datas(i).本日收款 <> Nothing, datas(i).本日收款.ToString("#,##"), "0"), dataStyle)
                        .WriteToCell(rowIndex, 7, If(datas(i).結欠 <> Nothing, datas(i).結欠.ToString("#,##"), "0"), dataStyle)

                    Next

                    .SetCustomBorders(rowIndex, 1, rowIndex, 7, bottomStyle:=XLBorderStyleValues.Thin)

                    rowIndex += 1

                    .WriteToCell(rowIndex, 1, "合計:", dataStyle)
                    .WriteToCell(rowIndex, 2, datas.Sum(Function(x) x.存氣).ToString("#,##"), dataStyle)
                    .WriteToCell(rowIndex, 3, datas.Sum(Function(x) x.本日提量).ToString("#,##"), dataStyle)
                    .WriteToCell(rowIndex, 4, datas.Sum(Function(x) x.當月累計提量).ToString("#,##"), dataStyle)
                    .WriteToCell(rowIndex, 5, datas.Sum(Function(x) x.本日氣款).ToString("#,##"), dataStyle)
                    .WriteToCell(rowIndex, 6, datas.Sum(Function(x) x.本日收款).ToString("#,##"), dataStyle)
                    .WriteToCell(rowIndex, 7, datas.Sum(Function(x) x.結欠).ToString("#,##"), dataStyle)

                    '存檔
                    SaveExcel($"日氣量氣款收付明細表_{d:yyyyMMdd}", xml)
                End With
            End Using
        Catch ex As Exception
            MsgBox("列印失敗:" + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生客戶提氣清冊
    ''' </summary>
    ''' <param name="d"></param>
    Public Sub GenerateCustomersGetGasList(d As Date)
        Try
            '蒐集資料
            Dim datas = _rep.CustomersGetGasList(d)

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "客戶提氣清冊範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim titleStyle = New CloseXML_Excel.CellFormatOptions With {
                        .FontSize = 14,
                        .IsBold = True,
                        .Horizontal = XLAlignmentHorizontalValues.Center
                    }
                    .MergeCells(1, 1, 1, 23)
                    .WriteToCell(1, 1, "豐原液化煤氣分裝場", titleStyle)

                    .MergeCells(2, 1, 2, 23)
                    .WriteToCell(2, 1, "客戶提氣量清單", titleStyle)

                    Dim dateStyle = New CloseXML_Excel.CellFormatOptions With {
                        .Horizontal = XLAlignmentHorizontalValues.Left
                    }
                    .WriteToCell(3, 1, $"提氣日期: {d:yyyy/MM/dd}", dateStyle)

                    Dim printDateStyle = New CloseXML_Excel.CellFormatOptions With {
                        .Horizontal = XLAlignmentHorizontalValues.Right
                    }
                    .WriteToCell(3, 23, $"列印日期: {Now:yyyy/MM/dd}", printDateStyle)

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
                    SaveExcel($"客戶提氣清冊_{d:yyyyMMdd}", xml)
                End With
            End Using
        Catch ex As Exception
            MsgBox("列印失敗:" + ex.Message)
        End Try
    End Sub

    Public Sub GetManuCmb()
        _view.SetGasVendorCmb(_manuRep.GetGasVendorCmbItems())
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
                    .WriteToCell(rowIndex, 3, datas.Sum(Function(x) x.普氣))
                    .WriteToCell(rowIndex, 4, datas.Average(Function(x) x.普氣單價.ToString("N2")))
                    .WriteToCell(rowIndex, 5, datas.Sum(Function(x) x.普氣金額))
                    .WriteToCell(rowIndex, 6, datas.Sum(Function(x) x.丙氣))
                    .WriteToCell(rowIndex, 7, datas.Average(Function(x) x.丙氣單價.ToString("N2")))
                    .WriteToCell(rowIndex, 8, datas.Sum(Function(x) x.丙氣金額))
                    .WriteToCell(rowIndex, 9, datas.Sum(Function(x) x.總計))
                    .WriteToCell(rowIndex, 10, datas.Sum(Function(x) x.餘額))
                    .WriteToCell(rowIndex, 11, datas.Last().累計)

                    '存檔
                    SaveExcel($"大氣進貨明細_{vendorName}_{d:yyyyMMdd}", xml)

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
    Public Sub GenerateDailyCustomerReceivable(d As Date, cusCode As String)
        Try
            '蒐集資料
            Dim datas = _rep.DailyCustomerReceivable(d, cusCode)

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "單一客戶每日的應收帳明細表範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim cusName = datas.FirstOrDefault().客戶名稱
                    .WriteToCell(1, 1, $"{d:yyyy}年豐原液化煤氣分裝場氣款應收帳")
                    .WriteToCell(4, 1, cusName)

                    Dim rowIndex As Integer

                    For i As Integer = 0 To datas.Count - 1
                        '資料開始列
                        rowIndex = 4 + i

                        .WriteToCell(rowIndex, 2, datas(i).日期)
                        .WriteToCell(rowIndex, 3, datas(i).總提氣)

                        .WriteToCell(rowIndex, 4, datas(i).廠運總提氣)
                        .WriteToCell(rowIndex, 5, datas(i).廠運普氣)
                        .WriteToCell(rowIndex, 6, datas(i).廠運普氣退氣)
                        .WriteToCell(rowIndex, 7, datas(i).廠運普氣單價)
                        .WriteToCell(rowIndex, 8, datas(i).廠運普氣金額)
                        .WriteToCell(rowIndex, 9, datas(i).廠運丙氣)
                        .WriteToCell(rowIndex, 10, datas(i).廠運丙氣退氣)
                        .WriteToCell(rowIndex, 11, datas(i).廠運丙氣單價)
                        .WriteToCell(rowIndex, 12, datas(i).廠運丙氣金額)

                        .WriteToCell(rowIndex, 13, datas(i).自運總提氣)
                        .WriteToCell(rowIndex, 14, datas(i).自運普氣)
                        .WriteToCell(rowIndex, 15, datas(i).自運普氣退氣)
                        .WriteToCell(rowIndex, 16, datas(i).自運普氣單價)
                        .WriteToCell(rowIndex, 17, datas(i).自運普氣金額)
                        .WriteToCell(rowIndex, 18, datas(i).自運丙氣)
                        .WriteToCell(rowIndex, 19, datas(i).自運丙氣退氣)
                        .WriteToCell(rowIndex, 20, datas(i).自運丙氣單價)
                        .WriteToCell(rowIndex, 21, datas(i).自運丙氣金額)

                        .WriteToCell(rowIndex, 22, datas(i).總額)
                        .WriteToCell(rowIndex, 23, datas(i).現金)
                        .WriteToCell(rowIndex, 24, datas(i).票據)
                        .WriteToCell(rowIndex, 25, datas(i).掛帳)
                        .WriteToCell(rowIndex, 26, datas(i).累計)
                    Next

                    '存檔
                    SaveExcel($"單一客戶每日的應收帳明細表_{cusName}_{d:yyyyMMdd}", xml)
                End With
            End Using
        Catch ex As Exception
            MsgBox("列印失敗:" + ex.Message)
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
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "提量支數統計範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    .WriteToCell(2, 1, $"{month:yyyy}年")

                    Dim rowIndex As Integer

                    For i As Integer = 0 To datas.Count - 1
                        '資料開始列
                        rowIndex = 3 + i

                        .WriteToCell(rowIndex, 1, datas(i).日期)
                        .WriteToCell(rowIndex, 2, datas(i).退氣)
                        .WriteToCell(rowIndex, 3, datas(i).退氣累計量)
                        .WriteToCell(rowIndex, 4, datas(i).提氣量)
                        .WriteToCell(rowIndex, 5, datas(i).提氣累計量)
                        .WriteToCell(rowIndex, 6, datas(i).總支數)
                        .WriteToCell(rowIndex, 7, datas(i).瓦斯瓶50Kg)
                        .WriteToCell(rowIndex, 8, datas(i).瓦斯瓶20Kg)
                        .WriteToCell(rowIndex, 9, datas(i).瓦斯瓶16Kg)
                        .WriteToCell(rowIndex, 10, datas(i).瓦斯瓶4Kg)
                        .WriteToCell(rowIndex, 11, datas(i).瓦斯瓶18Kg)
                        .WriteToCell(rowIndex, 12, datas(i).瓦斯瓶14Kg)
                        .WriteToCell(rowIndex, 13, datas(i).瓦斯瓶5Kg)
                        .WriteToCell(rowIndex, 14, datas(i).瓦斯瓶2Kg)
                    Next

                    '存檔
                    SaveExcel($"單一客戶每日的應收帳明細表_{month:yyyy}", xml)
                End With
            End Using
        Catch ex As Exception
            MsgBox("列印失敗:" + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生現金帳
    ''' </summary>
    ''' <param name="month"></param>
    Public Sub GenerateCashAccount(month As Date)
        Try
            '取得資料
            Dim data = _rep.GetCashAccount(month)

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "現金帳範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    .WriteToCell(1, 1, $"{month.Year}年{month.Month}月")

                    Dim rowIndex = 3
                    For i As Integer = 0 To data.Count - 1
                        .WriteToCell(rowIndex + i, 1, data(i).日)
                        .WriteToCell(rowIndex + i, 2, data(i).科目)
                        .WriteToCell(rowIndex + i, 3, data(i).摘要)
                        .WriteToCell(rowIndex + i, 4, data(i).收入金額)
                        .WriteToCell(rowIndex + i, 5, data(i).支出金額)
                        .WriteToCell(rowIndex + i, 6, data(i).餘額)
                    Next

                    Dim exportFilePath = Path.Combine(Application.StartupPath, "報表", "現金帳.xlsx")
                    .SaveAs(exportFilePath)

                    '列印
                    Dim printerName = _printerSer.GetOrSelectPrinter
                    .Print(exportFilePath, printerName)

                    '存檔
                    SaveExcel($"現金帳_{month:yyyyMM}", xml)
                End With
            End Using
        Catch ex As Exception
            MsgBox("列印失敗:" + ex.Message)
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
                    .WriteToCell(1, 1, $"{data.年月}")

                    Dim rowIndex = 3

                    For Each bankAccount In data.List
                        .WriteToCell(rowIndex, 1, bankAccount.日期)
                        .WriteToCell(rowIndex, 2, bankAccount.科目)
                        .WriteToCell(rowIndex, 3, bankAccount.摘要)
                        .WriteToCell(rowIndex, 4, bankAccount.借方)
                        .WriteToCell(rowIndex, 5, bankAccount.貸方)
                        .WriteToCell(rowIndex, 6, bankAccount.餘額)
                        .SetCustomBorders(rowIndex, 1, rowIndex, 6, bottomStyle:=XLBorderStyleValues.Thin)
                        rowIndex += 1
                    Next

                    Dim totalDebit = data.List.Sum(Function(x) x.借方)
                    Dim totalCredit = data.List.Sum(Function(x) x.貸方)

                    .WriteToCell(rowIndex, 3, "合計")
                    .WriteToCell(rowIndex, 4, totalDebit)
                    .WriteToCell(rowIndex, 5, totalCredit)

                    Dim exportFilePath = Path.Combine(Application.StartupPath, "報表", "銀行帳.xlsx")
                    .SaveAs(exportFilePath)

                    '列印
                    Dim printerName = _printerSer.GetOrSelectPrinter
                    .Print(exportFilePath, printerName)

                    '存檔
                    SaveExcel($"銀行帳_{data.年月}", xml)
                End With
            End Using
        Catch ex As Exception
            MsgBox("列印失敗:" + ex.Message)
            Console.WriteLine(ex.InnerException.Message)
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
                        .WriteToCell(rowIndex, 3, bankAccount.瓦斯瓶50Kg)
                        .WriteToCell(rowIndex, 4, bankAccount.瓦斯瓶20Kg)
                        .WriteToCell(rowIndex, 5, bankAccount.瓦斯瓶16Kg)
                        .WriteToCell(rowIndex, 6, bankAccount.瓦斯瓶10Kg)
                        .WriteToCell(rowIndex, 7, bankAccount.瓦斯瓶4Kg)
                        .WriteToCell(rowIndex, 8, bankAccount.瓦斯瓶18Kg)
                        .WriteToCell(rowIndex, 9, bankAccount.瓦斯瓶14Kg)
                        .WriteToCell(rowIndex, 10, bankAccount.瓦斯瓶5Kg)
                        .WriteToCell(rowIndex, 11, bankAccount.瓦斯瓶2Kg)
                        .SetCustomBorders(rowIndex, 1, rowIndex, 11, bottomStyle:=ClosedXML.Excel.XLBorderStyleValues.Thin)
                        rowIndex += 1
                    Next

                    '存檔
                    SaveExcel($"客戶寄桶結存瓶_{data.CustomerName}", xml)
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
                    .WriteToCell(3, 12, total50)
                    .WriteToCell(3, 13, total20)
                    .WriteToCell(3, 14, total16)
                    .WriteToCell(3, 15, total10)
                    .WriteToCell(3, 16, total4)
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
                        .WriteToCell(rowIndex, 2, in50)
                        .WriteToCell(rowIndex, 3, in20)
                        .WriteToCell(rowIndex, 4, in16)
                        .WriteToCell(rowIndex, 5, in10)
                        .WriteToCell(rowIndex, 6, in4)

                        '支出
                        .WriteToCell(rowIndex, 7, out50)
                        .WriteToCell(rowIndex, 8, out20)
                        .WriteToCell(rowIndex, 9, out16)
                        .WriteToCell(rowIndex, 10, out10)
                        .WriteToCell(rowIndex, 11, out4)

                        '結餘
                        total50 += in50 - out50
                        total20 += in20 - out20
                        total16 += in16 - out16
                        total10 += in10 - out10
                        total4 += in4 - out4

                        .WriteToCell(rowIndex, 12, in50 - out50)
                        .WriteToCell(rowIndex, 13, in20 - out20)
                        .WriteToCell(rowIndex, 14, in16 - out16)
                        .WriteToCell(rowIndex, 15, in10 - out10)
                        .WriteToCell(rowIndex, 16, in4 - out4)

                        .WriteToCell(rowIndex, 17, detail.Memo)
                        .WriteToCell(rowIndex, 18, detail.PayDate)

                        If detail.In50 <> 0 Then
                            .WriteToCell(rowIndex, 19, data.PayUnitPrice50)

                            Dim amount = data.PayUnitPrice50 * in50
                            .WriteToCell(rowIndex, 20, amount)
                            payAmount50 += amount
                        End If

                        If detail.In20 <> 0 Then
                            .WriteToCell(rowIndex, 21, data.PayUnitPrice20)

                            Dim amount = data.PayUnitPrice20 * in20
                            .WriteToCell(rowIndex, 22, amount)
                            payAmount20 += amount
                        End If

                        If detail.In16 <> 0 Then
                            .WriteToCell(rowIndex, 23, data.PayUnitPrice16)

                            Dim amount = data.PayUnitPrice16 * in16
                            .WriteToCell(rowIndex, 24, amount)
                            payAmount16 += amount
                        End If

                        If detail.In10 <> 0 Then
                            .WriteToCell(rowIndex, 25, data.PayUnitPrice10)

                            Dim amount = data.PayUnitPrice10 * in10
                            .WriteToCell(rowIndex, 26, amount)
                            payAmount10 += amount
                        End If

                        If detail.In4 <> 0 Then
                            .WriteToCell(rowIndex, 27, data.PayUnitPrice4)

                            Dim amount = data.PayUnitPrice4 * in4
                            .WriteToCell(rowIndex, 28, amount)
                            payAmount4 += amount
                        End If

                        If detail.Out50 <> 0 Then
                            .WriteToCell(rowIndex, 29, data.IncomeUnitPrice50)

                            Dim amount = data.IncomeUnitPrice50 * out50
                            .WriteToCell(rowIndex, 30, amount)
                            incomingAmount50 += amount
                        End If

                        If detail.Out20 <> 0 Then
                            .WriteToCell(rowIndex, 31, data.IncomeUnitPrice20)

                            Dim amount = data.IncomeUnitPrice20 * out20
                            .WriteToCell(rowIndex, 32, amount)
                            incomingAmount20 += amount
                        End If

                        If detail.Out16 <> 0 Then
                            .WriteToCell(rowIndex, 33, data.IncomeUnitPrice16)

                            Dim amount = data.IncomeUnitPrice16 * out16
                            .WriteToCell(rowIndex, 34, amount)
                            incomingAmount16 += amount
                        End If

                        If detail.Out10 <> 0 Then
                            .WriteToCell(rowIndex, 35, data.IncomeUnitPrice10)

                            Dim amount = data.IncomeUnitPrice10 * out10
                            .WriteToCell(rowIndex, 36, amount)
                            incomingAmount10 += amount
                        End If

                        If detail.Out4 <> 0 Then
                            .WriteToCell(rowIndex, 37, data.IncomeUnitPrice4)

                            Dim amount = data.IncomeUnitPrice4 * out4
                            .WriteToCell(rowIndex, 38, amount)
                            incomingAmount4 += amount
                        End If

                        .InsertRow(rowIndex)
                        rowIndex += 1
                    Next

                    '總計
                    .WriteToCell(rowIndex, 1, "總計")
                    .WriteToCell(rowIndex, 2, data.List.Sum(Function(x) x.In50))
                    .WriteToCell(rowIndex, 3, data.List.Sum(Function(x) x.In20))
                    .WriteToCell(rowIndex, 4, data.List.Sum(Function(x) x.In16))
                    .WriteToCell(rowIndex, 5, data.List.Sum(Function(x) x.In10))
                    .WriteToCell(rowIndex, 6, data.List.Sum(Function(x) x.In4))
                    .WriteToCell(rowIndex, 7, data.List.Sum(Function(x) x.Out50))
                    .WriteToCell(rowIndex, 8, data.List.Sum(Function(x) x.Out20))
                    .WriteToCell(rowIndex, 9, data.List.Sum(Function(x) x.Out16))
                    .WriteToCell(rowIndex, 10, data.List.Sum(Function(x) x.Out10))
                    .WriteToCell(rowIndex, 11, data.List.Sum(Function(x) x.Out4))

                    .WriteToCell(rowIndex, 12, total50)
                    .WriteToCell(rowIndex, 13, total20)
                    .WriteToCell(rowIndex, 14, total16)
                    .WriteToCell(rowIndex, 15, total10)
                    .WriteToCell(rowIndex, 16, total4)

                    .WriteToCell(rowIndex, 20, payAmount50)
                    .WriteToCell(rowIndex, 22, payAmount20)
                    .WriteToCell(rowIndex, 24, payAmount16)
                    .WriteToCell(rowIndex, 26, payAmount10)
                    .WriteToCell(rowIndex, 28, payAmount4)

                    .WriteToCell(rowIndex, 30, incomingAmount50)
                    .WriteToCell(rowIndex, 32, incomingAmount20)
                    .WriteToCell(rowIndex, 34, incomingAmount16)
                    .WriteToCell(rowIndex, 36, incomingAmount10)
                    .WriteToCell(rowIndex, 38, incomingAmount4)

                    '存檔
                    SaveExcel($"新桶明細_{month:yyyyMM}", xml)
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生應收票據
    ''' </summary>
    ''' <param name="companyId"></param>
    ''' <param name="month"></param>
    Public Sub GenerateBillsReceivable(companyId As Integer, bankId As Integer, month As Date)
        Try
            Dim formatDate = New Date(month.Year, month.Month, 1)

            '取得資料
            Dim data = _rep.GetBillsReceivable(companyId, bankId, formatDate)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "應收票據範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    .WriteToCell(1, 1, data.CompanyName)
                    .WriteToCell(2, 2, data.BankAccount)

                    Dim rowIndex = 4

                    For Each item In data.List

                        .WriteToCell(rowIndex, 1, rowIndex - 3)
                        .WriteToCell(rowIndex, 2, item.ReceiveDate.ToString("yyyy/MM/dd"))
                        .WriteToCell(rowIndex, 3, item.CusCode)
                        .WriteToCell(rowIndex, 4, item.ChequeNumber)
                        .WriteToCell(rowIndex, 5, item.IssuerName)
                        .WriteToCell(rowIndex, 6, item.PayBankName)
                        .WriteToCell(rowIndex, 7, item.AvailableDate.ToString("yyyy/MM/dd"))
                        .WriteToCell(rowIndex, 8, item.Amount)
                        .WriteToCell(rowIndex, 9, If(item.CollectDate.HasValue, item.CollectDate.ToString("yyyy/MM/dd"), ""))
                        .WriteToCell(rowIndex, 11, item.Memo)

                        .InsertRow(rowIndex)
                        rowIndex += 1
                    Next

                    Dim total = data.List.Sum(Function(x) x.Amount)
                    .WriteToCell(rowIndex, 7, "總計")
                    .WriteToCell(rowIndex, 8, total)

                    '存檔
                    SaveExcel($"應收票據_{data.CompanyName}_{data.BankAccount}_{month:yyyyMM}", xml)
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
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "發票範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim rowIndex = 2
                    Dim totalNotInoice As Integer

                    For Each item In data

                        .WriteToCell(rowIndex, 1, item.CusCode)
                        .WriteToCell(rowIndex, 2, item.CusName)
                        .WriteToCell(rowIndex, 3, item.TaxId)
                        .WriteToCell(rowIndex, 4, item.Amount)
                        .WriteToCell(rowIndex, 5, item.IsInvoice)
                        Dim notInvoice = item.Amount - item.IsInvoice
                        .WriteToCell(rowIndex, 6, item.Amount - item.IsInvoice)

                        totalNotInoice += notInvoice
                        .InsertRow(rowIndex)
                        rowIndex += 1
                    Next

                    .WriteToCell(rowIndex, 3, "總計")
                    .WriteToCell(rowIndex, 4, data.Sum(Function(x) x.Amount))
                    .WriteToCell(rowIndex, 5, data.Sum(Function(x) x.IsInvoice))
                    .WriteToCell(rowIndex, 6, totalNotInoice)

                    '存檔
                    SaveExcel($"發票_{month:yyyyMM}", xml)
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
                        .WriteToCell(rowIndex, 2, item.AccountsReceivable)
                        .WriteToCell(rowIndex, 3, item.AccountsReceived)

                        If item.AccountsReceivable < item.AccountsReceived Then
                            Dim over = item.AccountsReceived - item.AccountsReceivable
                            totalOver += over
                            .WriteToCell(rowIndex, 4, over)
                            .WriteToCell(rowIndex, 5, 0)
                        Else
                            Dim notCollect = item.AccountsReceivable - item.AccountsReceived
                            totalNotCollect += notCollect
                            .WriteToCell(rowIndex, 4, 0)
                            .WriteToCell(rowIndex, 5, notCollect)
                        End If

                        .WriteToCell(rowIndex, 6, item.Discount)

                        .InsertRow(rowIndex)
                        rowIndex += 1
                    Next

                    .WriteToCell(rowIndex, 1, "合計")
                    .WriteToCell(rowIndex, 2, data.List.Sum(Function(x) x.AccountsReceivable))
                    .WriteToCell(rowIndex, 3, data.List.Sum(Function(x) x.AccountsReceived))
                    .WriteToCell(rowIndex, 4, totalOver)
                    .WriteToCell(rowIndex, 5, totalNotCollect)
                    .WriteToCell(rowIndex, 6, data.List.Sum(Function(x) x.Discount))

                    '存檔
                    SaveExcel($"月應收帳明細_{month:yyyyMM}", xml)
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
                        .WriteToCell(5, colIndex, monthData.OpeningBalance)

                        '各廠商的進貨量
                        For i As Integer = 0 To allVendors.Count - 1
                            Dim vendor = allVendors(i)

                            If monthData.PurchasesByVendor.ContainsKey(vendor) Then .WriteToCell(6 + i, colIndex, monthData.PurchasesByVendor(vendor))
                        Next

                        '進貨總數
                        Dim totalMonthlyPurchase = monthData.PurchasesByVendor.Sum(Function(x) x.Value)
                        .WriteToCell(19, colIndex, totalMonthlyPurchase)

                        '銷售總數
                        .WriteToCell(20, colIndex, monthData.Sale)

                        '期末存量
                        .WriteToCell(21, colIndex, monthData.CloseingBalance)

                        '差異
                        .WriteToCell(22, colIndex, monthData.OpeningBalance + totalMonthlyPurchase - monthData.Sale - monthData.CloseingBalance)
                    Next

                    '存檔
                    SaveExcel($"進銷存明細表_{data.Company}_{year:yyyy}", xml)
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 產生應付票據
    ''' </summary>
    ''' <param name="month"></param>
    Public Sub GeneratePayableCheck(month As Date)
        Try
            '取得資料
            Dim data = _rep.GetPayableCheck(month)

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "應付票據範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    For i As Integer = 0 To data.Count - 1
                        .WriteToCell(i + 3, 1, data(i).Day)
                        .WriteToCell(i + 3, 2, data(i).ChequeNumber)
                        .WriteToCell(i + 3, 3, data(i).Amount)
                        .WriteToCell(i + 3, 4, data(i).CashingDate)
                        .WriteToCell(i + 3, 5, data(i).Memo)
                        .WriteToCell(i + 3, 6, data(i).IsCashing)
                    Next

                    '存檔
                    SaveExcel($"應付票據_{month:yyyyMM}", xml)
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
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "財稅範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    .WriteToCell(1, 1, data.Month.ToString("yyyy年MM月") + " 財稅")

                    Dim rowIndex = 3

                    For Each item In data.List
                        .WriteToCell(rowIndex, 1, item.Day.ToString("MM/dd"))
                        .WriteToCell(rowIndex, 2, item.InvoiceNum)
                        .WriteToCell(rowIndex, 3, item.TaxId)
                        .WriteToCell(rowIndex, 4, item.UnitPrice)
                        .WriteToCell(rowIndex, 5, item.Quantity)
                        .WriteToCell(rowIndex, 6, item.Tax)
                        .WriteToCell(rowIndex, 7, item.Amount)
                        .WriteToCell(rowIndex, 8, item.Memo)

                        rowIndex += 1
                    Next

                    .SetCustomBorders(rowIndex - 1, 1, rowIndex - 1, 8, XLBorderStyleValues.Thin)
                    .WriteToCell(rowIndex, 4, "合計")
                    .WriteToCell(rowIndex, 5, data.List.Sum(Function(x) x.Quantity))
                    .WriteToCell(rowIndex, 6, data.List.Sum(Function(x) x.Tax))
                    .WriteToCell(rowIndex, 7, data.List.Sum(Function(x) x.Amount))

                    '存檔
                    SaveExcel($"財稅_{month:yyyyMM}", xml)
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
                    SaveExcel($"能源局_{month:yyyyMM}", xml)
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub AddHeaderRow(xml As CloseXML_Excel, rowIndex As Integer, colStart As Integer)
        Dim headers() As String = {"日期", "發票號碼", "統一編號", "數量"}
        For i As Integer = 0 To headers.Length - 1
            xml.WriteToCell(rowIndex, colStart + i, headers(i))
        Next

        xml.SetCustomBorders(rowIndex, 1, rowIndex, 8, bottomStyle:=ClosedXML.Excel.XLBorderStyleValues.Thin)
    End Sub

    Private Sub WriteItemToSheet(xml As CloseXML_Excel, rowIndex As Integer, startCol As Integer, item As EnergyBureau)
        With xml
            .WriteToCell(rowIndex, startCol, item.Day.ToString("MM/dd"))
            .WriteToCell(rowIndex, startCol + 1, item.InvoiceNum)
            .WriteToCell(rowIndex, startCol + 2, item.TaxId)
            .WriteToCell(rowIndex, startCol + 3, item.Quantity)
        End With
    End Sub

    Private Sub AddTotalRow(xml As CloseXML_Excel, rowIndex As Integer, colStart As Integer, page As Integer, total As Integer)
        With xml
            .WriteToCell(rowIndex, colStart, "總計")
            .WriteToCell(rowIndex, colStart + 3, total)
            .SetCustomBorders(rowIndex, 1, rowIndex, 8, topStyle:=ClosedXML.Excel.XLBorderStyleValues.Thin)
        End With
    End Sub

    ''' <summary>
    ''' 產生月結帳單
    ''' </summary>
    ''' <param name="cusId"></param>
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
                    .WriteToCell(1, 1, data.CompanyName + $" {month.Year}年{month.Month}月對帳單:")
                    .WriteToCell(1, 4, data.CusCode)

                    '家用瓦斯(變動前)
                    .WriteToCell(3, 2, data.GasNormalQuantity_First)
                    .WriteToCell(3, 3, data.GasNormalUnitPrice_First)

                    Dim gasAmount_1 = data.GasNormalQuantity_First * data.GasNormalUnitPrice_First
                    .WriteToCell(3, 4, gasAmount_1)

                    '工業氣(變動前)
                    .WriteToCell(4, 2, data.GasCQuantity_First)
                    .WriteToCell(4, 3, data.GasCUnitPrice_First)

                    Dim gasCAmount_1 = data.GasCQuantity_First * data.GasCUnitPrice_First
                    .WriteToCell(4, 4, gasCAmount_1)

                    '家用瓦斯(變動後)
                    .WriteToCell(5, 2, data.GasNormalQuantity)
                    .WriteToCell(5, 3, data.GasNormalUnitPrice)

                    Dim gasAmount = data.GasNormalQuantity * data.GasNormalUnitPrice
                    .WriteToCell(5, 4, gasAmount)

                    '工業氣(變動後)
                    .WriteToCell(6, 2, data.GasCQuantity)
                    .WriteToCell(6, 3, data.GasCUnitPrice)

                    Dim gasCAmount = data.GasCQuantity * data.GasCUnitPrice
                    .WriteToCell(6, 4, gasCAmount)

                    '保險
                    Dim totalQty = data.GasNormalQuantity_First + data.GasCQuantity_First + data.GasNormalQuantity + data.GasCQuantity
                    .WriteToCell(7, 2, totalQty)
                    .WriteToCell(7, 3, data.InsuranceUnitPrice)

                    Dim insuranceAmount = totalQty * data.InsuranceUnitPrice
                    .WriteToCell(7, 4, insuranceAmount)


                    Dim gasAccountsRecievable = gasAmount_1 + gasCAmount_1 + gasAmount + gasCAmount

                    If Not data.IsInsurance Then gasAccountsRecievable += insuranceAmount

                    .WriteToCell(8, 2, gasAccountsRecievable)
                    .WriteToCell(9, 2, data.GasAccountsReceived)
                    .WriteToCell(10, 2, data.NewBerralAccountsReceivable)
                    .WriteToCell(11, 2, gasAccountsRecievable + data.NewBerralAccountsReceivable - data.GasAccountsReceived)
                    .WriteToCell(12, 2, data.NewBerralTypesCount)

                    '存檔
                    SaveExcel($"月對帳單_{data.CusCode}_{month:yyyyMM}", xml)
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
                        .WriteToCell(rowIndex, 4, item.Amount)

                        rowIndex += 1
                    Next

                    .WriteToCell(rowIndex, 3, "合計")
                    .WriteToCell(rowIndex, 4, data.List.Sum(Function(x) x.Amount))
                    .SetCustomBorders(rowIndex, 1, rowIndex, 4, topStyle:=XLBorderStyleValues.Thin)

                    '存檔
                    SaveExcel($"保險_{data.CompanyName}_{month:yyyyMM}", xml)
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
                    .WriteToCell(6, 3, data.GasIncome)
                    .WriteToCell(7, 3, data.SalesDiscount)

                    ' 營業收入
                    .WriteToCell(5, 4, data.OperatingIncome)

                    ' 營業費用(上)
                    .WriteToCell(9, 3, data.Income)
                    .WriteToCell(8, 4, data.Income)

                    ' 銷貨毛利
                    Dim grossProfit = data.OperatingIncome - data.Income
                    .WriteToCell(10, 4, grossProfit)

                    ' 營業費用(下)
                    totalOperatingExpenses = data.PaymentList.Sum(Function(x) x.Amount)
                    .WriteToCell(11, 4, totalOperatingExpenses)

                    Dim rowIndex = 12

                    For Each item In data.PaymentList
                        .WriteToCell(rowIndex, 2, item.Subject)
                        .WriteToCell(rowIndex, 3, item.Amount)
                        rowIndex += 1
                    Next

                    ' 營業外收益
                    totalNonOperatingIncome = data.CollectionsList.Sum(Function(x) x.Amount)
                    .WriteToCell(rowIndex, 1, "營業外收益", New CloseXML_Excel.CellFormatOptions With {.IsBold = True, .Horizontal = XLAlignmentHorizontalValues.Left})


                    .WriteToCell(rowIndex, 4, totalNonOperatingIncome)
                    rowIndex += 1

                    For Each item In data.CollectionsList
                        .WriteToCell(rowIndex, 2, item.Subject)
                        .WriteToCell(rowIndex, 3, item.Amount)
                        rowIndex += 1
                    Next

                    .SetCustomBorders(rowIndex, 1, rowIndex, 4, topStyle:=XLBorderStyleValues.Thin)
                    .WriteToCell(rowIndex, 1, "存貨")
                    rowIndex += 1

                    Dim netIncome As Single = grossProfit - totalOperatingExpenses + totalNonOperatingIncome
                    .WriteToCell(rowIndex, 1, "本期損益", New CloseXML_Excel.CellFormatOptions With {.IsBold = True, .Horizontal = XLAlignmentHorizontalValues.Left})
                    .WriteToCell(rowIndex, 4, netIncome)

                    '存檔
                    SaveExcel($"損益表_{data.CompanyName}_{startDate:yyyyMMdd}-{endDate:yyyyMMdd}", xml)
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
                                .WriteToCell(rowIndex, startColumn + 1, group.Qty)
                                .WriteToCell(rowIndex, startColumn + 2, group.TaxAmount)
                                .WriteToCell(rowIndex, startColumn + 3, group.Amount)
                                rowIndex += 1
                            Next

                            rowIndex += 5 - monthData.RegularInvoices.Count

                            Dim kgSubTotal = monthData.RegularInvoices.Sum(Function(x) x.Qty)
                            Dim taxAmountSubTotal = monthData.RegularInvoices.Sum(Function(x) x.TaxAmount)
                            Dim amountSubTotal = monthData.RegularInvoices.Sum(Function(x) x.Amount)

                            Dim twoPartMachine = monthData.SpecialInvoices.TwoPartMachine
                            .WriteToCell(rowIndex, startColumn + 1, twoPartMachine.Qty)
                            kgSubTotal += twoPartMachine.Qty
                            .WriteToCell(rowIndex, startColumn + 2, twoPartMachine.TaxAmount)
                            taxAmountSubTotal += twoPartMachine.TaxAmount
                            .WriteToCell(rowIndex, startColumn + 3, twoPartMachine.Amount)
                            amountSubTotal += twoPartMachine.Amount
                            rowIndex += 1

                            Dim threePartHandwritten = monthData.SpecialInvoices.ThreePartHandwritten
                            .WriteToCell(rowIndex, startColumn + 1, threePartHandwritten.Qty)
                            kgSubTotal += threePartHandwritten.Qty
                            .WriteToCell(rowIndex, startColumn + 2, threePartHandwritten.TaxAmount)
                            taxAmountSubTotal += threePartHandwritten.TaxAmount
                            .WriteToCell(rowIndex, startColumn + 3, threePartHandwritten.Amount)
                            amountSubTotal += threePartHandwritten.Amount
                            rowIndex += 1

                            Dim twoPartHandwritten = monthData.SpecialInvoices.TwoPartHandwritten
                            .WriteToCell(rowIndex, startColumn + 1, twoPartHandwritten.Qty)
                            kgSubTotal += twoPartHandwritten.Qty
                            .WriteToCell(rowIndex, startColumn + 2, twoPartHandwritten.TaxAmount)
                            taxAmountSubTotal += twoPartHandwritten.TaxAmount
                            .WriteToCell(rowIndex, startColumn + 3, twoPartHandwritten.Amount)
                            amountSubTotal += twoPartHandwritten.Amount
                            rowIndex += 1

                            '小計
                            .WriteToCell(rowIndex, startColumn + 1, kgSubTotal)
                            kgTotal += kgSubTotal
                            .WriteToCell(rowIndex, startColumn + 2, taxAmountSubTotal)
                            taxAmountTotal += taxAmountSubTotal
                            .WriteToCell(rowIndex, startColumn + 3, amountSubTotal)
                            amountTotal += amountSubTotal

                            rowIndex += 2
                        Next

                        '合計
                        .WriteToCell(rowIndex - 1, startColumn + 1, kgTotal)
                        .WriteToCell(rowIndex - 1, startColumn + 2, taxAmountTotal)
                        .WriteToCell(rowIndex - 1, startColumn + 3, amountTotal)
                    Next

                    '分裝場
                    Dim taxSubTotal As Single = 0
                    Dim amountSubTotal_split As Single = 0

                    .WriteToCell(4, 11, splitCompanyData.FrontDate1)
                    .WriteToCell(4, 14, splitCompanyData.FrontTax1)
                    taxSubTotal += splitCompanyData.FrontTax1
                    .WriteToCell(4, 15, splitCompanyData.FrontAmount1)
                    amountSubTotal_split += splitCompanyData.FrontAmount1
                    .WriteToCell(4, 16, splitCompanyData.FrontVendorTaxId1)

                    .WriteToCell(5, 11, splitCompanyData.FrontDate2)
                    .WriteToCell(5, 14, splitCompanyData.FrontTax2)
                    taxSubTotal += splitCompanyData.FrontTax2
                    .WriteToCell(5, 15, splitCompanyData.FrontAmount2)
                    amountSubTotal_split += splitCompanyData.FrontAmount2
                    .WriteToCell(5, 16, splitCompanyData.FrontVendorTaxId2)

                    .WriteToCell(6, 11, splitCompanyData.EndDate1)
                    .WriteToCell(6, 14, splitCompanyData.EndTax1)
                    taxSubTotal += splitCompanyData.EndTax1
                    .WriteToCell(6, 15, splitCompanyData.EndAmount1)
                    amountSubTotal_split += splitCompanyData.EndAmount1
                    .WriteToCell(6, 16, splitCompanyData.EndVendorTaxId1)

                    .WriteToCell(7, 11, splitCompanyData.EndDate2)
                    .WriteToCell(7, 14, splitCompanyData.EndTax2)
                    taxSubTotal += splitCompanyData.EndTax2
                    .WriteToCell(7, 15, splitCompanyData.EndAmount2)
                    amountSubTotal_split += splitCompanyData.EndAmount2
                    .WriteToCell(7, 16, splitCompanyData.EndVendorTaxId2)

                    '小計
                    .WriteToCell(8, 14, taxSubTotal)
                    .WriteToCell(8, 15, amountSubTotal_split)

                    Dim rowIndex_split As Integer = 10

                    For Each item In splitCompanyData.InList
                        .WriteToCell(rowIndex_split, 11, item.Day)
                        .WriteToCell(rowIndex_split, 12, item.InvoiceNum)
                        .WriteToCell(rowIndex_split, 13, item.Name)
                        .WriteToCell(rowIndex_split, 14, item.Tax)
                        .WriteToCell(rowIndex_split, 15, item.Amount)
                        .WriteToCell(rowIndex_split, 16, item.VendorTaxId)
                        .SetCustomBorders(rowIndex_split, 11, rowIndex_split, 16, ClosedXML.Excel.XLBorderStyleValues.Thin, ClosedXML.Excel.XLBorderStyleValues.Thin,
                                          ClosedXML.Excel.XLBorderStyleValues.Thin, ClosedXML.Excel.XLBorderStyleValues.Thin)
                        rowIndex_split += 1
                    Next

                    '存檔
                    SaveExcel($"進項銷項_{year:yyyy}_{month}", xml)
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function WriteSubjectGroup(xml As CloseXML_Excel, subjects As List(Of IncomeStatementItem), rowIndex As Integer, ByRef total As Single) As Integer
        total = subjects.Sum(Function(x) x.Amount)

        With xml
            .WriteToCell(rowIndex, 4, total)
            rowIndex += 1

            For Each item In subjects
                .WriteToCell(rowIndex, 2, item.Subject)
                .WriteToCell(rowIndex, 3, item.Amount)
                rowIndex += 1
            Next
        End With

        Return rowIndex
    End Function

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

    Private Sub SaveExcel(fileName As String, xml As CloseXML_Excel)
        Using saveDialog As New SaveFileDialog
            With saveDialog
                .Filter = "Excel檔案|*.xlsx"
                .FileName = fileName + ".xlsx"
            End With

            If saveDialog.ShowDialog = DialogResult.OK Then
                xml.SaveAs(saveDialog.FileName)
            End If
        End Using

    End Sub
End Class