Public Interface IPricePlanRep
    Inherits IRepository(Of priceplan)

    Function GetDropdownAsync() As Task(Of List(Of SelectListItem))
End Interface
