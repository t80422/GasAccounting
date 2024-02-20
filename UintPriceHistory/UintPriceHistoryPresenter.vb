Public Class UintPriceHistoryPresenter
    Private _view As IUnitPriceHistoryView
    Private _maunService As ManufacturerService

    Public Sub New(view As IUnitPriceHistoryView, manuService As ManufacturerService)
        _view = view
        _maunService = manuService
    End Sub

    ''' <summary>
    ''' 取得大氣廠商選單資料
    ''' </summary>
    Public Sub GetManuCmb()
        _view.SetManuCmb(_maunService.GetGasVendorCmbItems)
    End Sub

    Public Sub Query(manuId As Integer, product As String, startDate As Date, endDate As Date)
        endDate = endDate.AddDays(1).AddSeconds(-1)

        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.purchases.Where(Function(x) x.pur_date >= startDate AndAlso x.pur_date <= endDate)
                If manuId <> 0 Then query = query.Where(Function(x) x.pur_manu_id = manuId)
                If product <> "" Then query = query.Where(Function(x) x.pur_product = product)

                Dim monthlyFirst = query.GroupBy(Function(x) New With {Key .Year = x.pur_date.Value.Year, Key .Month = x.pur_date.Value.Month}) _
                                           .Select(Function(g) g.OrderBy(Function(x) x.pur_date).FirstOrDefault())

                Dim result = monthlyFirst.Select(Function(x) New UnitPriceHistoryVM With {
                    .單價 = x.pur_unit_price,
                    .廠商 = x.manufacturer.manu_name,
                    .日期 = x.pur_date,
                    .產品 = x.pur_product,
                    .運費單價 = x.pur_deli_unit_price
                }).ToList
                _view.LoadList(result)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
