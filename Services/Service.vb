Public Class Service
    ''' <summary>
    ''' 取得價格方案選項
    ''' </summary>
    ''' <returns></returns>
    Public Function GetPricePlan_Cmb() As List(Of ComboBoxItems)
        Try
            Using db As New gas_accounting_systemEntities
                Return db.priceplans.Select(Function(x) New ComboBoxItems With {
                    .Value = x.pp_Id,
                    .Display = x.pp_Name
                }).ToList
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
End Class
