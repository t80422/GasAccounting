Public Interface ICheque
    Inherits IFormView(Of cheque, ChequeVM)

    Event SetBatchStatusRequest As EventHandler(Of Integer)
    Event PrintRequest As EventHandler(Of List(Of ChequeVM))

    Function GetSearchCriteria() As ChequeSC

    Function GetSelectedIds() As List(Of Integer)
End Interface