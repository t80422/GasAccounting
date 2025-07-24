Imports System.Runtime.InteropServices

Public Class ScrapBarrelDetailPresenter
    Private _view As IScrapBarrelDetailView
    Private _sbdRep As IScrapBarrelDetailRep
    Private _cusRep As ICustomerRep
    Private _currentData As scrap_barrel_detail

    Public Sub New(sbdRep As IScrapBarrelDetailRep, cusRep As ICustomerRep)
        _sbdRep = sbdRep
        _cusRep = cusRep
    End Sub

    Public Sub SetView(view As IScrapBarrelDetailView)
        _view = view
    End Sub

    Public Sub Reset()
        Try
            _view.ClearInput()
            _currentData = Nothing
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub LoadList(sbId As Integer)
        Try
            Dim data = _sbdRep.GetBySBId(sbId)
            Dim result = data.Select(Function(x) New ScrapBarrelDetailVM(x)).ToList()
            _view.ShowList(result)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Add()
        Try
            Dim data = _view.GetInput
            _sbdRep.AddAsync(data)
            Reset()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub LoadDetail(id As Integer)
        Try
            Dim data = _sbdRep.GetByIdAsync(id).Result
            _currentData = data
            _view.ClearInput()
            _view.ShowDetail(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Update()
        Try
            If _currentData Is Nothing Then
                MsgBox("No data selected for update.")
                Return
            End If
            Dim updatedData = _view.GetInput()
            updatedData.sbd_Id = _currentData.sbd_Id ' Ensure the ID remains the same
            _sbdRep.UpdateAsync(_currentData.sbd_Id, updatedData)
            Reset()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Delete()
        Try
            If _currentData Is Nothing Then
                MsgBox("No data selected for deletion.")
                Return
            End If
            _sbdRep.DeleteAsync(_currentData.sbd_Id)
            Reset()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function GetCusByCusCode(cusCode As String) As customer
        Try
            Return _cusRep.GetByCusCode(cusCode)
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
End Class
