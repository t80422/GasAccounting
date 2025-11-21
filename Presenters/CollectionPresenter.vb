Imports SixLabors.Fonts.Tables.General

Public Class CollectionPresenter
    Private _view As ICollectionView
    Private ReadOnly _bmbService As IBankMonthlyBalanceService
    Private ReadOnly _aeSer As IAccountingEntryService
    Private ReadOnly _ocmSer As IOrderCollectionMappingService
    Private ReadOnly _reportSer As IReportService
    Private _currentData As collection
    Private _currentCheque As cheque

    ''' <summary>
    ''' 建構子：使用 UnitOfWork 模式，只需注入 Service 層依賴
    ''' </summary>
    Public Sub New(bmbService As IBankMonthlyBalanceService, aeSer As IAccountingEntryService,
                   ocmSer As IOrderCollectionMappingService, reportSer As IReportService)
        _bmbService = bmbService
        _aeSer = aeSer
        _ocmSer = ocmSer
        _reportSer = reportSer
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
            MsgBox("載入清單時發生錯誤：" & ex.Message)
        End Try
    End Sub

    Public Async Sub Add()
        Using uow As New UnitOfWork()
            Try
                uow.BeginTransaction()

                Dim input = _view.GetUserInput
                Validate(input)
                input.col_UnmatchedAmount = input.col_Amount

                Dim col = Await uow.CollectionRepository.AddAsync(input)
                Dim chequeNo As String = ""

                Select Case input.col_Type
                    Case "應收票據"
                        Dim cheInput = _view.GetChequeInput
                        Validate(cheInput)
                        cheInput.che_col_Id = col.col_Id
                        Await uow.ChequeRepository.AddAsync(cheInput)

                    Case "銀行存款"
                        Await _bmbService.UpdateMonthBalanceAsync(col.col_bank_Id, col.col_AccountMonth)

                        ' 支票兌現
                        Dim subject = Await uow.SubjectRepository.GetByIdAsync(col.col_s_Id)

                        If subject.s_name = "應收票據" Then
                            chequeNo = _view.GetChequeNumber()

                            If Not String.IsNullOrEmpty(chequeNo) Then
                                Dim cheque = Await uow.ChequeRepository.GetByNumberAsync(chequeNo)

                                If cheque Is Nothing Then
                                    Throw New Exception("找不到此支票")
                                ElseIf cheque.chu_State = "已兌現" Then
                                    Throw New Exception("此支票已兌現")
                                ElseIf cheque.chu_State = Nothing Then
                                    Throw New Exception("此支票未代收")
                                Else
                                    cheque.chu_State = "已兌現"
                                    cheque.che_CashingDate = col.col_Date
                                    col.col_Cheque = chequeNo
                                End If
                            End If
                        End If
                End Select

                Dim entries = CreatePaymentEntries(input)
                _aeSer.AddEntries(entries)

                Await uow.SaveChangesAsync()

                uow.Commit()
                Initialize()
                MsgBox("新增成功")
            Catch ex As Exception
                uow.Rollback()
                Console.WriteLine(ex.StackTrace)
                MsgBox("新增時發生錯誤：" & ex.Message)
            End Try
        End Using
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

                Await uow.CollectionRepository.UpdateAsync(orgCol, col)

                Select Case col.col_Type
                    Case "現金"
                        If orgCol.col_Type = "銀行存款" Then
                            Await _bmbService.UpdateMonthBalanceAsync(orgCol.col_bank_Id, orgCol.col_AccountMonth)
                        ElseIf orgCol.col_Type = "應收票據" Then
                            Dim cheque = Await uow.ChequeRepository.GetByNumberAsync(orgCol.col_Cheque)
                            Await uow.ChequeRepository.DeleteAsync(cheque.che_Id)
                        End If

                    Case "銀行存款"
                        Await _bmbService.UpdateMonthBalanceAsync(col.col_bank_Id, col.col_AccountMonth)
                        Await _bmbService.UpdateMonthBalanceAsync(col.col_bank_Id, _currentData.col_AccountMonth)

                        If orgCol.col_bank_Id.HasValue Then
                            Await _bmbService.UpdateMonthBalanceAsync(orgCol.col_bank_Id, col.col_AccountMonth)
                            Await _bmbService.UpdateMonthBalanceAsync(orgCol.col_bank_Id, orgCol.col_AccountMonth)
                        End If

                        If orgCol.col_Type = "應收票據" Then
                            Dim cheque = Await uow.ChequeRepository.GetByNumberAsync(orgCol.col_Cheque)
                            Await uow.ChequeRepository.DeleteAsync(cheque.che_Id)
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
                            Await _bmbService.UpdateMonthBalanceAsync(orgCol.col_bank_Id, orgCol.col_AccountMonth)
                        End If
                End Select

                Dim entries = CreatePaymentEntries(col)
                _aeSer.UpdateEntries(entries)

                Await uow.SaveChangesAsync()
                uow.Commit()
                Initialize()
                MsgBox("修改成功")
            Catch ex As Exception
                uow.Rollback()
                MsgBox("修改時發生錯誤：" & ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub DeleteAsync()
        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.No Then Exit Sub

        Using uow As New UnitOfWork()
            Try
                uow.BeginTransaction()

                Dim orgCol = Await uow.CollectionRepository.GetByIdAsync(_currentData.col_Id)
                If orgCol Is Nothing Then Throw New Exception("找不到要更新的收款資料，可能已被刪除")

                Dim payType = orgCol.col_Type

                ' 銷帳
                _ocmSer.DeleteCollection(orgCol.col_Id)

                If payType = "應收票據" Then
                    Dim cheque = Await uow.ChequeRepository.GetByNumberAsync(orgCol.col_Cheque)
                    Await uow.ChequeRepository.DeleteAsync(cheque.che_Id)

                ElseIf payType = "銀行存款" Then
                    Await _bmbService.UpdateMonthBalanceAsync(orgCol.col_bank_Id, orgCol.col_AccountMonth)

                    ' 更新支票資訊
                    Dim cheque = Await uow.ChequeRepository.GetByNumberAsync(orgCol.col_Cheque)

                    If cheque IsNot Nothing Then
                        cheque.che_CashingDate = Nothing
                        cheque.chu_State = "已代收"
                    End If
                End If

                '刪除資料
                _aeSer.DeleteEntries("收款作業", orgCol.col_Id)
                Await uow.CollectionRepository.DeleteAsync(orgCol)

                Await uow.SaveChangesAsync()
                uow.Commit()
                Initialize()
                MsgBox("刪除成功")
            Catch ex As Exception
                uow.Rollback()
                MsgBox("刪除時發生錯誤：" & ex.Message)
            End Try
        End Using
    End Sub

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