
Public Class OrderUserControl
    Implements IOrderView

    Private _presenter As OrderPresenter

    Public Sub New(presenter As OrderPresenter)
        InitializeComponent()
        _presenter = presenter
        _presenter.SetView(Me)
    End Sub

    Private Sub Order_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel_order.PerformClick()
        SetupSalesManagementHandlers()
        tcInOut.DrawMode = TabDrawMode.OwnerDrawFixed
        AddHandler tcInOut.DrawItem, AddressOf TabControl_DrawItem
        ReadDataGridWidth(dgvOrder)
    End Sub

    Public Sub DisplayCustomer(data As customer) Implements IOrderView.DisplayCustomer
        AutoMapEntityToControls(data, Me)
        txtCusID_order.Text = data.cus_id
        AutoMapEntityToControls(data, tpOut)
    End Sub

    Public Sub DisplayCusStk(data As customer, isIn As Boolean) Implements IOrderView.DisplayCusStk
        Dim tp = If(isIn, tpIn, tpOut)
        AutoMapEntityToControls(data, tp)
    End Sub

    Public Sub DisplayCarStk(data As car, isIn As Boolean) Implements IOrderView.DisplayCarStk
        Dim tp = If(isIn, tpIn, tpOut)
        AutoMapEntityToControls(data, tp)
    End Sub

    Public Sub DisplayGasAndPrice(gas As Integer, gasC As Integer, amount As Single, insurance As Single, barrelAmount As Integer, gasUnitPrice As Single, gasCUnitPrice As Single, unpaidAmount As Integer) Implements IOrderView.DisplayGasAndPrice
        txtTotalGas.Text = gas
        txtTotalGas_c.Text = gasC
        txtAmount_ord.Text = amount
        txtInsurance.Text = insurance
        txtBarrelAmount.Text = barrelAmount
        txtGasUnitPrice.Text = gasUnitPrice
        txtGasCUnitPrice.Text = gasCUnitPrice
        txtUnpaid.Text = unpaidAmount
    End Sub

    Public Sub DisplayInsurance(price As Single) Implements IOrderView.DisplayInsurance
        txtInsurance.Text = price
    End Sub

    Public Sub GetCusStkInput(currentEntity As customer) Implements IOrderView.GetCusStkInput
        AutoMapControlsToEntity(currentEntity, tcInOut.SelectedTab)
    End Sub

    Public Sub GetCarStkInput(currentEntity As car) Implements IOrderView.GetCarStkInput
        AutoMapControlsToEntity(currentEntity, tcInOut.SelectedTab)
    End Sub

    Public Sub SetCarDropdown(list As List(Of SelectListItem)) Implements IOrderView.SetCarDropdown
        SetComboBox(cmbCar_ord, New List(Of SelectListItem)(list))
        SetComboBox(cmbCarOut_ord, New List(Of SelectListItem)(list))
    End Sub

    Public Sub DisplayList(data As List(Of OrderVM)) Implements IBaseView(Of order, OrderVM).DisplayList
        dgvOrder.DataSource = data
        AddHandler dgvOrder.CellFormatting, AddressOf DgvOrder_CellFormatting
    End Sub

    Public Sub DisplayDetail(data As order) Implements IBaseView(Of order, OrderVM).DisplayDetail
        AutoMapEntityToControls(data.customer, Me)
        _presenter.LoadCar()
        AutoMapEntityToControls(data, Me)
        txtOperator.Text = data.employee?.emp_name

        If data.o_in_out = "進場單" Then
            AutoMapEntityToControls(data, tpIn)
            tpOut.Parent = Nothing
            tpIn.Parent = tcInOut
        Else
            AutoMapEntityToControls(data, tpOut)
            tpOut.Parent = tcInOut
            tpIn.Parent = Nothing
        End If
    End Sub

    Public Sub ClearInput() Implements IBaseView(Of order, OrderVM).ClearInput
        ClearControls(Me)
        ClearControls(tpIn)
        ClearControls(tpOut)

        tpOut.Parent = tcInOut
        tpIn.Parent = tcInOut
        txtOperator.Text = Nothing
        cmbCar_ord.DataSource = Nothing
        cmbCarOut_ord.DataSource = Nothing

        SetButtonState(btnCancel_order, True)

        '預設運送方式
        grpTransport.Controls.OfType(Of RadioButton).First(Function(x) x.Text = "自運").Checked = True

        txtCusCode_ord.ReadOnly = False
        btnQueryCus_ord.Enabled = True
        cmbCar_ord.Enabled = True
        cmbCarOut_ord.Enabled = True
        grpTransport.Enabled = True
    End Sub

    Public Function GetInInput() As order Implements IOrderView.GetInInput
        Dim data As New order
        AutoMapControlsToEntity(data, tpIn)
        Return data
    End Function

    Public Function GetOutInput() As order Implements IOrderView.GetOutInput
        Dim data As New order
        AutoMapControlsToEntity(data, tpOut)
        Return data
    End Function

    Public Function GetOrderInput() As order Implements IOrderView.GetOrderInput
        Dim data As New order
        AutoMapControlsToEntity(data, Me)
        Return data
    End Function

    Public Function GetSearchCriteria() As OrderSearchCriteria Implements IOrderView.GetSearchCriteria
        Dim cusId As Integer = If(String.IsNullOrEmpty(txtCusID_order.Text), 0, txtCusID_order.Text)
        Dim data As New OrderSearchCriteria With {
            .CusId = cusId,
            .EndDate = dtpEnd_order.Value,
            .IsDate = chkIsDate_ord.Checked,
            .StartDate = dtpStart_order.Value,
            .SearchIn = chkIn.Checked,
            .SearchOut = chkOut.Checked
        }

        Return data
    End Function

    Public Function GetUserInput() As order Implements IBaseView(Of order, OrderVM).GetUserInput
        Dim data As New order

        AutoMapControlsToEntity(data, Me)
        AutoMapControlsToEntity(data, tcInOut.SelectedTab)
        data.o_Operator = Nothing
        Return data
    End Function

    ''' <summary>
    ''' 丙氣的字體顏色設置為紅色
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DgvOrder_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim columnName = dgvOrder.Columns(e.ColumnIndex).Name
            If columnName.Contains("丙氣") Then
                e.CellStyle.ForeColor = Color.Red
            End If
        End If
    End Sub

    ''' <summary>
    ''' 控制項事件設定
    ''' </summary>
    Private Sub SetupSalesManagementHandlers()
        Try
            ' 進場單處理
            SetupTextBoxHandlers(tpIn, AddressOf CalculateOrder, "txto_in", "txto_new_in", "txto_inspect", "txtBarralUnitPrice")
            SetupTextBoxHandlers(tpIn, AddressOf CalculateCarStock, "txtDepositIn")

            ' 為所有非只讀的TextBox添加方向鍵處理
            AddDirectionHandlers(tpIn)

            ' 出場單處理
            SetupTextBoxHandlers(tpOut, AddressOf CalculateOrder, "txtGas", "txtEmpty")
            SetupTextBoxHandlers(tpOut, AddressOf CalculateCarStock, "txtDepositOut")

            AddDirectionHandlers(tpOut)

            ' 訂單處理
            SetupTextBoxHandlers(Me, AddressOf CalculateOrder, "txto_return", "txto_return_c", "txto_sales_allowance")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' 設定TextBox事件
    ''' </summary>
    ''' <param name="container"></param>
    ''' <param name="handler"></param>
    ''' <param name="prefixes"></param>
    Private Sub SetupTextBoxHandlers(container As Control, handler As KeyEventHandler, ParamArray prefixes As String())
        For Each txt In container.Controls.OfType(Of TextBox)()
            If prefixes.Any(Function(prefix) txt.Name.StartsWith(prefix)) Then
                AddHandler txt.KeyUp, handler
            End If
        Next
    End Sub

    ''' <summary>
    ''' 設定方向事件
    ''' </summary>
    ''' <param name="container"></param>
    Private Sub AddDirectionHandlers(container As Control)
        For Each txt In container.Controls.OfType(Of TextBox)().Where(Function(x) Not x.ReadOnly)
            AddHandler txt.KeyDown, Sub(sender, e) Direction(sender, e, container)
        Next
    End Sub

    ''' <summary>
    ''' 計算進出場單
    ''' </summary>
    Private Sub CalculateOrder()
        Try
            Dim isIn As Boolean = tcInOut.SelectedTab.Text = "進場單"
            _presenter.CalculateStkAndPrice(isIn)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' 計算車寄桶存量
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CalculateCarStock(sender As Object, e As KeyEventArgs)
        Dim btn As TextBox = sender
        Dim isIn As Boolean = btn.Parent.Name = "tpIn"
        _presenter.CalculateCarStk(isIn)
    End Sub

    ''' <summary>
    ''' 方向鍵處理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <param name="container"></param>
    Private Sub Direction(sender As Object, e As KeyEventArgs, container As Control)
        Dim btn As TextBox = sender
        Dim txtName = btn.Name
        Dim currentI As Integer
        Dim currentJ As Integer
        Dim nextI As Integer
        Dim nextJ As Integer
        Dim inputTxts As String(,) = {
        {"txto_in_50", "txto_in_20", "txto_in_16", "txto_in_10", "txto_in_4", "txto_in_18", "txto_in_14", "txto_in_5", "txto_in_2"},
        {"txto_new_in_50", "txto_new_in_20", "txto_new_in_16", "txto_new_in_10", "txto_new_in_4", "txto_new_in_18", "txto_new_in_14", "txto_new_in_5", "txto_new_in_2"},
        {"txtBarralUnitPrice_50", "txtBarralUnitPrice_20", "txtBarralUnitPrice_16", "txtBarralUnitPrice_10", "txtBarralUnitPrice_4", "txtBarralUnitPrice_18", "txtBarralUnitPrice_14", "txtBarralUnitPrice_5", "txtBarralUnitPrice_2"},
        {"txto_inspect_50", "txto_inspect_20", "txto_inspect_16", "txto_inspect_10", "txto_inspect_4", "txto_inspect_18", "txto_inspect_14", "txto_inspect_5", "txto_inspect_2"},
        {"txtDepositIn_50", "txtDepositIn_20", "txtDepositIn_16", "txtDepositIn_10", "txtDepositIn_4", "txtDepositIn_18", "txtDepositIn_14", "txtDepositIn_5", "txtDepositIn_2"}
    }
        Dim outputTxts As String(,) = {
        {"txtGas_c_50", "txtGas_c_20", "txtGas_c_16", "txtGas_c_10", "txtGas_c_4", "txtGas_c_18", "txtGas_c_14", "txtGas_c_5", "txtGas_c_2"},
        {"txtGas_50", "txtGas_20", "txtGas_16", "txtGas_10", "txtGas_4", "txtGas_18", "txtGas_14", "txtGas_5", "txtGas_2"},
        {"txtEmpty_50", "txtEmpty_20", "txtEmpty_16", "txtEmpty_10", "txtEmpty_4", "txtEmpty_18", "txtEmpty_14", "txtEmpty_5", "txtEmpty_2"},
        {"txtDepositOut_50", "txtDepositOut_20", "txtDepositOut_16", "txtDepositOut_10", "txtDepositOut_4", "txtDepositOut_18", "txtDepositOut_14", "txtDepositOut_5", "txtDepositOut_2"}
    }
        Dim arr = If(container.Text = "進場單", inputTxts, outputTxts)

        For i As Integer = 0 To arr.GetLength(0) - 1
            For j As Integer = 0 To arr.GetLength(1) - 1
                If txtName = arr(i, j) Then
                    currentI = i
                    currentJ = j
                End If
            Next
        Next

        nextI = currentI
        nextJ = currentJ

        Select Case e.KeyCode
            Case Keys.Right, Keys.Enter
                If currentJ < arr.GetLength(1) - 1 Then
                    nextJ = currentJ + 1
                ElseIf currentI = arr.GetLength(0) - 1 And currentJ = arr.GetLength(1) - 1 Then
                    nextI = 0
                    nextJ = 0
                Else
                    nextI = currentI + 1
                    nextJ = 0
                End If

            Case Keys.Left
                If currentJ <> 0 Then
                    nextJ = currentJ - 1
                ElseIf currentI = 0 And currentJ = 0 Then
                    nextI = arr.GetLength(0) - 1
                    nextJ = arr.GetLength(1) - 1
                Else
                    nextI = currentI - 1
                    nextJ = arr.GetLength(1) - 1
                End If

            Case Keys.Up
                nextI = If(currentI = 0, arr.GetLength(0) - 1, currentI - 1)

            Case Keys.Down
                nextI = If(currentI = arr.GetLength(0) - 1, 0, currentI + 1)

            Case Else
                Exit Sub
        End Select

        Dim txt As TextBox = container.Controls.OfType(Of TextBox).FirstOrDefault(Function(x) x.Name = arr(nextI, nextJ))
        txt.Focus()
        txt.SelectAll()
    End Sub

    Private Sub GetDetail(id As Integer)
        _presenter.LoadDetail(id)
        SetButtonState(dgvOrder, False)
        dgvOrder.Focus()
        txtCusCode_ord.ReadOnly = True
        btnQueryCus_ord.Enabled = False
        cmbCar_ord.Enabled = False
        cmbCarOut_ord.Enabled = False
        grpTransport.Enabled = False
    End Sub

    ''' <summary>
    ''' 設定銷售管理搜尋相關控制項狀態
    ''' </summary>
    ''' <param name="btnQuery"></param>
    Private Sub SetOrderQueryCtrl(btnQuery As Button)

        Dim lst = New List(Of Control) From {
            lblCusCode
        }

        SetQueryControls(btnQuery, lst)
    End Sub

    ' 取消
    Private Async Sub btnCancel_order_Click(sender As Object, e As EventArgs) Handles btnCancel_order.Click
        SetButtonState(btnCancel_order, True)
        Await _presenter.InitializeAsync()
        dgvOrder.ClearSelection()
        ClearInput()
        txtOperator.Text = Nothing
        dtpOrder.Value = Now
        txtCusCode_ord.Focus()
    End Sub

    ' 客戶代號
    Private Sub txtCusCode_ord_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_ord.KeyDown
        '按下Enter時,搜尋客戶資料
        If e.KeyCode = Keys.Enter AndAlso _presenter.LoadCustomerByCusCode(txtCusCode_ord.Text) Then
            dtpOrder.Focus()
            If rdoPickUp.Checked Then _presenter.LoadCar()
        End If
    End Sub

    ' 搜尋客戶
    Private Sub btnQueryCus_ord_Click(sender As Object, e As EventArgs) Handles btnQueryCus_ord.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                _presenter.LoadCustomerByCusCode(searchForm.CusCode)
                dtpOrder.Focus()
            End If
        End Using
    End Sub

    ' 日期
    Private Sub dtpOrder_KeyDown(sender As Object, e As KeyEventArgs) Handles dtpOrder.KeyDown
        '按下Enter時,跳到"車號"
        If e.KeyCode = Keys.Enter Then
            If rdoPickUp.Checked Then
                cmbCar_ord.Focus()
                '自動展開選單
                cmbCar_ord.DroppedDown = True
            Else
                tcInOut.Focus()
            End If
        End If
    End Sub

    ' 車號-選項改變時
    Private Sub cmbCar_ord_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCar_ord.SelectedIndexChanged
        Dim cmb As ComboBox = sender

        If cmb.SelectedIndex >= 0 Then
            If cmbCarOut_ord.Items.Count > 0 Then
                '同步"出場單"車號
                cmbCarOut_ord.SelectedIndex = cmb.SelectedIndex
            End If

            _presenter.LoadCarStk_In(cmb.SelectedItem.Value)

            If cmb.Focused Then
                '新增時切換車號要重新計算
                _presenter.CalculateCarStk(True)
            End If
        Else
            tpIn.Controls.OfType(Of TextBox).
                Where(Function(txt) txt.Tag.ToString.StartsWith("c_deposit")).
                ToList().
                ForEach(Sub(t) t.Clear())
        End If
    End Sub

    ' 車號-按下Enter
    Private Sub cmbCar_ord_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbCar_ord.KeyDown
        If e.KeyCode = Keys.Enter Then
            tcInOut.Focus()
        End If
    End Sub

    ' 進出場單-切換
    Private Sub tcInOut_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tcInOut.SelectedIndexChanged
        Try
            Dim tc As TabControl = sender

            '顯示明細的狀況下只會有顯示一種單,不用計算
            If tc.TabPages.Count = 1 Then Return

            Dim isIn As Boolean = tc.SelectedTab.Text = "進場單"
            Dim exception = New List(Of String) From {grpTransport.Name}

            If isIn Then
                ClearControls(tpOut, exception)
                If rdoPickUp.Checked AndAlso cmbCar_ord.SelectedItem IsNot Nothing Then _presenter.LoadCarStk_In(cmbCar_ord.SelectedItem.Value)
            Else
                ClearControls(tpIn, exception)
                cmbCarOut_ord.SelectedIndex = cmbCar_ord.SelectedIndex
                If cmbCarOut_ord.SelectedItem IsNot Nothing Then _presenter.LoadCarStk_Out(cmbCarOut_ord.SelectedItem.Value)
            End If
            _presenter.CalculateStkAndPrice(isIn)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ' 進出場單-按下Enter
    Private Sub tcInOut_KeyDown(sender As Object, e As KeyEventArgs) Handles tcInOut.KeyDown
        ' 跳到當前sheet第一格
        If e.KeyCode = Keys.Enter Then
            If tcInOut.SelectedTab.Text = "進場單" Then
                txto_in_50.Focus()
            ElseIf tcInOut.SelectedTab.Text = "出場單" Then
                txtGas_c_50.Focus()
            End If
        End If
    End Sub

    ' 出場單-車號-選項改變時
    Private Sub cmbCarOut_ord_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCarOut_ord.SelectedIndexChanged
        Dim cmb As ComboBox = sender

        If cmb.SelectedIndex >= 0 Then
            _presenter.LoadCarStk_Out(cmb.SelectedItem.Value)

            If cmb.Focused Then
                '新增時切換車號要重新計算
                _presenter.CalculateCarStk(False)
            End If
        Else
            tpOut.Controls.OfType(Of TextBox).
                Where(Function(txt) txt.Tag.ToString.StartsWith("c_deposit")).
                ToList().
                ForEach(Sub(t) t.Clear())
        End If
    End Sub

    ' 新增
    Private Async Sub btnCreate_ord_Click(sender As Object, e As EventArgs) Handles btnCreate_ord.Click
        Dim id = Await _presenter.Add()

        If id <> 0 Then GetDetail(id)
    End Sub

    ' 修改
    Private Sub btnEdit_ord_Click(sender As Object, e As EventArgs) Handles btnEdit_ord.Click
        _presenter.Update()
    End Sub

    ' 刪除
    Private Sub btnDelete_order_Click(sender As Object, e As EventArgs) Handles btnDelete_order.Click
        _presenter.Delete()
    End Sub

    ' 列印
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim id As Integer
        If Integer.TryParse(txto_id.Text, id) Then
            _presenter.Print(id)
        Else
            MsgBox("請選擇對象")
        End If
    End Sub

    ' 列印客戶鋼瓶結存總冊
    Private Sub btnPrintCusStk_Click(sender As Object, e As EventArgs) Handles btnPrintCusStk.Click
        Dim result = MessageBox.Show(
            "是否列印昨天的報表?",
            "客戶鋼瓶結存總冊",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question
        )

        Select Case result
            Case DialogResult.Yes
                _presenter.PrintCusStk(True)
            Case DialogResult.No
                _presenter.PrintCusStk(False)
            Case Else
        End Select
    End Sub

    ' 氣量氣款收付明細表
    Private Sub btnCusGasPayCollect_Click(sender As Object, e As EventArgs) Handles btnCusGasPayCollect.Click
        Using frm As New frmDatePicker
            If frm.ShowDialog = DialogResult.OK Then _presenter.GenerateCustomersGasDetailByDay(frm.SelectedDate, frm.isMonth)
        End Using
    End Sub

    ' 客戶提氣清冊
    Private Sub btnCusGetGasList_Click(sender As Object, e As EventArgs) Handles btnCusGetGasList.Click
        Using frm As New frmDatePicker
            If frm.ShowDialog = DialogResult.OK Then _presenter.GenerateCustomersGetGasList(frm.SelectedDate, frm.isMonth)
        End Using
    End Sub

    ' 供父表單呼叫的快捷鍵處理
    Public Async Sub HandleShortcut(key As Keys)
        Select Case key
            Case Keys.F1
                If btnCreate_ord.Enabled Then Await _presenter.Add()
            Case Keys.F2
                If btnEdit_ord.Enabled Then _presenter.Update()
            Case Keys.F3
                If btnDelete_order.Enabled Then _presenter.Delete()
            Case Keys.F4
                btnCancel_order_Click(btnCancel_order, EventArgs.Empty)
            Case Keys.F5
                btnPrint.PerformClick()
            Case Keys.F6
                btnPrintCusStk.PerformClick()
            Case Keys.F7
                btnCusGasPayCollect_Click(btnCusGasPayCollect, EventArgs.Empty)
            Case Keys.F8
                btnCusGetGasList_Click(btnCusGetGasList, EventArgs.Empty)
        End Select
    End Sub

    ' 選擇廠運時
    Private Sub rdoDelivery_CheckedChanged(sender As Object, e As EventArgs) Handles rdoDelivery.CheckedChanged
        Dim rdo As RadioButton = sender
        If rdo.Checked Then
            '清空車號
            cmbCar_ord.DataSource = Nothing
            cmbCarOut_ord.DataSource = Nothing
            _presenter.CurrentCarIn = Nothing
            _presenter.CurrentCarOut = Nothing

            '清空寄桶
            tpIn.Controls.OfType(Of TextBox).
                          Where(Function(x) x.Tag.ToString.StartsWith("o_deposit_in_") Or x.Tag.ToString.StartsWith("c_deposit_")).
                          ToList.
                          ForEach(Sub(x)
                                      x.Clear()
                                      x.ReadOnly = True
                                  End Sub)
            tpOut.Controls.OfType(Of TextBox).
                           Where(Function(x) x.Tag.ToString.StartsWith("o_deposit_out_") Or x.Tag.ToString.StartsWith("c_deposit_")).
                           ToList.
                           ForEach(Sub(x)
                                       x.Clear()
                                       x.ReadOnly = True
                                   End Sub)
        End If
    End Sub

    ' 選擇自運時
    Private Sub rdoPickUp_CheckedChanged(sender As Object, e As EventArgs) Handles rdoPickUp.CheckedChanged
        Dim rdo As RadioButton = sender
        If rdo.Checked AndAlso dgvOrder.Columns.Count > 0 Then
            _presenter.LoadCar()
            tpIn.Controls.OfType(Of TextBox).
              Where(Function(x) x.Tag.ToString.StartsWith("o_deposit_in_") Or x.Tag.ToString.StartsWith("c_deposit_")).
              ToList.
              ForEach(Sub(x) x.ReadOnly = False)
            tpOut.Controls.OfType(Of TextBox).
                           Where(Function(x) x.Tag.ToString.StartsWith("o_deposit_out_") Or x.Tag.ToString.StartsWith("c_deposit_")).
                           ToList.
                           ForEach(Sub(x) x.ReadOnly = False)
        End If
    End Sub

    ' dgv
    Private Sub dgvOrder_SelectionChanged(sender As Object, e As EventArgs) Handles dgvOrder.SelectionChanged, dgvOrder.CellMouseClick
        If Not dgvOrder.Focused OrElse dgvOrder.SelectedRows.Count = 0 Then Return
        Dim row = dgvOrder.SelectedRows(0)
        Dim id As Integer = row.Cells("編號").Value
        GetDetail(id)
    End Sub

    ' 查詢
    Private Async Sub btnQuery_order_Click(sender As Object, e As EventArgs) Handles btnQuery_order.Click
        Dim btn As Button = btnQuery_order
        SetOrderQueryCtrl(btn)

        If btn.Text = "查  詢" Then
            Await _presenter.LoadList(True)
        End If
    End Sub

    ' 紀錄dgv欄寬
    Private Sub dgvOrder_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvOrder.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub
End Class
