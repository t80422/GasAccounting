Public Class CollectionRep
    Implements ICollectionRep

    Private _journalService As IJournalService = New JournalService

    Public Sub Add(collection As collection, Optional journal As journal = Nothing, Optional cheque As cheque = Nothing) Implements ICollectionRep.Add
        Using db As New gas_accounting_systemEntities
            Using transaction = db.Database.BeginTransaction
                Try
                    '新增傳票
                    If journal IsNot Nothing Then
                        Dim SubpoenaNo = _journalService.GetSubpoenaNo()
                        journal.j_SubpoenaNo = SubpoenaNo
                        db.journals.Add(journal)
                        collection.col_SubpoenaNo = SubpoenaNo
                    End If

                    db.collections.Add(collection)

                    '新增支票
                    If cheque IsNot Nothing Then
                        db.cheques.Add(cheque)
                    End If

                    db.SaveChanges()
                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Public Sub Edit(col As collection, jour As journal, che As cheque) Implements ICollectionRep.Edit
        Using db As New gas_accounting_systemEntities
            Using transaction = db.Database.BeginTransaction
                Try
                    Dim exCol = db.collections.Find(col.col_Id)
                    col.col_SubpoenaNo = exCol.col_SubpoenaNo
                    col.col_Cheque = exCol.col_Cheque
                    db.Entry(exCol).CurrentValues.SetValues(col)

                    '更新傳票
                    If jour IsNot Nothing Then
                        Dim exJour = db.journals.First(Function(x) x.j_SubpoenaNo = col.col_SubpoenaNo)
                        jour.j_Id = exJour.j_Id
                        db.Entry(exJour).CurrentValues.SetValues(jour)
                    End If

                    '更新支票
                    If che IsNot Nothing Then
                        Dim exChe = db.cheques.First(Function(x) x.che_Number = col.col_Cheque)
                        che.che_Id = exChe.che_Id
                        db.Entry(exChe).CurrentValues.SetValues(che)
                    End If

                    db.SaveChanges()
                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Public Sub Delete(id As Integer) Implements ICollectionRep.Delete
        Using db As New gas_accounting_systemEntities
            Using transaction = db.Database.BeginTransaction
                Try
                    Dim col = db.collections.Find(id)

                    '刪除傳票
                    Dim jour = db.journals.FirstOrDefault(Function(x) x.j_SubpoenaNo = col.col_SubpoenaNo)
                    If jour IsNot Nothing Then db.journals.Remove(jour)

                    '刪除支票
                    Dim che = db.cheques.FirstOrDefault(Function(x) x.che_Number = col.col_Cheque)
                    If che IsNot Nothing Then db.cheques.Remove(che)

                    db.collections.Remove(col)
                    db.SaveChanges()
                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Public Sub UpdateCheque(colId As Integer) Implements ICollectionRep.UpdateCheque
        Using db As New gas_accounting_systemEntities
            Using transaction = db.Database.BeginTransaction
                Try
                    Dim col = db.collections.Find(colId)
                    Dim subpoenaNo = _journalService.GetSubpoenaNo
                    Dim jour = New journal With {
                        .j_Amount = col.col_Amount,
                        .j_Memo = col.col_Memo,
                        .j_s_Id = 4,
                        .j_SubpoenaNo = subpoenaNo
                    }
                    db.journals.Add(jour)
                    col.col_SubpoenaNo = subpoenaNo

                    Dim che = db.cheques.FirstOrDefault(Function(x) x.che_Number = col.col_Cheque)
                    che.chu_State = "已兌現"
                    che.che_CashingDate = Now.Date

                    db.SaveChanges()
                    transaction.Commit()
                Catch ex As Exception
                    transaction.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub
End Class
