Public Interface IEmployeeView
    Inherits ICommonView_old(Of employee, EmployeeVM)

    Sub SetRolesCmb(data As List(Of SelectListItem))
End Interface
