Imports System.IO
Imports ClosedXML.Excel

Public Class ChequePresenter
    Private ReadOnly _view As ICheque
    Private ReadOnly _cheRep As IChequeRep
    Private ReadOnly _printerSer As IPrinterService

    Public ReadOnly Property View As ICheque
        Get
            Return _view
        End Get
    End Property

    Public Sub New(view As ICheque, cheRep As IChequeRep, printerSer As IPrinterService)
        _cheRep = cheRep
        _printerSer = printerSer
        _view = view

        AddHandler _view.CancelRequest, AddressOf Initialize
        AddHandler _view.DataSelectedRequest, AddressOf SelectRow
        AddHandler _view.SearchRequest, AddressOf Search
        AddHandler _view.SetBatchStatusRequest, AddressOf SetBatchStatus
        AddHandler _view.PrintRequest, AddressOf Print
    End Sub

    Private Sub Initialize()
        _view.ClearInput()
        LoadList()
    End Sub

    ''' <summary>
    ''' 取得選取的資料
    ''' </summary>
    ''' <param name="id"></param>
    Private Sub SelectRow(sender As Object, id As Integer)
        Try
            Dim data = _cheRep.GetByIdAsync(id).Result

            If data IsNot Nothing Then
                _cheRep.Reload(data)
                _view.ClearInput()
                _view.ShowDetail(data)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 取得列表
    ''' </summary>
    ''' <param name="conditions"></param>
    Private Sub LoadList(Optional conditions As ChequeSC = Nothing)
        Try
            Dim data = _cheRep.GetList(conditions)
            _view.ShowList(data.Select(Function(x) New ChequeVM(x)).ToList)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Search()
        Dim criteria = _view.GetSearchCriteria
        LoadList(criteria)
    End Sub

    ''' <summary>
    ''' 批次設定狀態
    ''' </summary>
    ''' <param name="chequeIds"></param>
    ''' <param name="d"></param>
    ''' <param name="isCollect">true:已代收 false:已兌現</param>
    Public Async Sub SetBatchStatus(sender As Object, isCollect As Boolean)
        Try
            Dim ids = _view.GetSelectedIds
            Dim input As New cheque
            _view.GetInput(input)

            If ids.Count = 0 Then Throw New Exception("請先選擇要設定的支票")

            If isCollect Then
                Await _cheRep.UpdateCollectionStatusAsync(ids, input.che_CollectionDate)
            Else
                _cheRep.UpdateRedeemedStatus(ids, input.che_CashingDate)
            End If

            LoadList()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Print(sender As Object, datas As List(Of ChequeVM))
        Try
            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "應收支票管理範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    .WriteToCell("A1", "應收支票管理")

                    Dim rowIndex = 3

                    For Each item In datas
                        .WriteToCell(rowIndex, 1, item.收票日期.Value.ToString("yyyy/MM/dd"))
                        .WriteToCell(rowIndex, 2, item.支票號碼)
                        .WriteToCell(rowIndex, 3, item.客戶代號)
                        .WriteToCell(rowIndex, 4, item.銀行帳號)
                        .WriteToCell(rowIndex, 5, item.發票人)
                        .WriteToCell(rowIndex, 6, item.金額.ToString)
                        .WriteToCell(rowIndex, 7, item.狀態)
                        .WriteToCell(rowIndex, 8, If(item.支票兌現日期.HasValue, item.支票兌現日期.Value.ToString("yyyy/MM/dd"), ""))
                        .WriteToCell(rowIndex, 9, If(item.代收日期.HasValue, item.代收日期.Value.ToString("yyyy/MM/dd"), ""))

                        rowIndex += 1
                    Next

                    .SetCustomBorders(rowIndex, 1, rowIndex, 8, XLBorderStyleValues.Thin)
                    .WriteToCell(rowIndex, 5, "合計")
                    .WriteToCell(rowIndex, 6, datas.Sum(Function(x) x.金額).ToString("N0"))

                    .SaveExcel("應收支票管理")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class