Public Class PurchasePresenter
    Inherits BasePresenter(Of purchase, PurchaseVM, IPurchaseView)
    Implements IPresenter(Of purchase, PurchaseVM)

    Private _compService As ICompanyService
    Private _manuService As IManufacturerService
    Private _journalService As IJournalService

    Public Sub New(view As IPurchaseView, compService As ICompanyService, manuService As IManufacturerService)
        MyBase.New(view)
        _presenter = Me
        _compService = compService
        _manuService = manuService
        _journalService = New JournalService
    End Sub

    ''' <summary>
    ''' 設定公司下拉選單
    ''' </summary>
    Public Sub SetCompanyCmb()
        Dim data = _compService.GetCompanyComboBoxData
        _view.SetCompanyComboBox(data)
    End Sub

    ''' <summary>
    ''' 設定大氣廠商下拉選單
    ''' </summary>
    Public Sub SetGasVendorCmb()
        _view.SetGasVendorComboBox(_manuService.GetGasVendorCmbItems)
    End Sub

    ''' <summary>
    ''' 搜尋
    ''' </summary>
    Public Sub Query()
        LoadList(_view.GetSearchCondition)
    End Sub

    ''' <summary>
    ''' 取得預設產品單價、運費單價
    ''' </summary>
    ''' <param name="manuId"></param>
    Public Sub GetDefaultPrice(manuId As Integer, productName As String)
        Try
            Using db As New gas_accounting_systemEntities
                Dim unitPriceQuery = db.purchases.
                    Where(Function(x) x.pur_manu_id = manuId AndAlso x.pur_product = productName AndAlso Not x.pur_SpecialUP).
                    OrderByDescending(Function(x) x.pur_date).
                    ThenByDescending(Function(x) x.pur_id).
                    FirstOrDefault()
                Dim unitPrice As Double = 0

                If unitPriceQuery IsNot Nothing Then unitPrice = unitPriceQuery.pur_unit_price

                Dim deliveryUnitPriceQuery = db.purchases.
                    Where(Function(x) x.pur_manu_id = manuId AndAlso x.pur_product = productName AndAlso Not x.pur_SpecialDUP).
                    OrderByDescending(Function(x) x.pur_date).
                    ThenByDescending(Function(x) x.pur_id).
                    FirstOrDefault
                Dim deliveryUnitPrice As Double = 0

                If deliveryUnitPriceQuery IsNot Nothing Then deliveryUnitPrice = deliveryUnitPriceQuery.pur_deli_unit_price

                _view.SetDefaultPrice(unitPrice, deliveryUnitPrice)
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Overrides Sub Add()
        If Not CheckRequired(_view.SetRequired()) Then Exit Sub

        Dim data = _view.GetUserInput
        data.pur_SubpoenaNo = _journalService.GetSubpoenaNo()

        If data IsNot Nothing Then
            Try
                Using db As New gas_accounting_systemEntities
                    db.purchases.Add(data)

#Region "新增傳票"
                    Dim journal As New journal() With
                    {
                        .j_Amount = data.pur_price,
                        .j_Memo = data.pur_Memo,
                        .j_SubpoenaNo = data.pur_SubpoenaNo,
                        .j_s_Id = 1
                    }
                    _journalService.Add(journal)
#End Region

                    db.SaveChanges()

                    LoadList()
                    _view.ClearInput()
                    MsgBox("新增成功")
                End Using

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Protected Function SetSearchConditions(query As IQueryable(Of purchase), conditions As Object) As IQueryable(Of purchase) Implements IPresenter(Of purchase, PurchaseVM).SetSearchConditions
        Dim c As purchase = conditions

        If c.pur_comp_id <> 0 Then query = query.Where(Function(x) x.pur_comp_id = c.pur_comp_id)
        If c.pur_manu_id <> 0 Then query = query.Where(Function(x) x.pur_manu_id = c.pur_manu_id)
        If c.pur_product <> Nothing Then query = query.Where(Function(x) x.pur_product = c.pur_product)

        Return query
    End Function

    Private Function IPresenter_SetListViewModel(query As IQueryable(Of purchase)) As List(Of PurchaseVM) Implements IPresenter(Of purchase, PurchaseVM).SetListViewModel
        Try
            Return query.Select(Function(x) New PurchaseVM With {
                        .編號 = x.pur_id,
                        .日期 = x.pur_date,
                        .產品 = x.pur_product,
                        .單價 = x.pur_unit_price,
                        .重量 = x.pur_quantity,
                        .廠商 = x.manufacturer.manu_name,
                        .運費單價 = x.pur_deli_unit_price,
                        .運費 = x.pur_delivery_fee,
                        .金額 = x.pur_price,
                        .公司 = x.company.comp_name,
                        .特殊單價 = x.pur_SpecialUP,
                        .特殊運費 = x.pur_SpecialDUP
                    }).OrderByDescending(Function(x) x.編號).
                    ToList

        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Overrides Sub Edit(id As Integer)
        If Not CheckRequired(_view.SetRequired()) Then Exit Sub

        Dim data = _view.GetUserInput

        Try
            Using db As New gas_accounting_systemEntities
                If data IsNot Nothing Then
                    Dim existingSubject = db.purchases.Find(id)
                    If existingSubject IsNot Nothing Then
                        With existingSubject
                            .pur_comp_id = data.pur_comp_id
                            .pur_date = data.pur_date
                            .pur_delivery_fee = data.pur_delivery_fee
                            .pur_deli_unit_price = data.pur_deli_unit_price
                            .pur_manu_id = data.pur_manu_id
                            .pur_Memo = data.pur_Memo
                            .pur_price = data.pur_price
                            .pur_product = data.pur_product
                            .pur_quantity = data.pur_quantity
                            .pur_SpecialDUP = data.pur_SpecialDUP
                            .pur_SpecialUP = data.pur_SpecialUP
                            .pur_unit_price = data.pur_unit_price
                        End With

#Region "修改傳票"
                        Dim journal As New journal With {
                            .j_Amount = existingSubject.pur_price,
                            .j_Memo = existingSubject.pur_Memo,
                            .j_SubpoenaNo = existingSubject.pur_SubpoenaNo
                        }
                        _journalService.Edit(journal)
#End Region

                        db.SaveChanges()
                        LoadList()
                        MsgBox("修改成功。")
                    Else
                        MsgBox("未找到指定的對象。")
                    End If
                End If
            End Using

        Catch ex As Exception
            MsgBox("修改時發生錯誤: " & ex.Message)
        End Try
    End Sub

    Public Overrides Sub Delete(id As Integer)
        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.No Then Exit Sub

        Try
            Using db As New gas_accounting_systemEntities
                Dim data = db.purchases.Find(id)
                If data IsNot Nothing Then
                    '刪除傳票
                    _journalService.Delete(data.pur_SubpoenaNo)

                    db.purchases.Remove(data)
                    db.SaveChanges()
                    LoadList()
                    _view.ClearInput()
                    MsgBox("刪除成功。")

                Else
                    MsgBox("未找到要刪除的對象。")
                End If
            End Using

        Catch ex As Exception
            MsgBox("刪除時發生錯誤: " & ex.Message)
        End Try

    End Sub
End Class