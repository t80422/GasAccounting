Public Interface IManufacturerRep
    Inherits IRepository(Of manufacturer)

    Function GetGasVendorCmbDataAsync() As Task(Of IEnumerable(Of ComboBoxItems))
    Function GetVendorCmbWithoutGasAsync() As Task(Of IEnumerable(Of ComboBoxItems))
End Interface
