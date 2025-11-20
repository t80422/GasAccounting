Imports System.Data.Entity

Public Class Repository(Of TEntity As Class)
    Implements IRepository(Of TEntity)

    Protected ReadOnly _context As gas_accounting_systemEntities
    Protected ReadOnly _dbSet As DbSet(Of TEntity)

    Public ReadOnly Property Context As DbContext Implements IRepository(Of TEntity).Context
        Get
            Return _context
        End Get
    End Property

    Public Sub New(context As gas_accounting_systemEntities)
        _context = context
        _dbSet = context.Set(Of TEntity)
    End Sub

    Public Async Function GetAllAsync() As Task(Of IEnumerable(Of TEntity)) Implements IRepository(Of TEntity).GetAllAsync
        Try
            Return Await _dbSet.AsNoTracking.ToListAsync()
        Catch ex As Exception
            Throw New Exception("獲取所有實體時發生錯誤", ex)
        End Try
    End Function

    Public Async Function GetByIdAsync(id As Integer) As Task(Of TEntity) Implements IRepository(Of TEntity).GetByIdAsync
        Try
            Dim entity = Await _dbSet.FindAsync(id)

            If entity Is Nothing Then Return Nothing

            '刷新實體,加載所有導航屬性
            Await _context.Entry(entity).ReloadAsync

            Return Await _dbSet.FindAsync(id)
        Catch ex As Exception
            Throw New Exception($"獲取Id為{id}的實體時發生錯誤", ex)
        End Try
    End Function

    Public Async Function AddAsync(entity As TEntity) As Task(Of TEntity) Implements IRepository(Of TEntity).AddAsync
        Try
            _dbSet.Add(entity)
            Await SaveChangesAsync()
            Return entity
        Catch ex As Exception
            Throw New Exception("新增時發生錯誤", ex)
        End Try
    End Function

    Public Async Function DeleteAsync(id As Integer) As Task Implements IRepository(Of TEntity).DeleteAsync
        Try
            Dim entity = Await GetByIdAsync(id)
            If entity IsNot Nothing Then
                _dbSet.Remove(entity)
                Await SaveChangesAsync()
            End If
        Catch ex As Exception
            Throw New Exception($"刪除時發生錯誤", ex)
        End Try
    End Function

    Public Async Function DeleteAsync(currentEntity As TEntity) As Task Implements IRepository(Of TEntity).DeleteAsync
        Try
            _dbSet.Remove(currentEntity)
            Await SaveChangesAsync()
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

    Public Async Function UpdateAsync(id As Integer, updateEntity As TEntity) As Task Implements IRepository(Of TEntity).UpdateAsync
        Try
            Dim existingEntity = Await GetByIdAsync(id)
            If existingEntity Is Nothing Then
                Throw New Exception($"未找到ID為{id}的實體")
            End If

            _context.Entry(existingEntity).CurrentValues.SetValues(updateEntity)
            Await SaveChangesAsync()
        Catch ex As Exception
            Throw New Exception("更新時發生錯誤", ex)
        End Try
    End Function

    Public Async Function UpdateAsync(currentEntity As TEntity, updateEntity As TEntity) As Task Implements IRepository(Of TEntity).UpdateAsync
        Try
            _context.Entry(currentEntity).CurrentValues.SetValues(updateEntity)
            Await SaveChangesAsync()
        Catch ex As Exception
            Throw New Exception("更新時發生錯誤", ex)
        End Try
    End Function

    Public Function BeginTransaction() As DbContextTransaction Implements IRepository(Of TEntity).BeginTransaction
        Return _context.Database.BeginTransaction
    End Function

    Public Sub Reload(entity As TEntity) Implements IRepository(Of TEntity).Reload
        If entity Is Nothing Then Return
        _context.Entry(entity).Reload()
    End Sub
End Class