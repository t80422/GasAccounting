''' <summary>
''' 大氣結帳
''' </summary>
Public Class GasCheckoutPresenter
    Private ReadOnly _view As IGasCheckoutView
    Private ReadOnly _purRep As IPurchaseRep
    Private ReadOnly _manuRep As IManufacturerRep

    Public ReadOnly Property View As IGasCheckoutView
        Get
            Return _view
        End Get
    End Property

    Public Sub New(view As IGasCheckoutView, purRep As IPurchaseRep, manuRep As IManufacturerRep)
        _view = view
        _purRep = purRep
        _manuRep = manuRep

        AddHandler _view.QueryClicked, AddressOf LoadList
        AddHandler _view.CheckoutClicked, AddressOf Checkout
        AddHandler _view.CancelClicked, AddressOf Reset
    End Sub

    Private Sub Reset()
        Try
            _view.ClearInput()
            LoadVendors()
            LoadList()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Async Sub LoadVendors()
        Try
            Dim datas = Await _manuRep.GetGasVendorCmbDataAsync
            _view.LoadVendors(datas)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub LoadList()
        Try
            Dim criteria = _view.GetUserInput
            Dim datas = _purRep.GetPurchasesWithoutCheckoutAsync(criteria).Result.Select(Function(x) New PurchaseVM(x)).ToList
            _view.ShowList(datas)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Async Sub Checkout()
        Try
            Await _purRep.UpdateCheckoutStatusAsync(_view.GetSelectedIds())
            LoadList()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class