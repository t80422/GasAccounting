Public Interface IBankRep
    Inherits IRepository(Of bank)

    Function GetBankDropdownAsync(Optional companyId As Integer? = Nothing) As Task(Of List(Of SelectListItem))
End Interface
