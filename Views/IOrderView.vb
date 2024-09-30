Public Interface IOrderView
    Inherits IBaseView(Of order, OrderVM)

    Sub DisplayCustomer(data As customer)

    Sub DisplayCusStk(data As customer, isIn As Boolean)

    Sub DisplayCarStk(data As car, isIn As Boolean)

    Sub DisplayGasAndPrice(gas As Integer, gasC As Integer, amount As Single, insurance As Single)

    Sub DisplayInsurance(price As Single)

    ''' <summary>
    ''' 取得進貨單輸入
    ''' </summary>
    Function GetInInput() As order

    ''' <summary>
    ''' 取得出貨單輸入
    ''' </summary>
    ''' <returns></returns>
    Function GetOutInput() As order

    Function GetOrderInput() As order

    Sub GetCusStkInput(currentEntity As customer)

    Sub GetCarStkInput(currentEntity As car)

    Sub SetCarDropdown(list As List(Of SelectListItem))

    Function GetSearchCriteria() As OrderSearchCriteria

End Interface
