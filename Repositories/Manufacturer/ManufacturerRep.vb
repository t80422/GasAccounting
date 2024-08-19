Imports System.Data.Entity

Public Class ManufacturerRep
    Inherits Repository(Of manufacturer)
    Implements IManufacturerRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function GetGasVendorCmbDataAsync() As Task(Of IEnumerable(Of ComboBoxItems)) Implements IManufacturerRep.GetGasVendorCmbDataAsync
        Try
            Return Await _dbSet.Where(Function(x) x.manu_GasVendor).
                                Select(Function(x) New ComboBoxItems With {
                                    .Display = x.manu_name,
                                    .Value = x.manu_id
                                }).ToListAsync
        Catch ex As Exception
            Throw New Exception("取得瓦斯供應商下拉列表數據時發生錯誤", ex)
        End Try
    End Function

    Public Async Function GetVendorCmbWithoutGasAsync() As Task(Of IEnumerable(Of ComboBoxItems)) Implements IManufacturerRep.GetVendorCmbWithoutGasAsync
        Try
            Return Await _dbSet.Where(Function(x) Not x.manu_GasVendor).
                                Select(Function(x) New ComboBoxItems With {
                                    .Display = x.manu_name,
                                    .Value = x.manu_id
                                }).ToListAsync
        Catch ex As Exception
            Throw New Exception("取得非瓦斯供應商的廠商下拉選單數據時發生錯誤", ex)
        End Try
    End Function
End Class
