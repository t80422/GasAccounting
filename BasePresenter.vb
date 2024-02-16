Public MustInherit Class BasePresenter(Of TEntity As Class, TViewModel, TView As ICommonView(Of TEntity, TViewModel))
    Protected ReadOnly _view As TView
    Protected _presenter As IPresenter(Of TEntity, TViewModel)

    ''' <summary>
    ''' 要設定_presenter=Me
    ''' </summary>
    ''' <param name="view"></param>
    Public Sub New(view As TView)
        _view = view
    End Sub

    ''' <summary>
    ''' 取得列表
    ''' </summary>
    ''' <param name="conditions"></param>
    Public Overridable Sub LoadList(Optional conditions As Object = Nothing)
        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.Set(Of TEntity).AsQueryable

                If conditions IsNot Nothing AndAlso _presenter IsNot Nothing Then
                    query = _presenter.SetSearchConditions(query, conditions)
                End If

                _view.ShowList(_presenter.SetListViewModel(query))
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 新增
    ''' </summary>
    Public Overridable Sub Add()
        Dim data = _view.GetUserInput

        If data IsNot Nothing Then
            Try
                Using db As New gas_accounting_systemEntities
                    db.Set(Of TEntity).Add(data)
                    db.SaveChanges()
                    LoadList()
                    _view.ClearInput()
                    MsgBox("新增成功")
                End Using

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' 取得選取的資料
    ''' </summary>
    ''' <param name="id"></param>
    Public Overridable Sub SelectRow(id As Integer)
        Try
            Using db As New gas_accounting_systemEntities
                Dim data = db.Set(Of TEntity).Find(id)
                If data IsNot Nothing Then
                    _view.ClearInput()
                    _view.SetDataToControl(data)
                End If
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 修改
    ''' </summary>
    Public Overridable Sub Edit(id As Integer)
        Try
            Using db As New gas_accounting_systemEntities
                Dim data = _view.GetUserInput

                If data IsNot Nothing Then
                    Dim existingSubject = db.Set(Of TEntity).Find(id)

                    If existingSubject IsNot Nothing Then
                        db.Entry(existingSubject).CurrentValues.SetValues(data)
                        db.SaveChanges()
                        LoadList()
                        MsgBox("修改成功。")
                    Else
                        MsgBox("未找到指定的對象。")
                    End If
                End If

            End Using

        Catch ex As Exception
            MsgBox("修改時發生錯誤: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 刪除
    ''' </summary>
    ''' <param name="id"></param>
    Public Overridable Sub Delete(id As Integer)
        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.No Then Exit Sub

        Try
            Using db As New gas_accounting_systemEntities
                Dim data = db.Set(Of TEntity).Find(id)
                If data IsNot Nothing Then
                    db.Set(Of TEntity).Remove(data)
                    db.SaveChanges()
                    LoadList()
                    _view.ClearInput()
                    MsgBox("刪除成功。")

                Else
                    MsgBox("未找到要刪除的對象。")
                End If
            End Using

        Catch ex As Exception
            MsgBox("刪除時發生錯誤: " & ex.Message)
        End Try
    End Sub
End Class
