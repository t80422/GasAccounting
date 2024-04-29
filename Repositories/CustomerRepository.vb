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
End Class