Imports System.Text.RegularExpressions

Public Class frmMain
    Implements ISubjectGroupView, ISubjectsView

    Private _sgPresenter As SubjectGroupPresenter
    Private _subjectsPresenter As SubjectsPresenter
    Private _p As SubjectsPresenter
    Private orgStockValues As New Dictionary(Of String, Object) '儲存客戶瓦斯桶庫存
    Private orgDepositStockValues As New Dictionary(Of String, Object) '儲存司機寄瓶庫存
    Private orgGasValues As New Dictionary(Of String, Integer) '用於存儲 txtGas_、txtGas_c_ 開頭的 TextBox 的初始值
    Private orgDepositValues As New Dictionary(Of String, Object) '用於存儲TextBox.Tag = o_deposit開頭的初始值

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetSheetColor()
        SetDataGridViewStyle(Me)
        InitPermissions()
        InitTabPage()
        SetDGVColumnWidthSave()
        SetQueryEnterEven(Me)
        SetTextBoxIntOnly()
        PositiveIntegerOnly()
        TextChagedHandler()
    End Sub

    Private Sub InitPermissions()
        For Each sheet As String In Sheets
            flpRoles.Controls.Add(New CheckBox With
                                  {
                                    .Text = sheet,
                                    .Width = 200
                                  })
        Next

        With dgvPermissions.Columns
            .Add("編號", "編號")
            .Add("角色名稱", "角色名稱")
            For Each sheet As String In Sheets
                .Add(New DataGridViewCheckBoxColumn With
                     {
                        .Name = sheet,
                        .HeaderText = sheet
                     })
            Next
        End With
    End Sub

    ''' <summary>
    ''' 初始化各TabPage
    ''' </summary>
    Private Sub InitTabPage()
        'btnCancel_emp_Click(btnCancel_emp, EventArgs.Empty)
        'btnCancel_roles_Click(btnCancel_roles, EventArgs.Empty)
        'btnCancel_manu_Click(btnCancel_manu, EventArgs.Empty)
        btnCancel_cus_Click(btnCancel_cus, EventArgs.Empty)
        'btnCancel_comp_Click(btnCancel_comp, EventArgs.Empty)
        btnCancel_bp_Click(btnCancel_bp, EventArgs.Empty)
        'btnCancel_bank_Click(btnCancel_bank, EventArgs.Empty)
        btnCancel_order_Click(btnCancel_order, EventArgs.Empty)
        btnCancel_car_Click(btnCancel_car, EventArgs.Empty)
        _sgPresenter = New SubjectGroupPresenter(Me)
        btnCancel_sg_Click(btnCancel_sg, EventArgs.Empty)
        _subjectsPresenter = New SubjectsPresenter(Me)
    End Sub

    ''' <summary>
    ''' 設定TextBox值改變事件
    ''' </summary>
    Private Sub TextChagedHandler()
        Dim inTxts = tpIn.Controls.OfType(Of TextBox)
        inTxts.Where(Function(txt) txt.Name.StartsWith("txto_")).ToList.ForEach(Sub(t) AddHandler t.KeyUp, Sub(sender, e) CalculateStock(sender, e, True))
        inTxts.Where(Function(txt) txt.Name.StartsWith("txtDepositIn_")).ToList.ForEach(Sub(t) AddHandler t.KeyUp, Sub(sender, e) CalculateDeposit_TextChaged(sender, e, True))

        Dim outTxts = tpOut.Controls.OfType(Of TextBox)
        outTxts.Where(Function(txt) txt.Name.StartsWith("txtGas") Or txt.Name.StartsWith("txtEmpty")).
            ToList.ForEach(Sub(t) AddHandler t.KeyUp, Sub(sender, e) CalculateStock(sender, e, False))
        outTxts.Where(Function(txt) txt.Name.StartsWith("txtDepositOut_")).ToList.ForEach(Sub(t) AddHandler t.KeyUp, Sub(sender, e) CalculateDeposit_TextChaged(sender, e, False))

        Dim lstGas_c As New List(Of TextBox)
        Dim regGas_c As New Regex("^txtGas_c_\d+$")
        lstGas_c.AddRange(outTxts.Where(Function(txt) regGas_c.IsMatch(txt.Name)))
        lstGas_c.Add(txto_return_c)
        lstGas_c.ForEach(Sub(t) AddHandler t.TextChanged, Sub(sender, e) GasSum(sender, e, "txtGas_c_"))

        Dim lstGas As New List(Of TextBox)
        Dim regGas As New Regex("^txtGas_\d+$")
        lstGas.AddRange(outTxts.Where(Function(txt) regGas.IsMatch(txt.Name)))
        lstGas.Add(txto_return)
        lstGas.ForEach(Sub(t) AddHandler t.TextChanged, Sub(sender, e) GasSum(sender, e, "txtGas_"))

    End Sub

    ''' <summary>
    ''' 設定TextBox只能輸入正整數
    ''' </summary>
    Private Sub PositiveIntegerOnly()
        Dim textBoxes = New List(Of TextBox) From {txtGas_50, txtGas_20, txtGas_16, txtGas_10, txtGas_4, txtGas_15, txtGas_14, txtGas_5, txtGas_2,
            txtGas_c_50, txtGas_c_20, txtGas_c_16, txtGas_c_10, txtGas_c_4, txtGas_c_15, txtGas_c_14, txtGas_c_5, txtGas_c_2,
            txtEmpty_50, txtEmpty_20, txtEmpty_16, txtEmpty_10, txtEmpty_4, txtEmpty_15, txtEmpty_14, txtEmpty_5, txtEmpty_2,
            txtDepositOut_50, txtDepositOut_20, txtDepositOut_16, txtDepositOut_10, txtDepositOut_4, txtDepositOut_15, txtDepositOut_14, txtDepositOut_5, txtDepositOut_2,
            txto_in_50, txto_in_20, txto_in_16, txto_in_10, txto_in_4, txto_in_15, txto_in_14, txto_in_5, txto_in_2,
            txto_new_in_50, txto_new_in_20, txto_new_in_16, txto_new_in_10, txto_new_in_4, txto_new_in_15, txto_new_in_14, txto_new_in_5, txto_new_in_2,
            txto_inspect_50, txto_inspect_20, txto_inspect_16, txto_inspect_10, txto_inspect_4, txto_inspect_15, txto_inspect_14, txto_inspect_5, txto_inspect_2,
            txtDepositIn_50, txtDepositIn_20, txtDepositIn_16, txtDepositIn_10, txtDepositIn_4, txtDepositIn_15, txtDepositIn_14, txtDepositIn_5, txtDepositIn_2,
            txto_return, txto_return_c, txto_sales_allowance}

        textBoxes.ForEach(Sub(txt) AddHandler txt.KeyPress, AddressOf SetPositiveIntegerOnly)
    End Sub

    ''' <summary>
    ''' 設定TextBox只能輸入整數
    ''' </summary>
    Private Sub SetTextBoxIntOnly()
        Dim numeric As New List(Of TextBox) From {
            txtcus_gas_normal,
            txtcus_gas_c,
            txtcus_gas_normal_deliver,
            txtcus_gas_c_deliver
        }

        ' 為這些控件添加 KeyPress 事件處理器
        For Each textBox In numeric
            AddHandler textBox.KeyPress, AddressOf TextBox_KeyPress_Int
        Next
    End Sub

    ''' <summary>
    ''' 設定所有dgv在調整欄寬時紀錄
    ''' </summary>
    Private Sub SetDGVColumnWidthSave()
        Dim dgvs = GetControlInParent(Of DataGridView)(Me)
        GetControlInParent(Of DataGridView)(Me).ForEach(Sub(dgv) AddHandler dgv.ColumnWidthChanged, AddressOf SaveDataGridWidth)
        ReadDataGridWidth(dgvs)
    End Sub

    ''' <summary>
    ''' 自定義索引標籤、文字顏色
    ''' </summary>
    Private Sub SetSheetColor()
        TabControl1.DrawMode = DrawMode.OwnerDrawFixed
        AddHandler TabControl1.DrawItem, AddressOf TabControl_DrawItem
        tcBasicInfo.DrawMode = DrawMode.OwnerDrawFixed
        AddHandler tcBasicInfo.DrawItem, AddressOf TabControl_DrawItem
    End Sub

    '基本資料-客戶管理-取消
    Private Sub btnCancel_cus_Click(sender As Object, e As EventArgs) Handles btnCancel_cus.Click
        SetButtonState(sender, True)
        ClearControls(tpCustomer)
        btnQuery_cus.Text = "查  詢"

        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.customers.Include("cars").ToList()
                dgvCustomer.DataSource = query
                SetColumnHeaders("customer", dgvCustomer)
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '基本資料-客戶管理-新增
    Private Sub btnAdd_cus_Click(sender As Object, e As EventArgs) Handles btnCreate_cus.Click
        Dim required = New List(Of Control) From {txtCusCode, txtCusName_cus, txtCusContactPerson, txtCusPhone1}
        If Not CheckRequired(required) Then Exit Sub

        Try
            Using db As New gas_accounting_systemEntities
                Dim insert As New customer
                AutoMapControlsToEntity(insert, tpCustomer)
                AutoMapControlsToEntity(insert, grpStock)
                db.customers.Add(insert)
                db.SaveChanges()
                btnCancel_cus.PerformClick()
                MsgBox("新增成功")
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '基本資料-客戶管理-dgv
    Private Sub dgvCustomer_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCustomer.SelectionChanged, dgvCustomer.CellClick
        If Not sender.focused Then Return

        SetButtonState(sender, False)

        Dim id = dgvCustomer.SelectedRows(0).Cells("cus_id").Value

        Try
            Using db As New gas_accounting_systemEntities
                Dim cus = db.customers.Find(id)
                AutoMapEntityToControls(cus, tpCustomer)
                AutoMapEntityToControls(cus, grpStock)
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '基本資料-客戶管理-修改
    Private Sub btnEdit_cus_Click(sender As Object, e As EventArgs) Handles btnEdit_cus.Click
        Dim required = New List(Of Control) From {txtCusCode, txtCusName_cus, txtCusContactPerson, txtCusPhone1}
        If Not CheckRequired(required) Then Exit Sub

        Dim id As Integer = txtcus_id.Text

        Try
            Using db As New gas_accounting_systemEntities
                Dim update = db.customers.Find(id)
                AutoMapControlsToEntity(update, tpCustomer)
                AutoMapControlsToEntity(update, grpStock)
                db.SaveChanges()
                MsgBox("修改成功")
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '基本資料-客戶管理-刪除
    Private Sub btnDelete_cus_Click(sender As Object, e As EventArgs) Handles btnDelete_cus.Click
        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.No Then Exit Sub

        Dim id As Integer = txtcus_id.Text

        Try
            Using db As New gas_accounting_systemEntities
                Dim delete = db.customers.Find(id)
                db.customers.Remove(delete)
                db.SaveChanges()
                btnCancel_cus.PerformClick()
                MsgBox("刪除成功")
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '基本資料-客戶管理-查詢
    Private Sub btnQuery_cus_Click(sender As Object, e As EventArgs) Handles btnQuery_cus.Click
        Dim btn As Button = sender
        Dim lst = New List(Of Control) From {
            txtCusCode,
            txtCusName_cus,
            txtCusPhone1
        }

        SetQueryControls(btn, lst)

        If btn.Text = "查  詢" Then
            Try
                Using db As New gas_accounting_systemEntities
                    Dim query = From cus In db.customers
                    If Not String.IsNullOrEmpty(txtCusCode.Text) Then query = query.Where(Function(x) x.cus_code.Contains(txtCusCode.Text))
                    If Not String.IsNullOrEmpty(txtCusName_cus.Text) Then query = query.Where(Function(x) x.cus_name.Contains(txtCusName_cus.Text))
                    If Not String.IsNullOrEmpty(txtCusPhone1.Text) Then query = query.Where(Function(x) x.cus_phone1.Contains(txtCusPhone1.Text))
                    dgvCustomer.DataSource = query.ToList
                    'SetColumnHeaders("customer", dgvCustomer)
                End Using

                ClearControls(tpCustomer)
            Catch ex As Exception
                MsgBox(ex.Message)
                Console.WriteLine(ex.Message)
            End Try
        End If
    End Sub

    '基本資料-車輛管理-取消
    Private Sub btnCancel_car_Click(sender As Object, e As EventArgs) Handles btnCancel_car.Click
        SetButtonState(sender, True)
        ClearControls(tpCar)
        btnQuery_car.Text = "查  詢"

        Try
            Using db As New gas_accounting_systemEntities
                dgvCar.DataSource = CarMV.GetCarList(db.cars.AsQueryable)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '基本資料-車輛管理-新增
    Private Sub btnCreate_car_Click(sender As Object, e As EventArgs) Handles btnCreate_car.Click
        Dim required = New List(Of Control) From {txtCusId_car, txtCarNo}
        If Not CheckRequired(required) Then Exit Sub

        Try
            Using db As New gas_accounting_systemEntities
                Dim car As New car
                AutoMapControlsToEntity(car, tpCar)
                AutoMapControlsToEntity(car, grpStock_car)
                db.cars.Add(car)
                db.SaveChanges()
                btnCancel_car.PerformClick()
                MsgBox("新增成功")
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '基本資料-車輛管理-dgv
    Private Sub dgvCar_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCar.SelectionChanged, dgvCar.CellMouseClick
        If Not sender.focused Then Return

        SetButtonState(sender, False)

        Dim id = dgvCar.SelectedRows(0).Cells("編號").Value

        Try
            Using db As New gas_accounting_systemEntities
                Dim car = db.cars.Find(id)
                AutoMapEntityToControls(car, tpCar)
                AutoMapEntityToControls(car.customer, tpCar)
                AutoMapEntityToControls(car, grpStock_car)
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '基本資料-車輛管理-修改
    Private Sub btnEdit_car_Click(sender As Object, e As EventArgs) Handles btnEdit_car.Click
        Dim required = New List(Of Control) From {txtCusId_car, txtCarNo}
        If Not CheckRequired(required) Then Exit Sub

        Dim id As Integer = txtCarId.Text

        Try
            Using db As New gas_accounting_systemEntities
                Dim car = db.cars.Find(id)
                AutoMapControlsToEntity(car, tpCar)
                AutoMapControlsToEntity(car, grpStock_car)
                db.SaveChanges()
                btnCancel_car.PerformClick()
                MsgBox("更新成功")
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '基本資料-車輛管理-刪除
    Private Sub btnDelete_car_Click(sender As Object, e As EventArgs) Handles btnDelete_car.Click
        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.No Then Exit Sub
        Dim id As Integer = txtCarId.Text

        Try
            Using db As New gas_accounting_systemEntities
                Dim delete = db.cars.Find(id)
                db.cars.Remove(delete)
                db.SaveChanges()
                btnCancel_car.PerformClick()
                MsgBox("刪除成功")
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '基本資料-車輛管理-查詢客戶
    Private Sub btnQueryCus_car_Click(sender As Object, e As EventArgs) Handles btnQueryCus_car.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusId_car.Text = searchForm.ID
                txtCusName_car.Text = searchForm.Name
            End If
        End Using
    End Sub

    '基本資料-車輛管理-查詢
    Private Sub btnQuery_car_Click(sender As Object, e As EventArgs) Handles btnQuery_car.Click
        Dim btn As Button = sender
        Dim lst = New List(Of Control) From {
            txtCusId_car,
            txtCarNo
        }

        SetQueryControls(btn, lst)

        If btn.Text = "查  詢" Then
            Try
                Using db As New gas_accounting_systemEntities
                    Dim query = db.cars.AsQueryable
                    If Not String.IsNullOrEmpty(txtCusId_car.Text) Then query = query.Where(Function(x) x.c_cus_id = txtCusId_car.Text)
                    If Not String.IsNullOrEmpty(txtCarNo.Text) Then query = query.Where(Function(x) x.c_no.Contains(txtCarNo.Text))
                    dgvCar.DataSource = CarMV.GetCarList(query)
                End Using

                ClearControls(tpCar)
            Catch ex As Exception
                MsgBox(ex.Message)
                Console.WriteLine(ex.Message)
            End Try
        End If
    End Sub

    '基本資料-基礎價格-取消
    Private Sub btnCancel_bp_Click(sender As Object, e As EventArgs) Handles btnCancel_bp.Click
        SetButtonState(sender, True)
        ClearControls(tpBasePrice)

        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.basic_price.AsEnumerable
                dgvBasicPrice.DataSource = BasicPriceMV.GetBPList(query)
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    '基本資料-基礎價格-新增
    Private Sub btnInsert_bp_Click(sender As Object, e As EventArgs) Handles btnCreate_bp.Click
        '檢查重複年月
        Try
            Using db As New gas_accounting_systemEntities
                Dim year = dtpbp_date.Value.Year
                Dim month = dtpbp_date.Value.Month
                Dim query = From bp In db.basic_price
                            Where bp.bp_date.Year = year AndAlso bp.bp_date.Month = month

                If query.Count > 0 Then
                    MsgBox("重複年月份!")
                    Exit Sub
                End If

                Dim insert As New basic_price
                AutoMapControlsToEntity(insert, tpBasePrice)
                db.basic_price.Add(insert)
                db.SaveChanges()
                btnCancel_bp.PerformClick()
                MsgBox("新增成功")
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '系統設定-基礎價格-dgv點擊
    Private Sub dgvBasicPrice_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvBasicPrice.CellMouseClick
        If Not sender.focused Then Return
        SetButtonState(sender, False)

        Dim id = dgvBasicPrice.SelectedRows(0).Cells("編號").Value

        Try
            Using db As New gas_accounting_systemEntities
                Dim cus = db.basic_price.Find(id)
                AutoMapEntityToControls(cus, tpBasePrice)
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '系統設定-基礎價格-修改
    Private Sub btnEdit_bp_bp_Click(sender As Object, e As EventArgs) Handles btnEdit_bp.Click
        Dim id As Integer = txtbp_id.Text

        Try
            Using db As New gas_accounting_systemEntities
                Dim update = db.basic_price.Find(id)
                AutoMapControlsToEntity(update, tpBasePrice)
                db.SaveChanges()
                MsgBox("更新成功")
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '系統設定-基礎價格-查詢
    Private Sub btnQuery_bp_Click(sender As Object, e As EventArgs) Handles btnQuery_bp.Click
        Try
            Using db As New gas_accounting_systemEntities
                Dim year = dtpbp_date.Value.Year
                Dim month = dtpbp_date.Value.Month
                Dim query = From bp In db.basic_price.AsEnumerable
                            Where bp.bp_date.Year = year AndAlso bp.bp_date.Month = month

                dgvBasicPrice.DataSource = BasicPriceMV.GetBPList(query)
                ClearControls(tpBasePrice)

            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '銷售管理-銷售作業-取消
    Private Sub btnCancel_order_Click(sender As Object, e As EventArgs) Handles btnCancel_order.Click
        SetButtonState(sender, True)

        Dim exception = New List(Of String) From {grpTransport.Name}
        ClearControls(tpOrder, exception)
        cmbo_deposit_out_c_id.DataSource = Nothing
        orgGasValues.Clear()
        orgDepositValues.Clear()

        Using db As New gas_accounting_systemEntities
            Dim query = db.orders.Include("car").Include("car.customer")
            dgvOrder.DataSource = OrderMV.GetOrderList(query)
        End Using

        tpOut.Parent = tcInOut
        tpIn.Parent = tcInOut
    End Sub

    '銷售管理-銷售作業-搜尋客戶
    Private Sub btnQueryCus_ord_Click(sender As Object, e As EventArgs) Handles btnQueryCus_ord.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusID_order.Text = searchForm.ID
                txtCusName.Text = searchForm.Name
                cmbCarNo.Text = ""
            End If
        End Using
    End Sub

    '銷售管理-銷售作業-車號選擇
    Private Sub cmbCarNo_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbCarNo.SelectionChangeCommitted
        Dim cmb As ComboBox = sender
        cmbo_deposit_out_c_id.SelectedIndex = cmbCarNo.SelectedIndex

        LoadDepositValue(cmb, tpIn)
    End Sub

    '銷售管理-銷售作業-寄桶結存車號
    Private Sub cmbo_deposit_out_c_id_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbo_deposit_out_c_id.SelectedIndexChanged
        Dim cmb As ComboBox = sender
        LoadDepositValue(cmb, cmb.Parent)
    End Sub

    '載入車輛結存瓶
    Private Sub LoadDepositValue(cmb As ComboBox, container As Control)
        container.Controls.OfType(Of TextBox).Where(Function(txt) txt.Tag.ToString.StartsWith("o_deposit")).ToList.ForEach(Sub(t) t.Clear())

        If cmb.SelectedIndex <> -1 Then
            Dim id As Integer = cmb.SelectedItem.Key

            Using db As New gas_accounting_systemEntities
                Dim query = db.cars.Find(id)
                AutoMapEntityToControls(query, container)
                orgDepositStockValues = GetEntityFieldsByPrefix(query, "c_deposit_")

                If Not String.IsNullOrEmpty(txto_id.Text) Then
                    Dim ordId As Integer = txto_id.Text
                    Dim order = db.orders.Find(ordId)
                    orgDepositValues = GetEntityFieldsByPrefix(order, "o_deposit_out_")
                End If
            End Using
        End If
    End Sub

    '銷售管理-銷售作業-新增
    Private Sub btnCreate_ord_Click(sender As Object, e As EventArgs) Handles btnCreate_ord.Click
        dtpo_date.Value = Now

        Dim required = New List(Of Control) From {txtCusID_order, cmbCarNo}
        If Not CheckRequired(required) Then Exit Sub

        Dim container = tpOrder

        Try
            Using db As New gas_accounting_systemEntities
                '新增到order
                Dim order As New order
                AutoMapControlsToEntity(order, container)
                AutoMapControlsToEntity(order, tpOut)
                AutoMapControlsToEntity(order, tpIn)
                db.orders.Add(order)

                '更新客戶、車輛瓦斯瓶庫存
                Dim cusId As Integer = txtCusID_order.Text
                Dim cus = db.customers.Find(cusId)

                If tcInOut.SelectedIndex = 0 Then
                    AutoMapControlsToEntity(cus, tpIn)

                    Dim carId As Integer = cmbCarNo.SelectedItem.key
                    Dim car = db.cars.Find(carId)
                    AutoMapControlsToEntity(car, tpIn)

                Else
                    AutoMapControlsToEntity(cus, tpOut)

                    Dim carId As Integer = cmbo_deposit_out_c_id.SelectedItem.key
                    Dim car = db.cars.Find(carId)
                    AutoMapControlsToEntity(car, tpOut)
                End If

                db.SaveChanges()
            End Using

            btnCancel_order.PerformClick()
            MsgBox("新增成功")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 搜尋基礎價格
    ''' </summary>
    ''' <param name="d"></param>
    ''' <returns></returns>
    Private Function QueryBasicPrice(d As Date) As IEnumerable(Of basic_price)
        Dim query As IEnumerable(Of basic_price)

        Try
            Using db As New gas_accounting_systemEntities
                Dim year = d.Year
                Dim month = d.Month
                query = From bp In db.basic_price.AsEnumerable
                        Where bp.bp_date.Year = year AndAlso bp.bp_date.Month = month
            End Using
            Return query
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return Nothing
    End Function

    '銷售管理-銷售作業-客戶編號改變時
    Private Sub txtCusID_order_TextChanged(sender As Object, e As EventArgs) Handles txtCusID_order.TextChanged
        Dim txt As TextBox = sender
        Dim id As Integer

        ClearControls(tpIn)
        ClearControls(tpOut)

        If Integer.TryParse(txt.Text, id) Then
            Try
                Using db As New gas_accounting_systemEntities
                    Dim cus = db.customers.Find(id)
                    AutoMapEntityToControls(cus, tpOut)
                    AutoMapEntityToControls(cus, tpIn)

                    Dim query = (From car In db.cars
                                 Where car.c_cus_id = txtCusID_order.Text).ToList

                    Dim formatQuery = query.Select(Function(x) New With {
                                    .Key = x.c_id,
                                    .Value = $"{x.c_no} {x.c_driver}"
                                })

                    cmbCarNo.DataSource = formatQuery.ToList
                    cmbCarNo.DisplayMember = "Value"
                    cmbCarNo.ValueMember = "Key"
                    cmbCarNo.SelectedIndex = -1

                    cmbo_deposit_out_c_id.DataSource = formatQuery.ToList
                    cmbo_deposit_out_c_id.DisplayMember = "Value"
                    cmbo_deposit_out_c_id.ValueMember = "Key"
                    cmbo_deposit_out_c_id.SelectedIndex = -1

                    orgStockValues.Clear()
                    orgStockValues = GetEntityFieldsByPrefix(cus, "cus_gas")
                End Using

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    '銷售管理-銷售作業-修改
    Private Sub btnEdit_order_Click(sender As Object, e As EventArgs) Handles btnEdit_order.Click
        Dim required = New List(Of Control) From {txtCusID_order, cmbCarNo}
        If Not CheckRequired(required) Then Exit Sub

        Dim container = tpOrder

        Try
            Using db As New gas_accounting_systemEntities
                '修改order、car、customer
                Dim ordId As Integer = txto_id.Text
                Dim order = db.orders.Find(ordId)
                Dim carId As Integer = cmbCarNo.SelectedItem.Key
                Dim car = db.cars.Find(carId)
                Dim cusId As Integer = txtCusID_order.Text
                Dim customer = db.customers.Find(cusId)

                AutoMapControlsToEntity(order, container)
                AutoMapControlsToEntity(car, container)
                AutoMapControlsToEntity(customer, container)

                '修改客戶、車輛寄瓶庫存
                If tcInOut.SelectedIndex = 0 Then
                    AutoMapControlsToEntity(order, tpIn)
                    AutoMapControlsToEntity(car, tpIn)
                    AutoMapControlsToEntity(customer, tpIn)
                Else
                    AutoMapControlsToEntity(order, tpOut)
                    AutoMapControlsToEntity(car, tpOut)
                    AutoMapControlsToEntity(customer, tpOut)
                End If

                db.SaveChanges()
            End Using

            btnCancel_order.PerformClick()
            MsgBox("修改成功")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '銷售管理-銷售作業-dgv
    Private Sub dgvOrder_SelectionChanged(sender As Object, e As EventArgs) Handles dgvOrder.SelectionChanged, dgvOrder.CellMouseClick
        If Not sender.focused Then Return

        SetButtonState(sender, False)

        Dim row = dgvOrder.SelectedRows(0)

        Using db As New gas_accounting_systemEntities
            Dim ordId As Integer = row.Cells("編號").Value
            Dim order = db.orders.Include("car").Include("car.customer").FirstOrDefault(Function(o) o.o_id = ordId)

            AutoMapEntityToControls(order, tpOrder)
            AutoMapEntityToControls(order.car.customer, tpOrder)
            AutoMapEntityToControls(order.car, tpOrder)

            If order.o_in_out = "進場單" Then
                tcInOut.SelectedIndex = 0
                tpOut.Parent = Nothing
                tpIn.Parent = tcInOut

                AutoMapEntityToControls(order, tpIn)
                AutoMapEntityToControls(order.car, tpIn)

                Dim txts = tpIn.Controls.OfType(Of TextBox)
                For Each txtBox In txts.Where(Function(x) x.Name.StartsWith("txto_"))
                    Dim key = txtBox.Name
                    Dim value As Integer

                    If Integer.TryParse(txtBox.Text, value) Then
                        orgGasValues(key) = value
                    Else
                        orgGasValues(key) = 0
                    End If
                Next

                For Each txtBox In txts.Where(Function(x) x.Name.StartsWith("txtDepositIn_"))
                    Dim key = txtBox.Name
                    Dim value As Integer

                    If Integer.TryParse(txtBox.Text, value) Then
                        orgDepositValues(key) = value
                    Else
                        orgDepositValues(key) = 0
                    End If
                Next

            Else
                tcInOut.SelectedIndex = 1
                tpOut.Parent = tcInOut
                tpIn.Parent = Nothing

                AutoMapEntityToControls(order, tpOut)

                For Each txtBox In tpOut.Controls.OfType(Of TextBox).Where(Function(x) x.Name.StartsWith("txtGas") Or x.Name.StartsWith("txtEmpty"))
                    Dim key = txtBox.Name
                    Dim value As Integer

                    If Integer.TryParse(txtBox.Text, value) Then
                        orgGasValues(key) = value
                    Else
                        orgGasValues(key) = 0
                    End If
                Next
            End If
        End Using
    End Sub

    '銷售管理-銷售作業-刪除
    Private Sub btnDelete_order_Click(sender As Object, e As EventArgs) Handles btnDelete_order.Click
        Try
            Using db As New gas_accounting_systemEntities
                Dim ordId As Integer = txto_id.Text
                Dim order = db.orders.Find(ordId)
                Dim cusId As Integer = txtCusID_order.Text
                Dim customer = db.customers.Find(cusId)

                Dim carId As Integer
                Dim car As car

                If tcInOut.SelectedIndex = 0 Then
                    customer.cus_gas_50 -= GetOrderValue(order, "50", True)
                    customer.cus_gas_20 -= GetOrderValue(order, "20", True)
                    customer.cus_gas_16 -= GetOrderValue(order, "16", True)
                    customer.cus_gas_10 -= GetOrderValue(order, "10", True)
                    customer.cus_gas_4 -= GetOrderValue(order, "4", True)
                    customer.cus_gas_15 -= GetOrderValue(order, "15", True)
                    customer.cus_gas_14 -= GetOrderValue(order, "14", True)
                    customer.cus_gas_5 -= GetOrderValue(order, "5", True)
                    customer.cus_gas_2 -= GetOrderValue(order, "2", True)

                    '取得寄瓶庫存
                    carId = cmbCarNo.SelectedItem.Key
                    car = db.cars.Find(carId)

                    car.c_deposit_50 -= GetDepositValue(order, "50", True)
                    car.c_deposit_20 -= GetDepositValue(order, "20", True)
                    car.c_deposit_16 -= GetDepositValue(order, "16", True)
                    car.c_deposit_10 -= GetDepositValue(order, "10", True)
                    car.c_deposit_4 -= GetDepositValue(order, "4", True)
                    car.c_deposit_15 -= GetDepositValue(order, "15", True)
                    car.c_deposit_14 -= GetDepositValue(order, "14", True)
                    car.c_deposit_5 -= GetDepositValue(order, "5", True)
                    car.c_deposit_2 -= GetDepositValue(order, "2", True)
                Else
                    customer.cus_gas_50 += GetOrderValue(order, "50", False)
                    customer.cus_gas_20 += GetOrderValue(order, "20", False)
                    customer.cus_gas_16 += GetOrderValue(order, "16", False)
                    customer.cus_gas_10 += GetOrderValue(order, "10", False)
                    customer.cus_gas_4 += GetOrderValue(order, "4", False)
                    customer.cus_gas_15 += GetOrderValue(order, "15", False)
                    customer.cus_gas_14 += GetOrderValue(order, "14", False)
                    customer.cus_gas_5 += GetOrderValue(order, "5", False)
                    customer.cus_gas_2 += GetOrderValue(order, "2", False)

                    '取得寄瓶庫存
                    carId = order.o_deposit_out_c_id
                    car = db.cars.Find(carId)

                    car.c_deposit_50 -= GetDepositValue(order, "50", False)
                    car.c_deposit_20 -= GetDepositValue(order, "20", False)
                    car.c_deposit_16 -= GetDepositValue(order, "16", False)
                    car.c_deposit_10 -= GetDepositValue(order, "10", False)
                    car.c_deposit_4 -= GetDepositValue(order, "4", False)
                    car.c_deposit_15 -= GetDepositValue(order, "15", False)
                    car.c_deposit_14 -= GetDepositValue(order, "14", False)
                    car.c_deposit_5 -= GetDepositValue(order, "5", False)
                    car.c_deposit_2 -= GetDepositValue(order, "2", False)
                End If

                db.orders.Remove(order)
                db.SaveChanges()
            End Using

            btnCancel_order.PerformClick()
            MsgBox("刪除成功")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 取得寄瓶明細
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="group"></param>
    ''' <param name="isIn"></param>
    ''' <returns></returns>
    Private Function GetDepositValue(order As order, group As String, isIn As Boolean) As Integer
        If isIn Then
            Return order.GetType.GetProperties.
                Where(Function(p) p.Name.StartsWith("o_deposit_in_") And p.Name.Contains(group)).
                Sum(Function(x) x.GetValue(order))
        Else
            Return order.GetType.GetProperties.
                Where(Function(p) p.Name.StartsWith("o_deposit_out_") And p.Name.Contains(group)).
                Sum(Function(x) x.GetValue(order))
        End If
    End Function

    ''' <summary>
    ''' 取得訂單明細
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="group"></param>
    ''' <param name="isIn"></param>
    ''' <returns></returns>
    Private Function GetOrderValue(order As order, group As String, isIn As Boolean) As Integer
        If isIn Then
            Return order.GetType.GetProperties.
                Where(Function(p) (p.Name.StartsWith("o_in_") Or p.Name.StartsWith("o_new_in_") Or p.Name.StartsWith("o_inspect_")) And p.Name.Contains(group)).
                Sum(Function(x) x.GetValue(order))
        Else
            Return order.GetType.GetProperties.
                Where(Function(p) (p.Name.StartsWith("o_gas_") Or p.Name.StartsWith("o_gas_c_") Or p.Name.StartsWith("o_empty_")) And p.Name.Contains(group)).
                Sum(Function(x) x.GetValue(order))
        End If
    End Function

    '銷售管理-銷售作業-查詢
    Private Sub btnQuery_order_Click(sender As Object, e As EventArgs) Handles btnQuery_order.Click
        Dim btn As Button = sender
        Dim lst = New List(Of Control) From {
            txtCusID_order,
            lblCarNo
        }

        SetQueryControls(btn, lst)

        If btn.Text = "查  詢" Then
            Try
                Using db As New gas_accounting_systemEntities
                    ' 初始化查詢，包含關聯
                    Dim query = db.orders.Include("car").Include("car.customer")

                    ' 轉換輸入值
                    Dim startDate = dtpStart_order.Value
                    Dim endDate = dtpEnd_order.Value
                    Dim cusId As Integer = If(String.IsNullOrEmpty(txtCusID_order.Text), 0, Convert.ToInt32(txtCusID_order.Text))
                    Dim carId As Integer = If(String.IsNullOrEmpty(cmbCarNo.SelectedValue), 0, Convert.ToInt32(cmbCarNo.SelectedValue))

                    ' 應用過濾條件
                    If cusId > 0 Then
                        query = query.Where(Function(x) x.car.c_cus_id = cusId)
                    End If
                    If carId > 0 Then
                        query = query.Where(Function(x) x.o_c_id = carId)
                    End If
                    query = query.Where(Function(x) x.o_date >= startDate AndAlso x.o_date <= endDate)

                    ' 執行查詢並設置數據源
                    dgvOrder.DataSource = OrderMV.GetOrderList(query)
                End Using

                ClearControls(tpOrder)
            Catch ex As Exception
                MsgBox(ex.Message)
                Console.WriteLine(ex.Message)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' 即時計算客戶瓦斯桶存量
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <param name="isIn">是否為進場</param>
    Private Sub CalculateStock(sender As Object, e As EventArgs, isIn As Boolean)
        Dim txt As TextBox = sender
        Dim groupName As String = GetGroupName(txt.Name)
        Dim container = txt.Parent
        Dim targetTxtBox As TextBox = container.Controls.OfType(Of TextBox).FirstOrDefault(Function(x) x.Tag = "cus_gas_" & groupName)

        If String.IsNullOrEmpty(targetTxtBox.Text) Then Exit Sub

        Dim orgStock As Integer
        If orgStockValues.TryGetValue(targetTxtBox.Tag, orgStock) Then
            Dim sum As Integer = orgStock
            Dim txtValue As Integer = 0

            If isIn Then
                ' 入場單的計算
                For Each txtBox In container.Controls.OfType(Of TextBox).Where(Function(x) (x.Name.StartsWith("txto_")) AndAlso x.Tag.ToString.Contains(groupName))
                    Integer.TryParse(txtBox.Text, txtValue)
                    ' 使用初始值和當前值計算庫存
                    sum += txtValue - If(orgGasValues.TryGetValue(txt.Name, 0), orgGasValues(txt.Name), 0)
                Next

            Else
                ' 出場單的計算
                For Each txtBox In container.Controls.OfType(Of TextBox).
                    Where(Function(x) (x.Tag.ToString.StartsWith("o_gas_") Or x.Tag.ToString.StartsWith("o_empty_")) AndAlso x.Tag.ToString.Contains(groupName))
                    Integer.TryParse(txtBox.Text, txtValue)
                    sum -= txtValue - If(orgGasValues.TryGetValue(txt.Name, 0), orgGasValues(txt.Name), 0)
                Next
            End If

            targetTxtBox.Text = sum.ToString()
        End If
    End Sub

    ''' <summary>
    ''' 即時計算車輛寄桶數量
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <param name="isIn">是否為進場</param>
    Private Sub CalculateDeposit_TextChaged(sender As Object, e As EventArgs, isIn As Boolean)
        Dim txt As TextBox = sender
        Dim groupName As String = GetGroupName(txt.Name)
        Dim container = txt.Parent
        Dim targetTxtBox As TextBox = container.Controls.OfType(Of TextBox).FirstOrDefault(Function(x) x.Tag = "c_deposit_" & groupName)

        If targetTxtBox Is Nothing OrElse String.IsNullOrEmpty(targetTxtBox.Text) Then Exit Sub

        ' 從字典獲取初始庫存值
        Dim orgStock As Integer
        If orgDepositStockValues.TryGetValue(targetTxtBox.Tag, orgStock) Then
            Dim sum As Integer = orgStock

            For Each txtBox In container.Controls.OfType(Of TextBox).Where(Function(t) t.Tag.ToString.StartsWith("o_deposit_") AndAlso t.Tag.ToString.Contains(groupName))
                Dim txtValue As Integer = If(String.IsNullOrEmpty(txtBox.Text), 0, txtBox.Text)

                If isIn Then
                    sum += txtValue - If(orgDepositValues.TryGetValue(txt.Name, 0), orgDepositValues(txt.Name), 0)
                Else
                    sum -= txtValue - If(orgDepositValues.TryGetValue(txt.Tag, 0), orgDepositValues(txt.Tag), 0)
                End If
            Next
            'todo 寄桶出有問題       
            targetTxtBox.Text = sum.ToString()
        End If
    End Sub

    ''' <summary>
    ''' 計算總氣量
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub GasSum(sender As Object, e As EventArgs, gasTxtName As String)
        Dim txtbox As TextBox = sender

        Dim sum As Integer = 0
        Dim reg = New Regex($"^{gasTxtName}(\d+)$")
        tpOut.Controls.OfType(Of TextBox).
            Where(Function(txt) Not String.IsNullOrEmpty(txt.Text) AndAlso reg.IsMatch(txt.Name)).ToList.
            ForEach(Sub(x)
                        '提取txt名稱中的數字
                        Dim match = reg.Match(x.Name)
                        If match.Success AndAlso match.Groups.Count > 1 Then
                            sum += x.Text * match.Groups(1).Value
                        End If
                    End Sub)

        Dim minusTxtBox As TextBox = Nothing
        Dim targetTxtBox As TextBox = Nothing

        Select Case gasTxtName
            Case "txtGas_c_"
                minusTxtBox = txto_return_c
                targetTxtBox = txtSumGas_c
            Case "txtGas_"
                minusTxtBox = txto_return
                targetTxtBox = txtSumGas
            Case Else
                Throw New Exception($"未設定此 {gasTxtName} ,請到 GasSum 設定")
                Exit Sub
        End Select

        Dim returnGas As Integer = 0
        Integer.TryParse(minusTxtBox.Text, returnGas)

        targetTxtBox.Text = sum - returnGas
    End Sub

    Private Function GetGroupName(name As String) As String
        ' 提取名稱中的公斤數
        Dim match As Match = Regex.Match(name, "\d+")
        Return If(match.Success, match.Value, String.Empty)
    End Function

    Private Sub SumTotalPrice(sender As Object, e As EventArgs) Handles txtSumGas.TextChanged, txtSumGas_c.TextChanged, rdoDelivery.CheckedChanged, txto_sales_allowance.TextChanged
        '取得當月價格
        Dim price As Single = 0
        Dim price_c As Single = 0

        Using db As New gas_accounting_systemEntities
            Dim year = dtpo_date.Value.Year
            Dim month = dtpo_date.Value.Month
            Dim bp = db.basic_price.FirstOrDefault(Function(x) x.bp_date.Year = year AndAlso x.bp_date.Month = month)

            If bp Is Nothing Then
                MsgBox("該月份未設定價格,請至 基本資料-基礎價格 設定")
                Exit Sub
            End If

            price = bp.bp_normal_out
            price_c = bp.bp_c_out
        End Using

        '判斷運送方式,更新價格
        If rdoDelivery.Checked Then
            price += 0.5
            price_c += 0.5
        End If

        '計算價格,輸出
        Dim gas As Integer = 0
        Integer.TryParse(txtSumGas.Text, gas)

        Dim gas_c As Integer = 0
        Integer.TryParse(txtSumGas_c.Text, gas_c)

        Dim saleAllowance As Integer = 0
        Integer.TryParse(txto_sales_allowance.Text, saleAllowance)

        txto_total_amount.Text = (gas * price + gas_c * price_c) - saleAllowance
    End Sub

    Public Sub DisplaySubjects(subjects As List(Of SubjectGroupVM)) Implements ISubjectGroupView.DisplaySubjects
        dgvSubjectGroup.DataSource = subjects
    End Sub

    Public Function GetUserInput() As subjects_group Implements ISubjectGroupView.GetUserInput
        Dim req = New List(Of Control) From {
            txtName_sg,
            cmbType_sg
        }
        If Not CheckRequired(req) Then Return Nothing

        Dim sg As New subjects_group
        AutoMapControlsToEntity(sg, grpSG)
        Return sg
    End Function

    Public Sub ClearInputs() Implements ISubjectGroupView.ClearInputs
        ClearControls(grpSG)
    End Sub

    Public Sub SetSubject(subject As subjects_group) Implements ISubjectGroupView.SetSubject
        AutoMapEntityToControls(subject, grpSG)
    End Sub

    '會計管理-科目管理-科目分類-取消
    Private Sub btnCancel_sg_Click(sender As Object, e As EventArgs) Handles btnCancel_sg.Click
        ClearInputs()
        _sgPresenter.LoadList()
        SetButtonState(sender, True)

        '未選擇群組,子科目不能使用
        ISubjectsView_ClearInputs()
        dgvSubject.DataSource = Nothing
        grpSubjects.Enabled = False
    End Sub

    '會計管理-科目管理-科目分類-新增
    Private Sub btnAdd_sg_Click(sender As Object, e As EventArgs) Handles btnAdd_sg.Click
        Dim add As subjects_group = GetUserInput()
        If add IsNot Nothing Then _sgPresenter.Add(add)
    End Sub

    '會計管理-科目管理-科目分類-dgv
    Private Sub dgvSubjectGroup_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSubjectGroup.SelectionChanged, dgvSubjectGroup.CellMouseClick
        If Not sender.focused Then Return
        SetButtonState(sender, False)
        Dim id As Integer = dgvSubjectGroup.SelectedRows(0).Cells("編號").Value
        _sgPresenter.SelectRow(id)

        '選擇一個群組後,子科目可以使用,並顯示
        btnCancel_subjects_Click(btnCancel_subjects, EventArgs.Empty)
        grpSubjects.Enabled = True
    End Sub

    '會計管理-科目管理-科目分類-修改
    Private Sub btnEdit_sg_Click(sender As Object, e As EventArgs) Handles btnEdit_sg.Click
        Dim id As Integer = txtId_sg.Text
        Dim edit = GetUserInput()
        If edit IsNot Nothing Then
            edit.sg_id = id
            _sgPresenter.Edit(edit)
        End If
    End Sub

    '會計管理-科目管理-科目分類-刪除
    Private Sub btnDelete_sg_Click(sender As Object, e As EventArgs) Handles btnDelete_sg.Click
        Dim id As Integer = txtId_sg.Text

        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.Yes Then
            btnCancel_subjects_Click(btnCancel_subjects, EventArgs.Empty)
            dgvSubject.DataSource = Nothing
            grpSubjects.Enabled = False
            _sgPresenter.Delete(id)
        End If
    End Sub

    Public Sub DisplaySubjects(subjects As List(Of SubjectsVM)) Implements ISubjectsView.DisplaySubjects
        dgvSubject.DataSource = subjects
    End Sub

    Private Function ISubjectsView_GetUserInput() As subject Implements ISubjectsView.GetUserInput
        Dim req = New List(Of Control) From {txtName_subjects}
        If Not CheckRequired(req) Then Return Nothing
        Dim data As New subject
        AutoMapControlsToEntity(data, grpSubjects)
        data.s_sg_id = txtId_sg.Text
        Return data
    End Function

    Private Sub ISubjectsView_ClearInputs() Implements ISubjectsView.ClearInputs
        ClearControls(grpSubjects)
    End Sub

    Public Sub SetSebject(subjects As subject) Implements ISubjectsView.SetSebject
        AutoMapEntityToControls(subjects, grpSubjects)
    End Sub

    '會計管理-科目管理-子科目-取消
    Private Sub btnCancel_subjects_Click(sender As Object, e As EventArgs) Handles btnCancel_subjects.Click
        ISubjectsView_ClearInputs()
        _subjectsPresenter.LoadList(txtId_sg.Text)
        SetButtonState(sender, True)
    End Sub

    '會計管理-科目管理-子科目-新增
    Private Sub btnAdd_subjects_Click(sender As Object, e As EventArgs) Handles btnAdd_subjects.Click
        Dim add As subject = ISubjectsView_GetUserInput()
        If add IsNot Nothing Then _subjectsPresenter.Add(add)
    End Sub

    '會計管理-科目管理-子科目-dgv
    Private Sub dgvSubject_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSubject.SelectionChanged, dgvSubject.CellMouseClick
        If Not sender.focused Then Return
        ISubjectsView_ClearInputs()
        SetButtonState(sender, False)
        Dim id As Integer = dgvSubject.SelectedRows(0).Cells("編號").Value
        _subjectsPresenter.SelectRow(id)
    End Sub

    '會計管理-科目管理-子科目-修改
    Private Sub btnEdit_subjects_Click(sender As Object, e As EventArgs) Handles btnEdit_subjects.Click
        Dim id As Integer = txtId_subjects.Text
        Dim data As subject = ISubjectsView_GetUserInput()

        If data IsNot Nothing Then
            data.s_sg_id = txtId_sg.Text
            _subjectsPresenter.Edit(data)
        End If
    End Sub

    '會計管理-科目管理-子科目-刪除
    Private Sub btnDelete_subjects_Click(sender As Object, e As EventArgs) Handles btnDelete_subjects.Click
        Dim id As Integer = txtId_subjects.Text

        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.Yes Then
            _subjectsPresenter.Delete(id)
        End If
    End Sub
End Class
