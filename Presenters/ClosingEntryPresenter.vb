Public Class ClosingEntryPresenter
    Private _view As IClosingEntryView
    Private _ceRep As IClosingEntryRep
    Private _subjectRep As ISubjectRep
    Private _currentData As closing_entry

    Public Sub New(ceRep As IClosingEntryRep, subjectRep As ISubjectRep)
        _ceRep = ceRep
        _subjectRep = subjectRep
    End Sub

    Public Sub SetView(view As IClosingEntryView)
        _view = view
    End Sub

    Public Sub Reset()
        _view.ClearInput()
        LoadSubjects()
        LoadList()
        _currentData = Nothing
    End Sub

    Public Sub LoadList(Optional criteria As ClosingEntrySC = Nothing)
        Try
            Dim data = _ceRep.Search(criteria)
            _view.DisplayList(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Add()
        Try
            Dim data = _view.GetUserInput()
            _ceRep.AddAsync(data)
            Reset()
            LoadList()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Detail(id As Integer)
        Try
            Dim data = _ceRep.GetByIdAsync(id).Result

            If data IsNot Nothing Then
                _view.ClearInput()
                _currentData = data
                _view.DisplayDetail(data)
            Else
                MsgBox("找不到該檢查記錄")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Update()
        Try
            If _currentData Is Nothing Then
                MsgBox("請先選擇一條記錄")
                Return
            End If

            Dim updatedData = _view.GetUserInput()
            updatedData.ce_Id = _currentData.ce_Id
            _ceRep.UpdateAsync(_currentData.ce_Id, updatedData)
            Reset()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Delete()
        Try
            If _currentData Is Nothing Then
                MsgBox("請先選擇一條記錄")
                Return
            End If
            Dim confirm = MsgBox("確定要刪除這條記錄嗎？", MsgBoxStyle.YesNo)
            If confirm = MsgBoxResult.Yes Then
                _ceRep.DeleteAsync(_currentData.ce_Id)
                Reset()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadSubjects()
        Try
            Dim subjects = _subjectRep.GetAllAsync().Result
            Dim selectList = subjects.Select(Function(s) New SelectListItem With {
                .Value = s.s_id,
                .Display = s.s_name
            }).ToList()

            _view.SetSubjectDropdown(selectList)
        Catch ex As Exception
            MsgBox($"載入科目失敗：{ex.Message}")
        End Try
    End Sub
End Class
