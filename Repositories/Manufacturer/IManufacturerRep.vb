Public Interface IManufacturerRep
    Inherits IRepository(Of manufacturer)

    Function GetGasVendorsForCmb() As List(Of ComboBoxItems)
End Interface
