Public Class PricePlanRep
    Inherits Repository_old(Of priceplan)
    Implements IPricePlanRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub
End Class
