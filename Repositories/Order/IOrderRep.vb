Public Interface IOrderRep
    'Inherits IRepository_old(Of order)
    Inherits IRepository(Of order)

    ''' <summary>
    ''' 取得客戶提氣量憑單資料
    ''' </summary>
    ''' <param name="orderId"></param>
    ''' <returns></returns>
    Function GetOrderVoucherData(orderId As Integer) As OrderVoucherVM

    Function SearchAsync(criteria As OrderSearchCriteria) As Task(Of List(Of order))
End Interface
