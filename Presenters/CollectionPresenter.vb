Imports System.IO
Imports ClosedXML.Excel
Imports iText.Signatures.Validation.V1

Public Class CollectionPresenter
    Private _view As ICollectionView
    Private ReadOnly _subjectRep As ISubjectRep
    Private ReadOnly _colRep As ICollectionRep
    Private ReadOnly _bankRep As IBankRep
    Private ReadOnly _cusRep As ICustomerRep
    Private ReadOnly _bmbService As IBankMonthlyBalanceService
    Private ReadOnly _chequeRep As IChequeRep
    Private ReadOnly _aeSer As IAccountingEntryService
    Private ReadOnly _compRep As ICompanyRep
    Private ReadOnly _ocmSer As IOrderCollectionMappingService
    Private ReadOnly _report As ReportService = New ReportService
    Private _currentData As collection

    Public Sub New(view As ICollectionView, subjectRep As ISubjectRep, colRep As ICollectionRep, bankRep As IBankRep, cusRep As ICustomerRep,
                   bmbService As IBankMonthlyBalanceService, chequeRep As IChequeRep, aeSer As IAccountingEntryService, compRep As ICompanyRep, ocmSer As IOrderCollectionMappingService)
        _view = view
        _subjectRep = subjectRep
        _colRep = colRep
        _bankRep = bankRep
        _cusRep = cusRep
        _bmbService = bmbService
        _chequeRep = chequeRep
        _aeSer = aeSer
        _compRep = compRep
        _ocmSer = ocmSer
    End Sub

    Public Sub Initialize()
        Try
            _view.ClearInput()
            LoadCmbsAsync()
            LoadList()
            _currentData = Nothing

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Async Sub LoadCmbsAsync()
        _view.SetBankCmb(Await _bankRep.GetBankDropdownAsync)
        _view.SetCompanyCmb(Await _compRep.GetCompanyDropdownAsync)
        _view.SetSubjectCmb(Await _subjectRep.GetSubjectDropdownAsync)
    End Sub

    Public Sub LoadList()
        Try
            Dim datas = _colRep.Search(_view.GetSearchCriteria)
            _view.DisplayList(datas)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Add()
        Using transaction = _colRep.BeginTransaction
            Try
                Dim input = _view.GetUserInput
                Validate(input)
                input.col_UnmatchedAmount = input.col_Amount

                Dim col = Await _colRep.AddAsync(input)

                Select Case input.col_Type
                    Case "支票"
                        Dim cheInput = _view.GetChequeInput
                        Validate(cheInput)
                        cheInput.che_col_Id = col.col_Id
                        Await _chequeRep.AddAsync(cheInput)
                    Case "銀行"
                        Await _bmbService.UpdateMonthBalanceAsync(col.col_bank_Id, col.col_AccountMonth)
                End Select

                Dim entries = CreatePaymentEntries(input)
                _aeSer.AddEntries(entries)

                Await _colRep.SaveChangesAsync

                transaction.Commit()
                Initialize()
                MsgBox("新增成功")
            Catch ex As Exception
                transaction.Rollback()
                Console.WriteLine(ex.StackTrace)
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub LoadDetail(id As Integer)
        Try
            Dim data = Await _colRep.GetByIdAsync(id)
            _currentData = data
            _view.ClearInput()
            _view.DisplayDetail(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Edit()
        Using transaction = _colRep.BeginTransaction
            Try
                Dim col = _view.GetUserInput

                '未銷帳
                Dim paid = _ocmSer.CalculateCollectionUnmatched(col.col_Id)
                col.col_UnmatchedAmount = col.col_Amount - paid
                Await _colRep.UpdateAsync(_currentData, col)

                Select Case col.col_Type
                    Case "現金"
                        If _currentData.col_Type = "銀行" Then
                            Await _bmbService.UpdateMonthBalanceAsync(_currentData.col_bank_Id, _currentData.col_AccountMonth)
                        ElseIf _currentData.col_Type = "支票" Then
                            Dim cheque = Await _chequeRep.GetByNumberAsync(_currentData.col_Cheque)
                            Await _chequeRep.DeleteAsync(cheque.che_Id)
                        End If
                    Case "銀行"
                        Await _bmbService.UpdateMonthBalanceAsync(col.col_bank_Id, col.col_AccountMonth)
                        Await _bmbService.UpdateMonthBalanceAsync(col.col_bank_Id, _currentData.col_AccountMonth)

                        If _currentData.col_bank_Id.HasValue Then
                            Await _bmbService.UpdateMonthBalanceAsync(_currentData.col_bank_Id, col.col_AccountMonth)
                            Await _bmbService.UpdateMonthBalanceAsync(_currentData.col_bank_Id, _currentData.col_AccountMonth)
                        End If

                        If _currentData.col_Type = "支票" Then
                            Dim cheque = Await _chequeRep.GetByNumberAsync(_currentData.col_Cheque)
                            Await _chequeRep.DeleteAsync(cheque.che_Id)
                        End If
                    Case "支票"
                        Dim cheque = _view.GetChequeInput

                        If _currentData.col_Type = "支票" Then
                            Dim orgCheque = Await _chequeRep.GetByNumberAsync(_currentData.col_Cheque)
                            cheque.che_Id = orgCheque.che_Id
                            Await _chequeRep.UpdateAsync(orgCheque, cheque)
                        ElseIf _currentData.col_Type = "現金" Then
                            Await _chequeRep.AddAsync(cheque)
                        ElseIf _currentData.col_Type = "銀行" Then
                            Await _bmbService.UpdateMonthBalanceAsync(_currentData.col_bank_Id, _currentData.col_AccountMonth)
                        End If
                End Select

                Dim entries = CreatePaymentEntries(col)
                _aeSer.UpdateEntries(entries)

                Await _colRep.SaveChangesAsync
                transaction.Commit()
                Initialize()
                MsgBox("修改成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub DeleteAsync()
        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.No Then Exit Sub
        Using transaction = _colRep.BeginTransaction
            Try
                Dim payType = _currentData.col_Type

                ' 銷帳
                _ocmSer.DeleteCollection(_currentData.col_Id)

                '刪除資料
                Await _colRep.DeleteAsync(_currentData)

                If payType = "支票" Then
                    Dim cheque = Await _chequeRep.GetByNumberAsync(_currentData.col_Cheque)
                    Await _chequeRep.DeleteAsync(cheque.che_Id)
                ElseIf payType = "銀行" Then
                    Await _bmbService.UpdateMonthBalanceAsync(_currentData.col_bank_Id, _currentData.col_AccountMonth)
                End If

                _aeSer.DeleteEntries("收款作業", _currentData.col_Id)

                transaction.Commit()
                Initialize()
                MsgBox("刪除成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub UpdateCheque()
        Try
            If _currentData Is Nothing Then Return

            Dim cheque = Await _chequeRep.GetByNumberAsync(_currentData.col_Cheque)
            cheque.che_CashingDate = Now
            cheque.chu_State = "已兌現"
            Await _chequeRep.SaveChangesAsync()
            Initialize()
            MsgBox("兌現成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Validate(data As collection)
        If data.col_Amount = 0 Then Throw New Exception("請輸入金額")
        If data.col_s_Id Is Nothing Then Throw New Exception("請選擇科目")
        If data.col_comp_Id Is Nothing Then Throw New Exception("請選擇公司")
        If data.col_cus_Id = 0 Then Throw New Exception("請輸入客戶")
        If String.IsNullOrEmpty(data.col_Type) Then Throw New Exception("請選擇收款類型")
        If data.col_Type = "銀行" Then
            If data.col_bank_Id Is Nothing Then Throw New Exception("請選擇銀行")
        ElseIf data.col_Type = "支票" Then
            If String.IsNullOrEmpty(data.col_Cheque) Then Throw New Exception("請輸入支票號碼")
        End If
    End Sub

    Private Sub Validate(data As cheque)
        If String.IsNullOrEmpty(data.che_IssuerName) Then Throw New Exception("請輸入發票人")
        If String.IsNullOrEmpty(data.che_AccountNumber) Then Throw New Exception("請輸入支票銀行帳號")
    End Sub

    Public Function GetCustomer(cusCode As String) As customer
        Return _cusRep.GetByCusCode(cusCode)
    End Function

    Public Sub Print()
        Dim today = Now.Date
        Dim transferDatas = _colRep.GetTransferSubpoenaData(today)
        Dim cashIncomDatas = _colRep.GetTransferSubpoenaData(today, True)

        _report.GeneratorTransferSubpoena(today, transferDatas)
        _report.GeneratorCashIncomeSubpoena(today, cashIncomDatas, True)
    End Sub

    Private Function CreatePaymentEntries(data As collection) As List(Of accounting_entry)
        Dim entries = New List(Of accounting_entry)

        Select Case data.col_Type
            Case "現金"
                entries.Add(New accounting_entry With {
                    .ae_TransactionId = data.col_Id,
                    .ae_Date = data.col_Date,
                    .ae_TransactionType = "收款作業",
                    .ae_s_Id = data.col_s_Id,
                    .ae_Debit = 0,
                    .ae_Credit = data.col_Amount
                })

                entries.Add(New accounting_entry With {
                    .ae_TransactionId = data.col_Id,
                    .ae_Date = data.col_Date,
                    .ae_TransactionType = "收款作業",
                    .ae_s_Id = 10,
                    .ae_Debit = data.col_Amount,
                    .ae_Credit = 0
                })

            Case "銀行"
                entries.Add(New accounting_entry With {
                    .ae_TransactionId = data.col_Id,
                    .ae_Date = data.col_Date,
                    .ae_TransactionType = "收款作業",
                    .ae_s_Id = data.col_s_Id,
                    .ae_Debit = 0,
                    .ae_Credit = data.col_Amount
                })

                entries.Add(New accounting_entry With {
                    .ae_TransactionId = data.col_Id,
                    .ae_Date = data.col_Date,
                    .ae_TransactionType = "收款作業",
                    .ae_s_Id = 11,
                    .ae_Debit = data.col_Amount,
                    .ae_Credit = 0
                })

            Case "支票"
                entries.Add(New accounting_entry With {
                    .ae_TransactionId = data.col_Id,
                    .ae_Date = data.col_Date,
                    .ae_TransactionType = "收款作業",
                    .ae_s_Id = data.col_s_Id,
                    .ae_Debit = 0,
                    .ae_Credit = data.col_Amount
                })

                entries.Add(New accounting_entry With {
                    .ae_TransactionId = data.col_Id,
                    .ae_Date = data.col_Date,
                    .ae_TransactionType = "收款作業",
                    .ae_s_Id = 12,
                    .ae_Debit = data.col_Amount,
                    .ae_Credit = 0
                })
        End Select

        Return entries
    End Function
End Class