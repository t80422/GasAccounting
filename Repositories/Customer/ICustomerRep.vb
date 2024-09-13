Public Interface ICustomerRep
    Inherits IRepository(Of customer)

    Function SearchAsync(criteria As customer) As Task(Of List(Of customer))
    Function GetCustomerById(id As Integer) As customer
    Function GetByCusCode(cusCode As String) As customer
End Interface
