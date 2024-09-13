''' <summary>
''' 大氣採購
''' </summary>
Public Class PurchasePresenter
    Private ReadOnly _view As IPurchaseView
    Private ReadOnly _purRep As IPurchaseRep
    Private ReadOnly _compRep As ICompanyRep
    Private ReadOnly _manuRep As IManufacturerRep
    Private ReadOnly _subRep As ISubjectRep

    Public Sub New(view As IPurchaseView, purRep As IPurchaseRep, compRep As ICompanyRep, manuRep As IManufacturerRep, subRep As ISubjectRep)
        _view = view
        _purRep = purRep
        _compRep = compRep
        _manuRep = manuRep
        _subRep = subRep
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
        Try
            Dim purchase = _view.CurrentPurchase
            Validate(purchase)
            Await _purRep.AddAsync(purchase)
            Await _purRep.SaveChangesAsync()
            _view.ClearInput()
            Await LoadListAsync()
            MsgBox("新增成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Function EditAsync() As Task
        _view.GetUserInput()

        Try
            Dim purchase = _view.CurrentPurchase
            Validate(purchase)
            Await _purRep.SaveChangesAsync
            _view.ClearInput()
            Await LoadListAsync()
            MsgBox("修改成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
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
End Class