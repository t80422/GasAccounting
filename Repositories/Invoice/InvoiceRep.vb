Imports System.Data.Entity

Public Class InvoiceRep
    Inherits Repository(Of invoice)
    Implements IInvoiceRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function SearchAsync(criteria As InvoiceSearchCriteria) As Task(Of List(Of invoice)) Implements IInvoiceRep.SearchAsync
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If criteria.IsSearchMonth Then query = query.Where(Function(x) x.i_Date.Year = criteria.Month.Year AndAlso x.i_Date = criteria.Month)
            If criteria.CusId <> 0 Then query = query.Where(Function(x) x.i_cus_Id = criteria.CusId)
            If Not String.IsNullOrEmpty(criteria.Number) Then query = query.Where(Function(x) x.i_Number = criteria.Number)

            Return Await query.ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetByMonth(month As Date) As List(Of invoice) Implements IInvoiceRep.GetByMonth
        Try
            Return _dbSet.Where(Function(x) x.i_Date.Year = month.Year AndAlso x.i_Date.Month = month.Month).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetLastInvoiceNumberByType(invoiceType As String) As String Implements IInvoiceRep.GetLastInvoiceNumberByType
        Try
            Dim datas = _dbSet.Where(Function(x) x.i_InvoiceType = invoiceType).OrderByDescending(Function(x) x.i_Number).FirstOrDefault
            Dim result As String = Nothing

            If datas IsNot Nothing Then result = datas.i_Number

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
