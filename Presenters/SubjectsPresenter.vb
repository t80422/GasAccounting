Public Class SubjectsPresenter
    Private ReadOnly _view As ISubjectsView
    Private ReadOnly _subjectRep As ISubjectRep

    Public Sub New(view As ISubjectsView, subjectRep As ISubjectRep)
        _view = view
        _subjectRep = subjectRep
    End Sub

    Public Async Sub LoadList(Optional criteria As subject = Nothing)
        Try
            Dim lst As List(Of SubjectsVM) = Nothing

            If criteria Is Nothing Then
                Dim subjects = Await _subjectRep.GetAllAsync()
                lst = subjects.Select(Function(x) New SubjectsVM(x)).ToList
            End If

            _view.ClearInput()
            _view.ShowList(lst)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Add()
        Try
            If Not CheckRequired(_view.SetRequired()) Then Exit Sub

            Dim data = _view.GetUserInput()
            Await _subjectRep.AddAsync(data)
            Await _subjectRep.SaveChangesAsync
            _view.ClearInput()
            LoadList()
            MsgBox("新增成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub LoadDetail(id As Integer)
        Try
            Dim data = Await _subjectRep.GetByIdAsync(id)
            _view.ClearInput()
            _view.SetDataToControl(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Update()
        Try
            Dim data = _view.GetUserInput
            Await _subjectRep.UpdateAsync(data.s_id, data)
            _view.ClearInput()
            LoadList()
            MsgBox("修改成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Delete(id As Integer)
        Try
            Await _subjectRep.DeleteAsync(id)
            _view.ClearInput()
            LoadList()
            MsgBox("刪除成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
