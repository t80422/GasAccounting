' 新瓶作業
Public Class PurchaseBarrelUserControl
    Implements IPurchaseBarrelView

    ' 定義業務事件
    Public Event CreateRequest As EventHandler Implements IFormView(Of purchase_barrel, PurchaseBarrelVM).CreateRequest
    Public Event DataSelectedRequest As EventHandler(Of Integer) Implements IFormView(Of purchase_barrel, PurchaseBarrelVM).DataSelectedRequest
    Public Event UpdateRequest As EventHandler Implements IFormView(Of purchase_barrel, PurchaseBarrelVM).UpdateRequest
    Public Event DeleteRequest As EventHandler Implements IFormView(Of purchase_barrel, PurchaseBarrelVM).DeleteRequest
    Public Event CancelRequest As EventHandler Implements IFormView(Of purchase_barrel, PurchaseBarrelVM).CancelRequest
    Public Event SearchRequest As EventHandler Implements IFormView(Of purchase_barrel, PurchaseBarrelVM).SearchRequest
    Public Event PrintRequested As EventHandler(Of Object) Implements IPurchaseBarrelView.PrintRequested

    ' === 介面實作 ===
    Public Sub SetVendorCmb(data As List(Of SelectListItem)) Implements IPurchaseBarrelView.SetVendorCmb
        SetComboBox(cmbVendor, data)
    End Sub

    Public Sub SetCompanyCmb(data As List(Of SelectListItem)) Implements IPurchaseBarrelView.SetCompanyCmb
        SetComboBox(cmbCompany, data)
    End Sub

    Public Sub ShowList(data As List(Of PurchaseBarrelVM)) Implements IFormView(Of purchase_barrel, PurchaseBarrelVM).ShowList
        dgvPurchaseBarrel.DataSource = data
    End Sub

    Public Sub ShowDetail(data As purchase_barrel) Implements IFormView(Of purchase_barrel, PurchaseBarrelVM).ShowDetail
        AutoMapEntityToControls(data, Me)
        AutoMapEntityToControls(data, grpBarrel)
    End Sub

    Private Sub ClearInput() Implements IFormView(Of purchase_barrel, PurchaseBarrelVM).ClearInput
        ClearControls(Me, New List(Of String)({"grpBarrelInventory"}))
    End Sub

    Public Function GetSearchCriteria() As PurBarrelSC Implements IPurchaseBarrelView.GetSearchCriteria
        Return Nothing
    End Function

    Public Function GetInput(ByRef model As purchase_barrel) As Boolean Implements IFormView(Of purchase_barrel, PurchaseBarrelVM).GetInput
        AutoMapControlsToEntity(model, Me)
        AutoMapControlsToEntity(model, grpBarrel)
        Return True
    End Function

    Public Sub ButtonStatus(isSelectedRow As Boolean) Implements IFormView(Of purchase_barrel, PurchaseBarrelVM).ButtonStatus
        SetButtonState(Me, isSelectedRow)
    End Sub

    Public Sub SetBarrelInventory(data As GasBarrelDTO) Implements IPurchaseBarrelView.SetBarrelInventory
        data.GetType.GetProperties().ToList.ForEach(Sub(prop)
                                                        Dim txtBox = TryCast(grpBarrelInventory.Controls("txt" & prop.Name), TextBox)
                                                        If txtBox IsNot Nothing Then
                                                            txtBox.Text = prop.GetValue(data, Nothing).ToString()
                                                        End If
                                                    End Sub)
    End Sub

    ' === 控制項事件 ===
    Private Sub PurchaseBarrelUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        grpBarrel.Controls.OfType(Of TextBox).ToList.ForEach(Sub(x) AddHandler x.TextChanged, Sub(s, ev) CalculateAmount())
        RaiseEvent CancelRequest(Me, EventArgs.Empty)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        RaiseEvent CancelRequest(Me, EventArgs.Empty)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        RaiseEvent CreateRequest(Me, EventArgs.Empty)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        RaiseEvent UpdateRequest(Me, EventArgs.Empty)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If MsgBox("確認要刪除嗎?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            RaiseEvent DeleteRequest(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub dgvPurchaseBarrel_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPurchaseBarrel.SelectionChanged, dgvPurchaseBarrel.CellMouseClick
        If Not dgvPurchaseBarrel.Focused OrElse dgvPurchaseBarrel.SelectedRows.Count = 0 Then Return

        Dim row = dgvPurchaseBarrel.SelectedRows(0)
        Dim id As Integer = row.Cells("編號").Value

        RaiseEvent DataSelectedRequest(Me, id)
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        RaiseEvent PrintRequested(Me, dgvPurchaseBarrel.DataSource)
    End Sub

    ' === 方法 ===
    ''' <summary>
    ''' 計算總計
    ''' </summary>
    Private Sub CalculateAmount()
        Dim amount As Integer = 0
        Dim weights = grpBarrel.Controls.OfType(Of TextBox).
                                         Where(Function(txt) txt.Name.StartsWith("txtQty_")).
                                         Select(Function(txt) txt.Name.Substring(7)).
                                         ToList
        For Each weight In weights
            Dim qtyTxt = DirectCast(grpBarrel.Controls("txtQty_" & weight), TextBox)
            Dim unitPriceTxt = DirectCast(grpBarrel.Controls("txtUnitPrice_" & weight), TextBox)
            Dim qty As Integer
            Dim unitPrice As Integer

            If Integer.TryParse(qtyTxt.Text, qty) AndAlso Integer.TryParse(unitPriceTxt.Text, unitPrice) Then
                amount += qty * unitPrice
            End If
        Next

        txtAmount_pb.Text = amount
    End Sub
End Class
