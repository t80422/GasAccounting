Public Interface ISubjectRep
    Inherits IRepository(Of subject)

    Function GetCmb() As List(Of ComboBoxItems)
End Interface
