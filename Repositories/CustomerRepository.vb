Public Class CustomerRepository
    Implements ICustomerRepository

    Public Function SearchCustomers(condition As customer) As List(Of customer) Implements ICustomerRepository.SearchCustomers
        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.customers.AsNoTracking.AsEnumerable

                If Not String.IsNullOrEmpty(condition.cus_code) Then query = query.Where(Function(x) x.cus_code.Contains(condition.cus_code))
                If Not String.IsNullOrEmpty(condition.cus_name) Then query = query.Where(Function(x) x.cus_name.Contains(condition.cus_name))
                If Not String.IsNullOrEmpty(condition.cus_contact_person) Then query = query.Where(Function(x) x.cus_contact_person.Contains(condition.cus_contact_person))
                If Not String.IsNullOrEmpty(condition.cus_phone1) Then query = query.Where(Function(x) x.cus_phone1.Contains(condition.cus_phone1))
                If Not String.IsNullOrEmpty(condition.cus_phone1) Then query = query.Where(Function(x) x.cus_phone2.Contains(condition.cus_phone2))

                Return query.ToList
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetCustomerById(id As Integer) As customer Implements ICustomerRepository.GetCustomerById
        Try
            Using db As New gas_accounting_systemEntities
                Return db.customers.Find(id)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetCusByCusCode(cusCode As String) As customer Implements ICustomerRepository.GetCusByCusCode
        Try
            Using db As New gas_accounting_systemEntities
                Return db.customers.FirstOrDefault(Function(x) x.cus_code = cusCode)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function CustomersGasDetailByDay(d As Date) As List(Of CustomersGasDetailByDay) Implements ICustomerRepository.CustomersGasDetailByDay
        Try
            Using db As New gas_accounting_systemEntities
                Return db.customers.Select(Function(x) New CustomersGasDetailByDay With {
                     .客戶名稱 = x.cus_name,
                     .存氣 = 0,
                     .本日提量 = db.orders.Where(Function(o) o.car.c_cus_id = x.cus_id And o.o_date.Value.Date = d.Date).
                                           Sum(Function(o) o.o_gas_total + o.o_gas_c_total),
                     .當月累計提量 = db.orders.Where(Function(o) o.car.c_cus_id = x.cus_id And o.o_date.Value.Year = d.Year And o.o_date.Value.Month = d.Month).
                                               Sum(Function(o) o.o_gas_total + o.o_gas_c_total),
                     .本日氣款 = db.orders.Where(Function(o) o.car.c_cus_id = x.cus_id And o.o_date.Value.Date = d.Date).
                                           Sum(Function(o) o.o_total_amount),
                     .本日收款 = db.collections.Where(Function(c) c.col_cus_Id = x.cus_id And c.col_Date.Date = d.Date).
                                                Sum(Function(c) c.col_Amount),
                     .結欠 = db.orders.Where(Function(o) o.car.c_cus_id = x.cus_id And o.o_date.Value.Year = d.Year And o.o_date.Value.Month = d.Month).
                                       Sum(Function(o) o.o_total_amount) -
                             db.collections.Where(Function(c) c.col_cus_Id = x.cus_id And c.col_Date.Year = d.Year And c.col_Date.Month = d.Month).
                                       Sum(Function(c) c.col_Amount)
                 })
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw
            Return New List(Of CustomersGasDetailByDay)
        End Try
    End Function
End Class