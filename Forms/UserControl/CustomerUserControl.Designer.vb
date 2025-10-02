<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomerUserControl
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
        Me.txtInvoiceMemo = New System.Windows.Forms.TextBox()
        Me.Label185 = New System.Windows.Forms.Label()
        Me.cmbCompany_cus = New System.Windows.Forms.ComboBox()
        Me.Label184 = New System.Windows.Forms.Label()
        Me.grpInsurance = New System.Windows.Forms.GroupBox()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.chkIsInsurance = New System.Windows.Forms.CheckBox()
        Me.Label119 = New System.Windows.Forms.Label()
        Me.grpPricePlan = New System.Windows.Forms.GroupBox()
        Me.cmbPricePlan = New System.Windows.Forms.ComboBox()
        Me.Label314 = New System.Windows.Forms.Label()
        Me.lblNormalGas_cus = New System.Windows.Forms.Label()
        Me.lblCGas_cus = New System.Windows.Forms.Label()
        Me.txtcus_gas_c = New System.Windows.Forms.TextBox()
        Me.txtcus_gas_normal = New System.Windows.Forms.TextBox()
        Me.txtcus_gas_normal_deliver = New System.Windows.Forms.TextBox()
        Me.lblNormalGasDeliver_cus = New System.Windows.Forms.Label()
        Me.txtcus_gas_c_deliver = New System.Windows.Forms.TextBox()
        Me.lblCGasDeliver_cus = New System.Windows.Forms.Label()
        Me.grpStock = New System.Windows.Forms.GroupBox()
        Me.Label209 = New System.Windows.Forms.Label()
        Me.TextBox88 = New System.Windows.Forms.TextBox()
        Me.Label210 = New System.Windows.Forms.Label()
        Me.TextBox89 = New System.Windows.Forms.TextBox()
        Me.Label278 = New System.Windows.Forms.Label()
        Me.TextBox90 = New System.Windows.Forms.TextBox()
        Me.Label201 = New System.Windows.Forms.Label()
        Me.TextBox82 = New System.Windows.Forms.TextBox()
        Me.Label207 = New System.Windows.Forms.Label()
        Me.TextBox84 = New System.Windows.Forms.TextBox()
        Me.Label208 = New System.Windows.Forms.Label()
        Me.TextBox85 = New System.Windows.Forms.TextBox()
        Me.Label200 = New System.Windows.Forms.Label()
        Me.TextBox81 = New System.Windows.Forms.TextBox()
        Me.Label199 = New System.Windows.Forms.Label()
        Me.TextBox80 = New System.Windows.Forms.TextBox()
        Me.Label198 = New System.Windows.Forms.Label()
        Me.TextBox79 = New System.Windows.Forms.TextBox()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.txtCusCode = New System.Windows.Forms.TextBox()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtTaxId_cus = New System.Windows.Forms.TextBox()
        Me.txtCusContactPerson = New System.Windows.Forms.TextBox()
        Me.txtcus_phone2 = New System.Windows.Forms.TextBox()
        Me.txtcus_tax_id = New System.Windows.Forms.TextBox()
        Me.txtcus_principal = New System.Windows.Forms.TextBox()
        Me.txtcus_memo = New System.Windows.Forms.TextBox()
        Me.txtcus_address = New System.Windows.Forms.TextBox()
        Me.txtCusPhone1 = New System.Windows.Forms.TextBox()
        Me.txtCusName_cus = New System.Windows.Forms.TextBox()
        Me.txtcus_id = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnCreate = New System.Windows.Forms.Button()
        Me.dgvCustomer = New System.Windows.Forms.DataGridView()
        Me.grpInsurance.SuspendLayout()
        Me.grpPricePlan.SuspendLayout()
        Me.grpStock.SuspendLayout()
        CType(Me.dgvCustomer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtInvoiceMemo
        '
        Me.txtInvoiceMemo.Location = New System.Drawing.Point(978, 107)
        Me.txtInvoiceMemo.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtInvoiceMemo.Name = "txtInvoiceMemo"
        Me.txtInvoiceMemo.Size = New System.Drawing.Size(361, 30)
        Me.txtInvoiceMemo.TabIndex = 397
        Me.txtInvoiceMemo.Tag = "cus_InvoiceMemo"
        '
        'Label185
        '
        Me.Label185.AutoSize = True
        Me.Label185.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label185.Location = New System.Drawing.Point(884, 113)
        Me.Label185.Name = "Label185"
        Me.Label185.Size = New System.Drawing.Size(93, 19)
        Me.Label185.TabIndex = 398
        Me.Label185.Text = "發票備註"
        '
        'cmbCompany_cus
        '
        Me.cmbCompany_cus.FormattingEnabled = True
        Me.cmbCompany_cus.Location = New System.Drawing.Point(143, 199)
        Me.cmbCompany_cus.Name = "cmbCompany_cus"
        Me.cmbCompany_cus.Size = New System.Drawing.Size(150, 27)
        Me.cmbCompany_cus.TabIndex = 396
        Me.cmbCompany_cus.Tag = "cus_comp_Id"
        '
        'Label184
        '
        Me.Label184.AutoSize = True
        Me.Label184.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label184.Location = New System.Drawing.Point(39, 203)
        Me.Label184.Name = "Label184"
        Me.Label184.Size = New System.Drawing.Size(93, 19)
        Me.Label184.TabIndex = 395
        Me.Label184.Text = "所屬公司"
        '
        'grpInsurance
        '
        Me.grpInsurance.Controls.Add(Me.RadioButton2)
        Me.grpInsurance.Controls.Add(Me.RadioButton3)
        Me.grpInsurance.Controls.Add(Me.RadioButton1)
        Me.grpInsurance.Controls.Add(Me.chkIsInsurance)
        Me.grpInsurance.Location = New System.Drawing.Point(874, 4)
        Me.grpInsurance.Name = "grpInsurance"
        Me.grpInsurance.Size = New System.Drawing.Size(464, 57)
        Me.grpInsurance.TabIndex = 394
        Me.grpInsurance.TabStop = False
        Me.grpInsurance.Tag = "cus_InsurancePrice"
        Me.grpInsurance.Text = "保險"
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(160, 29)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(38, 23)
        Me.RadioButton2.TabIndex = 2
        Me.RadioButton2.Text = "0"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(83, 29)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(71, 23)
        Me.RadioButton3.TabIndex = 1
        Me.RadioButton3.Text = "0.08"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(6, 29)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(71, 23)
        Me.RadioButton1.TabIndex = 0
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "0.12"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'chkIsInsurance
        '
        Me.chkIsInsurance.AutoSize = True
        Me.chkIsInsurance.Location = New System.Drawing.Point(204, 29)
        Me.chkIsInsurance.Name = "chkIsInsurance"
        Me.chkIsInsurance.Size = New System.Drawing.Size(154, 23)
        Me.chkIsInsurance.TabIndex = 353
        Me.chkIsInsurance.Tag = "cus_IsInsurance"
        Me.chkIsInsurance.Text = "是否包含保險"
        Me.chkIsInsurance.UseVisualStyleBackColor = True
        '
        'Label119
        '
        Me.Label119.AutoSize = True
        Me.Label119.ForeColor = System.Drawing.Color.Red
        Me.Label119.Location = New System.Drawing.Point(306, 113)
        Me.Label119.Name = "Label119"
        Me.Label119.Size = New System.Drawing.Size(20, 19)
        Me.Label119.TabIndex = 393
        Me.Label119.Text = "*"
        Me.Label119.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpPricePlan
        '
        Me.grpPricePlan.Controls.Add(Me.cmbPricePlan)
        Me.grpPricePlan.Controls.Add(Me.Label314)
        Me.grpPricePlan.Controls.Add(Me.lblNormalGas_cus)
        Me.grpPricePlan.Controls.Add(Me.lblCGas_cus)
        Me.grpPricePlan.Controls.Add(Me.txtcus_gas_c)
        Me.grpPricePlan.Controls.Add(Me.txtcus_gas_normal)
        Me.grpPricePlan.Controls.Add(Me.txtcus_gas_normal_deliver)
        Me.grpPricePlan.Controls.Add(Me.lblNormalGasDeliver_cus)
        Me.grpPricePlan.Controls.Add(Me.txtcus_gas_c_deliver)
        Me.grpPricePlan.Controls.Add(Me.lblCGasDeliver_cus)
        Me.grpPricePlan.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.grpPricePlan.Location = New System.Drawing.Point(1344, 6)
        Me.grpPricePlan.Name = "grpPricePlan"
        Me.grpPricePlan.Padding = New System.Windows.Forms.Padding(5)
        Me.grpPricePlan.Size = New System.Drawing.Size(530, 160)
        Me.grpPricePlan.TabIndex = 392
        Me.grpPricePlan.TabStop = False
        Me.grpPricePlan.Text = "價格方案"
        '
        'cmbPricePlan
        '
        Me.cmbPricePlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPricePlan.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.cmbPricePlan.FormattingEnabled = True
        Me.cmbPricePlan.Items.AddRange(New Object() {"依發票", "依實際提貨量", "自填"})
        Me.cmbPricePlan.Location = New System.Drawing.Point(112, 28)
        Me.cmbPricePlan.Name = "cmbPricePlan"
        Me.cmbPricePlan.Size = New System.Drawing.Size(100, 27)
        Me.cmbPricePlan.TabIndex = 348
        Me.cmbPricePlan.Tag = "cus_pp_Id"
        '
        'Label314
        '
        Me.Label314.AutoSize = True
        Me.Label314.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label314.Location = New System.Drawing.Point(8, 31)
        Me.Label314.Name = "Label314"
        Me.Label314.Size = New System.Drawing.Size(93, 19)
        Me.Label314.TabIndex = 347
        Me.Label314.Text = "價格方案"
        '
        'lblNormalGas_cus
        '
        Me.lblNormalGas_cus.AutoSize = True
        Me.lblNormalGas_cus.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblNormalGas_cus.Location = New System.Drawing.Point(8, 74)
        Me.lblNormalGas_cus.Name = "lblNormalGas_cus"
        Me.lblNormalGas_cus.Size = New System.Drawing.Size(95, 19)
        Me.lblNormalGas_cus.TabIndex = 340
        Me.lblNormalGas_cus.Tag = ""
        Me.lblNormalGas_cus.Text = "普    氣"
        '
        'lblCGas_cus
        '
        Me.lblCGas_cus.AutoSize = True
        Me.lblCGas_cus.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblCGas_cus.Location = New System.Drawing.Point(221, 77)
        Me.lblCGas_cus.Name = "lblCGas_cus"
        Me.lblCGas_cus.Size = New System.Drawing.Size(95, 19)
        Me.lblCGas_cus.TabIndex = 338
        Me.lblCGas_cus.Text = "丙    氣"
        '
        'txtcus_gas_c
        '
        Me.txtcus_gas_c.Location = New System.Drawing.Point(325, 73)
        Me.txtcus_gas_c.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtcus_gas_c.Name = "txtcus_gas_c"
        Me.txtcus_gas_c.ReadOnly = True
        Me.txtcus_gas_c.Size = New System.Drawing.Size(100, 27)
        Me.txtcus_gas_c.TabIndex = 337
        Me.txtcus_gas_c.Tag = "pp_Gas_c"
        '
        'txtcus_gas_normal
        '
        Me.txtcus_gas_normal.Location = New System.Drawing.Point(112, 73)
        Me.txtcus_gas_normal.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtcus_gas_normal.Name = "txtcus_gas_normal"
        Me.txtcus_gas_normal.ReadOnly = True
        Me.txtcus_gas_normal.Size = New System.Drawing.Size(100, 27)
        Me.txtcus_gas_normal.TabIndex = 339
        Me.txtcus_gas_normal.Tag = "pp_Gas"
        '
        'txtcus_gas_normal_deliver
        '
        Me.txtcus_gas_normal_deliver.Location = New System.Drawing.Point(112, 116)
        Me.txtcus_gas_normal_deliver.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtcus_gas_normal_deliver.Name = "txtcus_gas_normal_deliver"
        Me.txtcus_gas_normal_deliver.ReadOnly = True
        Me.txtcus_gas_normal_deliver.Size = New System.Drawing.Size(100, 27)
        Me.txtcus_gas_normal_deliver.TabIndex = 343
        Me.txtcus_gas_normal_deliver.Tag = "pp_GasDelivery"
        '
        'lblNormalGasDeliver_cus
        '
        Me.lblNormalGasDeliver_cus.AutoSize = True
        Me.lblNormalGasDeliver_cus.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblNormalGasDeliver_cus.Location = New System.Drawing.Point(8, 117)
        Me.lblNormalGasDeliver_cus.Name = "lblNormalGasDeliver_cus"
        Me.lblNormalGasDeliver_cus.Size = New System.Drawing.Size(95, 19)
        Me.lblNormalGasDeliver_cus.TabIndex = 344
        Me.lblNormalGasDeliver_cus.Tag = ""
        Me.lblNormalGasDeliver_cus.Text = "廠    普"
        '
        'txtcus_gas_c_deliver
        '
        Me.txtcus_gas_c_deliver.Location = New System.Drawing.Point(325, 116)
        Me.txtcus_gas_c_deliver.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtcus_gas_c_deliver.Name = "txtcus_gas_c_deliver"
        Me.txtcus_gas_c_deliver.ReadOnly = True
        Me.txtcus_gas_c_deliver.Size = New System.Drawing.Size(100, 27)
        Me.txtcus_gas_c_deliver.TabIndex = 341
        Me.txtcus_gas_c_deliver.Tag = "pp_GasDelivery_c"
        '
        'lblCGasDeliver_cus
        '
        Me.lblCGasDeliver_cus.AutoSize = True
        Me.lblCGasDeliver_cus.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblCGasDeliver_cus.Location = New System.Drawing.Point(221, 120)
        Me.lblCGasDeliver_cus.Name = "lblCGasDeliver_cus"
        Me.lblCGasDeliver_cus.Size = New System.Drawing.Size(95, 19)
        Me.lblCGasDeliver_cus.TabIndex = 342
        Me.lblCGasDeliver_cus.Text = "廠    丙"
        '
        'grpStock
        '
        Me.grpStock.Controls.Add(Me.Label209)
        Me.grpStock.Controls.Add(Me.TextBox88)
        Me.grpStock.Controls.Add(Me.Label210)
        Me.grpStock.Controls.Add(Me.TextBox89)
        Me.grpStock.Controls.Add(Me.Label278)
        Me.grpStock.Controls.Add(Me.TextBox90)
        Me.grpStock.Controls.Add(Me.Label201)
        Me.grpStock.Controls.Add(Me.TextBox82)
        Me.grpStock.Controls.Add(Me.Label207)
        Me.grpStock.Controls.Add(Me.TextBox84)
        Me.grpStock.Controls.Add(Me.Label208)
        Me.grpStock.Controls.Add(Me.TextBox85)
        Me.grpStock.Controls.Add(Me.Label200)
        Me.grpStock.Controls.Add(Me.TextBox81)
        Me.grpStock.Controls.Add(Me.Label199)
        Me.grpStock.Controls.Add(Me.TextBox80)
        Me.grpStock.Controls.Add(Me.Label198)
        Me.grpStock.Controls.Add(Me.TextBox79)
        Me.grpStock.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.grpStock.Location = New System.Drawing.Point(1344, 172)
        Me.grpStock.Name = "grpStock"
        Me.grpStock.Padding = New System.Windows.Forms.Padding(5)
        Me.grpStock.Size = New System.Drawing.Size(530, 160)
        Me.grpStock.TabIndex = 391
        Me.grpStock.TabStop = False
        Me.grpStock.Text = "瓦斯桶庫存"
        '
        'Label209
        '
        Me.Label209.AutoSize = True
        Me.Label209.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label209.Location = New System.Drawing.Point(350, 120)
        Me.Label209.Name = "Label209"
        Me.Label209.Size = New System.Drawing.Size(42, 19)
        Me.Label209.TabIndex = 348
        Me.Label209.Text = "2Kg"
        '
        'TextBox88
        '
        Me.TextBox88.Location = New System.Drawing.Point(412, 119)
        Me.TextBox88.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.TextBox88.Name = "TextBox88"
        Me.TextBox88.Size = New System.Drawing.Size(100, 27)
        Me.TextBox88.TabIndex = 347
        Me.TextBox88.Tag = "cus_gas_2"
        '
        'Label210
        '
        Me.Label210.AutoSize = True
        Me.Label210.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label210.Location = New System.Drawing.Point(350, 77)
        Me.Label210.Name = "Label210"
        Me.Label210.Size = New System.Drawing.Size(53, 19)
        Me.Label210.TabIndex = 346
        Me.Label210.Text = "18Kg"
        '
        'TextBox89
        '
        Me.TextBox89.Location = New System.Drawing.Point(412, 76)
        Me.TextBox89.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.TextBox89.Name = "TextBox89"
        Me.TextBox89.Size = New System.Drawing.Size(100, 27)
        Me.TextBox89.TabIndex = 345
        Me.TextBox89.Tag = "cus_gas_18"
        '
        'Label278
        '
        Me.Label278.AutoSize = True
        Me.Label278.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label278.Location = New System.Drawing.Point(350, 34)
        Me.Label278.Name = "Label278"
        Me.Label278.Size = New System.Drawing.Size(53, 19)
        Me.Label278.TabIndex = 344
        Me.Label278.Text = "16Kg"
        '
        'TextBox90
        '
        Me.TextBox90.Location = New System.Drawing.Point(412, 33)
        Me.TextBox90.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.TextBox90.Name = "TextBox90"
        Me.TextBox90.Size = New System.Drawing.Size(100, 27)
        Me.TextBox90.TabIndex = 343
        Me.TextBox90.Tag = "cus_gas_16"
        '
        'Label201
        '
        Me.Label201.AutoSize = True
        Me.Label201.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label201.Location = New System.Drawing.Point(179, 120)
        Me.Label201.Name = "Label201"
        Me.Label201.Size = New System.Drawing.Size(42, 19)
        Me.Label201.TabIndex = 342
        Me.Label201.Text = "5Kg"
        '
        'TextBox82
        '
        Me.TextBox82.Location = New System.Drawing.Point(241, 119)
        Me.TextBox82.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.TextBox82.Name = "TextBox82"
        Me.TextBox82.Size = New System.Drawing.Size(100, 27)
        Me.TextBox82.TabIndex = 341
        Me.TextBox82.Tag = "cus_gas_5"
        '
        'Label207
        '
        Me.Label207.AutoSize = True
        Me.Label207.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label207.Location = New System.Drawing.Point(179, 77)
        Me.Label207.Name = "Label207"
        Me.Label207.Size = New System.Drawing.Size(42, 19)
        Me.Label207.TabIndex = 340
        Me.Label207.Text = "4Kg"
        '
        'TextBox84
        '
        Me.TextBox84.Location = New System.Drawing.Point(241, 76)
        Me.TextBox84.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.TextBox84.Name = "TextBox84"
        Me.TextBox84.Size = New System.Drawing.Size(100, 27)
        Me.TextBox84.TabIndex = 339
        Me.TextBox84.Tag = "cus_gas_4"
        '
        'Label208
        '
        Me.Label208.AutoSize = True
        Me.Label208.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label208.Location = New System.Drawing.Point(179, 34)
        Me.Label208.Name = "Label208"
        Me.Label208.Size = New System.Drawing.Size(53, 19)
        Me.Label208.TabIndex = 338
        Me.Label208.Text = "20Kg"
        '
        'TextBox85
        '
        Me.TextBox85.Location = New System.Drawing.Point(241, 33)
        Me.TextBox85.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.TextBox85.Name = "TextBox85"
        Me.TextBox85.Size = New System.Drawing.Size(100, 27)
        Me.TextBox85.TabIndex = 337
        Me.TextBox85.Tag = "cus_gas_20"
        '
        'Label200
        '
        Me.Label200.AutoSize = True
        Me.Label200.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label200.Location = New System.Drawing.Point(8, 120)
        Me.Label200.Name = "Label200"
        Me.Label200.Size = New System.Drawing.Size(53, 19)
        Me.Label200.TabIndex = 336
        Me.Label200.Text = "14Kg"
        '
        'TextBox81
        '
        Me.TextBox81.Location = New System.Drawing.Point(70, 119)
        Me.TextBox81.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.TextBox81.Name = "TextBox81"
        Me.TextBox81.Size = New System.Drawing.Size(100, 27)
        Me.TextBox81.TabIndex = 335
        Me.TextBox81.Tag = "cus_gas_14"
        '
        'Label199
        '
        Me.Label199.AutoSize = True
        Me.Label199.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label199.Location = New System.Drawing.Point(8, 77)
        Me.Label199.Name = "Label199"
        Me.Label199.Size = New System.Drawing.Size(53, 19)
        Me.Label199.TabIndex = 334
        Me.Label199.Text = "10Kg"
        '
        'TextBox80
        '
        Me.TextBox80.Location = New System.Drawing.Point(70, 76)
        Me.TextBox80.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.TextBox80.Name = "TextBox80"
        Me.TextBox80.Size = New System.Drawing.Size(100, 27)
        Me.TextBox80.TabIndex = 333
        Me.TextBox80.Tag = "cus_gas_10"
        '
        'Label198
        '
        Me.Label198.AutoSize = True
        Me.Label198.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label198.Location = New System.Drawing.Point(8, 34)
        Me.Label198.Name = "Label198"
        Me.Label198.Size = New System.Drawing.Size(53, 19)
        Me.Label198.TabIndex = 332
        Me.Label198.Text = "50Kg"
        '
        'TextBox79
        '
        Me.TextBox79.Location = New System.Drawing.Point(70, 30)
        Me.TextBox79.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.TextBox79.Name = "TextBox79"
        Me.TextBox79.Size = New System.Drawing.Size(100, 27)
        Me.TextBox79.TabIndex = 331
        Me.TextBox79.Tag = "cus_gas_50"
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.ForeColor = System.Drawing.Color.Red
        Me.Label54.Location = New System.Drawing.Point(302, 21)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(20, 19)
        Me.Label54.TabIndex = 390
        Me.Label54.Text = "*"
        Me.Label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCusCode
        '
        Me.txtCusCode.Location = New System.Drawing.Point(432, 15)
        Me.txtCusCode.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCusCode.Name = "txtCusCode"
        Me.txtCusCode.Size = New System.Drawing.Size(150, 30)
        Me.txtCusCode.TabIndex = 388
        Me.txtCusCode.Tag = "cus_code"
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label55.Location = New System.Drawing.Point(328, 21)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(95, 19)
        Me.Label55.TabIndex = 389
        Me.Label55.Text = "代    號"
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.ForeColor = System.Drawing.Color.Red
        Me.Label51.Location = New System.Drawing.Point(591, 21)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(20, 19)
        Me.Label51.TabIndex = 387
        Me.Label51.Text = "*"
        Me.Label51.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.ForeColor = System.Drawing.Color.Red
        Me.Label50.Location = New System.Drawing.Point(13, 159)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(20, 19)
        Me.Label50.TabIndex = 386
        Me.Label50.Text = "*"
        Me.Label50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.ForeColor = System.Drawing.Color.Red
        Me.Label49.Location = New System.Drawing.Point(13, 67)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(20, 19)
        Me.Label49.TabIndex = 385
        Me.Label49.Text = "*"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label19.Location = New System.Drawing.Point(332, 113)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(95, 19)
        Me.Label19.TabIndex = 384
        Me.Label19.Text = "統    編"
        '
        'txtTaxId_cus
        '
        Me.txtTaxId_cus.Location = New System.Drawing.Point(432, 107)
        Me.txtTaxId_cus.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtTaxId_cus.Name = "txtTaxId_cus"
        Me.txtTaxId_cus.Size = New System.Drawing.Size(150, 30)
        Me.txtTaxId_cus.TabIndex = 383
        Me.txtTaxId_cus.Tag = "cus_tax_id"
        '
        'txtCusContactPerson
        '
        Me.txtCusContactPerson.Location = New System.Drawing.Point(143, 153)
        Me.txtCusContactPerson.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCusContactPerson.Name = "txtCusContactPerson"
        Me.txtCusContactPerson.Size = New System.Drawing.Size(150, 30)
        Me.txtCusContactPerson.TabIndex = 381
        Me.txtCusContactPerson.Tag = "cus_contact_person"
        '
        'txtcus_phone2
        '
        Me.txtcus_phone2.Location = New System.Drawing.Point(710, 61)
        Me.txtcus_phone2.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtcus_phone2.Name = "txtcus_phone2"
        Me.txtcus_phone2.Size = New System.Drawing.Size(150, 30)
        Me.txtcus_phone2.TabIndex = 379
        Me.txtcus_phone2.Tag = "cus_phone2"
        '
        'txtcus_tax_id
        '
        Me.txtcus_tax_id.Location = New System.Drawing.Point(710, 107)
        Me.txtcus_tax_id.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtcus_tax_id.Name = "txtcus_tax_id"
        Me.txtcus_tax_id.Size = New System.Drawing.Size(150, 30)
        Me.txtcus_tax_id.TabIndex = 377
        Me.txtcus_tax_id.Tag = "cus_fax"
        '
        'txtcus_principal
        '
        Me.txtcus_principal.Location = New System.Drawing.Point(143, 107)
        Me.txtcus_principal.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtcus_principal.Name = "txtcus_principal"
        Me.txtcus_principal.Size = New System.Drawing.Size(150, 30)
        Me.txtcus_principal.TabIndex = 375
        Me.txtcus_principal.Tag = "cus_principal"
        '
        'txtcus_memo
        '
        Me.txtcus_memo.Location = New System.Drawing.Point(432, 197)
        Me.txtcus_memo.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtcus_memo.Name = "txtcus_memo"
        Me.txtcus_memo.Size = New System.Drawing.Size(428, 30)
        Me.txtcus_memo.TabIndex = 373
        Me.txtcus_memo.Tag = "cus_memo"
        '
        'txtcus_address
        '
        Me.txtcus_address.Location = New System.Drawing.Point(432, 153)
        Me.txtcus_address.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtcus_address.Name = "txtcus_address"
        Me.txtcus_address.Size = New System.Drawing.Size(428, 30)
        Me.txtcus_address.TabIndex = 371
        Me.txtcus_address.Tag = "cus_address"
        '
        'txtCusPhone1
        '
        Me.txtCusPhone1.Location = New System.Drawing.Point(710, 15)
        Me.txtCusPhone1.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCusPhone1.Name = "txtCusPhone1"
        Me.txtCusPhone1.Size = New System.Drawing.Size(150, 30)
        Me.txtCusPhone1.TabIndex = 369
        Me.txtCusPhone1.Tag = "cus_phone1"
        '
        'txtCusName_cus
        '
        Me.txtCusName_cus.Location = New System.Drawing.Point(143, 61)
        Me.txtCusName_cus.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCusName_cus.Name = "txtCusName_cus"
        Me.txtCusName_cus.Size = New System.Drawing.Size(438, 30)
        Me.txtCusName_cus.TabIndex = 367
        Me.txtCusName_cus.Tag = "cus_name"
        '
        'txtcus_id
        '
        Me.txtcus_id.Location = New System.Drawing.Point(143, 15)
        Me.txtcus_id.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtcus_id.Name = "txtcus_id"
        Me.txtcus_id.ReadOnly = True
        Me.txtcus_id.Size = New System.Drawing.Size(150, 30)
        Me.txtcus_id.TabIndex = 365
        Me.txtcus_id.Tag = "cus_id"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label20.Location = New System.Drawing.Point(39, 159)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(94, 19)
        Me.Label20.TabIndex = 382
        Me.Label20.Text = "聯 絡 人"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label21.Location = New System.Drawing.Point(617, 67)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(84, 19)
        Me.Label21.TabIndex = 380
        Me.Label21.Tag = ""
        Me.Label21.Text = "電 話 2"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label22.Location = New System.Drawing.Point(617, 113)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(95, 19)
        Me.Label22.TabIndex = 378
        Me.Label22.Text = "傳    真"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label23.Location = New System.Drawing.Point(39, 113)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(94, 19)
        Me.Label23.TabIndex = 376
        Me.Label23.Text = "負 責 人"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label24.Location = New System.Drawing.Point(332, 203)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(95, 19)
        Me.Label24.TabIndex = 374
        Me.Label24.Text = "備    註"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label25.Location = New System.Drawing.Point(332, 159)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(95, 19)
        Me.Label25.TabIndex = 372
        Me.Label25.Text = "地    址"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label26.Location = New System.Drawing.Point(617, 21)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(84, 19)
        Me.Label26.TabIndex = 370
        Me.Label26.Text = "電 話 1"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label27.Location = New System.Drawing.Point(39, 67)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(95, 19)
        Me.Label27.TabIndex = 368
        Me.Label27.Text = "名    稱"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label29.Location = New System.Drawing.Point(39, 21)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(95, 19)
        Me.Label29.TabIndex = 366
        Me.Label29.Text = "編    號"
        '
        'btnSearch
        '
        Me.btnSearch.BackColor = System.Drawing.Color.Lime
        Me.btnSearch.Location = New System.Drawing.Point(632, 288)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(140, 44)
        Me.btnSearch.TabIndex = 364
        Me.btnSearch.Text = "查  詢"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel.Location = New System.Drawing.Point(476, 288)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel.TabIndex = 363
        Me.btnCancel.Text = "取  消"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete.Location = New System.Drawing.Point(320, 288)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete.TabIndex = 362
        Me.btnDelete.Text = "刪  除"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit.Location = New System.Drawing.Point(164, 288)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(140, 44)
        Me.btnEdit.TabIndex = 361
        Me.btnEdit.Text = "修  改"
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnCreate
        '
        Me.btnCreate.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCreate.Location = New System.Drawing.Point(8, 288)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(140, 44)
        Me.btnCreate.TabIndex = 360
        Me.btnCreate.Tag = ""
        Me.btnCreate.Text = "新  增"
        Me.btnCreate.UseVisualStyleBackColor = False
        '
        'dgvCustomer
        '
        Me.dgvCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCustomer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvCustomer.Location = New System.Drawing.Point(10, 359)
        Me.dgvCustomer.Name = "dgvCustomer"
        Me.dgvCustomer.ReadOnly = True
        Me.dgvCustomer.RowTemplate.Height = 24
        Me.dgvCustomer.Size = New System.Drawing.Size(1862, 580)
        Me.dgvCustomer.TabIndex = 359
        '
        'CustomerUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtInvoiceMemo)
        Me.Controls.Add(Me.Label185)
        Me.Controls.Add(Me.cmbCompany_cus)
        Me.Controls.Add(Me.Label184)
        Me.Controls.Add(Me.grpInsurance)
        Me.Controls.Add(Me.Label119)
        Me.Controls.Add(Me.grpPricePlan)
        Me.Controls.Add(Me.grpStock)
        Me.Controls.Add(Me.Label54)
        Me.Controls.Add(Me.txtCusCode)
        Me.Controls.Add(Me.Label55)
        Me.Controls.Add(Me.Label51)
        Me.Controls.Add(Me.Label50)
        Me.Controls.Add(Me.Label49)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.txtTaxId_cus)
        Me.Controls.Add(Me.txtCusContactPerson)
        Me.Controls.Add(Me.txtcus_phone2)
        Me.Controls.Add(Me.txtcus_tax_id)
        Me.Controls.Add(Me.txtcus_principal)
        Me.Controls.Add(Me.txtcus_memo)
        Me.Controls.Add(Me.txtcus_address)
        Me.Controls.Add(Me.txtCusPhone1)
        Me.Controls.Add(Me.txtCusName_cus)
        Me.Controls.Add(Me.txtcus_id)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.Label23)
        Me.Controls.Add(Me.Label24)
        Me.Controls.Add(Me.Label25)
        Me.Controls.Add(Me.Label26)
        Me.Controls.Add(Me.Label27)
        Me.Controls.Add(Me.Label29)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnCreate)
        Me.Controls.Add(Me.dgvCustomer)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "CustomerUserControl"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.Size = New System.Drawing.Size(1882, 949)
        Me.grpInsurance.ResumeLayout(False)
        Me.grpInsurance.PerformLayout()
        Me.grpPricePlan.ResumeLayout(False)
        Me.grpPricePlan.PerformLayout()
        Me.grpStock.ResumeLayout(False)
        Me.grpStock.PerformLayout()
        CType(Me.dgvCustomer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtInvoiceMemo As TextBox
    Friend WithEvents Label185 As Label
    Friend WithEvents cmbCompany_cus As ComboBox
    Friend WithEvents Label184 As Label
    Friend WithEvents grpInsurance As GroupBox
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents RadioButton3 As RadioButton
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents chkIsInsurance As CheckBox
    Friend WithEvents Label119 As Label
    Friend WithEvents grpPricePlan As GroupBox
    Friend WithEvents cmbPricePlan As ComboBox
    Friend WithEvents Label314 As Label
    Friend WithEvents lblNormalGas_cus As Label
    Friend WithEvents lblCGas_cus As Label
    Friend WithEvents txtcus_gas_c As TextBox
    Friend WithEvents txtcus_gas_normal As TextBox
    Friend WithEvents txtcus_gas_normal_deliver As TextBox
    Friend WithEvents lblNormalGasDeliver_cus As Label
    Friend WithEvents txtcus_gas_c_deliver As TextBox
    Friend WithEvents lblCGasDeliver_cus As Label
    Friend WithEvents grpStock As GroupBox
    Friend WithEvents Label209 As Label
    Friend WithEvents TextBox88 As TextBox
    Friend WithEvents Label210 As Label
    Friend WithEvents TextBox89 As TextBox
    Friend WithEvents Label278 As Label
    Friend WithEvents TextBox90 As TextBox
    Friend WithEvents Label201 As Label
    Friend WithEvents TextBox82 As TextBox
    Friend WithEvents Label207 As Label
    Friend WithEvents TextBox84 As TextBox
    Friend WithEvents Label208 As Label
    Friend WithEvents TextBox85 As TextBox
    Friend WithEvents Label200 As Label
    Friend WithEvents TextBox81 As TextBox
    Friend WithEvents Label199 As Label
    Friend WithEvents TextBox80 As TextBox
    Friend WithEvents Label198 As Label
    Friend WithEvents TextBox79 As TextBox
    Friend WithEvents Label54 As Label
    Friend WithEvents txtCusCode As TextBox
    Friend WithEvents Label55 As Label
    Friend WithEvents Label51 As Label
    Friend WithEvents Label50 As Label
    Friend WithEvents Label49 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents txtTaxId_cus As TextBox
    Friend WithEvents txtCusContactPerson As TextBox
    Friend WithEvents txtcus_phone2 As TextBox
    Friend WithEvents txtcus_tax_id As TextBox
    Friend WithEvents txtcus_principal As TextBox
    Friend WithEvents txtcus_memo As TextBox
    Friend WithEvents txtcus_address As TextBox
    Friend WithEvents txtCusPhone1 As TextBox
    Friend WithEvents txtCusName_cus As TextBox
    Friend WithEvents txtcus_id As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents Label25 As Label
    Friend WithEvents Label26 As Label
    Friend WithEvents Label27 As Label
    Friend WithEvents Label29 As Label
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnCreate As Button
    Friend WithEvents dgvCustomer As DataGridView
End Class
