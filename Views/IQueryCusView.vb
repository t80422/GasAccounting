Public Interface IQueryCusView
    Sub ShowList(customers As List(Of QueryCusVM))
    Sub ShowDetails(data As customer)

    ''' <summary>
    ''' 取得搜尋條件
    ''' </summary>
    ''' <returns></returns>
    Function GetSearchCondition() As customer

    Sub Reset()
End Interface
