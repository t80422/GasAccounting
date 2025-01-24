Public Interface IOrderCollectionMappingService
    Function CalculateOrderUnPaid(ordId As Integer) As Integer
    Function CalculateCollectionUnmatched(colId As Integer) As Integer
    Sub DeleteOrder(ordId As Integer)
    Sub DeleteCollection(colId As Integer)
End Interface
