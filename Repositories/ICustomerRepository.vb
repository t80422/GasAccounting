Public Interface ICustomerRepository
    Function SearchCustomers(condition As customer) As List(Of customer)
    Function GetCustomerById(id As Integer) As customer
End Interface
