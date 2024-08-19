Imports System.Data.Entity

Public Class Repository(Of TEntity As Class)
    Implements IRepository(Of TEntity)

    Protected ReadOnly _context As gas_accounting_systemEntities
    Protected ReadOnly _dbSet As DbSet(Of TEntity)

    Public Sub New(context As gas_accounting_systemEntities)
        _context = context
        _dbSet = context.Set(Of TEntity)
    End Sub

    Public Async Function GetAllAsync() As Task(Of IEnumerable(Of TEntity)) Implements IRepository(Of TEntity).GetAllAsync
        Try
            Return Await _dbSet.ToListAsync()
        Catch ex As Exception
            Throw New Exception("獲取所有實體時發生錯誤", ex)
        End Try
    End Function

    Public Async Function GetByIdAsync(id As Integer) As Task(Of TEntity) Implements IRepository(Of TEntity).GetByIdAsync
        Try
            Return Await _dbSet.FindAsync(id)
        Catch ex As Exception
            Throw New Exception($"獲取Id為{id}的實體時發生錯誤", ex)
        End Try
    End Function

    Public Async Function AddAsync(entity As TEntity) As Task Implements IRepository(Of TEntity).AddAsync
        Try
            _dbSet.Add(entity)
            Await Task.CompletedTask
        Catch ex As Exception
            Throw New Exception("新增時發生錯誤", ex)
        End Try
    End Function

    'Public Async Function UpdateAsync(entity As TEntity) As Task Implements IRepository(Of TEntity).UpdateAsync
    '    Try
    '        _context.Entry(entity).State = EntityState.Modified
    '        Await Task.CompletedTask
    '    Catch ex As Exception
    '        Throw New Exception("更新時發生錯誤", ex)
    '    End Try
    'End Function

    Public Async Function DeleteAsync(entity As TEntity) As Task Implements IRepository(Of TEntity).DeleteAsync
        Try
            If entity IsNot Nothing Then _dbSet.Remove(entity)
            Await Task.CompletedTask
        Catch ex As Exception
            Throw New Exception($"刪除時發生錯誤", ex)
        End Try
    End Function

    Public Async Function SaveChangesAsync() As Task Implements IRepository(Of TEntity).SaveChangesAsync
        Try
            Await _context.SaveChangesAsync()
        Catch ex As Exception
            Throw New Exception("EF保存更變時發生錯誤", ex)
        End Try
    End Function
End Class