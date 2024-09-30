Public Interface IPriceCalculationService
    Function CalculateUnitPrice(customer As customer, month As Date, isDelivery As Boolean, isNormalGas As Boolean) As Single
End Interface
