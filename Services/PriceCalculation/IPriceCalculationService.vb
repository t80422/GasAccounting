Public Interface IPriceCalculationService
    ''' <summary>
    ''' 計算單價
    ''' </summary>
    ''' <param name="customer">客戶</param>
    ''' <param name="day">日期</param>
    ''' <param name="isDelivery">是否廠運</param>
    ''' <param name="isNormalGas">是否普氣</param>
    ''' <returns></returns>
    Function CalculateUnitPrice(customer As customer, day As Date, isDelivery As Boolean, isNormalGas As Boolean) As Single
End Interface
