Public Interface ICarRepository
    Function GetCmbByCusId(cusId As Integer) As List(Of ComboBoxItems)
    Function GetCarById(carId As Integer) As car
End Interface