Public Class PurchasePresenter
    Inherits BasePresenter(Of purchase, PurchaseVM, IPurchaseView)
    Implements IPresenter(Of purchase, PurchaseVM)

    Private _compService As ICompanyService
    Private _manuService As IManufacturerService

    Public Sub New(view As IPurchaseView, compService As ICompanyService, manuService As IManufacturerService)
        MyBase.New(view)
        _presenter = Me
        _compService = compService
        _manuService = manuService
    End Sub

    ''' <summary>
    ''' 設定公司下拉選單
    ''' </summary>
    Public Sub SetCompanyCmb()
        Dim data = _compService.GetCompanyComboBoxData
        _view.SetCompanyComboBox(data)
    End Sub

    ''' <summary>
    ''' 設定大氣廠商下拉選單
    ''' </summary>
    Public Sub SetGasVendorCmb()
        _view.SetGasVendorComboBox(_manuService.GetGasVendorCmbItems)
    End Sub

    ''' <summary>
    ''' 搜尋
    ''' </summary>
    Public Sub Query()
        LoadList(_view.GetSearchCondition)
    End Sub

    Protected Function SetSearchConditions(query As IQueryable(Of purchase), conditions As Object) As IQueryable(Of purchase) Implements IPresenter(Of purchase, PurchaseVM).SetSearchConditions
        Dim c As purchase = conditions

        If c.pur_comp_id <> 0 Then query = query.Where(Function(x) x.pur_comp_id = c.pur_comp_id)
        If c.pur_manu_id <> 0 Then query = query.Where(Function(x) x.pur_manu_id = c.pur_manu_id)
        If c.pur_product <> Nothing Then query = query.Where(Function(x) x.pur_product = c.pur_product)

        Return query
    End Function

    Private Function IPresenter_SetListViewModel(query As IQueryable(Of purchase)) As List(Of PurchaseVM) Implements IPresenter(Of purchase, PurchaseVM).SetListViewModel
        Return query.Select(Function(x) New PurchaseVM With {
                    .編號 = x.pur_id,
                    .日期 = x.pur_date,
                    .產品 = x.pur_product,
                    .單價 = x.pur_unit_price,
                    .重量 = x.pur_quantity,
                    .廠商 = x.manufacturer.manu_name,
                    .運費單價 = x.pur_deli_unit_price,
                    .運費 = x.pur_delivery_fee,
                    .金額 = x.pur_price,
                    .公司 = x.company.comp_name
                }).ToList
    End Function

End Class
