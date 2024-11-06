Public Class PaymentPresenter
    Private ReadOnly _view As IPaymentView
    Private ReadOnly _manufaturerRep As IManufacturerRep
    Private ReadOnly _bankRep As IBankRep
    Private ReadOnly _subjectRep As ISubjectRep
    Private ReadOnly _companyRep As ICompanyRep
    Private ReadOnly _paymentRep As IPaymentRep
    Private ReadOnly _bmbService As IBankMonthlyBalanceService
    Private ReadOnly _chequeRep As IChequeRep
    Private ReadOnly _aeSer As IAccountingEntryService
    Private selectData As payment

    Public Sub New(view As IPaymentView, manufaturerRep As IManufacturerRep, bankRep As IBankRep, subjectRep As ISubjectRep, companyRep As ICompanyRep, paymentRep As IPaymentRep,
                   bmbService As IBankMonthlyBalanceService, chequeRep As IChequeRep, aeSer As IAccountingEntryService)
        _view = view
        _manufaturerRep = manufaturerRep
        _bankRep = bankRep
        _subjectRep = subjectRep
        _companyRep = companyRep
        _paymentRep = paymentRep
        _bmbService = bmbService
        _chequeRep = chequeRep
        _aeSer = aeSer
    End Sub

    Public Async Function InitializeAsync() As Task
        Try
            _view.ClearInput()

            Await Task.WhenAll(
                LoadVendorDropdownAsync,
                LoadBanksDropdownAsync,
                LoadSubjectDropdownAsync,
                LoadCompanyDropdownAsync
            )

            Await SearchPaymentsAsync()
            selectData = Nothing
        Catch ex As Exception
            MsgBox("付款作業初始化發生錯誤:" + ex.Message)
            Console.WriteLine(ex.Source)
        End Try
    End Function

    Public Async Function SearchPaymentsAsync() As Task
        Try
            Dim criteria = _view.GetSearchCriteria
            Dim payments = Await _paymentRep.SearchPaymentAsync(criteria)
            _view.DisplayList(payments.Select(Function(x) New PaymentVM(x)).ToList)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Sub LoadVendorAmountDue(vendorId As Integer)
        Try
            Dim amountDue = _paymentRep.GetVendorAmountDue(vendorId)
            _view.DisplayAmountDueList(amountDue)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Add()
        Using transaction = _paymentRep.BeginTransaction
            Try
                Dim payment = _view.GetUserInput
                Await _paymentRep.AddAsync(payment)

                If payment.p_Type = "銀行" Then Await _bmbService.UpdateMonthBalanceAsync(payment.p_bank_Id, payment.p_AccountMonth)

                Dim entries = CreatePaymentEntries(payment)
                _aeSer.AddEntries(entries)

                transaction.Commit()
                Await InitializeAsync()
                MsgBox("新增成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub Update()
        Using transaction = _paymentRep.BeginTransaction
            Try
                Dim payment = _view.GetUserInput

                Await _paymentRep.UpdateAsync(selectData, payment)

                ' 更新付款記錄
                If payment.p_Type = "銀行" Then Await _bmbService.UpdateMonthBalanceAsync(payment.p_bank_Id, payment.p_AccountMonth)

                '更新會計分錄
                Dim entries = CreatePaymentEntries(payment)
                _aeSer.UpdateEntries(entries)

                Await _paymentRep.SaveChangesAsync()
                transaction.Commit()

                Await InitializeAsync()
                MsgBox("更新成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Function DeleteAsync(id As Integer) As Task
        Using transaction = _paymentRep.BeginTransaction
            Try
                Dim payment = Await _paymentRep.GetByIdAsync(id)
                Dim payType = payment.p_Type
                Dim bankId = payment.p_bank_Id
                Dim accountMonth = payment.p_AccountMonth

                '刪除支票
                If Not String.IsNullOrEmpty(payment.p_Cheque) Then
                    Dim che = Await _chequeRep.GetByNumberAsync(payment.p_Cheque)
                    Await _chequeRep.DeleteAsync(che.che_Id)
                End If

                '刪除付款
                Await _paymentRep.DeleteAsync(id)

                '更新月結餘額
                If payType = "銀行" Then Await _bmbService.UpdateMonthBalanceAsync(bankId, accountMonth)

                _aeSer.DeleteEntries("付款作業", payment.p_Id)
                Await _paymentRep.SaveChangesAsync()

                transaction.Commit()

                _view.ClearInput()
                Await SearchPaymentsAsync()
                MsgBox("刪除成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Function

    Public Async Function LoadPaymentDetailAsync(id As Integer) As Task
        Try
            selectData = Await _paymentRep.GetByIdAsync(id)
            _view.DisplayDetail(selectData)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Async Function LoadVendorDropdownAsync() As Task
        Try
            Dim vendors = Await _manufaturerRep.GetVendorDropdownAsync
            _view.PopulateVendorDropdown(vendors)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Async Function LoadBanksDropdownAsync() As Task
        Try
            Dim banks = Await _bankRep.GetBankDropdownAsync
            _view.PopulateBankDropdown(banks)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Async Function LoadSubjectDropdownAsync() As Task
        Try
            Dim subjects = Await _subjectRep.GetSubjectDropdownAsync
            _view.PopulateSubjectDropdown(subjects)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Async Function LoadCompanyDropdownAsync() As Task
        Try
            Dim companies = Await _companyRep.GetCompanyDropdownAsync
            _view.PopulateCompanyDropdown(companies)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

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

            Case "銀行"
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

            Case "支票"
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
End Class