Public Interface ISurplusGasView
    ' 事件
    Event Loaded As EventHandler
    Event SearchClicked As EventHandler
    Event AddClicked As EventHandler
    Event EditClicked As EventHandler
    Event DeleteClicked As EventHandler
    Event CancelClicked As EventHandler
    Event RowSelected As EventHandler(Of Integer)
    Event PrintClicked As EventHandler

    ' 資料
    Sub DisplayList(data As List(Of SurplusGasListVM))
    Sub DisplayDetail(data As surplus_gas)
    Function GetSearchCriteria() As Object
    Function GetInput() As surplus_gas
    Sub ClearInput()
    Sub ButtonControl(isCreate As Boolean)
End Interface
