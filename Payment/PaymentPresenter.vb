Public Class PaymentPresenter
    Inherits BasePresenter(Of payment, PaymentVM, IPaymentView)
    Implements IPresenter(Of payment, PaymentVM)

    Private _manuService As ManufacturerService
    Private _bankService As BankService
    Private _subjectsService As SubjectsService

    Public Sub New(view As IPaymentView, manuService As ManufacturerService, bankService As BankService, subjectsService As SubjectsService)
        MyBase.New(view)
        _presenter = Me
        _manuService = manuService
        _bankService = bankService
        _subjectsService = subjectsService
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of payment), conditions As Object) As IQueryable(Of payment) Implements IPresenter(Of payment, PaymentVM).SetSearchConditions
        Dim c As PaymentQueryVM = conditions

        query = query.Where(Function(x) x.p_Date >= c.StartDate AndAlso x.p_Date <= c.EndDate)

        If c.BankId <> 0 Then query = query.Where(Function(x) x.p_bank_Id = c.BankId)
        If c.ManufacturerId <> 0 Then query = query.Where(Function(x) x.p_m_Id = c.ManufacturerId)
        If c.SubjectsGroupId <> 0 Then query = query.Where(Function(x) x.subject.subjects_group.sg_id = c.SubjectsGroupId)
        If c.SubjectsId <> 0 Then query = query.Where(Function(x) x.p_s_Id = c.SubjectsId)
        If c.Type <> "" Then query = query.Where(Function(x) x.p_Type = c.Type)

        Return query
    End Function

    Public Function SetListViewModel(query As IQueryable(Of payment)) As List(Of PaymentVM) Implements IPresenter(Of payment, PaymentVM).SetListViewModel
        Return query.Select(Function(x) New PaymentVM With {
            .付款類型 = x.p_Type,
            .備註 = x.p_Memo,
            .廠商名稱 = x.manufacturer.manu_name,
            .支票號碼 = x.p_Cheque,
            .日期 = x.p_Date,
            .科目 = x.subject.s_name,
            .編號 = x.p_Id,
            .金額 = x.p_Amount,
            .銀行名稱 = x.bank.bank_name
        }).ToList
    End Function

    Public Sub SetManuCmb()
        _view.SetManuCmb(_manuService.GetVendorCmbItems())
    End Sub

    Public Sub SetBankCmb()
        _view.SetBankCmb(_bankService.GetBankCmbItems)
    End Sub

    Public Sub SetSubjectsGroupCmb()
        _view.SetSubjectsGroupCmb(_subjectsService.GetSubjectsGroupCmbItems)
    End Sub

    Public Sub SetSubjectsCmb(sgId As Integer)
        _view.SetSubjectsCmb(_subjectsService.GetSubjectsCmbItems(sgId))
    End Sub

    Public Sub Query()
        LoadList(_view.GetQueryConditions)
    End Sub

    Public Overrides Sub Add()
        If Not _view.CheckDataRequired Then Exit Sub

        Dim data = _view.GetUserInput

        If data IsNot Nothing Then
            Try
                Using db As New gas_accounting_systemEntities
                    db.payments.Add(data)

                    If data.p_Type = "支票" Then
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
        If Not _view.CheckDataRequired Then Exit Sub

        Dim data = _view.GetUserInput

        Try
            Using db As New gas_accounting_systemEntities
                If data IsNot Nothing Then
                    Dim payment = db.payments.Find(id)

                    If payment IsNot Nothing Then
                        If data.p_Type = "支票" Then
                            Dim cheques = db.cheques.FirstOrDefault(Function(x) x.che_Number = payment.p_Cheque)

                            If cheques IsNot Nothing Then
                                Dim updateCheques = _view.GetChequeDatas
                                updateCheques.che_Id = cheques.che_Id
                                db.Entry(cheques).CurrentValues.SetValues(updateCheques)
                            End If
                        End If

                        db.Entry(payment).CurrentValues.SetValues(data)
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
                Dim data = db.payments.Find(id)
                If data IsNot Nothing Then
                    If data.p_Type = "支票" Then
                        Dim cheques = db.cheques.FirstOrDefault(Function(x) x.che_Number = data.p_Cheque)

                        If cheques IsNot Nothing Then
                            db.cheques.Remove(cheques)
                        End If
                    End If

                    db.payments.Remove(data)
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
