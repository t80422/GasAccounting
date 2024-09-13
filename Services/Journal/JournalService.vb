Imports System.Data.Entity.Validation

Public Class JournalService
    Implements IJournalService

    Public Sub Edit(entry As journal) Implements IJournalService.Edit
        Try
            Using db As New gas_accounting_systemEntities
                Dim journal = db.journals.FirstOrDefault(Function(x) x.j_SubpoenaNo = entry.j_SubpoenaNo)

                If journal IsNot Nothing Then
                    journal.j_Amount = entry.j_Amount
                    journal.j_Memo = entry.j_Memo

                    db.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Delete(subpoenaNo As Integer) Implements IJournalService.Delete
        Try
            Using db As New gas_accounting_systemEntities
                Dim data = db.journals.Where(Function(x) x.j_SubpoenaNo = subpoenaNo)
                db.journals.RemoveRange(data)
                db.SaveChanges()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Add(entry As journal) Implements IJournalService.Add
        Try
            Using db As New gas_accounting_systemEntities
                Dim data = db.journals.FirstOrDefault(Function(x) x.j_SubpoenaNo = entry.j_SubpoenaNo)

                If data Is Nothing Then
                    data = New journal
                    db.journals.Add(data)
                End If

                data.j_Amount = entry.j_Amount
                data.j_Memo = entry.j_Memo
                data.j_SubpoenaNo = entry.j_SubpoenaNo
                data.j_s_Id = entry.j_s_Id

                Try
                    db.SaveChanges()
                Catch ex As DbEntityValidationException
                    MsgBox(ex.Message)
                End Try

            End Using
        Catch ex As Exception
            If ex.InnerException IsNot Nothing Then
                MsgBox(ex.InnerException.Message)
            Else
                MsgBox(ex.Message)
            End If
        End Try
    End Sub

    Public Function GetSubpoenaNo() As Integer Implements IJournalService.GetSubpoenaNo
        Try
            Using db As New gas_accounting_systemEntities

                If db.journals.Any() Then
                    Return db.journals.Max(Function(x) x.j_SubpoenaNo) + 1
                Else
                    Return 1
                End If
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
            Throw
        End Try
    End Function
End Class