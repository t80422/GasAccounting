Public Class BankMonthlyBalanceService
    Implements IBankMonthlyBalanceService

    Private ReadOnly _bmbRep As IBankMonthlyBalancesRep
    Private ReadOnly _bankRep As IBankRep
    Private ReadOnly _paymentRep As IPaymentRep
    Private ReadOnly _collectionRep As ICollectionRep

    Public Sub New(bmbRep As IBankMonthlyBalancesRep, bankRep As IBankRep, paymentRep As IPaymentRep, collectionRep As ICollectionRep)
        _bmbRep = bmbRep
        _bankRep = bankRep
        _paymentRep = paymentRep
        _collectionRep = collectionRep
    End Sub

    Private Async Function UpdateMonthBalanceAsync(bankId As Integer, inputMonth As Date) As Task Implements IBankMonthlyBalanceService.UpdateMonthBalanceAsync
        Dim month = New Date(inputMonth.Year, inputMonth.Month, 1)

        '取得payment該銀行帳戶這個月的支付金額
        Dim thisMonthPayments = Await _paymentRep.GetByBankAndMonthAsync(bankId, month)
        Dim totalCredit = thisMonthPayments.Sum(Function(x) x.p_Amount)

        '取得collection該銀行帳戶這個月的收取金額
        Dim thisMonthCollection = Await _collectionRep.GetByBankAndMonthAsync(bankId, month)
        Dim totalDebit = thisMonthCollection.Sum(Function(x) x.col_Amount)

        '取得上期結餘,若沒有則取得該銀行帳戶的初始金額
        Dim lastClosingBalance As Integer
        Dim lastBalance = Await _bmbRep.GetLastBalanceBeforeMonthAsync(month, bankId)

        If lastBalance IsNot Nothing Then
            lastClosingBalance = lastBalance.bm_ClosingBalance
        Else
            Dim bank = Await _bankRep.GetByIdAsync(bankId)
            lastClosingBalance = bank.bank_InitialBalance
        End If

        '更新月結餘額資料表
        Dim newBmb = New bank_monthly_balances With {
            .bm_bank_Id = bankId,
            .bm_Month = month,
            .bm_TotalCredit = totalCredit,
            .bm_TotalDebit = totalDebit,
            .bm_OpeningBalance = lastClosingBalance,
            .bm_ClosingBalance = lastClosingBalance + totalDebit - totalCredit
        }
        Await _bmbRep.UpdateBankMonthlyBalancesAsync(newBmb)
    End Function

    ' === 新方法（接受 Repository 參數，給 UnitOfWork 使用） ===

    Public Async Function UpdateMonthBalanceAsync(bmbRep As IBankMonthlyBalancesRep, bankRep As IBankRep, paymentRep As IPaymentRep, collectionRep As ICollectionRep, bankId As Integer, inputMonth As Date) As Task Implements IBankMonthlyBalanceService.UpdateMonthBalanceAsync
        Dim month = New Date(inputMonth.Year, inputMonth.Month, 1)

        '取得payment該銀行帳戶這個月的支付金額
        Dim thisMonthPayments = Await paymentRep.GetByBankAndMonthAsync(bankId, month)
        Dim totalCredit = thisMonthPayments.Sum(Function(x) x.p_Amount)

        '取得collection該銀行帳戶這個月的收取金額
        Dim thisMonthCollection = Await collectionRep.GetByBankAndMonthAsync(bankId, month)
        Dim totalDebit = thisMonthCollection.Sum(Function(x) x.col_Amount)

        '取得上期結餘,若沒有則取得該銀行帳戶的初始金額
        Dim lastClosingBalance As Integer
        Dim lastBalance = Await bmbRep.GetLastBalanceBeforeMonthAsync(month, bankId)

        If lastBalance IsNot Nothing Then
            lastClosingBalance = lastBalance.bm_ClosingBalance
        Else
            Dim bank = Await bankRep.GetByIdAsync(bankId)
            lastClosingBalance = bank.bank_InitialBalance
        End If

        '更新月結餘額資料表
        Dim newBmb = New bank_monthly_balances With {
            .bm_bank_Id = bankId,
            .bm_Month = month,
            .bm_TotalCredit = totalCredit,
            .bm_TotalDebit = totalDebit,
            .bm_OpeningBalance = lastClosingBalance,
            .bm_ClosingBalance = lastClosingBalance + totalDebit - totalCredit
        }
        Await bmbRep.UpdateBankMonthlyBalancesAsync(newBmb)
    End Function
End Class
