Public Interface IChequePayRep
    Inherits IRepository(Of chque_pay)

    Function GetByChequeNumber(chequeNumber As String) As chque_pay
    Function Search(criteria As ChequeSC) As Task(Of List(Of chque_pay))
End Interface
