Public Class CompanyUserControl
    Implements ICompanyView

    Private _presenter As CompanyPresenter

    Public Event Loaded As EventHandler Implements ICompanyView.Loaded
    Public Event CancelClicked As EventHandler Implements ICompanyView.CancelClicked
    Public Event RowSelected As EventHandler(Of Integer) Implements ICompanyView.RowSelected
    Public Event AddClicked As EventHandler Implements ICompanyView.AddClicked
    Public Event EditClicked As EventHandler Implements ICompanyView.EditClicked
    Public Event DeleteClicked As EventHandler Implements ICompanyView.DeleteClicked

    '=== View ===
    Public Sub DisplayList(data As List(Of CompanyVM)) Implements ICompanyView.DisplayList
        dgvCompany.DataSource = data
        SetColumnHeaders("company", dgvCompany)
    End Sub

    Public Sub DisplayDetail(data As company) Implements ICompanyView.DisplayDetail
        AutoMapEntityToControls(data, Me)
    End Sub

    Public Sub ClearInput() Implements ICompanyView.ClearInput
        ClearControls(Me)
    End Sub

    Public Function GetInput() As company Implements ICompanyView.GetInput
        If String.IsNullOrEmpty(txtName_comp.Text) Then Throw New Exception("請填寫名稱")
        If String.IsNullOrEmpty(txtShortName.Text) Then Throw New Exception("請填寫簡稱")
        If String.IsNullOrEmpty(txtTaxID.Text) Then Throw New Exception("請填寫統編")

        Dim data As New company
        AutoMapControlsToEntity(data, Me)
        Return data
    End Function

    ' === 控制項事件 ===
    ' 載入
    Private Sub CompanyUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _presenter = DependencyContainer.Resolve(Of CompanyPresenter)()
        _presenter.SetView(Me)
        RaiseEvent Loaded(Me, EventArgs.Empty)
        ReadDataGridWidth(dgvCompany)
    End Sub

    ' 取消
    Private Sub btnCancel_comp_Click(sender As Object, e As EventArgs) Handles btnCancel_comp.Click
        RaiseEvent CancelClicked(Me, EventArgs.Empty)
        SetButtonState(Me, False)
    End Sub

    ' 新增
    Private Sub btnAdd_comp_Click(sender As Object, e As EventArgs) Handles btnAdd_comp.Click
        RaiseEvent AddClicked(Me, EventArgs.Empty)
    End Sub

    ' dgv
    Private Sub dgvCompany_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCompany.SelectionChanged, dgvCompany.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return
        SetButtonState_old(ctrl, False)
        Dim id As Integer = ctrl.SelectedRows(0).Cells(0).Value
        RaiseEvent RowSelected(Me, id)
    End Sub

    ' 修改
    Private Sub btnEdit_comp_Click(sender As Object, e As EventArgs) Handles btnEdit_comp.Click
        RaiseEvent EditClicked(Me, EventArgs.Empty)
        SetButtonState_old(sender, True)
    End Sub

    ' 刪除
    Private Sub btnDelete_comp_Click(sender As Object, e As EventArgs) Handles btnDelete_comp.Click
        RaiseEvent DeleteClicked(Me, EventArgs.Empty)
        SetButtonState_old(sender, True)
    End Sub

    Private Sub dgvCompany_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvCompany.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub
End Class
