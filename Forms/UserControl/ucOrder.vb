''' <summary>
''' 銷售管理
''' </summary>
Public Class ucOrder
    Implements IOrderView

    ' === 介面 ===
    Public Event CreateRequest As EventHandler Implements IFormView(Of order, OrderListVM).CreateRequest
    Public Event UpdateRequest As EventHandler Implements IFormView(Of order, OrderListVM).UpdateRequest
    Public Event DeleteRequest As EventHandler Implements IFormView(Of order, OrderListVM).DeleteRequest
    Public Event CancelRequest As EventHandler Implements IFormView(Of order, OrderListVM).CancelRequest
    Public Event SearchRequest As EventHandler Implements IFormView(Of order, OrderListVM).SearchRequest
    Public Event CustomerSelected As EventHandler(Of String) Implements IOrderView.CustomerSelected
    Public Event TransportTypeSelected As EventHandler(Of String) Implements IOrderView.TransportTypeSelected
    Public Event BarrelInInput As EventHandler Implements IOrderView.BarrelInInput
    Public Event BarrelUnitPriceInput As EventHandler Implements IOrderView.BarrelUnitPriceInput
    Public Event BarrelOutInput As EventHandler Implements IOrderView.BarrelOutInput
    Public Event DepositInput As EventHandler Implements IOrderView.DepositInput
    Public Event CarSelected As EventHandler(Of Integer) Implements IOrderView.CarSelected
    Public Event OrderTypeChanged As EventHandler Implements IOrderView.OrderTypeChanged
    Public Event ReturnInput As EventHandler Implements IOrderView.ReturnInput
    Public Event DataSelectedRequest As EventHandler(Of Integer) Implements IFormView(Of order, OrderListVM).DataSelectedRequest
    Public Event PrintRequest As EventHandler(Of Integer) Implements IOrderView.PrintRequest
    Public Event PrintCusStkRequest As EventHandler(Of Boolean) Implements IOrderView.PrintCusStkRequest
    Public Event CustomersGasDetailRequest As EventHandler(Of Tuple(Of Date, Boolean)) Implements IOrderView.CustomersGasDetailRequest
    Public Event CusGetGasListRequest As EventHandler(Of Tuple(Of Date, Boolean)) Implements IOrderView.CusGetGasListRequest

#Region "實作介面"
    Private Sub ClearInput() Implements IFormView(Of order, OrderListVM).ClearInput
        ClearControls(Me)
        cmbCar.DataSource = Nothing
        cmbCarOut.DataSource = Nothing

        txtCusCode.Focus()

        '預設運送方式
        rdoPickUp.Checked = True
        tpOut.Parent = tcInOut
        tpIn.Parent = tcInOut
        EditMode(False)

        ' 設定退氣欄位狀態
        If tcInOut.SelectedTab.Text = "進場單" Then
            SetReturnGasReadOnly(True)
        Else
            SetReturnGasReadOnly(False)
        End If

        ' 日期刷新為當下
        dtpOrder.Value = Now
    End Sub

    Public Sub ShowList(data As List(Of OrderListVM)) Implements IFormView(Of order, OrderListVM).ShowList

    End Sub

    Public Sub DisplayList(data As IEnumerable(Of Object)) Implements IOrderView.DisplayList
        dgvOrder.DataSource = data
        AddHandler dgvOrder.CellFormatting, AddressOf DgvOrder_CellFormatting
    End Sub

    Public Sub ButtonStatus(isSelectedRow As Boolean) Implements IFormView(Of order, OrderListVM).ButtonStatus
        SetButtonState(Me, isSelectedRow)
    End Sub

    Public Sub ShowCustomer(data As customer) Implements IOrderView.ShowCustomer
        AutoMapEntityToControls(data, Me)
        txtCusID.Text = data.cus_id
        If rdoPickUp.Checked Then RaiseEvent TransportTypeSelected(Nothing, rdoPickUp.Text)
        dtpOrder.Focus()
    End Sub

    Public Sub SetCarDropdown(list As List(Of SelectListItem)) Implements IOrderView.SetCarDropdown
        SetComboBox(cmbCar, New List(Of SelectListItem)(list))
        SetComboBox(cmbCarOut, New List(Of SelectListItem)(list))
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

    Public Sub SetCusBarrelStock(isIn As Boolean, data As customer) Implements IOrderView.SetCusBarrelStock
        If isIn Then
            AutoMapEntityToControls(data, tpIn)
        Else
            AutoMapEntityToControls(data, tpOut)
        End If
    End Sub

    Public Function GetOrderType() As String Implements IOrderView.GetOrderType
        Return tcInOut.SelectedTab.Text
    End Function

    Public Sub ShowUnitPrice(data As order) Implements IOrderView.ShowUnitPrice
        AutoMapEntityToControls(data, Me)
        AutoMapEntityToControls(data, tpIn)
    End Sub

    Public Sub ShowBarrelPrice(price As Integer) Implements IOrderView.ShowBarrelPrice
        txtBarrelAmount.Text = price
    End Sub

    Public Sub ShowCarBarrelStock_In(data As car) Implements IOrderView.ShowCarBarrelStock_In
        AutoMapEntityToControls(data, tpIn)
    End Sub

    Public Sub ShowCarBarrelStock_Out(data As car) Implements IOrderView.ShowCarBarrelStock_Out
        AutoMapEntityToControls(data, tpOut)
    End Sub

    Public Sub ShowTotalAmount(data As Integer) Implements IOrderView.ShowTotalAmount
        txtTotalAmount.Text = data
    End Sub

    Public Function GetOrderInput() As order Implements IOrderView.GetOrderInput
        Dim data As New order
        AutoMapControlsToEntity(data, Me)
        Return data
    End Function

    Public Sub ShowGasAmount(data As Tuple(Of Integer, Integer)) Implements IOrderView.ShowGasAmount
        txtTotalGas.Text = data.Item1
        txtTotalGas_c.Text = data.Item2
    End Sub

    Public Sub ShowInsurance(data As Double) Implements IOrderView.ShowInsurance
        txtInsurance.Text = data
    End Sub

    Public Function GetInput(ByRef model As order) As Boolean Implements IFormView(Of order, OrderListVM).GetInput
        Try
            If String.IsNullOrEmpty(txtCusID.Text) Then Throw New Exception("請選擇客戶")
            If rdoPickUp.Checked AndAlso cmbCar.SelectedIndex = -1 Then Throw New Exception("請選擇車號")
            AutoMapControlsToEntity(model, Me)
            AutoMapControlsToEntity(model, tcInOut.SelectedTab)
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub GetCusStkInput(ByRef data As customer) Implements IOrderView.GetCusStkInput
        AutoMapControlsToEntity(data, tcInOut.SelectedTab)
    End Sub

    Public Sub GetCarStkInput(ByRef data As car) Implements IOrderView.GetCarStkInput
        AutoMapControlsToEntity(data, tcInOut.SelectedTab)
    End Sub

    Public Sub ShowDetail(data As order) Implements IFormView(Of order, OrderListVM).ShowDetail
        AutoMapEntityToControls(data, Me)

        If data.o_in_out = "進場單" Then
            AutoMapEntityToControls(data, tpIn)
            tpOut.Parent = Nothing
            tpIn.Parent = tcInOut
        Else
            AutoMapEntityToControls(data, tpOut)
            tpOut.Parent = tcInOut
            tpIn.Parent = Nothing
        End If

        EditMode(True)
    End Sub

    Public Function GetSearchCriteria() As OrderSearchCriteria Implements IOrderView.GetSearchCriteria
        Using frm As New frmSearch_Order
            If frm.ShowDialog = DialogResult.OK Then
                Return frm.Criteria
            End If
        End Using

        Return Nothing
    End Function

    Public Sub ShowOrderDetail(data As order) Implements IOrderView.ShowOrderDetail
        AutoMapEntityToControls(data, tpIn)
        AutoMapEntityToControls(data, tpOut)
    End Sub

    Public Sub ShowCusBarrelStock(data As order) Implements IOrderView.ShowCusBarrelStock
        Select Case tcInOut.SelectedTab.Text
            Case "進場單"
                txtBarrelIn_50.Text = data.o_cus_50
                txtBarrelIn_20.Text = data.o_cus_20
                txtBarrelIn_16.Text = data.o_cus_16
                txtBarrelIn_10.Text = data.o_cus_10
                txtBarrelIn_4.Text = data.o_cus_4
                txtBarrelIn_18.Text = data.o_cus_18
                txtBarrelIn_14.Text = data.o_cus_14
                txtBarrelIn_5.Text = data.o_cus_5
                txtBarrelIn_2.Text = data.o_cus_2
            Case "出場單"
                txtCusGas_50.Text = data.o_cus_50
                txtCusGas_20.Text = data.o_cus_20
                txtCusGas_16.Text = data.o_cus_16
                txtCusGas_10.Text = data.o_cus_10
                txtCusGas_4.Text = data.o_cus_4
                txtCusGas_18.Text = data.o_cus_18
                txtCusGas_14.Text = data.o_cus_14
                txtCusGas_5.Text = data.o_cus_5
                txtCusGas_2.Text = data.o_cus_2
        End Select
    End Sub

    Private Sub SetReturnGasReadOnly(isReadOnly As Boolean) Implements IOrderView.SetReturnGasReadOnly
        txto_return.ReadOnly = isReadOnly
        txto_return_c.ReadOnly = isReadOnly
        If isReadOnly Then
            txto_return.BackColor = Color.LightGray
            txto_return_c.BackColor = Color.LightGray
        Else
            txto_return.BackColor = Color.White
            txto_return_c.BackColor = Color.White
        End If
    End Sub
#End Region

#Region "UI事件"
    Private Sub OrderUserControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCancel.PerformClick()
        SetupSalesManagementHandlers()
        tcInOut.DrawMode = TabDrawMode.OwnerDrawFixed
        AddHandler tcInOut.DrawItem, AddressOf TabControl_DrawItem
        ReadDataGridWidth(dgvOrder)
    End Sub

    ' 取消
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        RaiseEvent CancelRequest(sender, e)
        txtCusCode.Focus()
    End Sub

    ' 客戶代號
    Private Sub txtCusCode_ord_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode.KeyDown
        If e.KeyCode = Keys.Enter Then RaiseEvent CustomerSelected(sender, txtCusCode.Text)
    End Sub

    ' 搜尋客戶
    Private Sub btnSearchCus_Click(sender As Object, e As EventArgs) Handles btnSearchCus.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then RaiseEvent CustomerSelected(sender, searchForm.CusCode)
        End Using
    End Sub

    ' 日期
    Private Sub dtpOrder_KeyDown(sender As Object, e As KeyEventArgs) Handles dtpOrder.KeyDown
        '按下Enter時,跳到"車號"
        If e.KeyCode = Keys.Enter Then
            If rdoPickUp.Checked Then
                cmbCar.Focus()
                '自動展開選單
                cmbCar.DroppedDown = True
            Else
                tcInOut.Focus()
            End If
        End If
    End Sub

    ' 運送方式-廠運
    Private Sub rdoDelivery_CheckedChanged(sender As Object, e As EventArgs) Handles rdoDelivery.CheckedChanged
        cmbCar.DataSource = Nothing
        cmbCarOut.DataSource = Nothing
    End Sub

    ' 運送方式-自運
    Private Sub rdoPickUp_CheckedChanged(sender As Object, e As EventArgs) Handles rdoPickUp.CheckedChanged
        If Not String.IsNullOrEmpty(txtCusCode.Text) Then RaiseEvent TransportTypeSelected(sender, txtCusCode.Text)
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

    ' 車號-選項改變時
    Private Sub cmbCar_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCar.SelectedIndexChanged
        Dim cmb As ComboBox = sender

        If cmb.Focused AndAlso cmb.SelectedIndex >= 0 Then
            If cmbCarOut.Items.Count > 0 Then
                '同步"出場單"車號
                cmbCarOut.SelectedIndex = cmb.SelectedIndex
            End If

            RaiseEvent CarSelected(sender, cmbCar.SelectedItem.Value)
        End If
    End Sub

    ' 出場-車號
    Private Sub cmbCarOut_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbCarOut.SelectionChangeCommitted
        RaiseEvent CarSelected(sender, cmbCarOut.SelectedItem.Value)
    End Sub

    ' 車號-按下Enter
    Private Sub cmbCar_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbCar.KeyDown
        If e.KeyCode = Keys.Enter Then
            tcInOut.Focus()
        End If
    End Sub

    ' 紀錄dgv欄寬
    Private Sub dgvOrder_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvOrder.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub

    ' 進出場單-切換
    Private Sub tcInOut_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tcInOut.SelectedIndexChanged
        Dim tc As TabControl = sender

        '顯示明細的狀況下只會有顯示一種單,不用計算
        If tc.TabPages.Count = 1 Then Return

        Dim isIn As Boolean = tc.SelectedTab.Text = "進場單"
        Dim tp = If(isIn, tpOut, tpIn)

        tp.Controls.OfType(Of TextBox).Where(Function(x) Not x.ReadOnly AndAlso Not x.Name.StartsWith("txtBarralUnitPrice_")).ToList.ForEach(Sub(txt) txt.Text = 0)

        ' 切到進場單 總普氣、丙氣、保險金額要清空
        If isIn Then
            txtTotalGas.Clear()
            txtTotalGas_c.Clear()
            txtInsurance.Clear()
        End If

        RaiseEvent OrderTypeChanged(sender, e)

        ' 設定退氣欄位狀態
        If isIn Then
            SetReturnGasReadOnly(True)
        Else
            SetReturnGasReadOnly(False)
        End If
    End Sub

    ' 新增
    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        RaiseEvent CreateRequest(sender, e)
    End Sub

    ' dgv
    Private Sub dgvOrder_SelectionChanged(sender As Object, e As EventArgs) Handles dgvOrder.SelectionChanged, dgvOrder.CellMouseClick
        If Not dgvOrder.Focused OrElse dgvOrder.SelectedRows.Count = 0 Then Return
        Dim row = dgvOrder.SelectedRows(0)
        Dim id As Integer = row.Cells("編號").Value
        RaiseEvent DataSelectedRequest(sender, id)
    End Sub

    ' 修改
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        RaiseEvent UpdateRequest(sender, e)
    End Sub

    ' 刪除
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        RaiseEvent DeleteRequest(sender, e)
    End Sub

    ' 列印
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim id As Integer
        If Integer.TryParse(txto_id.Text, id) Then
            RaiseEvent PrintRequest(sender, id)
        Else
            MessageBox.Show("請選擇對象")
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
                RaiseEvent PrintCusStkRequest(sender, True)
            Case DialogResult.No
                RaiseEvent PrintCusStkRequest(sender, False)
            Case Else
        End Select
    End Sub

    ' 氣量氣款收付明細表
    Private Sub btnCusGasPayCollect_Click(sender As Object, e As EventArgs) Handles btnCusGasPayCollect.Click
        Using frm As New frmDatePicker
            If frm.ShowDialog = DialogResult.OK Then RaiseEvent CustomersGasDetailRequest(sender, Tuple.Create(frm.SelectedDate, frm.isMonth))
        End Using
    End Sub

    ' 客戶提氣清冊
    Private Sub btnCusGetGasList_Click(sender As Object, e As EventArgs) Handles btnCusGetGasList.Click
        Using frm As New frmDatePicker
            If frm.ShowDialog = DialogResult.OK Then RaiseEvent CusGetGasListRequest(sender, Tuple.Create(frm.SelectedDate, frm.isMonth))
        End Using
    End Sub

    ' 查詢
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        RaiseEvent SearchRequest(sender, e)
    End Sub

    ''' <summary>
    ''' 瓦斯瓶進廠輸入
    ''' </summary>
    Private Sub BarrelInKeyIn()
        RaiseEvent BarrelInInput(Nothing, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' 瓦斯桶出輸入
    ''' </summary>
    Private Sub BarrelOutKeyIn()
        RaiseEvent BarrelOutInput(Nothing, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' 瓦斯桶單價輸入
    ''' </summary>
    Private Sub BarrelUnitPriceKeyIn()
        RaiseEvent BarrelUnitPriceInput(Nothing, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' 寄桶輸入
    ''' </summary>
    Private Sub DepositKeyIn()
        RaiseEvent DepositInput(Nothing, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' 退氣、折讓輸入
    ''' </summary>
    Private Sub ReturnKeyIn()
        RaiseEvent ReturnInput(Nothing, EventArgs.Empty)
    End Sub
#End Region

#Region "方法"
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
        ' 進場單
        SetupTextBoxHandlers(tpIn, AddressOf BarrelInKeyIn, "txto_in", "txto_new_in", "txto_inspect")
        SetupTextBoxHandlers(tpIn, AddressOf BarrelUnitPriceKeyIn, "txtBarralUnitPrice")
        SetupTextBoxHandlers(tpIn, AddressOf DepositKeyIn, "txtDepositIn")

        ' 出場單
        SetupTextBoxHandlers(tpOut, AddressOf BarrelOutKeyIn, "txtGas", "txtEmpty")
        SetupTextBoxHandlers(tpOut, AddressOf DepositKeyIn, "txtDepositOut")

        ' 訂單
        SetupTextBoxHandlers(Me, AddressOf ReturnKeyIn, "txto_return", "txto_return_c", "txto_sales_allowance")

        ' 為所有非只讀的TextBox添加方向鍵處理
        AddDirectionHandlers(tpIn)
        AddDirectionHandlers(tpOut)
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

    Private Sub EditMode(isDataSelected As Boolean)
        txtCusCode.ReadOnly = isDataSelected
        btnSearchCus.Enabled = Not isDataSelected
        cmbCar.Enabled = Not isDataSelected
        grpTransport.Enabled = Not isDataSelected
        cmbCarOut.Enabled = Not isDataSelected
    End Sub

    ''' <summary>
    ''' 供父表單呼叫的快捷鍵處理
    ''' </summary>
    ''' <param name="key"></param>
    Public Sub HandleShortcut(key As Keys)
        Select Case key
            Case Keys.F1
                If btnCreate.Enabled Then
                    btnCreate.PerformClick()
                End If
            Case Keys.F2
                If btnEdit.Enabled Then
                    btnEdit.PerformClick()
                End If
            Case Keys.F3
                If btnDelete.Enabled Then
                    btnDelete.PerformClick()
                End If
            Case Keys.F4
                btnCancel_Click(btnCancel, EventArgs.Empty)
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

    Private Sub dgvOrder_SelectionChanged(sender As Object, e As DataGridViewCellMouseEventArgs)

    End Sub
#End Region
End Class
