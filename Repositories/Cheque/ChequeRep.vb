Imports System.Data.Entity

Public Class ChequeRep
    Inherits Repository(Of cheque)
    Implements IChequeRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetState(cheNum As String) As String Implements IChequeRep.GetState
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

    Public Function Query(startDate As Date, endDate As Date, Optional filter As cheque = Nothing) As List(Of ChequeVM) Implements IChequeRep.Query
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

    Public Async Function UpdateCollectionStatusAsync(chequeIds As List(Of Integer), collectionDate As Date) As Task Implements IChequeRep.UpdateCollectionStatusAsync
        Try
            Dim cheques = Await _dbSet.Where(Function(x) chequeIds.Contains(x.che_Id) AndAlso x.chu_State = "未兌現").ToListAsync

            For Each cheque In cheques
                cheque.chu_State = "已代收"
                cheque.che_CollectionDate = collectionDate
            Next

            Await _context.SaveChangesAsync
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw New Exception("更新批量代收發生錯誤")
        End Try
    End Function

    Public Async Function GetByNumberAsync(chequeNum As String) As Task(Of cheque) Implements IChequeRep.GetByNumberAsync
        Try
            Return Await _dbSet.FirstOrDefaultAsync(Function(x) x.che_Number = chequeNum)
        Catch ex As Exception
            Throw New Exception("ChequeRep.GetByNumberAsync發生錯誤", ex)
        End Try
    End Function
End Class
