Public Class CollectionPresenter
    Inherits BasePresenter(Of collection, CollectionVM, ICollectionView)
    Implements IPresenter(Of collection, CollectionVM)

    Private _subjectsService As SubjectsService
    Private _bankService As BankService
    Private _companyService As CompanyService

    Public Sub New(view As ICollectionView, subjectsService As SubjectsService, bankService As BankService, companyService As CompanyService)
        MyBase.New(view)
        _presenter = Me
        _subjectsService = subjectsService
        _bankService = bankService
        _companyService = companyService
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of collection), conditions As Object) As IQueryable(Of collection) Implements IPresenter(Of collection, CollectionVM).SetSearchConditions
        Dim con As CollectionQueryVM = conditions
        con.EndDate = con.EndDate.AddDays(1).AddSeconds(-1)
        query = query.Where(Function(x) x.col_Date >= con.StartDate AndAlso x.col_Date <= con.EndDate)

        If con.Cheque <> "" Then query = query.Where(Function(x) x.col_Cheque = con.Cheque)
        If con.CusId <> 0 Then query = query.Where(Function(x) x.col_cus_Id = con.CusId)
        If con.Subjects <> 0 Then query = query.Where(Function(x) x.col_s_Id = con.Subjects)
        If con.Type <> "" Then query = query.Where(Function(x) x.col_Type = con.Type)

        Return query
    End Function

    Public Function SetListViewModel(query As IQueryable(Of collection)) As List(Of CollectionVM) Implements IPresenter(Of collection, CollectionVM).SetListViewModel
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

    Public Overrides Sub Add()
        If Not CheckRequired(_view.SetRequired()) Then Exit Sub

        Dim data = _view.GetUserInput

        If data IsNot Nothing Then
            Try
                Using db As New gas_accounting_systemEntities
                    db.collections.Add(data)

                    If data.col_Type = "支票" Then
                        db.cheques.Add(_view.GetChequeDatas)
                    End If

                    db.SaveChanges()
                    LoadList()
                    _view.ClearInput()
                    MsgBox("新增成功")
                End Using

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Public Overrides Sub Edit(id As Integer)
        If Not CheckRequired(_view.SetRequired()) Then Exit Sub

        Dim data = _view.GetUserInput

        Try
            Using db As New gas_accounting_systemEntities
                If data IsNot Nothing Then
                    Dim collection = db.collections.Find(id)

                    If collection IsNot Nothing Then
                        If data.col_Type = "支票" Then
                            Dim cheques = db.cheques.FirstOrDefault(Function(x) x.che_Number = collection.col_Cheque)

                            If cheques IsNot Nothing Then
                                Dim updateCheques = _view.GetChequeDatas
                                updateCheques.che_Id = cheques.che_Id
                                db.Entry(cheques).CurrentValues.SetValues(updateCheques)
                            End If
                        End If

                        db.Entry(collection).CurrentValues.SetValues(data)
                        db.SaveChanges()
                        LoadList()
                        MsgBox("修改成功。")
                    Else
                        MsgBox("未找到指定的對象。")
                    End If
                End If
            End Using

        Catch ex As Exception
            MsgBox("修改時發生錯誤: " & ex.Message)
        End Try
    End Sub

    Public Overrides Sub Delete(id As Integer)
        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.No Then Exit Sub

        Try
            Using db As New gas_accounting_systemEntities
                Dim data = db.collections.Find(id)
                If data IsNot Nothing Then
                    If data.col_Type = "支票" Then
                        Dim cheques = db.cheques.FirstOrDefault(Function(x) x.che_Number = data.col_Cheque)

                        If cheques IsNot Nothing Then
                            db.cheques.Remove(cheques)
                        End If
                    End If

                    db.collections.Remove(data)
                    db.SaveChanges()
                    LoadList()
                    _view.ClearInput()
                    MsgBox("刪除成功。")

                Else
                    MsgBox("未找到要刪除的對象。")
                End If
            End Using

        Catch ex As Exception
            MsgBox("刪除時發生錯誤: " & ex.Message)
        End Try
    End Sub
End Class
