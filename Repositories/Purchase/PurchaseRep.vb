Imports System.Data.Entity

Public Class PurchaseRep
    Inherits Repository(Of purchase)
    Implements IPurchaseRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetForCheckout(data As GasCheckoutUserInput) As List(Of PurchaseVM) Implements IPurchaseRep.GetForCheckout
        Try
            Dim query = _dbSet.Where(Function(x) Not x.pur_Checkout)

            If data.IsDateSearch Then query = query.Where(Function(x) x.pur_date >= data.StartDate AndAlso x.pur_date <= data.EndDate)

            If data.VendorId <> 0 Then query = query.Where(Function(x) x.pur_manu_id = data.VendorId)

            If Not String.IsNullOrEmpty(data.PaymentType) Then query = query.Where(Function(x) x.pur_PayType = data.PaymentType)

            Dim result = query.ToList

            Return result.Select(Function(x) New PurchaseVM(x)).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function UpdateCheckoutStatusAsync(datas As List(Of Integer)) As Task Implements IPurchaseRep.UpdateCheckoutStatusAsync
        Try
            Dim purchases = Await _dbSet.Where(Function(x) datas.Contains(x.pur_id)).ToListAsync

            For Each purchase In purchases
                purchase.pur_Checkout = True
            Next

            Await _context.SaveChangesAsync
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class