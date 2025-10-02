Public Interface ICustomerView
    Inherits IFormView(Of customer, CustomerVM)

    Event PricePlanSelectedChange As EventHandler(Of Integer)

    Sub SetPricePlanDetails(data As priceplan)

    Sub PopulatePricePlanDropdown(data As List(Of SelectListItem))

    Sub ClearPricePlan()

    Sub SetCompanyDropdown(data As List(Of SelectListItem))

    Function GetSearchCriteria() As customer
End Interface
