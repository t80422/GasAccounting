Imports System.Data.Entity
Imports System.Data.Entity.Core.Mapping

Public Class OrderRep
    Inherits Repository(Of order)
    Implements IOrderRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetOrderVoucherData(orderId As Integer) As OrderVoucherVM Implements IOrderRep.GetOrderVoucherData
        Try
            Dim order = _dbSet.AsNoTracking.FirstOrDefault(Function(x) x.o_id = orderId)
            If order IsNot Nothing Then
                Dim result = New OrderVoucherVM(order)
                Dim year = order.o_date.Value.Year
                Dim month = order.o_date.Value.Month
                Dim startDate = New DateTime(year, month, 1)
                Dim endDate = startDate.AddMonths(1)
                'Dim orderMonthData = _dbSet.AsNoTracking.
                '                     Where(Function(x) x.o_cus_Id = order.o_cus_Id AndAlso x.o_date >= startDate AndAlso x.o_date < endDate).
                '                     ToList
                'Dim orderMonth = orderMonthData.Select(Function(x) New OrderVoucherVM(x))
                Dim baseQuery = _dbSet.AsNoTracking().
                                Where(Function(x) x.o_date.Value >= startDate AndAlso
                                                 x.o_date.Value < endDate AndAlso
                                                 x.o_cus_Id = order.o_cus_Id)

                Dim count_t = baseQuery.Sum(Function(x) _
                                                x.o_gas_c_50 * 50 + x.o_gas_50 * 50 +
                                                x.o_gas_c_20 * 20 + x.o_gas_20 * 20 +
                                                x.o_gas_c_16 * 16 + x.o_gas_16 * 16 +
                                                x.o_gas_c_10 * 10 + x.o_gas_10 * 10 +
                                                x.o_gas_c_4 * 4 + x.o_gas_4 * 4 +
                                                x.o_gas_c_18 * 18 + x.o_gas_18 * 18 +
                                                x.o_gas_c_14 * 14 + x.o_gas_14 * 14 +
                                                x.o_gas_c_2 * 2 + x.o_gas_2 * 2 +
                                                x.o_gas_c_5 * 5 + x.o_gas_5 * 5
                )

                Dim count_re = baseQuery.Sum(Function(x) x.o_return + x.o_return_c)

                result.本月累計實提量 = count_t
                result.本月累計退氣 = count_re

                '確認是當月還是全部
                result.尚欠氣款 = 0
                result.已收氣款 = 0
                Return result
            End If
        Catch ex As Exception
            Throw
        End Try

        Return Nothing
    End Function

    Public Async Function SearchAsync(criteria As OrderSearchCriteria) As Task(Of List(Of order)) Implements IOrderRep.SearchAsync
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If Not criteria.SearchIn Then query = query.Where(Function(x) x.o_in_out <> "進場單")
            If Not criteria.SearchOut Then query = query.Where(Function(x) x.o_in_out <> "出場單")

            Dim startDate = criteria.StartDate.Date
            Dim endDate = criteria.EndDate.Date.AddDays(1)

            If criteria.IsDate Then
                query = query.Where(Function(x) x.o_date >= startDate)
                query = query.Where(Function(x) x.o_date < endDate)
            End If

            If criteria.CusId <> 0 Then query = query.Where(Function(x) x.o_cus_Id = criteria.CusId)

            Return Await query.OrderByDescending(Function(x) x.o_date).ThenByDescending(Function(x) x.o_id).ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetByMonth(month As Date) As List(Of order) Implements IOrderRep.GetByMonth
        Try
            Return _dbSet.Where(Function(x) x.o_date.Value.Year = month.Year AndAlso x.o_date.Value.Month = month.Month).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetByMonthAndCompany(month As Date, compId As Integer) As List(Of order) Implements IOrderRep.GetByMonthAndCompany
        Try
            Return _dbSet.Where(Function(x) x.o_date.Value.Year = month.Year AndAlso x.o_date.Value.Month = month.Month AndAlso x.customer.cus_comp_Id = compId).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetByCusIdAndDate(cusId As Integer, day As Date) As List(Of order) Implements IOrderRep.GetByCusIdAndDate
        Try
            Return _dbSet.AsNoTracking.Where(Function(x) x.o_date.Value.Year = day.Year AndAlso
                                            x.o_date.Value.Month = day.Month AndAlso
                                            x.o_date.Value.Day = day.Day AndAlso
                                            x.o_cus_Id = cusId).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetLastOrder(cusId As Integer) As order Implements IOrderRep.GetLastOrder
        Try
            Return _dbSet.AsNoTracking.
                Where(Function(x) x.o_cus_Id = cusId).
                OrderByDescending(Function(x) x.o_date).
                ThenByDescending(Function(x) x.o_id).
                FirstOrDefault()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetCustomerStock(cusId As Integer) As List(Of order) Implements IOrderRep.GetCustomerStock
        Try
            Dim endDate = Today.Date.AddDays(1)
            Return _dbSet.AsNoTracking.Where(Function(x) x.o_cus_Id = cusId AndAlso x.o_date < endDate).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class