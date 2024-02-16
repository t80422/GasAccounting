Public Class CompanyService
    Implements ICompanyService

    Public Function GetCompanyComboBoxData() As List(Of ComboBoxItems) Implements ICompanyService.GetCompanyComboBoxData
        Try
            Using db As New gas_accounting_systemEntities
                Return db.companies.Select(Function(x) New ComboBoxItems With {
                    .Value = x.comp_id,
                    .Display = x.comp_short
                }).ToList
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
End Class
