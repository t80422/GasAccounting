Imports System.Data.Entity

Public Class PricePlanRep
    Inherits Repository(Of priceplan)
    Implements IPricePlanRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function GetDropdownAsync() As Task(Of List(Of SelectListItem)) Implements IPricePlanRep.GetDropdownAsync
        Try
            Return Await _dbSet.Select(Function(x) New SelectListItem With {
                .Display = x.pp_Name,
                .Value = x.pp_Id
            }).ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
