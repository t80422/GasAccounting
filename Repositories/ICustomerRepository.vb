Public Interface ICustomerRepository
    Function SearchCustomers(condition As customer) As List(Of customer)
    Function GetCustomerById(id As Integer) As customer
    Function GetCusByCusCode(cusCode As String) As customer
    Function CustomersGasDetailByDay(d As Date) As List(Of CustomersGasDetailByDay)
End Interface
