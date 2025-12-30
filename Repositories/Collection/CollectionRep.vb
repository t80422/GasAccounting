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
            Dim query = _dbSet.Where(Function(x) x.col_Date = day)

            Dim result = query.Where(Function(x) x.col_Type = "現金").
                Select(Function(x) New CashSubpoenaDTO With {
                    .SubjectName = x.subject.s_name,
                    .Amount = x.col_Amount,
                    .Summary = x.col_Memo,
                    .Code = x.customer.cus_code
                }).ToList

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
                                  x.col_Type = "現金" AndAlso
                                  x.col_bank_Id = bankId AndAlso
                                  x.subject.s_name = "銀行存款").
                ToListAsync()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetTarnsferSubpoenaData(day As Date) As List(Of TransferSubpoenaDTO) Implements ICollectionRep.GetTarnsferSubpoenaData
        Try
            Dim query = _dbSet.Where(Function(x) x.col_Date = day)

            Dim result = query.Where(Function(x) x.col_Type = "銀行存款" OrElse x.col_Type = "應收票據").
                Select(Function(x) New TransferSubpoenaDTO With {
                    .CreditAmount = x.col_Amount,
                    .CreditSubjectName = x.subject.s_name,
                    .CreditSummary = If(x.customer IsNot Nothing, x.customer.cus_code, "") & x.col_Memo,
                    .DebitAmount = x.col_Amount,
                    .DebitSubjectName = x.col_Type
                }).ToList

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetBankAccount(bankId As Integer) As IEnumerable(Of collection) Implements ICollectionRep.GetBankAccount
        Try
            Return _dbSet.Where(Function(x) x.col_bank_Id = bankId AndAlso (x.col_Type = "銀行存款" OrElse x.subject.s_name = "銀行存款"))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetBankDepositsByAccountMonth(bankId As Integer, month As Date) As IEnumerable(Of collection) Implements ICollectionRep.GetBankDepositsByAccountMonth
        Try
            Return _dbSet.AsNoTracking.Where(Function(x) x.col_AccountMonth.Year = month.Year AndAlso
                                                        x.col_AccountMonth.Month = month.Month AndAlso
                                                        x.col_bank_Id = bankId AndAlso
                                                        x.col_Type = "銀行存款")
        Catch ex As Exception

        End Try
    End Function

    Public Function GetCashToBankTransfersByAccountMonth(bankId As Integer, month As Date) As IEnumerable(Of collection) Implements ICollectionRep.GetCashToBankTransfersByAccountMonth
        Try
            Return _dbSet.AsNoTracking.Where(Function(x) x.col_AccountMonth.Year = month.Year AndAlso
                                                         x.col_AccountMonth.Month = month.Month AndAlso
                                                         x.col_bank_Id = bankId AndAlso
                                                         x.col_Type = "現金" AndAlso
                                                         x.subject.s_name = "銀行存款")
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class