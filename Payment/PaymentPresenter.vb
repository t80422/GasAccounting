Public Class PaymentPresenter
    Inherits BasePresenter(Of payment, PaymentVM, IPaymentView)
    Implements IPresenter(Of payment, PaymentVM)

    Private _manuService As ManufacturerService
    Private _bankService As BankService
    Private _subjectsService As SubjectsService

    Public Sub New(view As IPaymentView, manuService As ManufacturerService, bankService As BankService, subjectsService As SubjectsService)
        MyBase.New(view)
        _presenter = Me
        _manuService = manuService
        _bankService = bankService
        _subjectsService = subjectsService
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of payment), conditions As Object) As IQueryable(Of payment) Implements IPresenter(Of payment, PaymentVM).SetSearchConditions
        Dim c As PaymentQueryVM = conditions

        query = query.Where(Function(x) x.p_Date >= c.StartDate AndAlso x.p_Date <= c.EndDate)

        If c.BankId <> 0 Then query = query.Where(Function(x) x.p_bank_Id = c.BankId)
        If c.ManufacturerId <> 0 Then query = query.Where(Function(x) x.p_m_Id = c.ManufacturerId)
        If c.SubjectsGroupId <> 0 Then query = query.Where(Function(x) x.subject.subjects_group.sg_id = c.SubjectsGroupId)
        If c.SubjectsId <> 0 Then query = query.Where(Function(x) x.p_s_Id = c.SubjectsId)
        If c.Type <> "" Then query = query.Where(Function(x) x.p_Type = c.Type)

        Return query
    End Function

    Public Function SetListViewModel(query As IQueryable(Of payment)) As List(Of PaymentVM) Implements IPresenter(Of payment, PaymentVM).SetListViewModel
        Return query.Select(Function(x) New PaymentVM With {
            .付款類型 = x.p_Type,
            .備註 = x.p_Memo,
            .廠商名稱 = x.manufacturer.manu_name,
            .支票號碼 = x.p_Cheque,
            .日期 = x.p_Date,
            .科目 = x.subject.s_name,
            .編號 = x.p_Id,
            .金額 = x.p_Amount,
            .銀行名稱 = x.bank.bank_name
        }).ToList
    End Function

    Public Sub SetManuCmb()
        _view.SetManuCmb(_manuService.GetVendorCmbItems())
    End Sub

    Public Sub SetBankCmb()
        _view.SetBankCmb(_bankService.GetBankCmbItems)
    End Sub

    Public Sub SetSubjectsGroupCmb()
        _view.SetSubjectsGroupCmb(_subjectsService.GetSubjectsGroupCmbItems)
    End Sub

    Public Sub SetSubjectsCmb(sgId As Integer)
        _view.SetSubjectsCmb(_subjectsService.GetSubjectsCmbItems(sgId))
    End Sub

    Public Sub Query()
        LoadList(_view.GetQueryConditions)
    End Sub
End Class
