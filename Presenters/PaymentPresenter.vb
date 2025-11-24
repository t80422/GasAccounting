Public Class PaymentPresenter
    Private ReadOnly _view As IPaymentView
    Private ReadOnly _bmbService As IBankMonthlyBalanceService
    Private ReadOnly _aeSer As IAccountingEntryService
    Private ReadOnly _reportSer As IReportService
    Private selectPayment As payment

    Public ReadOnly Property View As IPaymentView
        Get
            Return _view
        End Get
    End Property

    Public Sub New(view As IPaymentView, bmbService As IBankMonthlyBalanceService, aeSer As IAccountingEntryService, reportSer As IReportService)
        _view = view
        _bmbService = bmbService
        _aeSer = aeSer
        _reportSer = reportSer

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
            MsgBox("付款作業初始化發生錯誤:" + ex.Message)
        End Try
    End Sub

    Private Sub LoadList(Optional criteria As PaymentSearchCriteria = Nothing)
        Try
            Using uow As New UnitOfWork()
                Dim payments = uow.PaymentRepository.SearchPaymentAsync(criteria).Result
                _view.ShowList(payments.Select(Function(x) New PaymentListVM(x)).ToList)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub LoadVendorDropdown()
        Try
            Using uow As New UnitOfWork()
                Dim vendors = uow.ManufacturerRepository.GetVendorDropdownAsync.Result
                _view.PopulateVendorDropdown(vendors)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadSubjectDropdown()
        Try
            Using uow As New UnitOfWork()
                Dim subjects = uow.SubjectRepository.GetSubjectDropdownAsync.Result
                _view.PopulateSubjectDropdown(subjects)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadCompanyDropdown()
        Try
            Using uow As New UnitOfWork()
                Dim companies = uow.CompanyRepository.GetCompanyDropdownAsync.Result
                _view.PopulateCompanyDropdown(companies)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadBankDropdown(sender As Object, companyId As Integer)
        Try
            Using uow As New UnitOfWork()
                Dim data = uow.BankRepository.GetBankDropdownAsync(companyId).Result
                _view.PopulateBankDropdown(data)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub ManufacturerSelected(sender As Object, vendorId As Integer)
        Try
            Using uow As New UnitOfWork()
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
            Using uow As New UnitOfWork()
                Dim amountDue = uow.PaymentRepository.GetVendorAmountDue(vendorId)
                _view.DisplayAmountDueList(amountDue)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Add()
        Using uow As New UnitOfWork()
            uow.BeginTransaction()
            Try
                Dim data As New payment
                _view.GetInput(data)

                If data.p_Type = "應付票據" Then
                    Dim cheque As New chque_pay
                    _view.GetChequeInput(cheque)
                    cheque.cp_Date = data.p_Date
                    cheque.cp_Amount = data.p_Amount

                    uow.ChequePayRepository.AddAsync(cheque)
                    data.chque_pay = cheque
                ElseIf data.p_Type = "銀行存款" Then
                    ' 支票兌現
                    Dim subject = uow.SubjectRepository.GetByIdAsync(data.p_s_Id).Result

                    If subject.s_name = "應付票據" Then
                        Dim chequeNo = InputBox("請輸入支票號碼")

                        If Not String.IsNullOrEmpty(chequeNo) Then
                            Dim cheque = uow.ChequePayRepository.GetByChequeNumber(chequeNo)

                            If cheque Is Nothing Then
                                Throw New Exception("找不到此支票")
                            ElseIf cheque.cp_IsCashing Then
                                Throw New Exception("此支票已兌現")
                            Else
                                cheque.cp_IsCashing = True
                                cheque.cp_BankCashing = data.p_Date
                            End If
                        End If
                    End If
                End If
                
                uow.PaymentRepository.AddAsync(data)
                
                Dim entries = CreatePaymentEntries(data)
                _aeSer.AddEntries(uow.AccountingEntryRepository, entries)

                uow.SaveChangesAsync().Wait()
                uow.Commit()
                
                Initialize()
                MessageBox.Show("新增成功")
            Catch ex As Exception
                uow.Rollback()
                MessageBox.Show(ex.Message)
            End Try
        End Using
    End Sub

    Private Sub Update()
        Using uow As New UnitOfWork()
            uow.BeginTransaction()
            Try
                Dim data As New payment
                _view.GetInput(data)
                Dim orgData = uow.PaymentRepository.GetByIdAsync(selectPayment.p_Id).Result

                uow.PaymentRepository.UpdateAsync(orgData, data).Wait()

                If data.p_Type = "應付票據" Then
                    Dim cheque = uow.ChequePayRepository.GetByChequeNumber(data.chque_pay.cp_Number)

                    If cheque IsNot Nothing Then
                        cheque.cp_Date = data.p_Date
                        cheque.cp_Number = data.chque_pay.cp_Number
                        cheque.cp_Amount = data.p_Amount
                    Else
                        '如果支票不存在，則新增一個新的支票記錄
                        cheque = New chque_pay With {
                            .cp_Date = data.p_Date,
                            .cp_Number = data.chque_pay.cp_Number,
                            .cp_Amount = data.p_Amount
                        }
                        uow.ChequePayRepository.AddAsync(cheque)
                    End If
                End If

                '更新會計分錄
                Dim entries = CreatePaymentEntries(data)
                _aeSer.UpdateEntries(uow.AccountingEntryRepository, entries)

                uow.SaveChangesAsync().Wait()
                uow.Commit()

                Initialize()
                MsgBox("更新成功")
            Catch ex As Exception
                uow.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Private Sub Delete()
        Using uow As New UnitOfWork()
            uow.BeginTransaction()
            Try
                Dim payType = selectPayment.p_Type
                Dim bankId = selectPayment.p_bank_Id
                Dim chequePay = uow.ChequePayRepository.GetByIdAsync(selectPayment.p_cp_Id).Result

                '刪除支票
                If selectPayment.p_Type = "應付票據" Then
                    uow.ChequePayRepository.DeleteAsync(chequePay).Wait()
                End If

                '刪除付款
                uow.PaymentRepository.DeleteAsync(selectPayment.p_Id).Wait()

                '更新月結餘額
                If payType = "銀行" Then
                    _bmbService.UpdateMonthBalanceAsync(uow.BankMonthlyBalancesRepository, uow.BankRepository, uow.PaymentRepository, uow.CollectionRepository, bankId, selectPayment.p_Date).Wait()
                End If

                _aeSer.DeleteEntries(uow.AccountingEntryRepository, "付款作業", selectPayment.p_Id)
                
                uow.SaveChangesAsync().Wait()
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
            Using uow As New UnitOfWork()
                _view.ClearInput()
                selectPayment = uow.PaymentRepository.GetByIdAsync(id).Result
                uow.PaymentRepository.Reload(selectPayment)
                LoadBankDropdown(sender, selectPayment.p_comp_Id)
                _view.ShowDetail(selectPayment)
                If selectPayment.p_m_Id.HasValue Then ManufacturerSelected(sender, selectPayment.p_m_Id)
                _view.ButtonStatus(True)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Print(sender As Object, data As Tuple(Of Date, String))
        Try
            Using uow As New UnitOfWork()
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

    Private Function CreatePaymentEntries(payment As payment) As List(Of accounting_entry)
        Dim entries = New List(Of accounting_entry)

        Select Case payment.p_Type
            Case "現金"
                entries.Add(New accounting_entry With {
                    .ae_TransactionId = payment.p_Id,
                    .ae_Date = payment.p_Date,
                    .ae_TransactionType = "付款作業",
                    .ae_s_Id = payment.p_s_Id,
                    .ae_Debit = payment.p_Amount,
                    .ae_Credit = 0
                })

                entries.Add(New accounting_entry With {
                    .ae_TransactionId = payment.p_Id,
                    .ae_Date = payment.p_Date,
                    .ae_TransactionType = "付款作業",
                    .ae_s_Id = 5,
                    .ae_Debit = 0,
                    .ae_Credit = payment.p_Amount
                })

            Case "銀行存款"
                entries.Add(New accounting_entry With {
                    .ae_TransactionId = payment.p_Id,
                    .ae_Date = payment.p_Date,
                    .ae_TransactionType = "付款作業",
                    .ae_s_Id = payment.p_s_Id,
                    .ae_Debit = payment.p_Amount,
                    .ae_Credit = 0
                })

                entries.Add(New accounting_entry With {
                    .ae_TransactionId = payment.p_Id,
                    .ae_Date = payment.p_Date,
                    .ae_TransactionType = "付款作業",
                    .ae_s_Id = 6,
                    .ae_Debit = 0,
                    .ae_Credit = payment.p_Amount
                })

            Case "應付票據", "應收票據"
                entries.Add(New accounting_entry With {
                    .ae_TransactionId = payment.p_Id,
                    .ae_Date = payment.p_Date,
                    .ae_TransactionType = "付款作業",
                    .ae_s_Id = payment.p_s_Id,
                    .ae_Debit = payment.p_Amount,
                    .ae_Credit = 0
                })

                entries.Add(New accounting_entry With {
                    .ae_TransactionId = payment.p_Id,
                    .ae_Date = payment.p_Date,
                    .ae_TransactionType = "付款作業",
                    .ae_s_Id = 7,
                    .ae_Debit = 0,
                    .ae_Credit = payment.p_Amount
                })
        End Select

        Return entries
    End Function

    Private Sub Search()
        Try
            Dim criteria = _view.GetSearchCriteria
            LoadList(criteria)
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class