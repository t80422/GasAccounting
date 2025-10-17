Imports System.IO
Imports ClosedXML.Excel

Public Class ChequePayPresenter
    Private ReadOnly _rep As IChequePayRep
    Private ReadOnly _view As IChequePayView

    Public ReadOnly Property View As IChequePayView
        Get
            Return _view
        End Get
    End Property

    Public Sub New(view As IChequePayView, rep As IChequePayRep)
        _view = view
        _rep = rep

        AddHandler _view.Loaded, AddressOf OnLoaded
        AddHandler _view.SearchClicked, AddressOf OnSearchClicked
        AddHandler _view.CancelClicked, AddressOf OnCancelClicked
        AddHandler _view.RowSelected, AddressOf OnRowSelected
        AddHandler _view.PrintClicked, AddressOf OnPrintClicked
    End Sub

    Private Sub OnLoaded(sender As Object, e As EventArgs)
        LoadList()
    End Sub

    Private Sub OnCancelClicked(sender As Object, e As EventArgs)
        _view.ClearInput()
        LoadList()
    End Sub

    Private Sub OnSearchClicked(sender As Object, e As EventArgs)
        Dim criteria = _view.GetSearchCriteria()
        If criteria IsNot Nothing Then
            Try
                Dim items = _rep.GetList(criteria)
                _view.DisplayList(items)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub

    Private Async Sub OnRowSelected(sender As Object, id As Integer)
        Try
            Dim entity = Await _rep.GetByIdAsync(id)
            If entity IsNot Nothing Then
                _view.DisplayDetail(entity)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadList(Optional criteria As ChequeSC = Nothing)
        Try
            Dim items = _rep.GetList(criteria)
            _view.DisplayList(items)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub OnPrintClicked(sender As Object, e As EventArgs)
        Try
            ' 取得資料
            Dim datas = _view.GetDGVData

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "應付支票管理範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    .WriteToCell("A1", "應付支票管理")

                    Dim rowIndex = 3

                    For Each item In datas
                        .WriteToCell(rowIndex, 1, item.日期)
                        .WriteToCell(rowIndex, 2, item.支票號碼)
                        .WriteToCell(rowIndex, 3, item.對方銀行)
                        .WriteToCell(rowIndex, 4, item.銀行帳號)
                        .WriteToCell(rowIndex, 5, item.金額.ToString)
                        .WriteToCell(rowIndex, 6, item.支票兌現日期)
                        .WriteToCell(rowIndex, 7, item.備註)

                        rowIndex += 1
                    Next

                    .SetCustomBorders(rowIndex, 1, rowIndex, 7, XLBorderStyleValues.Thin)
                    .WriteToCell(rowIndex, 4, "合計")
                    .WriteToCell(rowIndex, 5, datas.Sum(Function(x) x.金額).ToString("N0"))

                    .SaveExcel("應付支票管理")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class


