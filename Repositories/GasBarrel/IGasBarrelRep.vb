Public Interface IGasBarrelRep
    Inherits IRepository(Of gas_barrel)

    Function GetIdByKgAsync(kg As String) As Task(Of Integer)
End Interface
