Imports System.Text.RegularExpressions

Public Class frmMain
    Implements ISubjectsView, ICompanyView, IManufacturerView, IPurchaseView, ICustomer, IPricePlanView, IEmployeeView, IBankView, IPaymentView, IUnitPriceHistoryView,
        ICollectionView, ICheque, IOrderView, IReportView, IGasCheckoutView, IPermissionView

    Public Structure UserData
        Public Id As Integer
        Public Name As String
    End Structure

    Public User As UserData

    Private _compService As ICompanyService = New CompanyService
    Private _manuService As IManufacturerService = New ManufacturerService
    Private _bankService As IBankService = New BankService
    Private _subjectService As ISubjectsService = New SubjectsService
    Private _subjectRep As ISubjectRep

    Private _cheque As ChequePresenter
    Private _permission As PermissionPresenter
    Private _subjects As New SubjectsPresenter(Me)
    Private _company As New CompanyPresenter(Me)
    Private _manufacturer As New ManufacturerPresenter(Me)
    Private _purchase As PurchasePresenter
    Private _customer As New CustomerPresenter(Me)
    Private _pricePlan As New PricePlanPresenter(Me)
    Private _employee As New EmployeePresenter(Me)
    Private _bank As New BankPresenter(Me)
    Private _payment As PaymentPresenter
    Private _uph As New UintPriceHistoryPresenter(Me, _manuService)
    Private _collect As CollectionPresenter
    Private _order As OrderPresenter
    Private _report As ReportPresenter
    Private _gasCheckout As GasCheckoutPresenter

    Private inputTxts As String(,) = {
        {"txto_in_50", "txto_in_20", "txto_in_16", "txto_in_10", "txto_in_4", "txto_in_15", "txto_in_14", "txto_in_5", "txto_in_2"},
        {"txto_new_in_50", "txto_new_in_20", "txto_new_in_16", "txto_new_in_10", "txto_new_in_4", "txto_new_in_15", "txto_new_in_14", "txto_new_in_5", "txto_new_in_2"},
        {"txto_inspect_50", "txto_inspect_20", "txto_inspect_16", "txto_inspect_10", "txto_inspect_4", "txto_inspect_15", "txto_inspect_14", "txto_inspect_5", "txto_inspect_2"},
        {"txtDepositIn_50", "txtDepositIn_20", "txtDepositIn_16", "txtDepositIn_10", "txtDepositIn_4", "txtDepositIn_15", "txtDepositIn_14", "txtDepositIn_5", "txtDepositIn_2"}
    }
    Private outputTxts As String(,) = {
        {"txtGas_c_50", "txtGas_c_20", "txtGas_c_16", "txtGas_c_10", "txtGas_c_4", "txtGas_c_15", "txtGas_c_14", "txtGas_c_5", "txtGas_c_2"},
        {"txtGas_50", "txtGas_20", "txtGas_16", "txtGas_10", "txtGas_4", "txtGas_15", "txtGas_14", "txtGas_5", "txtGas_2"},
        {"txtEmpty_50", "txtEmpty_20", "txtEmpty_16", "txtEmpty_10", "txtEmpty_4", "txtEmpty_15", "txtEmpty_14", "txtEmpty_5", "txtEmpty_2"},
        {"txtDepositOut_50", "txtDepositOut_20", "txtDepositOut_16", "txtDepositOut_10", "txtDepositOut_4", "txtDepositOut_15", "txtDepositOut_14", "txtDepositOut_5", "txtDepositOut_2"}
    }

    Private _currentPurchase As purchase

    Public Property CurrentPurchase As purchase Implements IPurchaseView.CurrentPurchase
        Get
            Return _currentPurchase
        End Get
        Set(value As purchase)
            _currentPurchase = value
        End Set
    End Property

    Private Event QueryClicked() Implements IGasCheckoutView.QueryClicked
    Private Event CheckoutClicked() Implements IGasCheckoutView.CheckoutClicked
    Private Event CancelClicked() Implements IGasCheckoutView.CancelClicked

    Public Sub New(user As employee, permissions As List(Of String))
        InitializeComponent()

        Me.User = New UserData With {
            .Id = user.emp_id,
            .Name = user.emp_name
        }

        InitializeTabPages(permissions)

        Dim context As New gas_accounting_systemEntities

        Dim bankRep As New BankRep(context)
        Dim carRep As New CarRepository(context)
        Dim cheRep As New ChequeRep(context)
        Dim compRep As New CompanyRep(context)
        Dim cusRep As New CustomerRepository(context)
        Dim ordRep As New OrderRepository(context)
        Dim bpRep As New BasicPriceRep(context)
        Dim permissionRep As New PermissionRep(context)
        Dim ppRep As New PricePlanRep(context)
        Dim reportRep As New ReportRep(context)
        Dim purRep As New PurchaseRep(context)
        Dim manuRep As New ManufacturerRep(context)
        Dim subjectRep As New SubjectRep(context)

        Dim service As ICusOrdByCarService = New CusOrdByCarService(cusRep, ordRep, carRep, bpRep, context)
        _order = New OrderPresenter(Me, service)
        _report = New ReportPresenter(Me, reportRep)
        _subjectRep = subjectRep

        _cheque = New ChequePresenter(Me, cheRep)
        _permission = New PermissionPresenter(Me, permissionRep)
        _purchase = New PurchasePresenter(Me, purRep, compRep, manuRep, subjectRep)
        _gasCheckout = New GasCheckoutPresenter(Me, purRep, manuRep)
        _payment = New PaymentPresenter(Me, _manuService, _bankService, _subjectService, _compService, bankRep)
        _collect = New CollectionPresenter(Me, _subjectService, _compService, bankRep, cusRep)
    End Sub

    Private Sub InitializeTabPages(permissions As List(Of String))
        For Each tabPage As TabPage In TabControl1.TabPages
            If Not permissions.Contains(tabPage.Name) AndAlso tabPage.Name <> "tpLogOut" Then
                tabPage.Parent = Nothing
            End If
        Next
    End Sub

    Private Sub Direction(sender As Object, e As KeyEventArgs, container As Control)
        Dim btn As TextBox = sender
        Dim txtName = btn.Name
        Dim currentI As Integer
        Dim currentJ As Integer
        Dim nextI As Integer
        Dim nextJ As Integer
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

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitTabPage()
        InitUI()
        SetCtrlStyle()
    End Sub

    Private Sub frmMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        frmLogin.Show()
    End Sub

    Private Sub InitUI()
        SetSheetColor()
        SetDataGridViewStyle(Me)
        SetDGVColumnWidthSave()
    End Sub

    Private Sub SetCtrlStyle()
        SetQueryEnterEven(Me)
        SetTextBoxIntOnly()
        PositiveIntegerOnly()
        PositiveFloatOnly()
        SetFloatOnly()
        TextChagedHandler()
    End Sub

    Private Sub SetFloatOnly()
        Dim textBoxes = New List(Of TextBox) From {txtGasPrice, txtGasPrice_c, txtGasPriceDelivery, txtGasPriceDelivery_c}

        textBoxes.ForEach(Sub(txt) AddHandler txt.KeyPress, AddressOf FloatOnly_TextBox)
    End Sub

    ''' <summary>
    ''' 初始化各TabPage
    ''' </summary>
    Private Sub InitTabPage()
        btnCancel_emp_Click(btnCancel_emp, EventArgs.Empty)
        btnCancel_cus_Click(btnCancel_cus, EventArgs.Empty)
        btnCancel_bp_Click(btnCancel_bp, EventArgs.Empty)
        btnCancel_bank_Click(btnCancel_bank, EventArgs.Empty)
        btnCancel_order_Click(btnCancel_order, EventArgs.Empty)
        btnCancel_car_Click(btnCancel_car, EventArgs.Empty)
        btnCancel_subjects_Click(btnCancel_subjects, EventArgs.Empty)
        btnCancel_comp_Click(btnCancel_comp, EventArgs.Empty)
        btnCancel_manu_Click(btnCancel_manu, EventArgs.Empty)
        btnCancel_pur_Click(btnCancel_pur, EventArgs.Empty)
        btnCancel_pp_Click(btnCancel_pp, EventArgs.Empty)
        btnCancel_roles_Click(btnCancel_roles, EventArgs.Empty)
        btnCancel_payment_Click(btnCancel_payment, EventArgs.Empty)
        btnCancel_uph_Click(btnCancel_uph, EventArgs.Empty)
        btnCancel_Che_Click(btnCancel_Che, EventArgs.Empty)
        ICollectionView_Reset()
        _report.GetManuCmb()
        btnCancel_gc_Click(btnCancel_gc, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' 設定TextBox值改變事件
    ''' </summary>
    Private Sub TextChagedHandler()
#Region "銷貨管理"
        Dim inTxts = tpIn.Controls.OfType(Of TextBox)
        inTxts.Where(Function(txt) txt.Name.StartsWith("txto_in") Or txt.Name.StartsWith("txto_new_in")).ToList.ForEach(Sub(t) AddHandler t.KeyUp, Sub(sender, e) CalculateStock(sender, e, True))
        inTxts.Where(Function(txt) txt.Name.StartsWith("txtDepositIn_")).ToList.ForEach(Sub(t) AddHandler t.KeyUp, Sub(sender, e) CalculateDeposit_TextChaged(sender, e, True))
        inTxts.Where(Function(txt) txt.Name.StartsWith("txto_inspect")).ToList.ForEach(Sub(t) AddHandler t.KeyUp, Sub(sendor, e) CalculateInspect_TextChaged(sendor, e, True))
        inTxts.Where(Function(x) x.ReadOnly = False).ToList.ForEach(Sub(x) AddHandler x.KeyDown, Sub(sender, e) Direction(sender, e, tpIn))

        Dim outTxts = tpOut.Controls.OfType(Of TextBox)
        outTxts.Where(Function(txt) txt.Name.StartsWith("txtGas") Or txt.Name.StartsWith("txtEmpty")).
            ToList.ForEach(Sub(t) AddHandler t.KeyUp, Sub(sender, e) CalculateStock(sender, e, False))
        outTxts.Where(Function(txt) txt.Name.StartsWith("txtDepositOut_")).ToList.ForEach(Sub(t) AddHandler t.KeyUp, Sub(sender, e) CalculateDeposit_TextChaged(sender, e, False))
        outTxts.Where(Function(x) x.ReadOnly = False).ToList.ForEach(Sub(x) AddHandler x.KeyDown, Sub(sender, e) Direction(sender, e, tpOut))

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
#End Region

#Region "大氣採購"
        Dim lstFreight As New List(Of TextBox) From {txtWeight_pur, txtDeliUnitPrice}
        lstFreight.ForEach(Sub(x) AddHandler x.TextChanged, Sub(sender, e) CalculateFreight())

        Dim lstSum_pur As New List(Of TextBox) From {txtWeight_pur, txtUnitPrice_pur, txtFreight}
        lstSum_pur.ForEach(Sub(x) AddHandler x.TextChanged, Sub(sender, e) CalculateSum_Purchase())
#End Region
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
            txto_return, txto_return_c, txto_sales_allowance, txtWeight_pur, txtAmount}

        textBoxes.ForEach(Sub(txt) AddHandler txt.KeyPress, AddressOf PositiveIntegerOnly_TextBox)
    End Sub

    ''' <summary>
    ''' 設定TextBox只能輸入正浮點數
    ''' </summary>
    Private Sub PositiveFloatOnly()
        Dim txts = New List(Of TextBox) From {txtUnitPrice_pur, txtDeliUnitPrice, txtInitGasStock}

        txts.ForEach(Sub(x) AddHandler x.KeyPress, AddressOf PositiveFloatOnly_TextBox)
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

    Public Sub ShowList_Customer(data As List(Of CustomerVM)) Implements ICommonView(Of customer, CustomerVM).ShowList
        dgvCustomer.DataSource = data
    End Sub

    Public Sub SetDataToControl_Customer(data As customer) Implements ICommonView(Of customer, CustomerVM).SetDataToControl
        AutoMapEntityToControls(data, tpCustomer)
        AutoMapEntityToControls(data, grpStock)
        AutoMapEntityToControls(data, grpPricePlan)
        AutoMapEntityToControls(data, grpCusStk)
    End Sub

    Private Function GetUserInput_Customer() As customer Implements ICommonView(Of customer, CustomerVM).GetUserInput
        Dim data As New customer
        AutoMapControlsToEntity(data, tpCustomer)
        AutoMapControlsToEntity(data, grpStock)
        AutoMapControlsToEntity(data, grpPricePlan)
        AutoMapControlsToEntity(data, grpCusStk)
        Return data
    End Function

    Private Sub ClearInput_Customer() Implements ICommonView(Of customer, CustomerVM).ClearInput
        ClearControls(tpCustomer)
    End Sub

    Private Function GetSearchConditions_Customer() As customer Implements ICustomer.GetSearchConditions
        Return New customer With {
            .cus_code = txtCusCode.Text,
            .cus_name = txtCusName_cus.Text,
            .cus_phone1 = txtCusPhone1.Text
        }
    End Function

    Public Sub SetPricePlan_Cmb(data As List(Of ComboBoxItems)) Implements ICustomer.SetPricePlan_Cmb
        SetComboBox(cmbPricePlan, data)
    End Sub

    Public Sub SetPricePlanDetails(data As priceplan) Implements ICustomer.SetPricePlanDetails
        AutoMapEntityToControls(data, grpPricePlan)
    End Sub

    Private Function SetRequired_customer() As List(Of Control) Implements ICommonView(Of customer, CustomerVM).SetRequired
        Return New List(Of Control) From {txtCusCode, txtCusName_cus, txtCusContactPerson, txtCusPhone1, txtTaxId_cus}
    End Function

    '基本資料-客戶管理-取消
    Private Sub btnCancel_cus_Click(sender As Object, e As EventArgs) Handles btnCancel_cus.Click
        SetButtonState(sender, True)
        If btnQuery_cus.Text = "確  認" Then SetOrderQueryCtrl(btnQuery_cus)
        _customer.LoadList()
        _customer.PricePlan_Cmb()
        ClearInput_Customer()
    End Sub

    '基本資料-客戶管理-新增
    Private Sub btnAdd_cus_Click(sender As Object, e As EventArgs) Handles btnCreate_cus.Click
        _customer.Add()
    End Sub

    '基本資料-客戶管理-dgv
    Private Sub dgvCustomer_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCustomer.SelectionChanged, dgvCustomer.CellClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return

        SetButtonState(ctrl, False)

        Dim id As Integer = ctrl.SelectedRows(0).Cells(0).Value
        _customer.SelectRow(id)
    End Sub

    '基本資料-客戶管理-修改
    Private Sub btnEdit_cus_Click(sender As Object, e As EventArgs) Handles btnEdit_cus.Click
        Dim id As Integer = txtcus_id.Text
        _customer.Edit(id)
    End Sub

    '基本資料-客戶管理-刪除
    Private Sub btnDelete_cus_Click(sender As Object, e As EventArgs) Handles btnDelete_cus.Click
        Dim id As Integer = txtcus_id.Text
        _customer.Delete(id)
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
            _customer.LoadList(GetSearchConditions_Customer)
        End If
    End Sub

    '基本資料-客戶管理-價格方案
    Private Sub cmbPricePlan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPricePlan.SelectedIndexChanged
        Dim cmb As ComboBox = sender

        If cmb.SelectedIndex >= 0 Then
            Dim id As Integer

            If Integer.TryParse(cmb.SelectedValue.ToString, id) Then
                _customer.GetPricePlanDetails(id)
            End If
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
        Dim required = New List(Of Control) From {txtCusCode_car, txtCarNo}
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
        Dim required = New List(Of Control) From {txtCusCode_car, txtCarNo}
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
                txtCusCode_car.Text = searchForm.CusCode
                txtCusName_car.Text = searchForm.CusName
                txtCusId_car.Text = searchForm.CusId
            End If
        End Using
    End Sub

    '基本資料-車輛管理-查詢
    Private Sub btnQuery_car_Click(sender As Object, e As EventArgs) Handles btnQuery_car.Click
        Dim btn As Button = sender
        Dim lst = New List(Of Control) From {
            txtCusCode_car,
            txtCarNo
        }

        SetQueryControls(btn, lst)

        If btn.Text = "查  詢" Then
            Try
                Using db As New gas_accounting_systemEntities
                    Dim query = db.cars.AsQueryable
                    If Not String.IsNullOrEmpty(txtCusCode_car.Text) Then query = query.Where(Function(x) x.c_cus_id = txtCusCode_car.Text)
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

    '基本資料-基礎價格-客戶輸入
    Private Sub txtCusCode_car_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_car.KeyDown
        '按下Enter時,搜尋客戶資料
        If e.KeyCode = Keys.Enter Then
            Dim cus = _order.GetCusDataByCusCode(txtCusCode_car.Text)
            btnCancel_car.PerformClick()

            If cus IsNot Nothing Then
                txtCusCode_car.Text = cus.cus_code
                txtCusName_car.Text = cus.cus_name
                txtCusId_car.Text = cus.cus_id
            Else
                MsgBox("查無此客戶")
            End If
        End If
    End Sub

    Private Function ISubjectsView_GetUserInput() As subject Implements ISubjectsView.GetUserInput
        Dim data As New subject
        AutoMapControlsToEntity(data, tpSubjects)
        Return data
    End Function

    Public Sub ShowList(data As List(Of SubjectsVM)) Implements ICommonView(Of subject, SubjectsVM).ShowList
        dgvSubject.DataSource = data
    End Sub

    Public Sub SetDataToControl(data As subject) Implements ICommonView(Of subject, SubjectsVM).SetDataToControl
        AutoMapEntityToControls(data, tpSubjects)
    End Sub

    Private Sub ClearInput_subjects() Implements ICommonView(Of subject, SubjectsVM).ClearInput
        ClearControls(tpSubjects)
    End Sub

    Private Function SetRequired_subjects() As List(Of Control) Implements ICommonView(Of subject, SubjectsVM).SetRequired
        Return New List(Of Control) From {txtName_subjects}
    End Function

    '基本資料-科目管理-取消
    Private Sub btnCancel_subjects_Click(sender As Object, e As EventArgs) Handles btnCancel_subjects.Click
        ClearInput_subjects()
        SetButtonState(sender, True)
        _subjects.LoadList()
    End Sub

    '基本資料-科目管理-子科目-新增
    Private Sub btnAdd_subjects_Click(sender As Object, e As EventArgs) Handles btnAdd_subjects.Click
        _subjects.Add()
    End Sub

    '基本資料-科目管理-dgv
    Private Sub dgvSubject_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSubject.SelectionChanged, dgvSubject.CellMouseClick
        If Not sender.focused Then Return
        SetButtonState(sender, False)
        Dim id As Integer = dgvSubject.SelectedRows(0).Cells("編號").Value
        _subjects.SelectRow(id)
    End Sub

    '基本資料-科目管理-子科目-修改
    Private Sub btnEdit_subjects_Click(sender As Object, e As EventArgs) Handles btnEdit_subjects.Click
        Dim id As Integer = txtId_subjects.Text
        _subjects.Edit(id)
    End Sub

    '基本資料-科目管理-子科目-刪除
    Private Sub btnDelete_subjects_Click(sender As Object, e As EventArgs) Handles btnDelete_subjects.Click
        Dim id As Integer = txtId_subjects.Text
        _subjects.Delete(id)
    End Sub

    Private Sub IPurchaseView_GetUserInput() Implements IPurchaseView.GetUserInput
        If _currentPurchase Is Nothing Then
            _currentPurchase = New purchase
        End If

        AutoMapControlsToEntity(_currentPurchase, tpPurchase)
    End Sub

    Public Function GetSearchCondition() As PurchaseCondition Implements IPurchaseView.GetSearchCondition
        Dim data = New PurchaseCondition With {
            .CompanyId = cmbCompany_pur.SelectedItem?.Value,
            .ManufacturerId = cmbGasVendor_pur.SelectedItem?.Value,
            .PayType = cmbPayType_pur.SelectedItem,
            .Product = cmbProduct_pur.SelectedItem,
            .StartDate = dtpStartDate_pur.Value.Date,
            .EndDate = dtpEndDate_pur.Value.Date,
            .IsDateSearch = chkDateRange_pur.Checked
        }

        Return data
    End Function

    Public Sub IPurchaseView_SetCompanyCmb(items As List(Of ComboBoxItems)) Implements IPurchaseView.SetCompanyCmb
        SetComboBox(cmbCompany_pur, items)
    End Sub

    Public Sub IPurchaseView_SetGasVendorCmb(items As List(Of ComboBoxItems)) Implements IPurchaseView.SetGasVendorCmb
        SetComboBox(cmbGasVendor_pur, items)
    End Sub

    Public Sub IPurchaseView_SetDriveVendorCmb(items As List(Of ComboBoxItems)) Implements IPurchaseView.SetDriveVendorCmb
        SetComboBox(cmbDriveCmp, items)
    End Sub

    Public Sub IPurchaseView_SetSubjectCmb(items As List(Of ComboBoxItems)) Implements IPurchaseView.SetSubjectCmb
        SetComboBox(cmbSubject, items)
    End Sub

    Public Sub SetDefaultPrice(unitPrice As Single, DeliveryUnitPrice As Single) Implements IPurchaseView.SetDefaultPrice
        txtUnitPrice_pur.Text = unitPrice
        txtDeliUnitPrice.Text = DeliveryUnitPrice
    End Sub

    Public Sub IPurchase_ShowList(datas As List(Of PurchaseVM)) Implements IPurchaseView.ShowList
        dgvPurchase.DataSource = datas
    End Sub

    Private Sub IPurchaseView_ClearInput() Implements IPurchaseView.ClearInput
        ClearControls(tpPurchase)
        _currentPurchase = Nothing
    End Sub

    Public Sub SetDataToControls(data As purchase) Implements IPurchaseView.SetDataToControls
        AutoMapEntityToControls(data, tpPurchase)
    End Sub

    '大氣採購-大氣採購-取消
    Private Async Sub btnCancel_pur_Click(sender As Object, e As EventArgs) Handles btnCancel_pur.Click
        SetButtonState(sender, True)
        ClearControls(tpPurchase)
        Await _purchase.InitializeAsync
        If btnQuery_pur.Text = "確  認" Then SetPurchaseQueryCtrlsState()
        Await _purchase.LoadListAsync()
    End Sub

    '大氣採購-大氣採購-新增
    Private Async Sub btnAdd_pur_Click(sender As Object, e As EventArgs) Handles btnAdd_pur.Click
        Await _purchase.AddAsync()
    End Sub

    '大氣採購-大氣採購-dgv
    Private Async Sub dgvPurchase_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPurchase.SelectionChanged, dgvPurchase.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return

        SetButtonState(ctrl, False)

        Dim id As Integer = ctrl.SelectedRows(0).Cells("編號").Value
        Await _purchase.SelectRowAsync(id)
    End Sub

    '大氣採購-大氣採購-修改
    Private Async Sub btnEdit_pur_Click(sender As Object, e As EventArgs) Handles btnEdit_pur.Click
        Await _purchase.EditAsync()
    End Sub

    '大氣採購-大氣採購-刪除
    Private Async Sub btnDelete_pur_Click(sender As Object, e As EventArgs) Handles btnDelete_pur.Click
        Await _purchase.DeleteAsync()
    End Sub

    '大氣採購-大氣採購-查詢
    Private Async Sub btnQuery_pur_Click(sender As Object, e As EventArgs) Handles btnQuery_pur.Click
        SetPurchaseQueryCtrlsState()

        If sender.Text = "查  詢" Then
            Await _purchase.LoadListAsync()
        End If
    End Sub

    '大氣採購-大氣採購-選擇大氣廠商、產品
    Private Async Sub GetLastUnitPrice(sender As Object, e As EventArgs) Handles cmbGasVendor_pur.SelectionChangeCommitted, cmbProduct_pur.SelectionChangeCommitted
        If cmbGasVendor_pur.SelectedIndex > -1 AndAlso cmbProduct_pur.SelectedIndex > -1 Then
            Await _purchase.GetDefaultPriceAsync(cmbGasVendor_pur.SelectedItem.Value, cmbProduct_pur.SelectedItem)
        End If
    End Sub

    '大氣採購-大氣採購-列印
    Private Sub btnPrint_pur_Click(sender As Object, e As EventArgs) Handles btnPrint_pur.Click

    End Sub

    ''' <summary>
    ''' 設定大氣採購查詢控制項狀態
    ''' </summary>
    Private Sub SetPurchaseQueryCtrlsState()
        Dim lst As New List(Of Control) From {lblCompany_pur, lblGasVendor_pur, lblProduct, lblPayType_pur, grpDateRange_pur}
        SetQueryControls(btnQuery_pur, lst)
    End Sub

    ''' <summary>
    ''' 計算運費
    ''' </summary>
    Private Sub CalculateFreight()
        Dim unitPrice As Single = If(String.IsNullOrEmpty(txtDeliUnitPrice.Text), 0, txtDeliUnitPrice.Text)
        Dim weight As Integer = If(String.IsNullOrEmpty(txtWeight_pur.Text), 0, txtWeight_pur.Text)
        txtFreight.Text = unitPrice * weight
    End Sub

    ''' <summary>
    ''' 計算大氣採購總金額
    ''' </summary>
    Private Sub CalculateSum_Purchase()
        Dim weight As Integer = If(String.IsNullOrEmpty(txtWeight_pur.Text), 0, txtWeight_pur.Text)
        Dim unitPrice As Single = If(String.IsNullOrEmpty(txtUnitPrice_pur.Text), 0, txtUnitPrice_pur.Text)
        Dim Frieght As Single = If(String.IsNullOrEmpty(txtFreight.Text), 0, txtFreight.Text)
        txtSum_pur.Text = weight * unitPrice + Frieght
    End Sub

    Public Sub SetManuCmb_uph(data As List(Of ComboBoxItems)) Implements IUnitPriceHistoryView.SetManuCmb
        SetComboBox(cmbManu_uph, data)
    End Sub

    Public Sub LoadList(data As List(Of UnitPriceHistoryVM)) Implements IUnitPriceHistoryView.LoadList
        dgvUPH.DataSource = data
    End Sub

    '大氣採購-歷史單價查詢-取消
    Private Sub btnCancel_uph_Click(sender As Object, e As EventArgs) Handles btnCancel_uph.Click
        ClearControls(tpUPH)
        dgvUPH.DataSource = Nothing
        _uph.GetManuCmb()
    End Sub

    '大氣採購-歷史單價查詢-查詢
    Private Sub btnQuery_UPH_Click(sender As Object, e As EventArgs) Handles btnQuery_UPH.Click
        Dim manuId As Integer = If(cmbManu_uph.SelectedItem Is Nothing, 0, cmbManu_uph.SelectedItem.Value)
        _uph.Query(manuId, cmbProduct_UPH.Text, dtpStart_UPH.Value.Date, dtpEnd_UPH.Value.Date)
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

    'Public Sub ShowList(list As List(Of OrderVM)) Implements IOrderView.ShowList
    '    dgvOrder.DataSource = list
    'End Sub

    Public Sub ShowCusStk(data As customer) Implements IOrderView.ShowCusStk
        AutoMapEntityToControls(data, tpOut)
        AutoMapEntityToControls(data, tpIn)
    End Sub

    Public Sub SetCmbCar(list As List(Of ComboBoxItems)) Implements IOrderView.SetCmbCar
        SetComboBox(cmbCarNo, New List(Of ComboBoxItems)(list))
        SetComboBox(cmbo_deposit_out_c_id, New List(Of ComboBoxItems)(list))
    End Sub

    Public Sub GetUserInput_ord(order As order) Implements IOrderView.GetUserInput_ord
        Dim required = New List(Of Control) From {txtCusID_order, cmbCarNo}
        If Not CheckRequired(required) Then Exit Sub

        AutoMapControlsToEntity(order, tpOrder)
        AutoMapControlsToEntity(order, tpOut)
        AutoMapControlsToEntity(order, tpIn)
        order.o_Operator = User.Id
    End Sub

    Public Function GetUserInput_cus(data As customer, container As Control) As customer Implements IOrderView.GetUserInput_cus
        AutoMapControlsToEntity(data, container)
        Return data
    End Function

    Public Function GetUserInput_car(data As car, container As Control) As car Implements IOrderView.GetUserInput_car
        AutoMapControlsToEntity(data, container)
        Return data
    End Function

    Public Sub Reset() Implements IOrderView.Reset
        Dim exception = New List(Of String) From {grpTransport.Name, grpSearch_ord.Name}
        ClearControls(tpOrder, exception)

        tpOut.Parent = tcInOut
        tpIn.Parent = tcInOut
        txtOperator.Text = User.Name
        cmbCarNo.DataSource = Nothing
        cmbo_deposit_out_c_id.DataSource = Nothing

        If btnQuery_order.Text = "確  認" Then SetOrderQueryCtrl(btnQuery_order)
        SetButtonState(btnCancel_order, True)

        txtCusCode_ord.Focus()

        '預設運送方式
        grpTransport.Controls.OfType(Of RadioButton).First(Function(x) x.Text = "自運").Checked = True
    End Sub

    Public Sub ShowDetails(data As order) Implements IOrderView.ShowDetails
        AutoMapEntityToControls(data, tpOrder)
        AutoMapEntityToControls(data.car.customer, tpOrder)
        AutoMapEntityToControls(data.car, tpOrder)
        txtOperator.Text = data.employee.emp_name
        cmbCarNo.SelectedValue = data.o_c_id

        If data.o_in_out = "進場單" Then
            tpOut.Parent = Nothing
            tpIn.Parent = tcInOut

            AutoMapEntityToControls(data, tpIn)
            Dim txts = tpIn.Controls.OfType(Of TextBox)

            For Each txtBox In txts.Where(Function(x) x.Name.StartsWith("txto_"))
                Dim key = txtBox.Name
                Dim value As Integer

                If Integer.TryParse(txtBox.Text, value) Then
                    _order.GasValues(key) = value
                Else
                    _order.GasValues(key) = 0
                End If
            Next

            For Each txtBox In txts.Where(Function(x) x.Name.StartsWith("txtDepositIn_"))
                Dim key = txtBox.Name
                Dim value As Integer

                If Integer.TryParse(txtBox.Text, value) Then
                    _order.DepositValues(key) = value
                Else
                    _order.DepositValues(key) = 0
                End If
            Next


        Else
            tpOut.Parent = tcInOut
            tpIn.Parent = Nothing

            AutoMapEntityToControls(data, tpOut)

            For Each txtBox In tpOut.Controls.OfType(Of TextBox).Where(Function(x) x.Name.StartsWith("txtGas") Or x.Name.StartsWith("txtEmpty"))
                Dim key = txtBox.Name
                Dim value As Integer

                If Integer.TryParse(txtBox.Text, value) Then
                    _order.GasValues(key) = value
                Else
                    _order.GasValues(key) = 0
                End If
            Next
        End If
    End Sub

    Public Sub ShowCarStk(data As car) Implements IOrderView.ShowCarStk
        AutoMapEntityToControls(data, tpOut)
        AutoMapEntityToControls(data, tpIn)
    End Sub

    Public Sub ShowOrderList(orders As IEnumerable(Of Object)) Implements IOrderView.ShowOrderList
        dgvOrder.DataSource = orders
    End Sub

    '銷售管理-取消
    Private Sub btnCancel_order_Click(sender As Object, e As EventArgs) Handles btnCancel_order.Click
        Reset()
        _order.SearchOrders()
    End Sub

    '銷售管理-搜尋客戶
    Private Sub btnQueryCus_ord_Click(sender As Object, e As EventArgs) Handles btnQueryCus_ord.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusID_order.Text = searchForm.CusId
                txtCusName.Text = searchForm.CusName
                txtCusCode_ord.Text = searchForm.CusCode
                cmbCarNo.Text = ""
            End If
        End Using
    End Sub

    '銷售管理-客戶編號改變時
    Private Sub txtCusID_order_TextChanged(sender As Object, e As EventArgs) Handles txtCusID_order.TextChanged
        Dim txt As TextBox = sender
        Dim id As Integer

        ClearControls(tpIn)
        ClearControls(tpOut)

        If Integer.TryParse(txt.Text, id) Then
            _order.GetCusStk(id)
            _order.LoadCmbCar(id)
        End If
    End Sub

    '銷售管理-車號選擇
    Private Sub cmbCarNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCarNo.SelectedIndexChanged
        Dim cmb As ComboBox = sender

        If cmb.SelectedIndex >= 0 AndAlso cmbo_deposit_out_c_id.Items.Count > 0 Then
            _order.GetCarStk(cmb.SelectedItem.value)

            Dim index As Integer = cmbCarNo.SelectedIndex
            cmbo_deposit_out_c_id.SelectedIndex = index

            '修改時,改變車號要重新計算寄桶結存
            If Not String.IsNullOrEmpty(txto_id.Text) AndAlso cmb.SelectedItem.value <> _order.OrgCarStk.c_id Then
                Dim txts As List(Of TextBox)
                Dim isIn As Boolean
                If tcInOut.SelectedTab.Text = "進場單" Then
                    txts = tpIn.Controls.OfType(Of TextBox).Where(Function(x) x.Tag.ToString.StartsWith("o_deposit_in_")).ToList
                    isIn = True
                Else
                    txts = tpOut.Controls.OfType(Of TextBox).Where(Function(x) x.Tag.ToString.StartsWith("o_deposit_out_")).ToList
                    isIn = False
                End If

                txts.ForEach(Sub(x) CalculateDeposit_TextChaged(x, e, isIn, True))
            End If
        Else
            tpIn.Controls.OfType(Of TextBox).Where(Function(txt) txt.Tag.ToString.StartsWith("c_deposit")).ToList.ForEach(Sub(t) t.Clear())
        End If
    End Sub

    '銷售管理-寄桶結存車號
    Private Sub cmbo_deposit_out_c_id_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbo_deposit_out_c_id.SelectedIndexChanged
        Dim cmb As ComboBox = sender
        'LoadDepositValue(cmb, cmb.Parent)
        'If cmb.SelectedIndex >= 0 Then
        '    _order.GetCarStk(cmb.SelectedItem.value)
        'End If
        If cmb.SelectedIndex >= 0 Then
            _order.GetCarStk(cmb.SelectedItem.value)

            '修改時,改變車號要重新計算寄桶結存
            If Not String.IsNullOrEmpty(txto_id.Text) AndAlso cmb.SelectedItem.value <> _order.OrgCarStk.c_id Then
                Dim txts As List(Of TextBox)
                txts = tpOut.Controls.OfType(Of TextBox).Where(Function(x) x.Tag.ToString.StartsWith("o_deposit_out_")).ToList
                txts.ForEach(Sub(x) CalculateDeposit_TextChaged(x, e, False, True))
            End If
        Else
            tpIn.Controls.OfType(Of TextBox).Where(Function(txt) txt.Tag.ToString.StartsWith("c_deposit")).ToList.ForEach(Sub(t) t.Clear())
        End If
    End Sub

    '銷售管理-新增
    Private Sub btnCreate_ord_Click(sender As Object, e As EventArgs) Handles btnCreate_ord.Click
        _order.Insert(If(tcInOut.SelectedTab.Text = "進場單", tpIn, tpOut))
    End Sub

    '銷售管理-dgv
    Private Sub dgvOrder_SelectionChanged(sender As Object, e As EventArgs) Handles dgvOrder.SelectionChanged, dgvOrder.CellMouseClick
        If Not dgvOrder.Focused Then Return

        SetButtonState(sender, False)

        Dim row = dgvOrder.SelectedRows(0)
        Dim ordId As Integer = row.Cells("編號").Value

        _order.GetDetail(ordId)

        '暫存客戶瓦斯瓶明細數量
        _order.GasBarrel = New Dictionary(Of String, Object)
        _order.InspectBarrel = New Dictionary(Of String, Object)

        Dim inTxts = tpIn.Controls.OfType(Of TextBox)
        For Each txt In inTxts.Where(Function(x) x.Tag.ToString.Contains("o_in_") Or x.Tag.ToString.Contains("o_new_in_"))
            _order.GasBarrel.Add(txt.Tag, txt.Text)
        Next

        For Each txt In inTxts.Where(Function(x) x.Tag.ToString.StartsWith("o_inspect_"))
            _order.InspectBarrel.Add(txt.Tag, txt.Text)
        Next

        For Each txt In tpOut.Controls.OfType(Of TextBox).Where(Function(x) x.Tag.ToString.Contains("o_gas_c_") _
                                                                         Or x.Tag.ToString.Contains("o_gas_") _
                                                                         Or x.Tag.ToString.Contains("o_empty_"))
            _order.GasBarrel.Add(txt.Tag, txt.Text)
        Next
    End Sub

    '銷售管理-修改
    Private Sub btnEdit_order_Click(sender As Object, e As EventArgs) Handles btnEdit_order.Click
        _order.Update(tcInOut.SelectedTab, txto_id.Text)
    End Sub

    '銷售管理-刪除
    Private Sub btnDelete_order_Click(sender As Object, e As EventArgs) Handles btnDelete_order.Click
        _order.Delete(txto_id.Text, tcInOut.SelectedTab.Text)
    End Sub

    '銷售管理-快捷鍵
    Private Sub frmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If TabControl1.SelectedTab.Name <> "tpOrder" Then Exit Sub

        Select Case e.KeyCode
            Case Keys.F1
                If btnCreate_ord.Enabled = True Then _order.Insert(If(tcInOut.SelectedIndex = 0, tpIn, tpOut))

            Case Keys.F2
                If btnEdit_order.Enabled = True Then _order.Update(tcInOut.SelectedTab, txto_id.Text)

            Case Keys.F3
                If btnDelete_order.Enabled = True Then _order.Delete(txto_id.Text, tcInOut.SelectedTab.Text)

            Case Keys.F4
                Reset()
                '_order.LoadList(False)
                _order.SearchOrders()

            Case Keys.F5

        End Select
    End Sub

    '銷售管理-客戶代號
    Private Sub txtCusCode_ord_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_ord.KeyDown
        '按下Enter時,搜尋客戶資料
        If e.KeyCode = Keys.Enter Then
            Dim cus = _order.GetCusDataByCusCode(txtCusCode_ord.Text)
            Reset()
            If cus IsNot Nothing Then
                txtCusCode_ord.Text = cus.cus_code
                txtCusName.Text = cus.cus_name
                txtCusID_order.Text = cus.cus_id

                '"客戶代號" 輸入完成並按下 "enter" ,有查到就跳到 "日期"
                dtpo_date.Focus()
            Else
                MsgBox("查無此客戶")
            End If
        End If
    End Sub

    '銷售管理-日期
    Private Sub dtpo_date_KeyDown(sender As Object, e As KeyEventArgs) Handles dtpo_date.KeyDown
        '按下Enter時,跳到"車號"
        If e.KeyCode = Keys.Enter Then
            cmbCarNo.Focus()
            '自動展開選單
            cmbCarNo.DroppedDown = True
        End If
    End Sub

    '銷售管理-車號
    Private Sub cmbCarNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbCarNo.KeyDown
        '按下Enter時,跳到"進出場單"的sheet
        If e.KeyCode = Keys.Enter Then
            tcInOut.Focus()
        End If
    End Sub

    '銷售管理-進出場單
    Private Sub tcInOut_KeyDown(sender As Object, e As KeyEventArgs) Handles tcInOut.KeyDown
        '按下Enter時,跳到當前sheet第一格
        If e.KeyCode = Keys.Enter Then
            If tcInOut.SelectedTab.Text = "進場單" Then
                txto_in_50.Focus()
            ElseIf tcInOut.SelectedTab.Text = "出場單" Then
                txtGas_c_50.Focus()
            End If
        End If
    End Sub

    '銷售管理-查詢
    Private Sub btnQuery_order_Click(sender As Object, e As EventArgs) Handles btnQuery_order.Click
        Dim btn As Button = btnQuery_order
        SetOrderQueryCtrl(btn)

        If btn.Text = "查  詢" Then
            Dim criteria As New OrderSearchCriteria With {
                .StartDate = dtpStart_order.Value.Date,
                .EndDate = dtpEnd_order.Value.Date.AddDays(1),
                .CusCode = txtCusCode_ord.Text,
                .InOut = grpSearch_ord.Controls.OfType(Of RadioButton).First(Function(x) x.Checked).Text
            }

            _order.SearchOrders(criteria)
        End If
    End Sub

    '銷售管理-列印
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        _order.Print(txto_id.Text)
    End Sub

    '銷售管理-列印客戶寄桶結存瓶
    Private Sub btnCusGasCylinderInventory_Click(sender As Object, e As EventArgs) Handles btnCusGasCylinderInventory.Click
        '要產生全部客戶還是個別客戶?
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

        If _order.StockValues.TryGetValue(targetTxtBox.Tag, orgStock) Then
            Dim products As List(Of TextBox)

            If isIn Then
                products = container.Controls.OfType(Of TextBox).Where(Function(x) (x.Name.StartsWith("txto_in_") Or x.Name.StartsWith("txto_new_in_")) AndAlso x.Tag.ToString.Contains(groupName)).ToList
            Else
                products = container.Controls.OfType(Of TextBox).Where(Function(x) (x.Tag.ToString.StartsWith("o_gas_") Or x.Tag.ToString.StartsWith("o_empty_")) _
                                                                            AndAlso x.Tag.ToString.Contains(groupName)).ToList
            End If

            targetTxtBox.Text = _order.CalculateCusStock(products, targetTxtBox.Text, isIn)
        End If
    End Sub

    ''' <summary>
    ''' 即時計算車輛寄桶數量
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <param name="isIn">是否為進場</param>
    Private Sub CalculateDeposit_TextChaged(sender As Object, e As EventArgs, isIn As Boolean, Optional firstCount As Boolean = False)
        Dim txt As TextBox = sender
        Dim groupName As String = GetGroupName(txt.Name)
        Dim container = txt.Parent
        Dim targetTxtBox As TextBox = container.Controls.OfType(Of TextBox).FirstOrDefault(Function(x) x.Tag = "c_deposit_" & groupName)

        If targetTxtBox Is Nothing OrElse String.IsNullOrEmpty(targetTxtBox.Text) Then Exit Sub

        Dim products As List(Of TextBox)

        If isIn Then
            products = container.Controls.OfType(Of TextBox).Where(Function(x) x.Tag.ToString.StartsWith("o_deposit_in_") AndAlso x.Tag.ToString.Substring(13) = groupName).ToList
        Else
            products = container.Controls.OfType(Of TextBox).Where(Function(x) x.Tag.ToString.StartsWith("o_deposit_out_") AndAlso x.Tag.ToString.Substring(14) = groupName).ToList
        End If

        targetTxtBox.Text = _order.CalculateCarStk(products, groupName, isIn, firstCount)
    End Sub

    Private Sub CalculateInspect_TextChaged(sendor As Object, e As EventArgs, isIn As Boolean)
        Dim txt As TextBox = sendor
        Dim groupName As String = GetGroupName(txt.Name)
        Dim container = txt.Parent
        Dim targetTxt As TextBox = container.Controls.OfType(Of TextBox).FirstOrDefault(Function(x) x.Tag = "cus_inspect_" & groupName)

        If targetTxt Is Nothing OrElse String.IsNullOrEmpty(targetTxt.Text) Then Exit Sub

        targetTxt.Text = _order.CalculateInspectStk(txt, targetTxt, isIn)
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

        'Dim minusTxtBox As TextBox = Nothing
        Dim targetTxtBox As TextBox = Nothing

        Select Case gasTxtName
            Case "txtGas_c_"
                'minusTxtBox = txto_return_c
                targetTxtBox = txtSumGas_c
            Case "txtGas_"
                'minusTxtBox = txto_return
                targetTxtBox = txtSumGas
            Case Else
                Throw New Exception($"未設定此 {gasTxtName} ,請到 GasSum 設定")
                Exit Sub
        End Select

        'Dim returnGas As Integer = 0
        'Integer.TryParse(minusTxtBox.Text, returnGas)

        'targetTxtBox.Text = sum - returnGas
        targetTxtBox.Text = sum
    End Sub

    Private Function GetGroupName(name As String) As String
        ' 提取名稱中的公斤數
        Dim match As Match = Regex.Match(name, "\d+")
        Return If(match.Success, match.Value, String.Empty)
    End Function

    '銷售管理-總金額
    Private Sub SumTotalPrice(sender As Object, e As EventArgs) Handles txtSumGas.TextChanged, txtSumGas_c.TextChanged
        If String.IsNullOrEmpty(txtCusID_order.Text) Then Exit Sub
        ''取得當月價格
        'Dim price As Single = 0
        'Dim price_c As Single = 0

        'Using db As New gas_accounting_systemEntities
        '    Dim year = dtpo_date.Value.Year
        '    Dim month = dtpo_date.Value.Month
        '    Dim bp = db.basic_price.FirstOrDefault(Function(x) x.bp_date.Year = year AndAlso x.bp_date.Month = month)

        '    If bp Is Nothing Then
        '        MsgBox("該月份未設定價格,請至 基本資料-基礎價格 設定")
        '        Exit Sub
        '    End If

        '    price = bp.bp_normal_out
        '    price_c = bp.bp_c_out
        'End Using

        ''判斷運送方式,更新價格
        'If rdoDelivery.Checked Then
        '    price += 0.5
        '    price_c += 0.5
        'End If

        Dim gas As Integer = 0
        Integer.TryParse(txtSumGas.Text, gas)

        Dim gas_c As Integer = 0
        Integer.TryParse(txtSumGas_c.Text, gas_c)

        Dim saleAllowance As Integer = 0
        Integer.TryParse(txto_sales_allowance.Text, saleAllowance)

        'txto_total_amount.Text = (gas * price + gas_c * price_c) - saleAllowance
        Dim type = grpTransport.Controls.OfType(Of RadioButton).First(Function(x) x.Checked).Text
        txto_total_amount.Text = _order.CalculateTotalPrice(gas, gas_c, txtCusID_order.Text, type, dtpo_date.Value) - saleAllowance
    End Sub

    Public Sub ShowList(data As List(Of CompanyVM)) Implements ICommonView(Of company, CompanyVM).ShowList
        dgvCompany.DataSource = data
        SetColumnHeaders("company", dgvCompany)
    End Sub

    Public Sub SetDataToControl(data As company) Implements ICompanyView.SetDataToControl
        AutoMapEntityToControls(data, tpCompany)
    End Sub

    Public Sub ClearInput() Implements ICompanyView.ClearInput
        ClearControls(tpCompany)
    End Sub

    Private Function GetUserInput_Company() As company Implements ICompanyView.GetUserInput
        Dim data As New company
        AutoMapControlsToEntity(data, tpCompany)
        Return data
    End Function

    Public Function SetRequired() As List(Of Control) Implements ICommonView(Of company, CompanyVM).SetRequired
        Return New List(Of Control) From {txtName_comp, txtShortName, txtTaxID, txtInitGasStock}
    End Function

    '基本資料-公司管理-取消
    Private Sub btnCancel_comp_Click(sender As Object, e As EventArgs) Handles btnCancel_comp.Click
        ClearInput()
        SetButtonState(sender, True)
        txtInitGasStock.ReadOnly = False
        _company.LoadList()
    End Sub

    '基本資料-公司管理-新增
    Private Sub btnAdd_comp_Click(sender As Object, e As EventArgs) Handles btnAdd_comp.Click
        _company.Add()
    End Sub

    '基本資料-公司管理-dgv
    Private Sub dgvCompany_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCompany.SelectionChanged, dgvCompany.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return
        ClearControls(ctrl.Parent)
        SetButtonState(ctrl, False)
        txtInitGasStock.ReadOnly = True
        Dim id As Integer = ctrl.SelectedRows(0).Cells(0).Value
        _company.SelectRow(id)
    End Sub

    '基本資料-公司管理-修改
    Private Sub btnEdit_comp_Click(sender As Object, e As EventArgs) Handles btnEdit_comp.Click
        Dim id As Integer = txtID_comp.Text
        _company.Edit(id)
    End Sub

    '基本資料-公司管理-刪除
    Private Sub btnDelete_comp_Click(sender As Object, e As EventArgs) Handles btnDelete_comp.Click
        Dim id As Integer = txtID_comp.Text
        _company.Delete(id)
    End Sub

    Public Sub ShowList(data As List(Of ManufacturerVM)) Implements ICommonView(Of manufacturer, ManufacturerVM).ShowList
        dgvManufacturer.DataSource = data
    End Sub

    Public Sub SetDataToControl(data As manufacturer) Implements ICommonView(Of manufacturer, ManufacturerVM).SetDataToControl
        AutoMapEntityToControls(data, tpManu)
    End Sub

    Private Function GetUserInput_Manufacturer() As manufacturer Implements ICommonView(Of manufacturer, ManufacturerVM).GetUserInput
        Dim data As New manufacturer
        AutoMapControlsToEntity(data, tpManu)
        Return data
    End Function

    Private Sub ICommonView_ClearInput() Implements ICommonView(Of manufacturer, ManufacturerVM).ClearInput
        ClearControls(tpManu)
    End Sub

    Public Function GetSearchConditions() As manufacturer Implements IManufacturerView.GetSearchConditions
        Return New manufacturer With {
            .manu_code = txtCode_manu.Text,
            .manu_name = txtName_menu.Text,
            .manu_phone1 = txtphone1_menu.Text
        }
    End Function

    Private Function SetRequired_Manufacturer() As List(Of Control) Implements ICommonView(Of manufacturer, ManufacturerVM).SetRequired
        Return New List(Of Control) From {txtCode_manu, txtName_menu, txtContact_manu, txtphone1_menu}
    End Function

    '基本資料-廠商管理-取消
    Private Sub btnCancel_manu_Click(sender As Object, e As EventArgs) Handles btnCancel_manu.Click
        SetButtonState(sender, True)
        ICommonView_ClearInput()

        If btnQuery_manu.Text = "確  認" Then btnQuery_manu_Click(btnQuery_manu, EventArgs.Empty)

        _manufacturer.LoadList()
    End Sub

    '基本資料-廠商管理-新增
    Private Sub btnAdd_manu_Click(sender As Object, e As EventArgs) Handles btnAdd_manu.Click
        _manufacturer.Add()
    End Sub

    '基本資料-廠商管理-dgv
    Private Sub dgvManufacturer_SelectionChanged(sender As Object, e As EventArgs) Handles dgvManufacturer.SelectionChanged, dgvManufacturer.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return

        SetButtonState(ctrl, False)

        Dim id As Integer = ctrl.SelectedRows(0).Cells(0).Value
        _manufacturer.SelectRow(id)
    End Sub

    '基本資料-廠商管理-修改
    Private Sub btnEdit_manu_Click(sender As Object, e As EventArgs) Handles btnEdit_manu.Click
        Dim id As Integer = txtID_manu.Text
        _manufacturer.Edit(id)
    End Sub

    '基本資料-廠商管理-刪除
    Private Sub btnDelete_manu_Click(sender As Object, e As EventArgs) Handles btnDelete_manu.Click
        Dim id As Integer = txtID_manu.Text
        _manufacturer.Delete(id)
    End Sub

    '基本資料-廠商管理-查詢
    Private Sub btnQuery_manu_Click(sender As Object, e As EventArgs) Handles btnQuery_manu.Click
        Dim btn As Button = sender
        Dim lst = New List(Of Control) From {txtCode_manu, txtName_menu, txtphone1_menu}
        SetQueryControls(btn, lst)

        If btn.Text = "查  詢" Then
            _manufacturer.LoadList(GetSearchConditions)
        End If
    End Sub

    Public Sub ShowList(data As List(Of PricePlanVM)) Implements ICommonView(Of priceplan, PricePlanVM).ShowList
        dgvPricePlan.DataSource = data
    End Sub

    Public Sub SetDataToControl(data As priceplan) Implements ICommonView(Of priceplan, PricePlanVM).SetDataToControl
        AutoMapEntityToControls(data, tpPricePlan)
    End Sub

    Private Function GetUserInput_PricePlan() As priceplan Implements ICommonView(Of priceplan, PricePlanVM).GetUserInput
        Dim data = New priceplan
        AutoMapControlsToEntity(data, tpPricePlan)
        Return data
    End Function

    Private Sub ClearInput_PricePlan() Implements ICommonView(Of priceplan, PricePlanVM).ClearInput
        ClearControls(tpPricePlan)
    End Sub

    Private Function SetRequired_PricePlan() As List(Of Control) Implements ICommonView(Of priceplan, PricePlanVM).SetRequired
        Return New List(Of Control) From {txtName_pp}
    End Function

    '基本資料-價格方案-取消
    Private Sub btnCancel_pp_Click(sender As Object, e As EventArgs) Handles btnCancel_pp.Click
        SetButtonState(sender, True)
        ClearInput_PricePlan()
        _pricePlan.LoadList()
    End Sub

    '基本資料-價格方案-新增
    Private Sub btnAdd_pp_Click(sender As Object, e As EventArgs) Handles btnAdd_pp.Click
        _pricePlan.Add()
    End Sub

    '基本資料-價格方案-修改
    Private Sub btnEdit_pp_Click(sender As Object, e As EventArgs) Handles btnEdit_pp.Click
        Dim id As Integer = txtId_pp.Text
        _pricePlan.Edit(id)
    End Sub

    '基本資料-價格方案-dgv
    Private Sub dgvPricePlan_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPricePlan.SelectionChanged, dgvPricePlan.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return

        SetButtonState(ctrl, False)

        Dim id As Integer = ctrl.SelectedRows(0).Cells(0).Value
        _pricePlan.SelectRow(id)
    End Sub

    '基本資料-價格方案-刪除
    Private Sub btnDelete_pp_Click(sender As Object, e As EventArgs) Handles btnDelete_pp.Click
        Dim id As Integer = txtId_pp.Text
        _pricePlan.Delete(id)
    End Sub

    Public Sub SetRolesAndPermissions(roles As List(Of RoleVM), permissions As List(Of String)) Implements IPermissionView.SetRolesAndPermissions
        ' 清除
        dgvRoles.Columns.Clear()
        dgvRoles.Rows.Clear()

        ' 添加角色 ID 和名稱欄位
        dgvRoles.Columns.Add("Id", "角色編號")
        dgvRoles.Columns.Add("Name", "角色名稱")

        ' 為每個權限添加欄位
        For Each permission In permissions
            Dim column = New DataGridViewCheckBoxColumn With {
                .Name = permission,
                .HeaderText = permission,
                .DataPropertyName = permission
            }
            dgvRoles.Columns.Add(column)
        Next

        ' 添加列
        For Each role In roles
            Dim rowValues As New List(Of Object)
            rowValues.Add(role.Id)
            rowValues.Add(role.Name)
            For Each permission In permissions
                rowValues.Add(role.GetPermissionValue(permission))
            Next
            dgvRoles.Rows.Add(rowValues.ToArray)
        Next
    End Sub

    Public Sub SetPermissions(permissions As List(Of permission)) Implements IPermissionView.SetPermissions
        flpRoles.Controls.Clear()
        For Each permission In permissions
            Dim chk As New CheckBox With {
                .Text = permission.per_Name,
                .Tag = permission.per_Id,
                .AutoSize = True
            }
            flpRoles.Controls.Add(chk)
        Next
    End Sub

    Public Function GetRoleName() As String Implements IPermissionView.GetRoleName
        Return txtRolesName.Text.Trim
    End Function

    Public Function GetSelectedPermissions() As Dictionary(Of String, Boolean) Implements IPermissionView.GetSelectedPermissions
        Return flpRoles.Controls.OfType(Of CheckBox).ToDictionary(Function(x) x.Text, Function(x) x.Checked)
    End Function

    Public Sub ClearInputs() Implements IPermissionView.ClearInputs
        ClearControls(tpRoles)

        'txtRolesName.Clear()
        'For Each control As Control In flpRoles.Controls
        '    If TypeOf control Is CheckBox Then
        '        DirectCast(control, CheckBox).Checked = False
        '    End If
        'Next
    End Sub

    Public Sub SetDataToControls(role As RoleVM) Implements IPermissionView.SetDataToControls
        txtRolesName.Text = role.Name
        For Each ctrl As Control In flpRoles.Controls
            If TypeOf ctrl Is CheckBox Then
                Dim chk = DirectCast(ctrl, CheckBox)
                Dim isChecked As Boolean
                If role.Permissions.TryGetValue(chk.Text, isChecked) Then
                    chk.Checked = isChecked
                Else
                    chk.Checked = False
                End If
            End If
        Next
    End Sub

    Public Function GetSelectedRoleId() As Integer Implements IPermissionView.GetSelectedRoleId
        If dgvRoles.SelectedRows.Count > 0 Then
            Return dgvRoles.SelectedRows(0).Cells("Id").Value
        End If

        Return -1
    End Function

    '基本資料-權限管理-取消
    Private Async Sub btnCancel_roles_Click(sender As Object, e As EventArgs) Handles btnCancel_roles.Click
        SetButtonState(sender, True)
        ClearControls(tpRoles)
        Await _permission.LoadRolesAndPermissionsAsync
    End Sub

    '基本資料-權限管理-新增
    Private Async Sub btnAdd_roles_Click(sender As Object, e As EventArgs) Handles btnAdd_roles.Click
        Await _permission.AddAsync
    End Sub

    '基本資料-權限管理-dgv
    Private Async Sub dgvRoles_SelectionChanged(sender As Object, e As EventArgs) Handles dgvRoles.SelectionChanged, dgvRoles.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return

        SetButtonState(ctrl, False)

        Dim id As Integer = ctrl.SelectedRows(0).Cells(0).Value
        Await _permission.SelectRoleAsync(id)
    End Sub

    '基本資料-權限管理-修改
    Private Async Sub btnEdit_roles_Click(sender As Object, e As EventArgs) Handles btnEdit_roles.Click
        Await _permission.UpdateAsync
    End Sub

    '基本資料-權限管理-刪除
    Private Async Sub btnDelete_roles_Click(sender As Object, e As EventArgs) Handles btnDelete_roles.Click
        Await _permission.DeleteAsync
    End Sub

    Public Sub SetRolesCmb(data As List(Of ComboBoxItems)) Implements IEmployeeView.SetRolesCmb
        SetComboBox(cmbRoles, data)
    End Sub

    Public Sub ShowList(data As List(Of EmployeeVM)) Implements ICommonView(Of employee, EmployeeVM).ShowList
        dgvEmployee.DataSource = data
    End Sub

    Public Sub SetDataToControl(data As employee) Implements ICommonView(Of employee, EmployeeVM).SetDataToControl
        AutoMapEntityToControls(data, tpEmployee)
    End Sub

    Private Function GetUserInput_emp() As employee Implements ICommonView(Of employee, EmployeeVM).GetUserInput
        Dim data As New employee
        AutoMapControlsToEntity(data, tpEmployee)
        Return data
    End Function

    Private Sub ClearInput_emp() Implements ICommonView(Of employee, EmployeeVM).ClearInput
        ClearControls(tpEmployee)
    End Sub

    Private Function SetRequired_Employee() As List(Of Control) Implements ICommonView(Of employee, EmployeeVM).SetRequired
        Return New List(Of Control) From {txtEmpName, txtEmpPhone}
    End Function

    '基本資料-員工管理-取消
    Private Sub btnCancel_emp_Click(sender As Object, e As EventArgs) Handles btnCancel_emp.Click
        SetButtonState(sender, True)
        _employee.GetRolesCmb()
        _employee.LoadList()
        ClearInput_emp()
    End Sub

    '基本資料-員工管理-新增
    Private Sub btnAdd_emp_Click(sender As Object, e As EventArgs) Handles btnAdd_emp.Click
        _employee.Add()

    End Sub

    '基本資料-員工管理-dgv
    Private Sub dgvEmployee_SelectionChanged(sender As Object, e As EventArgs) Handles dgvEmployee.SelectionChanged, dgvEmployee.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return

        SetButtonState(ctrl, False)

        Dim id As Integer = ctrl.SelectedRows(0).Cells(0).Value
        _employee.SelectRow(id)
    End Sub

    '基本資料-員工管理-修改
    Private Sub btnEdit_emp_Click(sender As Object, e As EventArgs) Handles btnEdit_emp.Click
        Dim id As Integer = txtId_emp.Text
        _employee.Edit(id)
    End Sub

    '基本資料-員工管理-刪除
    Private Sub btnDelete_emp_Click(sender As Object, e As EventArgs) Handles btnDelete_emp.Click
        Dim id As Integer = txtId_emp.Text
        _employee.Delete(id)
    End Sub

    Public Sub ShowList(data As List(Of BankVM)) Implements ICommonView(Of bank, BankVM).ShowList
        dgvBank.DataSource = data
    End Sub

    Public Sub SetDataToControl(data As bank) Implements ICommonView(Of bank, BankVM).SetDataToControl
        AutoMapEntityToControls(data, tpBank)
    End Sub

    Private Function GetUserInput_bank() As bank Implements ICommonView(Of bank, BankVM).GetUserInput
        Try
            Dim data As New bank
            AutoMapControlsToEntity(data, tpBank)
            Return data
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub ClearInput_bank() Implements ICommonView(Of bank, BankVM).ClearInput
        ClearControls(tpBank)
    End Sub

    Private Function SetRequired_Bank() As List(Of Control) Implements ICommonView(Of bank, BankVM).SetRequired
        Return New List(Of Control) From {txtBankName, txtAccountName, txtAccount_bank}
    End Function

    '基本資料-銀行帳戶-取消
    Private Sub btnCancel_bank_Click(sender As Object, e As EventArgs) Handles btnCancel_bank.Click
        SetButtonState(sender, True)
        _bank.LoadList()
        ClearInput_bank()
    End Sub

    '基本資料-銀行帳戶-新增
    Private Sub btnAdd_bank_Click(sender As Object, e As EventArgs) Handles btnAdd_bank.Click
        _bank.Add()
    End Sub

    '基本資料-銀行帳戶-dgv
    Private Sub dgvBank_SelectionChanged(sender As Object, e As EventArgs) Handles dgvBank.SelectionChanged, dgvBank.CellMouseClick
        Dim id = DGV_SelectionChanged(sender)
        If id > 0 Then _bank.SelectRow(id)
    End Sub

    '基本資料-銀行帳戶-修改
    Private Sub btnEdit_bank_Click(sender As Object, e As EventArgs) Handles btnEdit_bank.Click
        Dim id = txtId_bank.Text
        _bank.Edit(id)
    End Sub

    '基本資料-銀行帳戶-刪除
    Private Sub btnDelete_bank_Click(sender As Object, e As EventArgs) Handles btnDelete_bank.Click
        Dim id = txtId_bank.Text
        _bank.Delete(id)
    End Sub

    Public Sub ShowList(data As List(Of PaymentVM)) Implements ICommonView(Of payment, PaymentVM).ShowList
        dgvPayment.DataSource = data
    End Sub

    Public Sub SetDataToControl(data As payment) Implements ICommonView(Of payment, PaymentVM).SetDataToControl
        AutoMapEntityToControls(data, tpPayment)
    End Sub

    Private Function GetUserInput_Payment() As payment Implements ICommonView(Of payment, PaymentVM).GetUserInput
        Dim data As New payment
        AutoMapControlsToEntity(data, tpPayment)
        Return data
    End Function

    Private Sub ClearInput_Payment() Implements ICommonView(Of payment, PaymentVM).ClearInput
        ClearControls(tpPayment)
        dgvAmountDue.DataSource = Nothing
    End Sub

    Public Sub SetManuCmb(data As List(Of ComboBoxItems)) Implements IPaymentView.SetManuCmb
        SetComboBox(cmbManu_payment, data)
    End Sub

    Public Sub SetBankCmb(data As List(Of ComboBoxItems)) Implements IPaymentView.SetBankCmb
        SetComboBox(cmbBank_payment, data)
    End Sub

    Public Sub SetSubjectsCmb(data As List(Of ComboBoxItems)) Implements IPaymentView.SetSubjectsCmb
        SetComboBox(cmbSubjects_payment, data)
    End Sub

    Private Sub IPaymentView_SetCompanyCmb(data As List(Of ComboBoxItems)) Implements IPaymentView.SetCompanyCmb
        SetComboBox(cmbCompany_payment, data)
    End Sub

    Public Function GetQueryConditions() As PaymentQueryVM Implements IPaymentView.GetQueryConditions
        Return New PaymentQueryVM With {
            .BankId = If(cmbBank_payment.SelectedItem Is Nothing, 0, cmbBank_payment.SelectedItem.Value),
            .Type = cmbPayType.Text,
            .EndDate = dtpEnd_payment.Value.Date.AddDays(1).AddSeconds(-1),
            .ManufacturerId = If(cmbManu_payment.SelectedItem Is Nothing, 0, cmbManu_payment.SelectedItem.Value),
            .StartDate = dtpStart_payment.Value.Date,
            .SubjectsId = If(cmbSubjects_payment.SelectedItem Is Nothing, 0, cmbSubjects_payment.SelectedItem.Value)
        }
    End Function

    Public Function GetChequeDatas() As cheque Implements IPaymentView.GetChequeDatas
        Return New cheque With {
            .che_ReceivedDate = dtpCheDate.Value.Date,
            .che_Number = txtCheNo_payment.Text,
            .che_Amount = txtAmount.Text,
            .che_Memo = txtMemo_payment.Text,
            .che_Type = "借",
            .che_IssuerName = cmbCompany_payment.Text,
            .che_AccountNumber = cmbBank_payment.Text,
            .chu_State = "未兌現"
            }
    End Function

    Private Function SetRequired_Payment() As List(Of Control) Implements ICommonView(Of payment, PaymentVM).SetRequired
        Dim req = New List(Of Control) From {txtAmount, cmbPayType, cmbCompany_payment, cmbBank_payment}
        If cmbPayType.Text = "支票" Then req.Add(txtCheNo_payment)
        Return req
    End Function

    Private Sub IPaymentView_SetAmountDueDGV(data As List(Of AmountDueVM)) Implements IPaymentView.SetAmountDueDGV
        dgvAmountDue.DataSource = data
    End Sub

    '支出管理-付款作業-取消
    Private Sub btnCancel_payment_Click(sender As Object, e As EventArgs) Handles btnCancel_payment.Click
        SetButtonState(sender, True)
        _payment.SetManuCmb()
        _payment.LoadBanksList()
        _payment.SetCompanyCmb()
        _payment.SetSubjectsCmb()
        ClearInput_Payment()
        _payment.LoadList()
        If btnQuery_payment.Text = "確  認" Then SetPaymentQueryCtrlsState()
    End Sub

    '支出管理-付款作業-新增
    Private Sub btnAdd_payment_Click(sender As Object, e As EventArgs) Handles btnAdd_payment.Click
        _payment.Add()
    End Sub

    '支出管理-付款作業-dgv
    Private Sub dgvPayment_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPayment.SelectionChanged, dgvPayment.CellMouseClick
        Dim id = DGV_SelectionChanged(sender)
        If id > 0 Then _payment.SelectRow(id)
        QueryAmountDue()
    End Sub

    '支出管理-付款作業-修改
    Private Sub btnEdit_payment_Click(sender As Object, e As EventArgs) Handles btnEdit_payment.Click
        Dim id = txtId_payment.Text
        _payment.Edit(id)
    End Sub

    '支出管理-付款作業-刪除
    Private Sub btnDelete_payment_Click(sender As Object, e As EventArgs) Handles btnDelete_payment.Click
        Dim id = txtId_payment.Text
        _payment.Delete(id)
    End Sub

    '支出管理-付款作業-查詢
    Private Sub btnQuery_payment_Click(sender As Object, e As EventArgs) Handles btnQuery_payment.Click
        SetPaymentQueryCtrlsState()

        If sender.Text = "查  詢" Then
            _payment.Query()
        End If
    End Sub

    ''' <summary>
    ''' 設定付款作業查詢控制項狀態
    ''' </summary>
    Private Sub SetPaymentQueryCtrlsState()
        Dim lst As New List(Of Control) From {lblPayType_payment, lblSubjects_payment, lblManu_payment, lblBank_payment}
        SetQueryControls(btnQuery_payment, lst)
    End Sub

    '支出管理-付款作業-支票號碼必填
    Private Sub cmbPayType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPayType.SelectedIndexChanged
        Dim cmb As ComboBox = sender
        Dim ctrls As Control() = {lblReq_Chuque, lblCheNo_payment, txtCheNo_payment, lblCheDateReq_payment, dtpCheDate}

        For Each ctrl In ctrls
            If cmb.Text = "支票" Then
                ctrl.Visible = True
            Else
                ctrl.Visible = False
            End If
        Next
    End Sub

    Private Sub cmbManu_payment_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbManu_payment.SelectionChangeCommitted
        QueryAmountDue()
    End Sub

    ''' <summary>
    ''' 查詢應付未付
    ''' </summary>
    Private Sub QueryAmountDue()
        If cmbManu_payment.SelectedValue < 1 Then Return

        _payment.GetAmountDue(cmbManu_payment.SelectedValue)
    End Sub

    Private Sub btnQueryCus_col_Click(sender As Object, e As EventArgs) Handles btnQueryCus_col.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusId_col.Text = searchForm.CusId
                txtCusName_col.Text = searchForm.CusName
                txtCusCode_col.Text = searchForm.CusCode
            End If
        End Using
    End Sub

    Public Sub ShowList(data As List(Of CollectionVM)) Implements ICommonView(Of collection, CollectionVM).ShowList
        dgvCollection.DataSource = data
    End Sub

    Public Sub SetDataToControl(col As collection, Optional che As cheque = Nothing) Implements ICollectionView.SetDataToControl
        AutoMapEntityToControls(col, tpCollection)
        If che IsNot Nothing Then
            txtCheque_col.Text = che.che_Number
            txtCashingDate.Text = If(che.che_CashingDate.HasValue, che.che_CashingDate.Value.ToString("yyyy年MM月dd日"), "")
            txtIssuerName.Text = che.che_IssuerName
            txtCheAcctNum.Text = che.che_AccountNumber
        End If
    End Sub

    Public Sub SetDataToControl(data As collection) Implements ICommonView(Of collection, CollectionVM).SetDataToControl

    End Sub

    Private Function GetUserInput_collection() As collection Implements ICommonView(Of collection, CollectionVM).GetUserInput
        Dim list As New List(Of Control) From {txtAmount_collection, cmbType_col, cmbSubjects, txtCusId_col, cmbCompany_col}

        If cmbType_col.Text = "支票" Then
            Dim ctrls As Control() = {txtCheque_col, txtIssuerName, txtCheAcctNum}
            list.AddRange(ctrls)
        End If

        If Not CheckRequired(list) Then Return Nothing

        Dim data As New collection
        AutoMapControlsToEntity(data, tpCollection)
        Return data
    End Function

    Private Sub ClearInput_collection() Implements ICommonView(Of collection, CollectionVM).ClearInput
        ClearControls(tpCollection)
    End Sub

    Private Sub ICollectionView_SetSubjectsCmb(data As List(Of ComboBoxItems)) Implements ICollectionView.SetSubjectsCmb
        SetComboBox(cmbSubjects, data)
    End Sub

    Private Function ICollectionView_GetQueryConditions() As CollectionQueryVM Implements ICollectionView.GetQueryConditions
        Return New CollectionQueryVM With {
            .Cheque = txtCheque_col.Text,
            .CusId = If(txtCusId_col.Text = "", 0, txtCusId_col.Text),
            .Subjects = If(cmbSubjects.SelectedItem Is Nothing, 0, cmbSubjects.SelectedItem.Value),
            .EndDate = dtpEnd_col.Value.Date,
            .StartDate = dtpStart_col.Value.Date,
            .Type = cmbType_col.Text
        }
    End Function

    Private Function ICollectionView_GetChequeDatas() As cheque Implements ICollectionView.GetChequeDatas
        Return New cheque With {
            .che_Amount = txtAmount_collection.Text,
            .che_ReceivedDate = dtpDate_col.Value.Date,
            .che_Memo = txtMemo_col.Text,
            .che_Number = txtCheque_col.Text,
            .che_Type = "貸",
            .che_AccountNumber = txtCheAcctNum.Text,
            .che_IssuerName = txtIssuerName.Text,
            .chu_State = "未兌現"
        }
    End Function

    Private Sub ICollectionView_SetBankCmb(data As List(Of ComboBoxItems)) Implements ICollectionView.SetBankCmb
        SetComboBox(cmbBank_col, data)
    End Sub

    Public Sub SetCompanyCmb(data As List(Of ComboBoxItems)) Implements ICollectionView.SetCompanyCmb
        SetComboBox(cmbCompany_col, data)
    End Sub

    Public Function GetJournalDatas() As journal Implements ICollectionView.GetJournalDatas
        Return New journal With {
            .j_Amount = txtAmount_collection.Text,
            .j_Memo = txtMemo_col.Text,
            .j_s_Id = cmbSubjects.SelectedValue
        }
    End Function

    Private Sub ICollectionView_Reset() Implements ICollectionView.Reset
        ClearControls(tpCollection)

        _collect.LoadBankList()
        _collect.GetCompanyCmb()
        _collect.GetSubjectsCmb()
        _collect.LoadList()

        If btnQuery_col.Text = "確  認" Then SetOrderQueryCtrl(btnQuery_col)
        SetButtonState(btnCancel_col, True)

        btnCashing.Visible = False
        cmbType_col.Enabled = True
    End Sub

    '收入管理-收款作業-收款類型-支票號碼顯示
    Private Sub cmbType_col_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbType_col.SelectedIndexChanged
        Dim cmb As ComboBox = sender
        Dim bVisible As Boolean = cmb.Text = "支票"
        Dim ctrls As Control() = {lblChequeReq_col, lblCheque_col, txtCheque_col, lblCashingDate_col, txtCashingDate, lblIssuerNameReq, lblIssuerName, txtIssuerName,
            lblChequeAccountNumberReq, lblChequeAccountNumber, txtCheAcctNum}

        ctrls.ToList.ForEach(Sub(x) x.Visible = bVisible)

        If bVisible AndAlso btnQuery_col.Text = "確  認" Then
            lblCheque_col.BackColor = Color.Green
        End If
    End Sub

    '收入管理-收款作業-取消
    Private Sub btnCancel_col_Click(sender As Object, e As EventArgs) Handles btnCancel_col.Click
        ICollectionView_Reset()
    End Sub

    '收入管理-收款作業-新增
    Private Sub btnAdd_col_Click(sender As Object, e As EventArgs) Handles btnAdd_col.Click
        _collect.Add()
    End Sub

    '收入管理-收款作業-dgv
    Private Sub dgvCollection_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCollection.SelectionChanged, dgvCollection.CellMouseClick
        Dim id = DGV_SelectionChanged(sender)
        If id > 0 Then
            _collect.SelectRow(id)
            cmbType_col.Enabled = False
        End If
    End Sub

    '收入管理-收款作業-支票號碼改變
    Private Sub txtCheque_col_TextChanged(sender As Object, e As EventArgs) Handles txtCheque_col.TextChanged
        If Not String.IsNullOrEmpty(txtCheque_col.Text) AndAlso _cheque.IsCollected(txtCheque_col.Text) Then
            btnCashing.Visible = True
        Else
            btnCashing.Visible = False
        End If
    End Sub

    '收入管理-收款作業-修改
    Private Sub btnEdit_col_Click(sender As Object, e As EventArgs) Handles btnEdit_col.Click
        _collect.Edit()
    End Sub

    '收入管理-收款作業-刪除
    Private Sub btnDelete_col_Click(sender As Object, e As EventArgs) Handles btnDelete_col.Click
        Dim id As Integer = txtColId.Text
        _collect.Delete(id)
    End Sub

    '收入管理-收款作業-查詢
    Private Sub btnQuery_col_Click(sender As Object, e As EventArgs) Handles btnQuery_col.Click
        SetCollectionQueryCtrlsState()

        If sender.Text = "查  詢" Then
            _collect.Query()
        End If
    End Sub

    '收入管理-收款作業-支票兌現
    Private Sub btnCashing_Click(sender As Object, e As EventArgs) Handles btnCashing.Click
        _collect.UpdateCheque(txtColId.Text)
    End Sub

    '收入管理-收款作業-搜尋客戶
    Private Sub txtCusCode_col_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_col.KeyDown
        '按下Enter時,搜尋客戶資料
        If e.KeyCode = Keys.Enter Then
            Dim cus = _collect.GetCustomer(txtCusCode_col.Text)

            If cus IsNot Nothing Then
                txtCusCode_col.Text = cus.cus_code
                txtCusName_col.Text = cus.cus_name
                txtCusId_col.Text = cus.cus_id
            Else
                MsgBox("查無此客戶")
            End If
        End If
    End Sub

    ''' <summary>
    ''' 設定收款作業查詢控制項狀態
    ''' </summary>
    Private Sub SetCollectionQueryCtrlsState()
        Dim lst As New List(Of Control) From {lblType_col, lblCusCode_col, lblSubjects_col}

        If lblCheque_col.Visible = True Then lst.Add(lblCheque_col)

        SetQueryControls(btnQuery_col, lst)
    End Sub

    Public Sub ShowList(data As List(Of ChequeVM)) Implements ICommonView(Of cheque, ChequeVM).ShowList
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

    Public Sub SetDataToControl(data As cheque) Implements ICommonView(Of cheque, ChequeVM).SetDataToControl
        AutoMapEntityToControls(data, tpCheque)
    End Sub

    Public Function GetUserInput() As cheque Implements ICommonView(Of cheque, ChequeVM).GetUserInput
        Dim data As New cheque
        AutoMapControlsToEntity(data, tpCheque)
        Return data
    End Function

    Private Sub ClearInput_Cheque() Implements ICommonView(Of cheque, ChequeVM).ClearInput
        ClearControls(tpCheque)
    End Sub

    Private Function SetRequired_Cheque() As List(Of Control) Implements ICommonView(Of cheque, ChequeVM).SetRequired
        Return Nothing
    End Function

    '會計管理-支票管理-取消
    Private Sub btnCancel_Che_Click(sender As Object, e As EventArgs) Handles btnCancel_Che.Click
        ClearControls(tpCheque)
        _cheque.LoadList()
    End Sub

    '會計管理-支票管理-dgv
    Private Sub dgvCheque_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCheque.SelectionChanged, dgvCheque.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Or ctrl.SelectedRows.Count = 0 Then Return

        SetButtonState(ctrl, False)

        Dim id = ctrl.SelectedRows(0).Cells(1).Value
        _cheque.SelectRow(id)
    End Sub

    '會計管理-支票管理-狀態
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

    '會計管理-支票管理-查詢
    Private Sub btnQuery_che_Click(sender As Object, e As EventArgs) Handles btnQuery_che.Click
        _cheque.Query(dtpQueryStart_che.Value, dtpQueryEnd_che.Value)
    End Sub

    '會計管理-支票管理-全選
    Private Sub btnSelectAll_Click(sender As Object, e As EventArgs) Handles btnSelectAll.Click
        For Each row As DataGridViewRow In dgvCheque.Rows
            If Not row.IsNewRow Then
                row.Cells("ChkCol").Value = True
            End If
        Next
    End Sub

    '會計管理-支票管理-未兌現查詢
    Private Sub btnNotHonoredQuery_Click(sender As Object, e As EventArgs) Handles btnNotHonoredQuery.Click
        _cheque.ShowCollectionYetList(dtpQueryStart_che.Value.Date, dtpQueryEnd_che.Value.Date)
    End Sub

    '會計管理-支票管理-轉為已代收
    Private Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        Dim selectedIds = GetSelectedChequeIds()
        If selectedIds.Count = 0 Then
            MsgBox("請至少選擇一個對象")
            Return
        End If
        _cheque.SetBatchCollection(selectedIds, dtpCollectionDate.Value.Date)
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

    Private Function ICommonView_SetRequired() As List(Of Control) Implements ICommonView(Of collection, CollectionVM).SetRequired
        Throw New NotImplementedException()
    End Function

    Public Sub SetGasVendorCmb(item As List(Of ComboBoxItems)) Implements IReportView.SetGasVendorCmb
        SetComboBox(cmbManu, item)
    End Sub

    '會計管理-報表-氣量氣款收付明細表
    Private Sub btnCusGasPayCollect_Click(sender As Object, e As EventArgs) Handles btnCusGasPayCollect.Click
        _report.GenerateCustomersGasDetailByDay(dtpReport.Value)
    End Sub

    '會計管理-報表-客戶提氣清冊
    Private Sub btnCusGetGasList_Click(sender As Object, e As EventArgs) Handles btnCusGetGasList.Click
        _report.GenerateCustomersGetGasList(dtpReport.Value)
    End Sub

    '會計管理-報表-大氣進貨明細
    Private Sub btnGasPayableDetail_Click(sender As Object, e As EventArgs) Handles btnGasPayableDetail.Click
        If Not (cmbManu.SelectedIndex = -1 And String.IsNullOrEmpty(cmbManu.Text)) Then
            _report.GenerateGasPayableDetail(dtpReport.Value, cmbManu.SelectedValue)
        End If

    End Sub

    '會計管理-報表-單一客戶每日的應收帳明細表
    Private Sub btnDailyCusReceivable_Click(sender As Object, e As EventArgs) Handles btnDailyCusReceivable.Click
        Dim cusCode As Integer

        If Integer.TryParse(txtCusCode_report.Text, cusCode) Then
            _report.GenerateDailyCustomerReceivable(dtpReport.Value, cusCode)
        Else
            MsgBox("無效的客戶代號")
        End If
    End Sub

    '會計管理-報表-提量支數統計
    Private Sub btnGasUsageCylinderCount_Click(sender As Object, e As EventArgs) Handles btnGasUsageCylinderCount.Click
        _report.GenerateGasUsageAndCylinderCount(dtpReport.Value)
    End Sub

    '會計管理-報表-現金帳
    Private Sub btnCashAccount_Click(sender As Object, e As EventArgs) Handles btnCashAccount.Click
        _report.GenerateCashAccount(dtpReport.Value)
    End Sub

    Private Function IGasCheckoutView_GetUserInput() As PurchaseCondition Implements IGasCheckoutView.GetUserInput
        Return New PurchaseCondition With {
            .StartDate = dtpStart_gc.Value.Date,
            .EndDate = dtpEnd_gc.Value.Date,
            .ManufacturerId = If(cmbVendor_gc.SelectedValue IsNot Nothing, CInt(cmbVendor_gc.SelectedValue), 0),
            .PayType = cmbPayType_gc.SelectedItem?.ToString,
            .IsDateSearch = chkDate_gc.Checked
        }
    End Function

    Public Sub ShowList(datas As List(Of PurchaseVM)) Implements IGasCheckoutView.ShowList
        dgvGasCheckout.DataSource = datas
    End Sub

    Private Sub IGasCheckoutView_ClearInput() Implements IGasCheckoutView.ClearInput
        ClearControls(tpGasCheckout)
    End Sub

    Public Sub ShowMessage(message As String) Implements IGasCheckoutView.ShowMessage
        Console.WriteLine(message)
    End Sub

    Public Sub LoadVendors(datas As List(Of ComboBoxItems)) Implements IGasCheckoutView.LoadVendors
        SetComboBox(cmbVendor_gc, datas)
    End Sub

    Public Sub RefreshView() Implements IGasCheckoutView.RefreshView
        _gasCheckout.LoadAllDatas()
        _gasCheckout.LoadVendors()
    End Sub

    Public Function GetSelectedIds() As List(Of Integer) Implements IGasCheckoutView.GetSelectedIds
        Return dgvGasCheckout.SelectedRows.Cast(Of DataGridViewRow).Select(Function(x) CInt(x.Cells("編號").Value)).ToList
    End Function

    '大氣採購-大氣結帳-搜尋
    Private Sub btnQuery_gc_Click(sender As Object, e As EventArgs) Handles btnQuery_gc.Click
        RaiseEvent QueryClicked()
    End Sub

    '大氣採購-大氣結帳-結帳
    Private Sub btnCheckout_gc_Click(sender As Object, e As EventArgs) Handles btnCheckout_gc.Click
        RaiseEvent CheckoutClicked()
    End Sub

    '大氣採購-大氣結帳-取消
    Private Sub btnCancel_gc_Click(sender As Object, e As EventArgs) Handles btnCancel_gc.Click
        RaiseEvent CancelClicked()
    End Sub

    Private Sub TabControl1_Selected(sender As Object, e As TabControlEventArgs) Handles TabControl1.Selected
        If e.TabPage.Name = "tpLogOut" Then
            frmLogin.Show()
            Me.Close()
        End If
    End Sub
End Class