Public Class OrderCollectionMappingRep
    Inherits Repository(Of order_collection_mapping)
    Implements IOrderCollectionMappingRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Sub DeleteRange(ocms As List(Of order_collection_mapping)) Implements IOrderCollectionMappingRep.DeleteRange
        Try
            _dbSet.RemoveRange(ocms)
            _context.SaveChanges()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function GetByOrderId(ordId As Integer) As List(Of order_collection_mapping) Implements IOrderCollectionMappingRep.GetByOrderId
        Try
            Return _dbSet.Where(Function(x) x.ocm_o_Id = ordId).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetByColId(colId As Integer) As List(Of order_collection_mapping) Implements IOrderCollectionMappingRep.GetByColId
        Try
            Return _dbSet.Where(Function(x) x.ocm_col_Id = colId).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
