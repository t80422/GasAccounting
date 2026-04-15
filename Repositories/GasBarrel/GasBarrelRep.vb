Imports System.Data.Entity
Imports MySql.Data.MySqlClient

Public Class GasBarrelRep
    Inherits Repository(Of gas_barrel)
    Implements IGasBarrelRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function GetIdByKgAsync(kg As String) As Task(Of Integer) Implements IGasBarrelRep.GetIdByKgAsync
        Try
            Dim gb = Await _dbSet.FirstOrDefaultAsync(Function(x) x.gb_Name = kg)
            Return gb.gb_Id
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function UpdateInventoryByDeltaAsync(name As String, delta As Integer) As Task Implements IGasBarrelRep.UpdateInventoryByDeltaAsync
        Dim affected = Await Context.Database.ExecuteSqlCommandAsync(
            "UPDATE gas_barrel SET gb_Inventory = gb_Inventory + @delta WHERE gb_Name = @name AND (gb_Inventory + @delta) >= 0",
            New MySqlParameter("@delta", delta),
            New MySqlParameter("@name", name)
        )
        If affected = 0 Then
            Throw New Exception($"瓦斯桶「{name}Kg」庫存不足，無法執行此操作")
        End If
    End Function

    Public Async Function SetInventoryAsync(name As String, value As Integer) As Task Implements IGasBarrelRep.SetInventoryAsync
        Dim affected = Await Context.Database.ExecuteSqlCommandAsync(
            "UPDATE gas_barrel SET gb_Inventory = @value WHERE gb_Name = @name",
            New MySqlParameter("@value", value),
            New MySqlParameter("@name", name)
        )
        If affected = 0 Then
            Throw New Exception($"找不到瓦斯桶類型：{name}")
        End If
    End Function
End Class
