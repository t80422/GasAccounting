Public Class frmMain
    Implements ISubjectsView, IManufacturerView, IPricePlanView, IEmployeeView, IBankView, IUnitPriceHistoryView, IPermissionView, IBasicPriceView, IInvoiceView, IGasBarrelView, ICarView, IInvoiceSplitView, IInspectionView

    Public Structure UserData
        Public Id As Integer
        Public Name As String
    End Structure

    Public User As UserData


    Private _currentUserService As ICurrentUserService

    Private _manuService As IManufacturerService = New ManufacturerService

    Private _basicPrice As BasicPricePresenter
    Private _car As CarPresenter
    Private _gasBarrel As GasBarrelPresenter
    Private _invoice As InvoicePresenter
    Private _invoiceIn As InvoiceSplitPresenters
    Private _permission As PermissionPresenter
    Private _subjects As SubjectsPresenter
    Private _manufacturer As New ManufacturerPresenter(Me)
    Private _purchase As PurchasePresenter
    Private _pricePlan As New PricePlanPresenter(Me)
    Private _employee As New EmployeePresenter(Me)
    Private _bank As New BankPresenter(Me)
    Private _uph As New UintPriceHistoryPresenter(Me, _manuService)
    Private _inspection As InspectionPresenter

    Public Sub New(user As employee, permissions As List(Of String))
        InitializeComponent()

        ' 讓表單先攔截所有鍵盤事件，以便統一處理快捷鍵
        KeyPreview = True

        Me.User = New UserData With {
            .Id = user.emp_id,
            .Name = user.emp_name
        }

        InitializeTabPages(permissions)

        ' 初始化 DI 容器（需要在使用服務之前初始化）
        DependencyContainer.Initialize()

        ' 取得 CurrentUserService 並設定使用者資訊
        _currentUserService = DependencyContainer.Resolve(Of ICurrentUserService)()
        _currentUserService.SetCurrentUser(user.emp_id, user.emp_name, user.emp_r_id)

        Dim context As New gas_accounting_systemEntities

        Dim carRep As New CarRep(context)
        Dim compRep As New CompanyRep(context)
        Dim colRep As New CollectionRep(context)
        Dim cusRep As New CustomerRep(context)
        Dim gbRep As New GasBarrelRep(context)
        Dim invoiceRep As New InvoiceRep(context)
        Dim invoiceInRep As New InvoiceSplitRep(context)
        Dim ordRep As New OrderRep(context)
        Dim ocmRep As New OrderCollectionMappingRep(context)
        Dim bpRep As New BasicPriceRep(context)
        Dim permissionRep As New PermissionRep(context)
        Dim subjectRep As New SubjectRep(context)
        Dim inspectionRep As New InspectionRep(context)

        Dim ocmSer As IOrderCollectionMappingService = New OrderCollectionMappingService(ocmRep, ordRep, colRep)
        Dim priceCalSer As IPriceCalculationService = New PriceCalculationService(bpRep)
        Dim printerSer As IPrinterService = New PrinterService()

        _basicPrice = New BasicPricePresenter(Me, bpRep)
        _car = New CarPresenter(Me, cusRep, carRep)
        _gasBarrel = New GasBarrelPresenter(Me, gbRep)
        _invoice = New InvoicePresenter(Me, cusRep, invoiceRep, priceCalSer, ordRep)
        _invoiceIn = New InvoiceSplitPresenters(Me, invoiceInRep, compRep)
        _subjects = New SubjectsPresenter(Me, subjectRep)
        _permission = New PermissionPresenter(Me, permissionRep)
        _inspection = New InspectionPresenter(Me, cusRep, inspectionRep)

        InitializeUserControls()
    End Sub

    Private Sub InitializeUserControls()
        Try
            ' 創建 Presenter 和 View（需要特殊處理的事件驅動架構）
            Dim purchaseBarrelPresenter = DependencyContainer.Resolve(Of PurBarrelPresenter)()
            Dim purchaseBarrelView = DirectCast(purchaseBarrelPresenter.View, PurchaseBarrelUserControl)
            Dim paymentPresenter = DependencyContainer.Resolve(Of PaymentPresenter)()
            Dim paymentView = DirectCast(paymentPresenter.View, PaymentUserControl)
            Dim gasCheckoutPresenter = DependencyContainer.Resolve(Of GasCheckoutPresenter)
            Dim gasCheckoutView = DirectCast(gasCheckoutPresenter.View, GasCheckoutUserControl)
            Dim chequePayPresenter = DependencyContainer.Resolve(Of ChequePayPresenter)
            Dim chequePayVeiw = DirectCast(chequePayPresenter.View, ucChequePay)
            Dim chequeColPresenter = DependencyContainer.Resolve(Of ChequePresenter)
            Dim chequeColView = DirectCast(chequeColPresenter.View, ucCheque_col)
            Dim purchasePresenter = DependencyContainer.Resolve(Of PurchasePresenter)
            Dim purchaseVeiw = DirectCast(purchasePresenter.View, ucPurchase)
            Dim orderPresenter = DependencyContainer.Resolve(Of OrderPresenter)
            Dim orderView = DirectCast(orderPresenter.View, ucOrder)
            Dim customerPresenter = DependencyContainer.Resolve(Of CustomerPresenter)
            Dim customerView = DirectCast(customerPresenter.View, CustomerUserControl)

            ' 定義 TabPage 和對應 UserControl 的映射關係
            Dim userControlMappings As New Dictionary(Of TabPage, Control) From {
                {tpClosingEntry, DependencyContainer.Resolve(Of ucClosingEntry)()},
                {tpCustomer, customerView},
                {tpGasCheckout, gasCheckoutView},
                {tpReport, DependencyContainer.Resolve(Of Report)()},
                {tpOrder, orderView},
                {tpPayment, paymentView},
                {tpCollection, DependencyContainer.Resolve(Of CollectionUserControl)},
                {tpScrapBarrel, DependencyContainer.Resolve(Of ScrapBarrelUserControl)},
                {tpCheque_col, chequeColView},
                {tpChequePay, chequePayVeiw},
                {tpCompany, DependencyContainer.Resolve(Of CompanyUserControl)},
                {tpSurplusGas, DependencyContainer.Resolve(Of SurplusGasUserControl)},
                {tpBasicSet, DependencyContainer.Resolve(Of BasicSetUserControl)()},
                {tpPurchase, purchaseVeiw},
                {tpPurchaseBarrel, purchaseBarrelView}
            }

            ' 使用迴圈設定每個 UserControl
            For Each mapping In userControlMappings
                Dim tabPage As TabPage = mapping.Key
                Dim userControl As Control = mapping.Value

                ' 添加 UserControl 到 TabPage
                tabPage.Controls.Add(userControl)

                ' 設定 Dock 屬性為 Fill
                userControl.Dock = DockStyle.Fill
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub InitializeTabPages(permissions As List(Of String))
        For Each tabPage As TabPage In TabControl1.TabPages
            If Not permissions.Contains(tabPage.Name) AndAlso tabPage.Name <> "tpLogOut" Then
                tabPage.Parent = Nothing
            End If
        Next
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            InitTabPage()
            InitUI()
            SetCtrlStyle()
        Catch ex As Exception
            MessageBox.Show("frmMain_Load: " + ex.Message)
        End Try
    End Sub

    Private Sub frmMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        frmLogin.Show()
    End Sub

    ''' <summary>
    ''' 開啟銀行月結餘額重整工具
    ''' </summary>
    Public Sub OpenBankBalanceRecalculator()
        Try
            Dim recalculatorForm As New frmBankBalanceRecalculator()
            recalculatorForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("開啟重整工具時發生錯誤：" & ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InitUI()
        SetSheetColor()
        SetDataGridViewStyle(Me)
        SetDGVColumnWidthSave()
    End Sub

    Private Sub SetCtrlStyle()
        Try
            SetQueryEnterEven(Me)
            PositiveIntegerOnly()
            PositiveFloatOnly()
            SetFloatOnly()
            TextChagedHandler()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SetFloatOnly()
        Dim textBoxes = New List(Of TextBox) From {txtGasPrice, txtGasPrice_c, txtGasPriceDelivery, txtGasPriceDelivery_c}

        textBoxes.ForEach(Sub(txt) AddHandler txt.KeyPress, AddressOf FloatOnly_TextBox)
    End Sub

    ''' <summary>
    ''' 初始化各TabPage
    ''' </summary>
    Private Sub InitTabPage()
        Try
            btnCancel_emp_Click(btnCancel_emp, EventArgs.Empty)
            btnCancel_bp_Click(btnCancel_bp, EventArgs.Empty)
            btnCancel_bank_Click(btnCancel_bank, EventArgs.Empty)
            btnCancel_car_Click(btnCancel_car, EventArgs.Empty)
            btnCancel_subjects_Click(btnCancel_subjects, EventArgs.Empty)
            btnCancel_manu_Click(btnCancel_manu, EventArgs.Empty)
            btnCancel_pp_Click(btnCancel_pp, EventArgs.Empty)
            btnCancel_roles_Click(btnCancel_roles, EventArgs.Empty)
            btnCancel_uph_Click(btnCancel_uph, EventArgs.Empty)
            btnCancel_invoice_Click(btnCancel_invoice, EventArgs.Empty)
            btnCancel_gb_Click(btnCancel_gb, EventArgs.Empty)
            btnCancel_ii_Click(btnCancel_ii, EventArgs.Empty)
            btnCancel_ins_Click(btnCancel_ins, EventArgs.Empty)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' 設定TextBox值改變事件
    ''' </summary>
    Private Sub TextChagedHandler()
        Try
#Region "檢驗費自動運算"
            Dim inspectionTypes = New String() {"50", "20", "16", "10", "4", "Switch", "Freight", "RustProof", "Spraying"}
            For Each t In inspectionTypes
                Dim qtyCtrl = TryCast(grpIns.Controls("txtQty" & t), TextBox)
                Dim priceCtrl = TryCast(grpIns.Controls("txtPrice" & t), TextBox)
                If qtyCtrl IsNot Nothing Then AddHandler qtyCtrl.TextChanged, Sub(sender, e) CalculateInspectionFields()
                If priceCtrl IsNot Nothing Then AddHandler priceCtrl.TextChanged, Sub(sender, e) CalculateInspectionFields()
            Next
#End Region
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' 檢驗費自動計算
    Private Sub CalculateInspectionFields()
        Dim types = New String() {"50", "20", "16", "10", "4", "Switch", "Freight", "RustProof", "Spraying"}
        Dim totalQty As Integer = 0
        Dim totalAmount As Integer = 0
        Dim totalPrice As Decimal = 0
        For Each t In types
            Dim qtyCtrl = TryCast(grpIns.Controls("txtQty" & t), TextBox)
            Dim priceCtrl = TryCast(grpIns.Controls("txtPrice" & t), TextBox)
            Dim amountCtrl = TryCast(grpIns.Controls("txtAmount" & t), TextBox)
            If qtyCtrl IsNot Nothing AndAlso priceCtrl IsNot Nothing AndAlso amountCtrl IsNot Nothing Then
                Dim qty As Integer = 0
                Dim price As Decimal = 0
                Integer.TryParse(qtyCtrl.Text, qty)
                Decimal.TryParse(priceCtrl.Text, price)
                Dim amount = qty * price
                amountCtrl.Text = amount.ToString()
                totalQty += qty
                totalAmount += amount
                totalPrice += price
            End If
        Next
        txtQtyTotal.Text = totalQty.ToString()
        txtAmountTotal.Text = totalAmount.ToString()
        txtPriceTotal.Text = If(types.Length > 0, totalPrice.ToString(), "0")
    End Sub

    ''' <summary>
    ''' 設定TextBox只能輸入正整數
    ''' </summary>
    Private Sub PositiveIntegerOnly()
        Dim textBoxes = New List(Of TextBox) From {txtKg_invoice, txtAmount_ii}

        textBoxes.ForEach(Sub(txt) AddHandler txt.KeyPress, AddressOf PositiveIntegerOnly_TextBox)
    End Sub

    ''' <summary>
    ''' 設定TextBox只能輸入正浮點數
    ''' </summary>
    Private Sub PositiveFloatOnly()
        Dim txts = New List(Of TextBox) From {txtUnitPrice_invoice}

        txts.ForEach(Sub(x) AddHandler x.KeyPress, AddressOf PositiveFloatOnly_TextBox)
    End Sub

    ''' <summary>
    ''' 設定所有dgv在調整欄寬時紀錄
    ''' </summary>
    Private Sub SetDGVColumnWidthSave()
        Dim dgvs = GetControlInParent(Of DataGridView)(Me)
        dgvs.ForEach(Sub(dgv)
                         AddHandler dgv.ColumnWidthChanged, AddressOf SaveDataGridWidth
                         ReadDataGridWidth(dgv)
                     End Sub)
    End Sub

    ''' <summary>
    ''' 自定義索引標籤、文字顏色
    ''' </summary>
    Private Sub SetSheetColor()
        Dim list = New List(Of TabControl) From {TabControl1, tcBasicInfo, tcAccounting, TabControl3, TabControl2}

        list.ForEach(Sub(x)
                         x.DrawMode = TabDrawMode.OwnerDrawFixed
                         AddHandler x.DrawItem, AddressOf TabControl_DrawItem
                     End Sub)
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
        SetButtonState_old(sender, True)
        ClearControls(tpCar)
        If btnQuery_car.Text = "確  認" Then SetCarQueryCtrlsState()
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

        SetButtonState_old(sender, False)

        Dim id = dgvCar.SelectedRows(0).Cells("編號").Value
        _car.LoadDetail(id)
    End Sub

    '基本資料-車輛管理-修改
    Private Sub btnEdit_car_Click(sender As Object, e As EventArgs) Handles btnEdit_car.Click
        _car.Update()
        SetButtonState_old(sender, True)
    End Sub

    '基本資料-車輛管理-刪除
    Private Sub btnDelete_car_Click(sender As Object, e As EventArgs) Handles btnDelete_car.Click
        If MsgBox("確定要刪除?", vbYesNo, "警告") = MsgBoxResult.Yes Then
            _car.Delete()
            SetButtonState_old(sender, True)
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
        Try
            SetButtonState_old(sender, True)
            ClearControls(tpBasePrice)
            Await _basicPrice.SearchAsync(False)
        Catch ex As Exception
            MessageBox.Show("btnCancel_bp_Click: " + ex.Message)
        End Try
    End Sub

    '基本資料-基礎價格-新增
    Private Async Sub btnInsert_bp_Click(sender As Object, e As EventArgs) Handles btnCreate_bp.Click
        Await _basicPrice.AddAsync()
    End Sub

    '系統設定-基礎價格-dgv點擊
    Private Sub dgvBasicPrice_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvBasicPrice.CellMouseClick
        If Not sender.focused Then Return
        SetButtonState_old(sender, False)

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
        Return New List(Of Control) From {txtName_subjects}
    End Function

    '基本資料-科目管理-取消
    Private Sub btnCancel_subjects_Click(sender As Object, e As EventArgs) Handles btnCancel_subjects.Click
        SetButtonState(tpSubjects, False)
        _subjects.LoadList()
    End Sub

    '基本資料-科目管理-子科目-新增
    Private Sub btnAdd_subjects_Click(sender As Object, e As EventArgs) Handles btnAdd_subjects.Click
        _subjects.Add()
        SetButtonState(tpSubjects, False)
    End Sub

    '基本資料-科目管理-dgv
    Private Sub dgvSubject_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSubject.SelectionChanged, dgvSubject.CellMouseClick
        If Not sender.focused Then Return
        SetButtonState(tpSubjects, True)
        Dim id As Integer = dgvSubject.SelectedRows(0).Cells("編號").Value
        _subjects.LoadDetail(id)
    End Sub

    '基本資料-科目管理-修改
    Private Sub btnEdit_subjects_Click(sender As Object, e As EventArgs) Handles btnEdit_subjects.Click
        _subjects.Update()
        SetButtonState(tpSubjects, False)
    End Sub

    '基本資料-科目管理-刪除
    Private Sub btnDelete_subjects_Click(sender As Object, e As EventArgs) Handles btnDelete_subjects.Click
        Dim id As Integer = txtId_subjects.Text
        If MsgBox("確定要刪除?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            _subjects.Delete(id)
            SetButtonState(tpSubjects, False)
        End If
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
        SetButtonState_old(sender, True)
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

        SetButtonState_old(ctrl, False)

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
        SetButtonState_old(sender, True)
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

        SetButtonState_old(ctrl, False)

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
        SetButtonState_old(sender, True)
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

        SetButtonState_old(ctrl, False)

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
        SetButtonState_old(sender, True)
        _employee.GetRolesCmb()
        _employee.LoadList()
        ClearInput_emp()
    End Sub

    '基本資料-員工管理-新增
    Private Sub btnAdd_emp_Click(sender As Object, e As EventArgs) Handles btnAdd_emp.Click
        _employee.Add()

    End Sub

    '基本資料-員工管理-dgv-選擇列
    Private Sub dgvEmployee_SelectionChanged(sender As Object, e As EventArgs) Handles dgvEmployee.SelectionChanged, dgvEmployee.CellMouseClick
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused OrElse ctrl.SelectedRows.Count = 0 Then Return

        SetButtonState_old(ctrl, False)

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

    Private Sub IBankView_PopulateCompanyDropdown(data As List(Of SelectListItem)) Implements IBankView.PopulateCompanyDropdown
        SetComboBox(cmbCompany_bank, data)
    End Sub

    '基本資料-銀行帳戶-取消
    Private Sub btnCancel_bank_Click(sender As Object, e As EventArgs) Handles btnCancel_bank.Click
        SetButtonState_old(sender, True)
        _bank.Reset()
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
        AutoMapEntityToControls(data, tpInvoice_Out)
        AutoMapEntityToControls(data.customer, tpInvoice_Out)
    End Sub

    Private Function IInvoice_GetUserInput() As invoice Implements IBaseView(Of invoice, InvioceVM).GetUserInput
        Dim data As New invoice
        AutoMapControlsToEntity(data, tpInvoice_Out)
        Return data
    End Function

    Private Sub IInvoice_ClearInput() Implements IBaseView(Of invoice, InvioceVM).ClearInput
        ClearControls(tpInvoice_Out)
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

    Public Sub DisplayInvoiceInfo(info As (Integer, Integer)) Implements IInvoiceView.DisplayInvoiceInfo
        txtDeliNormTotal.Text = info.Item1
        txtDeliCTotal.Text = info.Item2
    End Sub

    '會計管理-發票管理-客戶代號
    Private Sub txtCusCode_invoice_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_invoice.KeyDown
        '按下Enter時,搜尋客戶資料
        If e.KeyCode = Keys.Enter AndAlso Not String.IsNullOrEmpty(txtCusCode_invoice.Text) Then
            _invoice.GetCustomerByCusCode(txtCusCode_invoice.Text)
            _invoice.CalculatePrices(cmbType_invoice.SelectedItem)

            Dim id As Integer = 0

            Integer.TryParse(txtCusId_invoice.Text, id)
            _invoice.LoadInvoiceInfo(id, dtpMonth_invoice.Value)
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
        SetButtonState_old(sender, True)
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

        SetButtonState_old(ctrl, False)

        Dim id = ctrl.SelectedRows(0).Cells(0).Value
        _invoice.LoadDetail(id)
    End Sub

    '會計管理-發票管理-修改
    Private Sub btnEdit_invoice_Click(sender As Object, e As EventArgs) Handles btnEdit_invoice.Click
        _invoice.Update()
        SetButtonState_old(sender, True)
    End Sub

    '會計管理-發票管理-刪除
    Private Sub btnDelete_invoice_Click(sender As Object, e As EventArgs) Handles btnDelete_invoice.Click
        If MsgBox("確定要刪除", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            _invoice.Delete()
            SetButtonState_old(sender, True)
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

    '會計管理-發票管理(銷項)-客戶編號、發票類型
    Private Sub DisplayDefaultInvoiceNumber(sender As Object, e As EventArgs) Handles cmbInvoiceType.SelectionChangeCommitted
        txtNumber_invoice.Text = _invoice.GetInvoiceDefaultNumber(cmbInvoiceType.SelectedItem)
    End Sub

    Public Sub DisplayList(data As List(Of InvoiceSplitVM)) Implements IBaseView(Of invoice_split, InvoiceSplitVM).DisplayList
        dgvInvoiceIn.DataSource = data
    End Sub

    Public Sub DisplayDetail(data As invoice_split) Implements IBaseView(Of invoice_split, InvoiceSplitVM).DisplayDetail
        AutoMapEntityToControls(data, tpInvoice_In)
    End Sub

    Private Function IBaseView_GetUserInput3() As invoice_split Implements IBaseView(Of invoice_split, InvoiceSplitVM).GetUserInput
        Dim data As New invoice_split
        AutoMapControlsToEntity(data, tpInvoice_In)
        Return data
    End Function

    Private Sub IBaseView_ClearInput3() Implements IBaseView(Of invoice_split, InvoiceSplitVM).ClearInput
        ClearControls(tpInvoice_In)
    End Sub

    Public Sub SetCompanyCmb(datas As List(Of SelectListItem)) Implements IInvoiceSplitView.SetCompanyCmb
        SetComboBox(cmbComp_is, datas)
    End Sub

    Private Function IInvoiceInView_GetSearchCriteria() As InvoiceSplitSearchCriteria Implements IInvoiceSplitView.GetSearchCriteria
        Return New InvoiceSplitSearchCriteria With {
            .EndDate = dtpEnd_ii.Value,
            .IsDate = chkIsDate_ii.Checked,
            .Name = txtName_is.Text,
            .Number = txtNumber_ii.Text,
            .StartDate = dtpStart_ii.Value,
            .TaxId = txtTaxId_is.Text
        }
    End Function

    '會計管理-發票管理(分裝場)-取消
    Private Sub btnCancel_ii_Click(sender As Object, e As EventArgs) Handles btnCancel_ii.Click
        SetButtonState_old(sender, True)
        _invoiceIn.Initialize()
    End Sub

    '會計管理-發票管理(分裝場)-新增
    Private Sub btnAdd_ii_Click(sender As Object, e As EventArgs) Handles btnAdd_ii.Click
        _invoiceIn.AddAsync()
    End Sub

    '會計管理-發票管理(分裝場)-dgv
    Private Sub dgvInvoiceIn_CellMouseClick(sender As Object, e As EventArgs) Handles dgvInvoiceIn.CellMouseClick, dgvInvoiceIn.SelectionChanged
        Dim ctrl As DataGridView = sender

        If Not ctrl.Focused Or ctrl.SelectedRows.Count = 0 Then Return

        SetButtonState_old(ctrl, False)

        Dim id = ctrl.SelectedRows(0).Cells(0).Value
        _invoiceIn.LoadDetailAsync(id)
    End Sub

    '會計管理-發票管理(分裝場)-修改
    Private Sub btnEdit_ii_Click(sender As Object, e As EventArgs) Handles btnEdit_ii.Click
        SetButtonState_old(sender, True)
        _invoiceIn.UpdateAsync()
    End Sub

    '會計管理-發票管理(分裝場)-刪除
    Private Sub btnDelete_ii_Click(sender As Object, e As EventArgs) Handles btnDelete_ii.Click
        If MsgBox("確定要刪除", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            _invoiceIn.DeleteAsync()
            SetButtonState_old(sender, True)
        End If
    End Sub

    '會計管理-發票管理(分裝場)-查詢
    Private Sub btnQuery_ii_Click(sender As Object, e As EventArgs) Handles btnQuery_ii.Click
        Dim lst As New List(Of Control) From {lblNumber_ii, lblName_ii, lblTaxId_is}

        SetQueryControls(btnQuery_ii, lst)

        If sender.Text = "查  詢" Then _invoiceIn.LoadList()
    End Sub

    '會計管理-發票管理(分裝場)-金額
    Private Sub txtAmount_ii_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAmount_ii.KeyUp
        Dim amount As Integer
        If Integer.TryParse(txtAmount_ii.Text, amount) Then
            txtTax_ii.Text = amount * 0.05
        End If
    End Sub

    '會計管理-發票管理(分裝場)-分類
    Private Sub cmbType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbType.SelectedIndexChanged
        Dim isInvoice = TryCast(sender, ComboBox)?.SelectedItem?.ToString()
        Dim isInput = isInvoice = "進項"
        Dim isOutput = isInvoice = "銷項"

        lblTaxId_is.Visible = isInput
        txtTaxId_is.Visible = isInput
        lblComp_is.Visible = isOutput
        cmbComp_is.Visible = isOutput

        If Not isOutput Then
            cmbComp_is.SelectedIndex = -1
            txtName_is.Clear()
        Else
            txtName_is.Text = "分裝費"
        End If
    End Sub

    Public Sub ShowCustomer(cus As customer) Implements IInspectionView.ShowCustomer
        txtCusName_ins.Text = cus.cus_name
        txtCusCode_ins.Text = cus.cus_code
        txtCusId_ins.Text = cus.cus_id
    End Sub

    Public Sub Clear() Implements IInspectionView.Clear
        ClearControls(tpInspection)
    End Sub

    Public Sub DisplayList(data As List(Of InspectionVM)) Implements IInspectionView.DisplayList
        dgvIns.DataSource = data
    End Sub

    Private Function IInspectionView_GetInput() As inspection Implements IInspectionView.GetInput
        Dim data As New inspection With {
            .in_Month = dtpIns.Value.Date
        }

        With data
            If String.IsNullOrEmpty(txtCusId_ins.Text) Then
                Throw New Exception("請先選擇客戶")
            Else .in_cus_Id = txtCusId_ins.Text
            End If
            Dim qty50 As Integer = 0, qty20 As Integer = 0, qty16 As Integer = 0, qty10 As Integer = 0, qty4 As Integer = 0
            Dim qtySwitch As Integer = 0, qtyFreight As Integer = 0, qtyRustProof As Integer = 0, qtySpraying As Integer = 0, qtyTotal As Integer = 0
            Dim price50 As Double = 0, price20 As Double = 0, price16 As Double = 0, price10 As Double = 0, price4 As Double = 0
            Dim priceSwitch As Double = 0, priceFreight As Double = 0, priceRustProof As Double = 0, priceSpraying As Double = 0, priceTotal As Double = 0
            Dim amount50 As Double = 0, amount20 As Double = 0, amount16 As Double = 0, amount10 As Double = 0, amount4 As Double = 0
            Dim amountSwitch As Double = 0, amountFreight As Double = 0, amountRustProof As Double = 0, amountSpraying As Double = 0, amountTotal As Double = 0

            Integer.TryParse(txtQty50.Text, qty50)
            Integer.TryParse(txtQty20.Text, qty20)
            Integer.TryParse(txtQty16.Text, qty16)
            Integer.TryParse(txtQty10.Text, qty10)
            Integer.TryParse(txtQty4.Text, qty4)
            Integer.TryParse(txtQtySwitch.Text, qtySwitch)
            Integer.TryParse(txtQtyFreight.Text, qtyFreight)
            Integer.TryParse(txtQtyRustProof.Text, qtyRustProof)
            Integer.TryParse(txtQtySpraying.Text, qtySpraying)
            Integer.TryParse(txtQtyTotal.Text, qtyTotal)

            Double.TryParse(txtPrice50.Text, price50)
            Double.TryParse(txtPrice20.Text, price20)
            Double.TryParse(txtPrice16.Text, price16)
            Double.TryParse(txtPrice10.Text, price10)
            Double.TryParse(txtPrice4.Text, price4)
            Double.TryParse(txtPriceSwitch.Text, priceSwitch)
            Double.TryParse(txtPriceFreight.Text, priceFreight)
            Double.TryParse(txtPriceRustProof.Text, priceRustProof)
            Double.TryParse(txtPriceSpraying.Text, priceSpraying)
            Double.TryParse(txtPriceTotal.Text, priceTotal)

            Double.TryParse(txtAmount50.Text, amount50)
            Double.TryParse(txtAmount20.Text, amount20)
            Double.TryParse(txtAmount16.Text, amount16)
            Double.TryParse(txtAmount10.Text, amount10)
            Double.TryParse(txtAmount4.Text, amount4)
            Double.TryParse(txtAmountSwitch.Text, amountSwitch)
            Double.TryParse(txtAmountFreight.Text, amountFreight)
            Double.TryParse(txtAmountRustProof.Text, amountRustProof)
            Double.TryParse(txtAmountSpraying.Text, amountSpraying)
            Double.TryParse(txtAmountTotal.Text, amountTotal)

            .in_Qty50 = qty50
            .in_Price50 = price50
            .in_Amount50 = amount50
            .in_Qty20 = qty20
            .in_Price20 = price20
            .in_Amount20 = amount20
            .in_Qty16 = qty16
            .in_Price16 = price16
            .in_Amount16 = amount16
            .in_Qty10 = qty10
            .in_Price10 = price10
            .in_Amount10 = amount10
            .in_Qty4 = qty4
            .in_Price4 = price4
            .in_Amount4 = amount4
            .in_QtySwitch = qtySwitch
            .in_PriceSwitch = priceSwitch
            .in_AmountSwitch = amountSwitch
            .in_QtyFreight = qtyFreight
            .in_PriceFreight = priceFreight
            .in_AmountFreight = amountFreight
            .in_QtyRustProof = qtyRustProof
            .in_PriceRustProof = priceRustProof
            .in_AmountRustProof = amountRustProof
            .in_QtySpraying = qtySpraying
            .in_PriceSpraying = priceSpraying
            .in_AmountSpraying = amountSpraying
            .in_QtyTotal = qtyTotal
            .in_PriceTotal = priceTotal
            .in_AmountTotal = amountTotal
        End With

        Return data
    End Function

    Public Sub ShowDetail(data As inspection) Implements IInspectionView.ShowDetail
        dtpIns.Value = data.in_Month
        txtCusId_ins.Text = data.in_cus_Id
        txtCusCode_ins.Text = data.customer.cus_code
        txtCusName_ins.Text = data.customer.cus_name
        txtQty50.Text = data.in_Qty50
        txtPrice50.Text = data.in_Price50
        txtAmount50.Text = data.in_Amount50
        txtQty20.Text = data.in_Qty20
        txtPrice20.Text = data.in_Price20
        txtAmount20.Text = data.in_Amount20
        txtQty16.Text = data.in_Qty16
        txtPrice16.Text = data.in_Price16
        txtAmount16.Text = data.in_Amount16
        txtQty10.Text = data.in_Qty10
        txtPrice10.Text = data.in_Price10
        txtAmount10.Text = data.in_Amount10
        txtQty4.Text = data.in_Qty4
        txtPrice4.Text = data.in_Price4
        txtAmount4.Text = data.in_Amount4
        txtQtySwitch.Text = data.in_QtySwitch
        txtPriceSwitch.Text = data.in_PriceSwitch
        txtAmountSwitch.Text = data.in_AmountSwitch
        txtQtyFreight.Text = data.in_QtyFreight
        txtPriceFreight.Text = data.in_PriceFreight
        txtAmountFreight.Text = data.in_AmountFreight
        txtQtyRustProof.Text = data.in_QtyRustProof
        txtPriceRustProof.Text = data.in_PriceRustProof
        txtAmountRustProof.Text = data.in_AmountRustProof
        txtQtySpraying.Text = data.in_QtySpraying
        txtPriceSpraying.Text = data.in_PriceSpraying
        txtAmountSpraying.Text = data.in_AmountSpraying
        txtQtyTotal.Text = data.in_QtyTotal
        txtPriceTotal.Text = data.in_PriceTotal
        txtAmountTotal.Text = data.in_AmountTotal
    End Sub

    ' 支出管理-檢驗費-搜尋客戶
    Private Sub btnCus_ins_Click(sender As Object, e As EventArgs) Handles btnCus_ins.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusId_ins.Text = searchForm.CusId
                txtCusCode_ins.Text = searchForm.CusCode
                txtCusName_ins.Text = searchForm.CusName
            End If
        End Using
    End Sub

    ' 支出管理-檢驗費-客戶代號
    Private Sub txtCusCode_ins_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCusCode_ins.KeyDown
        If e.KeyCode = Keys.Enter Then
            _inspection.GetCustomerByCusCode(txtCusCode_ins.Text)
        End If
    End Sub

    ' 支出管理-檢驗費-取消
    Private Sub btnCancel_ins_Click(sender As Object, e As EventArgs) Handles btnCancel_ins.Click
        SetButtonState_old(sender, True)
        _inspection.Reset()
    End Sub

    ' 支出管理-檢驗費-新增
    Private Sub btnCreate_ins_Click(sender As Object, e As EventArgs) Handles btnCreate_ins.Click
        _inspection.Add()
    End Sub

    ' 支出管理-檢驗費-dgv
    Private Sub dgvIns_CellMouseClick(sender As Object, e As EventArgs) Handles dgvIns.CellMouseClick, dgvIns.SelectionChanged
        Dim ctrl As DataGridView = sender

        If Not ctrl.Focused Or ctrl.SelectedRows.Count = 0 Then Return

        SetButtonState_old(ctrl, False)

        Dim id = ctrl.SelectedRows(0).Cells(0).Value
        _inspection.LoadDetail(id)
    End Sub

    ' 支出管理-檢驗費-修改
    Private Sub btnEdit_ins_Click(sender As Object, e As EventArgs) Handles btnEdit_ins.Click
        SetButtonState_old(sender, True)
        _inspection.Update()
    End Sub

    ' 支出管理-檢驗費-修改
    Private Sub btnDelete_ins_Click(sender As Object, e As EventArgs) Handles btnDelete_ins.Click
        _inspection.Delete()
    End Sub

    ' 支出管理-檢驗費-查詢
    Private Sub btnSearch_ins_Click(sender As Object, e As EventArgs) Handles btnSearch_ins.Click
        Using frm As New Search_Inspection
            If frm.ShowDialog = DialogResult.OK Then
                _inspection.LoadList(frm.Criteria)
            End If
        End Using
    End Sub

    ' 支出管理-檢驗費-列印
    Private Sub btnPrint_ins_Click(sender As Object, e As EventArgs) Handles btnPrint_ins.Click
        _inspection.Print()
    End Sub

    ' -----------------------------------------------------
    ' 表單層級快捷鍵處理 (F1~F8)
    ' -----------------------------------------------------
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Select Case keyData
            Case Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8
                ' 僅在當前頁為「訂單管理」時處理
                If TabControl1.SelectedTab Is tpOrder Then
                    Dim ordCtrl = TryCast(tpOrder.Controls.OfType(Of ucOrder)().FirstOrDefault(), ucOrder)
                    If ordCtrl IsNot Nothing Then
                        ordCtrl.HandleShortcut(keyData)
                        Return True ' 已處理，避免系統預設響應
                    End If
                End If
        End Select
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenBankBalanceRecalculator()
    End Sub
End Class