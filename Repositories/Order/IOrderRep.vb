Public Interface IOrderRep
    Inherits IRepository(Of order)

    ''' <summary>
    ''' 取得客戶提氣量憑單資料
    ''' </summary>
    ''' <param name="orderId"></param>
    ''' <returns></returns>
    Function GetOrderVoucherData(orderId As Integer) As OrderVoucherVM

    Function SearchAsync(criteria As OrderSearchCriteria) As Task(Of List(Of order))

    Function GetByMonth(month As Date) As List(Of order)

    Function GetByMonthAndCompany(month As Date, compId As Integer) As List(Of order)
End Interface
