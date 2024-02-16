Public Class BankService
    Implements IBankService

    Public Function GetBankCmbItems() As List(Of ComboBoxItems) Implements IBankService.GetBankCmbItems
        Try
            Using db As New gas_accounting_systemEntities
                Return db.banks.Select(Function(x) New ComboBoxItems With {
                    .Display = x.bank_name,
                    .Value = x.bank_id
                }).ToList
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
End Class
