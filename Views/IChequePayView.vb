Public Interface IChequePayView
	' 事件
	Event Loaded As EventHandler
	Event SearchClicked As EventHandler
	Event CancelClicked As EventHandler
	Event RowSelected As EventHandler(Of Integer)

	' 資料呈現
	Sub DisplayList(data As List(Of ChequePayVM))
	Sub DisplayDetail(data As chque_pay)
    Sub ClearInput()
	Function GetSearchCriteria() As ChequeSC
End Interface


