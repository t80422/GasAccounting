Public Interface IRepository(Of TEntity As Class)
    Function GetAllAsync() As Task(Of IEnumerable(Of TEntity))
    Function GetByIdAsync(id As Integer) As Task(Of TEntity)
    Function AddAsync(entity As TEntity) As Task
    'Function UpdateAsync(entity As TEntity) As Task
    Function DeleteAsync(entity As TEntity) As Task
    Function SaveChangesAsync() As Task
End Interface
