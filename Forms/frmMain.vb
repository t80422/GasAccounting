Public Class frmMain
    Implements ISubjectsView, ICompanyView, IManufacturerView, IPurchaseView, ICustomerView, IPricePlanView, IEmployeeView, IBankView, IPaymentView, IUnitPriceHistoryView, ICollectionView, ICheque, IOrderView,
        IReportView, IGasCheckoutView, IPermissionView, IBasicPriceView, IInvoiceView, IGasBarrelView, IPurchaseBarrelView, ICarView

    Public Structure UserData
        Public Id As Integer
        Public Name As String
    End Structure

    Public User As UserData

    Private _compService As ICompanyService = New CompanyService
    Private _manuService As IManufacturerService = New ManufacturerService
    Private _subjectService As ISubjectsService = New SubjectsService

    Private _basicPrice As BasicPricePresenter
    Private _car As CarPresenter
    Private _cheque As ChequePresenter
    Private _customer As CustomerPresenter
    Private _gasBarrel As GasBarrelPresenter
    Private _invoice As InvoicePresenter
    Private _permission As PermissionPresenter
    Private _purchaseBarrel As PurBarrelPresenter
    Private _subjects As SubjectsPresenter
    Private _company As New CompanyPresenter(Me)
    Private _manufacturer As New ManufacturerPresenter(Me)
    Private _purchase As PurchasePresenter
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
        Dim barMBRep As New BarrelMonthlyBalancesRep(context)
        Dim bmbRep As New BankMonthlyBalancesRep(context)
        Dim carRep As New CarRep(context)
        Dim cheRep As New ChequeRep(context)
        Dim compRep As New CompanyRep(context)
        Dim colRep As New CollectionRep(context)
        Dim cusRep As New CustomerRep(context)
        Dim gbRep As New GasBarrelRep(context)
        Dim gmbRep As New GasMonthlyBalanceRep(context)
        Dim invoiceRep As New InvoiceRep(context)
        Dim ordRep As New OrderRep(context)
        Dim pbRep As New PurchaseBarrelRep(context)
        Dim bpRep As New BasicPriceRep(context)
        Dim paymentRep As New PaymentRep(context)
        Dim permissionRep As New PermissionRep(context)
        Dim ppRep As New PricePlanRep(context)
        Dim reportRep As New ReportRep(context)
        Dim purRep As New PurchaseRep(context)
        Dim manuRep As New ManufacturerRep(context)
        Dim subjectRep As New SubjectRep(context)

        Dim barMBSer As IBarrelMonthlyBalanceService = New BarrelMonthlyBalanceService(barMBRep, gbRep, pbRep, ordRep)
        Dim bmbService As IBankMonthlyBalanceService = New BankMonthlyBalanceService(bmbRep, bankRep, paymentRep, colRep)
        Dim priceCalSer As IPriceCalculationService = New PriceCalculationService(bpRep)
        Dim gmbSer As IGasMonthlyBalanceService = New GasMonthlyBalanceService(gmbRep, ordRep, compRep)

        _basicPrice = New BasicPricePresenter(Me, bpRep)
        _car = New CarPresenter(Me, cusRep, carRep)
        _collect = New CollectionPresenter(Me, subjectRep, _compService, colRep, bankRep, cusRep, bmbService, cheRep)
        _customer = New CustomerPresenter(Me, cusRep, ppRep, compRep)
        _gasBarrel = New GasBarrelPresenter(Me, gbRep)
        _invoice = New InvoicePresenter(Me, cusRep, invoiceRep, priceCalSer, ordRep)
        _order = New OrderPresenter(Me, cusRep, carRep, ordRep, gbRep, barMBSer, priceCalSer)
        _payment = New PaymentPresenter(Me, manuRep, bankRep, subjectRep, compRep, paymentRep, bmbService, cheRep)
        _purchaseBarrel = New PurBarrelPresenter(Me, pbRep, manuRep, barMBSer, gbRep)
        _report = New ReportPresenter(Me, reportRep, bankRep, compRep)
        _subjects = New SubjectsPresenter(Me, subjectRep)
        _cheque = New ChequePresenter(Me, cheRep)
        _permission = New PermissionPresenter(Me, permissionRep)
        _purchase = New PurchasePresenter(Me, purRep, compRep, manuRep, subjectRep, gmbSer)
        _gasCheckout = New GasCheckoutPresenter(Me, purRep, manuRep)
    End Sub

    Private Sub InitializeTabPages(permissions As List(Of String))
        For Each tabPage As TabPage In TabControl1.TabPages
            If Not permissions.Contains(tabPage.Name) AndAlso tabPage.Name <> "tpLogOut" Then
                tabPage.Parent = Nothing
            End If
        Next
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
        btnRefresh_Click(btnRefresh, EventArgs.Empty)
        btnCancel_invoice_Click(btnCancel_invoice, EventArgs.Empty)
        btnCancel_gb_Click(btnCancel_gb, EventArgs.Empty)
        btnCancel_pb_Click(btnCancel_pb, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' 設定TextBox值改變事件
    ''' </summary>
    Private Sub TextChagedHandler()
#Region "大氣採購"
        Dim lstFreight As New List(Of TextBox) From {txtWeight_pur, txtDeliUnitPrice}
        lstFreight.ForEach(Sub(x) AddHandler x.TextChanged, Sub(sender, e) CalculateFreight())

        Dim lstSum_pur As New List(Of TextBox) From {txtWeight_pur, txtUnitPrice_pur, txtFreight}
        lstSum_pur.ForEach(Sub(x) AddHandler x.TextChanged, Sub(sender, e) CalculateSum_Purchase())
#End Region

#Region "新瓶採購"
        grpBarrel.Controls.OfType(Of TextBox).ToList.ForEach(Sub(x) AddHandler x.TextChanged, Sub(sender, e) CalculateAmount_PurchaseBarrel())
#End Region

#Region "銷售管理"
        SetupSalesManagementHandlers()
#End Region

#Region "大氣採購"
        'SetupGasPurchaseHandlers()
#End Region

#Region "新瓶採購"
        'SetupNewBottlePurchaseHandlers()
#End Region
    End Sub

    ''' <summary>
    ''' 設定TextBox只能輸入正整數
    ''' </summary>
    Private Sub PositiveIntegerOnly()
        Dim textBoxes = New List(Of TextBox) From {txtGas_50, txtGas_20, txtGas_16, txtGas_10, txtGas_4, txtGas_18, txtGas_14, txtGas_5, txtGas_2,
            txtGas_c_50, txtGas_c_20, txtGas_c_16, txtGas_c_10, txtGas_c_4, txtGas_c_18, txtGas_c_14, txtGas_c_5, txtGas_c_2,
            txtEmpty_50, txtEmpty_20, txtEmpty_16, txtEmpty_10, txtEmpty_4, txtEmpty_18, txtEmpty_14, txtEmpty_5, txtEmpty_2,
            txtDepositOut_50, txtDepositOut_20, txtDepositOut_16, txtDepositOut_10, txtDepositOut_4, txtDepositOut_18, txtDepositOut_14, txtDepositOut_5, txtDepositOut_2,
            txto_in_50, txto_in_20, txto_in_16, txto_in_10, txto_in_4, txto_in_18, txto_in_14, txto_in_5, txto_in_2,
            txto_new_in_50, txto_new_in_20, txto_new_in_16, txto_new_in_10, txto_new_in_4, txto_new_in_18, txto_new_in_14, txto_new_in_5, txto_new_in_2,
            txto_inspect_50, txto_inspect_20, txto_inspect_16, txto_inspect_10, txto_inspect_4, txto_inspect_18, txto_inspect_14, txto_inspect_5, txto_inspect_2,
            txtDepositIn_50, txtDepositIn_20, txtDepositIn_16, txtDepositIn_10, txtDepositIn_4, txtDepositIn_18, txtDepositIn_14, txtDepositIn_5, txtDepositIn_2,
            txto_return, txto_return_c, txto_sales_allowance, txtWeight_pur, txtAmount, txtKg_invoice, txtQty_50, txtQty_20, txtQty_16, txtQty_10, txtQty_4, txtQty_15, txtQty_14, txtQty_5,
            txtQty_2, txtUnitPrice_50, txtUnitPrice_20, txtUnitPrice_16, txtUnitPrice_10, txtUnitPrice_4, txtUnitPrice_15, txtUnitPrice_14, txtUnitPrice_5, txtUnitPrice_2}

        textBoxes.ForEach(Sub(txt) AddHandler txt.KeyPress, AddressOf PositiveIntegerOnly_TextBox)
    End Sub

    ''' <summary>
    ''' 設定TextBox只能輸入正浮點數
    ''' </summary>
    Private Sub PositiveFloatOnly()
        Dim txts = New List(Of TextBox) From {txtUnitPrice_pur, txtDeliUnitPrice, txtInitGasStock, txtUnitPrice_invoice}

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
        Dim list = New List(Of TabControl) From {TabControl1, tcBasicInfo, tcInOut}

        list.ForEach(Sub(x)
                         x.DrawMode = TabDrawMode.OwnerDrawFixed
                         AddHandler x.DrawItem, AddressOf TabControl_DrawItem
                     End Sub)
    End Sub

    Public Sub SetPricePlanDetails(data As priceplan) Implements ICustomerView.SetPricePlanDetails
        AutoMapEntityToControls(data, grpPricePlan)
    End Sub

    Public Sub ClearPricePlan() Implements ICustomerView.ClearPricePlan
        Dim exception = New List(Of String) From {cmbPricePlan.Name}
        ClearControls(grpPricePlan, exception)
    End Sub

    Public Sub PopulatePricePlanDropdown(data As List(Of SelectListItem)) Implements ICustomerView.PopulatePricePlanDropdown
        SetComboBox(cmbPricePlan, data)
    End Sub

    Public Sub DisplayList(data As List(Of CustomerVM)) Implements IBaseView(Of customer, CustomerVM).DisplayList
        dgvCustomer.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As customer) Implements IBaseView(Of customer, CustomerVM).DisplayDetail
        AutoMapEntityToControls(data, tpCustomer)
        AutoMapEntityToControls(data, grpStock)
        AutoMapEntityToControls(data, grpCusStk)
        AutoMapEntityToControls(data, grpPricePlan)
    End Sub

    Private Function ICustomerView_GetUserInput() As customer Implements IBaseView(Of customer, CustomerVM).GetUserInput
        Dim data As New customer
        AutoMapControlsToEntity(data, tpCustomer)
        AutoMapControlsToEntity(data, grpStock)
        AutoMapControlsToEntity(data, grpCusStk)
        AutoMapControlsToEntity(data, grpPricePlan)
        Return data
    End Function

    Private Sub ICustomerView_ClearInput() Implements IBaseView(Of customer, CustomerVM).ClearInput
        Dim exception = New List(Of String) From {grpInsurance.Name}
        ClearControls(tpCustomer, exception)
    End Sub

    Public Sub SetCompanyDropdown(data As List(Of SelectListItem)) Implements ICustomerView.SetCompanyDropdown
        SetComboBox(cmbCompany_cus, data)
    End Sub

    '基本資料-客戶管理-取消
    Private Sub btnCancel_cus_Click(sender As Object, e As EventArgs) Handles btnCancel_cus.Click
        SetButtonState(sender, True)
        If btnQuery_cus.Text = "確  認" Then SetOrderQueryCtrl(btnQuery_cus)
        _customer.InitializeAsync()
    End Sub

    '基本資料-客戶管理-新增
    Private Sub btnAdd_cus_Click(sender As Object, e As EventArgs) Handles btnCreate_cus.Click
        _customer.AddAsync()
    End Sub

    '基本資料-客戶管理-dgv
    Private Sub dgvCustomer_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCustomer.SelectionChanged, dgvCustomer.CellClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return

        SetButtonState(ctrl, False)

        Dim id As Integer = ctrl.SelectedRows(0).Cells(0).Value
        _customer.LoadDetailAsync(id)
    End Sub

    '基本資料-客戶管理-修改
    Private Sub btnEdit_cus_Click(sender As Object, e As EventArgs) Handles btnEdit_cus.Click
        _customer.UpdateAsync()
        SetButtonState(sender, True)
    End Sub

    '基本資料-客戶管理-刪除
    Private Sub btnDelete_cus_Click(sender As Object, e As EventArgs) Handles btnDelete_cus.Click
        Dim id As Integer = txtcus_id.Text
        _customer.DeleteAsync(id)
        SetButtonState(sender, True)
    End Sub

    '基本資料-客戶管理-查詢
    Private Async Sub btnQuery_cus_Click(sender As Object, e As EventArgs) Handles btnQuery_cus.Click
        Dim btn As Button = sender
        Dim lst = New List(Of Control) From {
            txtCusCode,
            txtCusName_cus,
            txtCusPhone1
        }

        SetQueryControls(btn, lst)

        If btn.Text = "查  詢" Then
            Await _customer.SearchAsync()
        End If
    End Sub

    '基本資料-客戶管理-價格方案
    Private Sub cmbPricePlan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPricePlan.SelectedIndexChanged
        Dim cmb As ComboBox = sender

        If cmb.SelectedIndex >= 0 Then
            Dim id As Integer

            If Integer.TryParse(cmb.SelectedValue.ToString, id) Then
                _customer.LoadPricePlanDetailsAsync(id)
            End If
        End If
    End Sub

    Public Sub DisplayList(data As List(Of CarVM)) Implements IBaseView(Of car, CarVM).DisplayList
        dgvCar.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As car) Implements IBaseView(Of car, CarVM).DisplayDetail
        AutoMapEntityToControls(data, tpCar)
        AutoMapEntityToControls(data.customer, tpCar)
        AutoMapEntityToControls(data, grpStock_car)
    End Sub

    Private Function ICarView_GetUserInput() As car Implements IBaseView(Of car, CarVM).GetUserInput
        Dim entity As New car
        AutoMapControlsToEntity(entity, tpCar)
        AutoMapControlsToEntity(entity, grpStock_car)
        Return entity
    End Function

    Private Sub ICarView_ClearInput() Implements IBaseView(Of car, CarVM).ClearInput
        ClearControls(tpCar)
    End Sub

    Public Sub DisplayCustomer(cusId As Integer, cusCode As String, cusName As String) Implements ICarView.DisplayCustomer
        txtCusCode_car.Text = cusCode
        txtCusName_car.Text = cusName
        txtCusId_car.Text = cusId
    End Sub

    Private Function ICarView_GetSearchCriteria() As CarSC Implements ICarView.GetSearchCriteria
        Return New CarSC With {.CusCode = txtCusCode_car.Text}
    End Function

    '基本資料-車輛管理-查詢客戶
    Private Sub btnQueryCus_car_Click(sender As Object, e As EventArgs) Handles btnQueryCus_car.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                _car.LoadCusByCusCode(searchForm.CusCode)
            End If
        End Using
    End Sub

    '基本資料-車輛管理-客戶代號
    Private Sub txtCusCode_car_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_car.KeyDown
        If e.KeyCode = Keys.Enter Then
            _car.LoadCusByCusCode(txtCusCode_car.Text)
        End If
    End Sub

    '基本資料-車輛管理-取消
    Private Sub btnCancel_car_Click(sender As Object, e As EventArgs) Handles btnCancel_car.Click
        SetButtonState(sender, True)
        ClearControls(tpCar)
        If btnQuery_pur.Text = "確  認" Then SetCarQueryCtrlsState()
        _car.LoadList()
    End Sub

    ''' <summary>
    ''' 設定車輛管理查詢控制項狀態
    ''' </summary>
    Private Sub SetCarQueryCtrlsState()
        Dim lst As New List(Of Control) From {lblCusCode_car}
        SetQueryControls(btnQuery_car, lst)
    End Sub

    '基本資料-車輛管理-新增
    Private Sub btnCreate_car_Click(sender As Object, e As EventArgs) Handles btnCreate_car.Click
        _car.Add()
    End Sub

    '基本資料-車輛管理-dgv
    Private Sub dgvCar_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCar.SelectionChanged, dgvCar.CellMouseClick
        If Not sender.focused Then Return

        SetButtonState(sender, False)

        Dim id = dgvCar.SelectedRows(0).Cells("編號").Value
        _car.LoadDetail(id)
    End Sub

    '基本資料-車輛管理-修改
    Private Sub btnEdit_car_Click(sender As Object, e As EventArgs) Handles btnEdit_car.Click
        _car.Update()
        SetButtonState(sender, True)
    End Sub

    '基本資料-車輛管理-刪除
    Private Sub btnDelete_car_Click(sender As Object, e As EventArgs) Handles btnDelete_car.Click
        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.Yes Then
            _car.Delete()
            SetButtonState(sender, True)
        End If
    End Sub

    '基本資料-車輛管理-查詢
    Private Sub btnQuery_car_Click(sender As Object, e As EventArgs) Handles btnQuery_car.Click
        Dim btn As Button = sender
        Dim lst = New List(Of Control) From {lblCusCode_car}

        SetQueryControls(btn, lst)

        If btn.Text = "查  詢" Then
            _car.LoadList()
        End If
    End Sub

    Public Sub DisplayList(data As List(Of BasicPriceMV)) Implements IBaseView(Of basic_price, BasicPriceMV).DisplayList
        dgvBasicPrice.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As basic_price) Implements IBaseView(Of basic_price, BasicPriceMV).DisplayDetail
        AutoMapEntityToControls(data, tpBasePrice)
    End Sub

    Private Function IBaseView_GetUserInput() As basic_price Implements IBaseView(Of basic_price, BasicPriceMV).GetUserInput
        Dim data = New basic_price
        AutoMapControlsToEntity(data, tpBasePrice)
        Return data
    End Function

    Private Sub IBaseView_ClearInput() Implements IBaseView(Of basic_price, BasicPriceMV).ClearInput
        ClearControls(tpBasePrice)
    End Sub

    '基本資料-基礎價格-取消
    Private Async Sub btnCancel_bp_Click(sender As Object, e As EventArgs) Handles btnCancel_bp.Click
        SetButtonState(sender, True)
        ClearControls(tpBasePrice)
        Await _basicPrice.SearchAsync(False)
    End Sub

    '基本資料-基礎價格-新增
    Private Async Sub btnInsert_bp_Click(sender As Object, e As EventArgs) Handles btnCreate_bp.Click
        Await _basicPrice.AddAsync()
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
    Private Async Sub btnEdit_bp_bp_Click(sender As Object, e As EventArgs) Handles btnEdit_bp.Click
        Await _basicPrice.UpdateAsync
    End Sub

    '系統設定-基礎價格-查詢
    Private Async Sub btnQuery_bp_Click(sender As Object, e As EventArgs) Handles btnQuery_bp.Click
        Await _basicPrice.SearchAsync(True)
    End Sub

    Private Function ISubjectsView_GetUserInput() As subject Implements ISubjectsView.GetUserInput
        Dim data As New subject
        AutoMapControlsToEntity(data, tpSubjects)
        Return data
    End Function

    Public Sub ShowList(data As List(Of SubjectsVM)) Implements ICommonView_old(Of subject, SubjectsVM).ShowList
        dgvSubject.DataSource = data
    End Sub

    Public Sub SetDataToControl(data As subject) Implements ICommonView_old(Of subject, SubjectsVM).SetDataToControl
        AutoMapEntityToControls(data, tpSubjects)
    End Sub

    Private Sub ClearInput_subjects() Implements ICommonView_old(Of subject, SubjectsVM).ClearInput
        ClearControls(tpSubjects)
    End Sub

    Private Function SetRequired_subjects() As List(Of Control) Implements ICommonView_old(Of subject, SubjectsVM).SetRequired
        Return New List(Of Control) From {txtName_subjects, cmbSubjectType}
    End Function

    '基本資料-科目管理-取消
    Private Sub btnCancel_subjects_Click(sender As Object, e As EventArgs) Handles btnCancel_subjects.Click
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
        _subjects.LoadDetail(id)
    End Sub

    '基本資料-科目管理-修改
    Private Sub btnEdit_subjects_Click(sender As Object, e As EventArgs) Handles btnEdit_subjects.Click
        _subjects.Update()
    End Sub

    '基本資料-科目管理-刪除
    Private Sub btnDelete_subjects_Click(sender As Object, e As EventArgs) Handles btnDelete_subjects.Click
        Dim id As Integer = txtId_subjects.Text
        If MsgBox("確定要刪除?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then _subjects.Delete(id)
    End Sub

    Public Sub DisplayList(data As List(Of GasBarrelVM)) Implements IBaseView(Of gas_barrel, GasBarrelVM).DisplayList
        dgvGB.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As gas_barrel) Implements IBaseView(Of gas_barrel, GasBarrelVM).DisplayDetail
        AutoMapEntityToControls(data, tpGasBarrel)
    End Sub

    Private Function IBaseView_GetUserInput1() As gas_barrel Implements IBaseView(Of gas_barrel, GasBarrelVM).GetUserInput
        Dim data As New gas_barrel
        AutoMapControlsToEntity(data, tpGasBarrel)
        Return data
    End Function

    Private Sub IBaseView_ClearInput1() Implements IBaseView(Of gas_barrel, GasBarrelVM).ClearInput
        ClearControls(tpGasBarrel)
    End Sub

    '基本資料-瓦斯桶管理-取消
    Private Sub btnCancel_gb_Click(sender As Object, e As EventArgs) Handles btnCancel_gb.Click
        _gasBarrel.LoadList()
    End Sub

    '基本資料-瓦斯桶管理-dgv
    Private Sub dgvGB_SelectionChanged(sender As Object, e As EventArgs) Handles dgvGB.SelectionChanged, dgvGB.CellMouseClick
        If Not sender.focused Then Return
        Dim id As Integer = dgvGB.SelectedRows(0).Cells("編號").Value
        _gasBarrel.LoadDetail(id)
    End Sub

    '基本資料-瓦斯桶管理-修改
    Private Sub btnEdit_gb_Click(sender As Object, e As EventArgs) Handles btnEdit_gb.Click
        Dim id As Integer
        If Integer.TryParse(txtId_gb.Text, id) Then
            _gasBarrel.Update()
        Else
            MsgBox("請選擇對象")
        End If
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

    Public Sub IPurchaseView_SetCompanyCmb(items As List(Of SelectListItem)) Implements IPurchaseView.SetCompanyCmb
        SetComboBox(cmbCompany_pur, items)
    End Sub

    Public Sub IPurchaseView_SetGasVendorCmb(items As List(Of SelectListItem)) Implements IPurchaseView.SetGasVendorCmb
        SetComboBox(cmbGasVendor_pur, items)
    End Sub

    Public Sub IPurchaseView_SetDriveVendorCmb(items As List(Of SelectListItem)) Implements IPurchaseView.SetDriveVendorCmb
        SetComboBox(cmbDriveCmp, items)
    End Sub

    Public Sub IPurchaseView_SetSubjectCmb(items As List(Of SelectListItem)) Implements IPurchaseView.SetSubjectCmb
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

    '採購管理-大氣採購-取消
    Private Async Sub btnCancel_pur_Click(sender As Object, e As EventArgs) Handles btnCancel_pur.Click
        SetButtonState(sender, True)
        ClearControls(tpPurchase)
        Await _purchase.InitializeAsync
        If btnQuery_pur.Text = "確  認" Then SetPurchaseQueryCtrlsState()
        Await _purchase.LoadListAsync()
    End Sub

    '採購管理-大氣採購-新增
    Private Async Sub btnAdd_pur_Click(sender As Object, e As EventArgs) Handles btnAdd_pur.Click
        Await _purchase.AddAsync()
    End Sub

    '採購管理-大氣採購-dgv
    Private Async Sub dgvPurchase_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPurchase.SelectionChanged, dgvPurchase.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return

        SetButtonState(ctrl, False)

        Dim id As Integer = ctrl.SelectedRows(0).Cells("編號").Value
        Await _purchase.SelectRowAsync(id)
    End Sub

    '採購管理-大氣採購-修改
    Private Async Sub btnEdit_pur_Click(sender As Object, e As EventArgs) Handles btnEdit_pur.Click
        Await _purchase.EditAsync()
    End Sub

    '採購管理-大氣採購-刪除
    Private Async Sub btnDelete_pur_Click(sender As Object, e As EventArgs) Handles btnDelete_pur.Click
        Await _purchase.DeleteAsync(txtId_pur.Text)
    End Sub

    '採購管理-大氣採購-查詢
    Private Async Sub btnQuery_pur_Click(sender As Object, e As EventArgs) Handles btnQuery_pur.Click
        SetPurchaseQueryCtrlsState()

        If sender.Text = "查  詢" Then
            Await _purchase.LoadListAsync()
        End If
    End Sub

    '採購管理-大氣採購-選擇大氣廠商、產品
    Private Async Sub GetLastUnitPrice(sender As Object, e As EventArgs) Handles cmbGasVendor_pur.SelectionChangeCommitted, cmbProduct_pur.SelectionChangeCommitted
        If cmbGasVendor_pur.SelectedIndex > -1 AndAlso cmbProduct_pur.SelectedIndex > -1 Then
            Await _purchase.GetDefaultPriceAsync(cmbGasVendor_pur.SelectedItem.Value, cmbProduct_pur.SelectedItem)
        End If
    End Sub

    '採購管理-大氣採購-列印
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

    Public Sub SetManuCmb_uph(data As List(Of SelectListItem)) Implements IUnitPriceHistoryView.SetManuCmb
        SetComboBox(cmbManu_uph, data)
    End Sub

    Public Sub LoadList(data As List(Of UnitPriceHistoryVM)) Implements IUnitPriceHistoryView.LoadList
        dgvUPH.DataSource = data
    End Sub

    '採購管理-歷史單價查詢-取消
    Private Sub btnCancel_uph_Click(sender As Object, e As EventArgs) Handles btnCancel_uph.Click
        ClearControls(tpUPH)
        dgvUPH.DataSource = Nothing
        _uph.GetManuCmb()
    End Sub

    '採購管理-歷史單價查詢-查詢
    Private Sub btnQuery_UPH_Click(sender As Object, e As EventArgs) Handles btnQuery_UPH.Click
        Dim manuId As Integer = If(cmbManu_uph.SelectedItem Is Nothing, 0, cmbManu_uph.SelectedItem.Value)
        _uph.Query(manuId, cmbProduct_UPH.Text, dtpStart_UPH.Value.Date, dtpEnd_UPH.Value.Date)
    End Sub

    Public Sub DisplayList(data As List(Of PurchaseBarrelVM)) Implements IBaseView(Of purchase_barrel, PurchaseBarrelVM).DisplayList
        dgvPurchaseBarrel.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As purchase_barrel) Implements IBaseView(Of purchase_barrel, PurchaseBarrelVM).DisplayDetail
        AutoMapEntityToControls(data, tpPurchaseBarrel)
        AutoMapEntityToControls(data, grpBarrel)
    End Sub

    Private Function IBaseView_GetUserInput2() As purchase_barrel Implements IBaseView(Of purchase_barrel, PurchaseBarrelVM).GetUserInput
        Dim data As New purchase_barrel
        AutoMapControlsToEntity(data, tpPurchaseBarrel)
        AutoMapControlsToEntity(data, grpBarrel)
        Return data
    End Function

    Private Sub IBaseView_ClearInput2() Implements IBaseView(Of purchase_barrel, PurchaseBarrelVM).ClearInput
        ClearControls(tpPurchaseBarrel)
    End Sub

    Public Sub SetVendorCmb(data As List(Of SelectListItem)) Implements IPurchaseBarrelView.SetVendorCmb
        SetComboBox(cmbVendor_pb, data)
    End Sub

    Private Function IPurchaseBarrelView_GetSearchCriteria() As PurBarrelSC Implements IPurchaseBarrelView.GetSearchCriteria
        Return New PurBarrelSC With {
            .EndDate = dtpEndDate_pb.Value.Date,
            .IsDate = chkIsDate_pb.Checked,
            .StartDate = dtpStartDate_pb.Value.Date,
            .VendorId = cmbVendor_pb.SelectedItem?.Value
        }
    End Function

    '採購管理-新瓶採購-新增
    Private Sub btnAdd_pb_Click(sender As Object, e As EventArgs) Handles btnAdd_pb.Click
        _purchaseBarrel.Add()
    End Sub

    '採購管理-新瓶採購-修改
    Private Sub btnEdit_pb_Click(sender As Object, e As EventArgs) Handles btnEdit_pb.Click
        _purchaseBarrel.Update()
    End Sub

    '採購管理-新瓶採購-刪除
    Private Sub btnDelete_pb_Click(sender As Object, e As EventArgs) Handles btnDelete_pb.Click
        If MsgBox("確認要刪除嗎?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            _purchaseBarrel.Delete()
        End If
    End Sub

    '採購管理-新瓶採購-取消
    Private Sub btnCancel_pb_Click(sender As Object, e As EventArgs) Handles btnCancel_pb.Click
        SetButtonState(sender, True)
        _purchaseBarrel.Initialize()
    End Sub

    '採購管理-新瓶採購-查詢
    Private Sub btnQuery_pb_Click(sender As Object, e As EventArgs) Handles btnQuery_pb.Click
        SetPurchaseBarrelCtrlState()

        If sender.Text = "查  詢" Then
            _purchaseBarrel.LoadList()
        End If
    End Sub

    '採購管理-新瓶採購-dgv
    Private Sub dgvPurchaseBarrel_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPurchaseBarrel.SelectionChanged, dgvPurchaseBarrel.CellMouseClick
        If Not dgvPurchaseBarrel.Focused Then Return

        SetButtonState(sender, False)

        Dim row = dgvPurchaseBarrel.SelectedRows(0)
        Dim id As Integer = row.Cells("編號").Value

        _purchaseBarrel.LoadDetail(id)
    End Sub

    ''' <summary>
    ''' 設定新瓶採購查詢欄位變化
    ''' </summary>
    Private Sub SetPurchaseBarrelCtrlState()
        Dim lst As New List(Of Control) From {lblVendor_pb}
        SetQueryControls(btnQuery_pb, lst)
    End Sub

    ''' <summary>
    ''' 計算總計
    ''' </summary>
    Private Sub CalculateAmount_PurchaseBarrel()
        Dim amount As Integer = 0
        Dim weights = grpBarrel.Controls.OfType(Of TextBox).
                                         Where(Function(txt) txt.Name.StartsWith("txtQty_")).
                                         Select(Function(txt) txt.Name.Substring(7)).
                                         ToList
        For Each weight In weights
            Dim qtyTxt = DirectCast(grpBarrel.Controls("txtQty_" & weight), TextBox)
            Dim unitPriceTxt = DirectCast(grpBarrel.Controls("txtUnitPrice_" & weight), TextBox)
            Dim qty As Integer
            Dim unitPrice As Integer

            If Integer.TryParse(qtyTxt.Text, qty) AndAlso Integer.TryParse(unitPriceTxt.Text, unitPrice) Then
                amount += qty * unitPrice
            End If
        Next

        txtAmount_pb.Text = amount
    End Sub

    Private Sub IOrderView_ClearInput() Implements IBaseView(Of order, OrderVM).ClearInput
        ClearControls(tpOrder)
        ClearControls(tpIn)
        ClearControls(tpOut)

        tpOut.Parent = tcInOut
        tpIn.Parent = tcInOut
        txtOperator.Text = User.Name
        cmbCar_ord.DataSource = Nothing
        cmbCarOut_ord.DataSource = Nothing

        If btnQuery_order.Text = "確  認" Then SetOrderQueryCtrl(btnQuery_order)

        SetButtonState(btnCancel_order, True)

        '預設運送方式
        grpTransport.Controls.OfType(Of RadioButton).First(Function(x) x.Text = "自運").Checked = True

        txtCusCode_ord.ReadOnly = False
        btnQueryCus_ord.Enabled = True
        cmbCar_ord.Enabled = True
        cmbCarOut_ord.Enabled = True
        grpTransport.Enabled = True
    End Sub

    Public Sub DisplayList(data As List(Of OrderVM)) Implements IBaseView(Of order, OrderVM).DisplayList
        dgvOrder.DataSource = data
        AddHandler dgvOrder.CellFormatting, AddressOf DgvOrder_CellFormatting
    End Sub

    Public Sub DisplayCustomer(data As customer) Implements IOrderView.DisplayCustomer
        AutoMapEntityToControls(data, tpOrder)
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

    Public Sub SetCarDropdown(list As List(Of SelectListItem)) Implements IOrderView.SetCarDropdown
        SetComboBox(cmbCar_ord, New List(Of SelectListItem)(list))
        SetComboBox(cmbCarOut_ord, New List(Of SelectListItem)(list))
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

    Public Sub DisplayGasAndPrice(gas As Integer, gasC As Integer, amount As Single, insurance As Single) Implements IOrderView.DisplayGasAndPrice
        txtTotalGas.Text = gas
        txtTotalGas_c.Text = gasC
        txtAmount_ord.Text = amount
        txtInsurance.Text = insurance
    End Sub

    Public Function GetOrderInput() As order Implements IOrderView.GetOrderInput
        Dim data As New order
        AutoMapControlsToEntity(data, tpOrder)
        Return data
    End Function

    Private Function IOrderView_GetUserInput() As order Implements IBaseView(Of order, OrderVM).GetUserInput
        Dim data As New order

        AutoMapControlsToEntity(data, tpOrder)
        AutoMapControlsToEntity(data, tcInOut.SelectedTab)
        data.o_Operator = User.Id
        Return data
    End Function

    Public Sub DisplayDetail(data As order) Implements IBaseView(Of order, OrderVM).DisplayDetail
        AutoMapEntityToControls(data.customer, tpOrder)
        _order.LoadCar()
        AutoMapEntityToControls(data, tpOrder)
        txtOperator.Text = data.employee.emp_name

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

    Private Function IOrderView_GetSearchCriteria() As OrderSearchCriteria Implements IOrderView.GetSearchCriteria
        Dim cusId As Integer = If(String.IsNullOrEmpty(txtCusID_order.Text), 0, txtCusID_order.Text)
        Dim data As New OrderSearchCriteria With {
            .CusId = cusId,
            .EndDate = dtpEnd_order.Value,
            .IsDate = chkIsDate_ord.Checked,
            .StartDate = dtpStart_order.Value
        }

        Return data
    End Function

    Public Sub DisplayInsurance(price As Single) Implements IOrderView.DisplayInsurance
        txtInsurance.Text = price
    End Sub

    Private Sub DgvOrder_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim columnName = dgvOrder.Columns(e.ColumnIndex).Name
            If columnName.Contains("丙氣") Then
                e.CellStyle.ForeColor = Color.Red
            End If
        End If
    End Sub

    Public Sub GetCusStkInput(currentEntity As customer) Implements IOrderView.GetCusStkInput
        AutoMapControlsToEntity(currentEntity, tcInOut.SelectedTab)
    End Sub

    Public Sub GetCarStkInput(currentEntity As car) Implements IOrderView.GetCarStkInput
        AutoMapControlsToEntity(currentEntity, tcInOut.SelectedTab)
    End Sub

    '銷售管理-取消
    Private Async Sub btnCancel_order_Click(sender As Object, e As EventArgs) Handles btnCancel_order.Click
        SetButtonState(sender, True)
        Await _order.InitializeAsync()
        txtOperator.Text = User.Name
        dtpOrder.Value = Now
        txtCusCode_ord.Focus()
    End Sub

    '銷售管理-搜尋客戶
    Private Sub btnQueryCus_ord_Click(sender As Object, e As EventArgs) Handles btnQueryCus_ord.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                _order.LoadCustomerByCusCode(searchForm.CusCode)
                dtpOrder.Focus()
            End If
        End Using
    End Sub

    '銷售管理-客戶代號
    Private Sub txtCusCode_ord_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_ord.KeyDown
        '按下Enter時,搜尋客戶資料
        If e.KeyCode = Keys.Enter AndAlso _order.LoadCustomerByCusCode(txtCusCode_ord.Text) Then
            dtpOrder.Focus()
            If rdoPickUp.Checked Then _order.LoadCar()
        End If
    End Sub

    '銷售管理-日期
    Private Sub dtpo_date_KeyDown(sender As Object, e As KeyEventArgs) Handles dtpOrder.KeyDown
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

    '銷售管理-車號-選項改變時
    Private Sub cmbCarNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCar_ord.SelectedIndexChanged
        Dim cmb As ComboBox = sender

        If cmb.SelectedIndex >= 0 Then
            If cmbCarOut_ord.Items.Count > 0 Then
                '同步"出場單"車號
                cmbCarOut_ord.SelectedIndex = cmb.SelectedIndex
            End If

            _order.LoadCarStk_In(cmb.SelectedItem.Value)

            If cmb.Focused Then
                '新增時切換車號要重新計算
                _order.CalculateCarStk(True)
            End If
        Else
            tpIn.Controls.OfType(Of TextBox).
                Where(Function(txt) txt.Tag.ToString.StartsWith("c_deposit")).
                ToList().
                ForEach(Sub(t) t.Clear())
        End If
    End Sub

    '銷售管理-車號-按下Enter
    Private Sub cmbCar_ord_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbCar_ord.KeyDown
        If e.KeyCode = Keys.Enter Then
            tcInOut.Focus()
        End If
    End Sub

    '銷售管理-出場單-車號-選項改變時
    Private Sub cmbCarOut_ord_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCarOut_ord.SelectedIndexChanged
        Dim cmb As ComboBox = sender

        If cmb.SelectedIndex >= 0 Then
            _order.LoadCarStk_Out(cmb.SelectedItem.Value)

            If cmb.Focused Then
                '新增時切換車號要重新計算
                _order.CalculateCarStk(False)
            End If
        Else
            tpOut.Controls.OfType(Of TextBox).
                Where(Function(txt) txt.Tag.ToString.StartsWith("c_deposit")).
                ToList().
                ForEach(Sub(t) t.Clear())
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

    '銷售管理-控制項事件
    Private Sub SetupSalesManagementHandlers()
        ' 進場單處理
        SetupTextBoxHandlers(tpIn, AddressOf CalculateOrder, "txto_in", "txto_new_in", "txto_inspect")
        SetupTextBoxHandlers(tpIn, AddressOf CalculateCarStock, "txtDepositIn")

        ' 為所有非只讀的TextBox添加方向鍵處理
        AddDirectionHandlers(tpIn)

        ' 出場單處理
        SetupTextBoxHandlers(tpOut, AddressOf CalculateOrder, "txtGas", "txtEmpty")
        SetupTextBoxHandlers(tpOut, AddressOf CalculateCarStock, "txtDepositOut")

        AddDirectionHandlers(tpOut)

        ' 訂單處理
        SetupTextBoxHandlers(tpOrder, AddressOf CalculateOrder, "txto_return", "txto_return_c", "txto_sales_allowance")
    End Sub

    Private Sub SetupTextBoxHandlers(container As Control, handler As KeyEventHandler, ParamArray prefixes As String())
        For Each txt In container.Controls.OfType(Of TextBox)()
            If prefixes.Any(Function(prefix) txt.Name.StartsWith(prefix)) Then
                AddHandler txt.KeyUp, handler
            End If
        Next
    End Sub

    ''' <summary>
    ''' 即時計算進出場單
    ''' </summary>
    Private Sub CalculateOrder()
        Dim isIn As Boolean = tcInOut.SelectedTab.Text = "進場單"
        _order.CalculateStkAndPrice(isIn)
    End Sub

    ''' <summary>
    ''' 即時計算車寄桶存量
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CalculateCarStock(sender As Object, e As KeyEventArgs)
        Dim btn As TextBox = sender
        Dim isIn As Boolean = btn.Parent.Name = "tpIn"
        _order.CalculateCarStk(isIn)
    End Sub

    Private Sub AddDirectionHandlers(container As Control)
        For Each txt In container.Controls.OfType(Of TextBox)().Where(Function(x) Not x.ReadOnly)
            AddHandler txt.KeyDown, Sub(sender, e) Direction(sender, e, container)
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

    '銷售管理-切換進出場單
    Private Sub tcInOut_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tcInOut.SelectedIndexChanged
        Dim tc As TabControl = sender

        '顯示明細的狀況下只會有顯示一種單,不用計算
        If tc.TabPages.Count = 1 Then Return

        Dim isIn As Boolean = tc.SelectedTab.Text = "進場單"
        Dim exception = New List(Of String) From {grpTransport.Name}

        If isIn Then
            ClearControls(tpOut, exception)
            If rdoPickUp.Checked AndAlso cmbCar_ord.SelectedItem IsNot Nothing Then _order.LoadCarStk_In(cmbCar_ord.SelectedItem.Value)
        Else
            ClearControls(tpIn, exception)
            cmbCarOut_ord.SelectedIndex = cmbCar_ord.SelectedIndex
            If cmbCarOut_ord.SelectedItem IsNot Nothing Then _order.LoadCarStk_Out(cmbCarOut_ord.SelectedItem.Value)
        End If
        _order.CalculateStkAndPrice(isIn)
    End Sub

    '銷售管理-客戶編號改變時
    Private Sub txtCusID_order_TextChanged(sender As Object, e As EventArgs) Handles txtCusID_order.TextChanged
        'Dim txt As TextBox = sender
        'Dim id As Integer

        'ClearControls(tpIn)
        'ClearControls(tpOut)

        'If Integer.TryParse(txt.Text, id) Then
        '    _order.GetCusStk(id)
        '    _order.LoadCmbCar(id)
        'End If
    End Sub

    '銷售管理-選擇廠運時
    Private Sub rdoDelivery_CheckedChanged(sender As Object, e As EventArgs) Handles rdoDelivery.CheckedChanged
        Dim rdo As RadioButton = sender
        If rdo.Checked Then
            '清空車號
            cmbCar_ord.DataSource = Nothing
            cmbCarOut_ord.DataSource = Nothing
            _order.CurrentCarIn = Nothing
            _order.CurrentCarOut = Nothing

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

    '銷售管理-選擇自運時
    Private Sub rdoPickUp_CheckedChanged(sender As Object, e As EventArgs) Handles rdoPickUp.CheckedChanged
        Dim rdo As RadioButton = sender
        If rdo.Checked AndAlso dgvOrder.Columns.Count > 0 Then
            _order.LoadCar()
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

    '銷售管理-新增
    Private Sub btnCreate_ord_Click(sender As Object, e As EventArgs) Handles btnCreate_ord.Click
        _order.Add()
    End Sub

    '銷售管理-dgv
    Private Sub dgvOrder_SelectionChanged(sender As Object, e As EventArgs) Handles dgvOrder.SelectionChanged, dgvOrder.CellMouseClick
        If Not dgvOrder.Focused Then Return
        Dim ordId As Integer = dgvOrder.SelectedRows(0).Cells("編號").Value
        _order.LoadDetail(ordId)
        SetButtonState(sender, False)
        dgvOrder.Focus()
        txtCusCode_ord.ReadOnly = True
        btnQueryCus_ord.Enabled = False
        cmbCar_ord.Enabled = False
        cmbCarOut_ord.Enabled = False
        grpTransport.Enabled = False
    End Sub

    '銷售管理-刪除
    Private Sub btnDelete_order_Click(sender As Object, e As EventArgs) Handles btnDelete_order.Click
        _order.Delete()
    End Sub

    '銷售管理-快捷鍵
    Private Sub frmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If TabControl1.SelectedTab.Name <> "tpOrder" Then Exit Sub

        Select Case e.KeyCode
            Case Keys.F1
                If btnCreate_ord.Enabled = True Then _order.Add()

            Case Keys.F3
                If btnDelete_order.Enabled = True Then _order.Delete()

            Case Keys.F4
                btnCancel_order.PerformClick()
        End Select
    End Sub

    '銷售管理-查詢
    Private Async Sub btnQuery_order_Click(sender As Object, e As EventArgs) Handles btnQuery_order.Click
        Dim btn As Button = btnQuery_order
        SetOrderQueryCtrl(btn)

        If btn.Text = "查  詢" Then
            Await _order.LoadList()
        End If
    End Sub

    '銷售管理-列印
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim id As Integer
        If Integer.TryParse(txto_id.Text, id) Then
            _order.Print(id)
        Else
            MsgBox("請選擇對象")
        End If
    End Sub

    '銷售管理-列印客戶寄桶結存瓶
    Private Sub btnCusGasCylinderInventory_Click(sender As Object, e As EventArgs) Handles btnCusGasCylinderInventory.Click
        '要產生全部客戶還是個別客戶?
    End Sub

    '銷售管理-修改
    Private Sub btnEdit_ord_Click(sender As Object, e As EventArgs) Handles btnEdit_ord.Click
        _order.Update()
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

    Public Sub ShowList(data As List(Of CompanyVM)) Implements ICommonView_old(Of company, CompanyVM).ShowList
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

    Public Function SetRequired() As List(Of Control) Implements ICommonView_old(Of company, CompanyVM).SetRequired
        Return New List(Of Control) From {txtName_comp, txtShortName, txtTaxID, txtInitGasStock}
    End Function

    '基本資料-公司管理-取消
    Private Sub btnCancel_comp_Click(sender As Object, e As EventArgs) Handles btnCancel_comp.Click
        ClearInput()
        SetButtonState(sender, True)
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

    Public Sub ShowList(data As List(Of ManufacturerVM)) Implements ICommonView_old(Of manufacturer, ManufacturerVM).ShowList
        dgvManufacturer.DataSource = data
    End Sub

    Public Sub SetDataToControl(data As manufacturer) Implements ICommonView_old(Of manufacturer, ManufacturerVM).SetDataToControl
        AutoMapEntityToControls(data, tpManu)
    End Sub

    Private Function GetUserInput_Manufacturer() As manufacturer Implements ICommonView_old(Of manufacturer, ManufacturerVM).GetUserInput
        Dim data As New manufacturer
        AutoMapControlsToEntity(data, tpManu)
        Return data
    End Function

    Private Sub ICommonView_ClearInput() Implements ICommonView_old(Of manufacturer, ManufacturerVM).ClearInput
        ClearControls(tpManu)
    End Sub

    Public Function GetSearchConditions() As manufacturer Implements IManufacturerView.GetSearchConditions
        Return New manufacturer With {
            .manu_code = txtCode_manu.Text,
            .manu_name = txtName_menu.Text,
            .manu_phone1 = txtphone1_menu.Text
        }
    End Function

    Private Function SetRequired_Manufacturer() As List(Of Control) Implements ICommonView_old(Of manufacturer, ManufacturerVM).SetRequired
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

    Public Sub ShowList(data As List(Of PricePlanVM)) Implements ICommonView_old(Of priceplan, PricePlanVM).ShowList
        dgvPricePlan.DataSource = data
    End Sub

    Public Sub SetDataToControl(data As priceplan) Implements ICommonView_old(Of priceplan, PricePlanVM).SetDataToControl
        AutoMapEntityToControls(data, tpPricePlan)
    End Sub

    Private Function GetUserInput_PricePlan() As priceplan Implements ICommonView_old(Of priceplan, PricePlanVM).GetUserInput
        Dim data = New priceplan
        AutoMapControlsToEntity(data, tpPricePlan)
        Return data
    End Function

    Private Sub ClearInput_PricePlan() Implements ICommonView_old(Of priceplan, PricePlanVM).ClearInput
        ClearControls(tpPricePlan)
    End Sub

    Private Function SetRequired_PricePlan() As List(Of Control) Implements ICommonView_old(Of priceplan, PricePlanVM).SetRequired
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

    Public Sub SetRolesCmb(data As List(Of SelectListItem)) Implements IEmployeeView.SetRolesCmb
        SetComboBox(cmbRoles, data)
    End Sub

    Public Sub ShowList(data As List(Of EmployeeVM)) Implements ICommonView_old(Of employee, EmployeeVM).ShowList
        dgvEmployee.DataSource = data
    End Sub

    Public Sub SetDataToControl(data As employee) Implements ICommonView_old(Of employee, EmployeeVM).SetDataToControl
        AutoMapEntityToControls(data, tpEmployee)
    End Sub

    Private Function GetUserInput_emp() As employee Implements ICommonView_old(Of employee, EmployeeVM).GetUserInput
        Dim data As New employee
        AutoMapControlsToEntity(data, tpEmployee)
        Return data
    End Function

    Private Sub ClearInput_emp() Implements ICommonView_old(Of employee, EmployeeVM).ClearInput
        ClearControls(tpEmployee)
    End Sub

    Private Function SetRequired_Employee() As List(Of Control) Implements ICommonView_old(Of employee, EmployeeVM).SetRequired
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
        If Not ctrl.Focused OrElse ctrl.SelectedRows.Count = 0 Then Return

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

    Public Sub ShowList(data As List(Of BankVM)) Implements ICommonView_old(Of bank, BankVM).ShowList
        dgvBank.DataSource = data
    End Sub

    Public Sub SetDataToControl(data As bank) Implements ICommonView_old(Of bank, BankVM).SetDataToControl
        AutoMapEntityToControls(data, tpBank)
    End Sub

    Private Function GetUserInput_bank() As bank Implements ICommonView_old(Of bank, BankVM).GetUserInput
        Try
            Dim data As New bank
            AutoMapControlsToEntity(data, tpBank)
            Return data
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub ClearInput_bank() Implements ICommonView_old(Of bank, BankVM).ClearInput
        ClearControls(tpBank)
    End Sub

    Private Function SetRequired_Bank() As List(Of Control) Implements ICommonView_old(Of bank, BankVM).SetRequired
        Return New List(Of Control) From {txtBankName, txtAccountName, txtAccount_bank, txtInitialBalance}
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

    Public Sub PopulateVendorDropdown(data As IReadOnlyList(Of SelectListItem)) Implements IPaymentView.PopulateVendorDropdown
        SetComboBox(cmbManu_payment, data)
    End Sub

    Public Sub PopulateBankDropdown(data As IReadOnlyList(Of SelectListItem)) Implements IPaymentView.PopulateBankDropdown
        SetComboBox(cmbBank_payment, data)
    End Sub

    Public Sub PopulateSubjectDropdown(data As IReadOnlyList(Of SelectListItem)) Implements IPaymentView.PopulateSubjectDropdown
        SetComboBox(cmbSubjects_payment, data)
    End Sub

    Public Sub PopulateCompanyDropdown(data As IReadOnlyList(Of SelectListItem)) Implements IPaymentView.PopulateCompanyDropdown
        SetComboBox(cmbCompany_payment, data)
    End Sub

    Public Sub DisplayAmountDueList(data As IReadOnlyList(Of AmountDueVM)) Implements IPaymentView.DisplayAmountDueList
        dgvAmountDue.DataSource = data
    End Sub

    Public Function GetSearchCriteria() As PaymentSearchCriteria Implements IPaymentView.GetSearchCriteria
        Return New PaymentSearchCriteria With {
            .BankId = cmbBank_payment.SelectedItem?.Value,
            .ChequeNo = txtCheNo_payment.Text.Trim,
            .CompanyId = cmbCompany_payment.SelectedItem?.Value,
            .IsSearchDate = chkIsSearchDate_payment.Checked,
            .StartDate = dtpStart_payment.Value.Date,
            .EndDate = dtpEnd_payment.Value.Date.AddDays(1),
            .SubjectId = cmbSubjects_payment.SelectedItem?.Value,
            .VendorId = cmbManu_payment.SelectedItem?.Value
        }
    End Function

    Public Sub DisplayList(data As List(Of PaymentVM)) Implements IBaseView(Of payment, PaymentVM).DisplayList
        dgvPayment.DataSource = data
    End Sub

    Private Sub IPaymentView_SetDataToControl(data As payment) Implements IBaseView(Of payment, PaymentVM).DisplayDetail
        AutoMapEntityToControls(data, tpPayment)
    End Sub

    Private Function IPaymentView_GetUserInput() As payment Implements IBaseView(Of payment, PaymentVM).GetUserInput
        Dim data As New payment
        AutoMapControlsToEntity(data, tpPayment)

        If Not data.p_comp_Id.HasValue Then Throw New Exception("請選擇公司")

        If String.IsNullOrEmpty(data.p_Type) Then
            Throw New Exception("請選擇付款類型")
        ElseIf data.p_Type = "銀行" AndAlso Not data.p_bank_Id.HasValue Then
            Throw New Exception("請選擇銀行帳號")
        ElseIf data.p_Type = "支票" AndAlso String.IsNullOrEmpty(data.p_Cheque) Then
            Throw New Exception("請選擇支票號碼")
        End If

        If Not data.p_s_Id.HasValue Then Throw New Exception("請選擇科目")

        If data.p_Amount <= 0 Then Throw New Exception("請選擇輸入金額")

        If data.p_Type <> "支票" Then data.p_CashingDate = Nothing

        '格式化帳款月份
        Dim accountMonth = data.p_AccountMonth.Value
        Dim year = accountMonth.Year
        Dim month = accountMonth.Month
        data.p_AccountMonth = New Date(year, month, 1)

        Return data
    End Function

    Private Sub IPaymentView_ClearInput() Implements IBaseView(Of payment, PaymentVM).ClearInput
        ClearControls(tpPayment)
        dgvAmountDue.DataSource = Nothing
    End Sub

    '支出管理-付款作業-取消
    Private Async Sub btnCancel_payment_Click(sender As Object, e As EventArgs) Handles btnCancel_payment.Click
        SetButtonState(sender, True)
        Await _payment.InitializeAsync
        If btnQuery_payment.Text = "確  認" Then SetPaymentQueryCtrlsState()
    End Sub

    '支出管理-付款作業-新增
    Private Sub btnAdd_payment_Click(sender As Object, e As EventArgs) Handles btnAdd_payment.Click
        _payment.Add()
    End Sub

    '支出管理-付款作業-dgv
    Private Async Sub dgvPayment_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPayment.SelectionChanged, dgvPayment.CellMouseClick

        Dim id = DGV_SelectionChanged(sender)
        If id > 0 Then
            Await _payment.LoadPaymentDetailAsync(id)
            _payment.LoadVendorAmountDue(cmbManu_payment.SelectedItem.Value)
        End If
    End Sub

    '支出管理-付款作業-修改
    Private Sub btnEdit_payment_Click(sender As Object, e As EventArgs) Handles btnEdit_payment.Click
        _payment.Update()
    End Sub

    '支出管理-付款作業-刪除
    Private Async Sub btnDelete_payment_Click(sender As Object, e As EventArgs) Handles btnDelete_payment.Click
        Await _payment.DeleteAsync(txtId_payment.Text)
        SetButtonState(sender, True)
    End Sub

    '支出管理-付款作業-查詢
    Private Async Sub btnQuery_payment_Click(sender As Object, e As EventArgs) Handles btnQuery_payment.Click
        SetPaymentQueryCtrlsState()

        If sender.Text = "查  詢" Then
            Await _payment.SearchPaymentsAsync()
        End If
    End Sub

    ''' <summary>
    ''' 設定付款作業查詢控制項狀態
    ''' </summary>
    Private Sub SetPaymentQueryCtrlsState()
        Dim lst As New List(Of Control) From {lblPayType_payment, lblSubjects_payment, lblManu_payment, lblBank_payment}
        SetQueryControls(btnQuery_payment, lst)
    End Sub

    '支出管理-付款作業-付款類型切換
    Private Sub cmbPayType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPayType.SelectedIndexChanged
        Dim cmb As ComboBox = sender
        Dim paymentType As String = cmb.Text
        Dim ctrlVisibility As New Dictionary(Of String, Control()) From {
            {"支票", {lblReq_Chuque, lblCheNo_payment, txtCheNo_payment, chkCashing, lblCashingDate_payment, dtpCashing}},
            {"銀行", {lblBankRequired_payment, lblBank_payment, cmbBank_payment}}
        }

        For Each kvp In ctrlVisibility
            Dim isVisible As Boolean = (paymentType = kvp.Key)
            For Each ctrl In kvp.Value
                ctrl.Visible = isVisible
            Next
        Next
    End Sub

    '支出管理-付款作業-選擇廠商時搜尋應付未付
    Private Sub cmbManu_payment_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbManu_payment.SelectionChangeCommitted
        _payment.LoadVendorAmountDue(cmbManu_payment.SelectedValue)
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

    Public Sub ShowList(data As List(Of CollectionVM)) Implements ICommonView_old(Of collection, CollectionVM).ShowList
        dgvCollection.DataSource = data
    End Sub

    Public Sub SetDataToControl(col As collection, Optional che As cheque = Nothing) Implements ICollectionView.SetDataToControl
        AutoMapEntityToControls(col, tpCollection)

        If che IsNot Nothing Then
            AutoMapEntityToControls(che, tpCollection)
        End If
    End Sub

    Public Sub SetDataToControl(data As collection) Implements ICommonView_old(Of collection, CollectionVM).SetDataToControl

    End Sub

    Private Function GetUserInput_collection() As collection Implements ICommonView_old(Of collection, CollectionVM).GetUserInput
        Dim list As New List(Of Control) From {txtAmount_collection, cmbType_col, cmbSubjects, txtCusId_col, cmbCompany_col}

        If cmbType_col.Text = "支票" Then
            Dim ctrls As Control() = {txtCheque_col, txtIssuerName, txtCheAcctNum}
            list.AddRange(ctrls)
        End If

        If Not CheckRequired(list) Then Return Nothing

        Dim data As New collection
        AutoMapControlsToEntity(data, tpCollection)

        Dim accountMonth = data.col_AccountMonth
        Dim formatAccountMonth = New Date(accountMonth.Year, accountMonth.Month, 1)
        data.col_AccountMonth = formatAccountMonth
        Return data
    End Function

    Private Sub ClearInput_collection() Implements ICommonView_old(Of collection, CollectionVM).ClearInput
        ClearControls(tpCollection)
    End Sub

    Private Sub ICollectionView_SetSubjectsCmb(data As IReadOnlyList(Of SelectListItem)) Implements ICollectionView.SetSubjectsCmb
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
            .che_AccountNumber = txtCheAcctNum.Text,
            .che_IssuerName = txtIssuerName.Text,
            .chu_State = "未兌現",
            .che_AbleCashingDate = dtpAbleCashingDate.Value,
            .che_PayBankName = txtPayBank.Text
        }
    End Function

    Private Sub ICollectionView_SetBankCmb(data As List(Of SelectListItem)) Implements ICollectionView.SetBankCmb
        SetComboBox(cmbBank_col, data)
    End Sub

    Public Sub SetCompanyCmb(data As List(Of SelectListItem)) Implements ICollectionView.SetCompanyCmb
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
        _collect.GetSubjectsCmbAsync()
        _collect.LoadList()

        If btnQuery_col.Text = "確  認" Then SetOrderQueryCtrl(btnQuery_col)
        SetButtonState(btnCancel_col, True)

        btnCashing.Visible = False
        cmbType_col.Enabled = True
    End Sub

    '收入管理-收款作業-收款類型-支票號碼顯示
    Private Sub cmbType_col_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbType_col.SelectedIndexChanged
        Dim cmb As ComboBox = sender
        Dim collectionType As String = cmb.Text
        Dim ctrlVisibility As New Dictionary(Of String, Control()) From {
            {"支票", {lblChequeReq_col, lblCheque_col, txtCheque_col, lblCashingDate_col, txtCashingDate, lblIssuerNameReq, lblIssuerName, txtIssuerName, lblChequeAccountNumberReq,
                      lblChequeAccountNumber, txtCheAcctNum, lblAbleCashingDate, dtpAbleCashingDate, lblPayBank, txtPayBank}}
        }

        For Each kvp In ctrlVisibility
            Dim isVisible As Boolean = (collectionType = kvp.Key)
            For Each ctrl In kvp.Value
                ctrl.Visible = isVisible
            Next
        Next
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
        _collect.DeleteAsync(id)
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
        AutoMapEntityToControls(data, tpCheque)
    End Sub

    Public Function GetUserInput() As cheque Implements ICommonView_old(Of cheque, ChequeVM).GetUserInput
        Dim data As New cheque
        AutoMapControlsToEntity(data, tpCheque)
        Return data
    End Function

    Private Sub ClearInput_Cheque() Implements ICommonView_old(Of cheque, ChequeVM).ClearInput
        ClearControls(tpCheque)
    End Sub

    Private Function SetRequired_Cheque() As List(Of Control) Implements ICommonView_old(Of cheque, ChequeVM).SetRequired
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

    Private Function ICommonView_SetRequired() As List(Of Control) Implements ICommonView_old(Of collection, CollectionVM).SetRequired
        Throw New NotImplementedException()
    End Function

    Public Sub SetGasVendorCmb(item As List(Of SelectListItem)) Implements IReportView.SetGasVendorCmb
        SetComboBox(cmbManu, item)
    End Sub

    Public Sub SetBankAccountCmb(items As List(Of SelectListItem)) Implements IReportView.SetBankAccountCmb
        SetComboBox(cmbBankAccount_BankAccount, items)
        SetComboBox(cmbBankAccount_br, items)
    End Sub

    Private Sub IReportView_SetCompanyCmb(items As List(Of SelectListItem)) Implements IReportView.SetCompanyCmb
        SetComboBox(cmbCompany_br, items)
        SetComboBox(cmbCompany_ITD, items)
    End Sub

    '會計管理-報表-刷新
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        _report.LoadBankAccount()
        _report.LoadCompany()
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

    '會計管理-報表-銀行帳
    Private Sub btnBankAccount_Click(sender As Object, e As EventArgs) Handles btnBankAccount.Click
        Dim selectDate = dtpMonth_BankAccount.Value.Date
        Dim month = New Date(selectDate.Year, selectDate.Month, 1)
        _report.GenerateBankAccount(month, cmbBankAccount_BankAccount.SelectedItem.Value)
    End Sub

    '會計管理-報表-客戶寄桶結存瓶-客戶代號
    Private Sub txtCusCode_cgci_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_cgci.KeyDown
        '按下Enter時,搜尋客戶資料
        If e.KeyCode = Keys.Enter Then
            Dim cus = _collect.GetCustomer(txtCusCode_cgci.Text)

            If cus IsNot Nothing Then
                txtCusCode_cgci.Text = cus.cus_code
                txtCusName_cgci.Text = cus.cus_name
                txtCusId_cgci.Text = cus.cus_id
            Else
                MsgBox("查無此客戶")
            End If
        End If
    End Sub

    '會計管理-報表-客戶寄桶結存瓶-客戶搜尋
    Private Sub btnSearchCus_cgci_Click(sender As Object, e As EventArgs) Handles btnSearchCus_cgci.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusId_cgci.Text = searchForm.CusId
                txtCusName_cgci.Text = searchForm.CusName
                txtCusCode_cgci.Text = searchForm.CusCode
            End If
        End Using
    End Sub

    '會計管理-報表-客戶寄桶結存瓶-產生
    Private Sub btnCGCI_Click(sender As Object, e As EventArgs) Handles btnCGCI.Click
        _report.GenerateCustomerGasCylinderInventory(txtCusId_cgci.Text)
    End Sub

    '會計管理-報表-新桶明細
    Private Sub btnNewBarrel_Click(sender As Object, e As EventArgs) Handles btnNewBarrel.Click
        _report.GenerateNewBarrelDetails(dtpMonth_newBarrel.Value)
    End Sub

    '會計管理-報表-應收票據
    Private Sub btnGenerate_br_Click(sender As Object, e As EventArgs) Handles btnGenerate_br.Click
        _report.GenerateBillsReceivable(cmbCompany_br.SelectedItem.Value, cmbBankAccount_br.SelectedItem.Value, dtpMonth_br.Value)
    End Sub

    '會計管理-報表-發票
    Private Sub btnGenerate_RI_Click(sender As Object, e As EventArgs) Handles btnGenerate_RI.Click
        _report.GenerateInvoice(dtpMonth_RI.Value)
    End Sub

    '會計管理-報表-月應收帳明細
    Private Sub btnMAR_Click(sender As Object, e As EventArgs) Handles btnMAR.Click
        _report.GenerateMonthlyAccountsReceivable(dtpMonth_MAR.Value)
    End Sub

    '會計管理-報表-進銷存明細
    Private Sub btnITD_Click(sender As Object, e As EventArgs) Handles btnITD.Click
        _report.GenerateInventoryTransactionDetail(dtpYear_ITD.Value, cmbCompany_ITD.SelectedItem.Value, User.Id)
    End Sub

    '會計管理-報表-應付票據
    Private Sub btnPayableCheck_Click(sender As Object, e As EventArgs) Handles btnPayableCheck.Click
        _report.GeneratePayableCheck(dtpMonth_PayableCheck.Value)
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

    Public Sub LoadVendors(datas As List(Of SelectListItem)) Implements IGasCheckoutView.LoadVendors
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

    Public Sub DisplayList(data As List(Of InvioceVM)) Implements IBaseView(Of invoice, InvioceVM).DisplayList
        dgvInvoice.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As invoice) Implements IBaseView(Of invoice, InvioceVM).DisplayDetail
        AutoMapEntityToControls(data, tpInvoice)
        AutoMapEntityToControls(data.customer, tpInvoice)
    End Sub

    Private Function IInvoice_GetUserInput() As invoice Implements IBaseView(Of invoice, InvioceVM).GetUserInput
        Dim data As New invoice
        AutoMapControlsToEntity(data, tpInvoice)
        Return data
    End Function

    Private Sub IInvoice_ClearInput() Implements IBaseView(Of invoice, InvioceVM).ClearInput
        ClearControls(tpInvoice)
    End Sub

    Public Sub SetCustomer(data As customer) Implements IInvoiceView.SetCustomer
        txtCusId_invoice.Text = data.cus_id
        txtCusCode_invoice.Text = data.cus_code
        txtCusName_invoice.Text = data.cus_name
        txtInvoiceMemo_invoice.Text = data.cus_InvoiceMemo
    End Sub

    Private Function IInvoiceView_GetSearchCriteria() As InvoiceSearchCriteria Implements IInvoiceView.GetSearchCriteria
        Return New InvoiceSearchCriteria With {
            .CusId = If(String.IsNullOrEmpty(txtCusId_invoice.Text), 0, txtCusId_invoice.Text),
            .Month = dtpMonth_invoice.Value,
            .Number = txtNumber_invoice.Text,
            .IsSearchMonth = chkSearchMonth.Checked
        }
    End Function

    Public Sub DisplayPrices(unitPrice As Single, tax As Single, amount As Single) Implements IInvoiceView.DisplayPrices
        txtUnitPrice_invoice.Text = unitPrice
        txtTax.Text = tax
        txtAmount_invoice.Text = amount
    End Sub

    Public Sub DisplayInvoiceInfo(info As InvoiceInfoVM) Implements IInvoiceView.DisplayInvoiceInfo
        txtDeliNormTotal.Text = info.DeliNormTotal
        txtDeliCTotal.Text = info.DeliCTotal
        txtDeliNormInvoice.Text = info.DeliNormInvoice
        txtDeliCInvoice.Text = info.DeliCInvoice
        txtPickNormTotal.Text = info.PickNormTotal
        txtPickCTotal.Text = info.PickCTotal
        txtPickNormInvoice.Text = info.PickNormInvoice
        txtPickCInvoice.Text = info.PickCInvoice
    End Sub

    '會計管理-發票管理-客戶代號
    Private Sub txtCusCode_invoice_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_invoice.KeyDown
        '按下Enter時,搜尋客戶資料
        If e.KeyCode = Keys.Enter AndAlso Not String.IsNullOrEmpty(txtCusCode_invoice.Text) Then
            _invoice.GetCustomerByCusCode(txtCusCode_invoice.Text)
            _invoice.CalculatePrices(cmbType_invoice.SelectedItem)
            _invoice.LoadInvoiceInfo(txtCusId_invoice.Text, dtpMonth_invoice.Value)
        End If
    End Sub

    '會計管理-發票管理-搜尋客戶
    Private Sub btnSearchCus_invoice_Click(sender As Object, e As EventArgs) Handles btnSearchCus_invoice.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                SetCustomer(searchForm.Customer)
                _invoice.CalculatePrices(cmbType_invoice.SelectedItem)
                _invoice.LoadInvoiceInfo(txtCusId_invoice.Text, dtpMonth_invoice.Value)
            End If
        End Using
    End Sub

    '會計管理-發票管理-KG
    Private Sub txtKg_invoice_KeyUp(sender As Object, e As EventArgs) Handles txtKg_invoice.KeyUp
        _invoice.CalculatePrices(cmbType_invoice.SelectedItem)
    End Sub

    '會計管理-發票管理-取消
    Private Async Sub btnCancel_invoice_Click(sender As Object, e As EventArgs) Handles btnCancel_invoice.Click
        SetButtonState(sender, True)
        Await _invoice.LoadList()
    End Sub

    '會計管理-發票管理-新增
    Private Sub btnAdd_invoice_Click(sender As Object, e As EventArgs) Handles btnAdd_invoice.Click
        _invoice.AddAsync()
    End Sub

    '會計管理-發票管理-dgv
    Private Sub dgvInvoice_CellMouseClick(sender As Object, e As EventArgs) Handles dgvInvoice.CellMouseClick, dgvInvoice.SelectionChanged
        Dim ctrl As DataGridView = sender

        If Not ctrl.Focused Or ctrl.SelectedRows.Count = 0 Then Return

        SetButtonState(ctrl, False)

        Dim id = ctrl.SelectedRows(0).Cells(0).Value
        _invoice.LoadDetail(id)
    End Sub

    '會計管理-發票管理-修改
    Private Sub btnEdit_invoice_Click(sender As Object, e As EventArgs) Handles btnEdit_invoice.Click
        _invoice.Update()
        SetButtonState(sender, True)
    End Sub

    '會計管理-發票管理-刪除
    Private Sub btnDelete_invoice_Click(sender As Object, e As EventArgs) Handles btnDelete_invoice.Click
        If MsgBox("確定要刪除", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            _invoice.Delete()
            SetButtonState(sender, True)
        End If
    End Sub

    '會計管理-發票管理-查詢
    Private Async Sub btnSearch_invoice_Click(sender As Object, e As EventArgs) Handles btnSearch_invoice.Click
        SetInvoiceQueryCtrlsState()

        If sender.Text = "查  詢" Then
            Await _invoice.LoadList()
        End If
    End Sub

    '會計管理-發票管理-日期改變
    Private Sub dtpMonth_invoice_ValueChanged(sender As Object, e As EventArgs) Handles dtpMonth_invoice.ValueChanged
        _invoice.CalculatePrices(cmbType_invoice.SelectedItem)
    End Sub

    '會計管理-發票管理-種類選擇完成
    Private Sub cmbType_invoice_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbType_invoice.SelectionChangeCommitted
        _invoice.CalculatePrices(cmbType_invoice.SelectedItem)
    End Sub

    ''' <summary>
    ''' 設定發票管理查詢控制項狀態
    ''' </summary>
    Private Sub SetInvoiceQueryCtrlsState()
        Dim lst As New List(Of Control) From {lblCusCode_invoice, lblMonth_invoice, lblNumber_invoice}

        SetQueryControls(btnSearch_invoice, lst)
    End Sub
End Class