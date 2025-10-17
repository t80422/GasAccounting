Public Interface IChequeRep
    Inherits IRepository(Of cheque)

    ''' <summary>
    ''' 將所選支票狀態更新為"已代收",並記錄代收日期
    ''' </summary>
    ''' <param name="chequeIds">選擇的支票</param>
    ''' <param name="collectionDate">代收日期</param>
    ''' <returns></returns>
    Function UpdateCollectionStatusAsync(chequeIds As List(Of Integer), collectionDate As Date) As Task

    Sub UpdateRedeemedStatus(chequeIds As List(Of Integer), redeemedDate As Date)

    Function GetByNumberAsync(chequeNum As String) As Task(Of cheque)

    Function GetList(Optional criteria As ChequeSC = Nothing) As List(Of cheque)
End Interface
