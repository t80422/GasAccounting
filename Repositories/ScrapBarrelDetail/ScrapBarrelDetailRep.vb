Public Class ScrapBarrelDetailRep
    Inherits Repository(Of scrap_barrel_detail)
    Implements IScrapBarrelDetailRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetBySBId(sbId As Integer) As List(Of scrap_barrel_detail) Implements IScrapBarrelDetailRep.GetBySBId
        Return _dbSet.Where(Function(x) x.sbd_sb_Id = sbId).ToList
    End Function
End Class
