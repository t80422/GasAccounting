Public Class OrderRepository
    Inherits Repository_old(Of order)
    Implements IOrderRepository

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    'Public Function QueryOrders(condition As OrderSearchCriteria, isQuery As Boolean) As List(Of order) Implements IOrderRepository.QueryOrders
    '    Try
    '        Using db As New gas_accounting_systemEntities
    '            Dim query = db.orders.Include("car").Include("car.customer").Include("employee")

    '            If isQuery Then
    '                If condition.StartDate <> Nothing Then query = query.Where(Function(x) x.o_date >= condition.StartDate)
    '                If condition.EndDate <> Nothing Then query = query.Where(Function(x) x.o_date < condition.EndDate)
    '                If condition.CusCode <> Nothing Then query = query.Where(Function(x) x.car.customer.cus_code = condition.CusCode)
    '            End If

    '            Return query.ToList
    '        End Using
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function GetOrderById(id As Integer) As order Implements IOrderRepository.GetOrderById
    '    Try
    '        Using db As New gas_accounting_systemEntities
    '            Dim order = db.orders.Include("car").Include("car.customer").Include("employee").FirstOrDefault(Function(o) o.o_id = id)
    '            Return order
    '        End Using
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    'Public Sub Edit(ord As order, car As car, cus As customer) Implements IOrderRepository.Edit
    '    Using db As New gas_accounting_systemEntities
    '        Using transation = db.Database.BeginTransaction
    '            Try
    '                '更新訂單
    '                Dim exOrd = db.orders.Find(ord.o_id)
    '                db.Entry(exOrd).CurrentValues.SetValues(ord)

    '                '更新車輛
    '                Dim exCar = db.cars.Find(car.c_id)
    '                db.Entry(exCar).CurrentValues.SetValues(car)

    '                '更新客戶
    '                Dim exCus = db.customers.Find(cus.cus_id)
    '                db.Entry(exCus).CurrentValues.SetValues(cus)

    '                db.SaveChanges()
    '                transation.Commit()
    '            Catch ex As DbUpdateException
    '                Dim innerException = ex.InnerException

    '                While innerException IsNot Nothing
    '                    Console.WriteLine(innerException.Message)
    '                    innerException = innerException.InnerException
    '                End While

    '                transation.Rollback()
    '                Throw
    '            Catch ex As Exception
    '                transation.Rollback()
    '                Throw
    '            End Try
    '        End Using
    '    End Using
    'End Sub

    'Public Sub Delete(ordId As Integer, car As car, cus As customer) Implements IOrderRepository.Delete
    '    Using db As New gas_accounting_systemEntities
    '        Using transation = db.Database.BeginTransaction
    '            Try
    '                Dim ord = db.orders.Find(ordId)
    '                db.orders.Remove(ord)

    '                '更新車輛
    '                Dim exCar = db.cars.Find(car.c_id)
    '                db.Entry(exCar).CurrentValues.SetValues(car)

    '                '更新客戶
    '                Dim exCus = db.customers.Find(cus.cus_id)
    '                db.Entry(exCus).CurrentValues.SetValues(cus)

    '                db.SaveChanges()
    '                transation.Commit()
    '            Catch ex As Exception
    '                transation.Rollback()
    '                Throw
    '            End Try
    '        End Using
    '    End Using
    'End Sub

    Public Function GetOrderVoucherData(orderId As Integer) As OrderVoucherVM Implements IOrderRepository.GetOrderVoucherData
        Try
            Using db As New gas_accounting_systemEntities
                Dim order = db.orders.AsNoTracking.FirstOrDefault(Function(x) x.o_id = orderId)

                If order IsNot Nothing Then
                    Dim result = New OrderVoucherVM(order)
                    Dim year = order.o_date.Value.Year
                    Dim month = order.o_date.Value.Month
                    Dim startDate = New DateTime(year, month, 1)
                    Dim endDate = startDate.AddMonths(1)
                    Dim orderMonthData = db.orders.AsNoTracking.
                                     Where(Function(x) x.o_date >= startDate AndAlso x.o_date < endDate).ToList
                    Dim orderMonth = orderMonthData.Select(Function(x) New OrderVoucherVM(x))

                    result.本月累計實提量 = orderMonth.Sum(Function(x) x.本日提量)
                    result.本月累計退氣 = orderMonth.Sum(Function(x) x.本日退氣)

                    '確認是當月還是全部
                    result.尚欠氣款 = 0
                    result.已收氣款 = 0
                    Return result
                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return Nothing
    End Function
End Class
