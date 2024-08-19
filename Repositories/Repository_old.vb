Imports System.Data.Entity

Public Class Repository_old(Of T As Class)
    Implements IRepository_old(Of T)

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

    Public Sub Insert(entity As T) Implements IRepository_old(Of T).Insert
        _dbSet.Add(entity)
    End Sub

    Public Sub Update(entity As T) Implements IRepository_old(Of T).Update
        _dbSet.Attach(entity)
        _context.Entry(entity).State = EntityState.Modified
    End Sub

    Public Sub Delete(id As Integer) Implements IRepository_old(Of T).Delete
        Dim entity As T = _dbSet.Find(id)

        If _context.Entry(entity).State = EntityState.Detached Then
            _dbSet.Attach(entity)
        End If

        _dbSet.Remove(entity)
    End Sub

    Public Sub Save() Implements IRepository_old(Of T).Save
        _context.SaveChanges()
    End Sub

    Public Function GetAll() As IEnumerable(Of T) Implements IRepository_old(Of T).GetAll
        Return _dbSet.ToList
    End Function

    Public Function GetById(id As Integer) As T Implements IRepository_old(Of T).GetById
        Return _dbSet.Find(id)
    End Function
End Class
