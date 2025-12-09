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

    Private Sub LoadBankDropdown(sender As Object, companyId As Integer)
        Try
            Using uow = _uowFactory.Create()
                Dim data = uow.BankRepository.GetBankDropdownAsync(companyId).Result
                _view.PopulateBankDropdown(data)
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

                'Dim entries = CreatePaymentEntries(data)
                '_aeSer.AddEntries(uow.AccountingEntryRepository, entries)

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

                            Await uow.PaymentChequeRepository.AddAsync(New payment_cheque With {
                                .pc_p_id = orgData.p_Id,
                                .pc_cp_id = cheque.cp_Id
                            })
                        Next
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
                LoadBankDropdown(sender, selectPayment.p_comp_Id)
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
                        _reportSer.GeneratorTransferSubpoena(data.Item1, uow.PaymentRepository.GetTransferSubpoenaData(data.Item1), False)
                    Case Else
                        Throw New Exception("類型錯誤")
                End Select
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    'Private Function CreatePaymentEntries(payment As payment) As List(Of accounting_entry)
    '    Dim entries = New List(Of accounting_entry)

    '    Select Case payment.p_Type
    '        Case "現金"
    '            entries.Add(New accounting_entry With {
    '                .ae_TransactionId = payment.p_Id,
    '                .ae_Date = payment.p_Date,
    '                .ae_TransactionType = "付款作業",
    '                .ae_s_Id = payment.p_s_Id,
    '                .ae_Debit = payment.p_Amount,
    '                .ae_Credit = 0
    '            })

    '            entries.Add(New accounting_entry With {
    '                .ae_TransactionId = payment.p_Id,
    '                .ae_Date = payment.p_Date,
    '                .ae_TransactionType = "付款作業",
    '                .ae_s_Id = 5,
    '                .ae_Debit = 0,
    '                .ae_Credit = payment.p_Amount
    '            })

    '        Case "銀行存款"
    '            entries.Add(New accounting_entry With {
    '                .ae_TransactionId = payment.p_Id,
    '                .ae_Date = payment.p_Date,
    '                .ae_TransactionType = "付款作業",
    '                .ae_s_Id = payment.p_s_Id,
    '                .ae_Debit = payment.p_Amount,
    '                .ae_Credit = 0
    '            })

    '            entries.Add(New accounting_entry With {
    '                .ae_TransactionId = payment.p_Id,
    '                .ae_Date = payment.p_Date,
    '                .ae_TransactionType = "付款作業",
    '                .ae_s_Id = 6,
    '                .ae_Debit = 0,
    '                .ae_Credit = payment.p_Amount
    '            })

    '        Case "應付票據", "應收票據"
    '            entries.Add(New accounting_entry With {
    '                .ae_TransactionId = payment.p_Id,
    '                .ae_Date = payment.p_Date,
    '                .ae_TransactionType = "付款作業",
    '                .ae_s_Id = payment.p_s_Id,
    '                .ae_Debit = payment.p_Amount,
    '                .ae_Credit = 0
    '            })

    '            entries.Add(New accounting_entry With {
    '                .ae_TransactionId = payment.p_Id,
    '                .ae_Date = payment.p_Date,
    '                .ae_TransactionType = "付款作業",
    '                .ae_s_Id = 7,
    '                .ae_Debit = 0,
    '                .ae_Credit = payment.p_Amount
    '            })
    '    End Select

    '    Return entries
    'End Function

    Private Sub Search()
        Try
            Dim criteria = _view.GetSearchCriteria
            LoadList(criteria)
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class