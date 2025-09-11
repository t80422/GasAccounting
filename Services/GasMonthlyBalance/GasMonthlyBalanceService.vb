Public Class GasMonthlyBalanceService
    Implements IGasMonthlyBalanceService

    Private ReadOnly _gmbRep As IGasMonthlyBalanceRep
    Private ReadOnly _ordRep As IOrderRep
    Private ReadOnly _compRep As ICompanyRep
    Private ReadOnly _bsRep As IBasicSetRep

    Public Sub New(gmbRep As IGasMonthlyBalanceRep, ordRep As IOrderRep, compRep As ICompanyRep, bsRep As IBasicSetRep)
        _gmbRep = gmbRep
        _ordRep = ordRep
        _compRep = compRep
        _bsRep = bsRep
    End Sub

    Public Async Sub UpdateOrAdd(month As Date, companyId As Integer) Implements IGasMonthlyBalanceService.UpdateOrAdd
        Try
            Dim comp = Await _compRep.GetByIdAsync(companyId)

            '取得當月所有採購資料
            Dim purDatas = comp.purchases.Where(Function(x) x.pur_date.Value.Year = month.Year AndAlso x.pur_date.Value.Month = month.Month)

            '取得當月所有銷售資料
            Dim saleDatas = _ordRep.GetByMonthAndCompany(month, companyId)

            '取得上期結餘
            Dim lastBalance = Await GetLastClosingBalanceAsync(month, companyId)

            '更新或新增月結餘資料
            Dim gmb = _gmbRep.GetByMonthAndCompany(month, companyId)
            Dim totalPurchases = purDatas.Sum(Function(x) x.pur_quantity)
            Dim totalSales = saleDatas.Sum(Function(x) x.o_gas_total + x.o_gas_c_total)
            Dim closingBalance = lastBalance + totalPurchases - totalSales
            If gmb Is Nothing Then
                Dim formatMonth = New Date(month.Year, month.Month, 1)

                Dim data = New gas_monthly_balances With {
                    .gmb_comp_Id = companyId,
                    .gmb_Month = formatMonth,
                    .gmb_OpeningBalance = lastBalance,
                    .gmb_PurchaseTotal = totalPurchases,
                    .gmb_SaleTotal = totalSales,
                    .gmb_ClosingBalance = closingBalance
                }
                Await _gmbRep.AddAsync(data)
            Else
                gmb.gmb_OpeningBalance = lastBalance
                gmb.gmb_PurchaseTotal = totalPurchases
                gmb.gmb_SaleTotal = totalSales
                gmb.gmb_ClosingBalance = closingBalance
            End If

            '更新後續月份
            UpdateSubsequentMonths(month, companyId, closingBalance)

            Await _gmbRep.SaveChangesAsync
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Async Function GetLastClosingBalanceAsync(month As Date, compId As Integer) As Task(Of Integer)
        Try
            Dim comp = Await _compRep.GetByIdAsync(compId)
            Dim formatDate = New Date(month.Year, month.Month, 1)
            Dim gmb = comp.gas_monthly_balances.Where(Function(x) x.gmb_Month < formatDate).OrderByDescending(Function(x) x.gmb_Month).FirstOrDefault
            Dim bs = _bsRep.GetAllAsync().Result.FirstOrDefault

            Return If(gmb IsNot Nothing, gmb.gmb_ClosingBalance, bs.bs_Gas)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Async Sub UpdateSubsequentMonths(month As Date, compId As Integer, currentClosingBalance As Integer)
        Try
            Dim comp = Await _compRep.GetByIdAsync(compId)
            Dim afterMonths = comp.gas_monthly_balances.Where(Function(x) x.gmb_Month > month).OrderBy(Function(x) x.gmb_Month)

            If afterMonths.Count > 0 Then
                Dim updateClosingBalance = currentClosingBalance

                For Each item In afterMonths
                    item.gmb_OpeningBalance = updateClosingBalance
                    item.gmb_ClosingBalance = updateClosingBalance + item.gmb_PurchaseTotal - item.gmb_SaleTotal
                    updateClosingBalance = item.gmb_ClosingBalance
                Next
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class
