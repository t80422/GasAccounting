Public Interface IManufacturerView
    Inherits ICommonView(Of manufacturer, ManufacturerVM)

    ''' <summary>
    ''' 取得搜尋條件
    ''' </summary>
    ''' <returns></returns>
    Function GetSearchConditions() As manufacturer
End Interface
