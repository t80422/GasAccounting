Public Interface IScrapBarrelDetailView
    Sub ClearInput()
    Sub ShowList(data As List(Of ScrapBarrelDetailVM))
    Function GetInput() As scrap_barrel_detail
    Sub ShowDetail(data As scrap_barrel_detail)
End Interface
