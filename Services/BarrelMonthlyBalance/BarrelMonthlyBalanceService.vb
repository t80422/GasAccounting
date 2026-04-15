Public Class BarrelMonthlyBalanceService
    Implements IBarrelMonthlyBalanceService

    Private ReadOnly _barMBRep As IBarrelMonthlyBalancesRep
    Private ReadOnly _gbRep As IGasBarrelRep
    Private ReadOnly _pbRep As IPurchaseBarrelRep
    Private ReadOnly _orderRep As IOrderRep

    Public Sub New(barMBRep As IBarrelMonthlyBalancesRep, gbRep As IGasBarrelRep, pbRep As IPurchaseBarrelRep, orderRep As IOrderRep)
        _barMBRep = barMBRep
        _gbRep = gbRep
        _pbRep = pbRep
        _orderRep = orderRep
    End Sub

    Public Async Function UpdateOrAddAsync(month As Date) As Task Implements IBarrelMonthlyBalanceService.UpdateOrAddAsync
        Try
            Dim thisMonth = New Date(month.Year, month.Month, 1)

            '取得當月所有新瓶採購資料
            Dim purDatas = Await _pbRep.GetByMonthAsync(thisMonth)

            '取得當月所有瓦斯瓶銷售資料
            Dim orderData = _orderRep.GetByMonth(thisMonth)

            '獲取所有瓦斯瓶類型
            Dim allBarrelTypes = Await _gbRep.GetAllAsync

            For Each barrelType In allBarrelTypes
                Dim gbId = barrelType.gb_Id

                '計算當月該類型瓦斯桶總採購量
                Dim kg = barrelType.gb_Name.Replace("Kg", "")
                Dim barrelTotals = CalculateTotalPurchase(purDatas, kg) - CalculateTotalOrder(orderData, kg)

                '取得上期結餘
                Dim lastClosingBalance = Await GetLastClosingBalance(thisMonth, gbId)

                '更新或新增月結餘額資料
                Dim barMB = Await _barMBRep.GetByMonthAndGbIdAsync(thisMonth, gbId)
                Dim closingBalance As Integer = lastClosingBalance + barrelTotals

                If barMB Is Nothing Then
                    Dim newBarMB = New barrel_monthly_balances With {
                        .barmb_gb_Id = barrelType.gb_Id,
                        .barmb_Month = thisMonth,
                        .barmb_OpeningBalance = lastClosingBalance,
                        .barmb_Total = barrelTotals,
                        .barmb_ClosingBalance = closingBalance
                    }

                    _barMBRep.AddEntityOnly(newBarMB)
                Else
                    barMB.barmb_OpeningBalance = lastClosingBalance
                    barMB.barmb_Total = barrelTotals
                    barMB.barmb_ClosingBalance = closingBalance
                End If

                '更新後續月份
                Await UpdateSubsequentMonths(thisMonth, gbId, closingBalance)
            Next

            Await _barMBRep.SaveChangesAsync()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Async Function UpdateSubsequentMonths(startMonth As Date, gbId As Integer, initialClosingBalance As Integer) As Task
        Try
            Dim afterMonth = Await _barMBRep.GetAfterByMonthAndGbIdAsync(startMonth, gbId)

            If afterMonth.Count > 0 Then
                Dim updateClosingBalance = initialClosingBalance

                For Each d In afterMonth
                    d.barmb_OpeningBalance = updateClosingBalance
                    d.barmb_ClosingBalance = updateClosingBalance + d.barmb_Total
                    updateClosingBalance = d.barmb_ClosingBalance
                Next
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Async Function GetLastClosingBalance(month As Date, gbId As Integer) As Task(Of Integer)
        Dim lastBalance = Await _barMBRep.GetLastClosingBalance(month, gbId)

        If lastBalance.HasValue Then
            Return lastBalance.Value
        Else
            Dim barrelType = Await _gbRep.GetByIdAsync(gbId)
            Return barrelType.gb_InitialInventory
        End If
    End Function

    Public Async Function RecalculateAllMonthsAsync(pbRep As IPurchaseBarrelRep, orderRep As IOrderRep) As Task Implements IBarrelMonthlyBalanceService.RecalculateAllMonthsAsync
        Dim allPurchases = Await pbRep.GetAllAsync()
        Dim allOrders = Await orderRep.GetAllAsync()

        Dim hasPurchase = allPurchases IsNot Nothing AndAlso allPurchases.Any()
        Dim hasOrder = allOrders IsNot Nothing AndAlso allOrders.Any()

        If Not hasPurchase AndAlso Not hasOrder Then Return

        Dim minDate As Date = Date.MaxValue
        Dim maxDate As Date = Date.MinValue

        If hasPurchase Then
            Dim pbMin = allPurchases.Min(Function(x) x.pb_Date)
            Dim pbMax = allPurchases.Max(Function(x) x.pb_Date)
            If pbMin < minDate Then minDate = pbMin
            If pbMax > maxDate Then maxDate = pbMax
        End If

        If hasOrder Then
            Dim ordMin = allOrders.Where(Function(x) x.o_date.HasValue).Min(Function(x) x.o_date.Value)
            Dim ordMax = allOrders.Where(Function(x) x.o_date.HasValue).Max(Function(x) x.o_date.Value)
            If ordMin < minDate Then minDate = ordMin
            If ordMax > maxDate Then maxDate = ordMax
        End If

        Dim startMonth As New Date(minDate.Year, minDate.Month, 1)
        Dim endMonth As New Date(maxDate.Year, maxDate.Month, 1)

        Dim current = startMonth
        While current <= endMonth
            Await UpdateOrAddAsync(current)
            current = current.AddMonths(1)
        End While
    End Function

    Private Function CalculateTotalPurchase(purchases As IEnumerable(Of purchase_barrel), kg As Integer) As Integer
        Dim propertyName = $"pb_Qty_{kg}"
        Return purchases.Sum(Function(x) x.GetType.GetProperty(propertyName).GetValue(x, Nothing))
    End Function

    Private Function CalculateTotalOrder(order As IEnumerable(Of order), kg As Integer) As Integer
        Dim propertyName = $"o_new_in_{kg}"
        Return order.Sum(Function(x) x.GetType.GetProperty(propertyName).GetValue(x, Nothing))
    End Function
End Class
