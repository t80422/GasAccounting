Public Class CusOrdByCarService
    Implements ICusOrdByCarService

    Private _cusRep As ICustomerRepository
    Private _ordRep As IOrderRepository
    Private _carRep As ICarRepository
    Private _bpRep As IBasicPriceRep
    Private _context As gas_accounting_systemEntities

    Public Sub New(cusRep As ICustomerRepository, ordRep As IOrderRepository, carRep As ICarRepository, bpRep As IBasicPriceRep, context As gas_accounting_systemEntities)
        _cusRep = cusRep
        _ordRep = ordRep
        _carRep = carRep
        _bpRep = bpRep
        _context = context
    End Sub

    Public ReadOnly Property OrdRep As IOrderRepository Implements ICusOrdByCarService.OrdRep
        Get
            Return _ordRep
        End Get
    End Property

    Public ReadOnly Property CusRep As ICustomerRepository Implements ICusOrdByCarService.CusRep
        Get
            Return _cusRep
        End Get
    End Property

    Public ReadOnly Property CarRep As ICarRepository Implements ICusOrdByCarService.CarRep
        Get
            Return _carRep
        End Get
    End Property

    Public ReadOnly Property BPRep As IBasicPriceRep Implements ICusOrdByCarService.BPRep
        Get
            Return _bpRep
        End Get
    End Property

    Public Sub Insert(order As order, customer As customer, car As car) Implements ICusOrdByCarService.Insert
        Using transaction = _context.Database.BeginTransaction
            Try
                _ordRep.Insert(order)
                _cusRep.Update(customer)
                _carRep.Update(car)

                _ordRep.Save()
                _cusRep.Save()
                _carRep.Save()

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw
            End Try
        End Using
    End Sub

    Public Sub Update(order As order, customer As customer, car As car, orgCar As car, orgOrd As order) Implements ICusOrdByCarService.Update
        Using transaction = _context.Database.BeginTransaction
            Try
                _ordRep.Update(order)
                _cusRep.Update(customer)
                _carRep.Update(car)

                If car.c_id <> orgCar.c_id Then
                    If order.o_in_out = "進場單" Then
                        '寄存桶
                        orgCar.c_deposit_50 -= orgOrd.o_deposit_in_50
                        orgCar.c_deposit_20 -= orgOrd.o_deposit_in_20
                        orgCar.c_deposit_16 -= orgOrd.o_deposit_in_16
                        orgCar.c_deposit_10 -= orgOrd.o_deposit_in_10
                        orgCar.c_deposit_4 -= orgOrd.o_deposit_in_4
                        orgCar.c_deposit_15 -= orgOrd.o_deposit_in_15
                        orgCar.c_deposit_14 -= orgOrd.o_deposit_in_14
                        orgCar.c_deposit_5 -= orgOrd.o_deposit_in_5
                        orgCar.c_deposit_2 -= orgOrd.o_deposit_in_2

                        '檢驗桶

                    Else
                        orgCar.c_deposit_50 += orgOrd.o_deposit_out_50
                        orgCar.c_deposit_20 += orgOrd.o_deposit_out_20
                        orgCar.c_deposit_16 += orgOrd.o_deposit_out_16
                        orgCar.c_deposit_10 += orgOrd.o_deposit_out_10
                        orgCar.c_deposit_4 += orgOrd.o_deposit_out_4
                        orgCar.c_deposit_15 += orgOrd.o_deposit_out_15
                        orgCar.c_deposit_14 += orgOrd.o_deposit_out_14
                        orgCar.c_deposit_5 += orgOrd.o_deposit_out_5
                        orgCar.c_deposit_2 += orgOrd.o_deposit_out_2
                    End If
                End If

                _ordRep.Save()
                _cusRep.Save()
                _carRep.Save()

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw
            End Try
        End Using
    End Sub

    Private Sub Delete(order As order, car As car, customer As customer) Implements ICusOrdByCarService.Delete
        Using transation = _context.Database.BeginTransaction
            Try
                _context.orders.Remove(order)

                '更新車輛
                Dim exCar = _context.cars.Find(car.c_id)
                _context.Entry(exCar).CurrentValues.SetValues(car)

                '更新客戶
                Dim exCus = _context.customers.Find(customer.cus_id)
                _context.Entry(exCus).CurrentValues.SetValues(customer)

                _context.SaveChanges()

                transation.Commit()
            Catch ex As Exception
                transation.Rollback()
                Throw
            End Try
        End Using
    End Sub

    Public Function SearchOrders(Optional criteria As OrderSearchCriteria = Nothing) As List(Of order) Implements ICusOrdByCarService.SearchOrders
        Dim query = _context.orders.AsQueryable

        If criteria IsNot Nothing Then
            If criteria.StartDate <> Date.MinValue Then
                query = query.Where(Function(x) x.o_date >= criteria.StartDate)
            End If

            If criteria.EndDate <> Date.MinValue Then
                query = query.Where(Function(x) x.o_date < criteria.EndDate)
            End If

            If Not String.IsNullOrEmpty(criteria.CusCode) Then
                query = query.Where(Function(x) x.car.customer.cus_code.Contains(criteria.CusCode))
            End If

            If Not String.IsNullOrEmpty(criteria.InOut) Then
                query = query.Where(Function(x) x.o_in_out = criteria.InOut)
            End If
        End If

        Return query.ToList
    End Function
End Class