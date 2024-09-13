Public Interface ISubjectRep
    Inherits IRepository(Of subject)

    Function GetSubjectDropdownAsync() As Task(Of IEnumerable(Of SelectListItem))
End Interface
