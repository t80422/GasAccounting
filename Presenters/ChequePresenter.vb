Imports System.IO
Imports ClosedXML.Excel

Public Class ChequePresenter
    Private _view As ICheque
    Private ReadOnly _cheRep As IChequeRep
    Private ReadOnly _printerSer As IPrinterService

    Public Sub New(cheRep As IChequeRep, printerSer As IPrinterService)
        _cheRep = cheRep
        _printerSer = printerSer
    End Sub

    Public Sub SetView(view As ICheque)
        _view = view
    End Sub

    ''' <summary>
    ''' 取得列表
    ''' </summary>
    ''' <param name="conditions"></param>
    Public Sub LoadList(Optional conditions As ChequeSC = Nothing)
        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.cheques.AsQueryable

                If conditions IsNot Nothing Then
                    If conditions.IsDate Then query = query.Where(Function(x) x.che_ReceivedDate > conditions.StartDate AndAlso x.che_ReceivedDate < conditions.EndDate)
                    If Not String.IsNullOrEmpty(conditions.Status) Then query = query.Where(Function(x) x.chu_State = conditions.Status)
                Else
                    query = query.Where(Function(x) x.chu_State <> "已兌現")
                End If
                _view.ShowList(SetListViewModel(query))
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 批次設定狀態
    ''' </summary>
    ''' <param name="chequeIds"></param>
    ''' <param name="d"></param>
    ''' <param name="isCollect">true:已代收 false:已兌現</param>
    Public Async Sub SetBatchStatus(chequeIds As List(Of Integer), d As Date, isCollect As Boolean)
        Try
            If isCollect Then
                Await _cheRep.UpdateCollectionStatusAsync(chequeIds, d)
            Else
                _cheRep.UpdateRedeemedStatus(chequeIds, d)
            End If

            LoadList()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 取得選取的資料
    ''' </summary>
    ''' <param name="id"></param>
    Public Sub SelectRow(id As Integer)
        Try
            Using db As New gas_accounting_systemEntities
                Dim data = db.cheques.Find(id)
                If data IsNot Nothing Then
                    _view.ClearInput()
                    _view.SetDataToControl(data)
                End If
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function IsCollected(cheNum As String) As Boolean
        Try
            Dim state = _cheRep.GetState(cheNum)
            If state = "已代收" Then Return True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return False
    End Function

    ''' <summary>
    ''' 列出未兌現清單
    ''' </summary>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    Public Sub ShowCollectionYetList(startDate As Date, endDate As Date)
        Dim list = _cheRep.Query(startDate, endDate, New cheque With {.chu_State = "未兌現"})
        _view.ShowList(list)
    End Sub

    Public Sub Query(startDate As Date, endDate As Date)
        _view.ShowList(_cheRep.Query(startDate, endDate))
    End Sub

    Public Sub Print(datas As List(Of ChequeVM))
        Try
            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "支票管理範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim rowIndex = 3

                    For Each item In datas
                        .WriteToCell(rowIndex, 1, item.收票日期.Value.ToString("yyyy/MM/dd"))
                        .WriteToCell(rowIndex, 2, item.支票號碼)
                        .WriteToCell(rowIndex, 3, item.銀行帳號)
                        .WriteToCell(rowIndex, 4, item.發票人)
                        .WriteToCell(rowIndex, 5, item.金額.ToString)
                        .WriteToCell(rowIndex, 6, item.狀態)
                        .WriteToCell(rowIndex, 7, If(item.兌現日期.HasValue, item.兌現日期.Value.ToString("yyyy/MM/dd"), ""))
                        .WriteToCell(rowIndex, 8, If(item.代收日期.HasValue, item.代收日期.Value.ToString("yyyy/MM/dd"), ""))

                        rowIndex += 1
                    Next
                    .SetCustomBorders(rowIndex, 1, rowIndex, 8, XLBorderStyleValues.Thin)
                    .WriteToCell(rowIndex, 4, "合計")
                    .WriteToCell(rowIndex, 5, datas.Sum(Function(x) x.金額).ToString("N0"))

                    .SaveExcel("支票管理")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function SetListViewModel(query As IQueryable(Of cheque)) As List(Of ChequeVM)
        Return query.Select(Function(x) New ChequeVM With {
            .代收日期 = If(x.che_CollectionDate, Nothing),
            .兌現日期 = If(x.che_CashingDate, Nothing),
            .支票號碼 = x.che_Number,
            .收票日期 = If(x.che_ReceivedDate, Nothing),
            .狀態 = x.chu_State,
            .發票人 = x.che_IssuerName,
            .金額 = x.che_Amount,
            .銀行帳號 = x.che_AccountNumber,
            .編號 = x.che_Id
        }).ToList
    End Function
End Class