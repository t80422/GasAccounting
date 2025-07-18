Public Class InspectionRep
    Inherits Repository(Of inspection)
    Implements IInspectionRep

    Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function Search(criteria As InspectionSC) As List(Of InspectionVM) Implements IInspectionRep.Search
        Dim query = _dbSet.AsNoTracking.AsQueryable.Join(_context.customers,
            Function(i) i.in_cus_Id,
            Function(c) c.cus_id,
            Function(i, c) New With {
                i,
                c
            })

        If criteria IsNot Nothing Then
            If criteria.IsDate AndAlso criteria.Month.HasValue Then query = query.Where(Function(x) x.i.in_Month.Value = criteria.Month.Value)
            If criteria.CusId.HasValue AndAlso criteria.CusId.Value <> 0 Then query = query.Where(Function(x) x.i.in_cus_Id = criteria.CusId.Value)
        End If


        Dim result = query.OrderByDescending(Function(x) x.i.in_Month).ToList()

        Return result.Select(
            Function(x) New InspectionVM With {
                .編號 = x.i.in_Id,
                .月份 = x.i.in_Month.Value.ToString("yyyy/MM"),
                .客戶代碼 = x.c.cus_code,
                .客戶名稱 = x.c.cus_name
            }).ToList()
    End Function
End Class
