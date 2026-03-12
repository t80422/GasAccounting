Imports System.ComponentModel

Public Class PaymentPresenter
    Private ReadOnly _view As IPaymentView
    Private ReadOnly _bmbService As IBankMonthlyBalanceService
    Private ReadOnly _aeSer As IAccountingEntryService
    Private ReadOnly _reportSer As IReportService
    Private ReadOnly _uowFactory As IUnitOfWorkFactory
    Private selectPayment As payment

    Public ReadOnly Property View As IPaymentView
        Get
            Return _view
        End Get
    End Property

    Public Sub New(view As IPaymentView,
                   bmbService As IBankMonthlyBalanceService,
                   aeSer As IAccountingEntryService,
                   reportSer As IReportService,
                   uowFactory As IUnitOfWorkFactory)
        _view = view
        _bmbService = bmbService
        _aeSer = aeSer
        _reportSer = reportSer
        _uowFactory = uowFactory

        SubscribeToViewEvents()
    End Sub

    ''' <summary>
    ''' 訂閱 View 的事件
    ''' </summary>
    Private Sub SubscribeToViewEvents()
        AddHandler _view.CreateRequest, AddressOf Add
        AddHandler _view.UpdateRequest, AddressOf Update
        AddHandler _view.DeleteRequest, AddressOf Delete
        AddHandler _view.CancelRequest, AddressOf Initialize
        AddHandler _view.DataSelectedRequest, AddressOf LoadPaymentDetail
        AddHandler _view.PrintRequested, AddressOf Print
        AddHandler _view.ManufacturerSelected, AddressOf ManufacturerSelected
        AddHandler _view.CompanySelected, AddressOf LoadBankDropdown
        AddHandler _view.SearchRequest, AddressOf Search
    End Sub

    Private Sub Initialize()
        Try
            _view.ClearInput()

            LoadVendorDropdown()
            LoadSubjectDropdown()
            LoadCompanyDropdown()

            LoadList()
            selectPayment = Nothing

            _view.ButtonStatus(False)
        Catch ex As Exception
            MessageBox.Show("付款作業初始化發生錯誤:" + ex.Message)
        End Try
    End Sub

    Private Sub LoadList(Optional criteria As PaymentSearchCriteria = Nothing)
        Try
            Using uow = _uowFactory.Create()
                Dim payments = uow.PaymentRepository.SearchPaymentAsync(criteria).Result
                _view.ShowList(payments.Select(Function(x) New PaymentListVM(x)).ToList)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub LoadVendorDropdown()
        Try
            Using uow = _uowFactory.Create()
                Dim vendors = uow.ManufacturerRepository.GetVendorDropdownAsync.Result
                _view.PopulateVendorDropdown(vendors)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadSubjectDropdown()
        Try
            Using uow = _uowFactory.Create()
                Dim subjects = uow.SubjectRepository.GetSubjectDropdownAsync.Result
                _view.PopulateSubjectDropdown(subjects)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadCompanyDropdown()
        Try
            Using uow = _uowFactory.Create()
                Dim companies = uow.CompanyRepository.GetCompanyDropdownAsync.Result
                _view.PopulateCompanyDropdown(companies)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadBankDropdown(sender As Object, company As Tuple(Of Integer, Integer))
        Try
            Dim compId = company.Item1
            Dim cmbCompNo = company.Item2

            Using uow = _uowFactory.Create()
                Dim data = uow.BankRepository.GetBankDropdownAsync(compId).Result
                _view.PopulateBankDropdown(data, cmbCompNo)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub ManufacturerSelected(sender As Object, vendorId As Integer)
        Try
            Using uow = _uowFactory.Create()
                Dim vendor = uow.ManufacturerRepository.GetByIdAsync(vendorId).Result
                _view.ShowVendorAccount(vendor.manu_account)
                LoadVendorAmountDue(vendorId)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub LoadVendorAmountDue(vendorId As Integer)
        Try
            Using uow = _uowFactory.Create()
                Dim amountDue = uow.PaymentRepository.GetVendorAmountDue(vendorId)
                _view.DisplayAmountDueList(amountDue)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Async Sub Add()
        Using uow = _uowFactory.Create()
            uow.BeginTransaction()
            Try
                Dim data As New payment
                _view.GetInput(data)
                Validate(data)
                Await uow.PaymentRepository.AddAsync(data)
                Await uow.SaveChangesAsync()

                If data.p_Type = "應付票據" Then
                    Dim cheque As New chque_pay
                    _view.GetChequeInput(cheque)
                    cheque.cp_Date = data.p_Date
                    cheque.cp_Amount = data.p_Amount

                    Await uow.ChequePayRepository.AddAsync(cheque)
                    data.chque_pay = cheque
                ElseIf data.p_Type = "銀行存款" Then
                    ' 支票兌現
                    Dim subject = uow.SubjectRepository.GetByIdAsync(data.p_s_Id).Result

                    If subject.s_name = "應付票據" Then
                        Dim chequeNums = _view.GetChequeNumbers?.
                            Select(Function(x) x.支票編號).
                            Where(Function(n) Not String.IsNullOrWhiteSpace(n)).
                            ToList()
                        If chequeNums Is Nothing Then chequeNums = New List(Of String)

                        If chequeNums.Count <> 0 Then
                            Dim pcList As New List(Of payment_cheque)

                            For Each chequeNum In chequeNums
                                Dim cheque = uow.ChequePayRepository.GetByChequeNumber(chequeNum)

                                If cheque Is Nothing Then
                                    Throw New Exception($"{chequeNum}找不到此支票")
                                ElseIf cheque.cp_IsCashing Then
                                    Throw New Exception($"{chequeNum}此支票已兌現")
                                Else
                                    cheque.cp_IsCashing = True
                                    cheque.cp_BankCashing = data.p_Date
                                End If

                                pcList.Add(New payment_cheque With {
                                    .pc_p_id = data.p_Id,
                                    .pc_cp_id = cheque.cp_Id
                                })
                            Next

                            uow.PaymentChequeRepository.AddBatch(pcList)
                        End If
                    End If

                    ' ✨ 使用增量更新：新增一筆支出
                    Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                        uow.BankMonthlyBalancesRepository,
                        uow.BankRepository,
                        data.p_bank_Id,
                        data.p_Date,
                        creditDelta:=data.p_Amount,
                        debitDelta:=0
                    )
                End If

                ' 處理借方(存入)銀行科目的餘額更新 (資金流入銀行 -> Debit 增加)
                ' 第一組
                Await ProcessDebitBankSubjectAsync(uow, Nothing, data, Nothing, data.p_s_Id, Nothing, data.p_bank_Id, Nothing, data.p_debit_amount_1, DateTime.MinValue, data.p_Date)
                ' 第二組
                Await ProcessDebitBankSubjectAsync(uow, Nothing, data, Nothing, data.p_debit_s_id_2, Nothing, data.p_debit_bank_id_2, Nothing, data.p_debit_amount_2, DateTime.MinValue, data.p_Date)
                ' 第三組
                Await ProcessDebitBankSubjectAsync(uow, Nothing, data, Nothing, data.p_debit_s_id_3, Nothing, data.p_debit_bank_id_3, Nothing, data.p_debit_amount_3, DateTime.MinValue, data.p_Date)

                Await uow.SaveChangesAsync()
                uow.Commit()

                Initialize()
                MessageBox.Show("新增成功")
            Catch ex As Exception
                uow.Rollback()
                MessageBox.Show(ex.Message)
            End Try
        End Using
    End Sub

    Private Async Sub Update()
        If selectPayment Is Nothing Then
            MessageBox.Show("請先選擇要更新的資料")
            Return
        End If

        Using uow = _uowFactory.Create()
            uow.BeginTransaction()
            Try
                Dim orgData = uow.PaymentRepository.GetByIdAsync(selectPayment.p_Id).Result
                Dim data As New payment
                _view.GetInput(data)
                Validate(data)

                ' 儲存舊資料用於月結更新
                Dim oldBankId = orgData.p_bank_Id
                Dim oldDate = orgData.p_Date
                Dim oldAmount = orgData.p_Amount
                Dim oldType = orgData.p_Type

                ' 支票處理（付款型態 = 應付票據）
                If data.p_Type = "應付票據" Then
                    Dim chequePay = orgData.chque_pay

                    If chequePay IsNot Nothing Then
                        _view.GetChequeInput(chequePay)
                        chequePay.cp_Date = data.p_Date
                        chequePay.cp_Amount = data.p_Amount
                        data.p_cp_Id = chequePay.cp_Id
                    Else
                        '如果支票不存在，則新增一個新的支票記錄
                        Dim chequeInput As New chque_pay
                        _view.GetChequeInput(chequeInput)
                        chequeInput.cp_Date = data.p_Date
                        chequeInput.cp_Amount = data.p_Amount

                        Await uow.ChequePayRepository.AddAsync(chequeInput)
                        data.p_cp_Id = chequeInput.cp_Id
                    End If

                ElseIf oldType = "應付票據" AndAlso data.p_Type <> "應付票據" Then
                    ' 從支票改為其他付款型態時清理舊支票紀錄
                    If orgData.p_cp_Id.HasValue Then
                        Dim oldCheque = Await uow.ChequePayRepository.GetByIdAsync(orgData.p_cp_Id.Value)
                        If oldCheque IsNot Nothing Then Await uow.ChequePayRepository.DeleteAsync(oldCheque)
                        orgData.p_cp_Id = Nothing
                    End If
                End If

                ' 銀行存款且科目為應付票據：處理兌現支票關聯
                If data.p_Type = "銀行存款" Then
                    Dim subject = uow.SubjectRepository.GetByIdAsync(data.p_s_Id).Result
                    If subject IsNot Nothing AndAlso subject.s_name = "應付票據" Then
                        Dim newChequeNums = _view.GetChequeNumbers?.
                            Select(Function(x) x.支票編號).
                            Where(Function(n) Not String.IsNullOrWhiteSpace(n)).
                            ToList()
                        If newChequeNums Is Nothing Then newChequeNums = New List(Of String)
                        Dim oldPcList = uow.PaymentChequeRepository.GetByPaymentId(orgData.p_Id)

                        ' 需移除的連結（舊有但這次未選的）
                        Dim removeList = oldPcList.Where(Function(pc) Not newChequeNums.Contains(pc.chque_pay?.cp_Number)).ToList()
                        For Each pc In removeList
                            If pc.chque_pay IsNot Nothing Then
                                pc.chque_pay.cp_IsCashing = False
                                pc.chque_pay.cp_BankCashing = Nothing
                            End If
                            Await uow.PaymentChequeRepository.DeleteAsync(pc)
                        Next

                        ' 需新增的連結（新選的號碼）
                        Dim existingNums = oldPcList.Select(Function(pc) pc.chque_pay?.cp_Number).Where(Function(n) n IsNot Nothing).ToList()
                        Dim addNums = newChequeNums.Where(Function(n) Not existingNums.Contains(n)).ToList()

                        If addNums.Count > 0 Then
                            Dim pcList As New List(Of payment_cheque)

                            For Each chequeNum In addNums
                                Dim cheque = uow.ChequePayRepository.GetByChequeNumber(chequeNum)
                                If cheque Is Nothing Then
                                    Throw New Exception($"{chequeNum}找不到此支票")
                                ElseIf cheque.cp_IsCashing Then
                                    Throw New Exception($"{chequeNum}此支票已兌現")
                                Else
                                    cheque.cp_IsCashing = True
                                    cheque.cp_BankCashing = data.p_Date
                                End If

                                pcList.Add(New payment_cheque With {
                                    .pc_p_id = orgData.p_Id,
                                    .pc_cp_id = cheque.cp_Id
                                })
                            Next

                            ' 批次新增所有 payment_cheque
                            uow.PaymentChequeRepository.AddBatch(pcList)
                        End If
                        ' 批次完成後一次儲存，避免只更新部分支票
                        Await uow.SaveChangesAsync()
                    ElseIf subject IsNot Nothing AndAlso subject.s_name <> "應付票據" Then
                        ' 科目改為非應付票據時，清理舊的 payment_cheque 關聯
                        Dim oldPcList = uow.PaymentChequeRepository.GetByPaymentId(orgData.p_Id)
                        For Each pc In oldPcList
                            If pc.chque_pay IsNot Nothing Then
                                pc.chque_pay.cp_IsCashing = False
                                pc.chque_pay.cp_BankCashing = Nothing
                            End If
                            Await uow.PaymentChequeRepository.DeleteAsync(pc)
                        Next
                    End If
                ElseIf oldType = "銀行存款" AndAlso data.p_Type <> "銀行存款" Then
                    ' 由銀行存款改為其他：若原科目為應付票據，需清除 payment_cheque 關聯並復原兌現狀態
                    Dim subject = uow.SubjectRepository.GetByIdAsync(orgData.p_s_Id).Result
                    If subject IsNot Nothing AndAlso subject.s_name = "應付票據" Then
                        Dim oldPcList = uow.PaymentChequeRepository.GetByPaymentId(orgData.p_Id)
                        For Each pc In oldPcList
                            If pc.chque_pay IsNot Nothing Then
                                pc.chque_pay.cp_IsCashing = False
                                pc.chque_pay.cp_BankCashing = Nothing
                            End If
                            Await uow.PaymentChequeRepository.DeleteAsync(pc)
                        Next
                    End If
                End If

                ' ✨ 智能判斷需要更新的月結餘額
                If oldType = "銀行存款" AndAlso data.p_Type = "銀行存款" Then
                    ' 情境 1: 都是銀行存款
                    If oldBankId = data.p_bank_Id Then
                        ' 1.1 同銀行
                        If oldDate.Year = data.p_Date.Year AndAlso oldDate.Month = data.p_Date.Month Then
                            ' 同月份 - 只調整差額
                            Dim amountDelta = data.p_Amount - oldAmount
                            If amountDelta <> 0 Then
                                Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                    uow.BankMonthlyBalancesRepository,
                                    uow.BankRepository,
                                    data.p_bank_Id,
                                    data.p_Date,
                                    creditDelta:=amountDelta,
                                    debitDelta:=0
                                )
                            End If
                        Else
                            ' 不同月份 - 舊月份減少，新月份增加
                            Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                uow.BankMonthlyBalancesRepository,
                                uow.BankRepository,
                                oldBankId,
                                oldDate,
                                creditDelta:=-oldAmount,
                                debitDelta:=0
                            )
                            Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                uow.BankMonthlyBalancesRepository,
                                uow.BankRepository,
                                data.p_bank_Id,
                                data.p_Date,
                                creditDelta:=data.p_Amount,
                                debitDelta:=0
                            )
                        End If
                    Else
                        ' 1.2 不同銀行 - 舊銀行減少，新銀行增加
                        Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                            uow.BankMonthlyBalancesRepository,
                            uow.BankRepository,
                            oldBankId,
                            oldDate,
                            creditDelta:=-oldAmount,
                            debitDelta:=0
                        )
                        Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                            uow.BankMonthlyBalancesRepository,
                            uow.BankRepository,
                            data.p_bank_Id,
                            data.p_Date,
                            creditDelta:=data.p_Amount,
                            debitDelta:=0
                        )
                    End If
                ElseIf oldType = "銀行存款" AndAlso data.p_Type <> "銀行存款" Then
                    ' 情境 2: 從銀行存款改為其他 - 舊銀行減少支出
                    Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                        uow.BankMonthlyBalancesRepository,
                        uow.BankRepository,
                        oldBankId,
                        oldDate,
                        creditDelta:=-oldAmount,
                        debitDelta:=0
                    )
                ElseIf oldType <> "銀行存款" AndAlso data.p_Type = "銀行存款" Then
                    ' 情境 3: 從其他改為銀行存款 - 新銀行增加支出
                    Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                        uow.BankMonthlyBalancesRepository,
                        uow.BankRepository,
                        data.p_bank_Id,
                        data.p_Date,
                        creditDelta:=data.p_Amount,
                        debitDelta:=0
                    )
                End If

                ' 處理借方科目(資金流入銀行)的變更
                ' 第一組
                Await ProcessDebitBankSubjectAsync(uow, orgData, data, orgData.p_s_Id, data.p_s_Id, orgData.p_bank_Id, data.p_bank_Id, orgData.p_debit_amount_1, data.p_debit_amount_1, oldDate, data.p_Date)
                ' 第二組
                Await ProcessDebitBankSubjectAsync(uow, orgData, data, orgData.p_debit_s_id_2, data.p_debit_s_id_2, orgData.p_debit_bank_id_2, data.p_debit_bank_id_2, orgData.p_debit_amount_2, data.p_debit_amount_2, oldDate, data.p_Date)
                ' 第三組
                Await ProcessDebitBankSubjectAsync(uow, orgData, data, orgData.p_debit_s_id_3, data.p_debit_s_id_3, orgData.p_debit_bank_id_3, data.p_debit_bank_id_3, orgData.p_debit_amount_3, data.p_debit_amount_3, oldDate, data.p_Date)

                ' 更新付款主檔
                Await uow.PaymentRepository.UpdateAsync(orgData, data)
                Await uow.SaveChangesAsync()
                uow.Commit()

                Initialize()
                MsgBox("更新成功")
            Catch ex As Exception
                uow.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Private Async Sub Delete()
        Using uow = _uowFactory.Create()
            uow.BeginTransaction()
            Try
                Dim payType = selectPayment.p_Type
                Dim bankId = selectPayment.p_bank_Id
                Dim payDate = selectPayment.p_Date
                Dim payAmount = selectPayment.p_Amount

                ' 刪除支票或還原兌現狀態
                If selectPayment.p_Type = "應付票據" Then
                    Dim chequePay = uow.ChequePayRepository.GetByIdAsync(selectPayment.p_cp_Id).Result
                    If chequePay IsNot Nothing Then
                        Await uow.ChequePayRepository.DeleteAsync(chequePay)
                    End If
                ElseIf selectPayment.p_Type = "銀行存款" Then
                    Dim subject = uow.SubjectRepository.GetByIdAsync(selectPayment.p_s_Id).Result
                    If subject IsNot Nothing AndAlso subject.s_name = "應付票據" Then
                        Dim pcList = uow.PaymentChequeRepository.GetByPaymentId(selectPayment.p_Id)
                        For Each pc In pcList
                            If pc.chque_pay IsNot Nothing Then
                                pc.chque_pay.cp_IsCashing = False
                                pc.chque_pay.cp_BankCashing = Nothing
                            End If
                            Await uow.PaymentChequeRepository.DeleteAsync(pc)
                        Next
                    End If
                End If

                '刪除付款
                Await uow.PaymentRepository.DeleteAsync(selectPayment.p_Id)

                ' ✨ 使用增量更新：減少一筆支出（負數）
                If payType = "銀行存款" Then
                    Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                        uow.BankMonthlyBalancesRepository,
                        uow.BankRepository,
                        bankId,
                        payDate,
                        creditDelta:=-payAmount,
                        debitDelta:=0
                    )
                End If

                ' 處理借方(存入)銀行科目的餘額更新 (資金流入銀行 -> Debit 減少，因為是刪除)
                ' 第一組
                Await ProcessDebitBankSubjectAsync(uow, selectPayment, Nothing, selectPayment.p_s_Id, Nothing, selectPayment.p_bank_Id, Nothing, selectPayment.p_debit_amount_1, Nothing, payDate, DateTime.MinValue)
                ' 第二組
                Await ProcessDebitBankSubjectAsync(uow, selectPayment, Nothing, selectPayment.p_debit_s_id_2, Nothing, selectPayment.p_debit_bank_id_2, Nothing, selectPayment.p_debit_amount_2, Nothing, payDate, DateTime.MinValue)
                ' 第三組
                Await ProcessDebitBankSubjectAsync(uow, selectPayment, Nothing, selectPayment.p_debit_s_id_3, Nothing, selectPayment.p_debit_bank_id_3, Nothing, selectPayment.p_debit_amount_3, Nothing, payDate, DateTime.MinValue)

                _aeSer.DeleteEntries(uow.AccountingEntryRepository, "付款作業", selectPayment.p_Id)

                Await uow.SaveChangesAsync()
                uow.Commit()

                Initialize()
                MessageBox.Show("刪除成功")
            Catch ex As Exception
                uow.Rollback()
                MessageBox.Show(ex.Message)
            End Try
        End Using
    End Sub

    Private Sub LoadPaymentDetail(sender As Object, id As Integer)
        Try
            Using uow = _uowFactory.Create()
                _view.ClearInput()
                selectPayment = uow.PaymentRepository.GetByIdAsync(id).Result
                uow.PaymentRepository.Reload(selectPayment)

                If selectPayment.p_comp_Id.HasValue Then
                    Dim compData = New Tuple(Of Integer, Integer)(selectPayment.p_comp_Id.Value, 1)
                    LoadBankDropdown(sender, compData)
                End If

                If selectPayment.p_debit_comp_id_2.HasValue Then
                    Dim compData = New Tuple(Of Integer, Integer)(selectPayment.p_debit_comp_id_2.Value, 2)
                    LoadBankDropdown(sender, compData)
                End If

                If selectPayment.p_debit_comp_id_3.HasValue Then
                    Dim compData = New Tuple(Of Integer, Integer)(selectPayment.p_debit_comp_id_3.Value, 3)
                    LoadBankDropdown(sender, compData)
                End If

                _view.ShowDetail(selectPayment)
                If selectPayment.p_m_Id.HasValue Then ManufacturerSelected(sender, selectPayment.p_m_Id)
                _view.ButtonStatus(True)
                Dim chequeListVM = uow.PaymentChequeRepository.GetByPaymentId(selectPayment.p_Id).
                                                               Select(Function(x) New SelectChequeVM(x)).ToList()
                Dim bindingList = New BindingList(Of SelectChequeVM)(chequeListVM)
                _view.ShowChequeList(bindingList)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Print(sender As Object, data As Tuple(Of Date, String))
        Try
            Using uow = _uowFactory.Create()
                Select Case data.Item2
                    Case "現金"
                        _reportSer.GeneratorCashSubpoena(data.Item1, uow.PaymentRepository.GetCashSubpoenaData(data.Item1), False)
                    Case "轉帳"
                        _reportSer.GeneratorTransferSubpoena(New TransferSubpoenaReportRequest With {
                            .Day = data.Item1,
                            .Groups = uow.PaymentRepository.GetTransferSubpoenaData(data.Item1),
                            .VoucherType = TransferVoucherType.Expense
                        })
                    Case Else
                        Throw New Exception("類型錯誤")
                End Select
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Search()
        Try
            Dim criteria = _view.GetSearchCriteria
            LoadList(criteria)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' 處理單組借方(Debit/資金去向)科目為銀行存款時的月結調整
    ''' </summary>
    Private Async Function ProcessDebitBankSubjectAsync(
        uow As IUnitOfWork,
        orgPayment As payment,
        newPayment As payment,
        oldSubjectId As Integer?,
        newSubjectId As Integer?,
        oldBankId As Integer?,
        newBankId As Integer?,
        oldAmount As Integer?,
        newAmount As Integer?,
        oldMonth As Date,
        newMonth As Date) As Task

        ' 如果新舊科目都沒有值，直接返回
        If Not oldSubjectId.HasValue AndAlso Not newSubjectId.HasValue Then
            Return
        End If

        ' 檢查舊科目和新科目是否為銀行存款
        Dim oldSubject = If(oldSubjectId.HasValue,
                            Await uow.SubjectRepository.GetByIdAsync(oldSubjectId.Value),
                            Nothing)
        Dim newSubject = If(newSubjectId.HasValue,
                            Await uow.SubjectRepository.GetByIdAsync(newSubjectId.Value),
                            Nothing)
        Dim oldIsBankSubject = oldSubject IsNot Nothing AndAlso oldSubject.s_name = "銀行存款"
        Dim newIsBankSubject = newSubject IsNot Nothing AndAlso newSubject.s_name = "銀行存款"

        ' 如果新舊都不是銀行存款，不需要調整
        If Not oldIsBankSubject AndAlso Not newIsBankSubject Then
            Return
        End If

        ' 調用銀行月結調整方法 (借方)
        Await AdjustBankMonthlyBalanceForDebitSubjectAsync(
            uow,
            oldIsBankSubject,
            newIsBankSubject,
            oldBankId.GetValueOrDefault(),
            oldMonth,
            oldAmount.GetValueOrDefault(),
            newBankId.GetValueOrDefault(),
            newMonth,
            newAmount.GetValueOrDefault()
        )
    End Function

    ''' <summary>
    ''' 針對借方科目(Debit/存款)的銀行餘額調整
    ''' </summary>
    Private Async Function AdjustBankMonthlyBalanceForDebitSubjectAsync(uow As IUnitOfWork,
                                                                       oldIsBankSubject As Boolean,
                                                                       newIsBankSubject As Boolean,
                                                                       oldBankId As Integer,
                                                                       oldMonth As Date,
                                                                       oldAmount As Decimal,
                                                                       newBankId As Integer,
                                                                       newMonth As Date,
                                                                       newAmount As Decimal) As Task
        ' 注意：此處處理的是「借方」，即資金流入銀行。
        ' 增加借方金額 = 增加 Debit (餘額增加)
        ' 減少借方金額 = 減少 Debit (餘額減少)

        If Not oldIsBankSubject AndAlso Not newIsBankSubject Then Exit Function

        If oldIsBankSubject AndAlso newIsBankSubject Then
            If oldBankId = newBankId AndAlso oldMonth.Year = newMonth.Year AndAlso oldMonth.Month = newMonth.Month Then
                ' 同銀行同月份：調整差額
                Dim amountDelta = newAmount - oldAmount
                If amountDelta <> 0 Then
                    Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                        uow.BankMonthlyBalancesRepository,
                        uow.BankRepository,
                        newBankId,
                        newMonth,
                        creditDelta:=0,
                        debitDelta:=amountDelta
                    )
                End If
            Else
                ' 不同銀行或月份：
                ' 1. 舊的減少 Debit (減少餘額) -> debitDelta 用負數
                Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                    uow.BankMonthlyBalancesRepository,
                    uow.BankRepository,
                    oldBankId,
                    oldMonth,
                    creditDelta:=0,
                    debitDelta:=-oldAmount
                )
                ' 2. 新的增加 Debit (增加餘額) -> debitDelta 用正數
                Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                    uow.BankMonthlyBalancesRepository,
                    uow.BankRepository,
                    newBankId,
                    newMonth,
                    creditDelta:=0,
                    debitDelta:=newAmount
                )
            End If
        ElseIf oldIsBankSubject Then
            ' 舊的是銀行（現在不是）：減少 Debit (減少餘額)
            Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                uow.BankMonthlyBalancesRepository,
                uow.BankRepository,
                oldBankId,
                oldMonth,
                creditDelta:=0,
                debitDelta:=-oldAmount
            )
        ElseIf newIsBankSubject Then
            ' 新的是銀行（以前不是）：增加 Debit (增加餘額)
            Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                uow.BankMonthlyBalancesRepository,
                uow.BankRepository,
                newBankId,
                newMonth,
                creditDelta:=0,
                debitDelta:=newAmount
            )
        End If
    End Function

    Private Sub Validate(data As payment)
        ' 驗證是否借貸平衡
        Dim debitTotal = data.p_debit_amount_1.GetValueOrDefault +
                         data.p_debit_amount_2.GetValueOrDefault +
                         data.p_debit_amount_3.GetValueOrDefault

        If debitTotal <> data.p_Amount Then Throw New Exception($"借方金額總和 ({debitTotal}) 必須等於貸方金額 ({data.p_Amount})")
    End Sub
End Class