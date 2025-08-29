Imports System.IO
Imports ClosedXML.Excel

Public Class ChequePayPresenter
    Private ReadOnly _rep As IChequePayRep
    Private _view As IChequePayView

    Public Sub New(rep As IChequePayRep)
        _rep = rep
    End Sub

    Public Sub SetView(view As IChequePayView)
        _view = view
        AddHandler _view.Loaded, AddressOf OnLoaded
        AddHandler _view.SearchClicked, AddressOf OnSearchClicked
        AddHandler _view.CancelClicked, AddressOf OnCancelClicked
        AddHandler _view.RowSelected, AddressOf OnRowSelected
        AddHandler _view.PrintClicked, AddressOf OnPrintClicked
    End Sub

    Private Async Sub OnLoaded(sender As Object, e As EventArgs)
        Await LoadAllAsync()
    End Sub

    Private Async Sub OnCancelClicked(sender As Object, e As EventArgs)
        _view.ClearInput()
        Await LoadAllAsync()
    End Sub

    Private Async Sub OnSearchClicked(sender As Object, e As EventArgs)
        Dim criteria = _view.GetSearchCriteria()
        If criteria IsNot Nothing Then
            Try
                Dim items = Await _rep.Search(criteria)
                _view.DisplayList(ToViewModel(items))
            Catch ex As Exception
                MsgBox(ex.Message)
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

    Private Async Function LoadAllAsync() As Task
        Try
            Dim items = Await _rep.GetAllAsync()
            _view.DisplayList(ToViewModel(items))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Function ToViewModel(items As IEnumerable(Of chque_pay)) As List(Of ChequePayVM)
        Return items.Select(Function(x) New ChequePayVM With {
            .編號 = x.cp_Id,
            .日期 = x.cp_Date.Value.ToString("yyyy/MM/dd"),
            .支票號碼 = x.cp_Number,
            .金額 = x.cp_Amount,
            .兌現日期 = If(x.cp_CashingDate.HasValue, x.cp_CashingDate.Value.ToString("yyyy/MM/dd"), Nothing),
            .是否兌現 = If(x.cp_IsCashing, False),
            .備註 = If(x.cp_Memo, ""),
            .對方銀行 = If(x.cp_AccountNumber, ""),
            .銀行帳號 = If(x.bank Is Nothing, "", x.bank.bank_Account)
        }).ToList()
    End Function

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
                        .WriteToCell(rowIndex, 6, item.兌現日期)
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


