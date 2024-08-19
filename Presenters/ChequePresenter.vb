Public Class ChequePresenter
    Private _view As ICheque
    Private ReadOnly _cheRep As IChequeRep

    Public Sub New(view As ICheque, cheRep As IChequeRep)
        _view = view
        _cheRep = cheRep
    End Sub

    ''' <summary>
    ''' 取得列表
    ''' </summary>
    ''' <param name="conditions"></param>
    Public Sub LoadList(Optional conditions As Object = Nothing)
        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.cheques.AsQueryable

                If conditions IsNot Nothing Then
                    query = SetSearchConditions(query, conditions)
                End If

                _view.ShowList(SetListViewModel(query))
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 批次代收
    ''' </summary>
    ''' <param name="chequeIds"></param>
    ''' <param name="d"></param>
    Public Async Sub SetBatchCollection(chequeIds As List(Of Integer), d As Date)
        Try
            'Using db As New gas_accounting_systemEntities
            '    Dim datas = db.cheques.Where(Function(x) chequeIds.Contains(x.che_Id) AndAlso x.chu_State = "未兌現").ToList

            '    datas.ForEach(Sub(x) x.chu_State = "已代收" And x.che_CollectionDate = d)
            '    db.SaveChanges()
            '    LoadList()
            'End Using
            Await _cheRep.UpdateCollectionStatusAsync(chequeIds, d)
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

    Private Function SetSearchConditions(query As IQueryable(Of cheque), conditions As Object) As IQueryable(Of cheque)
        Throw New NotImplementedException()
    End Function

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
            .借貸 = x.che_Type,
            .編號 = x.che_Id
        }).ToList
    End Function
End Class
