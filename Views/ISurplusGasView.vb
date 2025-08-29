Public Interface ISurplusGasView
    ' 事件
    Event Loaded As EventHandler
    Event SearchClicked As EventHandler
    Event CancelClicked As EventHandler

    ' 資料
    Sub DisplayList(data As List(Of SurplusGasListVM))
    Function GetSearchCriteria() As Object
End Interface
