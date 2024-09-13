Public Class ReportRep
    Implements IReportRep

    Private _context As gas_accounting_systemEntities

    Public Sub New(context As gas_accounting_systemEntities)
        _context = context
    End Sub

    Public Function CustomersGasDetailByDay(d As Date) As List(Of CustomersGasDetailByDay) Implements IReportRep.CustomersGasDetailByDay
        Dim result As New List(Of CustomersGasDetailByDay)

        Try
            Using db As New gas_accounting_systemEntities
                '獲取所有客戶資料
                Dim customers = db.customers.ToList

                '遍歷每個客戶並蒐集相關資料
                For Each cus In customers
                    Dim detail As New CustomersGasDetailByDay With {
                        .客戶名稱 = cus.cus_name
                    }

                    detail.存氣 = cus.cus_GasStock + cus.cus_GasCStock

                    Dim startDay = d.Date
                    Dim endDay = d.Date.AddDays(1)
                    Dim ordersToday = db.orders.Where(Function(x) x.car.c_cus_id = cus.cus_id And x.o_date.Value >= startDay And x.o_date.Value < endDay).ToList
                    detail.本日提量 = ordersToday.Sum(Function(x) x.o_gas_total + x.o_gas_c_total)
                    detail.本日氣款 = ordersToday.Sum(Function(x) x.o_total_amount)

                    Dim startMonth = New Date(d.Year, d.Month, 1)
                    Dim ordersByMonth = db.orders.Where(Function(x) x.car.c_cus_id = cus.cus_id And x.o_date >= startMonth And x.o_date.Value < endDay).ToList
                    detail.當月累計提量 = ordersByMonth.Sum(Function(x) x.o_gas_total + x.o_gas_c_total)

                    Dim collectToday = db.collections.Where(Function(x) x.col_cus_Id = cus.cus_id And x.col_Date >= startDay And x.col_Date < endDay).ToList
                    detail.本日收款 = collectToday.Sum(Function(x) x.col_Amount)

                    Dim collectByMonth = db.collections.Where(Function(x) x.col_cus_Id = cus.cus_id And x.col_Date >= startMonth And x.col_Date <= endDay).ToList()
                    Dim totalCollectAmountByMonth = collectByMonth.Sum(Function(x) x.col_Amount)
                    Dim totalOrderAmountByMonth = ordersByMonth.Sum(Function(x) x.o_total_amount)
                    detail.結欠 = totalOrderAmountByMonth - totalCollectAmountByMonth

                    result.Add(detail)
                Next
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw
        End Try

        Return result
    End Function

    Public Function CustomersGetGasList(d As Date) As List(Of CusGetGas) Implements IReportRep.CustomersGetGasList
        Dim result As New List(Of CusGetGas)

        Try
            Using db As New gas_accounting_systemEntities
                '獲取所有客戶資料
                Dim customers = db.customers.ToList

                '遍歷每個客戶並蒐集相關資料
                For Each cus In customers
                    Dim detail As New CusGetGas With {
                        .客戶名稱 = cus.cus_name
                    }

                    Dim startDay = d.Date
                    Dim endDay = d.Date.AddDays(1)
                    Dim ordersToday = db.orders.Where(Function(x) x.car.c_cus_id = cus.cus_id And x.o_date.Value >= startDay And x.o_date.Value < endDay).ToList

                    detail.普氣50Kg = ordersToday.Sum(Function(x) x.o_gas_50)
                    detail.丙氣50Kg = ordersToday.Sum(Function(x) x.o_gas_c_50)

                    detail.普氣20Kg = ordersToday.Sum(Function(x) x.o_gas_20)
                    detail.丙氣20Kg = ordersToday.Sum(Function(x) x.o_gas_c_20)

                    detail.普氣16Kg = ordersToday.Sum(Function(x) x.o_gas_16)
                    detail.丙氣16Kg = ordersToday.Sum(Function(x) x.o_gas_c_16)

                    detail.普氣10Kg = ordersToday.Sum(Function(x) x.o_gas_10)
                    detail.丙氣10Kg = ordersToday.Sum(Function(x) x.o_gas_c_10)

                    detail.普氣4Kg = ordersToday.Sum(Function(x) x.o_gas_4)
                    detail.丙氣4Kg = ordersToday.Sum(Function(x) x.o_gas_c_4)

                    detail.普氣15Kg = ordersToday.Sum(Function(x) x.o_gas_15)
                    detail.丙氣15Kg = ordersToday.Sum(Function(x) x.o_gas_c_15)

                    detail.普氣14Kg = ordersToday.Sum(Function(x) x.o_gas_14)
                    detail.丙氣14Kg = ordersToday.Sum(Function(x) x.o_gas_c_14)

                    detail.普氣5Kg = ordersToday.Sum(Function(x) x.o_gas_5)
                    detail.丙氣5Kg = ordersToday.Sum(Function(x) x.o_gas_c_5)

                    detail.普氣2Kg = ordersToday.Sum(Function(x) x.o_gas_2)
                    detail.丙氣2Kg = ordersToday.Sum(Function(x) x.o_gas_c_2)

                    detail.普氣瓶數 = detail.普氣50Kg + detail.普氣20Kg + detail.普氣16Kg + detail.普氣10Kg + detail.普氣4Kg + detail.普氣15Kg + detail.普氣14Kg + detail.普氣5Kg + detail.普氣2Kg
                    detail.丙氣瓶數 = detail.丙氣50Kg + detail.丙氣20Kg + detail.丙氣16Kg + detail.丙氣10Kg + detail.丙氣4Kg + detail.丙氣15Kg + detail.丙氣14Kg + detail.丙氣5Kg + detail.丙氣2Kg

                    detail.普氣Kg數 = ordersToday.Sum(Function(x) x.o_gas_total)
                    detail.丙氣Kg數 = ordersToday.Sum(Function(x) x.o_gas_c_total)

                    result.Add(detail)
                Next
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw
        End Try

        Return result
    End Function

    Public Function GasPayableDetailList(d As Date, manuId As Integer) As List(Of GasPayableDetail) Implements IReportRep.GasPayableDetailList
        Dim result As New List(Of GasPayableDetail)

        Try
            Using db As New gas_accounting_systemEntities
                '取得日期區間
                Dim startMonth = New Date(d.Year, d.Month, 1)
                Dim endDay = startMonth.AddMonths(1)
                Dim purchaseMonth = db.purchases.Where(Function(x) x.pur_date >= startMonth And x.pur_date < endDay)

                '獲取廠商大氣進貨資料
                Dim manu = purchaseMonth.Where(Function(x) x.pur_manu_id = manuId).OrderBy(Function(x) x.pur_date).ToList

                '遍歷每筆進貨
                Dim grandTotal As Integer = 0 '累計

                For Each item In manu
                    Dim detail As New GasPayableDetail
                    detail.廠商 = item.manufacturer.manu_name
                    detail.日期 = item.pur_date

                    If item.pur_product = "普氣" Then
                        detail.普氣 = item.pur_quantity
                        detail.普氣單價 = item.pur_unit_price + item.pur_deli_unit_price
                        detail.普氣金額 = item.pur_price + item.pur_delivery_fee
                    Else
                        detail.丙氣 = item.pur_quantity
                        detail.丙氣單價 = item.pur_unit_price + item.pur_deli_unit_price
                        detail.丙氣金額 = item.pur_price + item.pur_delivery_fee
                    End If

                    detail.總計 = detail.丙氣金額 + detail.普氣金額
                    detail.餘額 = detail.總計

                    grandTotal += detail.總計
                    detail.累計 = grandTotal

                    result.Add(detail)
                Next
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw
        End Try

        Return result
    End Function

    Public Function DailyCustomerReceivable(d As Date, cusCode As Integer) As List(Of DailyCustomerReceivable) Implements IReportRep.DailyCustomerReceivable
        Dim result As New List(Of DailyCustomerReceivable)

        Try
            Using db As New gas_accounting_systemEntities
                '取得日期區間
                Dim startMonth = New Date(d.Year, d.Month, 1)
                Dim endDay = startMonth.AddMonths(1)

                '取得客戶名稱
                Dim cus = db.customers.FirstOrDefault(Function(x) x.cus_code = cusCode)

                If cus Is Nothing Then
                    Throw New Exception("查無此客戶代號")

                End If

                Dim cusName = cus.cus_name

                '遍歷每天的訂單資料
                Dim grandTotal As Integer = 0 '累計
                Dim currentDate = startMonth

                While currentDate < endDay
                    Dim dailyReceivable As New DailyCustomerReceivable With {
                                            .客戶名稱 = cusName,
                                            .日期 = currentDate.ToString("MM月dd日")
                                        }

                    '取得該日訂單
                    Dim currentDateEnd = currentDate.AddDays(1)
                    Dim ordersToday = db.orders.Where(Function(x) x.o_date.Value >= currentDate And x.o_date < currentDateEnd And x.car.c_cus_id = cus.cus_id And x.o_in_out = "出場單").ToList

                    '取得廠運數據
                    Dim delivery = ordersToday.Where(Function(x) x.o_delivery_type = "廠運")
                    dailyReceivable.廠運普氣 = delivery.Sum(Function(x) x.o_gas_total)
                    dailyReceivable.廠運普氣退氣 = delivery.Sum(Function(x) x.o_return)
                    dailyReceivable.廠運普氣單價 = If(delivery.FirstOrDefault() Is Nothing, 0, delivery.FirstOrDefault.o_UnitPrice)
                    dailyReceivable.廠運普氣金額 = dailyReceivable.廠運普氣 * dailyReceivable.廠運普氣單價
                    dailyReceivable.廠運丙氣 = delivery.Sum(Function(x) x.o_gas_c_total)
                    dailyReceivable.廠運丙氣退氣 = delivery.Sum(Function(x) x.o_return_c)
                    dailyReceivable.廠運丙氣單價 = If(delivery.FirstOrDefault() Is Nothing, 0, delivery.FirstOrDefault.o_UnitPriceC)
                    dailyReceivable.廠運丙氣金額 = dailyReceivable.廠運丙氣 * dailyReceivable.廠運丙氣單價
                    dailyReceivable.廠運總提氣 = dailyReceivable.廠運普氣 + dailyReceivable.廠運丙氣

                    '取得自運數據
                    Dim pickUp = ordersToday.Where(Function(x) x.o_delivery_type = "自運")
                    dailyReceivable.自運普氣 = pickUp.Sum(Function(x) x.o_gas_total)
                    dailyReceivable.自運普氣退氣 = pickUp.Sum(Function(x) x.o_return)
                    dailyReceivable.自運普氣單價 = If(pickUp.FirstOrDefault() Is Nothing, 0, pickUp.FirstOrDefault().o_UnitPrice)
                    dailyReceivable.自運普氣金額 = dailyReceivable.自運普氣 * dailyReceivable.自運普氣單價
                    dailyReceivable.自運丙氣 = pickUp.Sum(Function(x) x.o_gas_c_total)
                    dailyReceivable.自運丙氣退氣 = pickUp.Sum(Function(x) x.o_return_c)
                    dailyReceivable.自運丙氣單價 = If(pickUp.FirstOrDefault() Is Nothing, 0, pickUp.FirstOrDefault().o_UnitPriceC)
                    dailyReceivable.自運丙氣金額 = dailyReceivable.自運丙氣 * dailyReceivable.自運丙氣單價
                    dailyReceivable.自運總提氣 = dailyReceivable.自運普氣 + dailyReceivable.自運丙氣

                    dailyReceivable.總提氣 = dailyReceivable.廠運總提氣 + dailyReceivable.自運總提氣
                    dailyReceivable.總額 = dailyReceivable.廠運丙氣金額 + dailyReceivable.廠運普氣金額 + dailyReceivable.自運丙氣金額 + dailyReceivable.自運普氣金額
                    dailyReceivable.掛帳 = dailyReceivable.總額

                    grandTotal += dailyReceivable.總額
                    dailyReceivable.累計 = grandTotal
                    result.Add(dailyReceivable)

                    currentDate = currentDate.AddDays(1)
                End While
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw
        End Try

        Return result
    End Function

    Public Function GasUsageAndCylinderCount(month As Date) As List(Of GasUsageAndCylinderCount) Implements IReportRep.GasUsageAndCylinderCount
        Dim result As New List(Of GasUsageAndCylinderCount)

        Try
            Using db As New gas_accounting_systemEntities
                '取得日期區間
                Dim startMonth = New Date(month.Year, month.Month, 1)
                Dim endDay = startMonth.AddMonths(1)

                '遍歷每天的訂單資料
                Dim returnGrandTotal As Integer = 0 '退氣累計
                Dim gasGrandTotal As Integer = 0 '提氣累計
                Dim currentDate = startMonth

                While currentDate < endDay
                    Dim data As New GasUsageAndCylinderCount With {
                                            .日期 = currentDate.ToString("MM月dd日")
                                        }

                    '取得該日訂單
                    Dim currentDateEnd = currentDate.AddDays(1)
                    Dim ordersToday = db.orders.Where(Function(x) x.o_date.Value >= currentDate And x.o_date < currentDateEnd).ToList

                    data.退氣 = ordersToday.Sum(Function(x) x.o_return + x.o_return_c)
                    returnGrandTotal += data.退氣
                    data.退氣累計量 = returnGrandTotal
                    data.提氣量 = ordersToday.Sum(Function(x) x.o_gas_total + x.o_gas_c_total)
                    gasGrandTotal += data.提氣量
                    data.提氣累計量 = gasGrandTotal

                    data.瓦斯瓶50Kg = ordersToday.Sum(Function(x) x.o_gas_50 + x.o_gas_c_50 + x.o_empty_50)
                    data.瓦斯瓶20Kg = ordersToday.Sum(Function(x) x.o_gas_20 + x.o_gas_c_20 + x.o_empty_20)
                    data.瓦斯瓶16Kg = ordersToday.Sum(Function(x) x.o_gas_16 + x.o_gas_c_16 + x.o_empty_16)
                    data.瓦斯瓶4Kg = ordersToday.Sum(Function(x) x.o_gas_4 + x.o_gas_c_4 + x.o_empty_4)
                    data.瓦斯瓶15Kg = ordersToday.Sum(Function(x) x.o_gas_15 + x.o_gas_c_15 + x.o_empty_15)
                    data.瓦斯瓶14Kg = ordersToday.Sum(Function(x) x.o_gas_14 + x.o_gas_c_14 + x.o_empty_14)
                    data.瓦斯瓶5Kg = ordersToday.Sum(Function(x) x.o_gas_5 + x.o_gas_c_5 + x.o_empty_5)
                    data.瓦斯瓶2Kg = ordersToday.Sum(Function(x) x.o_gas_2 + x.o_gas_c_2 + x.o_empty_2)

                    data.總支數 = data.瓦斯瓶50Kg + data.瓦斯瓶20Kg + data.瓦斯瓶16Kg + data.瓦斯瓶4Kg + data.瓦斯瓶15Kg + data.瓦斯瓶14Kg + data.瓦斯瓶5Kg + data.瓦斯瓶2Kg

                    result.Add(data)
                    currentDate = currentDate.AddDays(1)
                End While
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw
        End Try

        Return result
    End Function

    Public Function GetCashAccount(month As Date) As List(Of CashAccount) Implements IReportRep.GetCashAccount
        Dim result = New List(Of CashAccount)

        Try
            Using db As New gas_accounting_systemEntities
                '取得日期區間
                Dim startDate = New Date(month.Year, month.Month, 1)
                Dim endDate = startDate.AddMonths(1)

                '獲取收入數據
                Dim collections = db.collections.Where(Function(x) x.col_Date >= startDate AndAlso x.col_Date < endDate AndAlso x.col_Type = "現金").
                                                 Select(Function(x) New With {
                                                    .Date = x.col_Date,
                                                    .Subject = x.subject.s_name,
                                                    .Memo = x.col_Memo,
                                                    .Amount = x.col_Amount,
                                                    .IsIncome = True
                                                 })
                '獲取支出數據
                Dim payments = db.payments.Where(Function(x) x.p_Date >= startDate AndAlso x.p_Date < endDate AndAlso x.p_Type = "現金").
                                           Select(Function(x) New With {
                                                .Date = x.p_Date,
                                                .Subject = x.subject.s_name,
                                                .Memo = x.p_Memo,
                                                .Amount = x.p_Amount,
                                                .IsIncome = False
                                           })
                '合併並排列數據
                Dim allTransactions = collections.Union(payments) _
                                                 .OrderBy(Function(x) x.Date) _
                                                 .ThenBy(Function(x) x.IsIncome)

                '計算餘額並填入模型
                Dim balance As Integer = 0
                For Each transaction In allTransactions
                    balance += If(transaction.IsIncome, transaction.Amount, -transaction.Amount)
                    result.Add(New CashAccount With {
                        .日 = transaction.Date.Day,
                        .科目 = transaction.Subject,
                        .摘要 = transaction.Memo,
                        .收入金額 = If(transaction.IsIncome, transaction.Amount, 0),
                        .支出金額 = If(transaction.IsIncome, 0, transaction.Amount),
                        .餘額 = balance
                    })
                Next
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return result
    End Function

    Public Function GetBankAccount(month As Date, bankId As Integer) As Report_BankAccount Implements IReportRep.GetBankAccount
        Dim result As New Report_BankAccount

        Try
            result.年月 = month.ToString("yyyy年MM月")

            Dim startDate = New Date(month.Year, month.Month, 1)
            Dim endDate = startDate.AddMonths(1)

            '獲取收入數據
            Dim collections = _context.collections.Where(Function(x) x.col_AccountMonth >= startDate AndAlso x.col_AccountMonth < endDate AndAlso x.col_Type = "銀行" AndAlso x.col_bank_Id = bankId).
                                             Select(Function(x) New With {
                                                .Date = x.col_Date,
                                                .Subject = x.subject.s_name,
                                                .Memo = x.col_Memo,
                                                .Amount = x.col_Amount,
                                                .IsIncome = True
                                             })
            '獲取支出數據
            Dim payments = _context.payments.Where(Function(x) x.p_AccountMonth >= startDate AndAlso x.p_AccountMonth < endDate AndAlso x.p_Type = "銀行" AndAlso x.p_bank_Id = bankId).
                                       Select(Function(x) New With {
                                            .Date = x.p_Date,
                                            .Subject = x.subject.s_name,
                                            .Memo = x.p_Memo,
                                            .Amount = x.p_Amount,
                                            .IsIncome = False
                                       })
            '合併並排列數據
            Dim allTransactions = collections.Union(payments) _
                                             .OrderBy(Function(x) x.Date) _
                                             .ThenBy(Function(x) x.IsIncome)

#Region "計算餘額並填入模型"
            '取得上期餘額,若沒有則取得該銀行的初始資金
            Dim lastMonthlyBalance = _context.bank_monthly_balances.FirstOrDefault(Function(x) x.bm_Month < startDate)
            Dim lastClosingBalance As Integer

            If lastMonthlyBalance Is Nothing Then
                lastClosingBalance = _context.banks.Find(bankId)?.bank_InitialBalance
            Else
                lastClosingBalance = lastMonthlyBalance.bm_ClosingBalance
            End If

            result.List = New List(Of BankAccountList) From {
                New BankAccountList With {
                    .摘要 = "上期結餘",
                    .餘額 = lastClosingBalance
                }
            }

            '填入模型
            For Each transaction In allTransactions
                lastClosingBalance += If(transaction.IsIncome, transaction.Amount, -transaction.Amount)
                result.List.Add(New BankAccountList With {
                    .借方 = If(transaction.IsIncome, transaction.Amount, 0),
                    .摘要 = transaction.Memo,
                    .日期 = transaction.Date.Day,
                    .科目 = transaction.Subject,
                    .貸方 = If(transaction.IsIncome, 0, transaction.Amount),
                    .餘額 = lastClosingBalance
                })
            Next
#End Region
        Catch ex As Exception
            Throw New Exception("取得銀行帳資料發生錯誤", ex)
        End Try

        Return result
    End Function

    Public Function GetCustomerGasCylinderInventory(cusId As Integer) As Report_CustomerGasCylinderInventory Implements IReportRep.GetCustomerGasCylinderInventory
        Try
            Dim result As New Report_CustomerGasCylinderInventory With {
                .CustomerName = _context.customers.Find(cusId).cus_name,
                .List = New List(Of Report_CustomerGasCylinderInventory.DepositList)
            }

            Dim depositList = _context.cars.Where(Function(x) x.c_cus_id = cusId).
                                            Select(Function(x) New Report_CustomerGasCylinderInventory.DepositList With {
                                                .Barrel_10KG = x.c_deposit_10,
                                                .Barrel_14KG = x.c_deposit_14,
                                                .Barrel_15KG = x.c_deposit_15,
                                                .Barrel_16KG = x.c_deposit_16,
                                                .Barrel_20KG = x.c_deposit_20,
                                                .Barrel_2KG = x.c_deposit_2,
                                                .Barrel_4KG = x.c_deposit_4,
                                                .Barrel_50KG = x.c_deposit_50,
                                                .Barrel_5KG = x.c_deposit_5,
                                                .CarNo = x.c_no,
                                                .DriverName = x.c_driver
                                             }).ToList

            result.List = depositList

            Return result
        Catch ex As Exception
            Throw New Exception("取得客戶寄桶結存瓶資料發生錯誤", ex)
        End Try
    End Function
End Class