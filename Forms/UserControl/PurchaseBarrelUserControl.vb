Public Class PurchaseBarrelUserControl
    Implements IPurchaseBarrelView

    ' 定義業務事件
    Public Event AddRequested As EventHandler Implements IPurchaseBarrelView.AddRequested
    Public Event UpdateRequested As EventHandler Implements IPurchaseBarrelView.UpdateRequested
    Public Event DeleteRequested As EventHandler Implements IPurchaseBarrelView.DeleteRequested
    Public Event CancelRequested As EventHandler Implements IPurchaseBarrelView.CancelRequested
    Public Event DetailRequested As EventHandler(Of Integer) Implements IPurchaseBarrelView.DetailRequested
    Public Event PrintRequested As EventHandler(Of Object) Implements IPurchaseBarrelView.PrintRequested

    Public Sub SetVendorCmb(data As List(Of SelectListItem)) Implements IPurchaseBarrelView.SetVendorCmb
        SetComboBox(cmbVendor, data)
    End Sub

    Public Sub SetCompanyCmb(data As List(Of SelectListItem)) Implements IPurchaseBarrelView.SetCompanyCmb
        SetComboBox(cmbCompany, data)
    End Sub

    Public Sub DisplayList(data As List(Of PurchaseBarrelVM)) Implements IBaseView(Of purchase_barrel, PurchaseBarrelVM).DisplayList
        dgvPurchaseBarrel.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As purchase_barrel) Implements IBaseView(Of purchase_barrel, PurchaseBarrelVM).DisplayDetail
        AutoMapEntityToControls(data, Me)
        AutoMapEntityToControls(data, grpBarrel)
    End Sub

    Public Sub ClearInput() Implements IBaseView(Of purchase_barrel, PurchaseBarrelVM).ClearInput
        ClearControls(Me)
    End Sub

    Public Function GetSearchCriteria() As PurBarrelSC Implements IPurchaseBarrelView.GetSearchCriteria
        Return Nothing
    End Function

    Public Function GetUserInput() As purchase_barrel Implements IBaseView(Of purchase_barrel, PurchaseBarrelVM).GetUserInput
        Dim data As New purchase_barrel
        AutoMapControlsToEntity(data, Me)
        AutoMapControlsToEntity(data, grpBarrel)
        Return data
    End Function

    Public Sub SetButton(isSelectedRow As Boolean) Implements IPurchaseBarrelView.SetButton
        SetButtonState(Me, isSelectedRow)
    End Sub

    Private Sub PurchaseBarrelUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        grpBarrel.Controls.OfType(Of TextBox).ToList.ForEach(Sub(x) AddHandler x.TextChanged, Sub(s, ev) CalculateAmount())
        RaiseEvent CancelRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        RaiseEvent CancelRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        RaiseEvent AddRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        RaiseEvent UpdateRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If MsgBox("確認要刪除嗎?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            RaiseEvent DeleteRequested(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub dgvPurchaseBarrel_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPurchaseBarrel.SelectionChanged, dgvPurchaseBarrel.CellMouseClick
        If Not dgvPurchaseBarrel.Focused OrElse dgvPurchaseBarrel.SelectedRows.Count = 0 Then Return

        Dim row = dgvPurchaseBarrel.SelectedRows(0)
        Dim id As Integer = row.Cells("編號").Value

        RaiseEvent DetailRequested(Me, id)
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        RaiseEvent PrintRequested(Me, dgvPurchaseBarrel.DataSource)
    End Sub

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
