Public Interface IOrderRepository
    Function QueryOrders(condition As OrderQueryVM, isQuery As Boolean) As List(Of order)
    Sub Add(data As order, car As car, cus As customer)
    Sub Edit(data As order, car As car, cus As customer)
    Sub Delete(ordId As Integer, car As car, cus As customer)
    Function GetOrderById(id As Integer) As order
End Interface
