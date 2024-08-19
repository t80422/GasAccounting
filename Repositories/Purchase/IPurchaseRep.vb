Public Interface IPurchaseRep
    Inherits IRepository(Of purchase)

    Function SearchPurchasesAsync(conditions As PurchaseCondition) As Task(Of IEnumerable(Of purchase))

    ''' <summary>
    ''' 取得上筆單價、運費單價作為預設價格
    ''' </summary>
    ''' <param name="manuId"></param>
    ''' <param name="productName"></param>
    ''' <returns>單價,運費單價</returns>
    Function GetDefaultPricesAsync(manuId As Integer, productName As String) As Task(Of Tuple(Of Single, Single))

    ''' <summary>
    ''' 取得未結帳的訂貨資訊
    ''' </summary>
    ''' <returns></returns>
    Function GetPurchasesWithoutCheckoutAsync(input As PurchaseCondition) As Task(Of IEnumerable(Of purchase))

    Function UpdateCheckoutStatusAsync(ids As List(Of Integer)) As Task(Of Integer)
End Interface