Public Class ScrapBarrelRep
    Inherits Repository(Of scrap_barrel)
    Implements IScrapBarrelRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub
End Class
