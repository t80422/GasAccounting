Public Class BankService
    Implements IBankService

    Public Function GetBankCmbItems() As List(Of ComboBoxItems) Implements IBankService.GetBankCmbItems
        Try
            Using db As New gas_accounting_systemEntities
                Dim banks = db.banks.ToList
                Return banks.Select(Function(x) New ComboBoxItems With {
                    .Display = $"{x.bank_name} - {x.bank_Account}",
                    .Value = x.bank_id
                })
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
End Class
