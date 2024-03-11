Public Class SubjectsService
    Implements ISubjectsService

    Public Function GetSubjectsCmbItems(Optional type As String = Nothing) As List(Of ComboBoxItems) Implements ISubjectsService.GetSubjectsCmbItems
        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.subjects.AsQueryable

                If type IsNot Nothing Then query = query.Where(Function(x) x.s_Type = type)

                Return query.Select(Function(y) New ComboBoxItems With {
                    .Display = y.s_name,
                    .Value = y.s_id
                }).ToList
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
End Class
