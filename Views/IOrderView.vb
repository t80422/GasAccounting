Public Interface IOrderView
    ''' <summary>
    ''' 顯示客戶庫存
    ''' </summary>
    ''' <param name="data"></param>
    Sub ShowCusStk(data As customer)

    ''' <summary>
    ''' 顯示車庫存
    ''' </summary>
    ''' <param name="data"></param>
    Sub ShowCarStk(data As car)

    ''' <summary>
    ''' 設定車選單
    ''' </summary>
    ''' <param name="list"></param>
    Sub SetCmbCar(list As List(Of ComboBoxItems))

    Sub GetUserInput_ord(order As order)

    Function GetUserInput_cus(data As customer, container As Control) As customer

    Function GetUserInput_car(data As car, container As Control) As car

    Sub Reset()

    Sub ShowDetails(data As order)

    Sub ShowOrderList(orders As IEnumerable(Of Object))
End Interface
