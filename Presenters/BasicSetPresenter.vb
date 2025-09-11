Public Class BasicSetPresenter
    Private _view As IBasicSetView
    Private _bsRep As IBasicSetRep

    Public Sub New(bsRep As IBasicSetRep)
        _bsRep = bsRep
    End Sub

    Public Sub SetVeiw(view As IBasicSetView)
        _view = view
        AddHandler _view.Loaded, AddressOf OnLoaded
        AddHandler _view.SaveClicked, AddressOf OnSaveClicked
    End Sub

    Private Sub OnLoaded(sender As Object, e As EventArgs)
        Try
            Dim data = _bsRep.GetAllAsync().Result.FirstOrDefault
            If data IsNot Nothing Then
                _view.DisplayDetail(data)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Async Sub OnSaveClicked(sender As Object, e As EventArgs)
        Try
            Dim input = _view.GetInput()
            Dim data = _bsRep.GetAllAsync().Result.FirstOrDefault
            If data Is Nothing Then
                Await _bsRep.AddAsync(input)
            Else
                Await _bsRep.UpdateAsync(data, input)
            End If
            MessageBox.Show("儲存成功")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class
