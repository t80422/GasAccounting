Public Interface IOrderView
    Sub ShowList(list As List(Of OrderVM))

    ''' <summary>
    ''' 顯示客戶庫存
    ''' </summary>
    ''' <param name="data"></param>
    Sub ShowCusStk(data As customer)

    ''' <summary>
    ''' 設定車選單
    ''' </summary>
    ''' <param name="list"></param>
    Sub SetCmbCar(list As List(Of ComboBoxItems))

    Function GetUserInput_ord() As order

    Function GetUserInput_cus(data As customer, container As Control) As customer

    Function GetUserInput_car(data As car, container As Control) As car

    Sub Reset()

    Sub ShowDetails(data As order)

    Function GetQueryCondition() As OrderQueryVM
End Interface
