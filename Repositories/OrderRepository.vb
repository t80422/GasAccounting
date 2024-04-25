Imports System.Data.Entity.Infrastructure
Imports System.Data.Entity.Validation

Public Class OrderRepository
    Implements IOrderRepository

    Public Function QueryOrders(condition As OrderQueryVM, isQuery As Boolean) As List(Of order) Implements IOrderRepository.QueryOrders
        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.orders.Include("car").Include("car.customer").Include("employee")

                If condition.CarId <> Nothing Then query = query.Where(Function(x) x.o_c_id = condition.CarId)
                If condition.CusId <> Nothing Then query = query.Where(Function(x) x.o_id = condition.CusId)
                If isQuery Then
                    If condition.StartDate <> Nothing Then query = query.Where(Function(x) x.o_date <= condition.StartDate)
                    If condition.EndDate <> Nothing Then query = query.Where(Function(x) x.o_date <= condition.EndDate)
                End If


                Return query.ToList
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetOrderById(id As Integer) As order Implements IOrderRepository.GetOrderById
        Try
            Using db As New gas_accounting_systemEntities
                Return db.orders.Find(id)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub Add(data As order, car As car, cus As customer) Implements IOrderRepository.Add
        Using db As New gas_accounting_systemEntities
            Using transaction = db.Database.BeginTransaction()
                Try
                    db.orders.Add(data)
                    Dim oldCar = db.cars.Find(car.c_id)
                    db.Entry(oldCar).CurrentValues.SetValues(car)
                    Dim oldCus = db.customers.Find(cus.cus_id)
                    db.Entry(oldCus).CurrentValues.SetValues(cus)
                    db.SaveChanges()
                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Public Sub Edit(ord As order, car As car, cus As customer) Implements IOrderRepository.Edit
        Using db As New gas_accounting_systemEntities
            Using transation = db.Database.BeginTransaction
                Try
                    '更新訂單
                    Dim exOrd = db.orders.Find(ord.o_id)
                    db.Entry(exOrd).CurrentValues.SetValues(ord)

                    '更新車輛
                    Dim exCar = db.cars.Find(car.c_id)
                    db.Entry(exCar).CurrentValues.SetValues(car)

                    '更新客戶
                    Dim exCus = db.customers.Find(cus.cus_id)
                    db.Entry(exCus).CurrentValues.SetValues(cus)

                    db.SaveChanges()
                    transation.Commit()
                Catch ex As Exception
                    transation.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Public Sub Delete(ordId As Integer, car As car, cus As customer) Implements IOrderRepository.Delete
        Using db As New gas_accounting_systemEntities
            Using transation = db.Database.BeginTransaction
                Try
                    Dim ord = db.orders.Find(ordId)
                    db.orders.Remove(ord)

                    '更新車輛
                    Dim exCar = db.cars.Find(car.c_id)
                    db.Entry(exCar).CurrentValues.SetValues(car)

                    '更新客戶
                    Dim exCus = db.customers.Find(cus.cus_id)
                    db.Entry(exCus).CurrentValues.SetValues(cus)

                    db.SaveChanges()
                    transation.Commit()
                Catch ex As Exception
                    transation.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub
End Class
