Public Interface IBankRep
    Inherits IRepository(Of bank)

    Function GetBankCombobox() As List(Of ComboBoxItems)
End Interface
