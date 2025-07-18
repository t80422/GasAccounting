Public Interface IScrapBarrelDetailRep
    Inherits IRepository(Of scrap_barrel_detail)

    Function GetBySBId(sbId As Integer) As List(Of scrap_barrel_detail)
End Interface
