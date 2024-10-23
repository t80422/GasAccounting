Imports System.Data.Entity

Public Class CollectionRep
    Inherits Repository(Of collection)
    Implements ICollectionRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Sub Add(collection As collection, Optional cheque As cheque = Nothing) Implements ICollectionRep.Add
        Using db As New gas_accounting_systemEntities
            Using transaction = db.Database.BeginTransaction
                Try
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

    Public Sub Edit(col As collection, che As cheque) Implements ICollectionRep.Edit
        Using db As New gas_accounting_systemEntities
            Using transaction = db.Database.BeginTransaction
                Try
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

    Public Sub UpdateCheque(colId As Integer) Implements ICollectionRep.UpdateCheque
        Using db As New gas_accounting_systemEntities
            Using transaction = db.Database.BeginTransaction
                Try
                    Dim col = db.collections.Find(colId)
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

    Public Async Function GetByBankAndMonthAsync(bankId As Integer, month As Date) As Task(Of IEnumerable(Of collection)) Implements ICollectionRep.GetByBankAndMonthAsync
        Try
            Return Await _dbSet.AsNoTracking.Where(Function(x) x.col_AccountMonth = month AndAlso x.col_bank_Id = bankId).ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
