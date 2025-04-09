Public Interface IBasicPriceRep
    Inherits IRepository(Of basic_price)

    Function GetByMonth(month As Date) As basic_price
    Function GetByNearestDate(day As Date) As basic_price
    Function CheckDuplicateDateAsync(day As Date) As Task(Of Boolean)
    Function SearchAsync(criteria As basic_price) As Task(Of IEnumerable(Of basic_price))
End Interface
