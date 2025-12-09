Public Class UnitOfWorkFactory
    Implements IUnitOfWorkFactory

    Public Function Create() As IUnitOfWork Implements IUnitOfWorkFactory.Create
        Return New UnitOfWork()
    End Function
End Class

