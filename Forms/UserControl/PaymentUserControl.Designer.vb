<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PaymentUserControl
    Inherits System.Windows.Forms.UserControl

    'UserControl 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtVendorAccount_payment = New System.Windows.Forms.TextBox()
        Me.btnPrint_pay = New System.Windows.Forms.Button()
        Me.chkCashing = New System.Windows.Forms.CheckBox()
        Me.dtpCashing = New System.Windows.Forms.DateTimePicker()
        Me.lblCashingDate_payment = New System.Windows.Forms.Label()
        Me.Label144 = New System.Windows.Forms.Label()
        Me.Label83 = New System.Windows.Forms.Label()
        Me.grpAmountDue = New System.Windows.Forms.GroupBox()
        Me.dgvAmountDue = New System.Windows.Forms.DataGridView()
        Me.Label113 = New System.Windows.Forms.Label()
        Me.cmbCompany_payment = New System.Windows.Forms.ComboBox()
        Me.Label114 = New System.Windows.Forms.Label()
        Me.lblReq_Chuque = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkIsSearchDate_payment = New System.Windows.Forms.CheckBox()
        Me.dtpEnd_payment = New System.Windows.Forms.DateTimePicker()
        Me.dtpStart_payment = New System.Windows.Forms.DateTimePicker()
        Me.Label81 = New System.Windows.Forms.Label()
        Me.lblVendorBank_payment = New System.Windows.Forms.Label()
        Me.cmbManu_payment = New System.Windows.Forms.ComboBox()
        Me.lblManu_payment = New System.Windows.Forms.Label()
        Me.cmbSubjects_payment = New System.Windows.Forms.ComboBox()
        Me.lblSubjects_payment = New System.Windows.Forms.Label()
        Me.lblVendorBankRequired_payment = New System.Windows.Forms.Label()
        Me.txtCheNo_payment = New System.Windows.Forms.TextBox()
        Me.txtMemo_payment = New System.Windows.Forms.TextBox()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.txtId_payment = New System.Windows.Forms.TextBox()
        Me.lblCheNo_payment = New System.Windows.Forms.Label()
        Me.dtpPayment = New System.Windows.Forms.DateTimePicker()
        Me.cmbPayType = New System.Windows.Forms.ComboBox()
        Me.dgvPayment = New System.Windows.Forms.DataGridView()
        Me.btnQuery_payment = New System.Windows.Forms.Button()
        Me.btnCancel_payment = New System.Windows.Forms.Button()
        Me.btnDelete_payment = New System.Windows.Forms.Button()
        Me.btnEdit_payment = New System.Windows.Forms.Button()
        Me.btnAdd_payment = New System.Windows.Forms.Button()
        Me.lblPayType_payment = New System.Windows.Forms.Label()
        Me.Label190 = New System.Windows.Forms.Label()
        Me.Label191 = New System.Windows.Forms.Label()
        Me.Label192 = New System.Windows.Forms.Label()
        Me.lblBank = New System.Windows.Forms.Label()
        Me.lblBankReq = New System.Windows.Forms.Label()
        Me.cmbBank = New System.Windows.Forms.ComboBox()
        Me.grpAmountDue.SuspendLayout()
        CType(Me.dgvAmountDue, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgvPayment, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtVendorAccount_payment
        '
        Me.txtVendorAccount_payment.Location = New System.Drawing.Point(464, 133)
        Me.txtVendorAccount_payment.Name = "txtVendorAccount_payment"
        Me.txtVendorAccount_payment.Size = New System.Drawing.Size(458, 30)
        Me.txtVendorAccount_payment.TabIndex = 495
        '
        'btnPrint_pay
        '
        Me.btnPrint_pay.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnPrint_pay.Location = New System.Drawing.Point(632, 232)
        Me.btnPrint_pay.Name = "btnPrint_pay"
        Me.btnPrint_pay.Size = New System.Drawing.Size(140, 44)
        Me.btnPrint_pay.TabIndex = 494
        Me.btnPrint_pay.Text = "列印傳票"
        Me.btnPrint_pay.UseVisualStyleBackColor = False
        '
        'chkCashing
        '
        Me.chkCashing.AutoSize = True
        Me.chkCashing.Location = New System.Drawing.Point(38, 178)
        Me.chkCashing.Name = "chkCashing"
        Me.chkCashing.Size = New System.Drawing.Size(112, 23)
        Me.chkCashing.TabIndex = 493
        Me.chkCashing.Tag = "p_IsCashing"
        Me.chkCashing.Text = "是否兌付"
        Me.chkCashing.UseVisualStyleBackColor = True
        Me.chkCashing.Visible = False
        '
        'dtpCashing
        '
        Me.dtpCashing.CustomFormat = ""
        Me.dtpCashing.Location = New System.Drawing.Point(137, 133)
        Me.dtpCashing.Name = "dtpCashing"
        Me.dtpCashing.Size = New System.Drawing.Size(190, 30)
        Me.dtpCashing.TabIndex = 492
        Me.dtpCashing.Tag = "p_CashingDate"
        Me.dtpCashing.Visible = False
        '
        'lblCashingDate_payment
        '
        Me.lblCashingDate_payment.AutoSize = True
        Me.lblCashingDate_payment.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblCashingDate_payment.Location = New System.Drawing.Point(34, 139)
        Me.lblCashingDate_payment.Name = "lblCashingDate_payment"
        Me.lblCashingDate_payment.Size = New System.Drawing.Size(93, 19)
        Me.lblCashingDate_payment.TabIndex = 491
        Me.lblCashingDate_payment.Text = "兌現日期"
        Me.lblCashingDate_payment.Visible = False
        '
        'Label144
        '
        Me.Label144.AutoSize = True
        Me.Label144.ForeColor = System.Drawing.Color.Red
        Me.Label144.Location = New System.Drawing.Point(928, 10)
        Me.Label144.Name = "Label144"
        Me.Label144.Size = New System.Drawing.Size(20, 19)
        Me.Label144.TabIndex = 490
        Me.Label144.Text = "*"
        '
        'Label83
        '
        Me.Label83.AutoSize = True
        Me.Label83.ForeColor = System.Drawing.Color.Red
        Me.Label83.Location = New System.Drawing.Point(928, 53)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(20, 19)
        Me.Label83.TabIndex = 489
        Me.Label83.Text = "*"
        '
        'grpAmountDue
        '
        Me.grpAmountDue.Controls.Add(Me.dgvAmountDue)
        Me.grpAmountDue.Location = New System.Drawing.Point(1245, 4)
        Me.grpAmountDue.Name = "grpAmountDue"
        Me.grpAmountDue.Size = New System.Drawing.Size(630, 222)
        Me.grpAmountDue.TabIndex = 488
        Me.grpAmountDue.TabStop = False
        Me.grpAmountDue.Text = "應付未付列表"
        '
        'dgvAmountDue
        '
        Me.dgvAmountDue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAmountDue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAmountDue.Location = New System.Drawing.Point(3, 26)
        Me.dgvAmountDue.Name = "dgvAmountDue"
        Me.dgvAmountDue.ReadOnly = True
        Me.dgvAmountDue.RowTemplate.Height = 24
        Me.dgvAmountDue.Size = New System.Drawing.Size(624, 193)
        Me.dgvAmountDue.TabIndex = 435
        '
        'Label113
        '
        Me.Label113.AutoSize = True
        Me.Label113.ForeColor = System.Drawing.Color.Red
        Me.Label113.Location = New System.Drawing.Point(333, 53)
        Me.Label113.Name = "Label113"
        Me.Label113.Size = New System.Drawing.Size(20, 19)
        Me.Label113.TabIndex = 487
        Me.Label113.Text = "*"
        '
        'cmbCompany_payment
        '
        Me.cmbCompany_payment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCompany_payment.FormattingEnabled = True
        Me.cmbCompany_payment.Location = New System.Drawing.Point(464, 49)
        Me.cmbCompany_payment.Name = "cmbCompany_payment"
        Me.cmbCompany_payment.Size = New System.Drawing.Size(458, 27)
        Me.cmbCompany_payment.TabIndex = 486
        Me.cmbCompany_payment.Tag = "p_comp_Id"
        '
        'Label114
        '
        Me.Label114.AutoSize = True
        Me.Label114.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label114.Location = New System.Drawing.Point(361, 53)
        Me.Label114.Name = "Label114"
        Me.Label114.Size = New System.Drawing.Size(95, 19)
        Me.Label114.TabIndex = 485
        Me.Label114.Text = "公    司"
        '
        'lblReq_Chuque
        '
        Me.lblReq_Chuque.AutoSize = True
        Me.lblReq_Chuque.ForeColor = System.Drawing.Color.Red
        Me.lblReq_Chuque.Location = New System.Drawing.Point(8, 96)
        Me.lblReq_Chuque.Name = "lblReq_Chuque"
        Me.lblReq_Chuque.Size = New System.Drawing.Size(20, 19)
        Me.lblReq_Chuque.TabIndex = 484
        Me.lblReq_Chuque.Text = "*"
        Me.lblReq_Chuque.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkIsSearchDate_payment)
        Me.GroupBox1.Controls.Add(Me.dtpEnd_payment)
        Me.GroupBox1.Controls.Add(Me.dtpStart_payment)
        Me.GroupBox1.Location = New System.Drawing.Point(933, 85)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(5)
        Me.GroupBox1.Size = New System.Drawing.Size(306, 141)
        Me.GroupBox1.TabIndex = 483
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "查詢日期起迄"
        '
        'chkIsSearchDate_payment
        '
        Me.chkIsSearchDate_payment.AutoSize = True
        Me.chkIsSearchDate_payment.Location = New System.Drawing.Point(8, 103)
        Me.chkIsSearchDate_payment.Name = "chkIsSearchDate_payment"
        Me.chkIsSearchDate_payment.Size = New System.Drawing.Size(154, 23)
        Me.chkIsSearchDate_payment.TabIndex = 412
        Me.chkIsSearchDate_payment.Text = "使用日期查詢"
        Me.chkIsSearchDate_payment.UseVisualStyleBackColor = True
        '
        'dtpEnd_payment
        '
        Me.dtpEnd_payment.Location = New System.Drawing.Point(8, 67)
        Me.dtpEnd_payment.Name = "dtpEnd_payment"
        Me.dtpEnd_payment.Size = New System.Drawing.Size(180, 30)
        Me.dtpEnd_payment.TabIndex = 410
        Me.dtpEnd_payment.Tag = "p_Date"
        '
        'dtpStart_payment
        '
        Me.dtpStart_payment.Location = New System.Drawing.Point(8, 31)
        Me.dtpStart_payment.Name = "dtpStart_payment"
        Me.dtpStart_payment.Size = New System.Drawing.Size(180, 30)
        Me.dtpStart_payment.TabIndex = 409
        Me.dtpStart_payment.Tag = "p_Date"
        '
        'Label81
        '
        Me.Label81.AutoSize = True
        Me.Label81.ForeColor = System.Drawing.Color.Red
        Me.Label81.Location = New System.Drawing.Point(8, 53)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(20, 19)
        Me.Label81.TabIndex = 482
        Me.Label81.Text = "*"
        '
        'lblVendorBank_payment
        '
        Me.lblVendorBank_payment.AutoSize = True
        Me.lblVendorBank_payment.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblVendorBank_payment.Location = New System.Drawing.Point(361, 139)
        Me.lblVendorBank_payment.Name = "lblVendorBank_payment"
        Me.lblVendorBank_payment.Size = New System.Drawing.Size(93, 19)
        Me.lblVendorBank_payment.TabIndex = 481
        Me.lblVendorBank_payment.Text = "廠商帳號"
        '
        'cmbManu_payment
        '
        Me.cmbManu_payment.FormattingEnabled = True
        Me.cmbManu_payment.Location = New System.Drawing.Point(464, 6)
        Me.cmbManu_payment.Name = "cmbManu_payment"
        Me.cmbManu_payment.Size = New System.Drawing.Size(458, 27)
        Me.cmbManu_payment.TabIndex = 480
        Me.cmbManu_payment.Tag = "p_m_Id"
        '
        'lblManu_payment
        '
        Me.lblManu_payment.AutoSize = True
        Me.lblManu_payment.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblManu_payment.Location = New System.Drawing.Point(361, 10)
        Me.lblManu_payment.Name = "lblManu_payment"
        Me.lblManu_payment.Size = New System.Drawing.Size(95, 19)
        Me.lblManu_payment.TabIndex = 479
        Me.lblManu_payment.Text = "廠    商"
        '
        'cmbSubjects_payment
        '
        Me.cmbSubjects_payment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubjects_payment.FormattingEnabled = True
        Me.cmbSubjects_payment.Location = New System.Drawing.Point(1055, 6)
        Me.cmbSubjects_payment.Name = "cmbSubjects_payment"
        Me.cmbSubjects_payment.Size = New System.Drawing.Size(184, 27)
        Me.cmbSubjects_payment.TabIndex = 478
        Me.cmbSubjects_payment.Tag = "p_s_Id"
        '
        'lblSubjects_payment
        '
        Me.lblSubjects_payment.AutoSize = True
        Me.lblSubjects_payment.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblSubjects_payment.Location = New System.Drawing.Point(954, 10)
        Me.lblSubjects_payment.Name = "lblSubjects_payment"
        Me.lblSubjects_payment.Size = New System.Drawing.Size(93, 19)
        Me.lblSubjects_payment.TabIndex = 477
        Me.lblSubjects_payment.Text = "借方科目"
        '
        'lblVendorBankRequired_payment
        '
        Me.lblVendorBankRequired_payment.AutoSize = True
        Me.lblVendorBankRequired_payment.ForeColor = System.Drawing.Color.Red
        Me.lblVendorBankRequired_payment.Location = New System.Drawing.Point(333, 139)
        Me.lblVendorBankRequired_payment.Name = "lblVendorBankRequired_payment"
        Me.lblVendorBankRequired_payment.Size = New System.Drawing.Size(20, 19)
        Me.lblVendorBankRequired_payment.TabIndex = 476
        Me.lblVendorBankRequired_payment.Text = "*"
        '
        'txtCheNo_payment
        '
        Me.txtCheNo_payment.Location = New System.Drawing.Point(137, 90)
        Me.txtCheNo_payment.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCheNo_payment.Name = "txtCheNo_payment"
        Me.txtCheNo_payment.Size = New System.Drawing.Size(190, 30)
        Me.txtCheNo_payment.TabIndex = 474
        Me.txtCheNo_payment.Tag = "p_Cheque"
        Me.txtCheNo_payment.Visible = False
        '
        'txtMemo_payment
        '
        Me.txtMemo_payment.Location = New System.Drawing.Point(464, 90)
        Me.txtMemo_payment.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtMemo_payment.Name = "txtMemo_payment"
        Me.txtMemo_payment.Size = New System.Drawing.Size(458, 30)
        Me.txtMemo_payment.TabIndex = 463
        Me.txtMemo_payment.Tag = "p_Memo"
        '
        'txtAmount
        '
        Me.txtAmount.Location = New System.Drawing.Point(1055, 47)
        Me.txtAmount.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(184, 30)
        Me.txtAmount.TabIndex = 460
        Me.txtAmount.Tag = "p_Amount"
        '
        'txtId_payment
        '
        Me.txtId_payment.Location = New System.Drawing.Point(937, 246)
        Me.txtId_payment.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtId_payment.Name = "txtId_payment"
        Me.txtId_payment.ReadOnly = True
        Me.txtId_payment.Size = New System.Drawing.Size(145, 30)
        Me.txtId_payment.TabIndex = 459
        Me.txtId_payment.Tag = "p_Id"
        Me.txtId_payment.Visible = False
        '
        'lblCheNo_payment
        '
        Me.lblCheNo_payment.AutoSize = True
        Me.lblCheNo_payment.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblCheNo_payment.Location = New System.Drawing.Point(34, 96)
        Me.lblCheNo_payment.Name = "lblCheNo_payment"
        Me.lblCheNo_payment.Size = New System.Drawing.Size(93, 19)
        Me.lblCheNo_payment.TabIndex = 475
        Me.lblCheNo_payment.Text = "支票號碼"
        Me.lblCheNo_payment.Visible = False
        '
        'dtpPayment
        '
        Me.dtpPayment.Location = New System.Drawing.Point(137, 4)
        Me.dtpPayment.Name = "dtpPayment"
        Me.dtpPayment.Size = New System.Drawing.Size(190, 30)
        Me.dtpPayment.TabIndex = 473
        Me.dtpPayment.Tag = "p_Date"
        '
        'cmbPayType
        '
        Me.cmbPayType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbPayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPayType.FormattingEnabled = True
        Me.cmbPayType.Items.AddRange(New Object() {"現金", "銀行存款", "應付票據"})
        Me.cmbPayType.Location = New System.Drawing.Point(137, 49)
        Me.cmbPayType.Name = "cmbPayType"
        Me.cmbPayType.Size = New System.Drawing.Size(190, 27)
        Me.cmbPayType.TabIndex = 472
        Me.cmbPayType.Tag = "p_Type"
        '
        'dgvPayment
        '
        Me.dgvPayment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPayment.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvPayment.Location = New System.Drawing.Point(0, 309)
        Me.dgvPayment.Name = "dgvPayment"
        Me.dgvPayment.ReadOnly = True
        Me.dgvPayment.RowTemplate.Height = 24
        Me.dgvPayment.Size = New System.Drawing.Size(1882, 640)
        Me.dgvPayment.TabIndex = 471
        '
        'btnQuery_payment
        '
        Me.btnQuery_payment.BackColor = System.Drawing.Color.Lime
        Me.btnQuery_payment.Location = New System.Drawing.Point(788, 232)
        Me.btnQuery_payment.Name = "btnQuery_payment"
        Me.btnQuery_payment.Size = New System.Drawing.Size(140, 44)
        Me.btnQuery_payment.TabIndex = 470
        Me.btnQuery_payment.Text = "查  詢"
        Me.btnQuery_payment.UseVisualStyleBackColor = False
        '
        'btnCancel_payment
        '
        Me.btnCancel_payment.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel_payment.Location = New System.Drawing.Point(476, 232)
        Me.btnCancel_payment.Name = "btnCancel_payment"
        Me.btnCancel_payment.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel_payment.TabIndex = 469
        Me.btnCancel_payment.Text = "取  消"
        Me.btnCancel_payment.UseVisualStyleBackColor = False
        '
        'btnDelete_payment
        '
        Me.btnDelete_payment.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete_payment.Location = New System.Drawing.Point(320, 232)
        Me.btnDelete_payment.Name = "btnDelete_payment"
        Me.btnDelete_payment.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete_payment.TabIndex = 468
        Me.btnDelete_payment.Text = "刪  除"
        Me.btnDelete_payment.UseVisualStyleBackColor = False
        '
        'btnEdit_payment
        '
        Me.btnEdit_payment.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit_payment.Location = New System.Drawing.Point(164, 232)
        Me.btnEdit_payment.Name = "btnEdit_payment"
        Me.btnEdit_payment.Size = New System.Drawing.Size(140, 44)
        Me.btnEdit_payment.TabIndex = 467
        Me.btnEdit_payment.Text = "修  改"
        Me.btnEdit_payment.UseVisualStyleBackColor = False
        '
        'btnAdd_payment
        '
        Me.btnAdd_payment.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnAdd_payment.Location = New System.Drawing.Point(8, 232)
        Me.btnAdd_payment.Name = "btnAdd_payment"
        Me.btnAdd_payment.Size = New System.Drawing.Size(140, 44)
        Me.btnAdd_payment.TabIndex = 466
        Me.btnAdd_payment.Tag = ""
        Me.btnAdd_payment.Text = "新  增"
        Me.btnAdd_payment.UseVisualStyleBackColor = False
        '
        'lblPayType_payment
        '
        Me.lblPayType_payment.AutoSize = True
        Me.lblPayType_payment.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblPayType_payment.Location = New System.Drawing.Point(34, 53)
        Me.lblPayType_payment.Name = "lblPayType_payment"
        Me.lblPayType_payment.Size = New System.Drawing.Size(93, 19)
        Me.lblPayType_payment.TabIndex = 465
        Me.lblPayType_payment.Text = "貸方科目"
        '
        'Label190
        '
        Me.Label190.AutoSize = True
        Me.Label190.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label190.Location = New System.Drawing.Point(361, 96)
        Me.Label190.Name = "Label190"
        Me.Label190.Size = New System.Drawing.Size(95, 19)
        Me.Label190.TabIndex = 464
        Me.Label190.Text = "備    註"
        '
        'Label191
        '
        Me.Label191.AutoSize = True
        Me.Label191.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label191.Location = New System.Drawing.Point(34, 10)
        Me.Label191.Name = "Label191"
        Me.Label191.Size = New System.Drawing.Size(95, 19)
        Me.Label191.TabIndex = 462
        Me.Label191.Text = "日    期"
        '
        'Label192
        '
        Me.Label192.AutoSize = True
        Me.Label192.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label192.Location = New System.Drawing.Point(954, 53)
        Me.Label192.Name = "Label192"
        Me.Label192.Size = New System.Drawing.Size(95, 19)
        Me.Label192.TabIndex = 461
        Me.Label192.Text = "金    額"
        '
        'lblBank
        '
        Me.lblBank.AutoSize = True
        Me.lblBank.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblBank.Location = New System.Drawing.Point(361, 185)
        Me.lblBank.Name = "lblBank"
        Me.lblBank.Size = New System.Drawing.Size(93, 19)
        Me.lblBank.TabIndex = 497
        Me.lblBank.Text = "銀行帳號"
        '
        'lblBankReq
        '
        Me.lblBankReq.AutoSize = True
        Me.lblBankReq.ForeColor = System.Drawing.Color.Red
        Me.lblBankReq.Location = New System.Drawing.Point(333, 185)
        Me.lblBankReq.Name = "lblBankReq"
        Me.lblBankReq.Size = New System.Drawing.Size(20, 19)
        Me.lblBankReq.TabIndex = 496
        Me.lblBankReq.Text = "*"
        '
        'cmbBank
        '
        Me.cmbBank.FormattingEnabled = True
        Me.cmbBank.Location = New System.Drawing.Point(464, 181)
        Me.cmbBank.Name = "cmbBank"
        Me.cmbBank.Size = New System.Drawing.Size(458, 27)
        Me.cmbBank.TabIndex = 498
        Me.cmbBank.Tag = "p_bank_Id"
        '
        'PaymentUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cmbBank)
        Me.Controls.Add(Me.lblBank)
        Me.Controls.Add(Me.lblBankReq)
        Me.Controls.Add(Me.txtVendorAccount_payment)
        Me.Controls.Add(Me.btnPrint_pay)
        Me.Controls.Add(Me.chkCashing)
        Me.Controls.Add(Me.dtpCashing)
        Me.Controls.Add(Me.lblCashingDate_payment)
        Me.Controls.Add(Me.Label144)
        Me.Controls.Add(Me.Label83)
        Me.Controls.Add(Me.grpAmountDue)
        Me.Controls.Add(Me.Label113)
        Me.Controls.Add(Me.cmbCompany_payment)
        Me.Controls.Add(Me.Label114)
        Me.Controls.Add(Me.lblReq_Chuque)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label81)
        Me.Controls.Add(Me.lblVendorBank_payment)
        Me.Controls.Add(Me.cmbManu_payment)
        Me.Controls.Add(Me.lblManu_payment)
        Me.Controls.Add(Me.cmbSubjects_payment)
        Me.Controls.Add(Me.lblSubjects_payment)
        Me.Controls.Add(Me.lblVendorBankRequired_payment)
        Me.Controls.Add(Me.txtCheNo_payment)
        Me.Controls.Add(Me.txtMemo_payment)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.txtId_payment)
        Me.Controls.Add(Me.lblCheNo_payment)
        Me.Controls.Add(Me.dtpPayment)
        Me.Controls.Add(Me.cmbPayType)
        Me.Controls.Add(Me.dgvPayment)
        Me.Controls.Add(Me.btnQuery_payment)
        Me.Controls.Add(Me.btnCancel_payment)
        Me.Controls.Add(Me.btnDelete_payment)
        Me.Controls.Add(Me.btnEdit_payment)
        Me.Controls.Add(Me.btnAdd_payment)
        Me.Controls.Add(Me.lblPayType_payment)
        Me.Controls.Add(Me.Label190)
        Me.Controls.Add(Me.Label191)
        Me.Controls.Add(Me.Label192)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "PaymentUserControl"
        Me.Size = New System.Drawing.Size(1882, 949)
        Me.grpAmountDue.ResumeLayout(False)
        CType(Me.dgvAmountDue, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.dgvPayment, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtVendorAccount_payment As TextBox
    Friend WithEvents btnPrint_pay As Button
    Friend WithEvents chkCashing As CheckBox
    Friend WithEvents dtpCashing As DateTimePicker
    Friend WithEvents lblCashingDate_payment As Label
    Friend WithEvents Label144 As Label
    Friend WithEvents Label83 As Label
    Friend WithEvents grpAmountDue As GroupBox
    Friend WithEvents dgvAmountDue As DataGridView
    Friend WithEvents Label113 As Label
    Friend WithEvents cmbCompany_payment As ComboBox
    Friend WithEvents Label114 As Label
    Friend WithEvents lblReq_Chuque As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents chkIsSearchDate_payment As CheckBox
    Friend WithEvents dtpEnd_payment As DateTimePicker
    Friend WithEvents dtpStart_payment As DateTimePicker
    Friend WithEvents Label81 As Label
    Friend WithEvents lblVendorBank_payment As Label
    Friend WithEvents cmbManu_payment As ComboBox
    Friend WithEvents lblManu_payment As Label
    Friend WithEvents cmbSubjects_payment As ComboBox
    Friend WithEvents lblSubjects_payment As Label
    Friend WithEvents lblVendorBankRequired_payment As Label
    Friend WithEvents txtCheNo_payment As TextBox
    Friend WithEvents txtMemo_payment As TextBox
    Friend WithEvents txtAmount As TextBox
    Friend WithEvents txtId_payment As TextBox
    Friend WithEvents lblCheNo_payment As Label
    Friend WithEvents dtpPayment As DateTimePicker
    Friend WithEvents cmbPayType As ComboBox
    Friend WithEvents dgvPayment As DataGridView
    Friend WithEvents btnQuery_payment As Button
    Friend WithEvents btnCancel_payment As Button
    Friend WithEvents btnDelete_payment As Button
    Friend WithEvents btnEdit_payment As Button
    Friend WithEvents btnAdd_payment As Button
    Friend WithEvents lblPayType_payment As Label
    Friend WithEvents Label190 As Label
    Friend WithEvents Label191 As Label
    Friend WithEvents Label192 As Label
    Friend WithEvents lblBank As Label
    Friend WithEvents lblBankReq As Label
    Friend WithEvents cmbBank As ComboBox
End Class
