Public Class RolesPresenter
    Inherits BasePresenter(Of role, RolesVM, IRolesView)
    Implements IPresenter(Of role, RolesVM)

    Public Sub New(view As IRolesView)
        MyBase.New(view)
        _presenter = Me
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of role), conditions As Object) As IQueryable(Of role) Implements IPresenter(Of role, RolesVM).SetSearchConditions
        Return Nothing
    End Function

    Public Function SetListViewModel(query As IQueryable(Of role)) As List(Of RolesVM) Implements IPresenter(Of role, RolesVM).SetListViewModel
        Return query.Select(Function(x) New RolesVM With {
            .名稱 = x.r_name,
            .編號 = x.r_id
        }).ToList
    End Function
End Class
