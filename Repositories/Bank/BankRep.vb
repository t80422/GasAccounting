Public Class BankRep
    Inherits Repository_old(Of bank)
    Implements IBankRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetBankCombobox() As List(Of ComboBoxItems) Implements IBankRep.GetBankCombobox
        Try
            Dim banks = _dbSet.ToList
            Return banks.Select(Function(x) New ComboBoxItems With {.Display = $"{x.bank_name} - {x.bank_Account}", .Value = x.bank_id}).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
