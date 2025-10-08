Public Interface IChequePayRep
    Inherits IRepository(Of chque_pay)

    Function GetByChequeNumber(chequeNumber As String) As chque_pay
    Function GetList(Optional criteria As ChequeSC = Nothing) As List(Of ChequePayVM)
End Interface
