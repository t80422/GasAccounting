<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PaymentUserControl
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
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnCancel_payment = New System.Windows.Forms.Button()
        Me.btnDelete_payment = New System.Windows.Forms.Button()
        Me.btnEdit_payment = New System.Windows.Forms.Button()
        Me.btnAdd_payment = New System.Windows.Forms.Button()
        Me.lblPayType_payment = New System.Windows.Forms.Label()
        Me.Label190 = New System.Windows.Forms.Label()
        Me.Label191 = New System.Windows.Forms.Label()
        Me.Label192 = New System.Windows.Forms.Label()
        Me.lblBank = New System.Windows.Forms.Label()
        Me.cmbBank = New System.Windows.Forms.ComboBox()
        Me.dtpAccountMonth = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvCheque = New System.Windows.Forms.DataGridView()
        Me.txtDebitAmount1 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDebitAmount2 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbDebitBank2 = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbDebitCompany2 = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbDebitSubject2 = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtDebitAmount3 = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbDebitBank3 = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cmbDebitCompany3 = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cmbDebitSubject3 = New System.Windows.Forms.ComboBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.grpAmountDue.SuspendLayout()
        CType(Me.dgvAmountDue, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvPayment, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvCheque, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtVendorAccount_payment
        '
        Me.txtVendorAccount_payment.Location = New System.Drawing.Point(494, 105)
        Me.txtVendorAccount_payment.Name = "txtVendorAccount_payment"
        Me.txtVendorAccount_payment.Size = New System.Drawing.Size(458, 30)
        Me.txtVendorAccount_payment.TabIndex = 495
        '
        'btnPrint_pay
        '
        Me.btnPrint_pay.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnPrint_pay.Location = New System.Drawing.Point(669, 316)
        Me.btnPrint_pay.Name = "btnPrint_pay"
        Me.btnPrint_pay.Size = New System.Drawing.Size(140, 44)
        Me.btnPrint_pay.TabIndex = 494
        Me.btnPrint_pay.Text = "列印傳票"
        Me.btnPrint_pay.UseVisualStyleBackColor = False
        '
        'chkCashing
        '
        Me.chkCashing.AutoSize = True
        Me.chkCashing.Location = New System.Drawing.Point(958, 109)
        Me.chkCashing.Name = "chkCashing"
        Me.chkCashing.Size = New System.Drawing.Size(112, 23)
        Me.chkCashing.TabIndex = 493
        Me.chkCashing.Tag = "cp_IsCashing"
        Me.chkCashing.Text = "是否兌付"
        Me.chkCashing.UseVisualStyleBackColor = True
        Me.chkCashing.Visible = False
        '
        'dtpCashing
        '
        Me.dtpCashing.CustomFormat = ""
        Me.dtpCashing.Location = New System.Drawing.Point(180, 151)
        Me.dtpCashing.Name = "dtpCashing"
        Me.dtpCashing.Size = New System.Drawing.Size(180, 30)
        Me.dtpCashing.TabIndex = 492
        Me.dtpCashing.Tag = "cp_CashingDate"
        Me.dtpCashing.Visible = False
        '
        'lblCashingDate_payment
        '
        Me.lblCashingDate_payment.AutoSize = True
        Me.lblCashingDate_payment.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblCashingDate_payment.Location = New System.Drawing.Point(39, 157)
        Me.lblCashingDate_payment.Name = "lblCashingDate_payment"
        Me.lblCashingDate_payment.Size = New System.Drawing.Size(135, 19)
        Me.lblCashingDate_payment.TabIndex = 491
        Me.lblCashingDate_payment.Text = "支票兌現日期"
        Me.lblCashingDate_payment.Visible = False
        '
        'Label144
        '
        Me.Label144.AutoSize = True
        Me.Label144.ForeColor = System.Drawing.Color.Red
        Me.Label144.Location = New System.Drawing.Point(13, 201)
        Me.Label144.Name = "Label144"
        Me.Label144.Size = New System.Drawing.Size(20, 19)
        Me.Label144.TabIndex = 490
        Me.Label144.Text = "*"
        '
        'Label83
        '
        Me.Label83.AutoSize = True
        Me.Label83.ForeColor = System.Drawing.Color.Red
        Me.Label83.Location = New System.Drawing.Point(958, 65)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(20, 19)
        Me.Label83.TabIndex = 489
        Me.Label83.Text = "*"
        '
        'grpAmountDue
        '
        Me.grpAmountDue.Controls.Add(Me.dgvAmountDue)
        Me.grpAmountDue.Location = New System.Drawing.Point(1272, 13)
        Me.grpAmountDue.Name = "grpAmountDue"
        Me.grpAmountDue.Size = New System.Drawing.Size(588, 168)
        Me.grpAmountDue.TabIndex = 488
        Me.grpAmountDue.TabStop = False
        Me.grpAmountDue.Text = "應付未付列表"
        '
        'dgvAmountDue
        '
        Me.dgvAmountDue.AllowUserToAddRows = False
        Me.dgvAmountDue.AllowUserToDeleteRows = False
        Me.dgvAmountDue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAmountDue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAmountDue.Location = New System.Drawing.Point(3, 26)
        Me.dgvAmountDue.Name = "dgvAmountDue"
        Me.dgvAmountDue.ReadOnly = True
        Me.dgvAmountDue.RowTemplate.Height = 24
        Me.dgvAmountDue.Size = New System.Drawing.Size(582, 139)
        Me.dgvAmountDue.TabIndex = 435
        '
        'Label113
        '
        Me.Label113.AutoSize = True
        Me.Label113.ForeColor = System.Drawing.Color.Red
        Me.Label113.Location = New System.Drawing.Point(369, 201)
        Me.Label113.Name = "Label113"
        Me.Label113.Size = New System.Drawing.Size(20, 19)
        Me.Label113.TabIndex = 487
        Me.Label113.Text = "*"
        '
        'cmbCompany_payment
        '
        Me.cmbCompany_payment.FormattingEnabled = True
        Me.cmbCompany_payment.Location = New System.Drawing.Point(494, 197)
        Me.cmbCompany_payment.Name = "cmbCompany_payment"
        Me.cmbCompany_payment.Size = New System.Drawing.Size(458, 27)
        Me.cmbCompany_payment.TabIndex = 486
        Me.cmbCompany_payment.Tag = "p_comp_Id"
        '
        'Label114
        '
        Me.Label114.AutoSize = True
        Me.Label114.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label114.Location = New System.Drawing.Point(395, 201)
        Me.Label114.Name = "Label114"
        Me.Label114.Size = New System.Drawing.Size(95, 19)
        Me.Label114.TabIndex = 485
        Me.Label114.Text = "公    司"
        '
        'lblReq_Chuque
        '
        Me.lblReq_Chuque.AutoSize = True
        Me.lblReq_Chuque.ForeColor = System.Drawing.Color.Red
        Me.lblReq_Chuque.Location = New System.Drawing.Point(13, 111)
        Me.lblReq_Chuque.Name = "lblReq_Chuque"
        Me.lblReq_Chuque.Size = New System.Drawing.Size(20, 19)
        Me.lblReq_Chuque.TabIndex = 484
        Me.lblReq_Chuque.Text = "*"
        Me.lblReq_Chuque.Visible = False
        '
        'Label81
        '
        Me.Label81.AutoSize = True
        Me.Label81.ForeColor = System.Drawing.Color.Red
        Me.Label81.Location = New System.Drawing.Point(958, 19)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(20, 19)
        Me.Label81.TabIndex = 482
        Me.Label81.Text = "*"
        '
        'lblVendorBank_payment
        '
        Me.lblVendorBank_payment.AutoSize = True
        Me.lblVendorBank_payment.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblVendorBank_payment.Location = New System.Drawing.Point(395, 111)
        Me.lblVendorBank_payment.Name = "lblVendorBank_payment"
        Me.lblVendorBank_payment.Size = New System.Drawing.Size(93, 19)
        Me.lblVendorBank_payment.TabIndex = 481
        Me.lblVendorBank_payment.Text = "廠商帳號"
        '
        'cmbManu_payment
        '
        Me.cmbManu_payment.FormattingEnabled = True
        Me.cmbManu_payment.Location = New System.Drawing.Point(494, 15)
        Me.cmbManu_payment.Name = "cmbManu_payment"
        Me.cmbManu_payment.Size = New System.Drawing.Size(458, 27)
        Me.cmbManu_payment.TabIndex = 480
        Me.cmbManu_payment.Tag = "p_m_Id"
        '
        'lblManu_payment
        '
        Me.lblManu_payment.AutoSize = True
        Me.lblManu_payment.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblManu_payment.Location = New System.Drawing.Point(395, 19)
        Me.lblManu_payment.Name = "lblManu_payment"
        Me.lblManu_payment.Size = New System.Drawing.Size(95, 19)
        Me.lblManu_payment.TabIndex = 479
        Me.lblManu_payment.Text = "廠    商"
        '
        'cmbSubjects_payment
        '
        Me.cmbSubjects_payment.FormattingEnabled = True
        Me.cmbSubjects_payment.Location = New System.Drawing.Point(180, 197)
        Me.cmbSubjects_payment.Name = "cmbSubjects_payment"
        Me.cmbSubjects_payment.Size = New System.Drawing.Size(180, 27)
        Me.cmbSubjects_payment.TabIndex = 478
        Me.cmbSubjects_payment.Tag = "p_s_Id"
        '
        'lblSubjects_payment
        '
        Me.lblSubjects_payment.AutoSize = True
        Me.lblSubjects_payment.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblSubjects_payment.Location = New System.Drawing.Point(39, 201)
        Me.lblSubjects_payment.Name = "lblSubjects_payment"
        Me.lblSubjects_payment.Size = New System.Drawing.Size(93, 19)
        Me.lblSubjects_payment.TabIndex = 477
        Me.lblSubjects_payment.Text = "借方科目"
        '
        'lblVendorBankRequired_payment
        '
        Me.lblVendorBankRequired_payment.AutoSize = True
        Me.lblVendorBankRequired_payment.ForeColor = System.Drawing.Color.Red
        Me.lblVendorBankRequired_payment.Location = New System.Drawing.Point(369, 111)
        Me.lblVendorBankRequired_payment.Name = "lblVendorBankRequired_payment"
        Me.lblVendorBankRequired_payment.Size = New System.Drawing.Size(20, 19)
        Me.lblVendorBankRequired_payment.TabIndex = 476
        Me.lblVendorBankRequired_payment.Text = "*"
        '
        'txtCheNo_payment
        '
        Me.txtCheNo_payment.Location = New System.Drawing.Point(180, 105)
        Me.txtCheNo_payment.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCheNo_payment.Name = "txtCheNo_payment"
        Me.txtCheNo_payment.Size = New System.Drawing.Size(180, 30)
        Me.txtCheNo_payment.TabIndex = 474
        Me.txtCheNo_payment.Tag = "cp_Number"
        Me.txtCheNo_payment.Visible = False
        '
        'txtMemo_payment
        '
        Me.txtMemo_payment.Location = New System.Drawing.Point(494, 59)
        Me.txtMemo_payment.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtMemo_payment.Name = "txtMemo_payment"
        Me.txtMemo_payment.Size = New System.Drawing.Size(458, 30)
        Me.txtMemo_payment.TabIndex = 463
        Me.txtMemo_payment.Tag = "p_Memo"
        '
        'txtAmount
        '
        Me.txtAmount.Location = New System.Drawing.Point(1083, 59)
        Me.txtAmount.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(180, 30)
        Me.txtAmount.TabIndex = 460
        Me.txtAmount.Tag = "p_Amount"
        '
        'txtId_payment
        '
        Me.txtId_payment.Location = New System.Drawing.Point(982, 326)
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
        Me.lblCheNo_payment.Location = New System.Drawing.Point(39, 111)
        Me.lblCheNo_payment.Name = "lblCheNo_payment"
        Me.lblCheNo_payment.Size = New System.Drawing.Size(93, 19)
        Me.lblCheNo_payment.TabIndex = 475
        Me.lblCheNo_payment.Text = "支票號碼"
        Me.lblCheNo_payment.Visible = False
        '
        'dtpPayment
        '
        Me.dtpPayment.Location = New System.Drawing.Point(180, 13)
        Me.dtpPayment.Name = "dtpPayment"
        Me.dtpPayment.Size = New System.Drawing.Size(180, 30)
        Me.dtpPayment.TabIndex = 473
        Me.dtpPayment.Tag = "p_Date"
        '
        'cmbPayType
        '
        Me.cmbPayType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbPayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPayType.FormattingEnabled = True
        Me.cmbPayType.Items.AddRange(New Object() {"現金", "銀行存款", "應付票據"})
        Me.cmbPayType.Location = New System.Drawing.Point(1083, 15)
        Me.cmbPayType.Name = "cmbPayType"
        Me.cmbPayType.Size = New System.Drawing.Size(180, 27)
        Me.cmbPayType.TabIndex = 472
        Me.cmbPayType.Tag = "p_Type"
        '
        'dgvPayment
        '
        Me.dgvPayment.AllowUserToAddRows = False
        Me.dgvPayment.AllowUserToDeleteRows = False
        Me.dgvPayment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPayment.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvPayment.Location = New System.Drawing.Point(10, 379)
        Me.dgvPayment.Name = "dgvPayment"
        Me.dgvPayment.ReadOnly = True
        Me.dgvPayment.RowTemplate.Height = 24
        Me.dgvPayment.Size = New System.Drawing.Size(1862, 560)
        Me.dgvPayment.TabIndex = 471
        '
        'btnSearch
        '
        Me.btnSearch.BackColor = System.Drawing.Color.Lime
        Me.btnSearch.Location = New System.Drawing.Point(833, 316)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(140, 44)
        Me.btnSearch.TabIndex = 470
        Me.btnSearch.Text = "查  詢"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'btnCancel_payment
        '
        Me.btnCancel_payment.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel_payment.Location = New System.Drawing.Point(505, 316)
        Me.btnCancel_payment.Name = "btnCancel_payment"
        Me.btnCancel_payment.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel_payment.TabIndex = 469
        Me.btnCancel_payment.Text = "取  消"
        Me.btnCancel_payment.UseVisualStyleBackColor = False
        '
        'btnDelete_payment
        '
        Me.btnDelete_payment.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete_payment.Location = New System.Drawing.Point(341, 316)
        Me.btnDelete_payment.Name = "btnDelete_payment"
        Me.btnDelete_payment.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete_payment.TabIndex = 468
        Me.btnDelete_payment.Text = "刪  除"
        Me.btnDelete_payment.UseVisualStyleBackColor = False
        '
        'btnEdit_payment
        '
        Me.btnEdit_payment.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit_payment.Location = New System.Drawing.Point(177, 316)
        Me.btnEdit_payment.Name = "btnEdit_payment"
        Me.btnEdit_payment.Size = New System.Drawing.Size(140, 44)
        Me.btnEdit_payment.TabIndex = 467
        Me.btnEdit_payment.Text = "修  改"
        Me.btnEdit_payment.UseVisualStyleBackColor = False
        '
        'btnAdd_payment
        '
        Me.btnAdd_payment.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnAdd_payment.Location = New System.Drawing.Point(13, 316)
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
        Me.lblPayType_payment.Location = New System.Drawing.Point(984, 19)
        Me.lblPayType_payment.Name = "lblPayType_payment"
        Me.lblPayType_payment.Size = New System.Drawing.Size(93, 19)
        Me.lblPayType_payment.TabIndex = 465
        Me.lblPayType_payment.Text = "貸方科目"
        '
        'Label190
        '
        Me.Label190.AutoSize = True
        Me.Label190.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label190.Location = New System.Drawing.Point(395, 65)
        Me.Label190.Name = "Label190"
        Me.Label190.Size = New System.Drawing.Size(95, 19)
        Me.Label190.TabIndex = 464
        Me.Label190.Text = "備    註"
        '
        'Label191
        '
        Me.Label191.AutoSize = True
        Me.Label191.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label191.Location = New System.Drawing.Point(39, 19)
        Me.Label191.Name = "Label191"
        Me.Label191.Size = New System.Drawing.Size(95, 19)
        Me.Label191.TabIndex = 462
        Me.Label191.Text = "日    期"
        '
        'Label192
        '
        Me.Label192.AutoSize = True
        Me.Label192.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label192.Location = New System.Drawing.Point(984, 65)
        Me.Label192.Name = "Label192"
        Me.Label192.Size = New System.Drawing.Size(95, 19)
        Me.Label192.TabIndex = 461
        Me.Label192.Text = "金    額"
        '
        'lblBank
        '
        Me.lblBank.AutoSize = True
        Me.lblBank.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblBank.Location = New System.Drawing.Point(986, 201)
        Me.lblBank.Name = "lblBank"
        Me.lblBank.Size = New System.Drawing.Size(93, 19)
        Me.lblBank.TabIndex = 497
        Me.lblBank.Text = "銀行帳號"
        '
        'cmbBank
        '
        Me.cmbBank.FormattingEnabled = True
        Me.cmbBank.Location = New System.Drawing.Point(1094, 197)
        Me.cmbBank.Name = "cmbBank"
        Me.cmbBank.Size = New System.Drawing.Size(458, 27)
        Me.cmbBank.TabIndex = 498
        Me.cmbBank.Tag = "p_bank_Id"
        '
        'dtpAccountMonth
        '
        Me.dtpAccountMonth.CustomFormat = "yyyy年MM月"
        Me.dtpAccountMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpAccountMonth.Location = New System.Drawing.Point(180, 59)
        Me.dtpAccountMonth.Name = "dtpAccountMonth"
        Me.dtpAccountMonth.Size = New System.Drawing.Size(180, 30)
        Me.dtpAccountMonth.TabIndex = 500
        Me.dtpAccountMonth.Tag = "p_AccountMonth"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label1.Location = New System.Drawing.Point(39, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 19)
        Me.Label1.TabIndex = 499
        Me.Label1.Text = "帳款月份"
        '
        'dgvCheque
        '
        Me.dgvCheque.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCheque.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgvCheque.Location = New System.Drawing.Point(1083, 97)
        Me.dgvCheque.Name = "dgvCheque"
        Me.dgvCheque.RowTemplate.Height = 24
        Me.dgvCheque.Size = New System.Drawing.Size(180, 84)
        Me.dgvCheque.TabIndex = 501
        '
        'txtDebitAmount1
        '
        Me.txtDebitAmount1.Location = New System.Drawing.Point(1662, 195)
        Me.txtDebitAmount1.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtDebitAmount1.Name = "txtDebitAmount1"
        Me.txtDebitAmount1.Size = New System.Drawing.Size(180, 30)
        Me.txtDebitAmount1.TabIndex = 504
        Me.txtDebitAmount1.Tag = "p_debit_amount_1"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label3.Location = New System.Drawing.Point(1558, 201)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 19)
        Me.Label3.TabIndex = 505
        Me.Label3.Text = "金    額"
        '
        'txtDebitAmount2
        '
        Me.txtDebitAmount2.Location = New System.Drawing.Point(1662, 238)
        Me.txtDebitAmount2.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtDebitAmount2.Name = "txtDebitAmount2"
        Me.txtDebitAmount2.Size = New System.Drawing.Size(180, 30)
        Me.txtDebitAmount2.TabIndex = 514
        Me.txtDebitAmount2.Tag = "p_debit_amount_2"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label4.Location = New System.Drawing.Point(1558, 244)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(95, 19)
        Me.Label4.TabIndex = 515
        Me.Label4.Text = "金  額 2"
        '
        'cmbDebitBank2
        '
        Me.cmbDebitBank2.FormattingEnabled = True
        Me.cmbDebitBank2.Location = New System.Drawing.Point(1094, 240)
        Me.cmbDebitBank2.Name = "cmbDebitBank2"
        Me.cmbDebitBank2.Size = New System.Drawing.Size(458, 27)
        Me.cmbDebitBank2.TabIndex = 513
        Me.cmbDebitBank2.Tag = "p_debit_bank_id_2"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label5.Location = New System.Drawing.Point(986, 244)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(104, 19)
        Me.Label5.TabIndex = 512
        Me.Label5.Text = "銀行帳號2"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(13, 244)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(20, 19)
        Me.Label6.TabIndex = 511
        Me.Label6.Text = "*"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(369, 244)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(20, 19)
        Me.Label7.TabIndex = 510
        Me.Label7.Text = "*"
        '
        'cmbDebitCompany2
        '
        Me.cmbDebitCompany2.FormattingEnabled = True
        Me.cmbDebitCompany2.Location = New System.Drawing.Point(494, 240)
        Me.cmbDebitCompany2.Name = "cmbDebitCompany2"
        Me.cmbDebitCompany2.Size = New System.Drawing.Size(458, 27)
        Me.cmbDebitCompany2.TabIndex = 509
        Me.cmbDebitCompany2.Tag = "p_debit_comp_id_2"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label8.Location = New System.Drawing.Point(395, 244)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(95, 19)
        Me.Label8.TabIndex = 508
        Me.Label8.Text = "公  司 2"
        '
        'cmbDebitSubject2
        '
        Me.cmbDebitSubject2.FormattingEnabled = True
        Me.cmbDebitSubject2.Location = New System.Drawing.Point(180, 240)
        Me.cmbDebitSubject2.Name = "cmbDebitSubject2"
        Me.cmbDebitSubject2.Size = New System.Drawing.Size(180, 27)
        Me.cmbDebitSubject2.TabIndex = 507
        Me.cmbDebitSubject2.Tag = "p_debit_s_id_2"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label9.Location = New System.Drawing.Point(39, 244)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(104, 19)
        Me.Label9.TabIndex = 506
        Me.Label9.Text = "借方科目2"
        '
        'txtDebitAmount3
        '
        Me.txtDebitAmount3.Location = New System.Drawing.Point(1662, 281)
        Me.txtDebitAmount3.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtDebitAmount3.Name = "txtDebitAmount3"
        Me.txtDebitAmount3.Size = New System.Drawing.Size(180, 30)
        Me.txtDebitAmount3.TabIndex = 524
        Me.txtDebitAmount3.Tag = "p_debit_amount_3"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label10.Location = New System.Drawing.Point(1558, 287)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(95, 19)
        Me.Label10.TabIndex = 525
        Me.Label10.Text = "金  額 3"
        '
        'cmbDebitBank3
        '
        Me.cmbDebitBank3.FormattingEnabled = True
        Me.cmbDebitBank3.Location = New System.Drawing.Point(1094, 283)
        Me.cmbDebitBank3.Name = "cmbDebitBank3"
        Me.cmbDebitBank3.Size = New System.Drawing.Size(458, 27)
        Me.cmbDebitBank3.TabIndex = 523
        Me.cmbDebitBank3.Tag = "p_debit_bank_id_3"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label11.Location = New System.Drawing.Point(986, 287)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(104, 19)
        Me.Label11.TabIndex = 522
        Me.Label11.Text = "銀行帳號3"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Red
        Me.Label12.Location = New System.Drawing.Point(13, 287)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(20, 19)
        Me.Label12.TabIndex = 521
        Me.Label12.Text = "*"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.Color.Red
        Me.Label13.Location = New System.Drawing.Point(369, 287)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(20, 19)
        Me.Label13.TabIndex = 520
        Me.Label13.Text = "*"
        '
        'cmbDebitCompany3
        '
        Me.cmbDebitCompany3.FormattingEnabled = True
        Me.cmbDebitCompany3.Location = New System.Drawing.Point(494, 283)
        Me.cmbDebitCompany3.Name = "cmbDebitCompany3"
        Me.cmbDebitCompany3.Size = New System.Drawing.Size(458, 27)
        Me.cmbDebitCompany3.TabIndex = 519
        Me.cmbDebitCompany3.Tag = "p_debit_comp_id_3"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label14.Location = New System.Drawing.Point(395, 287)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(95, 19)
        Me.Label14.TabIndex = 518
        Me.Label14.Text = "公  司 3"
        '
        'cmbDebitSubject3
        '
        Me.cmbDebitSubject3.FormattingEnabled = True
        Me.cmbDebitSubject3.Location = New System.Drawing.Point(180, 283)
        Me.cmbDebitSubject3.Name = "cmbDebitSubject3"
        Me.cmbDebitSubject3.Size = New System.Drawing.Size(180, 27)
        Me.cmbDebitSubject3.TabIndex = 517
        Me.cmbDebitSubject3.Tag = "p_debit_s_id_3"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label15.Location = New System.Drawing.Point(39, 287)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(104, 19)
        Me.Label15.TabIndex = 516
        Me.Label15.Text = "借方科目3"
        '
        'PaymentUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtDebitAmount3)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.cmbDebitBank3)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.cmbDebitCompany3)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.cmbDebitSubject3)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.txtDebitAmount2)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbDebitBank2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.cmbDebitCompany2)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cmbDebitSubject2)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtDebitAmount1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.dgvCheque)
        Me.Controls.Add(Me.dtpAccountMonth)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbBank)
        Me.Controls.Add(Me.lblBank)
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
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnCancel_payment)
        Me.Controls.Add(Me.btnDelete_payment)
        Me.Controls.Add(Me.btnEdit_payment)
        Me.Controls.Add(Me.btnAdd_payment)
        Me.Controls.Add(Me.lblPayType_payment)
        Me.Controls.Add(Me.Label190)
        Me.Controls.Add(Me.Label191)
        Me.Controls.Add(Me.Label192)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "PaymentUserControl"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.Size = New System.Drawing.Size(1882, 949)
        Me.grpAmountDue.ResumeLayout(False)
        CType(Me.dgvAmountDue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvPayment, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvCheque, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnCancel_payment As Button
    Friend WithEvents btnDelete_payment As Button
    Friend WithEvents btnEdit_payment As Button
    Friend WithEvents btnAdd_payment As Button
    Friend WithEvents lblPayType_payment As Label
    Friend WithEvents Label190 As Label
    Friend WithEvents Label191 As Label
    Friend WithEvents Label192 As Label
    Friend WithEvents lblBank As Label
    Friend WithEvents cmbBank As ComboBox
    Friend WithEvents dtpAccountMonth As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents dgvCheque As DataGridView
    Friend WithEvents txtDebitAmount1 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtDebitAmount2 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents cmbDebitBank2 As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents cmbDebitCompany2 As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cmbDebitSubject2 As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtDebitAmount3 As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents cmbDebitBank3 As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents cmbDebitCompany3 As ComboBox
    Friend WithEvents Label14 As Label
    Friend WithEvents cmbDebitSubject3 As ComboBox
    Friend WithEvents Label15 As Label
End Class
