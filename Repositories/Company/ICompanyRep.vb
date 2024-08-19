Public Interface ICompanyRep
    Inherits IRepository(Of company)

    Function GetCmbDataAsync() As Task(Of IEnumerable(Of ComboBoxItems))
End Interface
