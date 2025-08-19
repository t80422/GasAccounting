Public Interface ICompanyView
    ' 事件
    Event Loaded As EventHandler
    Event AddClicked As EventHandler
    Event EditClicked As EventHandler
    Event DeleteClicked As EventHandler
    Event CancelClicked As EventHandler
	Event RowSelected As EventHandler(Of Integer)

	' 資料呈現
	Sub DisplayList(data As List(Of CompanyVM))
	Sub DisplayDetail(data As company)
	Sub ClearInput()
	Function GetInput() As company
End Interface
