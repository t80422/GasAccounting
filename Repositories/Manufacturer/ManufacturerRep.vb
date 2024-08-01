Public Class ManufacturerRep
    Inherits Repository(Of manufacturer)
    Implements IManufacturerRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetGasVendorsForCmb() As List(Of ComboBoxItems) Implements IManufacturerRep.GetGasVendorsForCmb
        Return _dbSet.Where(Function(x) x.manu_GasVendor = True).
                      Select(Function(x) New ComboBoxItems With {.Display = x.manu_name, .Value = x.manu_id}).ToList
    End Function
End Class
