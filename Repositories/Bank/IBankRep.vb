Public Interface IBankRep
    Inherits IRepository_old(Of bank)

    Function GetBankCombobox() As List(Of ComboBoxItems)
End Interface
