Public Interface IClosingEntryView
    Inherits IBaseView(Of closing_entry, ClosingEntryVM)

    Sub SetSubjectDropdown(data As List(Of SelectListItem))
End Interface
