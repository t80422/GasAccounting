Public Class ManufacturerService
    Implements IManufacturerService

    Public Function GetGasVendorCmbItems() As List(Of ComboBoxItems) Implements IManufacturerService.GetGasVendorCmbItems
        Try
            Using db As New gas_accounting_systemEntities
                Return db.manufacturers.Where(Function(m) m.manu_GasVendor = True).Select(Function(x) New ComboBoxItems With {
                    .Value = x.manu_id,
                    .Display = x.manu_name
                }).ToList
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetVendorCmbItems() As List(Of ComboBoxItems) Implements IManufacturerService.GetVendorCmbItems
        Try
            Using db As New gas_accounting_systemEntities
                Return db.manufacturers.Select(Function(x) New ComboBoxItems With {
                    .Display = x.manu_name,
                    .Value = x.manu_id
                }).ToList
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
End Class
