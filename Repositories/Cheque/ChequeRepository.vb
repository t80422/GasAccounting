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

    Public Function Query(startDate As Date, endDate As Date, Optional filter As cheque = Nothing) As List(Of ChequeVM) Implements IChequeRepository.Query
        Try
            Using db As New gas_accounting_systemEntities
                Dim start = startDate.Date
                Dim endd = endDate.Date.AddDays(1).AddSeconds(-1)
                Dim result As List(Of cheque)
                ' 先執行查詢取得數據
                Dim cheques = db.cheques.Where(Function(x) x.che_ReceivedDate >= start AndAlso x.che_ReceivedDate <= endd)

                If filter IsNot Nothing Then
                    If Not String.IsNullOrEmpty(filter.chu_State) Then
                        cheques = cheques.Where(Function(x) x.chu_State = filter.chu_State)
                    End If
                End If

                result = cheques.ToList

                ' 然後在內存中轉換數據到 ViewModel
                Return result.Select(Function(x) New ChequeVM(x)).ToList()
            End Using
        Catch ex As Exception
            MsgBox(ex.StackTrace)
            Console.WriteLine(ex.Message)
            Return New List(Of ChequeVM)
        End Try
    End Function
End Class
