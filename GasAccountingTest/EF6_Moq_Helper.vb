Imports System.Data.Entity
Imports Moq

Module EF6_Moq_Helper
    ''' <summary>
    ''' 建立 Mock DbSet 的輔助方法 (適用於 EF6)
    ''' </summary>
    Public Function CreateMockDbSet(Of T As Class)(data As List(Of T)) As Mock(Of DbSet(Of T))
        Dim queryable = data.AsQueryable()
        Dim mockSet = New Mock(Of DbSet(Of T))()

        ' 設定 IQueryable 的行為
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.Provider).Returns(queryable.Provider)
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.Expression).Returns(queryable.Expression)
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.ElementType).Returns(queryable.ElementType)
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.GetEnumerator()).Returns(queryable.GetEnumerator())

        ' 模擬 Find 方法 (支援單一主鍵的情況)
        mockSet.Setup(Function(m) m.Find(It.IsAny(Of Object()))).Returns(
            Function(keyValues As Object())
                Dim targetId = keyValues(0)
                Dim className = GetType(T).Name.ToLower()
                ' 優先尋找 [類名]_id 或 id，若找不到才找結尾為 id 的屬性
                Dim idProp = GetType(T).GetProperties().FirstOrDefault(Function(p)
                                                                           Dim pName = p.Name.ToLower()
                                                                           Return pName = className & "_id" OrElse pName = className & "id" OrElse pName = "id"
                                                                       End Function)

                If idProp Is Nothing Then
                    idProp = GetType(T).GetProperties().FirstOrDefault(Function(p) p.Name.ToLower().EndsWith("id"))
                End If

                If idProp IsNot Nothing Then
                    ' 使用字串比較以避免 Integer/Object 類型不匹配問題
                    Return data.FirstOrDefault(Function(x)
                                                   Dim val = idProp.GetValue(x)
                                                   Return val IsNot Nothing AndAlso val.ToString() = targetId.ToString()
                                               End Function)
                End If
                Return Nothing
            End Function)

        Return mockSet
    End Function
End Module
