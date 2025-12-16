Public Class CollectionPresenter
    Private _view As ICollectionView
    Private ReadOnly _bmbService As IBankMonthlyBalanceService
    Private ReadOnly _ocmSer As IOrderCollectionMappingService
    Private ReadOnly _reportSer As IReportService
    Private ReadOnly _collectionSer As ICollectionService
    Private _currentData As collection
    Private _currentCheque As cheque

    ''' <summary>
    ''' 建構子：使用 UnitOfWork 模式，只需注入 Service 層依賴
    ''' </summary>
    Public Sub New(bmbService As IBankMonthlyBalanceService, ocmSer As IOrderCollectionMappingService, reportSer As IReportService, collectionSer As ICollectionService)
        _bmbService = bmbService
        _ocmSer = ocmSer
        _reportSer = reportSer
        _collectionSer = collectionSer
    End Sub

    Public Sub SetView(view As ICollectionView)
        _view = view
    End Sub

    Public Sub Initialize()
        Try
            _view.ClearInput()
            LoadCmbsAsync()
            LoadList()
            _currentData = Nothing
            _currentCheque = Nothing
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Async Sub LoadCmbsAsync()
        Try
            Using uow As New UnitOfWork()
                _view.SetBankCmb(Await uow.BankRepository.GetBankDropdownAsync)
                _view.SetCompanyCmb(Await uow.CompanyRepository.GetCompanyDropdownAsync)
                _view.SetSubjectCmb(Await uow.SubjectRepository.GetSubjectDropdownAsync)
            End Using
        Catch ex As Exception
            MsgBox("載入下拉選單時發生錯誤：" & ex.Message)
        End Try
    End Sub

    Public Sub LoadList(Optional criteria As CollectionSearchCriteria = Nothing)
        Try
            Using uow As New UnitOfWork()
                Dim datas = uow.CollectionRepository.GetList(criteria)
                _view.DisplayList(datas)
            End Using
        Catch ex As Exception
            MessageBox.Show("載入清單時發生錯誤：" & ex.Message)
        End Try
    End Sub

    Public Async Sub Add()
        Try
            Dim input = _view.GetUserInput
            Validate(input)

            Dim chequeInput As cheque = Nothing
            Dim chequeNo As String = Nothing

            If input.col_Type = "應收票據" Then
                chequeInput = _view.GetChequeInput
                Validate(chequeInput)
            ElseIf input.col_Type = "銀行存款" AndAlso input.subject.s_name = "應收票據" Then
                chequeNo = _view.GetChequeNumber()
            End If

            Await _collectionSer.AddAsync(input, chequeInput, chequeNo)

            Initialize()
            MessageBox.Show("新增成功")
        Catch ex As Exception
            MessageBox.Show("新增時發生錯誤：" & ex.Message)
        End Try
    End Sub

    Public Sub LoadDetail(id As Integer)
        Try
            Using uow As New UnitOfWork()
                _currentData = uow.CollectionRepository.GetByIdAsync(id).Result
                _currentCheque = If(String.IsNullOrEmpty(_currentData.col_Cheque), Nothing,
                                   uow.ChequeRepository.GetByNumberAsync(_currentData.col_Cheque).Result)
                _view.ClearInput()
                _view.DisplayDetail(_currentData)
                If _currentCheque IsNot Nothing Then _view.ShowCheque(_currentCheque)
            End Using
        Catch ex As Exception
            MessageBox.Show("載入詳細資料時發生錯誤：" & ex.Message)
        End Try
    End Sub

    Public Async Sub Edit()
        Using uow As New UnitOfWork()
            Try
                uow.BeginTransaction()

                Dim col = _view.GetUserInput
                Validate(col)

                '未銷帳
                Dim paid = _ocmSer.CalculateCollectionUnmatched(col.col_Id)
                col.col_UnmatchedAmount = col.col_Amount - paid

                Dim orgCol = Await uow.CollectionRepository.GetByIdAsync(col.col_Id)
                If orgCol Is Nothing Then Throw New Exception("找不到要更新的收款資料，可能已被刪除")

                ' 儲存舊資料用於月結更新
                Dim oldBankId = orgCol.col_bank_Id
                Dim oldMonth = orgCol.col_AccountMonth
                Dim oldAmount = orgCol.col_Amount
                Dim oldType = orgCol.col_Type

                Select Case col.col_Type
                    Case "現金"
                        ' 從銀行存款改為現金 - 減少舊銀行收入
                        If orgCol.col_Type = "銀行存款" Then
                            Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                uow.BankMonthlyBalancesRepository,
                                uow.BankRepository,
                                orgCol.col_bank_Id,
                                orgCol.col_AccountMonth,
                                creditDelta:=0,
                                debitDelta:=-oldAmount
                            )
                        ElseIf orgCol.col_Type = "應收票據" Then
                            Dim cheque = Await uow.ChequeRepository.GetByNumberAsync(orgCol.col_Cheque)
                            Await uow.ChequeRepository.DeleteAsync(cheque.che_Id)
                        End If

                    Case "銀行存款"
                        ' ✨ 智能判斷需要更新的月結餘額
                        If oldType = "銀行存款" Then
                            ' 都是銀行存款
                            If oldBankId = col.col_bank_Id Then
                                ' 同銀行
                                If oldMonth.Year = col.col_AccountMonth.Year AndAlso oldMonth.Month = col.col_AccountMonth.Month Then
                                    ' 同月份 - 只調整差額
                                    Dim amountDelta = col.col_Amount - oldAmount
                                    If amountDelta <> 0 Then
                                        Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                            uow.BankMonthlyBalancesRepository,
                                            uow.BankRepository,
                                            col.col_bank_Id,
                                            col.col_AccountMonth,
                                            creditDelta:=0,
                                            debitDelta:=amountDelta
                                        )
                                    End If
                                Else
                                    ' 不同月份 - 舊月份減少，新月份增加
                                    Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                        uow.BankMonthlyBalancesRepository,
                                        uow.BankRepository,
                                        oldBankId,
                                        oldMonth,
                                        creditDelta:=0,
                                        debitDelta:=-oldAmount
                                    )
                                    Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                        uow.BankMonthlyBalancesRepository,
                                        uow.BankRepository,
                                        col.col_bank_Id,
                                        col.col_AccountMonth,
                                        creditDelta:=0,
                                        debitDelta:=col.col_Amount
                                    )
                                End If
                            Else
                                ' 不同銀行 - 舊銀行減少，新銀行增加
                                Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                    uow.BankMonthlyBalancesRepository,
                                    uow.BankRepository,
                                    oldBankId,
                                    oldMonth,
                                    creditDelta:=0,
                                    debitDelta:=-oldAmount
                                )
                                Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                    uow.BankMonthlyBalancesRepository,
                                    uow.BankRepository,
                                    col.col_bank_Id,
                                    col.col_AccountMonth,
                                    creditDelta:=0,
                                    debitDelta:=col.col_Amount
                                )
                            End If
                        ElseIf oldType = "應收票據" Then
                            ' 從應收票據改為銀行存款 - 新銀行增加收入，刪除舊支票
                            Dim cheque = Await uow.ChequeRepository.GetByNumberAsync(orgCol.col_Cheque)
                            Await uow.ChequeRepository.DeleteAsync(cheque.che_Id)

                            Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                uow.BankMonthlyBalancesRepository,
                                uow.BankRepository,
                                col.col_bank_Id,
                                col.col_AccountMonth,
                                creditDelta:=0,
                                debitDelta:=col.col_Amount
                            )
                        Else
                            ' 從現金改為銀行存款 - 新銀行增加收入
                            Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                uow.BankMonthlyBalancesRepository,
                                uow.BankRepository,
                                col.col_bank_Id,
                                col.col_AccountMonth,
                                creditDelta:=0,
                                debitDelta:=col.col_Amount
                            )
                        End If

                    Case "應收票據"
                        Dim cheque = _view.GetChequeInput

                        If _currentData.col_Type = "應收票據" Then
                            Dim orgCheque = Await uow.ChequeRepository.GetByNumberAsync(orgCol.col_Cheque)
                            If orgCheque Is Nothing Then
                                cheque.che_CollectionDate = col.col_Date
                                Await uow.ChequeRepository.AddAsync(cheque)
                            Else
                                cheque.che_Id = orgCheque.che_Id
                                Await uow.ChequeRepository.UpdateAsync(orgCheque, cheque)
                            End If
                        ElseIf orgCol.col_Type = "現金" Then
                            Await uow.ChequeRepository.AddAsync(cheque)
                        ElseIf orgCol.col_Type = "銀行存款" Then
                            ' 從銀行存款改為應收票據 - 減少舊銀行收入
                            Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                                uow.BankMonthlyBalancesRepository,
                                uow.BankRepository,
                                orgCol.col_bank_Id,
                                orgCol.col_AccountMonth,
                                creditDelta:=0,
                                debitDelta:=-oldAmount
                            )
                        End If
                End Select

                If oldBankId.HasValue AndAlso col.col_bank_Id.HasValue Then
                    ' 科目為銀行存款的特殊處理（同 Add）：依科目增量調整
                    Dim oldSubject = Await uow.SubjectRepository.GetByIdAsync(orgCol.col_s_Id)
                    Dim newSubject = Await uow.SubjectRepository.GetByIdAsync(col.col_s_Id)

                    Await AdjustBankMonthlyBalanceForBankSubjectAsync(
                        uow,
                        oldSubject IsNot Nothing AndAlso oldSubject.s_name = "銀行存款",
                        newSubject IsNot Nothing AndAlso newSubject.s_name = "銀行存款",
                        oldBankId,
                        oldMonth,
                        oldAmount,
                        col.col_bank_Id,
                        col.col_AccountMonth,
                        col.col_Amount
                    )
                End If

                Await uow.CollectionRepository.UpdateAsync(orgCol, col)

                Await uow.SaveChangesAsync()
                uow.Commit()
                Initialize()
                MessageBox.Show("修改成功")
            Catch ex As Exception
                uow.Rollback()
                MessageBox.Show("修改時發生錯誤：" & ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub DeleteAsync()
        If MessageBox.Show("確定要刪除?", "警告", MessageBoxButtons.OKCancel) = MsgBoxResult.No Then Exit Sub

        Try
            Await _collectionSer.DeleteAsync(_currentData.col_Id)
            Initialize()
            MessageBox.Show("刪除成功")
        Catch ex As Exception
            MessageBox.Show("刪除時發生錯誤：" & ex.Message)
        End Try
    End Sub

    Private Async Function AdjustBankMonthlyBalanceForBankSubjectAsync(uow As IUnitOfWork,
                                                                       oldIsBankSubject As Boolean,
                                                                       newIsBankSubject As Boolean,
                                                                       oldBankId As Integer,
                                                                       oldMonth As Date,
                                                                       oldAmount As Decimal,
                                                                       newBankId As Integer,
                                                                       newMonth As Date,
                                                                       newAmount As Decimal) As Task
        If Not oldIsBankSubject AndAlso Not newIsBankSubject Then Exit Function

        If oldIsBankSubject AndAlso newIsBankSubject Then
            If oldBankId = newBankId AndAlso oldMonth.Year = newMonth.Year AndAlso oldMonth.Month = newMonth.Month Then
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
                Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                    uow.BankMonthlyBalancesRepository,
                    uow.BankRepository,
                    oldBankId,
                    oldMonth,
                    creditDelta:=0,
                    debitDelta:=-oldAmount
                )
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
            Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                uow.BankMonthlyBalancesRepository,
                uow.BankRepository,
                oldBankId,
                oldMonth,
                creditDelta:=0,
                debitDelta:=-oldAmount
            )
        ElseIf newIsBankSubject Then
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

    Private Sub Validate(data As collection)
        If data.col_Amount = 0 Then Throw New Exception("請輸入金額")
        If data.col_s_Id Is Nothing Then Throw New Exception("請選擇科目")
        If data.col_comp_Id Is Nothing Then Throw New Exception("請選擇公司")
        If String.IsNullOrEmpty(data.col_Type) Then Throw New Exception("請選擇收款類型")
        If data.col_Type = "銀行存款" Then
            If data.col_bank_Id Is Nothing Then Throw New Exception("請選擇銀行")
        ElseIf data.col_Type = "應收票據" Then
            If String.IsNullOrEmpty(data.col_Cheque) Then Throw New Exception("請輸入支票號碼")
        End If
        If data.col_cus_Id = 0 Then data.col_cus_Id = Nothing
    End Sub

    Private Sub Validate(data As cheque)
        If String.IsNullOrEmpty(data.che_IssuerName) Then Throw New Exception("請輸入發票人")
        If String.IsNullOrEmpty(data.che_AccountNumber) Then Throw New Exception("請輸入支票銀行帳號")
    End Sub

    Public Function GetCustomer(cusCode As String) As customer
        Try
            Using uow As New UnitOfWork()
                Return uow.CustomerRepository.GetByCusCode(cusCode)
            End Using
        Catch ex As Exception
            MsgBox("取得客戶資料時發生錯誤：" & ex.Message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 列印
    ''' </summary>
    ''' <param name="selectDate">選擇日期</param>
    ''' <param name="type">傳票類型:現金、轉帳</param>
    Public Sub Print(selectDate As Date, type As String)
        Try
            Using uow As New UnitOfWork()
                Select Case type
                    Case "現金"
                        _reportSer.GeneratorCashSubpoena(selectDate, uow.CollectionRepository.GetCashSubpoenaData(selectDate), True)
                    Case "轉帳"
                        _reportSer.GeneratorTransferSubpoena(selectDate, uow.CollectionRepository.GetTarnsferSubpoenaData(selectDate), True)
                    Case Else
                        Throw New Exception("type 傳票類型錯誤")
                End Select
            End Using
        Catch ex As Exception
            MsgBox("列印時發生錯誤：" & ex.Message)
        End Try
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

            Case "銀行存款"
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

            Case "應收票據"
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