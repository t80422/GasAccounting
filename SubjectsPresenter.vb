Public Class SubjectsPresenter
    Private _view As ISubjectsView
    Private _db As gas_accounting_systemEntities

    Public Sub New(view As ISubjectsView)
        _view = view
        _db = New gas_accounting_systemEntities
    End Sub

    Public Sub LoadList(id As Integer)
        Try
            Dim lst = _db.subjects.
                Where(Function(x) x.s_sg_id = id).
                Select(Function(x) New SubjectsVM With {
                    .編號 = x.s_id,
                    .名稱 = x.s_name,
                    .備註 = x.s_memo
                }).ToList

            _view.DisplaySubjects(lst)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Add(subjects As subject)
        Try
            _db.subjects.Add(subjects)
            _db.SaveChanges()
            MsgBox("新增成功")
            _view.ClearInputs()
            LoadList(subjects.s_sg_id)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub SelectRow(id As Integer)
        Dim data = _db.subjects.Find(id)
        If data IsNot Nothing Then _view.SetSebject(data)
    End Sub

    Public Sub Edit(data As subject)
        Try
            Dim existingSubject = _db.subjects.Find(data.s_id)
            If existingSubject IsNot Nothing Then
                ' 使用反射來更新屬性
                For Each prop In GetType(subject).GetProperties()
                    If prop.CanRead AndAlso prop.CanWrite Then
                        Dim newValue = prop.GetValue(data)
                        prop.SetValue(existingSubject, newValue)
                    End If
                Next

                _db.SaveChanges()
                MsgBox("修改成功。")
                _view.ClearInputs()
                LoadList(data.s_sg_id)
            Else
                MsgBox("未找到指定的科目。")
            End If
        Catch ex As Exception
            MsgBox("修改時發生錯誤: " & ex.Message)
        End Try
    End Sub

    Public Sub Delete(Id As Integer)
        Try
            Dim data = _db.subjects.Find(Id)
            If data IsNot Nothing Then
                _db.subjects.Remove(data)
                _db.SaveChanges()
                MsgBox("刪除成功。")
                _view.ClearInputs()
                LoadList(data.s_sg_id)
            Else
                MsgBox("未找到要刪除的科目。")
            End If
        Catch ex As Exception
            MsgBox("刪除科目時發生錯誤: " & ex.Message)
        End Try
    End Sub
End Class
