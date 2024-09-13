Imports System.Data.Entity

Public Class PermissionRep
    Implements IPermissionRep

    Private ReadOnly _context As gas_accounting_systemEntities

    Public Sub New(context As gas_accounting_systemEntities)
        _context = context
    End Sub

    Public Async Function GetAllRolesWithPermissionsAsync() As Task(Of RolesAndPermissionsResult) Implements IPermissionRep.GetAllRolesWithPermissionsAsync
        Try
            Dim allPermissions = Await _context.permissions.Select(Function(x) x.per_Name).ToListAsync
            Dim rolesWithPermissions = Await _context.roles.Select(Function(x) New With {
                .Id = x.r_id,
                .Name = x.r_name,
                .Permissions = x.rolepermissions.Select(Function(y) y.permission.per_Name)
            }).ToListAsync
            Dim roles = rolesWithPermissions.Select(Function(r) New RoleVM With {
                .Id = r.Id,
                .Name = r.Name,
                .Permissions = allPermissions.ToDictionary(
                    Function(p) p,
                    Function(p) r.Permissions.Contains(p)
                )
            }).ToList()
            Return New RolesAndPermissionsResult With {
                .Roles = roles,
                .Permissions = allPermissions
            }
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw New Exception("取得角色以及權限發生錯誤")
        End Try
    End Function

    Public Async Function GetAllPermissionsAsync() As Task(Of List(Of permission)) Implements IPermissionRep.GetAllPermissionsAsync
        Try
            Return Await _context.permissions.ToListAsync
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw New Exception("取得權限對象發生錯誤")
        End Try
    End Function

    Public Async Function AddRoleAsync(role As role, permissions As Dictionary(Of String, Boolean)) As Task(Of Integer) Implements IPermissionRep.AddRoleAsync
        Using transaction As DbContextTransaction = _context.Database.BeginTransaction
            Try
                _context.roles.Add(role)
                Await _context.SaveChangesAsync
                Await UpdateRolePermissionsAsync(role.r_id, permissions)
                transaction.Commit()
                Return role.r_id
            Catch ex As Exception
                transaction.Rollback()
                Console.WriteLine(ex.Message)
                Throw New Exception("新增角色發生錯誤")
            End Try
        End Using

    End Function

    Public Async Function UpdateRoleAsync(role As role, permissions As Dictionary(Of String, Boolean)) As Task(Of Boolean) Implements IPermissionRep.UpdateRoleAsync
        Try
            Dim existingRole = Await _context.roles.FindAsync(role.r_id)
            If existingRole Is Nothing Then Return False
            existingRole.r_name = role.r_name
            Await _context.SaveChangesAsync
            Await UpdateRolePermissionsAsync(role.r_id, permissions)
            Return True
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw New Exception("修改角色發生錯誤")
        End Try
    End Function

    Public Async Function DeleteRoleAsync(roleId As Integer) As Task(Of Boolean) Implements IPermissionRep.DeleteRoleAsync
        Try
            Dim role = Await _context.roles.FindAsync(roleId)
            _context.roles.Remove(role)
            Await _context.SaveChangesAsync
            Return True
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw New Exception("刪除角色發生錯誤")
        End Try
    End Function

    Public Async Function GetRoleWithPermissionsAsync(roleId As Integer) As Task(Of RoleVM) Implements IPermissionRep.GetRoleWithPermissionsAsync
        Try
            Dim allPermissions = Await _context.permissions.Select(Function(x) x.per_Name).ToListAsync
            Dim roleData = Await _context.roles.
                Where(Function(r) r.r_id = roleId).
                Select(Function(r) New With {
                    .Id = r.r_id,
                    .Name = r.r_name,
                    .Permissions = r.rolepermissions.Select(Function(rp) rp.permission.per_Name)
                }).FirstOrDefaultAsync()
            If roleData Is Nothing Then
                Return Nothing
            End If
            Dim role = New RoleVM With {
                .Id = roleData.Id,
                .Name = roleData.Name,
                .Permissions = allPermissions.ToDictionary(
                    Function(p) p,
                    Function(p) roleData.Permissions.Contains(p)
                )
            }

            Return role
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw New Exception("獲取角色及其權限時發生錯誤")
        End Try
    End Function

    Private Async Function UpdateRolePermissionsAsync(roleId As Integer, permissions As Dictionary(Of String, Boolean)) As Task
        Try
            Dim existingPermissions = Await _context.rolepermissions.Where(Function(x) x.rp_r_Id = roleId).ToListAsync
            _context.rolepermissions.RemoveRange(existingPermissions)

            Dim allPermissions = Await _context.permissions.ToListAsync

            For Each permission In allPermissions
                If permissions.ContainsKey(permission.per_Name) AndAlso permissions(permission.per_Name) Then
                    _context.rolepermissions.Add(New rolepermission With {
                        .rp_r_Id = roleId,
                        .rp_per_Id = permission.per_Id
                    })
                End If
            Next

            Await _context.SaveChangesAsync
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.WriteLine(ex.InnerException.Message)
            Throw New Exception("設定角色與權限關聯時發生錯誤")
        End Try

    End Function
End Class
