Public Class Cheque_colUserControl
    Implements ICheque

    Private _presenter As ChequePresenter

    Public Sub New(presenter As ChequePresenter)
        InitializeComponent()
        _presenter = presenter
        _presenter.SetView(Me)
    End Sub

    Public Sub ShowList(data As List(Of ChequeVM)) Implements ICommonView_old(Of cheque, ChequeVM).ShowList
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

    Public Sub SetDataToControl(data As cheque) Implements ICommonView_old(Of cheque, ChequeVM).SetDataToControl
        AutoMapEntityToControls(data, Me)
    End Sub

    Public Sub ClearInput() Implements ICommonView_old(Of cheque, ChequeVM).ClearInput
        ClearControls(Me)
    End Sub

    Public Function GetUserInput() As cheque Implements ICommonView_old(Of cheque, ChequeVM).GetUserInput
        Dim data As New cheque
        AutoMapControlsToEntity(data, Me)
        Return data
    End Function

    Public Function SetRequired() As List(Of Control) Implements ICommonView_old(Of cheque, ChequeVM).SetRequired
        Return Nothing
    End Function

    ' 取消
    Private Sub btnCancel_Che_Click(sender As Object, e As EventArgs) Handles btnCancel_Che.Click
        ClearControls(Me)
        _presenter.LoadList()
    End Sub

    ' dgv
    Private Sub dgvCheque_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCheque.SelectionChanged, dgvCheque.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Or ctrl.SelectedRows.Count = 0 Then Return

        SetButtonState(ctrl, False)

        Dim id = ctrl.SelectedRows(0).Cells(1).Value
        _presenter.SelectRow(id)
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
        Using frm As New Search_Cheque
            If frm.ShowDialog = DialogResult.OK Then
                _presenter.LoadList(frm.Criteria)
            End If
        End Using
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
        Dim selectedIds = GetSelectedChequeIds()
        If selectedIds.Count = 0 Then
            MsgBox("請至少選擇一個對象")
            Return
        End If
        _presenter.SetBatchStatus(selectedIds, dtpCollectionDate.Value.Date, True)
    End Sub

    ''' <summary>
    ''' 取得勾選的Id
    ''' </summary>
    ''' <returns></returns>
    Private Function GetSelectedChequeIds() As List(Of Integer)
        Return dgvCheque.Rows.Cast(Of DataGridViewRow).
                Where(Function(x) Convert.ToBoolean(x.Cells("ChkCol").Value)).
                Select(Function(x) CInt(x.Cells("編號").Value)).ToList
    End Function

    ' 列印
    Private Sub btnPrint_cheque_Click(sender As Object, e As EventArgs) Handles btnPrint_cheque.Click
        _presenter.Print(dgvCheque.DataSource)
    End Sub

    Private Sub Cheque_colUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel_Che.PerformClick()
    End Sub

    ' 轉為已兌現
    Private Sub btnRedeemed_Click(sender As Object, e As EventArgs) Handles btnRedeemed.Click
        Dim selectedIds = GetSelectedChequeIds()
        If selectedIds.Count = 0 Then
            MsgBox("請至少選擇一個對象")
            Return
        End If
        _presenter.SetBatchStatus(selectedIds, dtpCashingDate.Value.Date, False)
    End Sub
End Class
