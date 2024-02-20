Public Interface IUnitPriceHistoryView
    ''' <summary>
    ''' 設定廠商下拉選單
    ''' </summary>
    Sub SetManuCmb(data As List(Of ComboBoxItems))

    ''' <summary>
    ''' 載入列表
    ''' </summary>
    ''' <param name="data"></param>
    Sub LoadList(data As List(Of UnitPriceHistoryVM))
End Interface
