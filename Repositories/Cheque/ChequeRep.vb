Imports System.Data.Entity

Public Class ChequeRep
    Inherits Repository(Of cheque)
    Implements IChequeRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Sub UpdateRedeemedStatus(chequeIds As List(Of Integer), redeemedDate As Date) Implements IChequeRep.UpdateRedeemedStatus
        Try
            Dim cheques = _dbSet.Where(Function(x) chequeIds.Contains(x.che_Id) AndAlso x.chu_State = "已代收").ToList()
            For Each cheque In cheques
                cheque.chu_State = "已兌現"
                cheque.che_CashingDate = redeemedDate
            Next

            _context.SaveChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Async Function UpdateCollectionStatusAsync(chequeIds As List(Of Integer), collectionDate As Date) As Task Implements IChequeRep.UpdateCollectionStatusAsync
        Try
            Dim cheques = Await _dbSet.Where(Function(x) chequeIds.Contains(x.che_Id) AndAlso x.chu_State = Nothing).ToListAsync

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

    Public Function GetList(Optional criteria As ChequeSC = Nothing) As List(Of cheque) Implements IChequeRep.GetList
        Try
            Dim query = _dbSet.AsNoTracking

            If criteria IsNot Nothing Then
                If criteria.IsDate Then query = query.Where(Function(x) x.che_ReceivedDate >= criteria.StartDate AndAlso x.che_ReceivedDate < criteria.EndDate)
                If criteria.IsStatus Then query = query.Where(Function(x) x.chu_State = criteria.Status)
                If criteria.BankId.HasValue Then query = query.Where(Function(x) x.collection.col_bank_Id = criteria.BankId)
            Else
                query = query.Where(Function(x) x.chu_State <> "已兌現")
            End If

            Return query.OrderByDescending(Function(x) x.che_Id).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
