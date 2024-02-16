Public Class SubjectsService
    Implements ISubjectsService

    Public Function GetSubjectsGroupCmbItems() As List(Of ComboBoxItems) Implements ISubjectsService.GetSubjectsGroupCmbItems
        Try
            Using db As New gas_accounting_systemEntities
                Return db.subjects_group.Select(Function(x) New ComboBoxItems With {
                    .Display = x.sg_name,
                    .Value = x.sg_id
                }).ToList
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSubjectsCmbItems(sgId As Integer) As List(Of ComboBoxItems) Implements ISubjectsService.GetSubjectsCmbItems
        Try
            Using db As New gas_accounting_systemEntities
                Return db.subjects.Where(Function(x) x.s_sg_id = sgId).Select(Function(y) New ComboBoxItems With {
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
