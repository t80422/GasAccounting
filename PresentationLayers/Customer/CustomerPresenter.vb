Public Class CustomerPresenter
    Inherits BasePresenter(Of customer, CustomerVM, ICustomer)
    Implements IPresenter(Of customer, CustomerVM)

    Private _service As New Service

    Public Sub New(view As ICustomer)
        MyBase.New(view)
        _presenter = Me
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of customer), conditions As Object) As IQueryable(Of customer) Implements IPresenter(Of customer, CustomerVM).SetSearchConditions
        Dim c As customer = conditions

        query = query.Where(Function(x) x.cus_code.Contains(c.cus_code))
        query = query.Where(Function(x) x.cus_name.Contains(c.cus_name))
        query = query.Where(Function(x) x.cus_phone1.Contains(c.cus_phone1))

        Return query
    End Function

    Public Function SetListViewModel(query As IQueryable(Of customer)) As List(Of CustomerVM) Implements IPresenter(Of customer, CustomerVM).SetListViewModel
        Return query.Select(Function(x) New CustomerVM With {
              .編號 = x.cus_id,
              .代號 = x.cus_code,
              .名稱 = x.cus_name,
              .電話1 = x.cus_phone1,
              .電話2 = x.cus_phone2,
              .聯絡人 = x.cus_contact_person,
              .統編 = x.cus_tax_id,
              .負責人 = x.cus_principal,
              .地址 = x.cus_address,
              .傳真 = x.cus_fax,
              .保險 = x.cus_insurance,
              .瓦斯桶50Kg = x.cus_gas_50,
              .瓦斯桶20Kg = x.cus_gas_20,
              .瓦斯桶16Kg = x.cus_gas_16,
              .瓦斯桶10Kg = x.cus_gas_10,
              .瓦斯桶4Kg = x.cus_gas_4,
              .瓦斯桶15Kg = x.cus_gas_15,
              .瓦斯桶14Kg = x.cus_gas_14,
              .瓦斯桶5Kg = x.cus_gas_5,
              .瓦斯桶2Kg = x.cus_gas_2,
              .備註 = x.cus_memo,
              .價格方案 = x.priceplan.pp_Name
            }).ToList
    End Function

    Public Sub PricePlan_Cmb()
        _view.SetPricePlan_Cmb(_service.GetPricePlan_Cmb)
    End Sub

    Public Sub GetPricePlanDetails(id As Integer)
        Try
            Using db As New gas_accounting_systemEntities
                Dim pp = db.priceplans.Find(id)
                _view.SetPricePlanDetails(pp)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
