Public Interface IBasicSetView
    ' 事件
    Event Loaded As EventHandler
    Event SaveClicked As EventHandler

    ' 資料
    Sub DisplayDetail(data As basic_set)
    Function GetInput() As basic_set
End Interface
