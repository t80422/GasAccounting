Public Class CollectionPresenter
    Private _view As ICollectionView
    Private ReadOnly _subjectRep As ISubjectRep
    Private _companyService As CompanyService
    Private ReadOnly _colRep As ICollectionRep
    Private ReadOnly _bankRep As IBankRep
    Private ReadOnly _cusRep As ICustomerRep
    Private ReadOnly _bmbService As IBankMonthlyBalanceService
    Private ReadOnly _chequeRep As IChequeRep

    Public Sub New(view As ICollectionView, subjectRep As ISubjectRep, companyService As CompanyService, colRep As ICollectionRep, bankRep As IBankRep, cusRep As ICustomerRep,
                   bmbService As IBankMonthlyBalanceService, chequeRep As IChequeRep)
        _view = view
        _subjectRep = subjectRep
        _companyService = companyService
        _colRep = colRep
        _bankRep = bankRep
        _cusRep = cusRep
        _bmbService = bmbService
        _chequeRep = chequeRep
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
    Public Async Sub GetSubjectsCmbAsync()
        Try
            Dim subjects = Await _subjectRep.GetSubjectDropdownAsync
            _view.SetSubjectsCmb(subjects)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 取得銀行選單
    ''' </summary>
    Public Async Sub LoadBankList()
        Dim banks = Await _bankRep.GetBankDropdownAsync
        _view.SetBankCmb(banks)
    End Sub

    ''' <summary>
    ''' 取得公司選單
    ''' </summary>
    Public Sub GetCompanyCmb()
        _view.ICollectionView_SetCompanyCmb(_companyService.GetCompanyComboBoxData)
    End Sub

    Public Sub Query()
        LoadList(_view.GetQueryConditions)
    End Sub

    Public Async Sub Add()
        Dim ord = _view.GetUserInput

        If ord IsNot Nothing Then
            Dim cheque As cheque = If(ord.col_Type = "支票", _view.GetChequeDatas, Nothing)

            If cheque IsNot Nothing Then cheque.che_col_Id = ord.col_Id

            Using transaction = _colRep.BeginTransaction
                Try
                    _colRep.Add(ord, cheque)

                    If ord.col_Type = "銀行" Then Await _bmbService.UpdateMonthBalanceAsync(ord.col_bank_Id, ord.col_AccountMonth)

                    transaction.Commit()
                    _view.Reset()
                    MsgBox("新增成功")
                Catch ex As Exception
                    transaction.Rollback()
                    MsgBox(ex.Message)
                End Try
            End Using
        End If
    End Sub

    Public Async Sub Edit()
        Dim col = _view.GetUserInput

        If col IsNot Nothing Then
            Dim cheque As cheque = If(col.col_Type = "支票", _view.GetChequeDatas, Nothing)
            cheque.che_col_Id = col.col_Id

            Using transaction = _colRep.BeginTransaction
                Try

                    Dim orgCollection = Await _colRep.GetByIdAsync(col.col_Id)
                    Dim orgAccountMonth = orgCollection.col_AccountMonth

                    _colRep.Edit(col, cheque)
                    Dim bankId = orgCollection.col_bank_Id
                    Dim updatedCollection = Await _colRep.GetByIdAsync(col.col_Id)

                    '處理銀行月結餘額
                    If bankId IsNot Nothing Then
                        '如果修改帳款月份,就更新原始月份的借、貸總額
                        If orgAccountMonth <> updatedCollection.col_AccountMonth Then Await _bmbService.UpdateMonthBalanceAsync(bankId, orgAccountMonth)

                        Await _bmbService.UpdateMonthBalanceAsync(bankId, updatedCollection.col_AccountMonth)
                    End If

                    transaction.Commit()
                    _view.Reset()
                    MsgBox("修改成功")

                Catch ex As Exception
                    transaction.Rollback()
                    MsgBox(ex.Message)
                End Try
            End Using
        End If
    End Sub

    Public Async Sub DeleteAsync(id As Integer)
        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.No Then Exit Sub
        Using transaction = _colRep.BeginTransaction
            Try
                Dim collection = Await _colRep.GetByIdAsync(id)
                Dim payType = collection.col_Type
                Dim bankId = collection.col_bank_Id
                Dim accountMonth = collection.col_AccountMonth

                '刪除支票
                If payType = "支票" Then
                    Dim cheque = Await _chequeRep.GetByNumberAsync(collection.col_Cheque)
                    Await _chequeRep.DeleteAsync(cheque.che_Id)
                End If

                '刪除資料
                Await _colRep.DeleteAsync(id)

                '更新月結餘額
                If payType = "銀行" Then Await _bmbService.UpdateMonthBalanceAsync(bankId, accountMonth)

                transaction.Commit()
                _view.Reset()
                MsgBox("刪除成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
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

    Public Function GetCustomer(cusCode As String) As customer
        Return _cusRep.GetByCusCode(cusCode)
    End Function

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
