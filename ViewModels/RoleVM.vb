Public Class RoleVM
    Public Property Id As Integer
    Public Property Name As String
    Public Property Permissions As Dictionary(Of String, Boolean)
    Public Function GetPermissionValue(permissionName As String) As Boolean
        Dim value As Boolean
        If Permissions.TryGetValue(permissionName, value) Then
            Return value
        End If

        Return False
    End Function
End Class
