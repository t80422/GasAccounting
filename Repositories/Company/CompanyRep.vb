Imports System.Data.Entity

Public Class CompanyRep
    Inherits Repository(Of company)
    Implements ICompanyRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function GetCmbDataAsync() As Task(Of IEnumerable(Of ComboBoxItems)) Implements ICompanyRep.GetCmbDataAsync
        Try
            Return Await _dbSet.Select(Function(x) New ComboBoxItems With {
                .Display = x.comp_name,
                .Value = x.comp_id
            }).ToListAsync
        Catch ex As Exception
            Throw New Exception("取得公司下拉選單資料時發生錯誤", ex)
        End Try
    End Function
End Class
