Public Interface IPermissionRep
    Function GetAllRolesWithPermissionsAsync() As Task(Of RolesAndPermissionsResult)
    Function GetAllPermissionsAsync() As Task(Of List(Of permission))
    Function AddRoleAsync(role As role, permissions As Dictionary(Of String, Boolean)) As Task(Of Integer)
    Function UpdateRoleAsync(role As role, permissions As Dictionary(Of String, Boolean)) As Task(Of Boolean)
    Function DeleteRoleAsync(roleId As Integer) As Task(Of Boolean)
    Function GetRoleWithPermissionsAsync(roleId As Integer) As Task(Of RoleVM)
End Interface
