Public Interface IGasBarrelRep
    Inherits IRepository(Of gas_barrel)

    Function GetIdByKgAsync(kg As String) As Task(Of Integer)
    Function UpdateInventoryByDeltaAsync(name As String, delta As Integer) As Task
    Function SetInventoryAsync(name As String, value As Integer) As Task
End Interface
