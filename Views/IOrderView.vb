Public Interface IOrderView
    Inherits IFormView(Of order, OrderListVM)

    ' 事件
    Event CustomerSelected As EventHandler(Of String)
    Event TransportTypeSelected As EventHandler(Of String)
    Event BarrelInInput As EventHandler
    Event BarrelOutInput As EventHandler
    Event BarrelUnitPriceInput As EventHandler
    Event CarSelected As EventHandler(Of Integer)
    Event DepositInput As EventHandler
    Event OrderTypeChanged As EventHandler
    Event ReturnInput As EventHandler
    Event PrintRequest As EventHandler(Of Integer)
    Event PrintCusStkRequest As EventHandler(Of Boolean)
    Event CustomersGasDetailRequest As EventHandler(Of Tuple(Of Date, Boolean))
    Event CusGetGasListRequest As EventHandler(Of Tuple(Of Date, Boolean))

    ''' <summary>
    ''' 顯示客戶資訊
    ''' </summary>
    ''' <param name="data"></param>
    Sub ShowCustomer(data As customer)

    ''' <summary>
    ''' 顯示單價
    ''' </summary>
    ''' <param name="data"></param>
    Sub ShowUnitPrice(data As order)

    ''' <summary>
    ''' 顯示瓦斯桶價格
    ''' </summary>
    ''' <param name="price"></param>
    Sub ShowBarrelPrice(price As Integer)

    ''' <summary>
    ''' 顯示進場寄瓶
    ''' </summary>
    ''' <param name="data"></param>
    Sub ShowCarBarrelStock_In(data As car)

    ''' <summary>
    ''' 顯示出場寄瓶
    ''' </summary>
    ''' <param name="data"></param>
    Sub ShowCarBarrelStock_Out(data As car)

    Sub ShowTotalAmount(data As Integer)

    Sub ShowInsurance(data As Double)

    ''' <summary>
    ''' 顯示總氣
    ''' </summary>
    ''' <param name="data">總普氣,總丙氣</param>
    Sub ShowGasAmount(data As Tuple(Of Integer, Integer))

    Sub ShowOrderDetail(data As order)

    Sub ShowCusBarrelStock(data As order)

    Sub DisplayList(data As IEnumerable(Of Object))

    Sub SetCarDropdown(list As List(Of SelectListItem))

    Sub SetCusBarrelStock(isIn As Boolean, data As customer)

    Sub SetReturnGasReadOnly(isReadOnly As Boolean)

    ''' <summary>
    ''' 取得進貨單輸入
    ''' </summary>
    Function GetInInput() As order

    ''' <summary>
    ''' 取得出貨單輸入
    ''' </summary>
    ''' <returns></returns>
    Function GetOutInput() As order

    Function GetOrderType() As String

    Function GetOrderInput() As order

    Sub GetCusStkInput(ByRef data As customer)

    Sub GetCarStkInput(ByRef data As car)

    Function GetSearchCriteria() As OrderSearchCriteria
End Interface
