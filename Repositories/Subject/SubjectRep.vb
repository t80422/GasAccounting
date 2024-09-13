Imports System.Data.Entity

Public Class SubjectRep
    Inherits Repository(Of subject)
    Implements ISubjectRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function GetSubjectDropdownAsync() As Task(Of IEnumerable(Of SelectListItem)) Implements ISubjectRep.GetSubjectDropdownAsync
        Try
            Return Await _dbSet.Select(Function(x) New SelectListItem With {
                .Display = x.s_name,
                .Value = x.s_id
            }).ToListAsync
        Catch ex As Exception
            Throw New Exception("取得科目下拉選單數據時發生錯誤", ex)
        End Try
    End Function
End Class
