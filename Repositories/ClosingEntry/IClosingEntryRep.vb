Public Interface IClosingEntryRep
    Inherits IRepository(Of closing_entry)

    Function Search(Optional criteria As ClosingEntrySC = Nothing) As List(Of ClosingEntryVM)
End Interface
