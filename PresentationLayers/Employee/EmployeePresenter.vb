Public Class EmployeePresenter
    Inherits BasePresenter(Of employee, EmployeeVM, IEmployeeView)
    Implements IPresenter(Of employee, EmployeeVM)

    Public Sub New(view As IEmployeeView)
        MyBase.New(view)
        _presenter = Me
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of employee), conditions As Object) As IQueryable(Of employee) Implements IPresenter(Of employee, EmployeeVM).SetSearchConditions
        Return Nothing
    End Function

    Public Function SetListViewModel(query As IQueryable(Of employee)) As List(Of EmployeeVM) Implements IPresenter(Of employee, EmployeeVM).SetListViewModel
        Return query.Select(Function(x) New EmployeeVM With {
            .編號 = x.emp_id,
            .姓名 = x.emp_name,
            .電話 = x.emp_phone,
            .地址 = x.emp_address,
            .密碼 = x.emp_psw,
            .帳號 = x.emp_acc,
            .權限 = x.role.r_name,
            .生日 = x.emp_birthday,
            .緊急聯絡人 = x.emp_ecp,
            .緊急聯絡人電話 = x.emp_ect,
            .身分證 = x.emp_identity_number
        }).ToList
    End Function

    Public Sub GetRolesCmb()
        Try
            Using db As New gas_accounting_systemEntities
                _view.SetRolesCmb(db.roles.Select(Function(x) New ComboBoxItems With {.Display = x.r_name, .Value = x.r_id}).ToList)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
