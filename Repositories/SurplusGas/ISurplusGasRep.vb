Public Interface ISurplusGasRep
    Inherits IRepository(Of surplus_gas)

    Function GetList(Optional criteria As surplus_gas = Nothing) As List(Of SurplusGasListVM)
End Interface
