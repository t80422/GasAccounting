Public Class CarRepository
    Inherits Repository(Of car)
    Implements ICarRepository

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetCmbByCusId(cusId As Integer) As List(Of ComboBoxItems) Implements ICarRepository.GetCmbByCusId
        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.cars.Where(Function(x) x.c_cus_id = cusId)
                Dim formatQuery = query.ToList.Select(Function(x) New ComboBoxItems With {
                    .Value = x.c_id,
                    .Display = $"{x.c_no} {x.c_driver}"
                }).ToList
                Return formatQuery
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetCarById(carId As Integer) As car Implements ICarRepository.GetCarById
        Try
            Using db As New gas_accounting_systemEntities
                Return db.cars.Find(carId)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
End Class
