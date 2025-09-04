Public Class CollectionUserControl
    Implements ICollectionView

    Private _presenter As CollectionPresenter

    Public Sub New(presenter As CollectionPresenter)
        InitializeComponent()
        _presenter = presenter
        _presenter.SetView(Me)
    End Sub

    Private Sub CollectionUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel_col.PerformClick()
        ReadDataGridWidth(dgvCollection)
    End Sub

    Public Sub SetSubjectCmb(datas As List(Of SelectListItem)) Implements ICollectionView.SetSubjectCmb
        SetComboBox(cmbSubjects, datas)
    End Sub

    Public Sub SetCompanyCmb(datas As List(Of SelectListItem)) Implements ICollectionView.SetCompanyCmb
        SetComboBox(cmbCompany_col, datas)
    End Sub

    Public Sub SetBankCmb(datas As List(Of SelectListItem)) Implements ICollectionView.SetBankCmb
        SetComboBox(cmbBank_col, datas)
    End Sub

    Public Sub DisplayList(data As List(Of CollectionVM)) Implements IBaseView(Of collection, CollectionVM).DisplayList
        dgvCollection.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As collection) Implements IBaseView(Of collection, CollectionVM).DisplayDetail
        AutoMapEntityToControls(data, Me)
        AutoMapEntityToControls(data.customer, Me)
        AutoMapEntityToControls(data.cheques, Me)
    End Sub

    Public Sub ClearInput() Implements IBaseView(Of collection, CollectionVM).ClearInput
        ClearControls(Me)
    End Sub

    Public Function GetChequeInput() As cheque Implements ICollectionView.GetChequeInput
        Dim data As New cheque
        AutoMapControlsToEntity(data, Me)
        data.che_Amount = txtAmount_collection.Text
        data.che_ReceivedDate = dtpDate_col.Value.Date
        data.che_col_Id = If(String.IsNullOrEmpty(txtColId.Text), Nothing, txtColId.Text)
        data.che_Number = txtCheque_col.Text
        Return data
    End Function

    Public Function GetUserInput() As collection Implements IBaseView(Of collection, CollectionVM).GetUserInput
        Dim data As New collection
        AutoMapControlsToEntity(data, Me)
        Return data
    End Function

    ' 取消
    Private Sub btnCancel_col_Click(sender As Object, e As EventArgs) Handles btnCancel_col.Click
        Try
            _presenter.Initialize()
            SetButtonState(btnCancel_col, True)

            ' 公司預設 "豐合"
            Dim index As Integer = cmbCompany_col.FindString("豐合")
            If index >= 0 Then cmbCompany_col.SelectedIndex = index

            ' 解鎖"收款類型"
            cmbType_col.Enabled = True

            dtpDate_col.Value = Now.Date
            DateTimePicker14.Value = Now.Date
        Catch ex As Exception
            MsgBox(ex.StackTrace)
        End Try
    End Sub

    ' 客戶代號
    Private Sub txtCusCode_col_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_col.KeyDown
        '按下Enter時,搜尋客戶資料
        If e.KeyCode = Keys.Enter Then
            Dim cus = _presenter.GetCustomer(txtCusCode_col.Text)

            If cus IsNot Nothing Then
                txtCusCode_col.Text = cus.cus_code
                txtCusName_col.Text = cus.cus_name
                txtCusId_col.Text = cus.cus_id
            Else
                MsgBox("查無此客戶")
            End If
        End If
    End Sub

    ' 搜尋客戶
    Private Sub btnQueryCus_col_Click(sender As Object, e As EventArgs) Handles btnQueryCus_col.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusId_col.Text = searchForm.CusId
                txtCusName_col.Text = searchForm.CusName
                txtCusCode_col.Text = searchForm.CusCode
            End If
        End Using
    End Sub

    ' 借方科目
    Private Sub cmbType_col_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbType_col.SelectedIndexChanged
        Dim cmb As ComboBox = sender
        Dim collectionType As String = cmb.Text
        Dim ctrlVisibility As New Dictionary(Of String, Control()) From {
            {"應收票據", {lblChequeReq_col, lblCheque_col, txtCheque_col, lblCashingDate_col, txtCashingDate, lblIssuerNameReq, lblIssuerName, txtIssuerName, lblChequeAccountNumberReq,
                      lblChequeAccountNumber, txtCheAcctNum, lblAbleCashingDate, dtpAbleCashingDate, lblPayBank, txtPayBank}}
        }

        For Each kvp In ctrlVisibility
            Dim isVisible As Boolean = (collectionType = kvp.Key)
            For Each ctrl In kvp.Value
                ctrl.Visible = isVisible
            Next
        Next
    End Sub

    ' 新增
    Private Sub btnAdd_col_Click(sender As Object, e As EventArgs) Handles btnAdd_col.Click
        _presenter.Add()
    End Sub

    ' dgv-選擇
    Private Sub dgvCollection_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCollection.SelectionChanged, dgvCollection.CellMouseClick
        Dim id = DGV_SelectionChanged(sender)
        If id > 0 Then
            _presenter.LoadDetail(id)
            cmbType_col.Enabled = False
        End If
    End Sub

    ' dgv-欄寬改變
    Private Sub dgvCollection_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvCollection.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub

    ' 修改
    Private Sub btnEdit_col_Click(sender As Object, e As EventArgs) Handles btnEdit_col.Click
        _presenter.Edit()
    End Sub

    ' 刪除
    Private Sub btnDelete_col_Click(sender As Object, e As EventArgs) Handles btnDelete_col.Click
        _presenter.DeleteAsync()
    End Sub

    ' 銷帳
    Private Sub btnWriteOff_Click(sender As Object, e As EventArgs) Handles btnWriteOff.Click
        If String.IsNullOrEmpty(txtColId.Text) Then
            MsgBox("請選擇對象")
            Return
        End If

        Using frm As New frmWriteOff(txtColId.Text)
            If frm.ShowDialog = DialogResult.OK Then
            End If
        End Using
    End Sub

    ' 列印
    Private Sub btnPrint_Col_Click(sender As Object, e As EventArgs) Handles btnPrint_Col.Click
        Using frm As New Print_Subpoena
            If frm.ShowDialog = DialogResult.OK Then
                _presenter.Print(frm.SelectDate, frm.Type)
            End If
        End Using
    End Sub

    ' 查詢
    Private Sub btnQuery_col_Click(sender As Object, e As EventArgs) Handles btnQuery_col.Click
        Dim db As New gas_accounting_systemEntities
        Using frm As New frmSearch_Collection(New SubjectRep(db), New CustomerRep(db))
            If frm.ShowDialog = DialogResult.OK Then
                _presenter.LoadList(frm.Criteria)
            End If
        End Using
    End Sub
End Class
