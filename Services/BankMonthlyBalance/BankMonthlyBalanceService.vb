Public Class BankMonthlyBalanceService
    Implements IBankMonthlyBalanceService

    Public Async Function UpdateMonthBalanceAsync(bmbRep As IBankMonthlyBalancesRep, bankRep As IBankRep, paymentRep As IPaymentRep, collectionRep As ICollectionRep, bankId As Integer, inputMonth As Date) As Task Implements IBankMonthlyBalanceService.UpdateMonthBalanceAsync
        Dim monthStart = New Date(inputMonth.Year, inputMonth.Month, 1)
        Dim monthEnd = monthStart.AddMonths(1)

        ' 當月借方：現金存銀行 + 銀行存款收入
        Dim monthCashToBank = Await paymentRep.GetCashToBankTransfersByDateRangeAsync(bankId, monthStart, monthEnd)
        Dim monthDeposits = Await collectionRep.GetBankDepositsByDateRangeAsync(bankId, monthStart, monthEnd)
        Dim totalDebit As Decimal = monthCashToBank.Sum(Function(x) x.p_Amount) + monthDeposits.Sum(Function(x) x.col_Amount)

        ' 當月貸方：現金提銀行 + 銀行存款付款
        Dim monthBankToCash = Await collectionRep.GetCashToBankTransfersByDateRangeAsync(bankId, monthStart, monthEnd)
        Dim monthPayments = Await paymentRep.GetBankPaymentsByDateRangeAsync(bankId, monthStart, monthEnd)
        Dim totalCredit As Decimal = monthBankToCash.Sum(Function(x) x.col_Amount) + monthPayments.Sum(Function(x) x.p_Amount)

        ' 取得期初餘額（上期結餘 + 上期結餘日後至本月初的交易）

        Dim lastBalance = Await bmbRep.GetLastBalanceBeforeMonthAsync(monthStart, bankId)
        Dim openingBalance = If(lastBalance IsNot Nothing, lastBalance.bm_ClosingBalance, bankRep.GetByIdAsync(bankId).Result.bank_InitialBalance)

        'If lastBalance IsNot Nothing Then
        '    openingBalance = lastBalance.bm_ClosingBalance

        '    Dim lastMonthEnd = New Date(lastBalance.bm_Month.Year,
        '                               lastBalance.bm_Month.Month,
        '                               Date.DaysInMonth(lastBalance.bm_Month.Year, lastBalance.bm_Month.Month))
        '    Dim preStart = lastMonthEnd.AddDays(1)
        '    Dim preEnd = monthStart

        '    If preStart < preEnd Then
        '        Dim preDeposits = Await collectionRep.GetBankDepositsByDateRangeAsync(bankId, preStart, preEnd)
        '        Dim preCashToBank = Await collectionRep.GetCashToBankTransfersByDateRangeAsync(bankId, preStart, preEnd)
        '        Dim prePayments = Await paymentRep.GetBankPaymentsByDateRangeAsync(bankId, preStart, preEnd)

        '        Dim preDebit = preDeposits.Sum(Function(x) x.col_Amount)
        '        Dim preCredit = preCashToBank.Sum(Function(x) x.col_Amount) + prePayments.Sum(Function(x) x.p_Amount)
        '        openingBalance += preDebit - preCredit
        '    End If
        'Else
        '    Dim bank = Await bankRep.GetByIdAsync(bankId)
        '    openingBalance = bank.bank_InitialBalance
        'End If

        ' 更新月結餘額資料表
        Dim newBmb = New bank_monthly_balances With {
            .bm_bank_Id = bankId,
            .bm_Month = monthStart,
            .bm_TotalCredit = totalCredit,
            .bm_TotalDebit = totalDebit,
            .bm_OpeningBalance = openingBalance,
            .bm_ClosingBalance = openingBalance + totalDebit - totalCredit
        }
        Await bmbRep.UpdateBankMonthlyBalancesAsync(newBmb)
    End Function

    ''' <summary>
    ''' 增量更新銀行月結餘額（Delta Update）- 根據變化量調整餘額，不需要重新計算整個月份
    ''' </summary>
    Public Async Function UpdateMonthBalanceIncrementalAsync(
        bmbRep As IBankMonthlyBalancesRep,
        bankRep As IBankRep,
        bankId As Integer,
        transactionMonth As Date,
        creditDelta As Decimal,
        debitDelta As Decimal
    ) As Task Implements IBankMonthlyBalanceService.UpdateMonthBalanceIncrementalAsync
        Try
            Dim month = New Date(transactionMonth.Year, transactionMonth.Month, 1)

            ' 取得或建立該月記錄
            Dim bmb = Await bmbRep.GetByMonthAndBankAsync(month, bankId)

            If bmb Is Nothing Then
                ' 首次建立該月記錄，需要取得上期結餘
                Dim lastBalance = Await bmbRep.GetLastBalanceBeforeMonthAsync(month, bankId)
                Dim openingBalance As Decimal = 0

                If lastBalance IsNot Nothing Then
                    openingBalance = lastBalance.bm_ClosingBalance
                Else
                    Dim bank = Await bankRep.GetByIdAsync(bankId)
                    openingBalance = bank.bank_InitialBalance
                End If

                bmb = New bank_monthly_balances With {
                    .bm_bank_Id = bankId,
                    .bm_Month = month,
                    .bm_OpeningBalance = openingBalance,
                    .bm_TotalCredit = 0,
                    .bm_TotalDebit = 0,
                    .bm_ClosingBalance = openingBalance
                }
                Await bmbRep.AddAsync(bmb)
            End If

            ' 增量調整
            bmb.bm_TotalCredit += creditDelta
            bmb.bm_TotalDebit += debitDelta
            bmb.bm_ClosingBalance = bmb.bm_OpeningBalance + bmb.bm_TotalDebit - bmb.bm_TotalCredit

            ' 更新後續月份的期初/期末餘額（連鎖反應）
            Await UpdateSubsequentMonthsAsync(bmbRep, bankId, month, bmb.bm_ClosingBalance)
        Catch ex As Exception
            Throw New Exception("增量更新銀行月結餘額時發生錯誤", ex)
        End Try
    End Function

    ''' <summary>
    ''' 更新後續月份的期初期末餘額（連鎖效應）
    ''' </summary>
    Private Async Function UpdateSubsequentMonthsAsync(
        bmbRep As IBankMonthlyBalancesRep,
        bankId As Integer,
        startMonth As Date,
        newClosingBalance As Decimal
    ) As Task
        Try
            Dim subsequentMonths = Await bmbRep.GetSubsequentMonthAsync(startMonth, bankId)
            Dim previousClosing = newClosingBalance

            For Each nextMonth In subsequentMonths
                nextMonth.bm_OpeningBalance = previousClosing
                nextMonth.bm_ClosingBalance = previousClosing + nextMonth.bm_TotalDebit - nextMonth.bm_TotalCredit
                previousClosing = nextMonth.bm_ClosingBalance
            Next
        Catch ex As Exception
            Throw New Exception("更新後續月份餘額時發生錯誤", ex)
        End Try
    End Function

    ''' <summary>
    ''' 重新計算指定銀行的所有月結餘額（從頭開始全量計算）
    ''' </summary>
    Public Async Function RecalculateBankBalancesAsync(
        bmbRep As IBankMonthlyBalancesRep,
        bankRep As IBankRep,
        paymentRep As IPaymentRep,
        collectionRep As ICollectionRep,
        bankId As Integer
    ) As Task Implements IBankMonthlyBalanceService.RecalculateBankBalancesAsync
        Try
            ' 取得該銀行所有交易月份（payment 和 collection 的月份聯集）
            Dim allMonths As New HashSet(Of Date)

            ' 從 payment 取得月份
            Dim paymentMonths = paymentRep.GetBankAccount(bankId).Select(Function(x) New Date(x.p_Date.Year, x.p_Date.Month, 1)).
                                                                  Distinct.
                                                                  ToList

            For Each m In paymentMonths
                allMonths.Add(m)
            Next

            ' 從 collection 取得月份
            Dim collectionMonths = collectionRep.GetBankAccount(bankId).Select(Function(c) New Date(c.col_Date.Year, c.col_Date.Month, 1)).
                                                                        Distinct.
                                                                        ToList
            collectionMonths.ForEach(Sub(x) allMonths.Add(x))

            ' 如果沒有任何交易，直接返回
            If allMonths.Count = 0 Then Return

            ' 按月份排序
            Dim sortedMonths = allMonths.OrderBy(Function(m) m).ToList()

            ' 刪除該銀行的所有舊月結記錄
            Dim bankOldBalances = Await bmbRep.GetAllByBankAsync(bankId)
            bankOldBalances.ToList.ForEach(Sub(x) bmbRep.DeleteAsync(x))

            ' 逐月重新計算
            sortedMonths.ForEach(Async Sub(x) Await UpdateMonthBalanceAsync(bmbRep, bankRep, paymentRep, collectionRep, bankId, x))
        Catch ex As Exception
            Throw New Exception($"重新計算銀行 {bankId} 月結餘額時發生錯誤", ex)
        End Try
    End Function

    ''' <summary>
    ''' 重新計算所有銀行的月結餘額（維護工具）
    ''' </summary>
    Public Async Function RecalculateAllBanksAsync(
        bmbRep As IBankMonthlyBalancesRep,
        bankRep As IBankRep,
        paymentRep As IPaymentRep,
        collectionRep As ICollectionRep
    ) As Task(Of String) Implements IBankMonthlyBalanceService.RecalculateAllBanksAsync
        Try
            ' 取得所有銀行
            Dim allBanks = Await bankRep.GetAllAsync()
            Dim processedCount As Integer = 0
            Dim errorCount As Integer = 0
            Dim errorMessages As New List(Of String)

            For Each bank In allBanks
                Try
                    Await RecalculateBankBalancesAsync(bmbRep, bankRep, paymentRep, collectionRep, bank.bank_id)
                    processedCount += 1
                Catch ex As Exception
                    errorCount += 1
                    errorMessages.Add($"銀行 {bank.bank_name}({bank.bank_id}): {ex.Message}")
                End Try
            Next

            ' 建立結果訊息
            Dim resultMessage As String = $"重整完成！{vbCrLf}"
            resultMessage &= $"成功處理: {processedCount} 個銀行{vbCrLf}"

            If errorCount > 0 Then
                resultMessage &= $"失敗: {errorCount} 個銀行{vbCrLf}{vbCrLf}"
                resultMessage &= "錯誤詳情:{vbCrLf}"
                resultMessage &= String.Join(vbCrLf, errorMessages)
            End If

            Return resultMessage
        Catch ex As Exception
            Throw New Exception("重新計算所有銀行月結餘額時發生錯誤", ex)
        End Try
    End Function
End Class
