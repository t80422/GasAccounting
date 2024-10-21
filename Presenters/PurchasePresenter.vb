Imports System.IO

''' <summary>
''' 大氣採購
''' </summary>
Public Class PurchasePresenter
    Private ReadOnly _view As IPurchaseView
    Private ReadOnly _purRep As IPurchaseRep
    Private ReadOnly _compRep As ICompanyRep
    Private ReadOnly _manuRep As IManufacturerRep
    Private ReadOnly _subRep As ISubjectRep
    Private ReadOnly _gmbSer As IGasMonthlyBalanceService

    Public Sub New(view As IPurchaseView, purRep As IPurchaseRep, compRep As ICompanyRep, manuRep As IManufacturerRep, subRep As ISubjectRep, gmbSer As IGasMonthlyBalanceService)
        _view = view
        _purRep = purRep
        _compRep = compRep
        _manuRep = manuRep
        _subRep = subRep
        _gmbSer = gmbSer
    End Sub

    Public Async Function InitializeAsync() As Task
        Try
            Await Task.WhenAll(
                SetCompanyCmbAsync,
                SetGasVendorCmbAsync,
                SetDriveCompanyCmbAsync,
                SetSubjectCmbAsync
            )
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Function GetDefaultPriceAsync(manuId As Integer, productName As String) As Task
        Try
            Dim result = Await _purRep.GetDefaultPricesAsync(manuId, productName)
            _view.SetDefaultPrice(result.Item1, result.Item2)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Function LoadListAsync() As Task
        Try
            Dim conditions = _view.GetSearchCondition
            Dim purchases = Await _purRep.SearchPurchasesAsync(conditions)
            Dim datas = purchases.Select(Function(x) New PurchaseVM(x)).ToList
            _view.ShowList(datas)
        Catch ex As Exception
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub Validate(data As purchase)
        If data.pur_comp_id = 0 Then Throw New Exception("請選擇公司")
        If String.IsNullOrEmpty(data.pur_product) Then Throw New Exception("請選擇產品")
        If data.pur_manu_id = 0 Then Throw New Exception("請選擇大氣廠商")
        If data.pur_quantity = 0 Then Throw New Exception("請輸入重量")
        If String.IsNullOrEmpty(data.pur_PayType) Then Throw New Exception("請選擇付款方式")
        If data.pur_unit_price = 0 Then Throw New Exception("請輸入單價")
    End Sub

    Public Async Function AddAsync() As Task
        _view.GetUserInput()
        Using transaction = _purRep.BeginTransaction
            Try
                Dim purchase = _view.CurrentPurchase
                Validate(purchase)

                Await _purRep.AddAsync(purchase)
                Await _purRep.SaveChangesAsync()

                _gmbSer.UpdateOrAdd(purchase.pur_date, purchase.pur_comp_id)

                transaction.Commit()
                _view.ClearInput()
                Await LoadListAsync()
                MsgBox("新增成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Function

    Public Async Function EditAsync() As Task
        _view.GetUserInput()
        Using transaction = _purRep.BeginTransaction
            Try
                Dim purchase = _view.CurrentPurchase
                Validate(purchase)
                Await _purRep.SaveChangesAsync

                _gmbSer.UpdateOrAdd(purchase.pur_date, purchase.pur_comp_id)

                transaction.Commit()
                _view.ClearInput()
                Await LoadListAsync()
                MsgBox("修改成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Function

    Public Async Function DeleteAsync(id As Integer) As Task
        Try
            Await _purRep.DeleteAsync(id)
            Await _purRep.SaveChangesAsync
            _view.ClearInput()
            Await LoadListAsync()
            MsgBox("刪除成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Function SelectRowAsync(id As Integer) As Task
        Try
            Dim data = Await _purRep.GetByIdAsync(id)
            If data IsNot Nothing Then
                _view.ClearInput()
                _view.CurrentPurchase = data
                _view.SetDataToControls(data)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Sub Print(datas As List(Of PurchaseVM))
        Try
            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "大氣採購範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim rowIndex = 3

                    For Each item In datas
                        .WriteToCell(rowIndex, 1, item.廠商)
                        .WriteToCell(rowIndex, 2, item.日期.ToString("yyyy年MM月dd日"))
                        .WriteToCell(rowIndex, 3, item.產品)
                        .WriteToCell(rowIndex, 4, item.重量)
                        .WriteToCell(rowIndex, 5, item.單價)
                        .WriteToCell(rowIndex, 6, item.金額)
                        .WriteToCell(rowIndex, 7, item.公司)
                        .WriteToCell(rowIndex, 8, item.運費單價)
                        .WriteToCell(rowIndex, 9, item.運費)
                        .WriteToCell(rowIndex, 10, item.特殊單價)
                        .WriteToCell(rowIndex, 11, item.特殊運費)
                        .WriteToCell(rowIndex, 12, item.科目)
                        .WriteToCell(rowIndex, 13, item.付款方式)

                        rowIndex += 1
                    Next

                    '存檔
                    Dim exportFilePath = Path.Combine(Application.StartupPath, "報表", "大氣採購.xlsx")
                    .SaveAs(exportFilePath)
                    .Print(exportFilePath)
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Async Function SetCompanyCmbAsync() As Task
        Try
            Dim companies = Await _compRep.GetCompanyDropdownAsync
            _view.SetCompanyCmb(companies)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Async Function SetGasVendorCmbAsync() As Task
        Try
            Dim data = Await _manuRep.GetGasVendorCmbDataAsync
            _view.SetGasVendorCmb(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Async Function SetDriveCompanyCmbAsync() As Task
        Try
            Dim data = Await _manuRep.GetVendorCmbWithoutGasAsync
            _view.SetDriveVendorCmb(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Async Function SetSubjectCmbAsync() As Task
        Try
            Dim data = Await _subRep.GetSubjectDropdownAsync
            _view.SetSubjectCmb(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function
End Class