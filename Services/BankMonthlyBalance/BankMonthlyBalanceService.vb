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
            Dim allPayments = Await paymentRep.GetAllAsync()
            Dim paymentMonths = allPayments.
                Where(Function(p) p.p_bank_Id = bankId AndAlso p.p_Type = "銀行存款").
                Select(Function(p) New Date(p.p_Date.Year, p.p_Date.Month, 1)).
                Distinct()

            For Each m In paymentMonths
                allMonths.Add(m)
            Next

            ' 從 collection 取得月份
            Dim allCollections = Await collectionRep.GetAllAsync()
            Dim collectionMonths = allCollections.
                Where(Function(c) c.col_bank_Id = bankId AndAlso c.col_Type = "銀行存款").
                Select(Function(c) New Date(c.col_AccountMonth.Year, c.col_AccountMonth.Month, 1)).
                Distinct()

            For Each m In collectionMonths
                allMonths.Add(m)
            Next

            ' 如果沒有任何交易，直接返回
            If allMonths.Count = 0 Then
                Return
            End If

            ' 按月份排序
            Dim sortedMonths = allMonths.OrderBy(Function(m) m).ToList()

            ' 刪除該銀行的所有舊月結記錄
            Dim bankOldBalances = Await bmbRep.GetAllByBankAsync(bankId)
            For Each oldBalance In bankOldBalances
                Await bmbRep.DeleteAsync(oldBalance)
            Next

            ' 逐月重新計算
            For Each m In sortedMonths
                Await UpdateMonthBalanceAsync(bmbRep, bankRep, paymentRep, collectionRep, bankId, m)
            Next
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
                    Await RecalculateBankBalancesAsync(bmbRep, bankRep, paymentRep, collectionRep, bank.bank_Id)
                    processedCount += 1
                Catch ex As Exception
                    errorCount += 1
                    errorMessages.Add($"銀行 {bank.bank_Name}({bank.bank_Id}): {ex.Message}")
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
