Public Class ChequeRepository
    Implements IChequeRepository

    Public Function GetState(cheNum As String) As String Implements IChequeRepository.GetState
        Try
            Using db As New gas_accounting_systemEntities
                Dim che = db.cheques.FirstOrDefault(Function(x) x.che_Number = cheNum)
                If che IsNot Nothing Then Return che.chu_State
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return Nothing
    End Function
End Class
