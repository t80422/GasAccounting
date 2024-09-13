Public Interface ICustomerView
    Inherits IBaseView(Of customer, CustomerVM)

    Sub SetPricePlanDetails(data As priceplan)

    Sub PopulatePricePlanDropdown(data As List(Of SelectListItem))

    Sub ClearPricePlan()
End Interface
