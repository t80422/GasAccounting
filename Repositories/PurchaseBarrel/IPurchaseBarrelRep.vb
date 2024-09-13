Public Interface IPurchaseBarrelRep
    Inherits IRepository(Of purchase_barrel)

    Function SearchAsync(criteria As PurBarrelSC) As Task(Of List(Of purchase_barrel))
    Function GetByMonthAsync(month As Date) As Task(Of List(Of purchase_barrel))
End Interface
