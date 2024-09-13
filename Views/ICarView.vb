Public Interface ICarView
    Inherits IBaseView(Of car, CarVM)

    Sub DisplayCustomer(cusId As Integer, cusCode As String, cusName As String)
    Function GetSearchCriteria() As CarSC
End Interface
