Public Class SurplusGasRep
    Inherits Repository(Of surplus_gas)
    Implements ISurplusGasRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetList(Optional criteria As surplus_gas = Nothing) As List(Of SurplusGasListVM) Implements ISurplusGasRep.GetList
        Return GetAllAsync.Result.Select(Function(x) New SurplusGasListVM(x)).ToList()
    End Function
End Class
