Public Interface IScrapBarrelView
    Sub ClearInput()
    Sub ShowList(data As List(Of ScrapBarrelVM))
    Function GetInput() As scrap_barrel
    Sub ShowDetail(data As scrap_barrel)
End Interface
