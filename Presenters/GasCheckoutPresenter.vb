''' <summary>
''' 大氣結帳
''' </summary>
Public Class GasCheckoutPresenter
    Private ReadOnly _view As IGasCheckoutView
    Private ReadOnly _purRep As IPurchaseRep
    Private ReadOnly _manuRep As IManufacturerRep

    Public Sub New(view As IGasCheckoutView, purRep As IPurchaseRep, manuRep As IManufacturerRep)
        _view = view
        _purRep = purRep
        _manuRep = manuRep

        AddHandler _view.QueryClicked, AddressOf Query
        AddHandler _view.CheckoutClicked, AddressOf Checkout
        AddHandler _view.CancelClicked, AddressOf Cancel
    End Sub

    Public Async Sub LoadVendors()
        Try
            Dim datas = Await _manuRep.GetGasVendorCmbDataAsync
            _view.LoadVendors(datas)
        Catch ex As Exception
            _view.ShowMessage(ex.Message)
        End Try
    End Sub

    Public Async Sub LoadAllDatas()
        Try
            'Dim purchases = Await _purRep.GetAllAsync.Where(Function(x) x.pur_Checkout = False).Select(Function(x) New purchase(x))
            Dim purchases = Await _purRep.GetAllAsync
            Dim datas = purchases.Where(Function(x) Not x.pur_Checkout).Select(Function(x) New PurchaseVM)
            _view.ShowList(datas)
        Catch ex As Exception
            _view.ShowMessage(ex.Message)
        End Try
    End Sub

    Private Async Sub Query()
        Try
            Dim input = _view.GetUserInput
            If Not ValidateInput(input) Then Return

            Dim purchases = Await _purRep.GetPurchasesWithoutCheckoutAsync(input)
            Dim datas = purchases.Select(Function(x) New PurchaseVM(x))
            _view.ShowList(datas)
        Catch ex As Exception
            _view.ShowMessage(ex.Message)
        End Try
    End Sub

    Private Function ValidateInput(input As PurchaseCondition) As Boolean
        If input.IsDateSearch AndAlso input.StartDate > input.EndDate Then
            _view.ShowMessage("起始日期不能晚於結束日期")
            Return False
        End If

        Return True
    End Function

    Private Async Sub Checkout()
        Try
            Await _purRep.UpdateCheckoutStatusAsync(_view.GetSelectedIds())
            Query()
        Catch ex As Exception
            _view.ShowMessage(ex.Message)
        End Try
    End Sub

    Private Sub Cancel()
        _view.ClearInput()
        _view.RefreshView()
    End Sub
End Class