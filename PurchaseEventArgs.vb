Public Class PurchaseEventArgs
    Inherits EventArgs

    Public Property PurchaseId As Integer

    Public Sub New(purchaseId As Integer)
        Me.PurchaseId = purchaseId
    End Sub
End Class
