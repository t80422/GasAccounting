Public Class CompanyPresenter
    Private _view As ICompanyView
    Private _companyRep As ICompanyRep
    Private currentData As company

    Public Sub New(companyRep As ICompanyRep)
        _companyRep = companyRep
    End Sub

    Public Sub SetView(view As ICompanyView)
        _view = view
        AddHandler _view.Loaded, AddressOf OnLoaded
        AddHandler _view.AddClicked, AddressOf OnAddClicked
        AddHandler _view.EditClicked, AddressOf OnEditClicked
        AddHandler _view.DeleteClicked, AddressOf OnDeleteClicked
        AddHandler _view.RowSelected, AddressOf OnRowSelected
        AddHandler _view.CancelClicked, AddressOf OnCancelClicked
    End Sub

    Private Sub OnLoaded(sender As Object, e As EventArgs)
        LoadList()
    End Sub

    Private Async Sub OnAddClicked(sender As Object, e As EventArgs)
        Try
            Dim data = _view.GetInput
            Await _companyRep.AddAsync(data)
            OnCancelClicked(sender, e)
            MsgBox("新增成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Async Sub OnEditClicked(sender As Object, e As EventArgs)
        Try
            Dim input = _view.GetInput

            If currentData Is Nothing Then
                MsgBox("請先選擇要編輯的公司")
                Return
            End If

            Await _companyRep.UpdateAsync(currentData, input)
            OnCancelClicked(sender, e)
            MsgBox("編輯成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Async Sub OnDeleteClicked(sender As Object, e As EventArgs)
        Try
            Await _companyRep.DeleteAsync(currentData)
            OnCancelClicked(sender, e)
            MsgBox("刪除成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub OnRowSelected(sender As Object, id As Integer)
        Try
            Dim data = _companyRep.GetByIdAsync(id).Result

            If data IsNot Nothing Then
                currentData = data
                _view.ClearInput()
                _view.DisplayDetail(data)
            Else
                MsgBox("找不到該公司資料")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub OnCancelClicked(sender As Object, e As EventArgs)
        _view.ClearInput()
        LoadList()
        currentData = Nothing
    End Sub

    Private Sub LoadList()
        Try
            Dim query = _companyRep.GetAllAsync.Result
            _view.DisplayList(query.Select(Function(x) New CompanyVM(x)).ToList())
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
