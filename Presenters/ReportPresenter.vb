Imports System.IO

Public Class ReportPresenter
    Private _rep As IReportRep
    Private _manuRep As IManufacturerService = New ManufacturerService
    Private _view As IReportView

    Public Sub New(view As IReportView, reportRep As IReportRep)
        _view = view
        _rep = reportRep
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

                    For i As Integer = 0 To datas.Count - 1
                        rowIndex = 5 + i

                        .WriteToCell(rowIndex, 1, datas(i).客戶名稱)
                        .WriteToCell(rowIndex, 2, If(datas(i).存氣 <> Nothing, datas(i).存氣.ToString("#,##"), 0))
                        .WriteToCell(rowIndex, 3, If(datas(i).本日提量 <> Nothing, datas(i).本日提量.ToString("#,##"), "0"))
                        .WriteToCell(rowIndex, 4, If(datas(i).當月累計提量 <> Nothing, datas(i).當月累計提量.ToString("#,##"), "0"))
                        .WriteToCell(rowIndex, 5, If(datas(i).本日氣款 <> Nothing, datas(i).本日氣款.ToString("#,##"), "0"))
                        .WriteToCell(rowIndex, 6, If(datas(i).本日收款 <> Nothing, datas(i).本日收款.ToString("#,##"), "0"))
                        .WriteToCell(rowIndex, 7, If(datas(i).結欠 <> Nothing, datas(i).結欠.ToString("#,##"), "0"))

                    Next

                    .SetBottomBorder(rowIndex, 1, rowIndex, 7)

                    rowIndex += 1

                    .WriteToCell(rowIndex, 1, "合計:")
                    .WriteToCell(rowIndex, 2, datas.Sum(Function(x) x.存氣).ToString("#,##"))
                    .WriteToCell(rowIndex, 3, datas.Sum(Function(x) x.本日提量).ToString("#,##"))
                    .WriteToCell(rowIndex, 4, datas.Sum(Function(x) x.當月累計提量).ToString("#,##"))
                    .WriteToCell(rowIndex, 5, datas.Sum(Function(x) x.本日氣款).ToString("#,##"))
                    .WriteToCell(rowIndex, 6, datas.Sum(Function(x) x.本日收款).ToString("#,##"))
                    .WriteToCell(rowIndex, 7, datas.Sum(Function(x) x.結欠).ToString("#,##"))


                    Dim exportFilePath = Path.Combine(Application.StartupPath, "報表", "日氣量氣款收付明細表.xlsx")
                    .SaveAs(exportFilePath)
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

                    .WriteToCell(3, 1, $"提氣日期: {d:yyyy/MM/dd}")
                    .WriteToCell(3, 23, $"列印日期: {Now:yyyy/MM/dd}")

                    Dim rowIndex As Integer

                    For i As Integer = 0 To datas.Count - 1
                        rowIndex = 6 + i

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
                        .WriteToCell(rowIndex, 12, If(datas(i).普氣15Kg <> Nothing, datas(i).普氣15Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 13, If(datas(i).丙氣15Kg <> Nothing, datas(i).丙氣15Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 14, If(datas(i).普氣14Kg <> Nothing, datas(i).普氣14Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 15, If(datas(i).丙氣14Kg <> Nothing, datas(i).丙氣14Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 16, If(datas(i).普氣5Kg <> Nothing, datas(i).普氣5Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 17, If(datas(i).丙氣5Kg <> Nothing, datas(i).丙氣5Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 18, If(datas(i).普氣2Kg <> Nothing, datas(i).普氣2Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 19, If(datas(i).丙氣2Kg <> Nothing, datas(i).丙氣2Kg.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 20, If(datas(i).普氣瓶數 <> Nothing, datas(i).普氣瓶數.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 21, If(datas(i).丙氣瓶數 <> Nothing, datas(i).丙氣瓶數.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 22, If(datas(i).普氣Kg數 <> Nothing, datas(i).普氣Kg數.ToString("#,##"), ""))
                        .WriteToCell(rowIndex, 23, If(datas(i).丙氣Kg數 <> Nothing, datas(i).丙氣Kg數.ToString("#,##"), ""))
                    Next

                    Dim exportFilePath = Path.Combine(Application.StartupPath, "報表", "客戶提氣清冊.xlsx")
                    .SaveAs(exportFilePath)
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

                    .WriteToCell(1, 1, $"{d:yyyy}年豐原液化煤氣分裝場應付明細帳")
                    .WriteToCell(3, 1, datas.FirstOrDefault().廠商)

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
                    .SetBottomBorder(rowIndex, 1, rowIndex, 11)

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

                    Dim exportFilePath = Path.Combine(Application.StartupPath, "報表", "大氣進貨明細.xlsx")
                    .SaveAs(exportFilePath)
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
    Public Sub GenerateDailyCustomerReceivable(d As Date, cusCode As Integer)
        Try
            '蒐集資料
            Dim datas = _rep.DailyCustomerReceivable(d, cusCode)

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "單一客戶每日的應收帳明細表範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    .WriteToCell(1, 1, $"{d:yyyy}年豐原液化煤氣分裝場氣款應收帳")
                    .WriteToCell(4, 1, datas.FirstOrDefault().客戶名稱)

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

                    Dim exportFilePath = Path.Combine(Application.StartupPath, "報表", "單一客戶每日的應收帳明細表.xlsx")
                    .SaveAs(exportFilePath)
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
                        .WriteToCell(rowIndex, 11, datas(i).瓦斯瓶15Kg)
                        .WriteToCell(rowIndex, 12, datas(i).瓦斯瓶14Kg)
                        .WriteToCell(rowIndex, 13, datas(i).瓦斯瓶5Kg)
                        .WriteToCell(rowIndex, 14, datas(i).瓦斯瓶2Kg)
                    Next

                    Dim exportFilePath = Path.Combine(Application.StartupPath, "報表", "提量支數統計.xlsx")
                    .SaveAs(exportFilePath)
                End With
            End Using
        Catch ex As Exception
            MsgBox("列印失敗:" + ex.Message)
        End Try
    End Sub
End Class
