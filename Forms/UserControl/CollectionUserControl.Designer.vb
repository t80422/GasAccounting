<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CollectionUserControl
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
        Me.btnPrint_Col = New System.Windows.Forms.Button()
        Me.TextBox14 = New System.Windows.Forms.TextBox()
        Me.Label141 = New System.Windows.Forms.Label()
        Me.btnWriteOff = New System.Windows.Forms.Button()
        Me.btnQuery_col = New System.Windows.Forms.Button()
        Me.dtpAbleCashingDate = New System.Windows.Forms.DateTimePicker()
        Me.lblAbleCashingDate = New System.Windows.Forms.Label()
        Me.txtPayBank = New System.Windows.Forms.TextBox()
        Me.lblPayBank = New System.Windows.Forms.Label()
        Me.txtCashingDate = New System.Windows.Forms.TextBox()
        Me.txtCusCode_col = New System.Windows.Forms.TextBox()
        Me.txtCheAcctNum = New System.Windows.Forms.TextBox()
        Me.lblChequeAccountNumberReq = New System.Windows.Forms.Label()
        Me.lblChequeAccountNumber = New System.Windows.Forms.Label()
        Me.txtIssuerName = New System.Windows.Forms.TextBox()
        Me.lblIssuerNameReq = New System.Windows.Forms.Label()
        Me.lblIssuerName = New System.Windows.Forms.Label()
        Me.lblCashingDate_col = New System.Windows.Forms.Label()
        Me.Label115 = New System.Windows.Forms.Label()
        Me.cmbCompany_col = New System.Windows.Forms.ComboBox()
        Me.Label98 = New System.Windows.Forms.Label()
        Me.cmbBank_col = New System.Windows.Forms.ComboBox()
        Me.lblBankAccount_col = New System.Windows.Forms.Label()
        Me.Label104 = New System.Windows.Forms.Label()
        Me.Label99 = New System.Windows.Forms.Label()
        Me.lblChequeReq_col = New System.Windows.Forms.Label()
        Me.Label97 = New System.Windows.Forms.Label()
        Me.Label96 = New System.Windows.Forms.Label()
        Me.cmbSubjects = New System.Windows.Forms.ComboBox()
        Me.lblSubjects_col = New System.Windows.Forms.Label()
        Me.txtCusName_col = New System.Windows.Forms.TextBox()
        Me.txtCusId_col = New System.Windows.Forms.TextBox()
        Me.btnQueryCus_col = New System.Windows.Forms.Button()
        Me.Label93 = New System.Windows.Forms.Label()
        Me.lblCusCode_col = New System.Windows.Forms.Label()
        Me.DateTimePicker14 = New System.Windows.Forms.DateTimePicker()
        Me.Label156 = New System.Windows.Forms.Label()
        Me.txtCheque_col = New System.Windows.Forms.TextBox()
        Me.txtMemo_col = New System.Windows.Forms.TextBox()
        Me.txtAmount_collection = New System.Windows.Forms.TextBox()
        Me.txtColId = New System.Windows.Forms.TextBox()
        Me.lblCheque_col = New System.Windows.Forms.Label()
        Me.dtpDate_col = New System.Windows.Forms.DateTimePicker()
        Me.cmbType_col = New System.Windows.Forms.ComboBox()
        Me.dgvCollection = New System.Windows.Forms.DataGridView()
        Me.btnCancel_col = New System.Windows.Forms.Button()
        Me.btnDelete_col = New System.Windows.Forms.Button()
        Me.btnEdit_col = New System.Windows.Forms.Button()
        Me.btnAdd_col = New System.Windows.Forms.Button()
        Me.lblType_col = New System.Windows.Forms.Label()
        Me.Label110 = New System.Windows.Forms.Label()
        Me.Label112 = New System.Windows.Forms.Label()
        Me.Label109 = New System.Windows.Forms.Label()
        CType(Me.dgvCollection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPrint_Col
        '
        Me.btnPrint_Col.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnPrint_Col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnPrint_Col.Location = New System.Drawing.Point(788, 174)
        Me.btnPrint_Col.Name = "btnPrint_Col"
        Me.btnPrint_Col.Size = New System.Drawing.Size(140, 44)
        Me.btnPrint_Col.TabIndex = 509
        Me.btnPrint_Col.Text = "列印傳票"
        Me.btnPrint_Col.UseVisualStyleBackColor = False
        '
        'TextBox14
        '
        Me.TextBox14.Location = New System.Drawing.Point(135, 136)
        Me.TextBox14.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.TextBox14.Name = "TextBox14"
        Me.TextBox14.ReadOnly = True
        Me.TextBox14.Size = New System.Drawing.Size(165, 30)
        Me.TextBox14.TabIndex = 507
        Me.TextBox14.Tag = "col_UnmatchedAmount"
        '
        'Label141
        '
        Me.Label141.AutoSize = True
        Me.Label141.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label141.Location = New System.Drawing.Point(34, 142)
        Me.Label141.Name = "Label141"
        Me.Label141.Size = New System.Drawing.Size(94, 19)
        Me.Label141.TabIndex = 508
        Me.Label141.Text = "未 銷 帳"
        '
        'btnWriteOff
        '
        Me.btnWriteOff.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnWriteOff.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnWriteOff.Location = New System.Drawing.Point(632, 174)
        Me.btnWriteOff.Name = "btnWriteOff"
        Me.btnWriteOff.Size = New System.Drawing.Size(140, 44)
        Me.btnWriteOff.TabIndex = 506
        Me.btnWriteOff.Text = "銷   帳"
        Me.btnWriteOff.UseVisualStyleBackColor = False
        '
        'btnQuery_col
        '
        Me.btnQuery_col.BackColor = System.Drawing.Color.Lime
        Me.btnQuery_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnQuery_col.Location = New System.Drawing.Point(944, 174)
        Me.btnQuery_col.Name = "btnQuery_col"
        Me.btnQuery_col.Size = New System.Drawing.Size(140, 44)
        Me.btnQuery_col.TabIndex = 468
        Me.btnQuery_col.Text = "查  詢"
        Me.btnQuery_col.UseVisualStyleBackColor = False
        '
        'dtpAbleCashingDate
        '
        Me.dtpAbleCashingDate.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.dtpAbleCashingDate.Location = New System.Drawing.Point(1388, 96)
        Me.dtpAbleCashingDate.Name = "dtpAbleCashingDate"
        Me.dtpAbleCashingDate.Size = New System.Drawing.Size(165, 30)
        Me.dtpAbleCashingDate.TabIndex = 505
        Me.dtpAbleCashingDate.Tag = "che_AbleCashingDate"
        Me.dtpAbleCashingDate.Visible = False
        '
        'lblAbleCashingDate
        '
        Me.lblAbleCashingDate.AutoSize = True
        Me.lblAbleCashingDate.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblAbleCashingDate.Location = New System.Drawing.Point(1247, 102)
        Me.lblAbleCashingDate.Name = "lblAbleCashingDate"
        Me.lblAbleCashingDate.Size = New System.Drawing.Size(135, 19)
        Me.lblAbleCashingDate.TabIndex = 504
        Me.lblAbleCashingDate.Text = "支票兌現日期"
        Me.lblAbleCashingDate.Visible = False
        '
        'txtPayBank
        '
        Me.txtPayBank.Location = New System.Drawing.Point(1661, 97)
        Me.txtPayBank.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtPayBank.Name = "txtPayBank"
        Me.txtPayBank.Size = New System.Drawing.Size(165, 30)
        Me.txtPayBank.TabIndex = 502
        Me.txtPayBank.Tag = "che_PayBankName"
        Me.txtPayBank.Visible = False
        '
        'lblPayBank
        '
        Me.lblPayBank.AutoSize = True
        Me.lblPayBank.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblPayBank.Location = New System.Drawing.Point(1559, 102)
        Me.lblPayBank.Name = "lblPayBank"
        Me.lblPayBank.Size = New System.Drawing.Size(93, 19)
        Me.lblPayBank.TabIndex = 503
        Me.lblPayBank.Text = "付款銀行"
        Me.lblPayBank.Visible = False
        '
        'txtCashingDate
        '
        Me.txtCashingDate.Location = New System.Drawing.Point(405, 96)
        Me.txtCashingDate.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCashingDate.Name = "txtCashingDate"
        Me.txtCashingDate.ReadOnly = True
        Me.txtCashingDate.Size = New System.Drawing.Size(165, 30)
        Me.txtCashingDate.TabIndex = 501
        Me.txtCashingDate.Tag = "che_CashingDate"
        Me.txtCashingDate.Visible = False
        '
        'txtCusCode_col
        '
        Me.txtCusCode_col.Location = New System.Drawing.Point(135, 51)
        Me.txtCusCode_col.Name = "txtCusCode_col"
        Me.txtCusCode_col.Size = New System.Drawing.Size(165, 30)
        Me.txtCusCode_col.TabIndex = 500
        Me.txtCusCode_col.Tag = "cus_code"
        '
        'txtCheAcctNum
        '
        Me.txtCheAcctNum.Location = New System.Drawing.Point(1050, 96)
        Me.txtCheAcctNum.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCheAcctNum.Name = "txtCheAcctNum"
        Me.txtCheAcctNum.Size = New System.Drawing.Size(165, 30)
        Me.txtCheAcctNum.TabIndex = 499
        Me.txtCheAcctNum.Tag = "che_AccountNumber"
        Me.txtCheAcctNum.Visible = False
        '
        'lblChequeAccountNumberReq
        '
        Me.lblChequeAccountNumberReq.AutoSize = True
        Me.lblChequeAccountNumberReq.ForeColor = System.Drawing.Color.Red
        Me.lblChequeAccountNumberReq.Location = New System.Drawing.Point(880, 102)
        Me.lblChequeAccountNumberReq.Name = "lblChequeAccountNumberReq"
        Me.lblChequeAccountNumberReq.Size = New System.Drawing.Size(20, 19)
        Me.lblChequeAccountNumberReq.TabIndex = 498
        Me.lblChequeAccountNumberReq.Text = "*"
        Me.lblChequeAccountNumberReq.Visible = False
        '
        'lblChequeAccountNumber
        '
        Me.lblChequeAccountNumber.AutoSize = True
        Me.lblChequeAccountNumber.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblChequeAccountNumber.Location = New System.Drawing.Point(906, 102)
        Me.lblChequeAccountNumber.Name = "lblChequeAccountNumber"
        Me.lblChequeAccountNumber.Size = New System.Drawing.Size(135, 19)
        Me.lblChequeAccountNumber.TabIndex = 497
        Me.lblChequeAccountNumber.Text = "支票銀行帳號"
        Me.lblChequeAccountNumber.Visible = False
        '
        'txtIssuerName
        '
        Me.txtIssuerName.Location = New System.Drawing.Point(706, 96)
        Me.txtIssuerName.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtIssuerName.Name = "txtIssuerName"
        Me.txtIssuerName.Size = New System.Drawing.Size(166, 30)
        Me.txtIssuerName.TabIndex = 496
        Me.txtIssuerName.Tag = "che_IssuerName"
        Me.txtIssuerName.Visible = False
        '
        'lblIssuerNameReq
        '
        Me.lblIssuerNameReq.AutoSize = True
        Me.lblIssuerNameReq.ForeColor = System.Drawing.Color.Red
        Me.lblIssuerNameReq.Location = New System.Drawing.Point(576, 102)
        Me.lblIssuerNameReq.Name = "lblIssuerNameReq"
        Me.lblIssuerNameReq.Size = New System.Drawing.Size(20, 19)
        Me.lblIssuerNameReq.TabIndex = 495
        Me.lblIssuerNameReq.Text = "*"
        Me.lblIssuerNameReq.Visible = False
        '
        'lblIssuerName
        '
        Me.lblIssuerName.AutoSize = True
        Me.lblIssuerName.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblIssuerName.Location = New System.Drawing.Point(602, 102)
        Me.lblIssuerName.Name = "lblIssuerName"
        Me.lblIssuerName.Size = New System.Drawing.Size(94, 19)
        Me.lblIssuerName.TabIndex = 494
        Me.lblIssuerName.Text = "發 票 人"
        Me.lblIssuerName.Visible = False
        '
        'lblCashingDate_col
        '
        Me.lblCashingDate_col.AutoSize = True
        Me.lblCashingDate_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblCashingDate_col.Location = New System.Drawing.Point(306, 102)
        Me.lblCashingDate_col.Name = "lblCashingDate_col"
        Me.lblCashingDate_col.Size = New System.Drawing.Size(93, 19)
        Me.lblCashingDate_col.TabIndex = 493
        Me.lblCashingDate_col.Text = "兌現日期"
        Me.lblCashingDate_col.Visible = False
        '
        'Label115
        '
        Me.Label115.AutoSize = True
        Me.Label115.ForeColor = System.Drawing.Color.Red
        Me.Label115.Location = New System.Drawing.Point(1221, 11)
        Me.Label115.Name = "Label115"
        Me.Label115.Size = New System.Drawing.Size(20, 19)
        Me.Label115.TabIndex = 492
        Me.Label115.Text = "*"
        '
        'cmbCompany_col
        '
        Me.cmbCompany_col.FormattingEnabled = True
        Me.cmbCompany_col.Location = New System.Drawing.Point(1388, 7)
        Me.cmbCompany_col.Name = "cmbCompany_col"
        Me.cmbCompany_col.Size = New System.Drawing.Size(165, 27)
        Me.cmbCompany_col.TabIndex = 491
        Me.cmbCompany_col.Tag = "col_comp_Id"
        '
        'Label98
        '
        Me.Label98.AutoSize = True
        Me.Label98.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label98.Location = New System.Drawing.Point(1247, 11)
        Me.Label98.Name = "Label98"
        Me.Label98.Size = New System.Drawing.Size(95, 19)
        Me.Label98.TabIndex = 490
        Me.Label98.Text = "公    司"
        '
        'cmbBank_col
        '
        Me.cmbBank_col.FormattingEnabled = True
        Me.cmbBank_col.Location = New System.Drawing.Point(1661, 7)
        Me.cmbBank_col.Name = "cmbBank_col"
        Me.cmbBank_col.Size = New System.Drawing.Size(165, 27)
        Me.cmbBank_col.TabIndex = 489
        Me.cmbBank_col.Tag = "col_bank_Id"
        '
        'lblBankAccount_col
        '
        Me.lblBankAccount_col.AutoSize = True
        Me.lblBankAccount_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblBankAccount_col.Location = New System.Drawing.Point(1559, 11)
        Me.lblBankAccount_col.Name = "lblBankAccount_col"
        Me.lblBankAccount_col.Size = New System.Drawing.Size(93, 19)
        Me.lblBankAccount_col.TabIndex = 488
        Me.lblBankAccount_col.Text = "銀行帳號"
        '
        'Label104
        '
        Me.Label104.AutoSize = True
        Me.Label104.ForeColor = System.Drawing.Color.Red
        Me.Label104.Location = New System.Drawing.Point(880, 11)
        Me.Label104.Name = "Label104"
        Me.Label104.Size = New System.Drawing.Size(20, 19)
        Me.Label104.TabIndex = 487
        Me.Label104.Text = "*"
        '
        'Label99
        '
        Me.Label99.AutoSize = True
        Me.Label99.ForeColor = System.Drawing.Color.Red
        Me.Label99.Location = New System.Drawing.Point(8, 57)
        Me.Label99.Name = "Label99"
        Me.Label99.Size = New System.Drawing.Size(20, 19)
        Me.Label99.TabIndex = 486
        Me.Label99.Text = "*"
        '
        'lblChequeReq_col
        '
        Me.lblChequeReq_col.AutoSize = True
        Me.lblChequeReq_col.ForeColor = System.Drawing.Color.Red
        Me.lblChequeReq_col.Location = New System.Drawing.Point(8, 102)
        Me.lblChequeReq_col.Name = "lblChequeReq_col"
        Me.lblChequeReq_col.Size = New System.Drawing.Size(20, 19)
        Me.lblChequeReq_col.TabIndex = 485
        Me.lblChequeReq_col.Text = "*"
        Me.lblChequeReq_col.Visible = False
        '
        'Label97
        '
        Me.Label97.AutoSize = True
        Me.Label97.ForeColor = System.Drawing.Color.Red
        Me.Label97.Location = New System.Drawing.Point(880, 57)
        Me.Label97.Name = "Label97"
        Me.Label97.Size = New System.Drawing.Size(20, 19)
        Me.Label97.TabIndex = 484
        Me.Label97.Text = "*"
        '
        'Label96
        '
        Me.Label96.AutoSize = True
        Me.Label96.ForeColor = System.Drawing.Color.Red
        Me.Label96.Location = New System.Drawing.Point(576, 11)
        Me.Label96.Name = "Label96"
        Me.Label96.Size = New System.Drawing.Size(20, 19)
        Me.Label96.TabIndex = 483
        Me.Label96.Text = "*"
        '
        'cmbSubjects
        '
        Me.cmbSubjects.FormattingEnabled = True
        Me.cmbSubjects.Location = New System.Drawing.Point(1050, 7)
        Me.cmbSubjects.Name = "cmbSubjects"
        Me.cmbSubjects.Size = New System.Drawing.Size(165, 27)
        Me.cmbSubjects.TabIndex = 482
        Me.cmbSubjects.Tag = "col_s_Id"
        '
        'lblSubjects_col
        '
        Me.lblSubjects_col.AutoSize = True
        Me.lblSubjects_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblSubjects_col.Location = New System.Drawing.Point(906, 11)
        Me.lblSubjects_col.Name = "lblSubjects_col"
        Me.lblSubjects_col.Size = New System.Drawing.Size(93, 19)
        Me.lblSubjects_col.TabIndex = 481
        Me.lblSubjects_col.Text = "貸方科目"
        '
        'txtCusName_col
        '
        Me.txtCusName_col.Location = New System.Drawing.Point(405, 51)
        Me.txtCusName_col.Name = "txtCusName_col"
        Me.txtCusName_col.ReadOnly = True
        Me.txtCusName_col.Size = New System.Drawing.Size(378, 30)
        Me.txtCusName_col.TabIndex = 479
        Me.txtCusName_col.Tag = "cus_name"
        '
        'txtCusId_col
        '
        Me.txtCusId_col.Location = New System.Drawing.Point(309, 136)
        Me.txtCusId_col.Name = "txtCusId_col"
        Me.txtCusId_col.ReadOnly = True
        Me.txtCusId_col.Size = New System.Drawing.Size(165, 30)
        Me.txtCusId_col.TabIndex = 477
        Me.txtCusId_col.Tag = "col_cus_Id"
        Me.txtCusId_col.Visible = False
        '
        'btnQueryCus_col
        '
        Me.btnQueryCus_col.AutoSize = True
        Me.btnQueryCus_col.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnQueryCus_col.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnQueryCus_col.Location = New System.Drawing.Point(789, 53)
        Me.btnQueryCus_col.Name = "btnQueryCus_col"
        Me.btnQueryCus_col.Size = New System.Drawing.Size(82, 26)
        Me.btnQueryCus_col.TabIndex = 480
        Me.btnQueryCus_col.Text = "搜尋客戶"
        Me.btnQueryCus_col.UseVisualStyleBackColor = False
        '
        'Label93
        '
        Me.Label93.AutoSize = True
        Me.Label93.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label93.Location = New System.Drawing.Point(306, 57)
        Me.Label93.Name = "Label93"
        Me.Label93.Size = New System.Drawing.Size(93, 19)
        Me.Label93.TabIndex = 478
        Me.Label93.Text = "客戶名稱"
        '
        'lblCusCode_col
        '
        Me.lblCusCode_col.AutoSize = True
        Me.lblCusCode_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblCusCode_col.Location = New System.Drawing.Point(34, 54)
        Me.lblCusCode_col.Name = "lblCusCode_col"
        Me.lblCusCode_col.Size = New System.Drawing.Size(93, 19)
        Me.lblCusCode_col.TabIndex = 476
        Me.lblCusCode_col.Text = "客戶代號"
        '
        'DateTimePicker14
        '
        Me.DateTimePicker14.CustomFormat = "yyyy年MM月"
        Me.DateTimePicker14.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.DateTimePicker14.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker14.Location = New System.Drawing.Point(405, 5)
        Me.DateTimePicker14.Name = "DateTimePicker14"
        Me.DateTimePicker14.Size = New System.Drawing.Size(165, 30)
        Me.DateTimePicker14.TabIndex = 475
        Me.DateTimePicker14.Tag = "col_AccountMonth"
        '
        'Label156
        '
        Me.Label156.AutoSize = True
        Me.Label156.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label156.Location = New System.Drawing.Point(306, 11)
        Me.Label156.Name = "Label156"
        Me.Label156.Size = New System.Drawing.Size(93, 19)
        Me.Label156.TabIndex = 474
        Me.Label156.Text = "帳款月份"
        '
        'txtCheque_col
        '
        Me.txtCheque_col.Location = New System.Drawing.Point(135, 96)
        Me.txtCheque_col.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCheque_col.Name = "txtCheque_col"
        Me.txtCheque_col.Size = New System.Drawing.Size(165, 30)
        Me.txtCheque_col.TabIndex = 472
        Me.txtCheque_col.Tag = "col_Cheque"
        Me.txtCheque_col.Visible = False
        '
        'txtMemo_col
        '
        Me.txtMemo_col.Location = New System.Drawing.Point(1388, 51)
        Me.txtMemo_col.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtMemo_col.Name = "txtMemo_col"
        Me.txtMemo_col.Size = New System.Drawing.Size(438, 30)
        Me.txtMemo_col.TabIndex = 461
        Me.txtMemo_col.Tag = "col_Memo"
        '
        'txtAmount_collection
        '
        Me.txtAmount_collection.Location = New System.Drawing.Point(706, 5)
        Me.txtAmount_collection.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtAmount_collection.Name = "txtAmount_collection"
        Me.txtAmount_collection.Size = New System.Drawing.Size(165, 30)
        Me.txtAmount_collection.TabIndex = 458
        Me.txtAmount_collection.Tag = "col_Amount"
        '
        'txtColId
        '
        Me.txtColId.Location = New System.Drawing.Point(483, 136)
        Me.txtColId.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtColId.Name = "txtColId"
        Me.txtColId.ReadOnly = True
        Me.txtColId.Size = New System.Drawing.Size(165, 30)
        Me.txtColId.TabIndex = 457
        Me.txtColId.Tag = "col_Id"
        Me.txtColId.Visible = False
        '
        'lblCheque_col
        '
        Me.lblCheque_col.AutoSize = True
        Me.lblCheque_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblCheque_col.Location = New System.Drawing.Point(34, 97)
        Me.lblCheque_col.Name = "lblCheque_col"
        Me.lblCheque_col.Size = New System.Drawing.Size(93, 19)
        Me.lblCheque_col.TabIndex = 473
        Me.lblCheque_col.Text = "支票號碼"
        Me.lblCheque_col.Visible = False
        '
        'dtpDate_col
        '
        Me.dtpDate_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.dtpDate_col.Location = New System.Drawing.Point(135, 5)
        Me.dtpDate_col.Name = "dtpDate_col"
        Me.dtpDate_col.Size = New System.Drawing.Size(165, 30)
        Me.dtpDate_col.TabIndex = 471
        Me.dtpDate_col.Tag = "col_Date"
        '
        'cmbType_col
        '
        Me.cmbType_col.FormattingEnabled = True
        Me.cmbType_col.Items.AddRange(New Object() {"現金", "銀行", "支票"})
        Me.cmbType_col.Location = New System.Drawing.Point(1050, 53)
        Me.cmbType_col.Name = "cmbType_col"
        Me.cmbType_col.Size = New System.Drawing.Size(165, 27)
        Me.cmbType_col.TabIndex = 470
        Me.cmbType_col.Tag = "col_Type"
        '
        'dgvCollection
        '
        Me.dgvCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCollection.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvCollection.Location = New System.Drawing.Point(0, 249)
        Me.dgvCollection.Name = "dgvCollection"
        Me.dgvCollection.ReadOnly = True
        Me.dgvCollection.RowTemplate.Height = 24
        Me.dgvCollection.Size = New System.Drawing.Size(1882, 700)
        Me.dgvCollection.TabIndex = 469
        '
        'btnCancel_col
        '
        Me.btnCancel_col.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnCancel_col.Location = New System.Drawing.Point(476, 174)
        Me.btnCancel_col.Name = "btnCancel_col"
        Me.btnCancel_col.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel_col.TabIndex = 467
        Me.btnCancel_col.Text = "取  消"
        Me.btnCancel_col.UseVisualStyleBackColor = False
        '
        'btnDelete_col
        '
        Me.btnDelete_col.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnDelete_col.Location = New System.Drawing.Point(320, 174)
        Me.btnDelete_col.Name = "btnDelete_col"
        Me.btnDelete_col.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete_col.TabIndex = 466
        Me.btnDelete_col.Text = "刪  除"
        Me.btnDelete_col.UseVisualStyleBackColor = False
        '
        'btnEdit_col
        '
        Me.btnEdit_col.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnEdit_col.Location = New System.Drawing.Point(164, 174)
        Me.btnEdit_col.Name = "btnEdit_col"
        Me.btnEdit_col.Size = New System.Drawing.Size(140, 44)
        Me.btnEdit_col.TabIndex = 465
        Me.btnEdit_col.Text = "修  改"
        Me.btnEdit_col.UseVisualStyleBackColor = False
        '
        'btnAdd_col
        '
        Me.btnAdd_col.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnAdd_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnAdd_col.Location = New System.Drawing.Point(8, 174)
        Me.btnAdd_col.Name = "btnAdd_col"
        Me.btnAdd_col.Size = New System.Drawing.Size(140, 44)
        Me.btnAdd_col.TabIndex = 464
        Me.btnAdd_col.Tag = ""
        Me.btnAdd_col.Text = "新  增"
        Me.btnAdd_col.UseVisualStyleBackColor = False
        '
        'lblType_col
        '
        Me.lblType_col.AutoSize = True
        Me.lblType_col.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblType_col.Location = New System.Drawing.Point(906, 57)
        Me.lblType_col.Name = "lblType_col"
        Me.lblType_col.Size = New System.Drawing.Size(93, 19)
        Me.lblType_col.TabIndex = 463
        Me.lblType_col.Text = "借方科目"
        '
        'Label110
        '
        Me.Label110.AutoSize = True
        Me.Label110.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label110.Location = New System.Drawing.Point(1247, 57)
        Me.Label110.Name = "Label110"
        Me.Label110.Size = New System.Drawing.Size(95, 19)
        Me.Label110.TabIndex = 462
        Me.Label110.Text = "備    註"
        '
        'Label112
        '
        Me.Label112.AutoSize = True
        Me.Label112.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label112.Location = New System.Drawing.Point(34, 11)
        Me.Label112.Name = "Label112"
        Me.Label112.Size = New System.Drawing.Size(95, 19)
        Me.Label112.TabIndex = 460
        Me.Label112.Text = "日    期"
        '
        'Label109
        '
        Me.Label109.AutoSize = True
        Me.Label109.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label109.Location = New System.Drawing.Point(602, 11)
        Me.Label109.Name = "Label109"
        Me.Label109.Size = New System.Drawing.Size(95, 19)
        Me.Label109.TabIndex = 459
        Me.Label109.Text = "金    額"
        '
        'CollectionUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnPrint_Col)
        Me.Controls.Add(Me.TextBox14)
        Me.Controls.Add(Me.Label141)
        Me.Controls.Add(Me.btnWriteOff)
        Me.Controls.Add(Me.btnQuery_col)
        Me.Controls.Add(Me.dtpAbleCashingDate)
        Me.Controls.Add(Me.lblAbleCashingDate)
        Me.Controls.Add(Me.txtPayBank)
        Me.Controls.Add(Me.lblPayBank)
        Me.Controls.Add(Me.txtCashingDate)
        Me.Controls.Add(Me.txtCusCode_col)
        Me.Controls.Add(Me.txtCheAcctNum)
        Me.Controls.Add(Me.lblChequeAccountNumberReq)
        Me.Controls.Add(Me.lblChequeAccountNumber)
        Me.Controls.Add(Me.txtIssuerName)
        Me.Controls.Add(Me.lblIssuerNameReq)
        Me.Controls.Add(Me.lblIssuerName)
        Me.Controls.Add(Me.lblCashingDate_col)
        Me.Controls.Add(Me.Label115)
        Me.Controls.Add(Me.cmbCompany_col)
        Me.Controls.Add(Me.Label98)
        Me.Controls.Add(Me.cmbBank_col)
        Me.Controls.Add(Me.lblBankAccount_col)
        Me.Controls.Add(Me.Label104)
        Me.Controls.Add(Me.Label99)
        Me.Controls.Add(Me.lblChequeReq_col)
        Me.Controls.Add(Me.Label97)
        Me.Controls.Add(Me.Label96)
        Me.Controls.Add(Me.cmbSubjects)
        Me.Controls.Add(Me.lblSubjects_col)
        Me.Controls.Add(Me.txtCusName_col)
        Me.Controls.Add(Me.txtCusId_col)
        Me.Controls.Add(Me.btnQueryCus_col)
        Me.Controls.Add(Me.Label93)
        Me.Controls.Add(Me.lblCusCode_col)
        Me.Controls.Add(Me.DateTimePicker14)
        Me.Controls.Add(Me.Label156)
        Me.Controls.Add(Me.txtCheque_col)
        Me.Controls.Add(Me.txtMemo_col)
        Me.Controls.Add(Me.txtAmount_collection)
        Me.Controls.Add(Me.txtColId)
        Me.Controls.Add(Me.lblCheque_col)
        Me.Controls.Add(Me.dtpDate_col)
        Me.Controls.Add(Me.cmbType_col)
        Me.Controls.Add(Me.dgvCollection)
        Me.Controls.Add(Me.btnCancel_col)
        Me.Controls.Add(Me.btnDelete_col)
        Me.Controls.Add(Me.btnEdit_col)
        Me.Controls.Add(Me.btnAdd_col)
        Me.Controls.Add(Me.lblType_col)
        Me.Controls.Add(Me.Label110)
        Me.Controls.Add(Me.Label112)
        Me.Controls.Add(Me.Label109)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "CollectionUserControl"
        Me.Size = New System.Drawing.Size(1882, 949)
        CType(Me.dgvCollection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnPrint_Col As Button
    Friend WithEvents TextBox14 As TextBox
    Friend WithEvents Label141 As Label
    Friend WithEvents btnWriteOff As Button
    Friend WithEvents btnQuery_col As Button
    Friend WithEvents dtpAbleCashingDate As DateTimePicker
    Friend WithEvents lblAbleCashingDate As Label
    Friend WithEvents txtPayBank As TextBox
    Friend WithEvents lblPayBank As Label
    Friend WithEvents txtCashingDate As TextBox
    Friend WithEvents txtCusCode_col As TextBox
    Friend WithEvents txtCheAcctNum As TextBox
    Friend WithEvents lblChequeAccountNumberReq As Label
    Friend WithEvents lblChequeAccountNumber As Label
    Friend WithEvents txtIssuerName As TextBox
    Friend WithEvents lblIssuerNameReq As Label
    Friend WithEvents lblIssuerName As Label
    Friend WithEvents lblCashingDate_col As Label
    Friend WithEvents Label115 As Label
    Friend WithEvents cmbCompany_col As ComboBox
    Friend WithEvents Label98 As Label
    Friend WithEvents cmbBank_col As ComboBox
    Friend WithEvents lblBankAccount_col As Label
    Friend WithEvents Label104 As Label
    Friend WithEvents Label99 As Label
    Friend WithEvents lblChequeReq_col As Label
    Friend WithEvents Label97 As Label
    Friend WithEvents Label96 As Label
    Friend WithEvents cmbSubjects As ComboBox
    Friend WithEvents lblSubjects_col As Label
    Friend WithEvents txtCusName_col As TextBox
    Friend WithEvents txtCusId_col As TextBox
    Friend WithEvents btnQueryCus_col As Button
    Friend WithEvents Label93 As Label
    Friend WithEvents lblCusCode_col As Label
    Friend WithEvents DateTimePicker14 As DateTimePicker
    Friend WithEvents Label156 As Label
    Friend WithEvents txtCheque_col As TextBox
    Friend WithEvents txtMemo_col As TextBox
    Friend WithEvents txtAmount_collection As TextBox
    Friend WithEvents txtColId As TextBox
    Friend WithEvents lblCheque_col As Label
    Friend WithEvents dtpDate_col As DateTimePicker
    Friend WithEvents cmbType_col As ComboBox
    Friend WithEvents dgvCollection As DataGridView
    Friend WithEvents btnCancel_col As Button
    Friend WithEvents btnDelete_col As Button
    Friend WithEvents btnEdit_col As Button
    Friend WithEvents btnAdd_col As Button
    Friend WithEvents lblType_col As Label
    Friend WithEvents Label110 As Label
    Friend WithEvents Label112 As Label
    Friend WithEvents Label109 As Label
End Class
