Public Class ScrapBarrelUserControl
    Implements IScrapBarrelView, IScrapBarrelDetailView

    Private _sbPresenter As ScrapBarrelPresenter
    Private _sbdPresenter As ScrapBarrelDetailPresenter

    Public Sub New(sbPresenter As ScrapBarrelPresenter, sbdPresenter As ScrapBarrelDetailPresenter)
        InitializeComponent()
        _sbPresenter = sbPresenter
        _sbdPresenter = sbdPresenter
        _sbPresenter.SetView(Me)
        _sbdPresenter.SetView(Me)
    End Sub

    Private Sub ScrapBarrelUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel_sb.PerformClick()
        ReadDataGridWidth(dgvSB)
        ReadDataGridWidth(dgvSBD)
        TextChangedHandler()
    End Sub

    Private Sub TextChangedHandler()
        Dim scrapBarrelQtyTxts = New List(Of TextBox) From {txtQty50_sbd, txtQty20_sbd, txtQty16_sbd, txtQty10_sbd, txtQty4_sbd}
        scrapBarrelQtyTxts.ForEach(Sub(x) AddHandler x.TextChanged, Sub(sender, e) CalculateScrapBarrelDetail())
    End Sub

    ''' <summary>
    ''' 計算報廢桶客戶設定金額
    ''' </summary>
    Private Sub CalculateScrapBarrelDetail()
        Try
            ' 定義容量陣列
            Dim capacities = {"50", "20", "16", "10", "4"}

            For Each capacity In capacities
                ' 取得對應的控制項
                Dim qtyCtrl = TryCast(grpPrice_sbd.Controls($"txtQty{capacity}_sbd"), TextBox)
                Dim buyPriceCtrl = TryCast(grpPrice_sb.Controls($"txtBuy{capacity}"), TextBox)
                Dim acquisitionsPriceCtrl = TryCast(grpPrice_sb.Controls($"txtAcquisitions{capacity}"), TextBox)
                Dim buyResultCtrl = TryCast(grpPrice_sbd.Controls($"txtBuy{capacity}_sbd"), TextBox)
                Dim acquisitionsResultCtrl = TryCast(grpPrice_sbd.Controls($"txtAcquisitions{capacity}_sbd"), TextBox)

                If qtyCtrl IsNot Nothing AndAlso buyPriceCtrl IsNot Nothing AndAlso
                   acquisitionsPriceCtrl IsNot Nothing AndAlso buyResultCtrl IsNot Nothing AndAlso
                   acquisitionsResultCtrl IsNot Nothing Then

                    Dim qty As Integer = 0
                    Dim buyPrice As Decimal = 0
                    Dim acquisitionsPrice As Decimal = 0

                    Integer.TryParse(qtyCtrl.Text, qty)
                    Decimal.TryParse(buyPriceCtrl.Text, buyPrice)
                    Decimal.TryParse(acquisitionsPriceCtrl.Text, acquisitionsPrice)

                    ' 計算並設定結果
                    buyResultCtrl.Text = (buyPrice * qty).ToString()
                    acquisitionsResultCtrl.Text = (acquisitionsPrice * qty).ToString()
                End If
            Next
        Catch ex As Exception
            Console.WriteLine($"計算報廢桶金額時發生錯誤: {ex.Message}")
        End Try
    End Sub

    Public Sub ClearInput() Implements IScrapBarrelView.ClearInput
        ClearControls(grpSB)
    End Sub

    Public Sub ShowList(data As List(Of ScrapBarrelVM)) Implements IScrapBarrelView.ShowList
        dgvSB.DataSource = data
    End Sub

    Public Sub ShowList(data As List(Of ScrapBarrelDetailVM)) Implements IScrapBarrelDetailView.ShowList
        dgvSBD.DataSource = data
    End Sub

    Public Sub ShowDetail(data As scrap_barrel) Implements IScrapBarrelView.ShowDetail
        dtpSC.Value = data.sb_Month.Value
        txtAcquisitions50.Text = data.sb_Acquisitions50
        txtBuy50.Text = data.sb_Buy50
        txtAcquisitions20.Text = data.sb_Acquisitions20
        txtBuy20.Text = data.sb_Buy20
        txtAcquisitions16.Text = data.sb_Acquisitions16
        txtBuy16.Text = data.sb_Buy16
        txtAcquisitions10.Text = data.sb_Acquisitions10
        txtBuy10.Text = data.sb_Buy10
        txtAcquisitions4.Text = data.sb_Acquisitions4
        txtBuy4.Text = data.sb_Buy4
        txtSBId.Text = data.sb_Id
    End Sub

    Public Sub ShowDetail(data As scrap_barrel_detail) Implements IScrapBarrelDetailView.ShowDetail
        txtCusId_sbd.Text = data.sbd_cus_Id
        txtCusCode_sbd.Text = data.customer.cus_code
        txtCusName_sbd.Text = data.customer.cus_name
        txtQty50_sbd.Text = data.sbd_Qty50
        txtQty20_sbd.Text = data.sbd_Qty20
        txtQty16_sbd.Text = data.sbd_Qty16
        txtQty10_sbd.Text = data.sbd_Qty10
        txtQty4_sbd.Text = data.sbd_Qty4
        chkIsMonthlyStatement.Checked = data.sbd_isMonthlyStatement
    End Sub

    Private Sub IScrapBarrelDetailView_ClearInput() Implements IScrapBarrelDetailView.ClearInput
        ClearControls(grpSBD)
    End Sub

    Public Function GetInput() As scrap_barrel Implements IScrapBarrelView.GetInput
        Return New scrap_barrel With {
            .sb_Month = dtpSC.Value.Date,
            .sb_Acquisitions50 = If(String.IsNullOrEmpty(txtAcquisitions50.Text), 0, CInt(txtAcquisitions50.Text)),
            .sb_Buy50 = If(String.IsNullOrEmpty(txtBuy50.Text), 0, CInt(txtBuy50.Text)),
            .sb_Acquisitions20 = If(String.IsNullOrEmpty(txtAcquisitions20.Text), 0, CInt(txtAcquisitions20.Text)),
            .sb_Buy20 = If(String.IsNullOrEmpty(txtBuy20.Text), 0, CInt(txtBuy20.Text)),
            .sb_Acquisitions16 = If(String.IsNullOrEmpty(txtAcquisitions16.Text), 0, CInt(txtAcquisitions16.Text)),
            .sb_Buy16 = If(String.IsNullOrEmpty(txtBuy16.Text), 0, CInt(txtBuy16.Text)),
            .sb_Acquisitions10 = If(String.IsNullOrEmpty(txtAcquisitions10.Text), 0, CInt(txtAcquisitions10.Text)),
            .sb_Buy10 = If(String.IsNullOrEmpty(txtBuy10.Text), 0, CInt(txtBuy10.Text)),
            .sb_Acquisitions4 = If(String.IsNullOrEmpty(txtAcquisitions4.Text), 0, CInt(txtAcquisitions4.Text)),
            .sb_Buy4 = If(String.IsNullOrEmpty(txtBuy4.Text), 0, CInt(txtBuy4.Text))
        }
    End Function

    Private Function IScrapBarrelDetailView_GetInput() As scrap_barrel_detail Implements IScrapBarrelDetailView.GetInput
        If String.IsNullOrEmpty(txtCusId_sbd.Text) Then Throw New Exception("請選擇用戶")

        Return New scrap_barrel_detail With {
            .sbd_cus_Id = txtCusId_sbd.Text,
            .sbd_Qty50 = If(String.IsNullOrEmpty(txtQty50_sbd.Text), 0, CInt(txtQty50_sbd.Text)),
            .sbd_Qty20 = If(String.IsNullOrEmpty(txtQty20_sbd.Text), 0, CInt(txtQty20_sbd.Text)),
            .sbd_Qty16 = If(String.IsNullOrEmpty(txtQty16_sbd.Text), 0, CInt(txtQty16_sbd.Text)),
            .sbd_Qty10 = If(String.IsNullOrEmpty(txtQty10_sbd.Text), 0, CInt(txtQty10_sbd.Text)),
            .sbd_Qty4 = If(String.IsNullOrEmpty(txtQty4_sbd.Text), 0, CInt(txtQty4_sbd.Text)),
            .sbd_sb_Id = txtSBId.Text,
            .sbd_isMonthlyStatement = chkIsMonthlyStatement.Checked
        }
    End Function

    ' 月價格設定-取消
    Private Sub btnCancel_sb_Click(sender As Object, e As EventArgs) Handles btnCancel_sb.Click
        SetButtonState(sender, True)
        _sbPresenter.Reset()
        _sbdPresenter.Reset()
        grpSBD.Enabled = False
        btnCancel_sbd_Click(btnCancel_sbd, EventArgs.Empty)
    End Sub

    ' 月價格設定-新增
    Private Sub btnCreate_sb_Click(sender As Object, e As EventArgs) Handles btnCreate_sb.Click
        _sbPresenter.Add()
    End Sub

    ' 月價格設定-dgv-選擇
    Private Sub dgvSB_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSB.SelectionChanged, dgvSB.CellMouseClick
        Dim ctrl As DataGridView = sender

        If Not ctrl.Focused Or ctrl.SelectedRows.Count = 0 Then Return

        SetButtonState(ctrl, False)

        Dim id = ctrl.SelectedRows(0).Cells(0).Value
        _sbPresenter.LoadDetail(id)
        grpSBD.Enabled = True

        btnCancel_sbd_Click(btnCancel_sbd, EventArgs.Empty)
    End Sub

    ' 月價格設定-dgv-欄寬改變
    Private Sub dgvSB_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvSB.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub

    ' 月價格設定-修改
    Private Sub BtnEdit_sb_Click(sender As Object, e As EventArgs) Handles BtnEdit_sb.Click
        _sbPresenter.Update()
    End Sub

    ' 月價格設定-刪除
    Private Sub btnDelete_sb_Click(sender As Object, e As EventArgs) Handles btnDelete_sb.Click
        _sbPresenter.Delete()
    End Sub

    ' 月價格設定-列印
    Private Sub btnPrint_sb_Click(sender As Object, e As EventArgs) Handles btnPrint_sb.Click
        Dim sbId = dgvSB.SelectedRows(0).Cells(0).Value.ToString()
        _sbPresenter.Print(sbId)
    End Sub

    ' 客戶設定-取消
    Private Sub btnCancel_sbd_Click(sender As Object, e As EventArgs) Handles btnCancel_sbd.Click
        SetButtonState(sender, True)
        _sbdPresenter.Reset()
        dgvSBD.DataSource = Nothing

        Dim sbId = txtSBId.Text
        If Not String.IsNullOrEmpty(sbId) Then
            _sbdPresenter.LoadList(sbId)
        End If
    End Sub

    ' 客戶設定-新增
    Private Sub btnCreate_sbd_Click(sender As Object, e As EventArgs) Handles btnCreate_sbd.Click
        _sbdPresenter.Add()
        _sbdPresenter.LoadList(txtSBId.Text)
    End Sub

    ' 客戶設定-dgv-選擇
    Private Sub dgvSBD_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSBD.SelectionChanged, dgvSBD.CellMouseClick
        Dim ctrl As DataGridView = sender

        If Not ctrl.Focused Or ctrl.SelectedRows.Count = 0 Then Return

        SetButtonState(ctrl, False)

        Dim id = ctrl.SelectedRows(0).Cells(0).Value
        _sbdPresenter.LoadDetail(id)
    End Sub

    ' 客戶設定-dgv-欄寬改變
    Private Sub dgvSBD_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvSBD.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub

    ' 客戶設定-修改
    Private Sub btnEdit_sbd_Click(sender As Object, e As EventArgs) Handles btnEdit_sbd.Click
        _sbdPresenter.Update()
        _sbdPresenter.LoadList(txtSBId.Text)
    End Sub

    ' 客戶設定-刪除
    Private Sub btnDelete_sbd_Click(sender As Object, e As EventArgs) Handles btnDelete_sbd.Click
        _sbdPresenter.Delete()
        _sbdPresenter.LoadList(txtSBId.Text)
    End Sub

    ' 客戶設定-客戶代號
    Private Sub txtCusCode_sbd_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_sbd.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim cus = _sbdPresenter.GetCusByCusCode(txtCusCode_sbd.Text)
            If cus IsNot Nothing Then
                txtCusId_sbd.Text = cus.cus_id
                txtCusCode_sbd.Text = cus.cus_code
                txtCusName_sbd.Text = cus.cus_name
            Else
                MsgBox("找不到客戶")
            End If
        End If
    End Sub

    ' 客戶設定-搜尋客戶
    Private Sub btnSearchCus_sbd_Click(sender As Object, e As EventArgs) Handles btnSearchCus_sbd.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusId_sbd.Text = searchForm.CusId
                txtCusCode_sbd.Text = searchForm.CusCode
                txtCusName_sbd.Text = searchForm.CusName
            End If
        End Using
    End Sub
End Class
