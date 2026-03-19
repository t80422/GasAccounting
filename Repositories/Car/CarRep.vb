Public Class CarRep
    Inherits Repository(Of car)
    Implements ICarRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetDropdownByCusId(cusId As Integer) As List(Of SelectListItem) Implements ICarRep.GetDropdownByCusId
        Try
            Dim cars = _dbSet.Where(Function(x) x.c_cus_id = cusId).ToList

            Return cars.Select(Function(x) New SelectListItem With {
                .Display = $"{x.c_no} {x.c_driver}",
                .Value = x.c_id
            }).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function Search(criteria As CarSC) As List(Of car) Implements ICarRep.Search
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If Not String.IsNullOrEmpty(criteria.CusCode) Then query = query.Where(Function(x) x.customer.cus_code = criteria.CusCode)

            Return query.ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetDelivery() As car Implements ICarRep.GetDelivery
        Try
            Return _dbSet.AsNoTracking.FirstOrDefault(Function(x) x.c_no = "廠運")
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
