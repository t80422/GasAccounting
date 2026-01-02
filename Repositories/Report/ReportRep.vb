Imports System.Data.Entity

Public Class ReportRep
    Implements IReportRep

    Private _context As gas_accounting_systemEntities

    Public Sub New(context As gas_accounting_systemEntities)
        _context = context
    End Sub

    Public Function CustomersGasDetailByDay(selectDate As Date, isMonth As Boolean) As List(Of CustomersGasDetailByDay) Implements IReportRep.CustomersGasDetailByDay
        Try
            ' 根據 isMonth 參數定義時間範圍
            Dim monthStart = New Date(selectDate.Year, selectDate.Month, 1)
            Dim periodStart As Date
            Dim periodEnd As Date

            If isMonth Then
                ' 月份模式：計算整個月的資料
                periodStart = monthStart
                periodEnd = monthStart.AddMonths(1)
            Else
                ' 單日模式：計算當日的資料
                periodStart = selectDate.Date
                periodEnd = periodStart.AddDays(1)
            End If

            ' 一次性取得所有客戶資料
            Dim customers = _context.customers.AsNoTracking.Select(Function(c) New With {
                .客戶Id = c.cus_id,
                .客戶名稱 = c.cus_name
            }).ToList()

            ' 一次性取得主要期間的訂單資料（當日或當月）
            Dim periodOrders = _context.orders.AsNoTracking.Where(Function(o) o.o_date >= periodStart AndAlso o.o_date < periodEnd).
                Select(Function(o) New With {
                    .客戶Id = o.o_cus_Id,
                    .存氣 = o.o_return + o.o_return_c,
                    .提氣量 = o.o_gas_total + o.o_gas_c_total,
                    .氣款 = o.o_total_amount
                }).ToList()

            ' 一次性取得當月累計訂單資料（始終計算到選定期間結束）
            Dim monthOrders = _context.orders.AsNoTracking.Where(Function(o) o.o_date >= monthStart AndAlso o.o_date < periodEnd).
                Select(Function(o) New With {
                    .客戶Id = o.o_cus_Id,
                    .提氣量 = o.o_gas_total + o.o_gas_c_total,
                    .氣款 = o.o_total_amount
                }).ToList()

            ' 一次性取得主要期間的收款資料（當日或當月）
            Dim periodCollections = _context.collections.AsNoTracking.Where(Function(c) c.col_Date >= periodStart AndAlso
                                                                           c.col_Date < periodEnd AndAlso
                                                                           c.col_AccountMonth.Year = selectDate.Year AndAlso
                                                                           c.col_AccountMonth.Month = selectDate.Month AndAlso
                                                                           c.col_cus_Id IsNot Nothing AndAlso
                                                                           c.subject.s_name = "氣款收入").
                                                        Select(Function(c) New With {
                                                            .客戶Id = c.col_cus_Id,
                                                            .收款 = c.col_Amount
                                                        }).ToList()

            ' 一次性取得當月收款資料（始終計算到選定期間結束）
            Dim monthCollections = _context.collections.AsNoTracking.Where(Function(c) c.col_Date >= monthStart AndAlso
                                                                          c.col_Date < periodEnd AndAlso
                                                                          c.col_cus_Id.HasValue AndAlso
                                                                          c.col_AccountMonth.Year = selectDate.Year AndAlso
                                                                          c.col_AccountMonth.Month = selectDate.Month AndAlso
                                                                          c.col_cus_Id IsNot Nothing AndAlso
                                                                          c.subject.s_name = "氣款收入").
                                                        Select(Function(c) New With {
                                                            .客戶Id = c.col_cus_Id,
                                                            .收款 = c.col_Amount
                                                        }).ToList()

            ' 將資料按客戶分組
            Dim periodOrdersByCustomer = periodOrders.GroupBy(Function(o) o.客戶Id).
                ToDictionary(Function(g) g.Key, Function(g) g.ToList())

            Dim monthOrdersByCustomer = monthOrders.GroupBy(Function(o) o.客戶Id).
                ToDictionary(Function(g) g.Key, Function(g) g.ToList())

            Dim periodCollectionsByCustomer = periodCollections.GroupBy(Function(c) c.客戶Id).
                ToDictionary(Function(g) g.Key, Function(g) g.ToList())

            Dim monthCollectionsByCustomer = monthCollections.GroupBy(Function(c) c.客戶Id).
                ToDictionary(Function(g) g.Key, Function(g) g.ToList())

            ' 組合結果
            Dim result = customers.Select(Function(cus) New CustomersGasDetailByDay With {
                .客戶名稱 = cus.客戶名稱,
                .存氣 = If(periodOrdersByCustomer.ContainsKey(cus.客戶Id),
                           periodOrdersByCustomer(cus.客戶Id).Sum(Function(o) o.存氣), 0),
                .本日提量 = If(periodOrdersByCustomer.ContainsKey(cus.客戶Id),
                              periodOrdersByCustomer(cus.客戶Id).Sum(Function(o) o.提氣量), 0),
                .本日氣款 = If(periodOrdersByCustomer.ContainsKey(cus.客戶Id),
                              periodOrdersByCustomer(cus.客戶Id).Sum(Function(o) o.氣款), 0),
                .當月累計提量 = If(monthOrdersByCustomer.ContainsKey(cus.客戶Id),
                                monthOrdersByCustomer(cus.客戶Id).Sum(Function(o) o.提氣量), 0),
                .本日收款 = If(periodCollectionsByCustomer.ContainsKey(cus.客戶Id),
                              periodCollectionsByCustomer(cus.客戶Id).Sum(Function(c) c.收款), 0),
                .結欠 = If(monthOrdersByCustomer.ContainsKey(cus.客戶Id),
                          monthOrdersByCustomer(cus.客戶Id).Sum(Function(o) o.氣款), 0) -
                       If(monthCollectionsByCustomer.ContainsKey(cus.客戶Id),
                          monthCollectionsByCustomer(cus.客戶Id).Sum(Function(c) c.收款), 0)
            }).ToList()

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function CustomersGetGasList(d As Date, isMonth As Boolean) As List(Of CusGetGas) Implements IReportRep.CustomersGetGasList
        Dim result As New List(Of CusGetGas)

        Try
            '獲取所有客戶資料
            Dim customers = _context.customers.AsNoTracking.OrderBy(Function(x) x.cus_code).ToList

            '遍歷每個客戶並蒐集相關資料
            For Each cus In customers
                Dim detail As New CusGetGas With {
                    .客戶名稱 = cus.cus_name
                }

                Dim startDay As Date
                Dim endDay As Date

                ' 設定日期範圍
                If isMonth Then
                    startDay = New Date(d.Year, d.Month, 1)
                    endDay = startDay.AddMonths(1)
                Else
                    startDay = d.Date
                    endDay = d.Date.AddDays(1)
                End If

                Dim ordersToday = _context.orders.AsNoTracking.Where(Function(x) x.o_cus_Id = cus.cus_id And x.o_date.Value >= startDay And x.o_date.Value < endDay).ToList

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

                detail.普氣18Kg = ordersToday.Sum(Function(x) x.o_gas_18)
                detail.丙氣18Kg = ordersToday.Sum(Function(x) x.o_gas_c_18)

                detail.普氣14Kg = ordersToday.Sum(Function(x) x.o_gas_14)
                detail.丙氣14Kg = ordersToday.Sum(Function(x) x.o_gas_c_14)

                detail.普氣5Kg = ordersToday.Sum(Function(x) x.o_gas_5)
                detail.丙氣5Kg = ordersToday.Sum(Function(x) x.o_gas_c_5)

                detail.普氣2Kg = ordersToday.Sum(Function(x) x.o_gas_2)
                detail.丙氣2Kg = ordersToday.Sum(Function(x) x.o_gas_c_2)

                detail.普氣殘氣 = ordersToday.Sum(Function(x) x.o_return)
                detail.丙氣殘氣 = ordersToday.Sum(Function(x) x.o_return_c)

                detail.普氣提量 = ordersToday.Sum(Function(x) x.o_gas_total + x.o_return)
                detail.丙氣提量 = ordersToday.Sum(Function(x) x.o_gas_c_total + x.o_return_c)

                result.Add(detail)
            Next
        Catch ex As Exception
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

    Public Function MonthlyCustomerReceivable(d As Date, cusCode As String) As List(Of DailyCustomerReceivable) Implements IReportRep.MonthlyCustomerReceivable
        Dim result As New List(Of DailyCustomerReceivable)

        Try
            '取得日期區間
            Dim startMonth = New Date(d.Year, d.Month, 1)
            Dim endDay = startMonth.AddMonths(1)

            '取得客戶名稱
            Dim cus = _context.customers.FirstOrDefault(Function(x) x.cus_code = cusCode)

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
                Dim ordersToday = _context.orders.Where(Function(x) x.o_date.Value >= currentDate And x.o_date < currentDateEnd And x.o_cus_Id = cus.cus_id And x.o_in_out = "出場單").ToList

                '取得廠運數據
                Dim delivery = ordersToday.Where(Function(x) x.o_delivery_type = "廠運")
                Dim gasTotal_delivery = delivery.Sum(Function(x) x.o_gas_total)
                Dim gasCTotal_delivery = delivery.Sum(Function(x) x.o_gas_c_total)

                dailyReceivable.廠運普氣退氣 = delivery.Sum(Function(x) x.o_return)
                dailyReceivable.廠運普氣 = gasTotal_delivery + dailyReceivable.廠運普氣退氣
                dailyReceivable.廠運普氣單價 = If(delivery.FirstOrDefault() Is Nothing, 0, delivery.FirstOrDefault.o_UnitPrice)
                dailyReceivable.廠運普氣金額 = gasTotal_delivery * dailyReceivable.廠運普氣單價
                dailyReceivable.廠運丙氣退氣 = delivery.Sum(Function(x) x.o_return_c)
                dailyReceivable.廠運丙氣 = gasCTotal_delivery + dailyReceivable.廠運丙氣退氣
                dailyReceivable.廠運丙氣單價 = If(delivery.FirstOrDefault() Is Nothing, 0, delivery.FirstOrDefault.o_UnitPriceC)
                dailyReceivable.廠運丙氣金額 = gasCTotal_delivery * dailyReceivable.廠運丙氣單價
                dailyReceivable.廠運總提氣 = dailyReceivable.廠運普氣 + dailyReceivable.廠運丙氣

                '取得自運數據
                Dim pickUp = ordersToday.Where(Function(x) x.o_delivery_type = "自運")
                Dim gasTotal_pickUp = pickUp.Sum(Function(x) x.o_gas_total)
                Dim gasCTotal_pickUp = pickUp.Sum(Function(x) x.o_gas_c_total)

                dailyReceivable.自運普氣退氣 = pickUp.Sum(Function(x) x.o_return)
                dailyReceivable.自運普氣 = gasTotal_pickUp + dailyReceivable.自運普氣退氣
                dailyReceivable.自運普氣單價 = If(pickUp.FirstOrDefault() Is Nothing, 0, pickUp.FirstOrDefault().o_UnitPrice)
                dailyReceivable.自運普氣金額 = gasTotal_pickUp * dailyReceivable.自運普氣單價
                dailyReceivable.自運丙氣退氣 = pickUp.Sum(Function(x) x.o_return_c)
                dailyReceivable.自運丙氣 = gasCTotal_pickUp + dailyReceivable.自運丙氣退氣
                dailyReceivable.自運丙氣單價 = If(pickUp.FirstOrDefault() Is Nothing, 0, pickUp.FirstOrDefault().o_UnitPriceC)
                dailyReceivable.自運丙氣金額 = gasCTotal_pickUp * dailyReceivable.自運丙氣單價
                dailyReceivable.自運總提氣 = dailyReceivable.自運普氣 + dailyReceivable.自運丙氣

                ' 收錢
                Dim collectionsToday = _context.collections.Where(Function(x) x.col_Date >= currentDate And
                                                                              x.col_Date < currentDateEnd And
                                                                              x.col_cus_Id = cus.cus_id And
                                                                              x.subject.s_name = "氣款收入").
                                                            GroupBy(Function(x) x.col_Type).ToList

                For Each group In collectionsToday
                    Select Case group.Key
                        Case "現金"
                            dailyReceivable.現金 = group.Sum(Function(x) x.col_Amount)
                        Case "銀行存款"
                            dailyReceivable.銀行 = group.Sum(Function(x) x.col_Amount)
                        Case "應收票據"
                            dailyReceivable.票據 = group.Sum(Function(x) x.col_Amount)
                    End Select
                Next
                ' 總計
                dailyReceivable.總提氣 = dailyReceivable.廠運總提氣 + dailyReceivable.自運總提氣
                dailyReceivable.總額 = dailyReceivable.廠運丙氣金額 + dailyReceivable.廠運普氣金額 + dailyReceivable.自運丙氣金額 + dailyReceivable.自運普氣金額
                dailyReceivable.掛帳 = dailyReceivable.總額 - dailyReceivable.現金 - dailyReceivable.銀行 - dailyReceivable.票據

                grandTotal += dailyReceivable.掛帳
                dailyReceivable.累計 = grandTotal
                result.Add(dailyReceivable)

                currentDate = currentDate.AddDays(1)
            End While
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw
        End Try

        Return result
    End Function

    Public Function GasUsageAndCylinderCount(month As Date) As List(Of GasUsageAndCylinderCount) Implements IReportRep.GasUsageAndCylinderCount
        Dim result As New List(Of GasUsageAndCylinderCount)

        Try
            '取得日期區間
            Dim startMonth = New Date(month.Year, month.Month, 1)
            Dim endDay = startMonth.AddMonths(1)

            '遍歷每天的訂單資料
            Dim currentDate = startMonth

            While currentDate < endDay
                Dim data As New GasUsageAndCylinderCount With {.日期 = currentDate.ToString("MM月dd日")}

                '取得該日訂單
                Dim currentDateEnd = currentDate.AddDays(1)
                Dim ordersToday = _context.orders.Where(Function(x) x.o_date.Value >= currentDate And x.o_date < currentDateEnd).ToList
                data.退氣 = ordersToday.Sum(Function(x) x.o_return + x.o_return_c)
                data.提氣量 = ordersToday.Sum(Function(x) x.o_gas_total + x.o_gas_c_total)
                data.瓦斯瓶50Kg = ordersToday.Sum(Function(x) x.o_gas_50 + x.o_gas_c_50 + x.o_empty_50)
                data.瓦斯瓶20Kg = ordersToday.Sum(Function(x) x.o_gas_20 + x.o_gas_c_20 + x.o_empty_20)
                data.瓦斯瓶16Kg = ordersToday.Sum(Function(x) x.o_gas_16 + x.o_gas_c_16 + x.o_empty_16)
                data.瓦斯瓶10Kg = ordersToday.Sum(Function(x) x.o_gas_10 + x.o_gas_c_10 + x.o_empty_10)
                data.瓦斯瓶4Kg = ordersToday.Sum(Function(x) x.o_gas_4 + x.o_gas_c_4 + x.o_empty_4)

                result.Add(data)
                currentDate = currentDate.AddDays(1)
            End While
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw
        End Try

        Return result
    End Function

    Public Function GetCashAccount(startDate As Date, endDate As Date) As List(Of CashAccount) Implements IReportRep.GetCashAccount
        Dim result = New List(Of CashAccount)

        Try
            Dim formatEndDate = endDate.AddDays(1)
            ' 取得前期餘額
            Dim initialBalance = _context.basic_set.FirstOrDefault.bs_Cash

            Dim lastCollection = _context.collections.Where(Function(x) x.col_Date < startDate AndAlso x.col_Type = "現金")
            Dim lastCollectionSum = If(lastCollection.Any(), lastCollection.Sum(Function(x) x.col_Amount), 0)

            Dim lastPayment = _context.payments.Where(Function(x) x.p_Date < startDate AndAlso x.p_Type = "現金")
            Dim lastPaymentSum = If(lastPayment.Any, lastPayment.Sum(Function(x) x.p_Amount), 0)

            result.Add(New CashAccount With {
                .摘要 = "前期餘額",
                .餘額 = initialBalance + lastCollectionSum - lastPaymentSum
            })

            '獲取收入數據
            Dim collections = _context.collections.AsNoTracking.
                Where(Function(x) x.col_Date >= startDate AndAlso
                                  x.col_Date < formatEndDate AndAlso
                                  x.col_Type = "現金").
                Select(Function(x) New With {
                    .Date = x.col_Date,
                    .Subject = x.subject.s_name,
                    .Memo = If(x.customer IsNot Nothing, x.customer.cus_code + " ", "") + x.col_Memo,
                    .Amount = x.col_Amount,
                    .IsIncome = True
                }).ToList

            '獲取支出數據
            Dim payments = _context.payments.Where(Function(x) x.p_Date >= startDate AndAlso
                                                         x.p_Date < formatEndDate AndAlso
                                                         x.p_Type = "現金").
                                            Select(Function(x) New With {
                                                        .Date = x.p_Date,
                                                        .Subject = x.subject.s_name,
                                                        .Memo = x.p_Memo,
                                                        .Amount = x.p_Amount,
                                                        .IsIncome = False
                                            }).ToList
            '合併並排列數據
            Dim allTransactions = collections.Union(payments) _
                                             .OrderBy(Function(x) x.Date) _
                                             .ThenBy(Function(x) x.IsIncome).ToList

            '計算餘額並填入模型
            Dim balance As Integer = initialBalance + lastCollectionSum - lastPaymentSum
            For Each transaction In allTransactions
                balance += If(transaction.IsIncome, transaction.Amount, -transaction.Amount)
                result.Add(New CashAccount With {
                        .日期 = transaction.Date.ToString("MM月dd日"),
                        .科目 = transaction.Subject,
                        .摘要 = transaction.Memo,
                        .收入金額 = If(transaction.IsIncome, transaction.Amount, 0),
                        .支出金額 = If(transaction.IsIncome, 0, transaction.Amount),
                        .餘額 = balance
                    })
            Next
        Catch ex As Exception
            MessageBox.Show(ex.StackTrace)
            Throw
        End Try

        Return result
    End Function

    Public Function GetBankAccount(month As Date, bankId As Integer) As BankAccount Implements IReportRep.GetBankAccount
        Dim result As New BankAccount

        Try
            result.日期 = $"{month:yyyy年MM月}"

            ' 取得銀行名稱
            result.銀行名稱 = If(_context.banks.Find(bankId)?.bank_name, "未知銀行")

            '獲取收入數據
            Dim collections = _context.collections.AsNoTracking.
                                                   Where(Function(x) x.col_Date.Year = month.Year AndAlso
                                                                     x.col_Date.Month = month.Month AndAlso
                                                                     x.col_Type = "銀行存款" AndAlso
                                                                     x.col_bank_Id = bankId).
                                                   Select(Function(x) New With {
                                                        .Date = x.col_Date,
                                                        .Subject = x.subject.s_name,
                                                        .Memo = x.col_Memo,
                                                        .Amount = x.col_Amount,
                                                        .IsIncome = True,
                                                        .Target = x.customer.cus_code
                                                   })

            '銀行取出現金：獨立撈取現金類型且科目為銀行存款
            Dim cashToBankCollections = _context.collections.AsNoTracking.
                                                         Where(Function(x) x.col_Date.Year = month.Year AndAlso
                                                                           x.col_Date.Month = month.Month AndAlso
                                                                           x.col_credit_bank_id = bankId AndAlso
                                                                           x.subject.s_name = "銀行存款").
                                                         Select(Function(x) New With {
                                                              .Date = x.col_Date,
                                                              .Subject = x.col_Type,
                                                              .Memo = x.col_Memo,
                                                              .Amount = x.col_Amount,
                                                              .IsIncome = False,
                                                              .Target = x.customer.cus_code
                                                         })
            '獲取支出數據
            Dim payments = _context.payments.AsNoTracking.
                                             Where(Function(x) x.p_Date.Year = month.Year AndAlso
                                                               x.p_Date.Month = month.Month AndAlso
                                                               x.p_Type = "銀行存款" AndAlso
                                                               x.p_bank_Id = bankId).
                                             Select(Function(x) New With {
                                                .Date = x.p_Date,
                                                .Subject = x.subject.s_name,
                                                .Memo = x.p_Memo,
                                                .Amount = x.p_Amount,
                                                .IsIncome = False,
                                                .Target = If(x.company IsNot Nothing, x.company.comp_name, "")
                                             })

            ' 銀行存入現金: 獨立撈取現金類型且科目為銀行存款
            Dim cashToBankPayments = _context.payments.AsNoTracking.
                                                 Where(Function(x) x.p_Date.Year = month.Year AndAlso
                                                                   x.p_Date.Month = month.Month AndAlso
                                                                   x.p_Type = "現金" AndAlso
                                                                   x.p_bank_Id = bankId AndAlso
                                                                   x.subject.s_name = "銀行存款").
                                                 Select(Function(x) New With {
                                                      .Date = x.p_Date,
                                                      .Subject = x.p_Type,
                                                      .Memo = x.p_Memo,
                                                      .Amount = x.p_Amount,
                                                      .IsIncome = True,
                                                      .Target = If(x.company IsNot Nothing, x.company.comp_name, "")
                                                 })

            '合併並排列數據
            Dim allTransactions = collections.Concat(cashToBankCollections).Concat(payments).Concat(cashToBankPayments).
                                              OrderBy(Function(x) x.Date).
                                              ThenBy(Function(x) x.IsIncome).
                                              ToList

            '取得上期餘額
            '1. 先取得上期月結餘額
            Dim startMonth = New Date(month.Year, month.Month, 1)
            Dim lastMonthlyBalance = _context.bank_monthly_balances.AsNoTracking.
                                                                    Where(Function(x) x.bm_Month < startMonth AndAlso
                                                                                      x.bm_bank_Id = bankId).
                                                                    OrderByDescending(Function(x) x.bm_Month).
                                                                    FirstOrDefault()
            Dim lastClosingBalance As Integer = If(lastMonthlyBalance Is Nothing,
                                                 _context.banks.Find(bankId)?.bank_InitialBalance,
                                                 lastMonthlyBalance.bm_ClosingBalance)

            ''2. 計算從上期月結到開始日期前的交易金額
            'If lastMonthlyBalance IsNot Nothing Then
            '    '將上期月結日期轉換到該月的最後一天
            '    Dim lastMonthEndDate = New Date(lastMonthlyBalance.bm_Month.Year,
            '                                  lastMonthlyBalance.bm_Month.Month,
            '                                  Date.DaysInMonth(lastMonthlyBalance.bm_Month.Year, lastMonthlyBalance.bm_Month.Month))

            '    Dim collectionsQuery = _context.collections.Where(Function(x) x.col_Date > lastMonthEndDate AndAlso
            '                                                                 x.col_Date < startMonth AndAlso
            '                                                                 x.col_Type = "銀行" AndAlso
            '                                                                 x.col_bank_Id = bankId)
            '    Dim paymentsQuery = _context.payments.Where(Function(x) x.p_Date > lastMonthEndDate AndAlso
            '                                                           x.p_Date < startMonth AndAlso
            '                                                           x.p_Type = "銀行" AndAlso
            '                                                           x.p_bank_Id = bankId)

            '    Dim collectionsSum As Integer = If(collectionsQuery.Any(), collectionsQuery.Sum(Function(x) x.col_Amount), 0)
            '    Dim paymentsSum As Integer = If(paymentsQuery.Any(), paymentsQuery.Sum(Function(x) x.p_Amount), 0)

            '    Dim transactionsBeforeStart = collectionsSum - paymentsSum
            '    lastClosingBalance += transactionsBeforeStart
            'End If

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
                    .摘要 = transaction.Target + transaction.Memo,
                    .日期 = transaction.Date,
                    .科目 = transaction.Subject,
                    .貸方 = If(transaction.IsIncome, 0, transaction.Amount),
                    .餘額 = lastClosingBalance
                })
            Next
        Catch ex As Exception
            Throw New Exception("取得銀行帳資料發生錯誤", ex)
        End Try

        Return result
    End Function

    Public Function GetCustomerGasCylinderInventory(cusId As Integer) As CustomerGasCylinderInventory Implements IReportRep.GetCustomerGasCylinderInventory
        Try
            Dim result As New CustomerGasCylinderInventory With {
                .CustomerName = _context.customers.Find(cusId).cus_name,
                .List = New List(Of CustomerGasCylinderInventory.DepositList)
            }

            Dim depositList = _context.cars.Where(Function(x) x.c_cus_id = cusId).
                                            Select(Function(x) New CustomerGasCylinderInventory.DepositList With {
                                                .瓦斯瓶10Kg = x.c_deposit_10,
                                                .瓦斯瓶14Kg = x.c_deposit_14,
                                                .瓦斯瓶18Kg = x.c_deposit_18,
                                                .瓦斯瓶16Kg = x.c_deposit_16,
                                                .瓦斯瓶20Kg = x.c_deposit_20,
                                                .瓦斯瓶2Kg = x.c_deposit_2,
                                                .瓦斯瓶4Kg = x.c_deposit_4,
                                                .瓦斯瓶50Kg = x.c_deposit_50,
                                                .瓦斯瓶5Kg = x.c_deposit_5,
                                                .CarNo = x.c_no,
                                                .DriverName = x.c_driver
                                             }).ToList

            result.List = depositList

            Return result
        Catch ex As Exception
            Throw New Exception("取得客戶寄桶結存瓶資料發生錯誤", ex)
        End Try
    End Function

    Public Function GetNewBarrelDetails(month As Date) As NewBarrelDetails Implements IReportRep.GetNewBarrelDetails
        Try
            Dim result As New NewBarrelDetails
            Dim barrelTypes = {"50", "20", "16", "10", "4"}
            Dim endDate = month.AddMonths(1)

            '取得上期結餘
            Dim lastBalances = _context.barrel_monthly_balances.Where(Function(x) x.barmb_Month < month).
                                                                GroupBy(Function(x) x.gas_barrel.gb_Name).
                                                                ToDictionary(Function(g) g.Key, Function(g) g.OrderByDescending(Function(x) x.barmb_Month).FirstOrDefault?.barmb_ClosingBalance)

            Dim initInventories = _context.gas_barrel.ToDictionary(Function(x) x.gb_Name, Function(x) x.gb_InitialInventory)

            For Each barrelType In barrelTypes
                Dim lastBalance = If(lastBalances.ContainsKey(barrelType), lastBalances(barrelType), initInventories(barrelType))
                SetLastBalance(result, barrelType, lastBalance)
            Next

            '取得購買價格
            Dim payPrices = _context.purchase_barrel.Where(Function(x) x.pb_Date < endDate).
                                                     OrderByDescending(Function(x) x.pb_Date).
                                                     FirstOrDefault

            If payPrices Is Nothing Then Throw New Exception("尚未有進貨資料")

            With result
                .PayUnitPrice50 = payPrices.pb_UnitPrice_50
                .PayUnitPrice20 = payPrices.pb_UnitPrice_20
                .PayUnitPrice16 = payPrices.pb_UnitPrice_16
                .PayUnitPrice10 = payPrices.pb_UnitPrice_10
                .PayUnitPrice4 = payPrices.pb_UnitPrice_4
            End With

            '取得進貨數據
            Dim inData = _context.purchase_barrel.Where(Function(x) x.pb_Date.Year = month.Year AndAlso x.pb_Date.Month = month.Month).
                                                  GroupBy(Function(x) New With {Key .Date = x.pb_Date, Key .ManuId = x.pb_manu_Id}).
                                                  Select(Function(g) New With {
                                                        .Day = g.Key.Date,
                                                        g.Key.ManuId,
                                                        .Qty50 = g.Sum(Function(x) x.pb_Qty_50),
                                                        .Qty20 = g.Sum(Function(x) x.pb_Qty_20),
                                                        .Qty16 = g.Sum(Function(x) x.pb_Qty_16),
                                                        .Qty10 = g.Sum(Function(x) x.pb_Qty_10),
                                                        .Qty4 = g.Sum(Function(x) x.pb_Qty_4),
                                                        .Memo = g.Select(Function(x) x.manufacturer.manu_name).FirstOrDefault
                                                  }).ToList

            '取得出貨數據
            Dim outData = _context.orders.Where(Function(x) x.o_date.Value.Year = month.Year AndAlso x.o_date.Value.Month = month.Month).
                                          GroupBy(Function(x) New With {Key .Date = x.o_date, Key .CusId = x.o_cus_Id}).
                                          Select(Function(g) New With {
                                                .Day = g.Key.Date,
                                                g.Key.CusId,
                                                .Qty50 = g.Sum(Function(x) x.o_new_in_50),
                                                .Qty20 = g.Sum(Function(x) x.o_new_in_20),
                                                .Qty16 = g.Sum(Function(x) x.o_new_in_16),
                                                .Qty10 = g.Sum(Function(x) x.o_new_in_10),
                                                .Qty4 = g.Sum(Function(x) x.o_new_in_4),
                                                .Memo = g.Select(Function(x) x.customer.cus_name).FirstOrDefault
                                          }).ToList

            '合併數據
            For Each item In inData
                result.List.Add(New NewBarrelDetailsList With {
                    .Day = item.Day.ToString("yyyy.MM.dd"),
                    .In50 = item.Qty50,
                    .In20 = item.Qty20,
                    .In16 = item.Qty16,
                    .In10 = item.Qty10,
                    .In4 = item.Qty4,
                    .Memo = item.Memo
                })
            Next

            For Each item In outData
                result.List.Add(New NewBarrelDetailsList With {
                    .Day = item.Day.Value.ToString("yyyy.MM.dd"),
                    .Out50 = item.Qty50,
                    .Out20 = item.Qty20,
                    .Out16 = item.Qty16,
                    .Out10 = item.Qty10,
                    .Out4 = item.Qty4,
                    .Memo = item.Memo
                })
            Next

            result.List = result.List.OrderBy(Function(x) x.Day).ToList
            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetInvoice(month As Date) As List(Of Report_Invoice) Implements IReportRep.GetInvoice
        Try
            Dim query = (From o In _context.orders
                         Where o.o_date.Value.Year = month.Year AndAlso o.o_date.Value.Month = month.Month
                         Group Join i In _context.invoices On o.o_cus_Id Equals i.i_cus_Id Into invoiceGroup = Group
                         From invoice In invoiceGroup.DefaultIfEmpty()
                         Group By Key = New With {
                            .CusId = o.o_cus_Id,
                            .CusCode = o.customer.cus_code,
                            .CusName = o.customer.cus_name,
                            .TaxId = o.customer.cus_tax_id
                        } Into g = Group
                         Select New Report_Invoice With {
                            .CusCode = Key.CusCode,
                            .CusName = Key.CusName,
                            .TaxId = Key.TaxId,
                            .Amount = g.Sum(Function(x) x.o.o_gas_total + x.o.o_gas_c_total),
                            .IsInvoice = g.Sum(Function(x) If(x.invoice Is Nothing, 0, x.invoice.i_KG))
                        }).OrderBy(Function(x) x.CusCode)

            Return query.ToList()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub SetLastBalance(result As NewBarrelDetails, barrelType As String, lastBalance As Integer)
        Select Case barrelType
            Case "50Kg" : result.Last50 = lastBalance
            Case "20Kg" : result.Last20 = lastBalance
            Case "16Kg" : result.Last16 = lastBalance
            Case "10Kg" : result.Last10 = lastBalance
            Case "4Kg" : result.Last4 = lastBalance
        End Select
    End Sub

    Public Function GetMonthlyAccountsReceivable(month As Date) As MonthlyAccountsReceivable Implements IReportRep.GetMonthlyAccountsReceivable
        Try
            Dim result = New MonthlyAccountsReceivable With {
                .Month = $"{month:yyyy年MM月} 應收帳明細表"
            }

            result.List = (From o In _context.orders
                           Where o.o_date.Value.Year = month.Year AndAlso o.o_date.Value.Month = month.Month
                           Group Join c In _context.collections On o.o_cus_Id Equals c.col_cus_Id Into collectionGroup = Group
                           From collection In collectionGroup.DefaultIfEmpty
                           Group By Key = New With {
                             .CusCode = o.customer.cus_code
                           } Into g = Group
                           Select New MonthlyAccountsReceivableList With {
                             .CusCode = Key.CusCode,
                             .AccountsReceivable = g.Sum(Function(x) x.o.o_total_amount),
                             .AccountsReceived = g.Sum(Function(x) If(x.collection Is Nothing, 0, x.collection.col_Amount)),
                             .Discount = g.Sum(Function(x) x.o.o_sales_allowance)
                           }).OrderBy(Function(x) x.CusCode).ToList

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetInventoryTransactionDetail(year As Date, compId As Integer, empId As Integer) As InventoryTransactionDetail Implements IReportRep.GetInventoryTransactionDetail
        Try
            Dim company = _context.companies.Find(compId)
            Dim result = New InventoryTransactionDetail With {
                .Company = company.comp_name,
                .OperatorName = _context.employees.Find(empId).emp_name,
                .Phone = company.comp_Phone,
                .Year = year.Year.ToString() & " 年度",
                .List = New List(Of InventoryTransactionDetailList)
            }

            '取得所有供應商
            Dim allVendors = _context.manufacturers.Where(Function(x) x.manu_GasVendor).Select(Function(x) x.manu_name).ToList

            ' 取得該年度每月數據
            Dim monthlyData = _context.gas_monthly_balances.Where(Function(gmb) gmb.gmb_Month.Year = year.Year AndAlso gmb.gmb_comp_Id = compId).
                                                            OrderBy(Function(x) x.gmb_Month).ToList

            ' 預先獲取整年的進貨數據
            Dim rawData = _context.purchases.Where(Function(x) x.pur_date.Value.Year = year.Year AndAlso x.pur_comp_id = compId).
                                             Select(Function(x) New With {
                                                 .Month = x.pur_date.Value.Month,
                                                 .Vendor = x.manufacturer.manu_name,
                                                 .Quantity = x.pur_quantity
                                             }).ToList()

            Dim yearPurchases = rawData.GroupBy(Function(x) New With {Key .Month = x.Month, Key .Vendor = x.Vendor}).
                                        Select(Function(g) New With {
                                            .Month = g.Key.Month,
                                            .Vendor = g.Key.Vendor,
                                            .Amount = g.Sum(Function(x) x.Quantity)
                                        }).ToList()

            For Each item In monthlyData
                Dim purchasesByVendor = yearPurchases.Where(Function(x) x.Month = item.gmb_Month.Month).
                                                      ToDictionary(Function(x) x.Vendor, Function(x) x.Amount.Value)
                '確保所有廠商都在字典中,如果沒有購買則為0
                For Each vendor In allVendors
                    If Not purchasesByVendor.ContainsKey(vendor) Then purchasesByVendor(vendor) = 0
                Next

                result.List.Add(New InventoryTransactionDetailList With {
                    .Month = item.gmb_Month.Month,
                    .OpeningBalance = item.gmb_OpeningBalance,
                    .Sale = item.gmb_SaleTotal,
                    .CloseingBalance = item.gmb_ClosingBalance,
                    .PurchasesByVendor = purchasesByVendor
                })
            Next

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetTax(month As Date) As Tax Implements IReportRep.GetTax
        Try
            Dim result = New Tax With {.Month = month}

            result.List = _context.invoices.Where(Function(x) x.i_Date.Year = month.Year AndAlso x.i_Date.Month = month.Month).
                                            Select(Function(x) New TaxList With {
                                                .Amount = x.i_Amount,
                                                .Day = x.i_Date,
                                                .InvoiceNum = x.i_Number,
                                                .Memo = x.i_Memo,
                                                .Quantity = x.i_KG,
                                                .Tax = x.i_Tax,
                                                .TaxId = x.customer.cus_tax_id,
                                                .UnitPrice = x.i_UnitPrice
                                            }).ToList
            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetBureau(month As Date) As List(Of EnergyBureau) Implements IReportRep.GetBureau
        Try
            Return _context.invoices.Where(Function(x) x.i_Date.Year = month.Year AndAlso x.i_Date.Month = month.Month).
                                     Select(Function(x) New EnergyBureau With {
                                         .Day = x.i_Date,
                                         .InvoiceNum = x.i_Number,
                                         .Quantity = x.i_KG,
                                         .TaxId = x.customer.cus_tax_id
                                     }).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetMonthlyStatement(cusCode As String, month As Date) As MonthlyStatement Implements IReportRep.GetMonthlyStatement
        Try
            Dim result = New MonthlyStatement

            ' 取得客戶該月訂單
            Dim orderByCusAndMonth = _context.orders.Where(Function(x) x.o_date.Value.Year = month.Year AndAlso
                                                                       x.o_date.Value.Month = month.Month AndAlso
                                                                       x.customer.cus_code = cusCode).ToList

            If orderByCusAndMonth.Count = 0 Then Throw New Exception($"該客戶 {month:yyyy/MM} 無訂購資料")

            ' 取得客戶資訊
            Dim cus = orderByCusAndMonth.FirstOrDefault.customer
            result.CusCode = cus.cus_code
            result.CompanyName = cus.company?.comp_name
            result.CusName = cus.cus_name

            ' 分類運送方式
            Dim orderGroup = orderByCusAndMonth.GroupBy(Function(x) x.o_delivery_type).ToList

            For Each group In orderGroup
                Select Case group.Key
                    Case "自運"
                        result.ToGo = New MonthlyStatementDetail
                        SetMonthlyStatementDetail(result.ToGo, group.ToList(), month)
                    Case "廠運"
                        result.Delivery = New MonthlyStatementDetail
                        SetMonthlyStatementDetail(result.Delivery, group.ToList(), month)
                End Select
            Next

            '保險
            result.InsuranceUnitPrice = cus.cus_InsurancePrice

            '本月已收氣款
            Dim col = _context.collections.Where(Function(x) x.col_AccountMonth.Year = month.Year AndAlso
                                                             x.col_AccountMonth.Month = month.Month AndAlso
                                                             x.col_cus_Id = cus.cus_id)

            If col.Count <> 0 Then result.GasAccountsReceived = col.Sum(Function(x) x.col_Amount)

            '新桶
            result.NewBerralAccountsReceivable = col.Where(Function(x) x.col_s_Id.Value = 22).Sum(Function(x) CType(x.col_Amount, Integer?)).GetValueOrDefault()

            ' 報廢桶
            result.ScrapBarrel = _context.scrap_barrel.
                Join(_context.scrap_barrel_detail,
                    Function(sb) sb.sb_Id,
                    Function(sbd) sbd.sbd_sb_Id,
                    Function(sb, sbd) New With {sb, sbd}).
                Where(Function(x) x.sbd.sbd_cus_Id = cus.cus_id AndAlso
                                  x.sb.sb_Month.Value.Year = month.Year AndAlso
                                  x.sb.sb_Month.Value.Month = month.Month AndAlso
                                  x.sbd.sbd_isMonthlyStatement).
                Select(Function(x) x.sbd.sbd_Qty50 * x.sb.sb_Buy50 +
                                   x.sbd.sbd_Qty20 * x.sb.sb_Buy20 +
                                   x.sbd.sbd_Qty16 * x.sb.sb_Buy16 +
                                   x.sbd.sbd_Qty10 * x.sb.sb_Buy10 +
                                   x.sbd.sbd_Qty4 * x.sb.sb_Buy4).
                DefaultIfEmpty(0).
                Sum()

            '註新桶
            Dim newBarrelCounts = New Dictionary(Of String, Integer) From {{"50", 0}, {"20", 0}, {"16", 0}, {"10", 0}, {"4", 0}, {"18", 0}, {"14", 0}, {"5", 0}, {"2", 0}}

            For Each order In orderByCusAndMonth
                newBarrelCounts("50") += order.o_new_in_50
                newBarrelCounts("20") += order.o_new_in_20
                newBarrelCounts("16") += order.o_new_in_16
                newBarrelCounts("10") += order.o_new_in_10
                newBarrelCounts("4") += order.o_new_in_4
                newBarrelCounts("18") += order.o_new_in_18
                newBarrelCounts("14") += order.o_new_in_14
                newBarrelCounts("5") += order.o_new_in_5
                newBarrelCounts("2") += order.o_new_in_2
            Next

            result.IsInsurance = cus.cus_IsInsurance

            Return result
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Function

    Public Function GetInsurance(compId As Integer, month As Date) As Insurance Implements IReportRep.GetInsurance
        Try
            Dim result = New Insurance With {
                .CompanyName = _context.companies.Find(compId).comp_name,
                .Month = month.Date
            }

            result.List = _context.orders.Where(Function(x) x.o_date.Value.Year = month.Year AndAlso
                                                            x.o_date.Value.Month = month.Month AndAlso
                                                            x.customer.cus_comp_Id = compId).
                                          GroupBy(Function(x) New With {
                                              Key .CusCode = x.customer.cus_code,
                                              Key .CusName = x.customer.cus_name,
                                              Key .TaxId = x.customer.cus_tax_id
                                          }).
                                          Select(Function(x) New InsuranceList With {
                                              .Amount = x.Sum(Function(o) o.o_Insurance),
                                              .CusCode = x.Key.CusCode,
                                              .CusName = x.Key.CusName,
                                              .TaxId = x.Key.TaxId
                                          }).OrderBy(Function(x) x.CusCode).ToList

            Return result
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Function

    Public Function GetIncomeStatement(startDate As Date, endDate As Date, compId As Integer) As IncomeStatementModel Implements IReportRep.GetIncomeStatement
        Try
            Dim result = New IncomeStatementModel With {
                .CompanyName = _context.companies.Find(compId).comp_name,
                .DateRange = $"{startDate:yyyy.MM.dd} ~ {endDate:yyyy.MM.dd}"
            }

            Dim formatEndDate = endDate.Date.AddDays(1)
            Dim orders = _context.orders.Where(Function(x) x.o_date >= startDate.Date AndAlso
                                                                       x.o_date < formatEndDate AndAlso
                                                                       x.customer.cus_comp_Id = compId)
            '營業收入
            result.OperatingIncome = orders.Select(Function(x) x.o_total_amount).
                                            DefaultIfEmpty(0).
                                            Sum

            ' 銷貨折讓
            result.SalesDiscount = orders.Select(Function(x) x.o_sales_allowance).
                                          DefaultIfEmpty(0).
                                          Sum

            ' 氣款收入
            result.GasIncome = result.OperatingIncome + result.SalesDiscount

            ' 進貨
            result.Income = _context.purchases.Where(Function(x) x.pur_date >= startDate.Date AndAlso
                                                                 x.pur_date < formatEndDate AndAlso
                                                                 x.pur_comp_id = compId).
                                               Select(Function(x) x.pur_price).
                                               DefaultIfEmpty(0).
                                               Sum

            ' 營業外收益
            result.CollectionsList = _context.collections.Where(Function(x) x.col_Date >= startDate.Date AndAlso
                                                                            x.col_Date < formatEndDate AndAlso
                                                                            x.col_comp_Id = compId).
                                                          GroupBy(Function(g) New With {Key .Name = g.subject.s_name}).
                                                          Select(Function(g) New IncomeStatementItem With {
                                                            .Subject = g.Key.Name,
                                                            .Amount = g.Sum(Function(x) x.col_Amount)
                                                          }).ToList

            ' 營業費用
            result.PaymentList = _context.payments.Where(Function(x) x.p_Date >= startDate.Date AndAlso
                                                                     x.p_Date < formatEndDate AndAlso
                                                                     x.p_comp_Id = compId).
                                                   GroupBy(Function(g) New With {Key .Name = g.subject.s_name}).
                                                   Select(Function(g) New IncomeStatementItem With {
                                                        .Subject = g.Key.Name,
                                                        .Amount = g.Sum(Function(x) x.p_Amount)
                                                   }).ToList

            Return result
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Function

    Public Function GetOutInvoice(year As Integer, months As String) As OutInvoice Implements IReportRep.GetOutInvoice
        Try
            Dim result = New OutInvoice With {
                .Companies = New List(Of CompanyInvoiceData)
            }

            Dim targetCompanies As New List(Of String) From {"豐合能源股份有限公司", "豐牛有限公司"}
            Dim monthPart = months.Split("-"c)
            Dim startMonth = Integer.Parse(monthPart(0))
            Dim endMonth = Integer.Parse(monthPart(1))
            Dim startDate = New Date(year, startMonth, 1)
            Dim endDate = New Date(year, endMonth, 1).AddMonths(1)

            '取得日期內的發票
            Dim invoices = _context.invoices.Where(Function(x) x.i_Date >= startDate AndAlso
                                                               x.i_Date < endDate AndAlso
                                                               Not x.i_IsInvalid AndAlso
                                                               targetCompanies.Contains(x.customer.company.comp_name)).
                                             OrderBy(Function(x) x.i_Date).ToList

            '依公司分組
            For Each companyName In targetCompanies
                Dim companyInvoices = invoices.Where(Function(x) x.customer.company.comp_name = companyName)

                If Not companyInvoices.Any Then Continue For

                Dim companyData As New CompanyInvoiceData With {
                    .CompanyName = companyName,
                    .MonthlyData = New List(Of MonthInvoiceData)
                }

                '依月分處理發票
                For month As Integer = startMonth To endMonth
                    Dim monthData As New MonthInvoiceData With {
                        .Month = month,
                        .RegularInvoices = New List(Of InvoiceGroup),
                        .SpecialInvoices = New SpecialInvoices
                    }
                    Dim month1 As Integer = month
                    Dim monthInvoices = companyInvoices.Where(Function(x) x.i_Date.Month = month1)

                    '處理機開三聯
                    Dim regularInvoices = monthInvoices.Where(Function(x) x.i_InvoiceType = "機開三聯").OrderBy(Function(x) x.i_Number).ToList
                    Dim groupCount = 5
                    For i As Integer = 0 To regularInvoices.Count - 1 Step groupCount
                        Dim group = regularInvoices.Skip(i).Take(groupCount)
                        monthData.RegularInvoices.Add(New InvoiceGroup With {
                            .GroupNumber = i \ groupCount + 1,
                            .Qty = group.Sum(Function(x) x.i_KG),
                            .Amount = group.Sum(Function(x) x.i_Amount),
                            .TaxAmount = group.Sum(Function(x) x.i_Tax)
                        })
                    Next

                    '處理其他發票
                    monthData.SpecialInvoices.TwoPartMachine = ProcessSpecialInvoices(monthInvoices, "機開二聯")
                    monthData.SpecialInvoices.ThreePartHandwritten = ProcessSpecialInvoices(monthInvoices, "手開三聯")
                    monthData.SpecialInvoices.TwoPartHandwritten = ProcessSpecialInvoices(monthInvoices, "手開二聯")

                    companyData.MonthlyData.Add(monthData)
                Next

                result.Companies.Add(companyData)
            Next

            Return result
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Function

    Public Function GetSplitCompany(year As Integer, months As String) As SplitCompanyInvoice Implements IReportRep.GetSplitCompany
        Try
            Dim result As New SplitCompanyInvoice
            Dim monthPart = months.Split("-"c)
            Dim startMonth = Integer.Parse(monthPart(0))
            Dim endMonth = Integer.Parse(monthPart(1))
            Dim startDate = New Date(year, startMonth, 1)
            Dim midDate = New Date(year, startMonth, Date.DaysInMonth(year, startMonth))
            Dim endDate = New Date(year, endMonth, Date.DaysInMonth(year, endMonth)).AddDays(1)

            ' 取得日期內的銷項發票
            Dim splitInvoices = _context.invoice_split.Where(Function(x) x.is_Type = "銷項" AndAlso
                                                                         x.is_Date >= startDate AndAlso
                                                                         x.is_Date < endDate).
                                                       OrderBy(Function(x) x.is_Date).
                                                       ThenBy(Function(x) x.is_comp_Id).ToList

            ' 依公司和月份分組
            Dim groupedInvoices = splitInvoices.GroupBy(Function(x) New With {
                Key .CompId = x.is_comp_Id,
                Key .Month = x.is_Date.Month
            })

            If splitInvoices.Count >= 4 Then
                Dim groups = groupedInvoices.OrderBy(Function(x) x.Key.Month).ThenBy(Function(x) x.Key.CompId).ToList
                ' 第一個月，第一個公司
                result.FrontDate1 = midDate.ToString("MM月dd日")
                result.FrontTax1 = groups(0).Sum(Function(x) x.is_Tax)
                result.FrontAmount1 = groups(0).Sum(Function(x) x.is_Amount)
                result.FrontVendorTaxId1 = groups(0).First.company.comp_tax_id

                ' 第一個月，第二個公司
                result.FrontDate2 = midDate.ToString("MM月dd日")
                result.FrontTax2 = groups(1).Sum(Function(x) x.is_Tax)
                result.FrontAmount2 = groups(1).Sum(Function(x) x.is_Amount)
                result.FrontVendorTaxId2 = groups(1).First.company.comp_tax_id

                ' 第二個月，第一個公司
                result.EndDate1 = endDate.AddDays(-1).ToString("MM月dd日")
                result.EndTax1 = groups(2).Sum(Function(x) x.is_Tax)
                result.EndAmount1 = groups(2).Sum(Function(x) x.is_Amount)
                result.EndVendorTaxId1 = groups(2).First.company.comp_tax_id

                ' 第二個月，第二個公司
                result.EndDate2 = endDate.AddDays(-1).ToString("MM月dd日")
                result.EndTax2 = groups(3).Sum(Function(x) x.is_Tax)
                result.EndAmount2 = groups(3).Sum(Function(x) x.is_Amount)
                result.EndVendorTaxId2 = groups(3).First.company.comp_tax_id
            End If

            ' 取得進項發票
            result.InList = _context.invoice_split.Where(Function(x) x.is_Type = "進項" AndAlso
                                                                     x.is_Date >= startDate AndAlso
                                                                     x.is_Date < endDate).ToList.
                                                   Select(Function(x) New InDetail With {
                                                       .Day = x.is_Date.ToString("MM月dd日"),
                                                       .InvoiceNum = x.is_Number,
                                                       .Name = x.is_Name,
                                                       .Tax = x.is_Tax,
                                                       .Amount = x.is_Amount,
                                                       .VendorTaxId = x.is_VendorTaxId
                                                   }).OrderBy(Function(x) x.Day).ToList
            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function ProcessSpecialInvoices(invoices As IEnumerable(Of invoice), invoiceType As String) As InvoiceGroup
        Try
            Dim specialInvoices = invoices.Where(Function(x) x.i_InvoiceType = invoiceType)
            Return New InvoiceGroup With {
                .Qty = specialInvoices.Sum(Function(x) x.i_KG),
                .Amount = specialInvoices.Sum(Function(x) x.i_Amount),
                .TaxAmount = specialInvoices.Sum(Function(x) x.i_Tax)
            }
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetDailySubjectSummary(day As Date) As List(Of DailySubjectSummary) Implements IReportRep.GetDailySubjectSummary
        Try
            Dim subjectSummary As New Dictionary(Of String, DailySubjectSummary)
            Dim startDate = New Date(day.Year, day.Month, 1)

            ' 取得收入管理當日的科目匯總
            Dim collectionDatas = _context.collections.Where(Function(x) x.col_Date >= startDate AndAlso x.col_Date <= day.Date).AsNoTracking().ToList()

            ' 收入管理 - 借方 (科目名稱)
            For Each group In collectionDatas.GroupBy(Function(x) x.col_Type)
                Dim subject = group.Key
                Dim amount = group.Sum(Function(x) x.col_Amount)

                If Not subjectSummary.ContainsKey(subject) Then
                    subjectSummary(subject) = New DailySubjectSummary With {.Subject = subject, .Debit = 0, .Credit = 0}
                End If
                subjectSummary(subject).Debit += amount
            Next

            ' 收入管理 - 貸方 (收入類型)
            For Each group In collectionDatas.GroupBy(Function(x) x.subject.s_name)
                Dim subject = group.Key
                Dim amount = group.Sum(Function(x) x.col_Amount)

                If Not subjectSummary.ContainsKey(subject) Then
                    subjectSummary(subject) = New DailySubjectSummary With {.Subject = subject, .Debit = 0, .Credit = 0}
                End If
                subjectSummary(subject).Credit += amount
            Next

            ' 取得支出管理當日的科目彙總
            Dim paymentDatas = _context.payments.Where(Function(x) x.p_Date >= startDate AndAlso x.p_Date <= day.Date).AsNoTracking().ToList()

            ' 支出管理 - 貸方 (科目名稱)
            For Each group In paymentDatas.GroupBy(Function(x) x.p_Type)
                Dim subject = group.Key
                Dim amount = group.Sum(Function(x) x.p_Amount)

                If Not subjectSummary.ContainsKey(subject) Then
                    subjectSummary(subject) = New DailySubjectSummary With {.Subject = subject, .Debit = 0, .Credit = 0}
                End If
                subjectSummary(subject).Credit += amount
            Next

            ' 支出管理 - 借方 (支出類型)
            For Each group In paymentDatas.GroupBy(Function(x) x.subject.s_name)
                Dim subject = group.Key
                Dim amount = group.Sum(Function(x) x.p_Amount)

                If Not subjectSummary.ContainsKey(subject) Then
                    subjectSummary(subject) = New DailySubjectSummary With {.Subject = subject, .Debit = 0, .Credit = 0}
                End If
                subjectSummary(subject).Debit += amount
            Next

            ' 取得結轉
            Dim closingEntrie = _context.closing_entry.AsNoTracking.Where(Function(x) x.ce_Date >= startDate AndAlso x.ce_Date <= day.Date)

            '結轉-貸方
            For Each group In closingEntrie.GroupBy(Function(x) x.subject.s_name)
                Dim subject = group.Key
                Dim amount = group.Sum(Function(x) x.ce_CreditAmount)

                If Not subjectSummary.ContainsKey(subject) Then
                    subjectSummary(subject) = New DailySubjectSummary With {.Subject = subject, .Debit = 0, .Credit = 0}
                End If
                subjectSummary(subject).Credit += amount
            Next

            ' 結轉-借方
            For Each group In closingEntrie.GroupBy(Function(x) x.subject1.s_name)
                Dim subject = group.Key
                Dim amount = group.Sum(Function(x) x.ce_DebitAmount)

                If Not subjectSummary.ContainsKey(subject) Then
                    subjectSummary(subject) = New DailySubjectSummary With {.Subject = subject, .Debit = 0, .Credit = 0}
                End If
                subjectSummary(subject).Debit += amount
            Next

            Return subjectSummary.Values.ToList()

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetAccountBalance(month As Date) As List(Of AccountBalanceDTO) Implements IReportRep.GetAccountBalance
        Try
            Dim result As New List(Of AccountBalanceDTO)

            ' 設定查詢時間範圍
            Dim startDate = New Date(month.Year, month.Month, 1)
            Dim endDate = startDate.AddMonths(1)

            ' 取得 collection 資料
            Dim collectionData = _context.collections.AsNoTracking().
                Where(Function(x) x.col_Date >= startDate AndAlso x.col_Date < endDate).
                Include(Function(x) x.subject).
                ToList()

            ' 取得 payment 資料
            Dim paymentData = _context.payments.AsNoTracking().
                Where(Function(x) x.p_Date >= startDate AndAlso x.p_Date < endDate).
                Include(Function(x) x.subject).
                ToList()

            ' 取得 closingEntry 資料
            Dim closingEntryData = _context.closing_entry.AsNoTracking.Where(Function(x) x.ce_Date >= startDate AndAlso x.ce_Date < endDate).ToList

            ' 處理 collection 資料
            ' collection: 借方=col_Type, 貸方=col_s_Id (科目)
            For Each item In collectionData
                ' 處理借方科目 (col_Type)
                Dim debitSubject = result.FirstOrDefault(Function(x) x.SubjectName = item.col_Type)

                If debitSubject Is Nothing Then
                    debitSubject = New AccountBalanceDTO With {
                        .SubjectName = item.col_Type,
                        .DebitList = New List(Of SubjectTransaction),
                        .CreditList = New List(Of SubjectTransaction)
                    }
                    result.Add(debitSubject)
                End If

                ' 處理貸方科目 (col_s_Id 關聯的科目)
                Dim creditSubject = result.FirstOrDefault(Function(x) x.SubjectName = item.subject.s_name)

                If creditSubject Is Nothing Then
                    creditSubject = New AccountBalanceDTO With {
                        .SubjectName = item.subject.s_name,
                        .DebitList = New List(Of SubjectTransaction),
                        .CreditList = New List(Of SubjectTransaction)
                    }
                    result.Add(creditSubject)
                End If

                ' 處理明細
                Dim transaction As New SubjectTransaction With {
                    .Day = item.col_Date,
                    .Amount = item.col_Amount
                }
                debitSubject.DebitList.Add(transaction)
                creditSubject.CreditList.Add(transaction)
            Next

            ' 處理 payment 資料
            ' payment: 借方=p_s_Id (科目), 貸方=p_Type
            For Each item In paymentData
                ' 處理借方科目 (p_s_Id 關聯的科目)
                Dim debitSubject = result.FirstOrDefault(Function(x) x.SubjectName = item.subject.s_name)

                If debitSubject Is Nothing Then
                    debitSubject = New AccountBalanceDTO With {
                        .SubjectName = item.subject.s_name,
                        .DebitList = New List(Of SubjectTransaction),
                        .CreditList = New List(Of SubjectTransaction)
                    }
                    result.Add(debitSubject)
                End If

                ' 處理貸方科目 (p_Type)
                Dim creditSubject = result.FirstOrDefault(Function(x) x.SubjectName = item.p_Type)

                If creditSubject Is Nothing Then
                    creditSubject = New AccountBalanceDTO With {
                        .SubjectName = item.p_Type,
                        .DebitList = New List(Of SubjectTransaction),
                        .CreditList = New List(Of SubjectTransaction)
                    }
                    result.Add(creditSubject)
                End If

                ' 處理明細
                Dim transaction As New SubjectTransaction With {
                    .Day = item.p_Date,
                    .Amount = item.p_Amount
                }

                debitSubject.DebitList.Add(transaction)
                creditSubject.CreditList.Add(transaction)
            Next

            ' 處理 closing_entry 資料  
            ' closing_entry: 借方=subject1 (ce_Debit), 貸方=subject (ce_Credit)
            For Each item In closingEntryData
                ' 先檢查必要的資料是否存在
                If item.subject1 IsNot Nothing AndAlso item.subject IsNot Nothing AndAlso
                   item.ce_Date.HasValue AndAlso item.ce_DebitAmount.HasValue AndAlso item.ce_CreditAmount.HasValue Then

                    ' 處理借方科目 (subject1)
                    Dim debitSubject = result.FirstOrDefault(Function(x) x.SubjectName = item.subject1.s_name)

                    If debitSubject Is Nothing Then
                        debitSubject = New AccountBalanceDTO With {
                            .SubjectName = item.subject1.s_name,
                            .DebitList = New List(Of SubjectTransaction),
                            .CreditList = New List(Of SubjectTransaction)
                        }
                        result.Add(debitSubject)
                    End If

                    ' 處理貸方科目 (subject)
                    Dim creditSubject = result.FirstOrDefault(Function(x) x.SubjectName = item.subject.s_name)

                    If creditSubject Is Nothing Then
                        creditSubject = New AccountBalanceDTO With {
                            .SubjectName = item.subject.s_name,
                            .DebitList = New List(Of SubjectTransaction),
                            .CreditList = New List(Of SubjectTransaction)
                        }
                        result.Add(creditSubject)
                    End If

                    ' 分別建立借方和貸方交易記錄（金額可能不同）
                    Dim debitTransaction As New SubjectTransaction With {
                        .Day = item.ce_Date.Value,
                        .Amount = item.ce_DebitAmount.Value
                    }
                    debitSubject.DebitList.Add(debitTransaction)

                    Dim creditTransaction As New SubjectTransaction With {
                        .Day = item.ce_Date.Value,
                        .Amount = item.ce_CreditAmount.Value
                    }
                    creditSubject.CreditList.Add(creditTransaction)
                End If
            Next

            Return result

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetSurplusGasReport(data As surplus_gas) As SurplusGasDTO Implements IReportRep.GetSurplusGasReport
        Try
            Dim result As New SurplusGasDTO With {
                .Month = data.sg_Moth,
                .Platform = data.sg_Platform,
                .Platform_C = data.sg_Platform_C,
                .Slot = data.sg_Slot,
                .Slot_C = data.sg_Slot_C,
                .Car = data.sg_Car,
                .Car_C = data.sg_Car_C,
                .Sell = data.sg_Sell
            }

            ' 取得該月份的進貨資料，包含廠商資訊，並只篩選瓦斯廠商
            Dim purchases = _context.purchases.AsNoTracking.
                Where(Function(x) x.pur_date.Value.Year = data.sg_Moth.Value.Year AndAlso x.pur_date.Value.Month = data.sg_Moth.Value.Month).
                Include(Function(p) p.manufacturer).
                Where(Function(p) p.manufacturer.manu_GasVendor = True).ToList

            ' 按廠商分組並統計普氣/丙氣數量
            Dim purchaseDetails = purchases.GroupBy(Function(p) p.pur_manu_id).
                Select(Function(g) New PurchaseCompanyDetail With {
                    .CompanyName = g.First().manufacturer.manu_name,
                    .Gas = g.Where(Function(p) p.pur_product = "普氣").
                           Sum(Function(p) If(p.pur_quantity, 0)),
                    .Gas_C = g.Where(Function(p) p.pur_product = "丙氣").
                             Sum(Function(p) If(p.pur_quantity, 0))
                }).ToList

            result.PurchaseDetails = purchaseDetails

            ' 取得總提氣量
            result.TotalOrder = _context.orders.AsNoTracking.
                Where(Function(x) x.o_date.Value >= data.sg_StartDate AndAlso x.o_date.Value <= data.sg_EndDate).
                Sum(Function(x) x.o_gas_total + x.o_gas_c_total)

            ' 取得上月結餘氣
            Dim lastMonth = data.sg_Moth.Value.AddMonths(-1)
            Dim lastSurplus = _context.surplus_gas.AsNoTracking.FirstOrDefault(Function(x) x.sg_Moth = lastMonth)
            If lastSurplus IsNot Nothing Then result.LastMonthSurplus = lastSurplus.sg_Total

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub SetMonthlyStatementDetail(ByRef result As MonthlyStatementDetail, data As List(Of order), month As Date)
        Dim firstDate = New Date(month.Year, month.Month, 1)
        Dim orderByFirstDate = data.Where(Function(x) x.o_date.Value.Date = firstDate)

        If orderByFirstDate.Count <> 0 Then
            '家用瓦斯(變動前)
            result.GasNormalQuantity_First = orderByFirstDate.Sum(Function(x) x.o_gas_total)
            result.GasNormalUnitPrice_First = If(orderByFirstDate.FirstOrDefault.o_UnitPrice, 0)

            '工業氣(變動前)
            result.GasCQuantity_First = orderByFirstDate.Sum(Function(x) x.o_gas_c_total)
            result.GasCUnitPrice_First = If(orderByFirstDate.FirstOrDefault.o_UnitPriceC, 0)
        End If

        Dim orderByOtherDate = data.Where(Function(x) x.o_date.Value.Date <> firstDate)

        If orderByOtherDate.Count <> 0 Then
            '家用瓦斯(變動後)
            result.GasNormalQuantity = orderByOtherDate.Sum(Function(x) x.o_gas_total)
            result.GasNormalUnitPrice = If(orderByOtherDate.FirstOrDefault.o_UnitPrice, 0)

            '工業氣(變動後)
            result.GasCQuantity = orderByOtherDate.Sum(Function(x) x.o_gas_c_total)
            result.GasCUnitPrice = If(orderByOtherDate.FirstOrDefault.o_UnitPriceC, 0)
        End If
    End Sub
End Class