Public Class BasicSetRep
    Inherits Repository(Of basic_set)
    Implements IBasicSetRep

    Public Sub New(dbContext As gas_accounting_systemEntities)
        MyBase.New(dbContext)
    End Sub
End Class
