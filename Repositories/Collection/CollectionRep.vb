Imports System.Data.Entity

Public Class CollectionRep
    Inherits Repository(Of collection)
    Implements ICollectionRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetList(Optional criteria As CollectionSearchCriteria = Nothing) As List(Of CollectionVM) Implements ICollectionRep.GetList
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If criteria IsNot Nothing Then
                ' 日期範圍
                If criteria.IsDate Then
                    Dim startDate = criteria.StartDate.Value.Date
                    Dim endDate = criteria.EndDate.Value.Date.AddDays(1)
                    query = query.Where(Function(x) x.col_Date >= startDate AndAlso x.col_Date < endDate)
                End If

                ' 客戶編號
                If criteria.CusId <> 0 Then query = query.Where(Function(x) x.col_cus_Id = criteria.CusId)

                ' 科目編號
                If criteria.SubjectId.HasValue Then query = query.Where(Function(x) x.col_s_Id = criteria.SubjectId)

                ' 收款類型
                If Not String.IsNullOrEmpty(criteria.Type) Then query = query.Where(Function(x) x.col_Type = criteria.Type)

                ' 支票號碼
                If Not String.IsNullOrEmpty(criteria.ChequeNum) Then query = query.Where(Function(x) x.col_Cheque = criteria.ChequeNum)

                ' 銀行編號
                If criteria.BankId IsNot Nothing Then query = query.Where(Function(x) x.col_bank_Id = criteria.BankId)

                ' 帳款月份
                If criteria.IsAccountMonth Then query = query.Where(Function(x) x.col_AccountMonth.Year = criteria.AccountMonth.Year AndAlso
                                                                                    x.col_AccountMonth.Month = criteria.AccountMonth.Month)
            Else
                query = query.Where(Function(x) x.col_Date.Year = Now.Year AndAlso x.col_Date.Month = Now.Month)
            End If

            Return query.OrderByDescending(Function(x) x.col_Date).
                         ThenByDescending(Function(x) x.col_Id).ToList.
                         Select(Function(x) New CollectionVM(x)).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetCashSubpoenaData(day As Date) As List(Of CashSubpoenaDTO) Implements ICollectionRep.GetCashSubpoenaData
        Try
            Dim collections = _dbSet.Where(Function(x) x.col_Date = day AndAlso x.col_Type = "現金").ToList()

            Dim result As New List(Of CashSubpoenaDTO)

            For Each x In collections
                ' 第一組：原本的科目與金額 1
                If x.col_credit_amount_1.GetValueOrDefault() > 0 Then
                    result.Add(New CashSubpoenaDTO With {
                        .SubjectName = x.subject1?.s_name,
                        .Amount = x.col_credit_amount_1.Value,
                        .Summary = x.col_Memo,
                        .Code = x.customer?.cus_code
                    })
                End If

                ' 第二組：科目 2 與金額 2
                If x.col_credit_amount_2.GetValueOrDefault() > 0 Then
                    result.Add(New CashSubpoenaDTO With {
                        .SubjectName = x.subject2?.s_name,
                        .Amount = x.col_credit_amount_2.Value,
                        .Summary = x.col_Memo,
                        .Code = x.customer?.cus_code
                    })
                End If

                ' 第三組：科目 1 (對應 s_Id_3) 與金額 3
                If x.col_credit_amount_3.GetValueOrDefault() > 0 Then
                    result.Add(New CashSubpoenaDTO With {
                        .SubjectName = x.subject?.s_name,
                        .Amount = x.col_credit_amount_3.Value,
                        .Summary = x.col_Memo,
                        .Code = x.customer?.cus_code
                    })
                End If
            Next

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetBankDepositsByDateRangeAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of IEnumerable(Of collection)) Implements ICollectionRep.GetBankDepositsByDateRangeAsync
        Try
            Return Await _dbSet.AsNoTracking.
                Where(Function(x) x.col_Date >= startDate AndAlso
                                  x.col_Date < endDate AndAlso
                                  x.col_Type = "銀行存款" AndAlso
                                  x.col_bank_Id = bankId).
                ToListAsync()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetCashToBankTransfersByDateRangeAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of IEnumerable(Of collection)) Implements ICollectionRep.GetCashToBankTransfersByDateRangeAsync
        Try
            Return Await _dbSet.AsNoTracking.
                Where(Function(x) x.col_Date >= startDate AndAlso
                                  x.col_Date < endDate AndAlso
                                  x.col_credit_bank_id = bankId AndAlso
                                  x.subject.s_name = "銀行存款").
                ToListAsync()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetTarnsferSubpoenaData(day As Date) As List(Of TransferSubpoenaDTO) Implements ICollectionRep.GetTarnsferSubpoenaData
        Try
            ' 轉帳傳票範圍：銀行存款、應收票據、轉帳折讓
            Dim collections = _dbSet.Where(Function(x) x.col_Date = day AndAlso
                                          (x.col_Type = "銀行存款" OrElse x.col_Type = "應收票據" OrElse x.col_Type = "銷貨折讓")).ToList()

            Dim result As New List(Of TransferSubpoenaDTO)

            For Each x In collections
                Dim commonDebitSummary = If(x.customer IsNot Nothing, x.customer.cus_code, "") & x.col_Memo

                ' 第一組：金額 1 -> subject1
                If x.col_credit_amount_1.GetValueOrDefault() > 0 Then
                    result.Add(New TransferSubpoenaDTO With {
                        .DebitSubjectName = x.col_Type,
                        .DebitAmount = x.col_credit_amount_1.Value,
                        .DebitSummary = commonDebitSummary,
                        .CreditSubjectName = x.subject1?.s_name,
                        .CreditAmount = x.col_credit_amount_1.Value,
                        .CreditSummary = commonDebitSummary
                    })
                End If

                ' 第二組：金額 2 -> subject2
                If x.col_credit_amount_2.GetValueOrDefault() > 0 Then
                    result.Add(New TransferSubpoenaDTO With {
                        .DebitSubjectName = x.col_Type,
                        .DebitAmount = x.col_credit_amount_2.Value,
                        .DebitSummary = commonDebitSummary,
                        .CreditSubjectName = x.subject2?.s_name,
                        .CreditAmount = x.col_credit_amount_2.Value,
                        .CreditSummary = commonDebitSummary
                    })
                End If

                ' 第三組：金額 3 -> subject
                If x.col_credit_amount_3.GetValueOrDefault() > 0 Then
                    result.Add(New TransferSubpoenaDTO With {
                        .DebitSubjectName = x.col_Type,
                        .DebitAmount = x.col_credit_amount_3.Value,
                        .DebitSummary = commonDebitSummary,
                        .CreditSubjectName = x.subject?.s_name,
                        .CreditAmount = x.col_credit_amount_3.Value,
                        .CreditSummary = commonDebitSummary
                    })
                End If
            Next

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetBankAccount(bankId As Integer) As IEnumerable(Of collection) Implements ICollectionRep.GetBankAccount
        Try
            Return _dbSet.Where(Function(x) (x.col_bank_Id = bankId AndAlso x.col_Type = "銀行存款") Or
                                            (x.col_credit_bank_id = bankId AndAlso x.subject.s_name = "銀行存款") Or
                                            (x.col_credit_bank_id_2 = bankId AndAlso x.subject2.s_name = "銀行存款") Or
                                            (x.col_credit_bank_id_3 = bankId AndAlso x.subject1.s_name = "銀行存款"))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetBankMainDepositSumAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of Integer) Implements ICollectionRep.GetBankMainDepositSumAsync
        Try
            Dim total = Await _dbSet.AsNoTracking.
                Where(Function(x) x.col_Date >= startDate AndAlso
                                  x.col_Date < endDate AndAlso
                                  x.col_Type = "銀行存款" AndAlso
                                  x.col_bank_Id = bankId).
                SumAsync(Function(x) x.col_Amount)
            Return total
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Async Function GetBankSideWithdrawalSumAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of Integer) Implements ICollectionRep.GetBankSideWithdrawalSumAsync
        Try
            ' 加總三個拆分欄位中，科目為銀行存款且銀行 ID 符合的部分
            ' 分開計算避免 LINQ 翻譯複雜度過高

            Dim sum1 = Await _dbSet.AsNoTracking.
                Where(Function(x) x.col_Date >= startDate AndAlso
                                  x.col_Date < endDate AndAlso
                                  x.col_credit_bank_id = bankId AndAlso
                                  x.subject.s_name = "銀行存款").
                SumAsync(Function(x) CType(x.col_credit_amount_1, Integer?))

            Dim sum2 = Await _dbSet.AsNoTracking.
                Where(Function(x) x.col_Date >= startDate AndAlso
                                  x.col_Date < endDate AndAlso
                                  x.col_credit_bank_id_2 = bankId AndAlso
                                  x.subject2.s_name = "銀行存款").
                SumAsync(Function(x) CType(x.col_credit_amount_2, Integer?))

            Dim sum3 = Await _dbSet.AsNoTracking.
                Where(Function(x) x.col_Date >= startDate AndAlso
                                  x.col_Date < endDate AndAlso
                                  x.col_credit_bank_id_3 = bankId AndAlso
                                  x.subject1.s_name = "銀行存款").
                SumAsync(Function(x) CType(x.col_credit_amount_3, Integer?))

            Return sum1.GetValueOrDefault() + sum2.GetValueOrDefault() + sum3.GetValueOrDefault()
        Catch ex As Exception
            Return 0
        End Try
    End Function
End Class