Public Interface IBasicPriceRep
    Inherits IRepository(Of basic_price)

    Function GetByMonth(month As Date) As IEnumerable(Of basic_price)
End Interface
