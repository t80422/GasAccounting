Public Interface IPermissionView
    Sub SetRolesAndPermissions(roles As List(Of RoleVM), permissions As List(Of String))
    Sub SetPermissions(permissions As List(Of permission))
    Function GetRoleName() As String
    Function GetSelectedPermissions() As Dictionary(Of String, Boolean)
    Sub ClearInputs()
    Sub SetDataToControls(role As RoleVM)
    Function GetSelectedRoleId() As Integer
End Interface
