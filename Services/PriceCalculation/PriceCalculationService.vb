Public Class PriceCalculationService
    Implements IPriceCalculationService

    Private ReadOnly _bpRep As IBasicPriceRep

    Public Sub New(bpRep As IBasicPriceRep)
        _bpRep = bpRep
    End Sub

    Public Function CalculateUnitPrice(customer As customer, month As Date, isDelivery As Boolean, isNormalGas As Boolean) As Single Implements IPriceCalculationService.CalculateUnitPrice
        Try
            Dim baseUnitPrice As Single
            Dim customerPriceAdjustment As Single

            '每個月2號才是當月的開始
            If month.Day = 1 Then month = month.AddDays(-1)

            Dim basePrice = _bpRep.GetByMonth(month)

            If basePrice Is Nothing Then Throw New Exception("請先設定當月基礎價格")

            Dim pricePlan = customer.priceplan

            If isDelivery Then
                baseUnitPrice = If(isNormalGas, basePrice.bp_Delivery_Normal, basePrice.bp_Delivery_C)

                If pricePlan IsNot Nothing Then
                    customerPriceAdjustment = If(isNormalGas, pricePlan.pp_GasDelivery, pricePlan.pp_GasDelivery_c)
                End If
            Else
                baseUnitPrice = If(isNormalGas, basePrice.bp_normal_out, basePrice.bp_c_out)

                If pricePlan IsNot Nothing Then
                    customerPriceAdjustment = If(isNormalGas, pricePlan.pp_Gas, pricePlan.pp_Gas_c)
                End If
            End If

            Return baseUnitPrice + customerPriceAdjustment
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
