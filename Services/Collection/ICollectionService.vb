Public Interface ICollectionService
    Function AddAsync(input As collection,
                      Optional chequeInput As cheque = Nothing,
                      Optional chequeNumber As String = Nothing) As Task(Of collection)

    Function DeleteAsync(collectionId As Integer) As Task
End Interface

