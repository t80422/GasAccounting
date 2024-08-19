Public Interface IRepository_old(Of T As Class)
    Function GetAll() As IEnumerable(Of T)
    Function GetById(id As Integer) As T
    Sub Insert(entity As T)
    Sub Update(entity As T)
    Sub Delete(id As Integer)
    Sub Save()
End Interface
