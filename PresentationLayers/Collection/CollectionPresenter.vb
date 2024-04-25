Public Class CollectionPresenter
    Private _view As ICollectionView
    Private _subjectsService As SubjectsService
    Private _bankService As BankService
    Private _companyService As CompanyService
    Private _colRep As ICollectionRep = New CollectionRep

    Public Sub New(view As ICollectionView, subjectsService As SubjectsService, bankService As BankService, companyService As CompanyService)
        _view = view
        _subjectsService = subjectsService
        _bankService = bankService
        _companyService = companyService
    End Sub

    ''' <summary>
    ''' 取得列表
    ''' </summary>
    ''' <param name="conditions"></param>
    Public Sub LoadList(Optional conditions As Object = Nothing)
        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.collections.AsNoTracking.AsQueryable

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
    ''' 取得選取的資料
    ''' </summary>
    ''' <param name="id"></param>
    Public Sub SelectRow(id As Integer)
        Try
            Using db As New gas_accounting_systemEntities
                Dim data = db.collections.Find(id)
                If data IsNot Nothing Then
                    Dim che = db.cheques.FirstOrDefault(Function(x) x.che_Number = data.col_Cheque)
                    _view.ClearInput()
                    _view.SetDataToControl(data, che)
                End If
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 取得科目選單
    ''' </summary>
    Public Sub GetSubjectsCmb()
        _view.SetSubjectsCmb(_subjectsService.GetSubjectsCmbItems("借"))
    End Sub

    ''' <summary>
    ''' 取得銀行選單
    ''' </summary>
    Public Sub GetBankCmb()
        _view.SetBankCmb(_bankService.GetBankCmbItems)
    End Sub

    ''' <summary>
    ''' 取得公司選單
    ''' </summary>
    Public Sub GetCompanyCmb()
        _view.SetCompanyCmb(_companyService.GetCompanyComboBoxData)
    End Sub

    Public Sub Query()
        LoadList(_view.GetQueryConditions)
    End Sub

    Public Sub Add()
        Dim ord = _view.GetUserInput

        If ord IsNot Nothing Then
            Dim journal As journal = If(ord.col_Type <> "支票", _view.GetJournalDatas, Nothing)
            Dim cheque As cheque = If(ord.col_Type = "支票", _view.GetChequeDatas, Nothing)

            Try
                _colRep.Add(ord, journal, cheque)
                _view.Reset()
                MsgBox("新增成功")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Public Sub Edit()
        Dim col = _view.GetUserInput

        If col IsNot Nothing Then
            Dim journal As journal = If(col.col_Type <> "支票", _view.GetJournalDatas, Nothing)
            Dim cheque As cheque = If(col.col_Type = "支票", _view.GetChequeDatas, Nothing)

            Try
                _colRep.Edit(col, journal, cheque)
                _view.Reset()
                MsgBox("修改成功")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Public Sub Delete(id As Integer)
        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.No Then Exit Sub
        Try
            _colRep.Delete(id)
            _view.Reset()
            MsgBox("刪除成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub UpdateCheque(colId As Integer)
        Try
            _colRep.UpdateCheque(colId)
            _view.Reset()
            MsgBox("兌現成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function SetSearchConditions(query As IQueryable(Of collection), conditions As CollectionQueryVM) As IQueryable(Of collection)
        conditions.EndDate = conditions.EndDate.AddDays(1).AddSeconds(-1)
        query = query.Where(Function(x) x.col_Date >= conditions.StartDate AndAlso x.col_Date <= conditions.EndDate)

        If conditions.Cheque <> "" Then query = query.Where(Function(x) x.col_Cheque = conditions.Cheque)
        If conditions.CusId <> 0 Then query = query.Where(Function(x) x.col_cus_Id = conditions.CusId)
        If conditions.Subjects <> 0 Then query = query.Where(Function(x) x.col_s_Id = conditions.Subjects)
        If conditions.Type <> "" Then query = query.Where(Function(x) x.col_Type = conditions.Type)

        Return query
    End Function

    Private Function SetListViewModel(query As IQueryable(Of collection)) As List(Of CollectionVM)
        Dim collections = query.ToList

        Return collections.Select(Function(x) New CollectionVM With {
            .備註 = x.col_Memo,
            .客戶名稱 = x.customer.cus_name,
            .帳款月份 = x.col_AccountMonth.ToString("yyyy年MM月"),
            .支票號碼 = x.col_Cheque,
            .收款類型 = x.col_Type,
            .日期 = x.col_Date,
            .科目 = x.subject.s_name,
            .編號 = x.col_Id,
            .金額 = x.col_Amount
        }).ToList
    End Function
End Class
