Public Interface ICusOrdByCarService
    ReadOnly Property OrdRep As IOrderRepository

    ReadOnly Property CusRep As ICustomerRepository

    ReadOnly Property CarRep As ICarRepository

    ReadOnly Property BPRep As IBasicPriceRep

    Sub Insert(order As order, customer As customer, car As car)

    Sub Update(order As order, customer As customer, car As car, orgCar As car, orgOrd As order)

    Sub Delete(order As order, car As car, customer As customer)

    Function SearchOrders(Optional criteria As OrderSearchCriteria = Nothing) As List(Of order)
End Interface
