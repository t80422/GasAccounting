Public Interface IBankRep
    Inherits IRepository(Of bank)

    Function GetBankDropdownAsync() As Task(Of List(Of SelectListItem))
End Interface
