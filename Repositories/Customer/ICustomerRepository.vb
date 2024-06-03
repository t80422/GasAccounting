Public Interface ICustomerRepository
    Inherits IRepository(Of customer)

    Function SearchCustomers(condition As customer) As List(Of customer)
    Function GetCustomerById(id As Integer) As customer
    Function GetCusByCusCode(cusCode As String) As customer
End Interface
