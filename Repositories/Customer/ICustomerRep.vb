Public Interface ICustomerRep
    Inherits IRepository(Of customer)

    Function SearchAsync(Optional criteria As customer = Nothing) As Task(Of List(Of customer))
    Function GetCustomerById(id As Integer) As customer
    Function GetByCusCode(cusCode As String) As customer
End Interface
