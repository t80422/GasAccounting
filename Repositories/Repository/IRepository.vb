Imports System.Data.Entity

Public Interface IRepository(Of TEntity As Class)
    ReadOnly Property Context As DbContext
    Function GetAllAsync() As Task(Of IEnumerable(Of TEntity))
    Function GetByIdAsync(id As Integer) As Task(Of TEntity)
    Function AddAsync(entity As TEntity) As Task(Of TEntity)
    Sub AddEntityOnly(entity As TEntity)
    Function UpdateAsync(id As Integer, updateEntity As TEntity) As Task
    Function UpdateAsync(currentEntity As TEntity, updateEntity As TEntity) As Task
    Function DeleteAsync(id As Integer) As Task
    Function DeleteAsync(currentEntity As TEntity) As Task
    Function SaveChangesAsync() As Task
    Function BeginTransaction() As DbContextTransaction
    Sub Reload(entity As TEntity)
    Sub DetachAllUnchanged()
End Interface
