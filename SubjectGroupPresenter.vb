Public Class SubjectGroupPresenter
    Private _view As ISubjectGroupView
    Private _db As gas_accounting_systemEntities ' 替換成您的數據模型類

    Public Sub New(view As ISubjectGroupView)
        _view = view
        _db = New gas_accounting_systemEntities() ' 初始化您的數據模型
    End Sub

    ''' <summary>
    ''' 從數據庫加載並顯示到 View
    ''' </summary>
    Public Sub LoadList()
        Try
            Dim subjects = _db.subjects_group.Select(Function(x) New SubjectGroupVM With {
                .編號 = x.sg_id,
                .名稱 = x.sg_name,
                .借貸 = x.sg_type
            }).ToList ' 替換成從模型獲取數據的方法
            _view.DisplaySubjects(subjects)
        Catch ex As Exception
            MsgBox("錯誤: " & ex.Message)
        End Try
    End Sub

    Public Sub Add(subject As subjects_group)
        Try
            _db.subjects_group.Add(subject)
            _db.SaveChanges() ' 保存更改到資料庫
            MsgBox("科目新增成功。")
            _view.ClearInputs()
            LoadList() ' 重新加載並顯示更新後的科目列表
        Catch ex As Exception
            MsgBox("新增科目時發生錯誤: " & ex.Message)
        End Try
    End Sub

    Public Sub SelectRow(id As Integer)
        Dim sg = _db.subjects_group.Find(id)
        If sg IsNot Nothing Then _view.SetSubject(sg)
    End Sub

    Public Sub Edit(subject As subjects_group)
        Try
            Dim existingSubject = _db.subjects_group.Find(subject.sg_id)
            If existingSubject IsNot Nothing Then
                ' 使用反射來更新屬性
                For Each prop In GetType(subjects_group).GetProperties()
                    If prop.CanRead AndAlso prop.CanWrite Then
                        Dim newValue = prop.GetValue(subject)
                        prop.SetValue(existingSubject, newValue)
                    End If
                Next

                _db.SaveChanges()
                MsgBox("修改成功。")
                LoadList()
            Else
                MsgBox("未找到指定的科目。")
            End If
        Catch ex As Exception
            MsgBox("修改時發生錯誤: " & ex.Message)
        End Try
    End Sub

    Public Sub Delete(Id As Integer)
        Try
            Dim data = _db.subjects_group.Find(Id)
            If data IsNot Nothing Then
                _db.subjects_group.Remove(data)
                _db.SaveChanges()
                MsgBox("刪除成功。")
                _view.ClearInputs()
                LoadList()
            Else
                MsgBox("未找到要刪除的科目。")
            End If
        Catch ex As Exception
            MsgBox("刪除科目時發生錯誤: " & ex.Message)
        End Try
    End Sub
End Class
