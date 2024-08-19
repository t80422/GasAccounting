Public Class PermissionPresenter
    Private ReadOnly _view As IPermissionView
    Private ReadOnly _rep As IPermissionRep

    Public Sub New(view As IPermissionView, rep As IPermissionRep)
        _view = view
        _rep = rep
    End Sub

    Public Async Function LoadRolesAndPermissionsAsync() As Task
        Try
            Dim result = Await _rep.GetAllRolesWithPermissionsAsync
            _view.SetRolesAndPermissions(result.Roles, result.Permissions)

            Dim permissions = Await _rep.GetAllPermissionsAsync
            _view.SetPermissions(permissions)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Function AddAsync() As Task
        Try
            Dim roleName = _view.GetRoleName
            If String.IsNullOrEmpty(roleName) Then
                MsgBox("請輸入角色名稱")
                Return
            End If
            Dim permissions = _view.GetSelectedPermissions
            Dim newRole = New role With {.r_name = roleName}
            Await _rep.AddRoleAsync(newRole, permissions)
            Await LoadRolesAndPermissionsAsync()
            _view.ClearInputs()
            MsgBox("新增成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Function UpdateAsync() As Task
        Try
            Dim roleId = _view.GetSelectedRoleId
            If roleId <= 0 Then
                MsgBox("請選擇角色")
                Return
            End If

            Dim roleName = _view.GetRoleName
            If String.IsNullOrEmpty(roleName) Then
                MsgBox("請輸入角色名稱")
                Return
            End If

            Dim permissions = _view.GetSelectedPermissions
            Dim updatedRole = New role With {.r_id = roleId, .r_name = roleName}

            If Await _rep.UpdateRoleAsync(updatedRole, permissions) Then
                MsgBox("修改成功")
                Await LoadRolesAndPermissionsAsync()
            Else
                MsgBox("修改失敗")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Function DeleteAsync() As Task
        Try
            Dim roleId = _view.GetSelectedRoleId
            If roleId <= 0 Then
                MsgBox("請選擇角色")
                Return
            End If

            If Await _rep.DeleteRoleAsync(roleId) Then
                MsgBox("刪除成功")
                Await LoadRolesAndPermissionsAsync()
                _view.ClearInputs()
            End If
        Catch ex As Exception
            MsgBox("刪除失敗:" + ex.Message)
        End Try
    End Function

    Public Async Function SelectRoleAsync(roleId As Integer) As Task
        Try
            Dim selectedRole = Await _rep.GetRoleWithPermissionsAsync(roleId)
            If selectedRole IsNot Nothing Then
                _view.SetDataToControls(selectedRole)
            End If
        Catch ex As Exception
            MsgBox("獲取角色數據時發生錯誤: " & ex.Message)
        End Try
    End Function
End Class
