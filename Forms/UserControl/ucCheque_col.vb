''' <summary>
''' 會計管理-應收支票管理
''' </summary>
Public Class ucCheque_col
    Implements ICheque

    Public Event CreateRequest As EventHandler Implements IFormView(Of cheque, ChequeVM).CreateRequest
    Public Event DataSelectedRequest As EventHandler(Of Integer) Implements IFormView(Of cheque, ChequeVM).DataSelectedRequest
    Public Event UpdateRequest As EventHandler Implements IFormView(Of cheque, ChequeVM).UpdateRequest
    Public Event DeleteRequest As EventHandler Implements IFormView(Of cheque, ChequeVM).DeleteRequest
    Public Event CancelRequest As EventHandler Implements IFormView(Of cheque, ChequeVM).CancelRequest
    Public Event SearchRequest As EventHandler Implements IFormView(Of cheque, ChequeVM).SearchRequest
    Public Event SetBatchStatusRequest As EventHandler(Of Boolean) Implements ICheque.SetBatchStatusRequest
    Public Event PrintRequest As EventHandler(Of List(Of ChequeVM)) Implements ICheque.PrintRequest

    ' === 介面 ===
    Private Sub ShowList(data As List(Of ChequeVM)) Implements IFormView(Of cheque, ChequeVM).ShowList
        '檢查是否有chk
        Dim chkColName = "ChkCol"

        If Not dgvCheque.Columns.Cast(Of DataGridViewColumn).Any(Function(x) x.Name = chkColName) Then
            Dim chkCol As New DataGridViewCheckBoxColumn With {
                .HeaderText = "選擇",
                .Name = chkColName,
                .Width = 50
            }

            dgvCheque.Columns.Insert(0, chkCol)
        End If

        dgvCheque.DataSource = data

        ' 設置列的唯讀屬性
        For Each column As DataGridViewColumn In dgvCheque.Columns
            If column.Name = chkColName Then
                column.ReadOnly = False
            Else
                column.ReadOnly = True
            End If
        Next
    End Sub

    Public Sub ShowDetail(data As cheque) Implements IFormView(Of cheque, ChequeVM).ShowDetail
        AutoMapEntityToControls(data, Me)
    End Sub

    Private Sub ClearInput() Implements IFormView(Of cheque, ChequeVM).ClearInput
        ClearControls(Me)
    End Sub

    Public Function GetInput(ByRef model As cheque) As Boolean Implements IFormView(Of cheque, ChequeVM).GetInput
        AutoMapControlsToEntity(model, Me)
        Return True
    End Function

    Public Sub ButtonStatus(isSelectedRow As Boolean) Implements IFormView(Of cheque, ChequeVM).ButtonStatus
        SetButtonState(Me, isSelectedRow)
    End Sub

    Public Function GetSearchCriteria() As ChequeSC Implements ICheque.GetSearchCriteria
        Using frm As New Search_Cheque
            If frm.ShowDialog = DialogResult.OK Then
                Return frm.Criteria
            Else
                Return Nothing
            End If
        End Using
    End Function

    Public Function GetSelectedIds() As List(Of Integer) Implements ICheque.GetSelectedIds
        Return dgvCheque.Rows.Cast(Of DataGridViewRow).
                Where(Function(x) Convert.ToBoolean(x.Cells("ChkCol").Value)).
                Select(Function(x) CInt(x.Cells("編號").Value)).ToList
    End Function

    ' === 事件 ===
    ' 取消
    Private Sub btnCancel_Che_Click(sender As Object, e As EventArgs) Handles btnCancel_Che.Click
        RaiseEvent CancelRequest(sender, EventArgs.Empty)
    End Sub

    ' dgv
    Private Sub dgvCheque_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCheque.SelectionChanged, dgvCheque.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Or ctrl.SelectedRows.Count = 0 Then Return
        Dim id = ctrl.SelectedRows(0).Cells(1).Value
        RaiseEvent DataSelectedRequest(sender, id)
    End Sub

    ' 狀態
    Private Sub cmbState_Che_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbState_Che.SelectedIndexChanged
        Dim cmb As ComboBox = sender
        Dim ctrls As Control() = {lblCashingDate, dtpCashingDate}
        ctrls.ToList.ForEach(Sub(x) x.Visible = False)

        Select Case cmb.Text
            Case "已兌現"
                lblCashingDate.Visible = True
                dtpCashingDate.Visible = True
        End Select
    End Sub

    ' 查詢
    Private Sub btnQuery_che_Click(sender As Object, e As EventArgs) Handles btnQuery_che.Click
        RaiseEvent SearchRequest(sender, e)
    End Sub

    ' 全選
    Private Sub btnSelectAll_Click(sender As Object, e As EventArgs) Handles btnSelectAll.Click
        For Each row As DataGridViewRow In dgvCheque.Rows
            If Not row.IsNewRow Then
                row.Cells("ChkCol").Value = True
            End If
        Next
    End Sub

    ' 轉為已代收
    Private Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        RaiseEvent SetBatchStatusRequest(sender, True)
    End Sub

    ' 轉為已兌現
    Private Sub btnRedeemed_Click(sender As Object, e As EventArgs) Handles btnRedeemed.Click
        RaiseEvent SetBatchStatusRequest(sender, False)
    End Sub

    ' 列印
    Private Sub btnPrint_cheque_Click(sender As Object, e As EventArgs) Handles btnPrint_cheque.Click
        Cursor = Cursors.WaitCursor
        RaiseEvent PrintRequest(sender, dgvCheque.DataSource)
        Cursor = Cursors.Default
    End Sub

    Private Sub Cheque_colUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel_Che.PerformClick()
        ReadDataGridWidth(dgvCheque)
    End Sub
End Class
