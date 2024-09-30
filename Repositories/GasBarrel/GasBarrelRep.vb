Imports System.Data.Entity

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
End Class
