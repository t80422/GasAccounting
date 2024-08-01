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

    Public Sub LoadVendors()
        Try
            Dim datas = _manuRep.GetGasVendorsForCmb
            _view.LoadVendors(datas)
        Catch ex As Exception
            _view.ShowMessage(ex.Message)
        End Try
    End Sub

    Public Sub LoadAllDatas()
        Try
            Dim datas = _purRep.GetAll.Where(Function(x) x.pur_Checkout = False).Select(Function(x) New PurchaseVM(x)).ToList
            _view.ShowList(datas)
        Catch ex As Exception
            _view.ShowMessage(ex.Message)
        End Try
    End Sub

    Private Sub Query()
        Try
            Dim input = _view.GetUserInput
            If Not ValidateInput(input) Then Return

            Dim datas = _purRep.GetForCheckout(input)
            _view.ShowList(datas)
        Catch ex As Exception
            _view.ShowMessage(ex.Message)
        End Try
    End Sub

    Private Function ValidateInput(input As GasCheckoutUserInput) As Boolean
        If input.IsDateSearch AndAlso input.StartDate > input.EndDate Then
            _view.ShowMessage("起始日期不能晚於結束日期")
            Return False
        End If

        Return True
    End Function

    Private Async Sub Checkout(selectdDatas As List(Of Integer))
        Try
            Await _purRep.UpdateCheckoutStatusAsync(selectdDatas)
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