Public Interface ISubjectsView
    'todo 可以改成繼承ICommonView
    Sub DisplaySubjects(subjects As List(Of SubjectsVM))
    Function GetUserInput() As subject
    Sub ClearInputs()
    Sub SetSebject(subjects As subject)
End Interface
