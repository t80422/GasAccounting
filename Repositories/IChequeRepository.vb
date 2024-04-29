Public Interface IChequeRepository
    Function GetState(cheNum As String) As String

    Function Query(startDate As Date, endDate As Date, Optional filter As cheque = Nothing) As List(Of ChequeVM)
End Interface
