Public Class PaymentPresenter
    Inherits BasePresenter(Of payment, PaymentVM, IPaymentView)
    Implements IPresenter(Of payment, PaymentVM)

    Private _manuService As ManufacturerService
    Private _bankService As BankService
    Private _subjectsService As SubjectsService
    Private _companyService As CompanyService

    Private _journalService As IJournalService

    Public Sub New(view As IPaymentView, manuService As ManufacturerService, bankService As BankService, subjectsService As SubjectsService, companyService As CompanyService)
        MyBase.New(view)
        _presenter = Me
        _manuService = manuService
        _bankService = bankService
        _subjectsService = subjectsService
        _companyService = companyService
        _journalService = New JournalService
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of payment), conditions As Object) As IQueryable(Of payment) Implements IPresenter(Of payment, PaymentVM).SetSearchConditions
        Dim c As PaymentQueryVM = conditions

        query = query.Where(Function(x) x.p_Date >= c.StartDate AndAlso x.p_Date <= c.EndDate)

        If c.BankId <> 0 Then query = query.Where(Function(x) x.p_bank_Id = c.BankId)
        If c.ManufacturerId <> 0 Then query = query.Where(Function(x) x.p_m_Id = c.ManufacturerId)
        If c.SubjectsId <> 0 Then query = query.Where(Function(x) x.p_s_Id = c.SubjectsId)
        If c.Type <> "" Then query = query.Where(Function(x) x.p_Type = c.Type)

        Return query
    End Function

    Public Function SetListViewModel(query As IQueryable(Of payment)) As List(Of PaymentVM) Implements IPresenter(Of payment, PaymentVM).SetListViewModel
        Dim payment = query.ToList
        Return payment.Select(Function(x) New PaymentVM With {
                .付款類型 = x.p_Type,
                .備註 = x.p_Memo,
                .廠商名稱 = x.manufacturer.manu_name,
                .支票號碼 = x.p_Cheque,
                .日期 = x.p_Date,
                .科目 = x.subject.s_name,
                .編號 = x.p_Id,
                .金額 = x.p_Amount,
                .銀行名稱 = If(x.bank IsNot Nothing, x.bank.bank_name, ""),
                .帳款月份 = If(x.p_AccountMonth.HasValue, x.p_AccountMonth.Value.ToString("yyyy年MM月"), Nothing)
            }).ToList()
    End Function

    ''' <summary>
    ''' 設定廠商下拉選單
    ''' </summary>
    Public Sub SetManuCmb()
        _view.SetManuCmb(_manuService.GetVendorCmbItems())
    End Sub

    ''' <summary>
    ''' 設定銀行下拉選單
    ''' </summary>
    Public Sub SetBankCmb()
        _view.SetBankCmb(_bankService.GetBankCmbItems)
    End Sub

    ''' <summary>
    ''' 設定公司下拉選單
    ''' </summary>
    Public Sub SetCompanyCmb()
        _view.SetCompanyCmb(_companyService.GetCompanyComboBoxData)
    End Sub

    ''' <summary>
    ''' 科目下拉選單
    ''' </summary>
    Public Sub SetSubjectsCmb()
        _view.SetSubjectsCmb(_subjectsService.GetSubjectsCmbItems("貸"))
    End Sub

    ''' <summary>
    ''' 查詢該廠商應付未付款
    ''' </summary>
    ''' <param name="manufacturerId"></param>
    Public Sub GetAmountDue(manufacturerId As Integer)
        Try
            Using db As New gas_accounting_systemEntities
                ' 取得該製造商的購買和付款數據
                Dim purchaseData = (
                From pur In db.purchases.AsNoTracking().AsEnumerable
                Where pur.pur_manu_id = manufacturerId
                Group By Year = pur.pur_date.Value.Year, Month = pur.pur_date.Value.Month Into GroupTotal = Sum(pur.pur_price)
                Select New With {Year, Month, .TotalPurchase = GroupTotal}
                ).ToList()

                Dim paymentData = (
                From pay In db.payments.AsNoTracking().AsEnumerable
                Where pay.p_m_Id = manufacturerId
                Group By Year = pay.p_AccountMonth.Value.Year, Month = pay.p_AccountMonth.Value.Month Into GroupTotal = Sum(pay.p_Amount)
                Select New With {Year, Month, .TotalPayment = GroupTotal}
                ).ToList()

                ' 未付帳款列表
                Dim amountDueList As New List(Of AmountDueVM)

                ' 計算每個月的未付帳款
                For Each pur In purchaseData
                    Dim correspondingPayment = paymentData.
                    Where(Function(p) p.Year = pur.Year AndAlso p.Month = pur.Month).
                    Select(Function(p) p.TotalPayment).
                    DefaultIfEmpty(0).
                    FirstOrDefault()
                    Dim unpaidAmount = pur.TotalPurchase - correspondingPayment
                    If unpaidAmount > 0 Then
                        amountDueList.Add(New AmountDueVM With {
                        .廠商 = (From manu In db.manufacturers Where manu.manu_id = manufacturerId Select manu.manu_name).FirstOrDefault(),
                        .年月份 = New DateTime(pur.Year, pur.Month, 1).ToString("yyyy/MM"),
                        .未付帳款 = unpaidAmount
                    })
                    End If
                Next

                ' 將結果集傳遞給視圖
                _view.SetAmountDueDGV(amountDueList)
            End Using
        Catch ex As Exception
            If ex.InnerException IsNot Nothing Then
                MsgBox("發生內部錯誤:" & ex.InnerException.Message)
            Else
                MsgBox("發生錯誤: " & ex.Message)
            End If
        End Try
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
                    data.p_SubpoenaNo = _journalService.GetSubpoenaNo
                    db.payments.Add(data)

                    If data.p_Type = "支票" Then
                        db.cheques.Add(_view.GetChequeDatas)
                    End If

                    Dim journal = New journal With {
                        .j_Amount = data.p_Amount,
                        .j_Memo = data.p_Memo,
                        .j_SubpoenaNo = data.p_SubpoenaNo,
                        .j_s_Id = data.p_s_Id
                    }
                    _journalService.Add(journal)

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
                    Dim payment = db.payments.Find(id)
                    data.p_SubpoenaNo = payment.p_SubpoenaNo

                    If payment IsNot Nothing Then
                        If data.p_Type = "支票" Then
                            Dim cheques = db.cheques.FirstOrDefault(Function(x) x.che_Number = payment.p_Cheque)

                            If cheques IsNot Nothing Then
                                Dim updateCheques = _view.GetChequeDatas
                                updateCheques.che_Id = cheques.che_Id
                                db.Entry(cheques).CurrentValues.SetValues(updateCheques)
                            End If
                        End If

#Region "修改傳票"
                        Dim journal = New journal With {
                            .j_Amount = data.p_Amount,
                            .j_Memo = data.p_Memo,
                            .j_s_Id = data.p_s_Id,
                            .j_SubpoenaNo = data.p_SubpoenaNo
                        }

                        _journalService.Edit(journal)
#End Region

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

                    '刪除傳票
                    _journalService.Delete(data.p_SubpoenaNo)

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
