Public Class InvoiceSplitRep
    Inherits Repository(Of invoice_split)
    Implements IInvoiceSplitRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function Search(criteria As InvoiceSplitSearchCriteria) As List(Of invoice_split) Implements IInvoiceSplitRep.Search
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If criteria.IsDate Then
                Dim startDate = criteria.StartDate.Date
                Dim endDate = criteria.EndDate.Date.AddDays(1)
                query = query.Where(Function(x) x.is_Date >= startDate AndAlso x.is_Date < endDate)
            End If

            If Not String.IsNullOrEmpty(criteria.Name) Then query = query.Where(Function(x) x.is_Name.Contains(criteria.Name))
            If Not String.IsNullOrEmpty(criteria.Number) Then query = query.Where(Function(x) x.is_Number.Contains(criteria.Number))
            If Not String.IsNullOrEmpty(criteria.TaxId) Then query = query.Where(Function(x) x.is_VendorTaxId.Contains(criteria.TaxId))

            Return query.ToList
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class