Imports System.ComponentModel
Imports System.Data.Entity.Core.Mapping

Public Class PaymentChequeRep
    Inherits Repository(Of payment_cheque)
    Implements IPaymentChequeRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Sub AddBatch(list As List(Of payment_cheque)) Implements IPaymentChequeRep.AddBatch
        Try
            _dbSet.AddRange(list)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function GetByPaymentId(pId As Integer) As List(Of payment_cheque) Implements IPaymentChequeRep.GetByPaymentId
        Try
            Return _dbSet.Where(Function(x) x.pc_p_id = pId).ToList()
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
