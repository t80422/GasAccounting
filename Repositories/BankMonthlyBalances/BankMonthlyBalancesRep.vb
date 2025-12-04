Imports System.Data.Entity

Public Class BankMonthlyBalancesRep
    Inherits Repository(Of bank_monthly_balances)
    Implements IBankMonthlyBalancesRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function UpdateBankMonthlyBalancesAsync(bmb As bank_monthly_balances) As Task Implements IBankMonthlyBalancesRep.UpdateBankMonthlyBalancesAsync
        Try
            Dim currentMonthBmb = Await GetByMonthAndBankAsync(bmb.bm_Month, bmb.bm_bank_Id)

            If currentMonthBmb IsNot Nothing Then
                '更新當前月份
                currentMonthBmb.bm_ClosingBalance = bmb.bm_ClosingBalance
                currentMonthBmb.bm_OpeningBalance = bmb.bm_OpeningBalance
                currentMonthBmb.bm_TotalCredit = bmb.bm_TotalCredit
                currentMonthBmb.bm_TotalDebit = bmb.bm_TotalDebit
            Else
                '新增當前月份
                Await AddAsync(bmb)
            End If

            '更新後續月份
            Dim subsequentMonths = Await GetSubsequentMonthAsync(bmb.bm_Month, bmb.bm_bank_Id)
            Dim previousClosingBalance = bmb.bm_ClosingBalance

            For Each subsequentBmb In subsequentMonths
                subsequentBmb.bm_OpeningBalance = previousClosingBalance
                subsequentBmb.bm_ClosingBalance = previousClosingBalance + subsequentBmb.bm_TotalDebit - subsequentBmb.bm_TotalCredit
                previousClosingBalance = subsequentBmb.bm_ClosingBalance
            Next

            Await SaveChangesAsync()
        Catch ex As Exception
            Throw New Exception("更新銀行月結餘額資料表發生錯誤", ex)
        End Try
    End Function

    Public Async Function GetByMonthAndBankAsync(month As Date, bankId As Integer) As Task(Of bank_monthly_balances) Implements IBankMonthlyBalancesRep.GetByMonthAndBankAsync
        Try
            Return Await _dbSet.FirstOrDefaultAsync(Function(x) x.bm_Month = month AndAlso x.bm_bank_Id = bankId)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetSubsequentMonthAsync(month As Date, bankId As Integer) As Task(Of IEnumerable(Of bank_monthly_balances)) Implements IBankMonthlyBalancesRep.GetSubsequentMonthAsync
        Try
            Return Await _dbSet.Where(Function(x) x.bm_Month > month AndAlso x.bm_bank_Id = bankId).
                    OrderBy(Function(x) x.bm_Month).
                    ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetLastBalanceBeforeMonthAsync(startMonth As Date, bankId As Integer) As Task(Of bank_monthly_balances) Implements IBankMonthlyBalancesRep.GetLastBalanceBeforeMonthAsync
        Try
            Return _dbSet.Where(Function(x) x.bm_bank_Id = bankId AndAlso x.bm_Month < startMonth).
                          OrderByDescending(Function(x) x.bm_Month).
                          FirstOrDefaultAsync
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetAllByBankAsync(bankId As Integer) As Task(Of IEnumerable(Of bank_monthly_balances)) Implements IBankMonthlyBalancesRep.GetAllByBankAsync
        Try
            ' 不使用 AsNoTracking，確保實體被追蹤，以便後續可以刪除
            Return Await _dbSet.Where(Function(x) x.bm_bank_Id = bankId).ToListAsync()
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class