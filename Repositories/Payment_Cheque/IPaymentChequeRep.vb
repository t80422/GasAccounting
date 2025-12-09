Imports System.ComponentModel

Public Interface IPaymentChequeRep
    Inherits IRepository(Of payment_cheque)
    Sub AddBatch(list As List(Of payment_cheque))
    Function GetByPaymentId(pId As Integer) As List(Of payment_cheque)
End Interface
