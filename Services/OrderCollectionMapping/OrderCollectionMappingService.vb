Public Class OrderCollectionMappingService
    Implements IOrderCollectionMappingService

    Private ReadOnly _ocmRep As IOrderCollectionMappingRep
    Private ReadOnly _ordRep As IOrderRep
    Private ReadOnly _colRep As ICollectionRep

    Public Sub New(ocmRep As IOrderCollectionMappingRep, ordRep As IOrderRep, colRep As ICollectionRep)
        _ocmRep = ocmRep
        _ordRep = ordRep
        _colRep = colRep
    End Sub

    Public Function CalculateOrderUnPaid(ordId As Integer) As Integer Implements IOrderCollectionMappingService.CalculateOrderUnPaid
        Try
            Dim ords = _ocmRep.GetByOrderId(ordId)

            Return If(ords IsNot Nothing, ords.Sum(Function(x) x.ocm_Amount), 0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function CalculateCollectionUnmatched(colId As Integer) As Integer Implements IOrderCollectionMappingService.CalculateCollectionUnmatched
        Try
            Dim cols = _ocmRep.GetByColId(colId)

            Return If(cols IsNot Nothing, cols.Sum(Function(x) x.ocm_Amount), 0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Sub DeleteOrder(ordId As Integer) Implements IOrderCollectionMappingService.DeleteOrder
        Try
            ' 取得該訂單銷帳紀錄
            Dim ocmByOrds = _ocmRep.GetByOrderId(ordId)

            ' 取得所有收款單編號
            Dim colIds = ocmByOrds.Select(Function(x) x.ocm_col_Id)

            ' 刪除該訂單銷帳明細
            _ocmRep.DeleteRange(ocmByOrds)

            For Each colId In colIds
                Dim col = Await _colRep.GetByIdAsync(colId)
                Dim ocmByColIds = _ocmRep.GetByColId(colId)
                Dim matches As Integer = 0

                If ocmByColIds IsNot Nothing Then
                    matches = ocmByColIds.Sum(Function(x) x.ocm_Amount)
                End If

                col.col_UnmatchedAmount = col.col_Amount - matches
            Next

            Await _colRep.SaveChangesAsync
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Async Sub DeleteCollection(colId As Integer) Implements IOrderCollectionMappingService.DeleteCollection
        Try
            ' 取得該收款單銷帳紀錄
            Dim ocmByCols = _ocmRep.GetByColId(colId)

            ' 取得所有訂單編號
            Dim ordIds = ocmByCols.Select(Function(x) x.ocm_o_Id)

            ' 刪除該收款單銷帳明細
            _ocmRep.DeleteRange(ocmByCols)

            For Each ordId In ordIds
                Dim ord = Await _ordRep.GetByIdAsync(ordId)
                Dim ocmByOrdIds = _ocmRep.GetByOrderId(ordId)
                Dim paids As Integer = 0

                If ocmByOrdIds IsNot Nothing Then
                    paids = ocmByOrdIds.Sum(Function(x) x.ocm_Amount)
                End If
            Next

            Await _ordRep.SaveChangesAsync
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class
