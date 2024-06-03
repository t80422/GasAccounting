Public Class ManufacturerPresenter
    Inherits BasePresenter(Of manufacturer, ManufacturerVM, IManufacturerView)
    Implements IPresenter(Of manufacturer, ManufacturerVM)

    Public Sub New(view As IManufacturerView)
        MyBase.New(view)
        _presenter = Me
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of manufacturer), conditions As Object) As IQueryable(Of manufacturer) Implements IPresenter(Of manufacturer, ManufacturerVM).SetSearchConditions
        Dim c As manufacturer = conditions

        query = query.Where(Function(x) x.manu_code.Contains(c.manu_code))
        query = query.Where(Function(x) x.manu_name.Contains(c.manu_name))
        query = query.Where(Function(x) x.manu_phone1.Contains(c.manu_phone1))

        Return query
    End Function

    Public Function SetListViewModel(query As IQueryable(Of manufacturer)) As List(Of ManufacturerVM) Implements IPresenter(Of manufacturer, ManufacturerVM).SetListViewModel
        Return query.Select(Function(x) New ManufacturerVM With {
                    .編號 = x.manu_id,
                    .代號 = x.manu_code,
                    .名稱 = x.manu_name,
                    .負責人 = x.manu_principal,
                    .聯絡人 = x.manu_contact_person,
                    .電話1 = x.manu_phone1,
                    .電話2 = x.manu_phone2,
                    .地址 = x.manu_address,
                    .統編 = x.manu_tax_id,
                    .傳真 = x.manu_fax,
                    .是否為大氣公司 = x.manu_GasVendor,
                    .銀行 = x.manu_bank,
                    .分行 = x.manu_branches,
                    .銀行代號 = x.manu_bank_code,
                    .戶名 = x.manu_account_name,
                    .帳號 = x.manu_account,
                    .備註 = x.manu_memo
                }).ToList
    End Function
End Class
