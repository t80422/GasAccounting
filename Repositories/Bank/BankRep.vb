Imports System.Data.Entity

Public Class BankRep
    Inherits Repository(Of bank)
    Implements IBankRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function GetBankDropdownAsync(Optional companyId As Integer? = Nothing) As Task(Of List(Of SelectListItem)) Implements IBankRep.GetBankDropdownAsync
        Try
            Dim query = _context.banks.AsNoTracking.AsQueryable()

            If companyId.HasValue Then query = query.Where(Function(x) x.bank_comp_Id = companyId)

            Return Await query.
                Select(Function(x) New SelectListItem With {
                    .Display = x.bank_name & "-" & x.bank_Account,
                    .Value = x.bank_id
                }).ToListAsync()
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
