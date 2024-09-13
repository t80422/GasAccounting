Public Interface ICarRep
    Inherits IRepository(Of car)

    Function GetDropdownByCusId(cusId As Integer) As List(Of SelectListItem)
    Function Search(criteria As CarSC) As List(Of car)
End Interface