Public Class frmQueryCustomer
    Implements IQueryCusView

    Private _presenter As New QueryCusPresenter(Me, New CustomerRepository)

    Public ReadOnly Property CusCode As String
        Get
            Return dgvCustomer.SelectedRows(0).Cells("代號").Value
        End Get
    End Property

    Public ReadOnly Property CusName As String
        Get
            Return dgvCustomer.SelectedRows(0).Cells("名稱").Value
        End Get
    End Property

    Public ReadOnly Property CusId As Integer
        Get
            Return dgvCustomer.SelectedRows(0).Cells("編號").Value
        End Get
    End Property

    Public Sub ShowList(customers As List(Of QueryCusVM)) Implements IQueryCusView.ShowList
        dgvCustomer.DataSource = customers
    End Sub

    Public Sub ShowDetails(data As customer) Implements IQueryCusView.ShowDetails
        AutoMapEntityToControls(data, Me)
    End Sub

    Public Sub Reset() Implements IQueryCusView.Reset
        ClearControls(Me)
    End Sub

    Private Sub frmQueryCustomer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetDataGridViewStyle(Me)
        Dim lst = New List(Of DataGridView) From {dgvCustomer}
        ReadDataGridWidth(lst)
        _presenter.LoadList()
    End Sub

    Private Sub btnQuery_Click(sender As Object, e As EventArgs) Handles btnQuery.Click
        _presenter.LoadList()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If dgvCustomer.SelectedRows.Count > 0 Then
            DialogResult = DialogResult.OK
            Close()
        Else
            DialogResult = DialogResult.None
        End If
    End Sub

    Private Sub dgvCustomer_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvCustomer.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub

    Private Sub dgvCustomer_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCustomer.CellMouseClick, dgvCustomer.SelectionChanged
        Dim dgv = CType(sender, DataGridView)
        If Not dgv.Focused Then Exit Sub

        Dim selectRow = dgv.SelectedRows(0)
        Dim id As Integer = selectRow.Cells("編號").Value

        _presenter.LoadDetails(id)
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        _presenter.Reset()
    End Sub

    Public Function GetSearchCondition() As customer Implements IQueryCusView.GetSearchCondition

        Return New customer With {
            .cus_code = txtCusCode.Text,
            .cus_name = txtCusName.Text,
            .cus_contact_person = txtCusContactPerson.Text,
            .cus_phone1 = txtCusPhone.Text
        }
    End Function
End Class