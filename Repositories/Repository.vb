Imports System.Data.Entity

Public Class Repository(Of T As Class)
    Implements IRepository(Of T)

    Protected _context As gas_accounting_systemEntities
    Protected _dbSet As DbSet(Of T)

    Public Sub New(context As gas_accounting_systemEntities)
        _context = context
        _dbSet = context.Set(Of T)
    End Sub

    Public ReadOnly Property Context As gas_accounting_systemEntities
        Get
            Return _context
        End Get
    End Property

    Public Sub Insert(entity As T) Implements IRepository(Of T).Insert
        _dbSet.Add(entity)
    End Sub

    Public Sub Update(entity As T) Implements IRepository(Of T).Update
        _context.Entry(entity).State = EntityState.Modified
    End Sub

    Public Sub Delete(id As Integer) Implements IRepository(Of T).Delete
        Dim entity As T = _dbSet.Find(id)
        _dbSet.Remove(entity)
    End Sub

    Public Sub Save() Implements IRepository(Of T).Save
        _context.SaveChanges()
    End Sub

    Public Function GetAll() As IEnumerable(Of T) Implements IRepository(Of T).GetAll
        Return _dbSet.ToList
    End Function

    Public Function GetById(id As Integer) As T Implements IRepository(Of T).GetById
        Return _dbSet.Find(id)
    End Function
End Class
