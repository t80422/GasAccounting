Public Interface ICompanyRep
    Inherits IRepository(Of company)

    Function GetCompanyDropdownAsync() As Task(Of IEnumerable(Of SelectListItem))
End Interface
