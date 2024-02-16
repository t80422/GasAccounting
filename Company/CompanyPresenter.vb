Public Class CompanyPresenter
    Inherits BasePresenter(Of company, CompanyVM, ICompanyView)
    Implements IPresenter(Of company, CompanyVM)

    Public Sub New(view As ICompanyView)
        MyBase.New(view)
        _presenter = Me
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of company), conditions As Object) As IQueryable(Of company) Implements IPresenter(Of company, CompanyVM).SetSearchConditions
        Return Nothing
    End Function

    Private Function SetListViewModel(query As IQueryable(Of company)) As List(Of CompanyVM) Implements IPresenter(Of company, CompanyVM).SetListViewModel
        Return query.Select(Function(x) New CompanyVM With {
            .編號 = x.comp_id,
            .名稱 = x.comp_name,
            .簡稱 = x.comp_short,
            .統編 = x.comp_tax_id,
            .備註 = x.comp_memo
        }).ToList
    End Function
End Class
