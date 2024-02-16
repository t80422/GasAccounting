Public Interface ISubjectsService
    Function GetSubjectsGroupCmbItems() As List(Of ComboBoxItems)
    Function GetSubjectsCmbItems(sgId As Integer) As List(Of ComboBoxItems)
End Interface
