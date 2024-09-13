Public Interface IManufacturerRep
    Inherits IRepository(Of manufacturer)

    Function GetGasVendorCmbDataAsync() As Task(Of IEnumerable(Of SelectListItem))
    Function GetVendorCmbWithoutGasAsync() As Task(Of IEnumerable(Of SelectListItem))
    Function GetVendorDropdownAsync() As Task(Of List(Of SelectListItem))
End Interface
