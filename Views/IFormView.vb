Public Interface IFormView(Of TDataViewModel, TListViewModel)
    ' 事件
    Event CreateRequest As EventHandler
    Event DataSelectedRequest As EventHandler(Of Integer)
    Event UpdateRequest As EventHandler
    Event DeleteRequest As EventHandler
    Event CancelRequest As EventHandler
    Event SearchRequest As EventHandler

    ' View 動作
    Sub ShowList(data As List(Of TListViewModel))
    Sub ShowDetail(data As TDataViewModel)
    Function GetInput(ByRef model As TDataViewModel) As Boolean
    Sub ClearInput()
    Sub ButtonStatus(isSelectedRow As Boolean)
End Interface
