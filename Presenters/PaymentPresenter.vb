Public Class PaymentPresenter
    Private ReadOnly _view As IPaymentView
    Private ReadOnly _manufaturerRep As IManufacturerRep
    Private ReadOnly _subjectRep As ISubjectRep
    Private ReadOnly _companyRep As ICompanyRep
    Private ReadOnly _paymentRep As IPaymentRep
    Private ReadOnly _bmbService As IBankMonthlyBalanceService
    Private ReadOnly _aeSer As IAccountingEntryService
    Private ReadOnly _reportSer As IReportService
    Private ReadOnly _cpRep As IChequePayRep
    Private ReadOnly _bankRep As IBankRep
    Private selectPayment As payment

    Public ReadOnly Property View As IPaymentView
        Get
            Return _view
        End Get
    End Property

    Public Sub New(view As IPaymentView, manufaturerRep As IManufacturerRep, subjectRep As ISubjectRep, companyRep As ICompanyRep, paymentRep As IPaymentRep,
                   bmbService As IBankMonthlyBalanceService, aeSer As IAccountingEntryService, reportSer As IReportService, cpRep As IChequePayRep, bankRep As IBankRep)
        _view = view
        _manufaturerRep = manufaturerRep
        _subjectRep = subjectRep
        _companyRep = companyRep
        _paymentRep = paymentRep
        _bmbService = bmbService
        _aeSer = aeSer
        _reportSer = reportSer
        _cpRep = cpRep
        _bankRep = bankRep

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
            Dim payments = _paymentRep.SearchPaymentAsync(criteria).Result
            _view.ShowList(payments.Select(Function(x) New PaymentListVM(x)).ToList)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub LoadVendorDropdown()
        Try
            Dim vendors = _manufaturerRep.GetVendorDropdownAsync.Result
            _view.PopulateVendorDropdown(vendors)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadSubjectDropdown()
        Try
            Dim subjects = _subjectRep.GetSubjectDropdownAsync.Result
            _view.PopulateSubjectDropdown(subjects)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadCompanyDropdown()
        Try
            Dim companies = _companyRep.GetCompanyDropdownAsync.Result
            _view.PopulateCompanyDropdown(companies)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadBankDropdown(sender As Object, companyId As Integer)
        Try
            Dim data = _bankRep.GetBankDropdownAsync(companyId).Result
            _view.PopulateBankDropdown(data)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub ManufacturerSelected(sender As Object, vendorId As Integer)
        Try
            Dim vendor = _manufaturerRep.GetByIdAsync(vendorId).Result
            _view.ShowVendorAccount(vendor.manu_account)
            LoadVendorAmountDue(vendorId)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub LoadVendorAmountDue(vendorId As Integer)
        Try
            Dim amountDue = _paymentRep.GetVendorAmountDue(vendorId)
            _view.DisplayAmountDueList(amountDue)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Add()
        Using transaction = _paymentRep.BeginTransaction
            Try
                Dim data As New payment
                _view.GetInput(data)


                If data.p_Type = "應付票據" Then
                    Dim cheque As New chque_pay
                    _view.GetChequeInput(cheque)
                    cheque.cp_Date = data.p_Date
                    cheque.cp_Amount = data.p_Amount

                    _cpRep.AddAsync(cheque)
                    data.chque_pay = cheque
                ElseIf data.p_Type = "銀行存款" Then
                    ' 支票兌現
                    Dim subject = _subjectRep.GetByIdAsync(data.p_s_Id).Result

                    If subject.s_name = "應付票據" Then
                        Dim chequeNo = InputBox("請輸入支票號碼")

                        If Not String.IsNullOrEmpty(chequeNo) Then
                            Dim cheque = _cpRep.GetByChequeNumber(chequeNo)

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
                _paymentRep.AddAsync(data)
                Dim entries = CreatePaymentEntries(data)
                _aeSer.AddEntries(entries)

                transaction.Commit()
                Initialize()
                MessageBox.Show("新增成功")
            Catch ex As Exception
                transaction.Rollback()
                MessageBox.Show(ex.Message)
            End Try
        End Using
    End Sub

    Private Sub Update()
        Using transaction = _paymentRep.BeginTransaction
            Try
                Dim data As New payment
                _view.GetInput(data)

                _paymentRep.UpdateAsync(selectPayment, data)

                If data.p_Type = "應付票據" Then
                    Dim cheque = _cpRep.GetByChequeNumber(data.chque_pay.cp_Number)

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
                        _cpRep.AddAsync(cheque)
                    End If
                End If

                '更新會計分錄
                Dim entries = CreatePaymentEntries(data)
                _aeSer.UpdateEntries(entries)

                _paymentRep.SaveChangesAsync()
                transaction.Commit()

                Initialize()
                MsgBox("更新成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Private Sub Delete()
        Using transaction = _paymentRep.BeginTransaction
            Try
                Dim payType = selectPayment.p_Type
                Dim bankId = selectPayment.p_bank_Id

                '刪除支票
                If selectPayment.p_Type = "應付票據" Then
                    _cpRep.DeleteAsync(selectPayment.chque_pay)
                End If

                '刪除付款
                _paymentRep.DeleteAsync(selectPayment.p_Id)

                '更新月結餘額
                If payType = "銀行" Then _bmbService.UpdateMonthBalanceAsync(bankId, selectPayment.p_Date)

                _aeSer.DeleteEntries("付款作業", selectPayment.p_Id)
                _paymentRep.SaveChangesAsync()

                transaction.Commit()
                Initialize()
                MessageBox.Show("刪除成功")
            Catch ex As Exception
                transaction.Rollback()
                MessageBox.Show(ex.Message)
            End Try
        End Using
    End Sub

    Private Sub LoadPaymentDetail(sender As Object, id As Integer)
        Try
            _view.ClearInput()
            selectPayment = _paymentRep.GetByIdAsync(id).Result
            _paymentRep.Reload(selectPayment)
            LoadBankDropdown(sender, selectPayment.p_comp_Id)
            _view.ShowDetail(selectPayment)
            If selectPayment.p_m_Id.HasValue Then ManufacturerSelected(sender, selectPayment.p_m_Id)
            _view.ButtonStatus(True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Print(sender As Object, data As Tuple(Of Date, String))
        Try
            Select Case data.Item2
                Case "現金"
                    _reportSer.GeneratorCashSubpoena(data.Item1, _paymentRep.GetCashSubpoenaData(data.Item1), False)
                Case "轉帳"
                    _reportSer.GeneratorTransferSubpoena(data.Item1, _paymentRep.GetTransferSubpoenaData(data.Item1), False)
                Case Else
                    Throw New Exception("類型錯誤")
            End Select
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