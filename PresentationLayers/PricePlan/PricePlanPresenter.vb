Public Class PricePlanPresenter
    Inherits BasePresenter(Of priceplan, PricePlanVM, IPricePlanView)
    Implements IPresenter(Of priceplan, PricePlanVM)

    Sub New(view As IPricePlanView)
        MyBase.New(view)
        _presenter = Me
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of priceplan), conditions As Object) As IQueryable(Of priceplan) Implements IPresenter(Of priceplan, PricePlanVM).SetSearchConditions
        Return Nothing
    End Function

    Public Function SetListViewModel(query As IQueryable(Of priceplan)) As List(Of PricePlanVM) Implements IPresenter(Of priceplan, PricePlanVM).SetListViewModel
        Return query.Select(Function(x) New PricePlanVM With {
            .編號 = x.pp_Id,
            .名稱 = x.pp_Name,
            .自運普氣 = x.pp_Gas,
            .自運丙氣 = x.pp_Gas_c,
            .廠運普氣 = x.pp_GasDelivery,
            .廠運丙氣 = x.pp_GasDelivery_c
        }).ToList
    End Function
End Class
