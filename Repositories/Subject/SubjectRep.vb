Public Class SubjectRep
    Inherits Repository(Of subject)
    Implements ISubjectRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetCmb() As List(Of ComboBoxItems) Implements ISubjectRep.GetCmb
        Try
            Using db As New gas_accounting_systemEntities
                Return db.subjects.Select(Function(x) New ComboBoxItems With {
                    .Display = x.s_name,
                    .Value = x.s_id
                }).ToList
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
