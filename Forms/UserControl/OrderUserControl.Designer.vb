<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class OrderUserControl
    Inherits System.Windows.Forms.UserControl

    'UserControl 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請勿使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtUnpaid = New System.Windows.Forms.TextBox()
        Me.Label137 = New System.Windows.Forms.Label()
        Me.btnCusGetGasList = New System.Windows.Forms.Button()
        Me.btnCusGasPayCollect = New System.Windows.Forms.Button()
        Me.btnPrintCusStk = New System.Windows.Forms.Button()
        Me.txtGasCUnitPrice = New System.Windows.Forms.TextBox()
        Me.Label241 = New System.Windows.Forms.Label()
        Me.txtGasUnitPrice = New System.Windows.Forms.TextBox()
        Me.Label240 = New System.Windows.Forms.Label()
        Me.txtBarrelAmount = New System.Windows.Forms.TextBox()
        Me.Label238 = New System.Windows.Forms.Label()
        Me.btnEdit_ord = New System.Windows.Forms.Button()
        Me.txtInsurance = New System.Windows.Forms.TextBox()
        Me.Label153 = New System.Windows.Forms.Label()
        Me.txtCusCode_ord = New System.Windows.Forms.TextBox()
        Me.txtOperator = New System.Windows.Forms.TextBox()
        Me.Label107 = New System.Windows.Forms.Label()
        Me.tcInOut = New System.Windows.Forms.TabControl()
        Me.tpIn = New System.Windows.Forms.TabPage()
        Me.txtBarralUnitPrice_2 = New System.Windows.Forms.TextBox()
        Me.txtBarralUnitPrice_4 = New System.Windows.Forms.TextBox()
        Me.txtBarralUnitPrice_5 = New System.Windows.Forms.TextBox()
        Me.txtBarralUnitPrice_10 = New System.Windows.Forms.TextBox()
        Me.txtBarralUnitPrice_14 = New System.Windows.Forms.TextBox()
        Me.txtBarralUnitPrice_18 = New System.Windows.Forms.TextBox()
        Me.txtBarralUnitPrice_16 = New System.Windows.Forms.TextBox()
        Me.txtBarralUnitPrice_20 = New System.Windows.Forms.TextBox()
        Me.txtBarralUnitPrice_50 = New System.Windows.Forms.TextBox()
        Me.Label100 = New System.Windows.Forms.Label()
        Me.txtInspect2 = New System.Windows.Forms.TextBox()
        Me.txtInspect4 = New System.Windows.Forms.TextBox()
        Me.txtInspect5 = New System.Windows.Forms.TextBox()
        Me.txtInspect10 = New System.Windows.Forms.TextBox()
        Me.txtInspect14 = New System.Windows.Forms.TextBox()
        Me.txtInspect18 = New System.Windows.Forms.TextBox()
        Me.txtInspect16 = New System.Windows.Forms.TextBox()
        Me.txtInspect20 = New System.Windows.Forms.TextBox()
        Me.txtInspect50 = New System.Windows.Forms.TextBox()
        Me.Label142 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox16 = New System.Windows.Forms.TextBox()
        Me.TextBox17 = New System.Windows.Forms.TextBox()
        Me.TextBox18 = New System.Windows.Forms.TextBox()
        Me.TextBox19 = New System.Windows.Forms.TextBox()
        Me.TextBox20 = New System.Windows.Forms.TextBox()
        Me.TextBox73 = New System.Windows.Forms.TextBox()
        Me.TextBox74 = New System.Windows.Forms.TextBox()
        Me.TextBox78 = New System.Windows.Forms.TextBox()
        Me.Label206 = New System.Windows.Forms.Label()
        Me.txtBarrelIn_2 = New System.Windows.Forms.TextBox()
        Me.txtBarrelIn_4 = New System.Windows.Forms.TextBox()
        Me.txtBarrelIn_5 = New System.Windows.Forms.TextBox()
        Me.txtBarrelIn_10 = New System.Windows.Forms.TextBox()
        Me.txtBarrelIn_14 = New System.Windows.Forms.TextBox()
        Me.txtBarrelIn_18 = New System.Windows.Forms.TextBox()
        Me.txtBarrelIn_16 = New System.Windows.Forms.TextBox()
        Me.txtBarrelIn_20 = New System.Windows.Forms.TextBox()
        Me.txtBarrelIn_50 = New System.Windows.Forms.TextBox()
        Me.Label205 = New System.Windows.Forms.Label()
        Me.txtDepositIn_2 = New System.Windows.Forms.TextBox()
        Me.txtDepositIn_4 = New System.Windows.Forms.TextBox()
        Me.txtDepositIn_5 = New System.Windows.Forms.TextBox()
        Me.txtDepositIn_10 = New System.Windows.Forms.TextBox()
        Me.txtDepositIn_14 = New System.Windows.Forms.TextBox()
        Me.txtDepositIn_18 = New System.Windows.Forms.TextBox()
        Me.txtDepositIn_16 = New System.Windows.Forms.TextBox()
        Me.txtDepositIn_20 = New System.Windows.Forms.TextBox()
        Me.txtDepositIn_50 = New System.Windows.Forms.TextBox()
        Me.Label272 = New System.Windows.Forms.Label()
        Me.txto_inspect_2 = New System.Windows.Forms.TextBox()
        Me.txto_new_in_2 = New System.Windows.Forms.TextBox()
        Me.txto_in_2 = New System.Windows.Forms.TextBox()
        Me.txto_inspect_4 = New System.Windows.Forms.TextBox()
        Me.txto_new_in_4 = New System.Windows.Forms.TextBox()
        Me.txto_in_4 = New System.Windows.Forms.TextBox()
        Me.txto_inspect_5 = New System.Windows.Forms.TextBox()
        Me.txto_new_in_5 = New System.Windows.Forms.TextBox()
        Me.txto_in_5 = New System.Windows.Forms.TextBox()
        Me.txto_inspect_10 = New System.Windows.Forms.TextBox()
        Me.txto_new_in_10 = New System.Windows.Forms.TextBox()
        Me.txto_in_10 = New System.Windows.Forms.TextBox()
        Me.txto_inspect_14 = New System.Windows.Forms.TextBox()
        Me.txto_new_in_14 = New System.Windows.Forms.TextBox()
        Me.txto_in_14 = New System.Windows.Forms.TextBox()
        Me.txto_inspect_18 = New System.Windows.Forms.TextBox()
        Me.txto_new_in_18 = New System.Windows.Forms.TextBox()
        Me.txto_in_18 = New System.Windows.Forms.TextBox()
        Me.txto_inspect_16 = New System.Windows.Forms.TextBox()
        Me.txto_new_in_16 = New System.Windows.Forms.TextBox()
        Me.txto_in_16 = New System.Windows.Forms.TextBox()
        Me.txto_inspect_20 = New System.Windows.Forms.TextBox()
        Me.txto_new_in_20 = New System.Windows.Forms.TextBox()
        Me.txto_in_20 = New System.Windows.Forms.TextBox()
        Me.txto_inspect_50 = New System.Windows.Forms.TextBox()
        Me.txto_new_in_50 = New System.Windows.Forms.TextBox()
        Me.txto_in_50 = New System.Windows.Forms.TextBox()
        Me.Label212 = New System.Windows.Forms.Label()
        Me.Label213 = New System.Windows.Forms.Label()
        Me.Label214 = New System.Windows.Forms.Label()
        Me.Label215 = New System.Windows.Forms.Label()
        Me.Label216 = New System.Windows.Forms.Label()
        Me.Label217 = New System.Windows.Forms.Label()
        Me.Label264 = New System.Windows.Forms.Label()
        Me.Label266 = New System.Windows.Forms.Label()
        Me.Label267 = New System.Windows.Forms.Label()
        Me.Label268 = New System.Windows.Forms.Label()
        Me.Label269 = New System.Windows.Forms.Label()
        Me.Label270 = New System.Windows.Forms.Label()
        Me.Label271 = New System.Windows.Forms.Label()
        Me.tpOut = New System.Windows.Forms.TabPage()
        Me.txtCarDeposit_2 = New System.Windows.Forms.TextBox()
        Me.txtCarDeposit_5 = New System.Windows.Forms.TextBox()
        Me.txtCarDeposit_14 = New System.Windows.Forms.TextBox()
        Me.txtCarDeposit_18 = New System.Windows.Forms.TextBox()
        Me.txtCarDeposit_4 = New System.Windows.Forms.TextBox()
        Me.txtCarDeposit_10 = New System.Windows.Forms.TextBox()
        Me.txtCarDeposit_16 = New System.Windows.Forms.TextBox()
        Me.txtCarDeposit_20 = New System.Windows.Forms.TextBox()
        Me.txtCarDeposit_50 = New System.Windows.Forms.TextBox()
        Me.Label277 = New System.Windows.Forms.Label()
        Me.Label274 = New System.Windows.Forms.Label()
        Me.cmbCarOut_ord = New System.Windows.Forms.ComboBox()
        Me.txtCusGas_2 = New System.Windows.Forms.TextBox()
        Me.txtCusGas_5 = New System.Windows.Forms.TextBox()
        Me.txtCusGas_14 = New System.Windows.Forms.TextBox()
        Me.txtCusGas_18 = New System.Windows.Forms.TextBox()
        Me.txtCusGas_4 = New System.Windows.Forms.TextBox()
        Me.txtCusGas_10 = New System.Windows.Forms.TextBox()
        Me.txtCusGas_16 = New System.Windows.Forms.TextBox()
        Me.txtCusGas_20 = New System.Windows.Forms.TextBox()
        Me.txtCusGas_50 = New System.Windows.Forms.TextBox()
        Me.Label273 = New System.Windows.Forms.Label()
        Me.txtDepositOut_2 = New System.Windows.Forms.TextBox()
        Me.txtDepositOut_5 = New System.Windows.Forms.TextBox()
        Me.txtDepositOut_14 = New System.Windows.Forms.TextBox()
        Me.txtDepositOut_18 = New System.Windows.Forms.TextBox()
        Me.txtDepositOut_4 = New System.Windows.Forms.TextBox()
        Me.txtDepositOut_10 = New System.Windows.Forms.TextBox()
        Me.txtDepositOut_16 = New System.Windows.Forms.TextBox()
        Me.txtDepositOut_20 = New System.Windows.Forms.TextBox()
        Me.txtDepositOut_50 = New System.Windows.Forms.TextBox()
        Me.Label157 = New System.Windows.Forms.Label()
        Me.txtEmpty_2 = New System.Windows.Forms.TextBox()
        Me.txtGas_c_2 = New System.Windows.Forms.TextBox()
        Me.txtGas_2 = New System.Windows.Forms.TextBox()
        Me.txtEmpty_5 = New System.Windows.Forms.TextBox()
        Me.txtGas_c_5 = New System.Windows.Forms.TextBox()
        Me.txtGas_5 = New System.Windows.Forms.TextBox()
        Me.txtEmpty_14 = New System.Windows.Forms.TextBox()
        Me.txtGas_c_14 = New System.Windows.Forms.TextBox()
        Me.txtGas_14 = New System.Windows.Forms.TextBox()
        Me.txtEmpty_18 = New System.Windows.Forms.TextBox()
        Me.txtGas_c_18 = New System.Windows.Forms.TextBox()
        Me.txtGas_18 = New System.Windows.Forms.TextBox()
        Me.txtEmpty_4 = New System.Windows.Forms.TextBox()
        Me.txtGas_c_4 = New System.Windows.Forms.TextBox()
        Me.txtGas_4 = New System.Windows.Forms.TextBox()
        Me.txtEmpty_10 = New System.Windows.Forms.TextBox()
        Me.txtGas_c_10 = New System.Windows.Forms.TextBox()
        Me.txtGas_10 = New System.Windows.Forms.TextBox()
        Me.txtEmpty_16 = New System.Windows.Forms.TextBox()
        Me.txtGas_c_16 = New System.Windows.Forms.TextBox()
        Me.txtGas_16 = New System.Windows.Forms.TextBox()
        Me.txtEmpty_20 = New System.Windows.Forms.TextBox()
        Me.txtGas_c_20 = New System.Windows.Forms.TextBox()
        Me.txtGas_20 = New System.Windows.Forms.TextBox()
        Me.txtEmpty_50 = New System.Windows.Forms.TextBox()
        Me.txtGas_c_50 = New System.Windows.Forms.TextBox()
        Me.txtGas_50 = New System.Windows.Forms.TextBox()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.Label103 = New System.Windows.Forms.Label()
        Me.Label105 = New System.Windows.Forms.Label()
        Me.Label125 = New System.Windows.Forms.Label()
        Me.Label158 = New System.Windows.Forms.Label()
        Me.Label160 = New System.Windows.Forms.Label()
        Me.Label211 = New System.Windows.Forms.Label()
        Me.grpTransport = New System.Windows.Forms.GroupBox()
        Me.rdoPickUp = New System.Windows.Forms.RadioButton()
        Me.rdoDelivery = New System.Windows.Forms.RadioButton()
        Me.txto_return_c = New System.Windows.Forms.TextBox()
        Me.txtTotalGas_c = New System.Windows.Forms.TextBox()
        Me.txtTotalGas = New System.Windows.Forms.TextBox()
        Me.txto_sales_allowance = New System.Windows.Forms.TextBox()
        Me.txto_return = New System.Windows.Forms.TextBox()
        Me.txto_memo = New System.Windows.Forms.TextBox()
        Me.txtCusName = New System.Windows.Forms.TextBox()
        Me.txtAmount_ord = New System.Windows.Forms.TextBox()
        Me.txtCusID_order = New System.Windows.Forms.TextBox()
        Me.txto_id = New System.Windows.Forms.TextBox()
        Me.Label204 = New System.Windows.Forms.Label()
        Me.Label203 = New System.Windows.Forms.Label()
        Me.Label202 = New System.Windows.Forms.Label()
        Me.btnQueryCus_ord = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.grpSearch_ord = New System.Windows.Forms.GroupBox()
        Me.chkOut = New System.Windows.Forms.CheckBox()
        Me.chkIn = New System.Windows.Forms.CheckBox()
        Me.chkIsDate_ord = New System.Windows.Forms.CheckBox()
        Me.Label165 = New System.Windows.Forms.Label()
        Me.btnQuery_order = New System.Windows.Forms.Button()
        Me.dtpEnd_order = New System.Windows.Forms.DateTimePicker()
        Me.Label166 = New System.Windows.Forms.Label()
        Me.dtpStart_order = New System.Windows.Forms.DateTimePicker()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.Label64 = New System.Windows.Forms.Label()
        Me.Label63 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbCar_ord = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvOrder = New System.Windows.Forms.DataGridView()
        Me.btnCancel_order = New System.Windows.Forms.Button()
        Me.btnDelete_order = New System.Windows.Forms.Button()
        Me.btnCreate_ord = New System.Windows.Forms.Button()
        Me.lblCusCode = New System.Windows.Forms.Label()
        Me.lblCarNo = New System.Windows.Forms.Label()
        Me.dtpOrder = New System.Windows.Forms.DateTimePicker()
        Me.Label151 = New System.Windows.Forms.Label()
        Me.tcInOut.SuspendLayout()
        Me.tpIn.SuspendLayout()
        Me.tpOut.SuspendLayout()
        Me.grpTransport.SuspendLayout()
        Me.grpSearch_ord.SuspendLayout()
        CType(Me.dgvOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtUnpaid
        '
        Me.txtUnpaid.Location = New System.Drawing.Point(540, 222)
        Me.txtUnpaid.Name = "txtUnpaid"
        Me.txtUnpaid.ReadOnly = True
        Me.txtUnpaid.Size = New System.Drawing.Size(100, 30)
        Me.txtUnpaid.TabIndex = 526
        Me.txtUnpaid.Tag = "o_UnpaidAmount"
        '
        'Label137
        '
        Me.Label137.AutoSize = True
        Me.Label137.Location = New System.Drawing.Point(420, 225)
        Me.Label137.Name = "Label137"
        Me.Label137.Size = New System.Drawing.Size(114, 19)
        Me.Label137.TabIndex = 525
        Me.Label137.Text = "未收款金額"
        '
        'btnCusGetGasList
        '
        Me.btnCusGetGasList.AutoSize = True
        Me.btnCusGetGasList.BackColor = System.Drawing.Color.Lime
        Me.btnCusGetGasList.Location = New System.Drawing.Point(1300, 448)
        Me.btnCusGetGasList.Name = "btnCusGetGasList"
        Me.btnCusGetGasList.Size = New System.Drawing.Size(189, 44)
        Me.btnCusGetGasList.TabIndex = 524
        Me.btnCusGetGasList.Text = "客戶提氣清冊(F8)"
        Me.btnCusGetGasList.UseVisualStyleBackColor = False
        '
        'btnCusGasPayCollect
        '
        Me.btnCusGasPayCollect.AutoSize = True
        Me.btnCusGasPayCollect.BackColor = System.Drawing.Color.Lime
        Me.btnCusGasPayCollect.Location = New System.Drawing.Point(1032, 448)
        Me.btnCusGasPayCollect.Name = "btnCusGasPayCollect"
        Me.btnCusGasPayCollect.Size = New System.Drawing.Size(252, 44)
        Me.btnCusGasPayCollect.TabIndex = 523
        Me.btnCusGasPayCollect.Text = "氣量氣款收付明細表(F7)"
        Me.btnCusGasPayCollect.UseVisualStyleBackColor = False
        '
        'btnPrintCusStk
        '
        Me.btnPrintCusStk.AutoSize = True
        Me.btnPrintCusStk.BackColor = System.Drawing.Color.Lime
        Me.btnPrintCusStk.Location = New System.Drawing.Point(785, 448)
        Me.btnPrintCusStk.Name = "btnPrintCusStk"
        Me.btnPrintCusStk.Size = New System.Drawing.Size(231, 44)
        Me.btnPrintCusStk.TabIndex = 522
        Me.btnPrintCusStk.Text = "客戶鋼瓶結存總冊(F6)"
        Me.btnPrintCusStk.UseVisualStyleBackColor = False
        '
        'txtGasCUnitPrice
        '
        Me.txtGasCUnitPrice.Location = New System.Drawing.Point(519, 176)
        Me.txtGasCUnitPrice.Name = "txtGasCUnitPrice"
        Me.txtGasCUnitPrice.ReadOnly = True
        Me.txtGasCUnitPrice.Size = New System.Drawing.Size(100, 30)
        Me.txtGasCUnitPrice.TabIndex = 521
        Me.txtGasCUnitPrice.Tag = "o_UnitPriceC"
        '
        'Label241
        '
        Me.Label241.AutoSize = True
        Me.Label241.Location = New System.Drawing.Point(420, 182)
        Me.Label241.Name = "Label241"
        Me.Label241.Size = New System.Drawing.Size(93, 19)
        Me.Label241.TabIndex = 520
        Me.Label241.Text = "丙氣單價"
        '
        'txtGasUnitPrice
        '
        Me.txtGasUnitPrice.Location = New System.Drawing.Point(519, 133)
        Me.txtGasUnitPrice.Name = "txtGasUnitPrice"
        Me.txtGasUnitPrice.ReadOnly = True
        Me.txtGasUnitPrice.Size = New System.Drawing.Size(100, 30)
        Me.txtGasUnitPrice.TabIndex = 519
        Me.txtGasUnitPrice.Tag = "o_UnitPrice"
        '
        'Label240
        '
        Me.Label240.AutoSize = True
        Me.Label240.Location = New System.Drawing.Point(420, 139)
        Me.Label240.Name = "Label240"
        Me.Label240.Size = New System.Drawing.Size(93, 19)
        Me.Label240.TabIndex = 518
        Me.Label240.Text = "普氣單價"
        '
        'txtBarrelAmount
        '
        Me.txtBarrelAmount.Location = New System.Drawing.Point(108, 219)
        Me.txtBarrelAmount.Name = "txtBarrelAmount"
        Me.txtBarrelAmount.ReadOnly = True
        Me.txtBarrelAmount.Size = New System.Drawing.Size(100, 30)
        Me.txtBarrelAmount.TabIndex = 517
        Me.txtBarrelAmount.Tag = "o_BarrelPrice"
        '
        'Label238
        '
        Me.Label238.AutoSize = True
        Me.Label238.Location = New System.Drawing.Point(8, 225)
        Me.Label238.Name = "Label238"
        Me.Label238.Size = New System.Drawing.Size(94, 19)
        Me.Label238.TabIndex = 516
        Me.Label238.Text = "桶 金 額"
        '
        'btnEdit_ord
        '
        Me.btnEdit_ord.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit_ord.Location = New System.Drawing.Point(161, 448)
        Me.btnEdit_ord.Name = "btnEdit_ord"
        Me.btnEdit_ord.Size = New System.Drawing.Size(140, 44)
        Me.btnEdit_ord.TabIndex = 515
        Me.btnEdit_ord.Text = "修改 (F2)"
        Me.btnEdit_ord.UseVisualStyleBackColor = False
        '
        'txtInsurance
        '
        Me.txtInsurance.Location = New System.Drawing.Point(726, 176)
        Me.txtInsurance.Name = "txtInsurance"
        Me.txtInsurance.ReadOnly = True
        Me.txtInsurance.Size = New System.Drawing.Size(100, 30)
        Me.txtInsurance.TabIndex = 514
        Me.txtInsurance.Tag = "o_Insurance"
        '
        'Label153
        '
        Me.Label153.AutoSize = True
        Me.Label153.Location = New System.Drawing.Point(625, 182)
        Me.Label153.Name = "Label153"
        Me.Label153.Size = New System.Drawing.Size(93, 19)
        Me.Label153.TabIndex = 513
        Me.Label153.Text = "保險金額"
        '
        'txtCusCode_ord
        '
        Me.txtCusCode_ord.Location = New System.Drawing.Point(109, 4)
        Me.txtCusCode_ord.Name = "txtCusCode_ord"
        Me.txtCusCode_ord.Size = New System.Drawing.Size(100, 30)
        Me.txtCusCode_ord.TabIndex = 512
        Me.txtCusCode_ord.Tag = "cus_code"
        '
        'txtOperator
        '
        Me.txtOperator.Location = New System.Drawing.Point(585, 359)
        Me.txtOperator.Name = "txtOperator"
        Me.txtOperator.ReadOnly = True
        Me.txtOperator.Size = New System.Drawing.Size(100, 30)
        Me.txtOperator.TabIndex = 511
        Me.txtOperator.Tag = "emp_id"
        '
        'Label107
        '
        Me.Label107.AutoSize = True
        Me.Label107.Location = New System.Drawing.Point(486, 365)
        Me.Label107.Name = "Label107"
        Me.Label107.Size = New System.Drawing.Size(93, 19)
        Me.Label107.TabIndex = 510
        Me.Label107.Text = "填單人員"
        '
        'tcInOut
        '
        Me.tcInOut.Controls.Add(Me.tpIn)
        Me.tcInOut.Controls.Add(Me.tpOut)
        Me.tcInOut.Location = New System.Drawing.Point(832, 4)
        Me.tcInOut.Name = "tcInOut"
        Me.tcInOut.SelectedIndex = 0
        Me.tcInOut.Size = New System.Drawing.Size(1017, 438)
        Me.tcInOut.TabIndex = 509
        Me.tcInOut.Tag = "o_in_out"
        '
        'tpIn
        '
        Me.tpIn.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.tpIn.Controls.Add(Me.txtBarralUnitPrice_2)
        Me.tpIn.Controls.Add(Me.txtBarralUnitPrice_4)
        Me.tpIn.Controls.Add(Me.txtBarralUnitPrice_5)
        Me.tpIn.Controls.Add(Me.txtBarralUnitPrice_10)
        Me.tpIn.Controls.Add(Me.txtBarralUnitPrice_14)
        Me.tpIn.Controls.Add(Me.txtBarralUnitPrice_18)
        Me.tpIn.Controls.Add(Me.txtBarralUnitPrice_16)
        Me.tpIn.Controls.Add(Me.txtBarralUnitPrice_20)
        Me.tpIn.Controls.Add(Me.txtBarralUnitPrice_50)
        Me.tpIn.Controls.Add(Me.Label100)
        Me.tpIn.Controls.Add(Me.txtInspect2)
        Me.tpIn.Controls.Add(Me.txtInspect4)
        Me.tpIn.Controls.Add(Me.txtInspect5)
        Me.tpIn.Controls.Add(Me.txtInspect10)
        Me.tpIn.Controls.Add(Me.txtInspect14)
        Me.tpIn.Controls.Add(Me.txtInspect18)
        Me.tpIn.Controls.Add(Me.txtInspect16)
        Me.tpIn.Controls.Add(Me.txtInspect20)
        Me.tpIn.Controls.Add(Me.txtInspect50)
        Me.tpIn.Controls.Add(Me.Label142)
        Me.tpIn.Controls.Add(Me.TextBox1)
        Me.tpIn.Controls.Add(Me.TextBox16)
        Me.tpIn.Controls.Add(Me.TextBox17)
        Me.tpIn.Controls.Add(Me.TextBox18)
        Me.tpIn.Controls.Add(Me.TextBox19)
        Me.tpIn.Controls.Add(Me.TextBox20)
        Me.tpIn.Controls.Add(Me.TextBox73)
        Me.tpIn.Controls.Add(Me.TextBox74)
        Me.tpIn.Controls.Add(Me.TextBox78)
        Me.tpIn.Controls.Add(Me.Label206)
        Me.tpIn.Controls.Add(Me.txtBarrelIn_2)
        Me.tpIn.Controls.Add(Me.txtBarrelIn_4)
        Me.tpIn.Controls.Add(Me.txtBarrelIn_5)
        Me.tpIn.Controls.Add(Me.txtBarrelIn_10)
        Me.tpIn.Controls.Add(Me.txtBarrelIn_14)
        Me.tpIn.Controls.Add(Me.txtBarrelIn_18)
        Me.tpIn.Controls.Add(Me.txtBarrelIn_16)
        Me.tpIn.Controls.Add(Me.txtBarrelIn_20)
        Me.tpIn.Controls.Add(Me.txtBarrelIn_50)
        Me.tpIn.Controls.Add(Me.Label205)
        Me.tpIn.Controls.Add(Me.txtDepositIn_2)
        Me.tpIn.Controls.Add(Me.txtDepositIn_4)
        Me.tpIn.Controls.Add(Me.txtDepositIn_5)
        Me.tpIn.Controls.Add(Me.txtDepositIn_10)
        Me.tpIn.Controls.Add(Me.txtDepositIn_14)
        Me.tpIn.Controls.Add(Me.txtDepositIn_18)
        Me.tpIn.Controls.Add(Me.txtDepositIn_16)
        Me.tpIn.Controls.Add(Me.txtDepositIn_20)
        Me.tpIn.Controls.Add(Me.txtDepositIn_50)
        Me.tpIn.Controls.Add(Me.Label272)
        Me.tpIn.Controls.Add(Me.txto_inspect_2)
        Me.tpIn.Controls.Add(Me.txto_new_in_2)
        Me.tpIn.Controls.Add(Me.txto_in_2)
        Me.tpIn.Controls.Add(Me.txto_inspect_4)
        Me.tpIn.Controls.Add(Me.txto_new_in_4)
        Me.tpIn.Controls.Add(Me.txto_in_4)
        Me.tpIn.Controls.Add(Me.txto_inspect_5)
        Me.tpIn.Controls.Add(Me.txto_new_in_5)
        Me.tpIn.Controls.Add(Me.txto_in_5)
        Me.tpIn.Controls.Add(Me.txto_inspect_10)
        Me.tpIn.Controls.Add(Me.txto_new_in_10)
        Me.tpIn.Controls.Add(Me.txto_in_10)
        Me.tpIn.Controls.Add(Me.txto_inspect_14)
        Me.tpIn.Controls.Add(Me.txto_new_in_14)
        Me.tpIn.Controls.Add(Me.txto_in_14)
        Me.tpIn.Controls.Add(Me.txto_inspect_18)
        Me.tpIn.Controls.Add(Me.txto_new_in_18)
        Me.tpIn.Controls.Add(Me.txto_in_18)
        Me.tpIn.Controls.Add(Me.txto_inspect_16)
        Me.tpIn.Controls.Add(Me.txto_new_in_16)
        Me.tpIn.Controls.Add(Me.txto_in_16)
        Me.tpIn.Controls.Add(Me.txto_inspect_20)
        Me.tpIn.Controls.Add(Me.txto_new_in_20)
        Me.tpIn.Controls.Add(Me.txto_in_20)
        Me.tpIn.Controls.Add(Me.txto_inspect_50)
        Me.tpIn.Controls.Add(Me.txto_new_in_50)
        Me.tpIn.Controls.Add(Me.txto_in_50)
        Me.tpIn.Controls.Add(Me.Label212)
        Me.tpIn.Controls.Add(Me.Label213)
        Me.tpIn.Controls.Add(Me.Label214)
        Me.tpIn.Controls.Add(Me.Label215)
        Me.tpIn.Controls.Add(Me.Label216)
        Me.tpIn.Controls.Add(Me.Label217)
        Me.tpIn.Controls.Add(Me.Label264)
        Me.tpIn.Controls.Add(Me.Label266)
        Me.tpIn.Controls.Add(Me.Label267)
        Me.tpIn.Controls.Add(Me.Label268)
        Me.tpIn.Controls.Add(Me.Label269)
        Me.tpIn.Controls.Add(Me.Label270)
        Me.tpIn.Controls.Add(Me.Label271)
        Me.tpIn.Location = New System.Drawing.Point(4, 29)
        Me.tpIn.Name = "tpIn"
        Me.tpIn.Padding = New System.Windows.Forms.Padding(5)
        Me.tpIn.Size = New System.Drawing.Size(1009, 405)
        Me.tpIn.TabIndex = 0
        Me.tpIn.Text = "進場單"
        '
        'txtBarralUnitPrice_2
        '
        Me.txtBarralUnitPrice_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtBarralUnitPrice_2.Location = New System.Drawing.Point(762, 134)
        Me.txtBarralUnitPrice_2.Name = "txtBarralUnitPrice_2"
        Me.txtBarralUnitPrice_2.Size = New System.Drawing.Size(53, 30)
        Me.txtBarralUnitPrice_2.TabIndex = 529
        Me.txtBarralUnitPrice_2.Tag = "o_barrel_unit_price_2"
        '
        'txtBarralUnitPrice_4
        '
        Me.txtBarralUnitPrice_4.Location = New System.Drawing.Point(454, 134)
        Me.txtBarralUnitPrice_4.Name = "txtBarralUnitPrice_4"
        Me.txtBarralUnitPrice_4.Size = New System.Drawing.Size(53, 30)
        Me.txtBarralUnitPrice_4.TabIndex = 525
        Me.txtBarralUnitPrice_4.Tag = "o_barrel_unit_price_4"
        '
        'txtBarralUnitPrice_5
        '
        Me.txtBarralUnitPrice_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtBarralUnitPrice_5.Location = New System.Drawing.Point(685, 134)
        Me.txtBarralUnitPrice_5.Name = "txtBarralUnitPrice_5"
        Me.txtBarralUnitPrice_5.Size = New System.Drawing.Size(53, 30)
        Me.txtBarralUnitPrice_5.TabIndex = 528
        Me.txtBarralUnitPrice_5.Tag = "o_barrel_unit_price_5"
        '
        'txtBarralUnitPrice_10
        '
        Me.txtBarralUnitPrice_10.Location = New System.Drawing.Point(377, 134)
        Me.txtBarralUnitPrice_10.Name = "txtBarralUnitPrice_10"
        Me.txtBarralUnitPrice_10.Size = New System.Drawing.Size(53, 30)
        Me.txtBarralUnitPrice_10.TabIndex = 524
        Me.txtBarralUnitPrice_10.Tag = "o_barrel_unit_price_10"
        '
        'txtBarralUnitPrice_14
        '
        Me.txtBarralUnitPrice_14.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtBarralUnitPrice_14.Location = New System.Drawing.Point(608, 134)
        Me.txtBarralUnitPrice_14.Name = "txtBarralUnitPrice_14"
        Me.txtBarralUnitPrice_14.Size = New System.Drawing.Size(53, 30)
        Me.txtBarralUnitPrice_14.TabIndex = 527
        Me.txtBarralUnitPrice_14.Tag = "o_barrel_unit_price_14"
        '
        'txtBarralUnitPrice_18
        '
        Me.txtBarralUnitPrice_18.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtBarralUnitPrice_18.Location = New System.Drawing.Point(531, 134)
        Me.txtBarralUnitPrice_18.Name = "txtBarralUnitPrice_18"
        Me.txtBarralUnitPrice_18.Size = New System.Drawing.Size(53, 30)
        Me.txtBarralUnitPrice_18.TabIndex = 526
        Me.txtBarralUnitPrice_18.Tag = "o_barrel_unit_price_18"
        '
        'txtBarralUnitPrice_16
        '
        Me.txtBarralUnitPrice_16.Location = New System.Drawing.Point(300, 134)
        Me.txtBarralUnitPrice_16.Name = "txtBarralUnitPrice_16"
        Me.txtBarralUnitPrice_16.Size = New System.Drawing.Size(53, 30)
        Me.txtBarralUnitPrice_16.TabIndex = 523
        Me.txtBarralUnitPrice_16.Tag = "o_barrel_unit_price_16"
        '
        'txtBarralUnitPrice_20
        '
        Me.txtBarralUnitPrice_20.Location = New System.Drawing.Point(223, 134)
        Me.txtBarralUnitPrice_20.Name = "txtBarralUnitPrice_20"
        Me.txtBarralUnitPrice_20.Size = New System.Drawing.Size(53, 30)
        Me.txtBarralUnitPrice_20.TabIndex = 522
        Me.txtBarralUnitPrice_20.Tag = "o_barrel_unit_price_20"
        '
        'txtBarralUnitPrice_50
        '
        Me.txtBarralUnitPrice_50.Location = New System.Drawing.Point(146, 134)
        Me.txtBarralUnitPrice_50.Name = "txtBarralUnitPrice_50"
        Me.txtBarralUnitPrice_50.Size = New System.Drawing.Size(53, 30)
        Me.txtBarralUnitPrice_50.TabIndex = 521
        Me.txtBarralUnitPrice_50.Tag = "o_barrel_unit_price_50"
        '
        'Label100
        '
        Me.Label100.Location = New System.Drawing.Point(8, 140)
        Me.Label100.Name = "Label100"
        Me.Label100.Size = New System.Drawing.Size(114, 19)
        Me.Label100.TabIndex = 530
        Me.Label100.Text = "瓦斯桶單價"
        '
        'txtInspect2
        '
        Me.txtInspect2.BackColor = System.Drawing.SystemColors.Control
        Me.txtInspect2.Location = New System.Drawing.Point(762, 226)
        Me.txtInspect2.Name = "txtInspect2"
        Me.txtInspect2.ReadOnly = True
        Me.txtInspect2.Size = New System.Drawing.Size(53, 30)
        Me.txtInspect2.TabIndex = 519
        Me.txtInspect2.Tag = "cus_inspect_2"
        '
        'txtInspect4
        '
        Me.txtInspect4.Location = New System.Drawing.Point(454, 226)
        Me.txtInspect4.Name = "txtInspect4"
        Me.txtInspect4.ReadOnly = True
        Me.txtInspect4.Size = New System.Drawing.Size(53, 30)
        Me.txtInspect4.TabIndex = 518
        Me.txtInspect4.Tag = "cus_inspect_4"
        '
        'txtInspect5
        '
        Me.txtInspect5.BackColor = System.Drawing.SystemColors.Control
        Me.txtInspect5.Location = New System.Drawing.Point(685, 226)
        Me.txtInspect5.Name = "txtInspect5"
        Me.txtInspect5.ReadOnly = True
        Me.txtInspect5.Size = New System.Drawing.Size(53, 30)
        Me.txtInspect5.TabIndex = 517
        Me.txtInspect5.Tag = "cus_inspect_5"
        '
        'txtInspect10
        '
        Me.txtInspect10.Location = New System.Drawing.Point(377, 226)
        Me.txtInspect10.Name = "txtInspect10"
        Me.txtInspect10.ReadOnly = True
        Me.txtInspect10.Size = New System.Drawing.Size(53, 30)
        Me.txtInspect10.TabIndex = 516
        Me.txtInspect10.Tag = "cus_inspect_10"
        '
        'txtInspect14
        '
        Me.txtInspect14.BackColor = System.Drawing.SystemColors.Control
        Me.txtInspect14.Location = New System.Drawing.Point(608, 226)
        Me.txtInspect14.Name = "txtInspect14"
        Me.txtInspect14.ReadOnly = True
        Me.txtInspect14.Size = New System.Drawing.Size(53, 30)
        Me.txtInspect14.TabIndex = 515
        Me.txtInspect14.Tag = "cus_inspect_14"
        '
        'txtInspect18
        '
        Me.txtInspect18.BackColor = System.Drawing.SystemColors.Control
        Me.txtInspect18.Location = New System.Drawing.Point(531, 226)
        Me.txtInspect18.Name = "txtInspect18"
        Me.txtInspect18.ReadOnly = True
        Me.txtInspect18.Size = New System.Drawing.Size(53, 30)
        Me.txtInspect18.TabIndex = 514
        Me.txtInspect18.Tag = "cus_inspect_18"
        '
        'txtInspect16
        '
        Me.txtInspect16.Location = New System.Drawing.Point(300, 226)
        Me.txtInspect16.Name = "txtInspect16"
        Me.txtInspect16.ReadOnly = True
        Me.txtInspect16.Size = New System.Drawing.Size(53, 30)
        Me.txtInspect16.TabIndex = 513
        Me.txtInspect16.Tag = "cus_inspect_16"
        '
        'txtInspect20
        '
        Me.txtInspect20.Location = New System.Drawing.Point(223, 226)
        Me.txtInspect20.Name = "txtInspect20"
        Me.txtInspect20.ReadOnly = True
        Me.txtInspect20.Size = New System.Drawing.Size(53, 30)
        Me.txtInspect20.TabIndex = 512
        Me.txtInspect20.Tag = "cus_inspect_20"
        '
        'txtInspect50
        '
        Me.txtInspect50.Location = New System.Drawing.Point(146, 226)
        Me.txtInspect50.Name = "txtInspect50"
        Me.txtInspect50.ReadOnly = True
        Me.txtInspect50.Size = New System.Drawing.Size(53, 30)
        Me.txtInspect50.TabIndex = 511
        Me.txtInspect50.Tag = "cus_inspect_50"
        '
        'Label142
        '
        Me.Label142.Location = New System.Drawing.Point(8, 232)
        Me.Label142.Name = "Label142"
        Me.Label142.Size = New System.Drawing.Size(114, 19)
        Me.Label142.TabIndex = 520
        Me.Label142.Text = "檢驗結存瓶"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(762, 364)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(53, 30)
        Me.TextBox1.TabIndex = 510
        Me.TextBox1.Tag = "c_deposit_2"
        '
        'TextBox16
        '
        Me.TextBox16.Location = New System.Drawing.Point(685, 364)
        Me.TextBox16.Name = "TextBox16"
        Me.TextBox16.ReadOnly = True
        Me.TextBox16.Size = New System.Drawing.Size(53, 30)
        Me.TextBox16.TabIndex = 509
        Me.TextBox16.Tag = "c_deposit_5"
        '
        'TextBox17
        '
        Me.TextBox17.Location = New System.Drawing.Point(608, 364)
        Me.TextBox17.Name = "TextBox17"
        Me.TextBox17.ReadOnly = True
        Me.TextBox17.Size = New System.Drawing.Size(53, 30)
        Me.TextBox17.TabIndex = 508
        Me.TextBox17.Tag = "c_deposit_14"
        '
        'TextBox18
        '
        Me.TextBox18.Location = New System.Drawing.Point(531, 364)
        Me.TextBox18.Name = "TextBox18"
        Me.TextBox18.ReadOnly = True
        Me.TextBox18.Size = New System.Drawing.Size(53, 30)
        Me.TextBox18.TabIndex = 507
        Me.TextBox18.Tag = "c_deposit_18"
        '
        'TextBox19
        '
        Me.TextBox19.Location = New System.Drawing.Point(454, 364)
        Me.TextBox19.Name = "TextBox19"
        Me.TextBox19.ReadOnly = True
        Me.TextBox19.Size = New System.Drawing.Size(53, 30)
        Me.TextBox19.TabIndex = 506
        Me.TextBox19.Tag = "c_deposit_4"
        '
        'TextBox20
        '
        Me.TextBox20.Location = New System.Drawing.Point(377, 364)
        Me.TextBox20.Name = "TextBox20"
        Me.TextBox20.ReadOnly = True
        Me.TextBox20.Size = New System.Drawing.Size(53, 30)
        Me.TextBox20.TabIndex = 505
        Me.TextBox20.Tag = "c_deposit_10"
        '
        'TextBox73
        '
        Me.TextBox73.Location = New System.Drawing.Point(300, 364)
        Me.TextBox73.Name = "TextBox73"
        Me.TextBox73.ReadOnly = True
        Me.TextBox73.Size = New System.Drawing.Size(53, 30)
        Me.TextBox73.TabIndex = 504
        Me.TextBox73.Tag = "c_deposit_16"
        '
        'TextBox74
        '
        Me.TextBox74.Location = New System.Drawing.Point(223, 364)
        Me.TextBox74.Name = "TextBox74"
        Me.TextBox74.ReadOnly = True
        Me.TextBox74.Size = New System.Drawing.Size(53, 30)
        Me.TextBox74.TabIndex = 503
        Me.TextBox74.Tag = "c_deposit_20"
        '
        'TextBox78
        '
        Me.TextBox78.Location = New System.Drawing.Point(146, 364)
        Me.TextBox78.Name = "TextBox78"
        Me.TextBox78.ReadOnly = True
        Me.TextBox78.Size = New System.Drawing.Size(53, 30)
        Me.TextBox78.TabIndex = 502
        Me.TextBox78.Tag = "c_deposit_50"
        '
        'Label206
        '
        Me.Label206.Location = New System.Drawing.Point(8, 370)
        Me.Label206.Name = "Label206"
        Me.Label206.Size = New System.Drawing.Size(114, 19)
        Me.Label206.TabIndex = 501
        Me.Label206.Text = "寄桶結存瓶"
        '
        'txtBarrelIn_2
        '
        Me.txtBarrelIn_2.BackColor = System.Drawing.SystemColors.Control
        Me.txtBarrelIn_2.Location = New System.Drawing.Point(762, 272)
        Me.txtBarrelIn_2.Name = "txtBarrelIn_2"
        Me.txtBarrelIn_2.ReadOnly = True
        Me.txtBarrelIn_2.Size = New System.Drawing.Size(53, 30)
        Me.txtBarrelIn_2.TabIndex = 499
        Me.txtBarrelIn_2.Tag = "cus_gas_2"
        '
        'txtBarrelIn_4
        '
        Me.txtBarrelIn_4.Location = New System.Drawing.Point(454, 272)
        Me.txtBarrelIn_4.Name = "txtBarrelIn_4"
        Me.txtBarrelIn_4.ReadOnly = True
        Me.txtBarrelIn_4.Size = New System.Drawing.Size(53, 30)
        Me.txtBarrelIn_4.TabIndex = 498
        Me.txtBarrelIn_4.Tag = "cus_gas_4"
        '
        'txtBarrelIn_5
        '
        Me.txtBarrelIn_5.BackColor = System.Drawing.SystemColors.Control
        Me.txtBarrelIn_5.Location = New System.Drawing.Point(685, 272)
        Me.txtBarrelIn_5.Name = "txtBarrelIn_5"
        Me.txtBarrelIn_5.ReadOnly = True
        Me.txtBarrelIn_5.Size = New System.Drawing.Size(53, 30)
        Me.txtBarrelIn_5.TabIndex = 497
        Me.txtBarrelIn_5.Tag = "cus_gas_5"
        '
        'txtBarrelIn_10
        '
        Me.txtBarrelIn_10.Location = New System.Drawing.Point(377, 272)
        Me.txtBarrelIn_10.Name = "txtBarrelIn_10"
        Me.txtBarrelIn_10.ReadOnly = True
        Me.txtBarrelIn_10.Size = New System.Drawing.Size(53, 30)
        Me.txtBarrelIn_10.TabIndex = 496
        Me.txtBarrelIn_10.Tag = "cus_gas_10"
        '
        'txtBarrelIn_14
        '
        Me.txtBarrelIn_14.BackColor = System.Drawing.SystemColors.Control
        Me.txtBarrelIn_14.Location = New System.Drawing.Point(608, 272)
        Me.txtBarrelIn_14.Name = "txtBarrelIn_14"
        Me.txtBarrelIn_14.ReadOnly = True
        Me.txtBarrelIn_14.Size = New System.Drawing.Size(53, 30)
        Me.txtBarrelIn_14.TabIndex = 495
        Me.txtBarrelIn_14.Tag = "cus_gas_14"
        '
        'txtBarrelIn_18
        '
        Me.txtBarrelIn_18.BackColor = System.Drawing.SystemColors.Control
        Me.txtBarrelIn_18.Location = New System.Drawing.Point(531, 272)
        Me.txtBarrelIn_18.Name = "txtBarrelIn_18"
        Me.txtBarrelIn_18.ReadOnly = True
        Me.txtBarrelIn_18.Size = New System.Drawing.Size(53, 30)
        Me.txtBarrelIn_18.TabIndex = 494
        Me.txtBarrelIn_18.Tag = "cus_gas_18"
        '
        'txtBarrelIn_16
        '
        Me.txtBarrelIn_16.Location = New System.Drawing.Point(300, 272)
        Me.txtBarrelIn_16.Name = "txtBarrelIn_16"
        Me.txtBarrelIn_16.ReadOnly = True
        Me.txtBarrelIn_16.Size = New System.Drawing.Size(53, 30)
        Me.txtBarrelIn_16.TabIndex = 493
        Me.txtBarrelIn_16.Tag = "cus_gas_16"
        '
        'txtBarrelIn_20
        '
        Me.txtBarrelIn_20.Location = New System.Drawing.Point(223, 272)
        Me.txtBarrelIn_20.Name = "txtBarrelIn_20"
        Me.txtBarrelIn_20.ReadOnly = True
        Me.txtBarrelIn_20.Size = New System.Drawing.Size(53, 30)
        Me.txtBarrelIn_20.TabIndex = 492
        Me.txtBarrelIn_20.Tag = "cus_gas_20"
        '
        'txtBarrelIn_50
        '
        Me.txtBarrelIn_50.Location = New System.Drawing.Point(146, 272)
        Me.txtBarrelIn_50.Name = "txtBarrelIn_50"
        Me.txtBarrelIn_50.ReadOnly = True
        Me.txtBarrelIn_50.Size = New System.Drawing.Size(53, 30)
        Me.txtBarrelIn_50.TabIndex = 491
        Me.txtBarrelIn_50.Tag = "cus_gas_50"
        '
        'Label205
        '
        Me.Label205.Location = New System.Drawing.Point(8, 278)
        Me.Label205.Name = "Label205"
        Me.Label205.Size = New System.Drawing.Size(114, 19)
        Me.Label205.TabIndex = 500
        Me.Label205.Text = "結存瓶"
        '
        'txtDepositIn_2
        '
        Me.txtDepositIn_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDepositIn_2.Location = New System.Drawing.Point(762, 318)
        Me.txtDepositIn_2.Name = "txtDepositIn_2"
        Me.txtDepositIn_2.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositIn_2.TabIndex = 476
        Me.txtDepositIn_2.Tag = "o_deposit_in_2"
        '
        'txtDepositIn_4
        '
        Me.txtDepositIn_4.Location = New System.Drawing.Point(454, 318)
        Me.txtDepositIn_4.Name = "txtDepositIn_4"
        Me.txtDepositIn_4.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositIn_4.TabIndex = 19
        Me.txtDepositIn_4.Tag = "o_deposit_in_4"
        '
        'txtDepositIn_5
        '
        Me.txtDepositIn_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDepositIn_5.Location = New System.Drawing.Point(685, 318)
        Me.txtDepositIn_5.Name = "txtDepositIn_5"
        Me.txtDepositIn_5.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositIn_5.TabIndex = 474
        Me.txtDepositIn_5.Tag = "o_deposit_in_5"
        '
        'txtDepositIn_10
        '
        Me.txtDepositIn_10.Location = New System.Drawing.Point(377, 318)
        Me.txtDepositIn_10.Name = "txtDepositIn_10"
        Me.txtDepositIn_10.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositIn_10.TabIndex = 18
        Me.txtDepositIn_10.Tag = "o_deposit_in_10"
        '
        'txtDepositIn_14
        '
        Me.txtDepositIn_14.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDepositIn_14.Location = New System.Drawing.Point(608, 318)
        Me.txtDepositIn_14.Name = "txtDepositIn_14"
        Me.txtDepositIn_14.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositIn_14.TabIndex = 472
        Me.txtDepositIn_14.Tag = "o_deposit_in_14"
        '
        'txtDepositIn_18
        '
        Me.txtDepositIn_18.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDepositIn_18.Location = New System.Drawing.Point(531, 318)
        Me.txtDepositIn_18.Name = "txtDepositIn_18"
        Me.txtDepositIn_18.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositIn_18.TabIndex = 471
        Me.txtDepositIn_18.Tag = "o_deposit_in_18"
        '
        'txtDepositIn_16
        '
        Me.txtDepositIn_16.Location = New System.Drawing.Point(300, 318)
        Me.txtDepositIn_16.Name = "txtDepositIn_16"
        Me.txtDepositIn_16.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositIn_16.TabIndex = 17
        Me.txtDepositIn_16.Tag = "o_deposit_in_16"
        '
        'txtDepositIn_20
        '
        Me.txtDepositIn_20.Location = New System.Drawing.Point(223, 318)
        Me.txtDepositIn_20.Name = "txtDepositIn_20"
        Me.txtDepositIn_20.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositIn_20.TabIndex = 16
        Me.txtDepositIn_20.Tag = "o_deposit_in_20"
        '
        'txtDepositIn_50
        '
        Me.txtDepositIn_50.Location = New System.Drawing.Point(146, 318)
        Me.txtDepositIn_50.Name = "txtDepositIn_50"
        Me.txtDepositIn_50.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositIn_50.TabIndex = 15
        Me.txtDepositIn_50.Tag = "o_deposit_in_50"
        '
        'Label272
        '
        Me.Label272.Location = New System.Drawing.Point(8, 324)
        Me.Label272.Name = "Label272"
        Me.Label272.Size = New System.Drawing.Size(114, 19)
        Me.Label272.TabIndex = 490
        Me.Label272.Text = "寄桶"
        '
        'txto_inspect_2
        '
        Me.txto_inspect_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_inspect_2.Location = New System.Drawing.Point(762, 180)
        Me.txto_inspect_2.Name = "txto_inspect_2"
        Me.txto_inspect_2.Size = New System.Drawing.Size(53, 30)
        Me.txto_inspect_2.TabIndex = 467
        Me.txto_inspect_2.Tag = "o_inspect_2"
        '
        'txto_new_in_2
        '
        Me.txto_new_in_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_new_in_2.Location = New System.Drawing.Point(762, 88)
        Me.txto_new_in_2.Name = "txto_new_in_2"
        Me.txto_new_in_2.Size = New System.Drawing.Size(53, 30)
        Me.txto_new_in_2.TabIndex = 458
        Me.txto_new_in_2.Tag = "o_new_in_2"
        '
        'txto_in_2
        '
        Me.txto_in_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_in_2.Location = New System.Drawing.Point(762, 42)
        Me.txto_in_2.Name = "txto_in_2"
        Me.txto_in_2.Size = New System.Drawing.Size(53, 30)
        Me.txto_in_2.TabIndex = 449
        Me.txto_in_2.Tag = "o_in_2"
        '
        'txto_inspect_4
        '
        Me.txto_inspect_4.Location = New System.Drawing.Point(454, 180)
        Me.txto_inspect_4.Name = "txto_inspect_4"
        Me.txto_inspect_4.Size = New System.Drawing.Size(53, 30)
        Me.txto_inspect_4.TabIndex = 14
        Me.txto_inspect_4.Tag = "o_inspect_4"
        '
        'txto_new_in_4
        '
        Me.txto_new_in_4.Location = New System.Drawing.Point(454, 88)
        Me.txto_new_in_4.Name = "txto_new_in_4"
        Me.txto_new_in_4.Size = New System.Drawing.Size(53, 30)
        Me.txto_new_in_4.TabIndex = 9
        Me.txto_new_in_4.Tag = "o_new_in_4"
        '
        'txto_in_4
        '
        Me.txto_in_4.Location = New System.Drawing.Point(454, 42)
        Me.txto_in_4.Name = "txto_in_4"
        Me.txto_in_4.Size = New System.Drawing.Size(53, 30)
        Me.txto_in_4.TabIndex = 4
        Me.txto_in_4.Tag = "o_in_4"
        '
        'txto_inspect_5
        '
        Me.txto_inspect_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_inspect_5.Location = New System.Drawing.Point(685, 180)
        Me.txto_inspect_5.Name = "txto_inspect_5"
        Me.txto_inspect_5.Size = New System.Drawing.Size(53, 30)
        Me.txto_inspect_5.TabIndex = 465
        Me.txto_inspect_5.Tag = "o_inspect_5"
        '
        'txto_new_in_5
        '
        Me.txto_new_in_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_new_in_5.Location = New System.Drawing.Point(685, 88)
        Me.txto_new_in_5.Name = "txto_new_in_5"
        Me.txto_new_in_5.Size = New System.Drawing.Size(53, 30)
        Me.txto_new_in_5.TabIndex = 456
        Me.txto_new_in_5.Tag = "o_new_in_5"
        '
        'txto_in_5
        '
        Me.txto_in_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_in_5.Location = New System.Drawing.Point(685, 42)
        Me.txto_in_5.Name = "txto_in_5"
        Me.txto_in_5.Size = New System.Drawing.Size(53, 30)
        Me.txto_in_5.TabIndex = 447
        Me.txto_in_5.Tag = "o_in_5"
        '
        'txto_inspect_10
        '
        Me.txto_inspect_10.Location = New System.Drawing.Point(377, 180)
        Me.txto_inspect_10.Name = "txto_inspect_10"
        Me.txto_inspect_10.Size = New System.Drawing.Size(53, 30)
        Me.txto_inspect_10.TabIndex = 13
        Me.txto_inspect_10.Tag = "o_inspect_10"
        '
        'txto_new_in_10
        '
        Me.txto_new_in_10.Location = New System.Drawing.Point(377, 88)
        Me.txto_new_in_10.Name = "txto_new_in_10"
        Me.txto_new_in_10.Size = New System.Drawing.Size(53, 30)
        Me.txto_new_in_10.TabIndex = 8
        Me.txto_new_in_10.Tag = "o_new_in_10"
        '
        'txto_in_10
        '
        Me.txto_in_10.Location = New System.Drawing.Point(377, 42)
        Me.txto_in_10.Name = "txto_in_10"
        Me.txto_in_10.Size = New System.Drawing.Size(53, 30)
        Me.txto_in_10.TabIndex = 3
        Me.txto_in_10.Tag = "o_in_10"
        '
        'txto_inspect_14
        '
        Me.txto_inspect_14.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_inspect_14.Location = New System.Drawing.Point(608, 180)
        Me.txto_inspect_14.Name = "txto_inspect_14"
        Me.txto_inspect_14.Size = New System.Drawing.Size(53, 30)
        Me.txto_inspect_14.TabIndex = 463
        Me.txto_inspect_14.Tag = "o_inspect_14"
        '
        'txto_new_in_14
        '
        Me.txto_new_in_14.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_new_in_14.Location = New System.Drawing.Point(608, 88)
        Me.txto_new_in_14.Name = "txto_new_in_14"
        Me.txto_new_in_14.Size = New System.Drawing.Size(53, 30)
        Me.txto_new_in_14.TabIndex = 454
        Me.txto_new_in_14.Tag = "o_new_in_14"
        '
        'txto_in_14
        '
        Me.txto_in_14.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_in_14.Location = New System.Drawing.Point(608, 42)
        Me.txto_in_14.Name = "txto_in_14"
        Me.txto_in_14.Size = New System.Drawing.Size(53, 30)
        Me.txto_in_14.TabIndex = 445
        Me.txto_in_14.Tag = "o_in_14"
        '
        'txto_inspect_18
        '
        Me.txto_inspect_18.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_inspect_18.Location = New System.Drawing.Point(531, 180)
        Me.txto_inspect_18.Name = "txto_inspect_18"
        Me.txto_inspect_18.Size = New System.Drawing.Size(53, 30)
        Me.txto_inspect_18.TabIndex = 462
        Me.txto_inspect_18.Tag = "o_inspect_18"
        '
        'txto_new_in_18
        '
        Me.txto_new_in_18.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_new_in_18.Location = New System.Drawing.Point(531, 88)
        Me.txto_new_in_18.Name = "txto_new_in_18"
        Me.txto_new_in_18.Size = New System.Drawing.Size(53, 30)
        Me.txto_new_in_18.TabIndex = 453
        Me.txto_new_in_18.Tag = "o_new_in_18"
        '
        'txto_in_18
        '
        Me.txto_in_18.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txto_in_18.Location = New System.Drawing.Point(531, 42)
        Me.txto_in_18.Name = "txto_in_18"
        Me.txto_in_18.Size = New System.Drawing.Size(53, 30)
        Me.txto_in_18.TabIndex = 444
        Me.txto_in_18.Tag = "o_in_18"
        '
        'txto_inspect_16
        '
        Me.txto_inspect_16.Location = New System.Drawing.Point(300, 180)
        Me.txto_inspect_16.Name = "txto_inspect_16"
        Me.txto_inspect_16.Size = New System.Drawing.Size(53, 30)
        Me.txto_inspect_16.TabIndex = 12
        Me.txto_inspect_16.Tag = "o_inspect_16"
        '
        'txto_new_in_16
        '
        Me.txto_new_in_16.Location = New System.Drawing.Point(300, 88)
        Me.txto_new_in_16.Name = "txto_new_in_16"
        Me.txto_new_in_16.Size = New System.Drawing.Size(53, 30)
        Me.txto_new_in_16.TabIndex = 7
        Me.txto_new_in_16.Tag = "o_new_in_16"
        '
        'txto_in_16
        '
        Me.txto_in_16.Location = New System.Drawing.Point(300, 42)
        Me.txto_in_16.Name = "txto_in_16"
        Me.txto_in_16.Size = New System.Drawing.Size(53, 30)
        Me.txto_in_16.TabIndex = 2
        Me.txto_in_16.Tag = "o_in_16"
        '
        'txto_inspect_20
        '
        Me.txto_inspect_20.Location = New System.Drawing.Point(223, 180)
        Me.txto_inspect_20.Name = "txto_inspect_20"
        Me.txto_inspect_20.Size = New System.Drawing.Size(53, 30)
        Me.txto_inspect_20.TabIndex = 11
        Me.txto_inspect_20.Tag = "o_inspect_20"
        '
        'txto_new_in_20
        '
        Me.txto_new_in_20.Location = New System.Drawing.Point(223, 88)
        Me.txto_new_in_20.Name = "txto_new_in_20"
        Me.txto_new_in_20.Size = New System.Drawing.Size(53, 30)
        Me.txto_new_in_20.TabIndex = 6
        Me.txto_new_in_20.Tag = "o_new_in_20"
        '
        'txto_in_20
        '
        Me.txto_in_20.Location = New System.Drawing.Point(223, 42)
        Me.txto_in_20.Name = "txto_in_20"
        Me.txto_in_20.Size = New System.Drawing.Size(53, 30)
        Me.txto_in_20.TabIndex = 1
        Me.txto_in_20.Tag = "o_in_20"
        '
        'txto_inspect_50
        '
        Me.txto_inspect_50.Location = New System.Drawing.Point(146, 180)
        Me.txto_inspect_50.Name = "txto_inspect_50"
        Me.txto_inspect_50.Size = New System.Drawing.Size(53, 30)
        Me.txto_inspect_50.TabIndex = 10
        Me.txto_inspect_50.Tag = "o_inspect_50"
        '
        'txto_new_in_50
        '
        Me.txto_new_in_50.Location = New System.Drawing.Point(146, 88)
        Me.txto_new_in_50.Name = "txto_new_in_50"
        Me.txto_new_in_50.Size = New System.Drawing.Size(53, 30)
        Me.txto_new_in_50.TabIndex = 5
        Me.txto_new_in_50.Tag = "o_new_in_50"
        '
        'txto_in_50
        '
        Me.txto_in_50.Location = New System.Drawing.Point(146, 42)
        Me.txto_in_50.Name = "txto_in_50"
        Me.txto_in_50.Size = New System.Drawing.Size(53, 30)
        Me.txto_in_50.TabIndex = 0
        Me.txto_in_50.Tag = "o_in_50"
        '
        'Label212
        '
        Me.Label212.Location = New System.Drawing.Point(8, 186)
        Me.Label212.Name = "Label212"
        Me.Label212.Size = New System.Drawing.Size(114, 19)
        Me.Label212.TabIndex = 489
        Me.Label212.Text = "檢驗瓶"
        '
        'Label213
        '
        Me.Label213.Location = New System.Drawing.Point(8, 94)
        Me.Label213.Name = "Label213"
        Me.Label213.Size = New System.Drawing.Size(114, 19)
        Me.Label213.TabIndex = 488
        Me.Label213.Text = "新瓶"
        '
        'Label214
        '
        Me.Label214.Location = New System.Drawing.Point(8, 48)
        Me.Label214.Name = "Label214"
        Me.Label214.Size = New System.Drawing.Size(114, 19)
        Me.Label214.TabIndex = 487
        Me.Label214.Text = "收空瓶"
        '
        'Label215
        '
        Me.Label215.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label215.Location = New System.Drawing.Point(762, 5)
        Me.Label215.Name = "Label215"
        Me.Label215.Size = New System.Drawing.Size(53, 19)
        Me.Label215.TabIndex = 486
        Me.Label215.Text = "2KG"
        Me.Label215.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label216
        '
        Me.Label216.Location = New System.Drawing.Point(454, 5)
        Me.Label216.Name = "Label216"
        Me.Label216.Size = New System.Drawing.Size(53, 19)
        Me.Label216.TabIndex = 485
        Me.Label216.Text = "4KG"
        Me.Label216.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label217
        '
        Me.Label217.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label217.Location = New System.Drawing.Point(685, 5)
        Me.Label217.Name = "Label217"
        Me.Label217.Size = New System.Drawing.Size(53, 19)
        Me.Label217.TabIndex = 484
        Me.Label217.Text = "5KG"
        Me.Label217.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label264
        '
        Me.Label264.Location = New System.Drawing.Point(377, 5)
        Me.Label264.Name = "Label264"
        Me.Label264.Size = New System.Drawing.Size(53, 19)
        Me.Label264.TabIndex = 483
        Me.Label264.Text = "10KG"
        Me.Label264.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label266
        '
        Me.Label266.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label266.Location = New System.Drawing.Point(608, 5)
        Me.Label266.Name = "Label266"
        Me.Label266.Size = New System.Drawing.Size(53, 19)
        Me.Label266.TabIndex = 482
        Me.Label266.Text = "14KG"
        Me.Label266.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label267
        '
        Me.Label267.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label267.Location = New System.Drawing.Point(531, 5)
        Me.Label267.Name = "Label267"
        Me.Label267.Size = New System.Drawing.Size(53, 19)
        Me.Label267.TabIndex = 481
        Me.Label267.Text = "18KG"
        Me.Label267.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label268
        '
        Me.Label268.Location = New System.Drawing.Point(300, 5)
        Me.Label268.Name = "Label268"
        Me.Label268.Size = New System.Drawing.Size(53, 19)
        Me.Label268.TabIndex = 480
        Me.Label268.Text = "16KG"
        Me.Label268.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label269
        '
        Me.Label269.Location = New System.Drawing.Point(223, 5)
        Me.Label269.Name = "Label269"
        Me.Label269.Size = New System.Drawing.Size(53, 19)
        Me.Label269.TabIndex = 479
        Me.Label269.Text = "20KG"
        Me.Label269.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label270
        '
        Me.Label270.Location = New System.Drawing.Point(8, 5)
        Me.Label270.Name = "Label270"
        Me.Label270.Size = New System.Drawing.Size(114, 19)
        Me.Label270.TabIndex = 477
        Me.Label270.Text = "容器種類"
        '
        'Label271
        '
        Me.Label271.Location = New System.Drawing.Point(146, 5)
        Me.Label271.Name = "Label271"
        Me.Label271.Size = New System.Drawing.Size(53, 19)
        Me.Label271.TabIndex = 478
        Me.Label271.Text = "50KG"
        Me.Label271.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tpOut
        '
        Me.tpOut.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.tpOut.Controls.Add(Me.txtCarDeposit_2)
        Me.tpOut.Controls.Add(Me.txtCarDeposit_5)
        Me.tpOut.Controls.Add(Me.txtCarDeposit_14)
        Me.tpOut.Controls.Add(Me.txtCarDeposit_18)
        Me.tpOut.Controls.Add(Me.txtCarDeposit_4)
        Me.tpOut.Controls.Add(Me.txtCarDeposit_10)
        Me.tpOut.Controls.Add(Me.txtCarDeposit_16)
        Me.tpOut.Controls.Add(Me.txtCarDeposit_20)
        Me.tpOut.Controls.Add(Me.txtCarDeposit_50)
        Me.tpOut.Controls.Add(Me.Label277)
        Me.tpOut.Controls.Add(Me.Label274)
        Me.tpOut.Controls.Add(Me.cmbCarOut_ord)
        Me.tpOut.Controls.Add(Me.txtCusGas_2)
        Me.tpOut.Controls.Add(Me.txtCusGas_5)
        Me.tpOut.Controls.Add(Me.txtCusGas_14)
        Me.tpOut.Controls.Add(Me.txtCusGas_18)
        Me.tpOut.Controls.Add(Me.txtCusGas_4)
        Me.tpOut.Controls.Add(Me.txtCusGas_10)
        Me.tpOut.Controls.Add(Me.txtCusGas_16)
        Me.tpOut.Controls.Add(Me.txtCusGas_20)
        Me.tpOut.Controls.Add(Me.txtCusGas_50)
        Me.tpOut.Controls.Add(Me.Label273)
        Me.tpOut.Controls.Add(Me.txtDepositOut_2)
        Me.tpOut.Controls.Add(Me.txtDepositOut_5)
        Me.tpOut.Controls.Add(Me.txtDepositOut_14)
        Me.tpOut.Controls.Add(Me.txtDepositOut_18)
        Me.tpOut.Controls.Add(Me.txtDepositOut_4)
        Me.tpOut.Controls.Add(Me.txtDepositOut_10)
        Me.tpOut.Controls.Add(Me.txtDepositOut_16)
        Me.tpOut.Controls.Add(Me.txtDepositOut_20)
        Me.tpOut.Controls.Add(Me.txtDepositOut_50)
        Me.tpOut.Controls.Add(Me.Label157)
        Me.tpOut.Controls.Add(Me.txtEmpty_2)
        Me.tpOut.Controls.Add(Me.txtGas_c_2)
        Me.tpOut.Controls.Add(Me.txtGas_2)
        Me.tpOut.Controls.Add(Me.txtEmpty_5)
        Me.tpOut.Controls.Add(Me.txtGas_c_5)
        Me.tpOut.Controls.Add(Me.txtGas_5)
        Me.tpOut.Controls.Add(Me.txtEmpty_14)
        Me.tpOut.Controls.Add(Me.txtGas_c_14)
        Me.tpOut.Controls.Add(Me.txtGas_14)
        Me.tpOut.Controls.Add(Me.txtEmpty_18)
        Me.tpOut.Controls.Add(Me.txtGas_c_18)
        Me.tpOut.Controls.Add(Me.txtGas_18)
        Me.tpOut.Controls.Add(Me.txtEmpty_4)
        Me.tpOut.Controls.Add(Me.txtGas_c_4)
        Me.tpOut.Controls.Add(Me.txtGas_4)
        Me.tpOut.Controls.Add(Me.txtEmpty_10)
        Me.tpOut.Controls.Add(Me.txtGas_c_10)
        Me.tpOut.Controls.Add(Me.txtGas_10)
        Me.tpOut.Controls.Add(Me.txtEmpty_16)
        Me.tpOut.Controls.Add(Me.txtGas_c_16)
        Me.tpOut.Controls.Add(Me.txtGas_16)
        Me.tpOut.Controls.Add(Me.txtEmpty_20)
        Me.tpOut.Controls.Add(Me.txtGas_c_20)
        Me.tpOut.Controls.Add(Me.txtGas_20)
        Me.tpOut.Controls.Add(Me.txtEmpty_50)
        Me.tpOut.Controls.Add(Me.txtGas_c_50)
        Me.tpOut.Controls.Add(Me.txtGas_50)
        Me.tpOut.Controls.Add(Me.Label44)
        Me.tpOut.Controls.Add(Me.Label45)
        Me.tpOut.Controls.Add(Me.Label56)
        Me.tpOut.Controls.Add(Me.Label57)
        Me.tpOut.Controls.Add(Me.Label58)
        Me.tpOut.Controls.Add(Me.Label59)
        Me.tpOut.Controls.Add(Me.Label78)
        Me.tpOut.Controls.Add(Me.Label103)
        Me.tpOut.Controls.Add(Me.Label105)
        Me.tpOut.Controls.Add(Me.Label125)
        Me.tpOut.Controls.Add(Me.Label158)
        Me.tpOut.Controls.Add(Me.Label160)
        Me.tpOut.Controls.Add(Me.Label211)
        Me.tpOut.Location = New System.Drawing.Point(4, 29)
        Me.tpOut.Name = "tpOut"
        Me.tpOut.Padding = New System.Windows.Forms.Padding(5)
        Me.tpOut.Size = New System.Drawing.Size(1009, 405)
        Me.tpOut.TabIndex = 1
        Me.tpOut.Text = "出場單"
        '
        'txtCarDeposit_2
        '
        Me.txtCarDeposit_2.Location = New System.Drawing.Point(762, 257)
        Me.txtCarDeposit_2.Name = "txtCarDeposit_2"
        Me.txtCarDeposit_2.ReadOnly = True
        Me.txtCarDeposit_2.Size = New System.Drawing.Size(53, 30)
        Me.txtCarDeposit_2.TabIndex = 503
        Me.txtCarDeposit_2.Tag = "c_deposit_2"
        '
        'txtCarDeposit_5
        '
        Me.txtCarDeposit_5.Location = New System.Drawing.Point(685, 257)
        Me.txtCarDeposit_5.Name = "txtCarDeposit_5"
        Me.txtCarDeposit_5.ReadOnly = True
        Me.txtCarDeposit_5.Size = New System.Drawing.Size(53, 30)
        Me.txtCarDeposit_5.TabIndex = 502
        Me.txtCarDeposit_5.Tag = "c_deposit_5"
        '
        'txtCarDeposit_14
        '
        Me.txtCarDeposit_14.Location = New System.Drawing.Point(608, 257)
        Me.txtCarDeposit_14.Name = "txtCarDeposit_14"
        Me.txtCarDeposit_14.ReadOnly = True
        Me.txtCarDeposit_14.Size = New System.Drawing.Size(53, 30)
        Me.txtCarDeposit_14.TabIndex = 501
        Me.txtCarDeposit_14.Tag = "c_deposit_14"
        '
        'txtCarDeposit_18
        '
        Me.txtCarDeposit_18.Location = New System.Drawing.Point(531, 257)
        Me.txtCarDeposit_18.Name = "txtCarDeposit_18"
        Me.txtCarDeposit_18.ReadOnly = True
        Me.txtCarDeposit_18.Size = New System.Drawing.Size(53, 30)
        Me.txtCarDeposit_18.TabIndex = 500
        Me.txtCarDeposit_18.Tag = "c_deposit_18"
        '
        'txtCarDeposit_4
        '
        Me.txtCarDeposit_4.Location = New System.Drawing.Point(454, 257)
        Me.txtCarDeposit_4.Name = "txtCarDeposit_4"
        Me.txtCarDeposit_4.ReadOnly = True
        Me.txtCarDeposit_4.Size = New System.Drawing.Size(53, 30)
        Me.txtCarDeposit_4.TabIndex = 499
        Me.txtCarDeposit_4.Tag = "c_deposit_4"
        '
        'txtCarDeposit_10
        '
        Me.txtCarDeposit_10.Location = New System.Drawing.Point(377, 257)
        Me.txtCarDeposit_10.Name = "txtCarDeposit_10"
        Me.txtCarDeposit_10.ReadOnly = True
        Me.txtCarDeposit_10.Size = New System.Drawing.Size(53, 30)
        Me.txtCarDeposit_10.TabIndex = 498
        Me.txtCarDeposit_10.Tag = "c_deposit_10"
        '
        'txtCarDeposit_16
        '
        Me.txtCarDeposit_16.Location = New System.Drawing.Point(300, 257)
        Me.txtCarDeposit_16.Name = "txtCarDeposit_16"
        Me.txtCarDeposit_16.ReadOnly = True
        Me.txtCarDeposit_16.Size = New System.Drawing.Size(53, 30)
        Me.txtCarDeposit_16.TabIndex = 497
        Me.txtCarDeposit_16.Tag = "c_deposit_16"
        '
        'txtCarDeposit_20
        '
        Me.txtCarDeposit_20.Location = New System.Drawing.Point(223, 257)
        Me.txtCarDeposit_20.Name = "txtCarDeposit_20"
        Me.txtCarDeposit_20.ReadOnly = True
        Me.txtCarDeposit_20.Size = New System.Drawing.Size(53, 30)
        Me.txtCarDeposit_20.TabIndex = 496
        Me.txtCarDeposit_20.Tag = "c_deposit_20"
        '
        'txtCarDeposit_50
        '
        Me.txtCarDeposit_50.Location = New System.Drawing.Point(146, 257)
        Me.txtCarDeposit_50.Name = "txtCarDeposit_50"
        Me.txtCarDeposit_50.ReadOnly = True
        Me.txtCarDeposit_50.Size = New System.Drawing.Size(53, 30)
        Me.txtCarDeposit_50.TabIndex = 495
        Me.txtCarDeposit_50.Tag = "c_deposit_50"
        '
        'Label277
        '
        Me.Label277.Location = New System.Drawing.Point(8, 263)
        Me.Label277.Name = "Label277"
        Me.Label277.Size = New System.Drawing.Size(114, 19)
        Me.Label277.TabIndex = 494
        Me.Label277.Text = "寄桶結存瓶"
        '
        'Label274
        '
        Me.Label274.AutoSize = True
        Me.Label274.Location = New System.Drawing.Point(821, 263)
        Me.Label274.Name = "Label274"
        Me.Label274.Size = New System.Drawing.Size(51, 19)
        Me.Label274.TabIndex = 493
        Me.Label274.Text = "車號"
        '
        'cmbCarOut_ord
        '
        Me.cmbCarOut_ord.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCarOut_ord.FormattingEnabled = True
        Me.cmbCarOut_ord.Location = New System.Drawing.Point(878, 260)
        Me.cmbCarOut_ord.Name = "cmbCarOut_ord"
        Me.cmbCarOut_ord.Size = New System.Drawing.Size(118, 27)
        Me.cmbCarOut_ord.TabIndex = 492
        Me.cmbCarOut_ord.Tag = "o_deposit_out_c_id"
        '
        'txtCusGas_2
        '
        Me.txtCusGas_2.Location = New System.Drawing.Point(762, 171)
        Me.txtCusGas_2.Name = "txtCusGas_2"
        Me.txtCusGas_2.ReadOnly = True
        Me.txtCusGas_2.Size = New System.Drawing.Size(53, 30)
        Me.txtCusGas_2.TabIndex = 490
        Me.txtCusGas_2.Tag = "cus_gas_2"
        '
        'txtCusGas_5
        '
        Me.txtCusGas_5.Location = New System.Drawing.Point(685, 171)
        Me.txtCusGas_5.Name = "txtCusGas_5"
        Me.txtCusGas_5.ReadOnly = True
        Me.txtCusGas_5.Size = New System.Drawing.Size(53, 30)
        Me.txtCusGas_5.TabIndex = 489
        Me.txtCusGas_5.Tag = "cus_gas_5"
        '
        'txtCusGas_14
        '
        Me.txtCusGas_14.Location = New System.Drawing.Point(608, 171)
        Me.txtCusGas_14.Name = "txtCusGas_14"
        Me.txtCusGas_14.ReadOnly = True
        Me.txtCusGas_14.Size = New System.Drawing.Size(53, 30)
        Me.txtCusGas_14.TabIndex = 488
        Me.txtCusGas_14.Tag = "cus_gas_14"
        '
        'txtCusGas_18
        '
        Me.txtCusGas_18.Location = New System.Drawing.Point(531, 171)
        Me.txtCusGas_18.Name = "txtCusGas_18"
        Me.txtCusGas_18.ReadOnly = True
        Me.txtCusGas_18.Size = New System.Drawing.Size(53, 30)
        Me.txtCusGas_18.TabIndex = 487
        Me.txtCusGas_18.Tag = "cus_gas_18"
        '
        'txtCusGas_4
        '
        Me.txtCusGas_4.Location = New System.Drawing.Point(454, 171)
        Me.txtCusGas_4.Name = "txtCusGas_4"
        Me.txtCusGas_4.ReadOnly = True
        Me.txtCusGas_4.Size = New System.Drawing.Size(53, 30)
        Me.txtCusGas_4.TabIndex = 486
        Me.txtCusGas_4.Tag = "cus_gas_4"
        '
        'txtCusGas_10
        '
        Me.txtCusGas_10.Location = New System.Drawing.Point(377, 171)
        Me.txtCusGas_10.Name = "txtCusGas_10"
        Me.txtCusGas_10.ReadOnly = True
        Me.txtCusGas_10.Size = New System.Drawing.Size(53, 30)
        Me.txtCusGas_10.TabIndex = 485
        Me.txtCusGas_10.Tag = "cus_gas_10"
        '
        'txtCusGas_16
        '
        Me.txtCusGas_16.Location = New System.Drawing.Point(300, 171)
        Me.txtCusGas_16.Name = "txtCusGas_16"
        Me.txtCusGas_16.ReadOnly = True
        Me.txtCusGas_16.Size = New System.Drawing.Size(53, 30)
        Me.txtCusGas_16.TabIndex = 484
        Me.txtCusGas_16.Tag = "cus_gas_16"
        '
        'txtCusGas_20
        '
        Me.txtCusGas_20.Location = New System.Drawing.Point(223, 171)
        Me.txtCusGas_20.Name = "txtCusGas_20"
        Me.txtCusGas_20.ReadOnly = True
        Me.txtCusGas_20.Size = New System.Drawing.Size(53, 30)
        Me.txtCusGas_20.TabIndex = 483
        Me.txtCusGas_20.Tag = "cus_gas_20"
        '
        'txtCusGas_50
        '
        Me.txtCusGas_50.Location = New System.Drawing.Point(146, 171)
        Me.txtCusGas_50.Name = "txtCusGas_50"
        Me.txtCusGas_50.ReadOnly = True
        Me.txtCusGas_50.Size = New System.Drawing.Size(53, 30)
        Me.txtCusGas_50.TabIndex = 482
        Me.txtCusGas_50.Tag = "cus_gas_50"
        '
        'Label273
        '
        Me.Label273.Location = New System.Drawing.Point(8, 220)
        Me.Label273.Name = "Label273"
        Me.Label273.Size = New System.Drawing.Size(114, 19)
        Me.Label273.TabIndex = 491
        Me.Label273.Text = "寄桶"
        '
        'txtDepositOut_2
        '
        Me.txtDepositOut_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDepositOut_2.Location = New System.Drawing.Point(762, 214)
        Me.txtDepositOut_2.Name = "txtDepositOut_2"
        Me.txtDepositOut_2.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositOut_2.TabIndex = 480
        Me.txtDepositOut_2.Tag = "o_deposit_out_2"
        '
        'txtDepositOut_5
        '
        Me.txtDepositOut_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDepositOut_5.Location = New System.Drawing.Point(685, 214)
        Me.txtDepositOut_5.Name = "txtDepositOut_5"
        Me.txtDepositOut_5.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositOut_5.TabIndex = 479
        Me.txtDepositOut_5.Tag = "o_deposit_out_5"
        '
        'txtDepositOut_14
        '
        Me.txtDepositOut_14.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDepositOut_14.Location = New System.Drawing.Point(608, 214)
        Me.txtDepositOut_14.Name = "txtDepositOut_14"
        Me.txtDepositOut_14.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositOut_14.TabIndex = 478
        Me.txtDepositOut_14.Tag = "o_deposit_out_14"
        '
        'txtDepositOut_18
        '
        Me.txtDepositOut_18.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDepositOut_18.Location = New System.Drawing.Point(531, 214)
        Me.txtDepositOut_18.Name = "txtDepositOut_18"
        Me.txtDepositOut_18.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositOut_18.TabIndex = 477
        Me.txtDepositOut_18.Tag = "o_deposit_out_18"
        '
        'txtDepositOut_4
        '
        Me.txtDepositOut_4.Location = New System.Drawing.Point(454, 214)
        Me.txtDepositOut_4.Name = "txtDepositOut_4"
        Me.txtDepositOut_4.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositOut_4.TabIndex = 39
        Me.txtDepositOut_4.Tag = "o_deposit_out_4"
        '
        'txtDepositOut_10
        '
        Me.txtDepositOut_10.Location = New System.Drawing.Point(377, 214)
        Me.txtDepositOut_10.Name = "txtDepositOut_10"
        Me.txtDepositOut_10.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositOut_10.TabIndex = 38
        Me.txtDepositOut_10.Tag = "o_deposit_out_10"
        '
        'txtDepositOut_16
        '
        Me.txtDepositOut_16.Location = New System.Drawing.Point(300, 214)
        Me.txtDepositOut_16.Name = "txtDepositOut_16"
        Me.txtDepositOut_16.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositOut_16.TabIndex = 37
        Me.txtDepositOut_16.Tag = "o_deposit_out_16"
        '
        'txtDepositOut_20
        '
        Me.txtDepositOut_20.Location = New System.Drawing.Point(223, 214)
        Me.txtDepositOut_20.Name = "txtDepositOut_20"
        Me.txtDepositOut_20.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositOut_20.TabIndex = 36
        Me.txtDepositOut_20.Tag = "o_deposit_out_20"
        '
        'txtDepositOut_50
        '
        Me.txtDepositOut_50.Location = New System.Drawing.Point(146, 214)
        Me.txtDepositOut_50.Name = "txtDepositOut_50"
        Me.txtDepositOut_50.Size = New System.Drawing.Size(53, 30)
        Me.txtDepositOut_50.TabIndex = 35
        Me.txtDepositOut_50.Tag = "o_deposit_out_50"
        '
        'Label157
        '
        Me.Label157.Location = New System.Drawing.Point(8, 177)
        Me.Label157.Name = "Label157"
        Me.Label157.Size = New System.Drawing.Size(114, 19)
        Me.Label157.TabIndex = 481
        Me.Label157.Text = "結存瓶"
        '
        'txtEmpty_2
        '
        Me.txtEmpty_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtEmpty_2.Location = New System.Drawing.Point(762, 128)
        Me.txtEmpty_2.Name = "txtEmpty_2"
        Me.txtEmpty_2.Size = New System.Drawing.Size(53, 30)
        Me.txtEmpty_2.TabIndex = 458
        Me.txtEmpty_2.Tag = "o_empty_2"
        '
        'txtGas_c_2
        '
        Me.txtGas_c_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtGas_c_2.ForeColor = System.Drawing.Color.Red
        Me.txtGas_c_2.Location = New System.Drawing.Point(762, 42)
        Me.txtGas_c_2.Name = "txtGas_c_2"
        Me.txtGas_c_2.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_c_2.TabIndex = 449
        Me.txtGas_c_2.Tag = "o_gas_c_2"
        '
        'txtGas_2
        '
        Me.txtGas_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtGas_2.Location = New System.Drawing.Point(762, 85)
        Me.txtGas_2.Name = "txtGas_2"
        Me.txtGas_2.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_2.TabIndex = 440
        Me.txtGas_2.Tag = "o_gas_2"
        '
        'txtEmpty_5
        '
        Me.txtEmpty_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtEmpty_5.Location = New System.Drawing.Point(685, 128)
        Me.txtEmpty_5.Name = "txtEmpty_5"
        Me.txtEmpty_5.Size = New System.Drawing.Size(53, 30)
        Me.txtEmpty_5.TabIndex = 457
        Me.txtEmpty_5.Tag = "o_empty_5"
        '
        'txtGas_c_5
        '
        Me.txtGas_c_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtGas_c_5.ForeColor = System.Drawing.Color.Red
        Me.txtGas_c_5.Location = New System.Drawing.Point(685, 42)
        Me.txtGas_c_5.Name = "txtGas_c_5"
        Me.txtGas_c_5.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_c_5.TabIndex = 448
        Me.txtGas_c_5.Tag = "o_gas_c_5"
        '
        'txtGas_5
        '
        Me.txtGas_5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtGas_5.Location = New System.Drawing.Point(685, 85)
        Me.txtGas_5.Name = "txtGas_5"
        Me.txtGas_5.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_5.TabIndex = 439
        Me.txtGas_5.Tag = "o_gas_5"
        '
        'txtEmpty_14
        '
        Me.txtEmpty_14.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtEmpty_14.Location = New System.Drawing.Point(608, 128)
        Me.txtEmpty_14.Name = "txtEmpty_14"
        Me.txtEmpty_14.Size = New System.Drawing.Size(53, 30)
        Me.txtEmpty_14.TabIndex = 456
        Me.txtEmpty_14.Tag = "o_empty_14"
        '
        'txtGas_c_14
        '
        Me.txtGas_c_14.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtGas_c_14.ForeColor = System.Drawing.Color.Red
        Me.txtGas_c_14.Location = New System.Drawing.Point(608, 42)
        Me.txtGas_c_14.Name = "txtGas_c_14"
        Me.txtGas_c_14.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_c_14.TabIndex = 447
        Me.txtGas_c_14.Tag = "o_gas_c_14"
        '
        'txtGas_14
        '
        Me.txtGas_14.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtGas_14.Location = New System.Drawing.Point(608, 85)
        Me.txtGas_14.Name = "txtGas_14"
        Me.txtGas_14.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_14.TabIndex = 438
        Me.txtGas_14.Tag = "o_gas_14"
        '
        'txtEmpty_18
        '
        Me.txtEmpty_18.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtEmpty_18.Location = New System.Drawing.Point(531, 128)
        Me.txtEmpty_18.Name = "txtEmpty_18"
        Me.txtEmpty_18.Size = New System.Drawing.Size(53, 30)
        Me.txtEmpty_18.TabIndex = 455
        Me.txtEmpty_18.Tag = "o_empty_18"
        '
        'txtGas_c_18
        '
        Me.txtGas_c_18.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtGas_c_18.ForeColor = System.Drawing.Color.Red
        Me.txtGas_c_18.Location = New System.Drawing.Point(531, 42)
        Me.txtGas_c_18.Name = "txtGas_c_18"
        Me.txtGas_c_18.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_c_18.TabIndex = 446
        Me.txtGas_c_18.Tag = "o_gas_c_18"
        '
        'txtGas_18
        '
        Me.txtGas_18.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtGas_18.Location = New System.Drawing.Point(531, 85)
        Me.txtGas_18.Name = "txtGas_18"
        Me.txtGas_18.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_18.TabIndex = 437
        Me.txtGas_18.Tag = "o_gas_18"
        '
        'txtEmpty_4
        '
        Me.txtEmpty_4.Location = New System.Drawing.Point(454, 128)
        Me.txtEmpty_4.Name = "txtEmpty_4"
        Me.txtEmpty_4.Size = New System.Drawing.Size(53, 30)
        Me.txtEmpty_4.TabIndex = 34
        Me.txtEmpty_4.Tag = "o_empty_4"
        '
        'txtGas_c_4
        '
        Me.txtGas_c_4.ForeColor = System.Drawing.Color.Red
        Me.txtGas_c_4.Location = New System.Drawing.Point(454, 42)
        Me.txtGas_c_4.Name = "txtGas_c_4"
        Me.txtGas_c_4.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_c_4.TabIndex = 24
        Me.txtGas_c_4.Tag = "o_gas_c_4"
        '
        'txtGas_4
        '
        Me.txtGas_4.Location = New System.Drawing.Point(454, 85)
        Me.txtGas_4.Name = "txtGas_4"
        Me.txtGas_4.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_4.TabIndex = 29
        Me.txtGas_4.Tag = "o_gas_4"
        '
        'txtEmpty_10
        '
        Me.txtEmpty_10.Location = New System.Drawing.Point(377, 128)
        Me.txtEmpty_10.Name = "txtEmpty_10"
        Me.txtEmpty_10.Size = New System.Drawing.Size(53, 30)
        Me.txtEmpty_10.TabIndex = 33
        Me.txtEmpty_10.Tag = "o_empty_10"
        '
        'txtGas_c_10
        '
        Me.txtGas_c_10.ForeColor = System.Drawing.Color.Red
        Me.txtGas_c_10.Location = New System.Drawing.Point(377, 42)
        Me.txtGas_c_10.Name = "txtGas_c_10"
        Me.txtGas_c_10.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_c_10.TabIndex = 23
        Me.txtGas_c_10.Tag = "o_gas_c_10"
        '
        'txtGas_10
        '
        Me.txtGas_10.Location = New System.Drawing.Point(377, 85)
        Me.txtGas_10.Name = "txtGas_10"
        Me.txtGas_10.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_10.TabIndex = 28
        Me.txtGas_10.Tag = "o_gas_10"
        '
        'txtEmpty_16
        '
        Me.txtEmpty_16.Location = New System.Drawing.Point(300, 128)
        Me.txtEmpty_16.Name = "txtEmpty_16"
        Me.txtEmpty_16.Size = New System.Drawing.Size(53, 30)
        Me.txtEmpty_16.TabIndex = 32
        Me.txtEmpty_16.Tag = "o_empty_16"
        '
        'txtGas_c_16
        '
        Me.txtGas_c_16.ForeColor = System.Drawing.Color.Red
        Me.txtGas_c_16.Location = New System.Drawing.Point(300, 42)
        Me.txtGas_c_16.Name = "txtGas_c_16"
        Me.txtGas_c_16.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_c_16.TabIndex = 22
        Me.txtGas_c_16.Tag = "o_gas_c_16"
        '
        'txtGas_16
        '
        Me.txtGas_16.Location = New System.Drawing.Point(300, 85)
        Me.txtGas_16.Name = "txtGas_16"
        Me.txtGas_16.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_16.TabIndex = 27
        Me.txtGas_16.Tag = "o_gas_16"
        '
        'txtEmpty_20
        '
        Me.txtEmpty_20.Location = New System.Drawing.Point(223, 128)
        Me.txtEmpty_20.Name = "txtEmpty_20"
        Me.txtEmpty_20.Size = New System.Drawing.Size(53, 30)
        Me.txtEmpty_20.TabIndex = 31
        Me.txtEmpty_20.Tag = "o_empty_20"
        '
        'txtGas_c_20
        '
        Me.txtGas_c_20.ForeColor = System.Drawing.Color.Red
        Me.txtGas_c_20.Location = New System.Drawing.Point(223, 42)
        Me.txtGas_c_20.Name = "txtGas_c_20"
        Me.txtGas_c_20.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_c_20.TabIndex = 21
        Me.txtGas_c_20.Tag = "o_gas_c_20"
        '
        'txtGas_20
        '
        Me.txtGas_20.Location = New System.Drawing.Point(223, 85)
        Me.txtGas_20.Name = "txtGas_20"
        Me.txtGas_20.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_20.TabIndex = 26
        Me.txtGas_20.Tag = "o_gas_20"
        '
        'txtEmpty_50
        '
        Me.txtEmpty_50.Location = New System.Drawing.Point(146, 128)
        Me.txtEmpty_50.Name = "txtEmpty_50"
        Me.txtEmpty_50.Size = New System.Drawing.Size(53, 30)
        Me.txtEmpty_50.TabIndex = 30
        Me.txtEmpty_50.Tag = "o_empty_50"
        '
        'txtGas_c_50
        '
        Me.txtGas_c_50.ForeColor = System.Drawing.Color.Red
        Me.txtGas_c_50.Location = New System.Drawing.Point(146, 42)
        Me.txtGas_c_50.Name = "txtGas_c_50"
        Me.txtGas_c_50.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_c_50.TabIndex = 20
        Me.txtGas_c_50.Tag = "o_gas_c_50"
        '
        'txtGas_50
        '
        Me.txtGas_50.Location = New System.Drawing.Point(146, 85)
        Me.txtGas_50.Name = "txtGas_50"
        Me.txtGas_50.Size = New System.Drawing.Size(53, 30)
        Me.txtGas_50.TabIndex = 25
        Me.txtGas_50.Tag = "o_gas_50"
        '
        'Label44
        '
        Me.Label44.Location = New System.Drawing.Point(8, 134)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(114, 19)
        Me.Label44.TabIndex = 471
        Me.Label44.Text = "退空瓶"
        '
        'Label45
        '
        Me.Label45.Location = New System.Drawing.Point(8, 91)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(114, 19)
        Me.Label45.TabIndex = 470
        Me.Label45.Text = "正常瓶普"
        '
        'Label56
        '
        Me.Label56.Location = New System.Drawing.Point(8, 48)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(114, 19)
        Me.Label56.TabIndex = 469
        Me.Label56.Text = "正常瓶丙"
        '
        'Label57
        '
        Me.Label57.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label57.Location = New System.Drawing.Point(762, 5)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(53, 19)
        Me.Label57.TabIndex = 468
        Me.Label57.Text = "2KG"
        Me.Label57.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label58
        '
        Me.Label58.Location = New System.Drawing.Point(454, 5)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(53, 19)
        Me.Label58.TabIndex = 467
        Me.Label58.Text = "4KG"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label59
        '
        Me.Label59.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label59.Location = New System.Drawing.Point(685, 5)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(53, 19)
        Me.Label59.TabIndex = 466
        Me.Label59.Text = "5KG"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label78
        '
        Me.Label78.Location = New System.Drawing.Point(377, 5)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(53, 19)
        Me.Label78.TabIndex = 465
        Me.Label78.Text = "10KG"
        Me.Label78.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label103
        '
        Me.Label103.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label103.Location = New System.Drawing.Point(608, 5)
        Me.Label103.Name = "Label103"
        Me.Label103.Size = New System.Drawing.Size(53, 19)
        Me.Label103.TabIndex = 464
        Me.Label103.Text = "14KG"
        Me.Label103.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label105
        '
        Me.Label105.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label105.Location = New System.Drawing.Point(531, 5)
        Me.Label105.Name = "Label105"
        Me.Label105.Size = New System.Drawing.Size(53, 19)
        Me.Label105.TabIndex = 463
        Me.Label105.Text = "18KG"
        Me.Label105.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label125
        '
        Me.Label125.Location = New System.Drawing.Point(300, 5)
        Me.Label125.Name = "Label125"
        Me.Label125.Size = New System.Drawing.Size(53, 19)
        Me.Label125.TabIndex = 462
        Me.Label125.Text = "16KG"
        Me.Label125.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label158
        '
        Me.Label158.Location = New System.Drawing.Point(223, 5)
        Me.Label158.Name = "Label158"
        Me.Label158.Size = New System.Drawing.Size(53, 19)
        Me.Label158.TabIndex = 461
        Me.Label158.Text = "20KG"
        Me.Label158.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label160
        '
        Me.Label160.Location = New System.Drawing.Point(8, 5)
        Me.Label160.Name = "Label160"
        Me.Label160.Size = New System.Drawing.Size(114, 19)
        Me.Label160.TabIndex = 459
        Me.Label160.Text = "容器種類"
        '
        'Label211
        '
        Me.Label211.Location = New System.Drawing.Point(146, 5)
        Me.Label211.Name = "Label211"
        Me.Label211.Size = New System.Drawing.Size(53, 19)
        Me.Label211.TabIndex = 460
        Me.Label211.Text = "50KG"
        Me.Label211.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'grpTransport
        '
        Me.grpTransport.Controls.Add(Me.rdoPickUp)
        Me.grpTransport.Controls.Add(Me.rdoDelivery)
        Me.grpTransport.Location = New System.Drawing.Point(670, 4)
        Me.grpTransport.Name = "grpTransport"
        Me.grpTransport.Size = New System.Drawing.Size(150, 60)
        Me.grpTransport.TabIndex = 508
        Me.grpTransport.TabStop = False
        Me.grpTransport.Tag = "o_delivery_type"
        Me.grpTransport.Text = "運送方式"
        '
        'rdoPickUp
        '
        Me.rdoPickUp.AutoSize = True
        Me.rdoPickUp.Checked = True
        Me.rdoPickUp.Location = New System.Drawing.Point(81, 29)
        Me.rdoPickUp.Name = "rdoPickUp"
        Me.rdoPickUp.Size = New System.Drawing.Size(69, 23)
        Me.rdoPickUp.TabIndex = 1
        Me.rdoPickUp.TabStop = True
        Me.rdoPickUp.Text = "自運"
        Me.rdoPickUp.UseVisualStyleBackColor = True
        '
        'rdoDelivery
        '
        Me.rdoDelivery.AutoSize = True
        Me.rdoDelivery.Location = New System.Drawing.Point(6, 29)
        Me.rdoDelivery.Name = "rdoDelivery"
        Me.rdoDelivery.Size = New System.Drawing.Size(69, 23)
        Me.rdoDelivery.TabIndex = 0
        Me.rdoDelivery.Text = "廠運"
        Me.rdoDelivery.UseVisualStyleBackColor = True
        '
        'txto_return_c
        '
        Me.txto_return_c.Location = New System.Drawing.Point(108, 176)
        Me.txto_return_c.Name = "txto_return_c"
        Me.txto_return_c.Size = New System.Drawing.Size(100, 30)
        Me.txto_return_c.TabIndex = 507
        Me.txto_return_c.Tag = "o_return_c"
        '
        'txtTotalGas_c
        '
        Me.txtTotalGas_c.Location = New System.Drawing.Point(314, 176)
        Me.txtTotalGas_c.Name = "txtTotalGas_c"
        Me.txtTotalGas_c.ReadOnly = True
        Me.txtTotalGas_c.Size = New System.Drawing.Size(100, 30)
        Me.txtTotalGas_c.TabIndex = 505
        Me.txtTotalGas_c.Tag = "o_gas_c_total"
        '
        'txtTotalGas
        '
        Me.txtTotalGas.Location = New System.Drawing.Point(314, 133)
        Me.txtTotalGas.Name = "txtTotalGas"
        Me.txtTotalGas.ReadOnly = True
        Me.txtTotalGas.Size = New System.Drawing.Size(100, 30)
        Me.txtTotalGas.TabIndex = 503
        Me.txtTotalGas.Tag = "o_gas_total"
        '
        'txto_sales_allowance
        '
        Me.txto_sales_allowance.Location = New System.Drawing.Point(726, 133)
        Me.txto_sales_allowance.Name = "txto_sales_allowance"
        Me.txto_sales_allowance.Size = New System.Drawing.Size(100, 30)
        Me.txto_sales_allowance.TabIndex = 500
        Me.txto_sales_allowance.Tag = "o_sales_allowance"
        '
        'txto_return
        '
        Me.txto_return.Location = New System.Drawing.Point(108, 133)
        Me.txto_return.Name = "txto_return"
        Me.txto_return.Size = New System.Drawing.Size(100, 30)
        Me.txto_return.TabIndex = 499
        Me.txto_return.Tag = "o_return"
        '
        'txto_memo
        '
        Me.txto_memo.Location = New System.Drawing.Point(404, 90)
        Me.txto_memo.Name = "txto_memo"
        Me.txto_memo.Size = New System.Drawing.Size(260, 30)
        Me.txto_memo.TabIndex = 496
        Me.txto_memo.Tag = "o_memo"
        '
        'txtCusName
        '
        Me.txtCusName.Location = New System.Drawing.Point(109, 47)
        Me.txtCusName.Name = "txtCusName"
        Me.txtCusName.ReadOnly = True
        Me.txtCusName.Size = New System.Drawing.Size(555, 30)
        Me.txtCusName.TabIndex = 493
        Me.txtCusName.Tag = "cus_name"
        '
        'txtAmount_ord
        '
        Me.txtAmount_ord.Location = New System.Drawing.Point(314, 219)
        Me.txtAmount_ord.Name = "txtAmount_ord"
        Me.txtAmount_ord.ReadOnly = True
        Me.txtAmount_ord.Size = New System.Drawing.Size(100, 30)
        Me.txtAmount_ord.TabIndex = 490
        Me.txtAmount_ord.Tag = "o_total_amount"
        '
        'txtCusID_order
        '
        Me.txtCusID_order.Location = New System.Drawing.Point(1601, 458)
        Me.txtCusID_order.Name = "txtCusID_order"
        Me.txtCusID_order.ReadOnly = True
        Me.txtCusID_order.Size = New System.Drawing.Size(100, 30)
        Me.txtCusID_order.TabIndex = 483
        Me.txtCusID_order.Tag = "o_cus_Id"
        Me.txtCusID_order.Visible = False
        '
        'txto_id
        '
        Me.txto_id.Location = New System.Drawing.Point(1495, 458)
        Me.txto_id.Name = "txto_id"
        Me.txto_id.ReadOnly = True
        Me.txto_id.Size = New System.Drawing.Size(100, 30)
        Me.txto_id.TabIndex = 478
        Me.txto_id.Tag = "o_id"
        Me.txto_id.Visible = False
        '
        'Label204
        '
        Me.Label204.AutoSize = True
        Me.Label204.Location = New System.Drawing.Point(9, 182)
        Me.Label204.Name = "Label204"
        Me.Label204.Size = New System.Drawing.Size(94, 19)
        Me.Label204.TabIndex = 506
        Me.Label204.Text = "退 丙 氣"
        '
        'Label203
        '
        Me.Label203.AutoSize = True
        Me.Label203.Location = New System.Drawing.Point(214, 182)
        Me.Label203.Name = "Label203"
        Me.Label203.Size = New System.Drawing.Size(94, 19)
        Me.Label203.TabIndex = 504
        Me.Label203.Text = "總 丙 氣"
        '
        'Label202
        '
        Me.Label202.AutoSize = True
        Me.Label202.Location = New System.Drawing.Point(214, 139)
        Me.Label202.Name = "Label202"
        Me.Label202.Size = New System.Drawing.Size(94, 19)
        Me.Label202.TabIndex = 502
        Me.Label202.Text = "總 普 氣"
        '
        'btnQueryCus_ord
        '
        Me.btnQueryCus_ord.AutoSize = True
        Me.btnQueryCus_ord.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnQueryCus_ord.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnQueryCus_ord.Location = New System.Drawing.Point(215, 6)
        Me.btnQueryCus_ord.Name = "btnQueryCus_ord"
        Me.btnQueryCus_ord.Size = New System.Drawing.Size(82, 26)
        Me.btnQueryCus_ord.TabIndex = 501
        Me.btnQueryCus_ord.Text = "搜尋客戶"
        Me.btnQueryCus_ord.UseVisualStyleBackColor = False
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.Lime
        Me.btnPrint.Location = New System.Drawing.Point(629, 448)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(140, 44)
        Me.btnPrint.TabIndex = 484
        Me.btnPrint.Text = "列印 (F5)"
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'grpSearch_ord
        '
        Me.grpSearch_ord.Controls.Add(Me.chkOut)
        Me.grpSearch_ord.Controls.Add(Me.chkIn)
        Me.grpSearch_ord.Controls.Add(Me.chkIsDate_ord)
        Me.grpSearch_ord.Controls.Add(Me.Label165)
        Me.grpSearch_ord.Controls.Add(Me.btnQuery_order)
        Me.grpSearch_ord.Controls.Add(Me.dtpEnd_order)
        Me.grpSearch_ord.Controls.Add(Me.Label166)
        Me.grpSearch_ord.Controls.Add(Me.dtpStart_order)
        Me.grpSearch_ord.Location = New System.Drawing.Point(8, 259)
        Me.grpSearch_ord.Name = "grpSearch_ord"
        Me.grpSearch_ord.Size = New System.Drawing.Size(473, 134)
        Me.grpSearch_ord.TabIndex = 498
        Me.grpSearch_ord.TabStop = False
        Me.grpSearch_ord.Text = "搜尋"
        '
        'chkOut
        '
        Me.chkOut.AutoSize = True
        Me.chkOut.Location = New System.Drawing.Point(6, 94)
        Me.chkOut.Name = "chkOut"
        Me.chkOut.Size = New System.Drawing.Size(70, 23)
        Me.chkOut.TabIndex = 348
        Me.chkOut.Text = "出場"
        Me.chkOut.UseVisualStyleBackColor = True
        '
        'chkIn
        '
        Me.chkIn.AutoSize = True
        Me.chkIn.Location = New System.Drawing.Point(6, 65)
        Me.chkIn.Name = "chkIn"
        Me.chkIn.Size = New System.Drawing.Size(70, 23)
        Me.chkIn.TabIndex = 347
        Me.chkIn.Text = "進場"
        Me.chkIn.UseVisualStyleBackColor = True
        '
        'chkIsDate_ord
        '
        Me.chkIsDate_ord.AutoSize = True
        Me.chkIsDate_ord.Location = New System.Drawing.Point(160, 77)
        Me.chkIsDate_ord.Name = "chkIsDate_ord"
        Me.chkIsDate_ord.Size = New System.Drawing.Size(154, 23)
        Me.chkIsDate_ord.TabIndex = 346
        Me.chkIsDate_ord.Text = "使用日期搜尋"
        Me.chkIsDate_ord.UseVisualStyleBackColor = True
        '
        'Label165
        '
        Me.Label165.AutoSize = True
        Me.Label165.Location = New System.Drawing.Point(236, 35)
        Me.Label165.Name = "Label165"
        Me.Label165.Size = New System.Drawing.Size(30, 19)
        Me.Label165.TabIndex = 342
        Me.Label165.Text = "迄"
        '
        'btnQuery_order
        '
        Me.btnQuery_order.BackColor = System.Drawing.Color.Lime
        Me.btnQuery_order.Location = New System.Drawing.Point(320, 65)
        Me.btnQuery_order.Name = "btnQuery_order"
        Me.btnQuery_order.Size = New System.Drawing.Size(140, 44)
        Me.btnQuery_order.TabIndex = 339
        Me.btnQuery_order.Text = "查  詢"
        Me.btnQuery_order.UseVisualStyleBackColor = False
        '
        'dtpEnd_order
        '
        Me.dtpEnd_order.CustomFormat = "yyyy年MM月"
        Me.dtpEnd_order.Location = New System.Drawing.Point(272, 29)
        Me.dtpEnd_order.Name = "dtpEnd_order"
        Me.dtpEnd_order.Size = New System.Drawing.Size(188, 30)
        Me.dtpEnd_order.TabIndex = 343
        '
        'Label166
        '
        Me.Label166.AutoSize = True
        Me.Label166.Location = New System.Drawing.Point(6, 35)
        Me.Label166.Name = "Label166"
        Me.Label166.Size = New System.Drawing.Size(30, 19)
        Me.Label166.TabIndex = 0
        Me.Label166.Text = "起"
        '
        'dtpStart_order
        '
        Me.dtpStart_order.CustomFormat = "yyyy年MM月"
        Me.dtpStart_order.Location = New System.Drawing.Point(42, 29)
        Me.dtpStart_order.Name = "dtpStart_order"
        Me.dtpStart_order.Size = New System.Drawing.Size(188, 30)
        Me.dtpStart_order.TabIndex = 341
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.Location = New System.Drawing.Point(625, 139)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(95, 19)
        Me.Label77.TabIndex = 497
        Me.Label77.Text = "折    讓"
        '
        'Label64
        '
        Me.Label64.AutoSize = True
        Me.Label64.Location = New System.Drawing.Point(303, 96)
        Me.Label64.Name = "Label64"
        Me.Label64.Size = New System.Drawing.Size(95, 19)
        Me.Label64.TabIndex = 495
        Me.Label64.Text = "備    註"
        '
        'Label63
        '
        Me.Label63.AutoSize = True
        Me.Label63.Location = New System.Drawing.Point(8, 139)
        Me.Label63.Name = "Label63"
        Me.Label63.Size = New System.Drawing.Size(94, 19)
        Me.Label63.TabIndex = 494
        Me.Label63.Text = "退 普 氣"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 19)
        Me.Label2.TabIndex = 492
        Me.Label2.Text = "客戶名稱"
        '
        'cmbCar_ord
        '
        Me.cmbCar_ord.BackColor = System.Drawing.SystemColors.Window
        Me.cmbCar_ord.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCar_ord.FormattingEnabled = True
        Me.cmbCar_ord.Location = New System.Drawing.Point(109, 92)
        Me.cmbCar_ord.Name = "cmbCar_ord"
        Me.cmbCar_ord.Size = New System.Drawing.Size(150, 27)
        Me.cmbCar_ord.TabIndex = 491
        Me.cmbCar_ord.Tag = "o_c_id"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(214, 225)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 19)
        Me.Label1.TabIndex = 489
        Me.Label1.Text = "總 金 額"
        '
        'dgvOrder
        '
        Me.dgvOrder.AllowUserToAddRows = False
        Me.dgvOrder.AllowUserToDeleteRows = False
        Me.dgvOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOrder.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvOrder.Location = New System.Drawing.Point(0, 518)
        Me.dgvOrder.Name = "dgvOrder"
        Me.dgvOrder.ReadOnly = True
        Me.dgvOrder.RowTemplate.Height = 24
        Me.dgvOrder.Size = New System.Drawing.Size(1896, 470)
        Me.dgvOrder.TabIndex = 488
        '
        'btnCancel_order
        '
        Me.btnCancel_order.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel_order.Location = New System.Drawing.Point(473, 448)
        Me.btnCancel_order.Name = "btnCancel_order"
        Me.btnCancel_order.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel_order.TabIndex = 487
        Me.btnCancel_order.Text = "取消 (F4)"
        Me.btnCancel_order.UseVisualStyleBackColor = False
        '
        'btnDelete_order
        '
        Me.btnDelete_order.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete_order.Enabled = False
        Me.btnDelete_order.Location = New System.Drawing.Point(317, 448)
        Me.btnDelete_order.Name = "btnDelete_order"
        Me.btnDelete_order.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete_order.TabIndex = 486
        Me.btnDelete_order.Text = "刪除 (F3)"
        Me.btnDelete_order.UseVisualStyleBackColor = False
        '
        'btnCreate_ord
        '
        Me.btnCreate_ord.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCreate_ord.Location = New System.Drawing.Point(5, 448)
        Me.btnCreate_ord.Name = "btnCreate_ord"
        Me.btnCreate_ord.Size = New System.Drawing.Size(140, 44)
        Me.btnCreate_ord.TabIndex = 485
        Me.btnCreate_ord.Tag = ""
        Me.btnCreate_ord.Text = "新增 (F1)"
        Me.btnCreate_ord.UseVisualStyleBackColor = False
        '
        'lblCusCode
        '
        Me.lblCusCode.AutoSize = True
        Me.lblCusCode.Location = New System.Drawing.Point(8, 10)
        Me.lblCusCode.Name = "lblCusCode"
        Me.lblCusCode.Size = New System.Drawing.Size(93, 19)
        Me.lblCusCode.TabIndex = 482
        Me.lblCusCode.Text = "客戶代號"
        '
        'lblCarNo
        '
        Me.lblCarNo.AutoSize = True
        Me.lblCarNo.Location = New System.Drawing.Point(8, 96)
        Me.lblCarNo.Name = "lblCarNo"
        Me.lblCarNo.Size = New System.Drawing.Size(95, 19)
        Me.lblCarNo.TabIndex = 481
        Me.lblCarNo.Text = "車    號"
        '
        'dtpOrder
        '
        Me.dtpOrder.CustomFormat = "yyyy年MM月dd日 HH:mm"
        Me.dtpOrder.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpOrder.Location = New System.Drawing.Point(404, 4)
        Me.dtpOrder.Name = "dtpOrder"
        Me.dtpOrder.Size = New System.Drawing.Size(260, 30)
        Me.dtpOrder.TabIndex = 480
        Me.dtpOrder.Tag = "o_date"
        '
        'Label151
        '
        Me.Label151.AutoSize = True
        Me.Label151.Location = New System.Drawing.Point(303, 10)
        Me.Label151.Name = "Label151"
        Me.Label151.Size = New System.Drawing.Size(95, 19)
        Me.Label151.TabIndex = 479
        Me.Label151.Text = "日    期"
        '
        'OrderUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtUnpaid)
        Me.Controls.Add(Me.Label137)
        Me.Controls.Add(Me.btnCusGetGasList)
        Me.Controls.Add(Me.btnCusGasPayCollect)
        Me.Controls.Add(Me.btnPrintCusStk)
        Me.Controls.Add(Me.txtGasCUnitPrice)
        Me.Controls.Add(Me.Label241)
        Me.Controls.Add(Me.txtGasUnitPrice)
        Me.Controls.Add(Me.Label240)
        Me.Controls.Add(Me.txtBarrelAmount)
        Me.Controls.Add(Me.Label238)
        Me.Controls.Add(Me.btnEdit_ord)
        Me.Controls.Add(Me.txtInsurance)
        Me.Controls.Add(Me.Label153)
        Me.Controls.Add(Me.txtCusCode_ord)
        Me.Controls.Add(Me.txtOperator)
        Me.Controls.Add(Me.Label107)
        Me.Controls.Add(Me.tcInOut)
        Me.Controls.Add(Me.grpTransport)
        Me.Controls.Add(Me.txto_return_c)
        Me.Controls.Add(Me.txtTotalGas_c)
        Me.Controls.Add(Me.txtTotalGas)
        Me.Controls.Add(Me.txto_sales_allowance)
        Me.Controls.Add(Me.txto_return)
        Me.Controls.Add(Me.txto_memo)
        Me.Controls.Add(Me.txtCusName)
        Me.Controls.Add(Me.txtAmount_ord)
        Me.Controls.Add(Me.txtCusID_order)
        Me.Controls.Add(Me.txto_id)
        Me.Controls.Add(Me.Label204)
        Me.Controls.Add(Me.Label203)
        Me.Controls.Add(Me.Label202)
        Me.Controls.Add(Me.btnQueryCus_ord)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.grpSearch_ord)
        Me.Controls.Add(Me.Label77)
        Me.Controls.Add(Me.Label64)
        Me.Controls.Add(Me.Label63)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbCar_ord)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dgvOrder)
        Me.Controls.Add(Me.btnCancel_order)
        Me.Controls.Add(Me.btnDelete_order)
        Me.Controls.Add(Me.btnCreate_ord)
        Me.Controls.Add(Me.lblCusCode)
        Me.Controls.Add(Me.lblCarNo)
        Me.Controls.Add(Me.dtpOrder)
        Me.Controls.Add(Me.Label151)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "OrderUserControl"
        Me.Size = New System.Drawing.Size(1896, 988)
        Me.tcInOut.ResumeLayout(False)
        Me.tpIn.ResumeLayout(False)
        Me.tpIn.PerformLayout()
        Me.tpOut.ResumeLayout(False)
        Me.tpOut.PerformLayout()
        Me.grpTransport.ResumeLayout(False)
        Me.grpTransport.PerformLayout()
        Me.grpSearch_ord.ResumeLayout(False)
        Me.grpSearch_ord.PerformLayout()
        CType(Me.dgvOrder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtUnpaid As TextBox
    Friend WithEvents Label137 As Label
    Friend WithEvents btnCusGetGasList As Button
    Friend WithEvents btnCusGasPayCollect As Button
    Friend WithEvents btnPrintCusStk As Button
    Friend WithEvents txtGasCUnitPrice As TextBox
    Friend WithEvents Label241 As Label
    Friend WithEvents txtGasUnitPrice As TextBox
    Friend WithEvents Label240 As Label
    Friend WithEvents txtBarrelAmount As TextBox
    Friend WithEvents Label238 As Label
    Friend WithEvents btnEdit_ord As Button
    Friend WithEvents txtInsurance As TextBox
    Friend WithEvents Label153 As Label
    Friend WithEvents txtCusCode_ord As TextBox
    Friend WithEvents txtOperator As TextBox
    Friend WithEvents Label107 As Label
    Friend WithEvents tcInOut As TabControl
    Friend WithEvents tpIn As TabPage
    Friend WithEvents txtBarralUnitPrice_2 As TextBox
    Friend WithEvents txtBarralUnitPrice_4 As TextBox
    Friend WithEvents txtBarralUnitPrice_5 As TextBox
    Friend WithEvents txtBarralUnitPrice_10 As TextBox
    Friend WithEvents txtBarralUnitPrice_14 As TextBox
    Friend WithEvents txtBarralUnitPrice_18 As TextBox
    Friend WithEvents txtBarralUnitPrice_16 As TextBox
    Friend WithEvents txtBarralUnitPrice_20 As TextBox
    Friend WithEvents txtBarralUnitPrice_50 As TextBox
    Friend WithEvents Label100 As Label
    Friend WithEvents txtInspect2 As TextBox
    Friend WithEvents txtInspect4 As TextBox
    Friend WithEvents txtInspect5 As TextBox
    Friend WithEvents txtInspect10 As TextBox
    Friend WithEvents txtInspect14 As TextBox
    Friend WithEvents txtInspect18 As TextBox
    Friend WithEvents txtInspect16 As TextBox
    Friend WithEvents txtInspect20 As TextBox
    Friend WithEvents txtInspect50 As TextBox
    Friend WithEvents Label142 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox16 As TextBox
    Friend WithEvents TextBox17 As TextBox
    Friend WithEvents TextBox18 As TextBox
    Friend WithEvents TextBox19 As TextBox
    Friend WithEvents TextBox20 As TextBox
    Friend WithEvents TextBox73 As TextBox
    Friend WithEvents TextBox74 As TextBox
    Friend WithEvents TextBox78 As TextBox
    Friend WithEvents Label206 As Label
    Friend WithEvents txtBarrelIn_2 As TextBox
    Friend WithEvents txtBarrelIn_4 As TextBox
    Friend WithEvents txtBarrelIn_5 As TextBox
    Friend WithEvents txtBarrelIn_10 As TextBox
    Friend WithEvents txtBarrelIn_14 As TextBox
    Friend WithEvents txtBarrelIn_18 As TextBox
    Friend WithEvents txtBarrelIn_16 As TextBox
    Friend WithEvents txtBarrelIn_20 As TextBox
    Friend WithEvents txtBarrelIn_50 As TextBox
    Friend WithEvents Label205 As Label
    Friend WithEvents txtDepositIn_2 As TextBox
    Friend WithEvents txtDepositIn_4 As TextBox
    Friend WithEvents txtDepositIn_5 As TextBox
    Friend WithEvents txtDepositIn_10 As TextBox
    Friend WithEvents txtDepositIn_14 As TextBox
    Friend WithEvents txtDepositIn_18 As TextBox
    Friend WithEvents txtDepositIn_16 As TextBox
    Friend WithEvents txtDepositIn_20 As TextBox
    Friend WithEvents txtDepositIn_50 As TextBox
    Friend WithEvents Label272 As Label
    Friend WithEvents txto_inspect_2 As TextBox
    Friend WithEvents txto_new_in_2 As TextBox
    Friend WithEvents txto_in_2 As TextBox
    Friend WithEvents txto_inspect_4 As TextBox
    Friend WithEvents txto_new_in_4 As TextBox
    Friend WithEvents txto_in_4 As TextBox
    Friend WithEvents txto_inspect_5 As TextBox
    Friend WithEvents txto_new_in_5 As TextBox
    Friend WithEvents txto_in_5 As TextBox
    Friend WithEvents txto_inspect_10 As TextBox
    Friend WithEvents txto_new_in_10 As TextBox
    Friend WithEvents txto_in_10 As TextBox
    Friend WithEvents txto_inspect_14 As TextBox
    Friend WithEvents txto_new_in_14 As TextBox
    Friend WithEvents txto_in_14 As TextBox
    Friend WithEvents txto_inspect_18 As TextBox
    Friend WithEvents txto_new_in_18 As TextBox
    Friend WithEvents txto_in_18 As TextBox
    Friend WithEvents txto_inspect_16 As TextBox
    Friend WithEvents txto_new_in_16 As TextBox
    Friend WithEvents txto_in_16 As TextBox
    Friend WithEvents txto_inspect_20 As TextBox
    Friend WithEvents txto_new_in_20 As TextBox
    Friend WithEvents txto_in_20 As TextBox
    Friend WithEvents txto_inspect_50 As TextBox
    Friend WithEvents txto_new_in_50 As TextBox
    Friend WithEvents txto_in_50 As TextBox
    Friend WithEvents Label212 As Label
    Friend WithEvents Label213 As Label
    Friend WithEvents Label214 As Label
    Friend WithEvents Label215 As Label
    Friend WithEvents Label216 As Label
    Friend WithEvents Label217 As Label
    Friend WithEvents Label264 As Label
    Friend WithEvents Label266 As Label
    Friend WithEvents Label267 As Label
    Friend WithEvents Label268 As Label
    Friend WithEvents Label269 As Label
    Friend WithEvents Label270 As Label
    Friend WithEvents Label271 As Label
    Friend WithEvents tpOut As TabPage
    Friend WithEvents txtCarDeposit_2 As TextBox
    Friend WithEvents txtCarDeposit_5 As TextBox
    Friend WithEvents txtCarDeposit_14 As TextBox
    Friend WithEvents txtCarDeposit_18 As TextBox
    Friend WithEvents txtCarDeposit_4 As TextBox
    Friend WithEvents txtCarDeposit_10 As TextBox
    Friend WithEvents txtCarDeposit_16 As TextBox
    Friend WithEvents txtCarDeposit_20 As TextBox
    Friend WithEvents txtCarDeposit_50 As TextBox
    Friend WithEvents Label277 As Label
    Friend WithEvents Label274 As Label
    Friend WithEvents cmbCarOut_ord As ComboBox
    Friend WithEvents txtCusGas_2 As TextBox
    Friend WithEvents txtCusGas_5 As TextBox
    Friend WithEvents txtCusGas_14 As TextBox
    Friend WithEvents txtCusGas_18 As TextBox
    Friend WithEvents txtCusGas_4 As TextBox
    Friend WithEvents txtCusGas_10 As TextBox
    Friend WithEvents txtCusGas_16 As TextBox
    Friend WithEvents txtCusGas_20 As TextBox
    Friend WithEvents txtCusGas_50 As TextBox
    Friend WithEvents Label273 As Label
    Friend WithEvents txtDepositOut_2 As TextBox
    Friend WithEvents txtDepositOut_5 As TextBox
    Friend WithEvents txtDepositOut_14 As TextBox
    Friend WithEvents txtDepositOut_18 As TextBox
    Friend WithEvents txtDepositOut_4 As TextBox
    Friend WithEvents txtDepositOut_10 As TextBox
    Friend WithEvents txtDepositOut_16 As TextBox
    Friend WithEvents txtDepositOut_20 As TextBox
    Friend WithEvents txtDepositOut_50 As TextBox
    Friend WithEvents Label157 As Label
    Friend WithEvents txtEmpty_2 As TextBox
    Friend WithEvents txtGas_c_2 As TextBox
    Friend WithEvents txtGas_2 As TextBox
    Friend WithEvents txtEmpty_5 As TextBox
    Friend WithEvents txtGas_c_5 As TextBox
    Friend WithEvents txtGas_5 As TextBox
    Friend WithEvents txtEmpty_14 As TextBox
    Friend WithEvents txtGas_c_14 As TextBox
    Friend WithEvents txtGas_14 As TextBox
    Friend WithEvents txtEmpty_18 As TextBox
    Friend WithEvents txtGas_c_18 As TextBox
    Friend WithEvents txtGas_18 As TextBox
    Friend WithEvents txtEmpty_4 As TextBox
    Friend WithEvents txtGas_c_4 As TextBox
    Friend WithEvents txtGas_4 As TextBox
    Friend WithEvents txtEmpty_10 As TextBox
    Friend WithEvents txtGas_c_10 As TextBox
    Friend WithEvents txtGas_10 As TextBox
    Friend WithEvents txtEmpty_16 As TextBox
    Friend WithEvents txtGas_c_16 As TextBox
    Friend WithEvents txtGas_16 As TextBox
    Friend WithEvents txtEmpty_20 As TextBox
    Friend WithEvents txtGas_c_20 As TextBox
    Friend WithEvents txtGas_20 As TextBox
    Friend WithEvents txtEmpty_50 As TextBox
    Friend WithEvents txtGas_c_50 As TextBox
    Friend WithEvents txtGas_50 As TextBox
    Friend WithEvents Label44 As Label
    Friend WithEvents Label45 As Label
    Friend WithEvents Label56 As Label
    Friend WithEvents Label57 As Label
    Friend WithEvents Label58 As Label
    Friend WithEvents Label59 As Label
    Friend WithEvents Label78 As Label
    Friend WithEvents Label103 As Label
    Friend WithEvents Label105 As Label
    Friend WithEvents Label125 As Label
    Friend WithEvents Label158 As Label
    Friend WithEvents Label160 As Label
    Friend WithEvents Label211 As Label
    Friend WithEvents grpTransport As GroupBox
    Friend WithEvents rdoPickUp As RadioButton
    Friend WithEvents rdoDelivery As RadioButton
    Friend WithEvents txto_return_c As TextBox
    Friend WithEvents txtTotalGas_c As TextBox
    Friend WithEvents txtTotalGas As TextBox
    Friend WithEvents txto_sales_allowance As TextBox
    Friend WithEvents txto_return As TextBox
    Friend WithEvents txto_memo As TextBox
    Friend WithEvents txtCusName As TextBox
    Friend WithEvents txtAmount_ord As TextBox
    Friend WithEvents txtCusID_order As TextBox
    Friend WithEvents txto_id As TextBox
    Friend WithEvents Label204 As Label
    Friend WithEvents Label203 As Label
    Friend WithEvents Label202 As Label
    Friend WithEvents btnQueryCus_ord As Button
    Friend WithEvents btnPrint As Button
    Friend WithEvents grpSearch_ord As GroupBox
    Friend WithEvents chkOut As CheckBox
    Friend WithEvents chkIn As CheckBox
    Friend WithEvents chkIsDate_ord As CheckBox
    Friend WithEvents Label165 As Label
    Friend WithEvents btnQuery_order As Button
    Friend WithEvents dtpEnd_order As DateTimePicker
    Friend WithEvents Label166 As Label
    Friend WithEvents dtpStart_order As DateTimePicker
    Friend WithEvents Label77 As Label
    Friend WithEvents Label64 As Label
    Friend WithEvents Label63 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbCar_ord As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents dgvOrder As DataGridView
    Friend WithEvents btnCancel_order As Button
    Friend WithEvents btnDelete_order As Button
    Friend WithEvents btnCreate_ord As Button
    Friend WithEvents lblCusCode As Label
    Friend WithEvents lblCarNo As Label
    Friend WithEvents dtpOrder As DateTimePicker
    Friend WithEvents Label151 As Label
End Class
