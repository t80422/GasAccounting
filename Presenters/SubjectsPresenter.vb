Public Class SubjectsPresenter
    Inherits BasePresenter(Of subject, SubjectsVM, ISubjectsView)
    Implements IPresenter(Of subject, SubjectsVM)

    Public Sub New(view As ISubjectsView)
        MyBase.New(view)
        _presenter = Me
    End Sub

    Public Overrides Sub Delete(id As Integer)
        Dim list = New List(Of Integer) From {1, 2}
        If list.Contains(id) Then
            MsgBox("此為基本選項,無法刪除")
            Return
        End If

        MyBase.Delete(id)
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of subject), conditions As Object) As IQueryable(Of subject) Implements IPresenter(Of subject, SubjectsVM).SetSearchConditions
        Return Nothing
    End Function

    Public Function SetListViewModel(query As IQueryable(Of subject)) As List(Of SubjectsVM) Implements IPresenter(Of subject, SubjectsVM).SetListViewModel
        Return query.Select(Function(x) New SubjectsVM With {
            .備註 = x.s_memo,
            .名稱 = x.s_name,
            .編號 = x.s_id,
            .類型 = x.s_Type
        }).ToList
    End Function
End Class
