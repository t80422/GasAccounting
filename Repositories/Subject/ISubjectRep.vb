Public Interface ISubjectRep
    Inherits IRepository(Of subject)

    Function GetCmbAsync() As Task(Of IEnumerable(Of ComboBoxItems))
End Interface
