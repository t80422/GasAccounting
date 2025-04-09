Public Class MonthlyAccountService
    Implements IMonthlyAccountService

    ''' <summary>
    ''' 初始化月度帳單資料 (將歷史訂單資料遷移到月度帳單表)
    ''' </summary>
    Public Function InitializeMonthlyAccounts() As Boolean Implements IMonthlyAccountService.InitializeMonthlyAccounts
        Try
            Using db As New gas_accounting_systemEntities
                ' 檢查是否已有資料
                If db.monthly_account.Any() Then
                    ' 清空現有資料
                    db.monthly_account.RemoveRange(db.monthly_account)
                    db.SaveChanges()
                End If

                ' 按客戶和月份分組計算訂單總額
                Dim ordersWithValidData = db.orders.
                    Where(Function(o) o.o_cus_Id.HasValue AndAlso
                                     o.o_date.HasValue)

                Dim groupedOrders = ordersWithValidData.
                    GroupBy(Function(o) New With {
                        Key .CustomerId = o.o_cus_Id.Value,
                        o.o_date.Value.Year,
                        o.o_date.Value.Month
                    }).ToList()

                ' 建立月度帳單記錄
                For Each group In groupedOrders
                    Dim totalAmount = group.Sum(Function(o) o.o_total_amount)
                    
                    ' 初始化時，設定所有金額為未付款
                    Dim paidAmount As Decimal = 0
                    Dim unpaidAmount = totalAmount

                    ' 建立月度帳單記錄
                    Dim monthlyAccount = New monthly_account With {
                        .ma_cus_id = group.Key.CustomerId,
                        .ma_year = group.Key.Year,
                        .ma_month = group.Key.Month,
                        .ma_total_amount = totalAmount,
                        .ma_unpaid_amount = unpaidAmount,
                        .ma_paid_amount = paidAmount,
                        .ma_status = False,
                        .ma_last_updated = Date.Now
                    }

                    db.monthly_account.Add(monthlyAccount)
                Next

                db.SaveChanges()
                Return True
            End Using
        Catch ex As Exception
            Console.WriteLine("初始化月度帳單時發生錯誤: " & ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 同步特定訂單的月度帳單資料
    ''' </summary>
    Public Function SyncOrderToMonthlyAccount(order As order, isNew As Boolean, isDelete As Boolean) As Boolean Implements IMonthlyAccountService.SyncOrderToMonthlyAccount
        Try
            If Not order.o_date.HasValue OrElse Not order.o_cus_Id.HasValue Then
                Return False
            End If

            Using db As New gas_accounting_systemEntities
                Dim year = order.o_date.Value.Year
                Dim month = order.o_date.Value.Month
                Dim customerId = order.o_cus_Id.Value

                ' 查找該月的記錄
                Dim monthlyAccount = db.monthly_account.
                    FirstOrDefault(Function(x) x.ma_cus_id = customerId AndAlso
                                              x.ma_year = year AndAlso
                                              x.ma_month = month)
                
                ' 新訂單全額視為未付款
                Dim orderAmount = order.o_total_amount

                If isDelete Then
                    ' 刪除訂單時減少金額
                    If monthlyAccount IsNot Nothing Then
                        monthlyAccount.ma_total_amount -= orderAmount
                        monthlyAccount.ma_unpaid_amount -= orderAmount
                        
                        If monthlyAccount.ma_total_amount <= 0 Then
                            ' 如果該月已無訂單，刪除月度記錄
                            db.monthly_account.Remove(monthlyAccount)
                        Else
                            monthlyAccount.ma_status = monthlyAccount.ma_unpaid_amount <= 0
                            monthlyAccount.ma_last_updated = Date.Now
                        End If
                    End If
                ElseIf isNew Then
                    ' 新增訂單
                    If monthlyAccount Is Nothing Then
                        ' 如果不存在，創建新記錄
                        monthlyAccount = New monthly_account With {
                            .ma_cus_id = customerId,
                            .ma_year = year,
                            .ma_month = month,
                            .ma_total_amount = orderAmount,
                            .ma_paid_amount = 0,
                            .ma_unpaid_amount = orderAmount,
                            .ma_status = False,
                            .ma_last_updated = Date.Now
                        }
                        db.monthly_account.Add(monthlyAccount)
                    Else
                        ' 如果存在，增加金額
                        monthlyAccount.ma_total_amount += orderAmount
                        monthlyAccount.ma_unpaid_amount += orderAmount
                        monthlyAccount.ma_status = False
                        monthlyAccount.ma_last_updated = Date.Now
                    End If
                Else
                    ' 更新訂單
                    ' 檢索原始訂單資料進行比較
                    Dim oldOrder = db.orders.AsNoTracking.FirstOrDefault(Function(x) x.o_id = order.o_id)

                    If oldOrder IsNot Nothing Then
                        Dim amountDiff = orderAmount - oldOrder.o_total_amount

                        If monthlyAccount Is Nothing Then
                            ' 如果不存在，創建新記錄
                            monthlyAccount = New monthly_account With {
                                .ma_cus_id = customerId,
                                .ma_year = year,
                                .ma_month = month,
                                .ma_total_amount = orderAmount,
                                .ma_paid_amount = 0,
                                .ma_unpaid_amount = orderAmount,
                                .ma_status = False,
                                .ma_last_updated = Date.Now
                            }
                            db.monthly_account.Add(monthlyAccount)
                        Else
                            ' 更新金額
                            monthlyAccount.ma_total_amount += amountDiff
                            monthlyAccount.ma_unpaid_amount += amountDiff
                            monthlyAccount.ma_status = monthlyAccount.ma_unpaid_amount <= 0
                            monthlyAccount.ma_last_updated = Date.Now
                        End If
                    End If
                End If

                db.SaveChanges()
                Return True
            End Using
        Catch ex As Exception
            Console.WriteLine("更新月度帳單時發生錯誤: " & ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 更新銷帳後的月度帳單資料
    ''' </summary>
    Public Function UpdateMonthlyAccountAfterWriteOff(customerId As Integer, year As Integer, month As Integer, writeOffAmount As Integer) As Boolean Implements IMonthlyAccountService.UpdateMonthlyAccountAfterWriteOff
        Try
            Using db As New gas_accounting_systemEntities
                ' 查找該月的記錄
                Dim monthlyAccount = db.monthly_account.
                    FirstOrDefault(Function(x) x.ma_cus_id = customerId AndAlso
                                               x.ma_year = year AndAlso
                                               x.ma_month = month)

                If monthlyAccount IsNot Nothing Then
                    ' 更新銷帳金額
                    monthlyAccount.ma_paid_amount += writeOffAmount
                    monthlyAccount.ma_unpaid_amount -= writeOffAmount
                    monthlyAccount.ma_status = monthlyAccount.ma_unpaid_amount <= 0
                    monthlyAccount.ma_last_updated = Date.Now

                    db.SaveChanges()
                    Return True
                End If

                Return False
            End Using
        Catch ex As Exception
            Console.WriteLine("更新銷帳後的月度帳單時發生錯誤: " & ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 取得客戶未結案的月度帳單資料
    ''' </summary>
    Public Function GetCustomerUnpaidMonths(customerId As Integer) As List(Of monthly_account) Implements IMonthlyAccountService.GetCustomerUnpaidMonths
        Try
            Using db As New gas_accounting_systemEntities
                Return db.monthly_account.
                    Where(Function(x) x.ma_cus_id = customerId AndAlso
                                      x.ma_unpaid_amount > 0).
                    OrderByDescending(Function(x) x.ma_year).
                    ThenByDescending(Function(x) x.ma_month).
                    ToList()
            End Using
        Catch ex As Exception
            Console.WriteLine("取得未結案月度帳單時發生錯誤: " & ex.Message)
            Return New List(Of monthly_account)
        End Try
    End Function
End Class