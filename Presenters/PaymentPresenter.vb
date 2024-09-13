Public Class PaymentPresenter
    Private ReadOnly _view As IPaymentView
    Private ReadOnly _manufaturerRep As IManufacturerRep
    Private ReadOnly _bankRep As IBankRep
    Private ReadOnly _subjectRep As ISubjectRep
    Private ReadOnly _companyRep As ICompanyRep
    Private ReadOnly _paymentRep As IPaymentRep
    Private ReadOnly _bmbService As IBankMonthlyBalanceService
    Private ReadOnly _chequeRep As IChequeRep


    Public Sub New(view As IPaymentView, manufaturerRep As IManufacturerRep, bankRep As IBankRep, subjectRep As ISubjectRep, companyRep As ICompanyRep, paymentRep As IPaymentRep, bmbService As IBankMonthlyBalanceService, chequeRep As IChequeRep)
        _view = view
        _manufaturerRep = manufaturerRep
        _bankRep = bankRep
        _subjectRep = subjectRep
        _companyRep = companyRep
        _paymentRep = paymentRep
        _bmbService = bmbService
        _chequeRep = chequeRep
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
        Catch ex As Exception
            MsgBox("付款作業初始化發生錯誤:" + ex.Message)
            Console.WriteLine(ex.Source)
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

    Public Async Function AddAsync() As Task
        Using transaction = _paymentRep.BeginTransaction
            Try
                Dim payment = _view.GetUserInput
                Await _paymentRep.AddAsync(payment)

                If payment.p_Type = "支票" Then
                    Dim cheque = CreateChequeFromPayment(payment)
                    Await _chequeRep.AddAsync(cheque)
                ElseIf payment.p_Type = "銀行" Then
                    Await _bmbService.UpdateMonthBalanceAsync(payment.p_bank_Id, payment.p_AccountMonth)
                End If

                transaction.Commit()

                _view.ClearInput()
                Await SearchPaymentsAsync()
                MsgBox("新增成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Function

    Public Async Function UpdateAsync() As Task
        Using transaction = _paymentRep.BeginTransaction
            Try
                Dim payment = _view.GetUserInput
                Dim orgPayment = Await _paymentRep.GetByIdAsync(payment.p_Id)

                '檢查是否從支票改為其他類型
                If orgPayment.p_Type = "支票" AndAlso payment.p_Type <> "支票" Then
                    '刪除舊的支票記錄
                    Dim orgChe = Await _chequeRep.GetByNumberAsync(orgPayment.p_Cheque)
                    If orgChe IsNot Nothing Then Await _chequeRep.DeleteAsync(orgChe.che_Id)
                End If

                ' 更新付款記錄
                Dim orgBankId = payment.p_bank_Id
                Dim orgAccountMonth = orgPayment.p_AccountMonth
                If payment.p_Type <> "銀行" Then payment.p_bank_Id = Nothing
                Await _paymentRep.UpdateAsync(payment.p_Id, payment)
                Dim updatedPayment = Await _paymentRep.GetByIdAsync(payment.p_Id)

                ' 處理新的或更新的支票
                If payment.p_Type = "支票" Then
                    Dim existingCheque = Await _chequeRep.GetByNumberAsync(updatedPayment.p_Cheque)
                    Dim cheque = CreateChequeFromPayment(updatedPayment)
                    cheque.che_Id = If(existingCheque Is Nothing, 0, existingCheque.che_Id)

                    If existingCheque IsNot Nothing Then
                        Await _chequeRep.UpdateAsync(existingCheque.che_Id, cheque)
                    Else
                        Await _chequeRep.AddAsync(cheque)
                    End If
                End If

                '處理銀行月結餘額
                If orgBankId IsNot Nothing Then
                    '如果修改帳款月份,就更新原始月份的借、貸總額
                    If orgAccountMonth <> updatedPayment.p_AccountMonth Then Await _bmbService.UpdateMonthBalanceAsync(orgBankId, orgAccountMonth)

                    Await _bmbService.UpdateMonthBalanceAsync(orgBankId, updatedPayment.p_AccountMonth)
                End If

                transaction.Commit()

                _view.ClearInput()
                Await SearchPaymentsAsync()
                MsgBox("更新成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Function

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
            Dim payment = Await _paymentRep.GetByIdAsync(id)
            _view.DisplayDetail(payment)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Function CreateChequeFromPayment(payment As payment) As cheque
        Return New cheque With {
            .che_Amount = payment.p_Amount,
            .che_IssuerName = payment.company.comp_name,
            .che_Memo = payment.p_Memo,
            .che_Number = payment.p_Cheque,
            .che_ReceivedDate = payment.p_Date,
            .chu_State = "未兌現",
            .che_Type = "貸"
        }
    End Function
End Class