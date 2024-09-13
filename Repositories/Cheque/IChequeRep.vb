Public Interface IChequeRep
    Inherits IRepository(Of cheque)

    Function GetState(cheNum As String) As String

    Function Query(startDate As Date, endDate As Date, Optional filter As cheque = Nothing) As List(Of ChequeVM)

    ''' <summary>
    ''' 將所選支票狀態更新為"已代收",並記錄代收日期
    ''' </summary>
    ''' <param name="chequeIds">選擇的支票</param>
    ''' <param name="collectionDate">代收日期</param>
    ''' <returns></returns>
    Function UpdateCollectionStatusAsync(chequeIds As List(Of Integer), collectionDate As Date) As Task

    Function GetByNumberAsync(chequeNum As String) As Task(Of cheque)
End Interface
