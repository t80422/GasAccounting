Public Class SelectChequeVM
    Public Property 編號 As Integer
    Public Property 支票編號 As String

    Public Sub New(data As payment_cheque)
        編號 = data.pc_id
        支票編號 = data.chque_pay.cp_Number
    End Sub

    Public Sub New()
    End Sub
End Class
