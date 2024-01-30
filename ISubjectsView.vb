Public Interface ISubjectsView
    Sub DisplaySubjects(subjects As List(Of SubjectsVM))
    Function GetUserInput() As subject
    Sub ClearInputs()
    Sub SetSebject(subjects As subject)
End Interface
