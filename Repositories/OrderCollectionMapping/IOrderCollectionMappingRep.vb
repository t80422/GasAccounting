Public Interface IOrderCollectionMappingRep
    Inherits IRepository(Of order_collection_mapping)

    Function GetByOrderId(ordId As Integer) As List(Of order_collection_mapping)
    Function GetByColId(colId As Integer) As List(Of order_collection_mapping)
    Sub DeleteRange(ocms As List(Of order_collection_mapping))
End Interface
