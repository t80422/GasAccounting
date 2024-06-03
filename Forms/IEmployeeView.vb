Public Interface IEmployeeView
    Inherits ICommonView(Of employee, EmployeeVM)

    Sub SetRolesCmb(data As List(Of ComboBoxItems))
End Interface
