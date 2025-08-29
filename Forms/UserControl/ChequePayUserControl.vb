Public Class ChequePayUserControl
	Implements IChequePayView

	Private _presenter As ChequePayPresenter

	' IChequePayView 事件
	Public Event Loaded As EventHandler Implements IChequePayView.Loaded
	Public Event SearchClicked As EventHandler Implements IChequePayView.SearchClicked
	Public Event CancelClicked As EventHandler Implements IChequePayView.CancelClicked
	Public Event RowSelected As EventHandler(Of Integer) Implements IChequePayView.RowSelected
	Public Event PrintClicked As EventHandler Implements IChequePayView.PrintClicked

	' IChequePayView 方法
	Public Sub DisplayList(data As List(Of ChequePayVM)) Implements IChequePayView.DisplayList
		dgvCheque.DataSource = data
	End Sub

	Public Sub DisplayDetail(data As chque_pay) Implements IChequePayView.DisplayDetail
		If data Is Nothing Then Exit Sub

		txtCheId.Text = data.cp_Id.ToString()
		dtpDate.Value = If(data.cp_Date, Date.Today)
		txtNumber.Text = data.cp_Number
		txtAmount.Text = If(data.cp_Amount.HasValue, data.cp_Amount.Value.ToString(), String.Empty)
		dtpCashingDate.Value = If(data.cp_CashingDate, Date.Today)
		chkIsCashing.Checked = data.cp_IsCashing.HasValue AndAlso data.cp_IsCashing.Value
	End Sub

	Public Sub ClearInput() Implements IChequePayView.ClearInput
		txtCheId.Clear()
		txtNumber.Clear()
		txtAmount.Clear()
		dtpDate.Value = Date.Today
		dtpCashingDate.Value = Date.Today
		chkIsCashing.Checked = False
	End Sub

	Public Function GetDGVData() As List(Of ChequePayVM) Implements IChequePayView.GetDGVData
		Return dgvCheque.DataSource
	End Function

	' UI 事件 → View 事件
	Private Sub ChequePayUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _presenter = DependencyContainer.Resolve(Of ChequePayPresenter)()
        _presenter.SetView(Me)
		RaiseEvent Loaded(Me, EventArgs.Empty)
	End Sub

	Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
		RaiseEvent SearchClicked(Me, EventArgs.Empty)
	End Sub

	Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
		RaiseEvent CancelClicked(Me, EventArgs.Empty)
	End Sub

    Private Sub dgvCheque_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCheque.SelectionChanged, dgvCheque.CellMouseClick
        If Not dgvCheque.Focused Then Exit Sub
        If dgvCheque.CurrentRow Is Nothing OrElse dgvCheque.CurrentRow.DataBoundItem Is Nothing Then Exit Sub
		Dim vm = TryCast(dgvCheque.CurrentRow.DataBoundItem, ChequePayVM)
		If vm IsNot Nothing Then
			RaiseEvent RowSelected(Me, vm.編號)
		End If
	End Sub

	Public Function GetSearchCriteria() As ChequeSC Implements IChequePayView.GetSearchCriteria
		Using frm As New Search_Cheque
            If frm.ShowDialog() = DialogResult.OK Then
				Return frm.Criteria
			Else
                Return Nothing
            End If
        End Using
	End Function

	Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
		RaiseEvent PrintClicked(Me, EventArgs.Empty)
	End Sub
End Class