<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucPurchase
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
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.cmbDriveCmp = New System.Windows.Forms.ComboBox()
        Me.Label136 = New System.Windows.Forms.Label()
        Me.chkSp = New System.Windows.Forms.CheckBox()
        Me.chkSpecial = New System.Windows.Forms.CheckBox()
        Me.Label308 = New System.Windows.Forms.Label()
        Me.Label307 = New System.Windows.Forms.Label()
        Me.Label305 = New System.Windows.Forms.Label()
        Me.txtDeliUnitPrice = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbGasVendor = New System.Windows.Forms.ComboBox()
        Me.cmbProduct = New System.Windows.Forms.ComboBox()
        Me.lblProduct = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.txtSum_pur = New System.Windows.Forms.TextBox()
        Me.txtFreight = New System.Windows.Forms.TextBox()
        Me.txtWeight_pur = New System.Windows.Forms.TextBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.txtId_pur = New System.Windows.Forms.TextBox()
        Me.txtUnitPrice_pur = New System.Windows.Forms.TextBox()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.cmbCompany_pur = New System.Windows.Forms.ComboBox()
        Me.lblCompany_pur = New System.Windows.Forms.Label()
        Me.dtpDate_pur = New System.Windows.Forms.DateTimePicker()
        Me.Label92 = New System.Windows.Forms.Label()
        Me.lblGasVendor_pur = New System.Windows.Forms.Label()
        Me.txtMemo_pur = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dgvPurchase = New System.Windows.Forms.DataGridView()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.dgvGasUnpaid = New System.Windows.Forms.DataGridView()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.dgvFreightUnpaid = New System.Windows.Forms.DataGridView()
        CType(Me.dgvPurchase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvGasUnpaid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgvFreightUnpaid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.Aqua
        Me.btnPrint.Location = New System.Drawing.Point(863, 383)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(154, 44)
        Me.btnPrint.TabIndex = 493
        Me.btnPrint.Text = "列  印"
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'cmbDriveCmp
        '
        Me.cmbDriveCmp.FormattingEnabled = True
        Me.cmbDriveCmp.Location = New System.Drawing.Point(418, 107)
        Me.cmbDriveCmp.Name = "cmbDriveCmp"
        Me.cmbDriveCmp.Size = New System.Drawing.Size(330, 27)
        Me.cmbDriveCmp.TabIndex = 492
        Me.cmbDriveCmp.Tag = "pur_DriveCmpId"
        '
        'Label136
        '
        Me.Label136.AutoSize = True
        Me.Label136.Location = New System.Drawing.Point(318, 111)
        Me.Label136.Name = "Label136"
        Me.Label136.Size = New System.Drawing.Size(93, 19)
        Me.Label136.TabIndex = 491
        Me.Label136.Text = "運輸公司"
        '
        'chkSp
        '
        Me.chkSp.Appearance = System.Windows.Forms.Appearance.Button
        Me.chkSp.AutoSize = True
        Me.chkSp.Location = New System.Drawing.Point(1010, 106)
        Me.chkSp.Name = "chkSp"
        Me.chkSp.Size = New System.Drawing.Size(40, 29)
        Me.chkSp.TabIndex = 490
        Me.chkSp.Tag = "pur_SpecialDUP"
        Me.chkSp.Text = "特"
        Me.chkSp.UseVisualStyleBackColor = True
        '
        'chkSpecial
        '
        Me.chkSpecial.Appearance = System.Windows.Forms.Appearance.Button
        Me.chkSpecial.AutoSize = True
        Me.chkSpecial.Location = New System.Drawing.Point(1010, 60)
        Me.chkSpecial.Name = "chkSpecial"
        Me.chkSpecial.Size = New System.Drawing.Size(40, 29)
        Me.chkSpecial.TabIndex = 489
        Me.chkSpecial.Tag = "pur_SpecialUP"
        Me.chkSpecial.Text = "特"
        Me.chkSpecial.UseVisualStyleBackColor = True
        '
        'Label308
        '
        Me.Label308.AutoSize = True
        Me.Label308.ForeColor = System.Drawing.Color.Red
        Me.Label308.Location = New System.Drawing.Point(753, 19)
        Me.Label308.Name = "Label308"
        Me.Label308.Size = New System.Drawing.Size(20, 19)
        Me.Label308.TabIndex = 487
        Me.Label308.Text = "*"
        '
        'Label307
        '
        Me.Label307.AutoSize = True
        Me.Label307.ForeColor = System.Drawing.Color.Red
        Me.Label307.Location = New System.Drawing.Point(1056, 19)
        Me.Label307.Name = "Label307"
        Me.Label307.Size = New System.Drawing.Size(20, 19)
        Me.Label307.TabIndex = 486
        Me.Label307.Text = "*"
        '
        'Label305
        '
        Me.Label305.AutoSize = True
        Me.Label305.ForeColor = System.Drawing.Color.Red
        Me.Label305.Location = New System.Drawing.Point(290, 19)
        Me.Label305.Name = "Label305"
        Me.Label305.Size = New System.Drawing.Size(20, 19)
        Me.Label305.TabIndex = 484
        Me.Label305.Text = "*"
        '
        'txtDeliUnitPrice
        '
        Me.txtDeliUnitPrice.Location = New System.Drawing.Point(880, 105)
        Me.txtDeliUnitPrice.Name = "txtDeliUnitPrice"
        Me.txtDeliUnitPrice.Size = New System.Drawing.Size(124, 30)
        Me.txtDeliUnitPrice.TabIndex = 483
        Me.txtDeliUnitPrice.Tag = "pur_deli_unit_price"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(781, 111)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(93, 19)
        Me.Label4.TabIndex = 482
        Me.Label4.Text = "運費單價"
        '
        'cmbGasVendor
        '
        Me.cmbGasVendor.FormattingEnabled = True
        Me.cmbGasVendor.Location = New System.Drawing.Point(418, 61)
        Me.cmbGasVendor.Name = "cmbGasVendor"
        Me.cmbGasVendor.Size = New System.Drawing.Size(329, 27)
        Me.cmbGasVendor.TabIndex = 481
        Me.cmbGasVendor.Tag = "pur_manu_id"
        '
        'cmbProduct
        '
        Me.cmbProduct.FormattingEnabled = True
        Me.cmbProduct.Items.AddRange(New Object() {"普氣", "丙氣"})
        Me.cmbProduct.Location = New System.Drawing.Point(880, 15)
        Me.cmbProduct.Name = "cmbProduct"
        Me.cmbProduct.Size = New System.Drawing.Size(170, 27)
        Me.cmbProduct.TabIndex = 480
        Me.cmbProduct.Tag = "pur_product"
        '
        'lblProduct
        '
        Me.lblProduct.AutoSize = True
        Me.lblProduct.Location = New System.Drawing.Point(779, 19)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.Size = New System.Drawing.Size(95, 19)
        Me.lblProduct.TabIndex = 479
        Me.lblProduct.Text = "產    品"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(1082, 19)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(95, 19)
        Me.Label28.TabIndex = 467
        Me.Label28.Text = "重量(KG)"
        '
        'txtSum_pur
        '
        Me.txtSum_pur.Location = New System.Drawing.Point(1183, 59)
        Me.txtSum_pur.Name = "txtSum_pur"
        Me.txtSum_pur.ReadOnly = True
        Me.txtSum_pur.Size = New System.Drawing.Size(187, 30)
        Me.txtSum_pur.TabIndex = 472
        Me.txtSum_pur.Tag = "pur_price"
        '
        'txtFreight
        '
        Me.txtFreight.Location = New System.Drawing.Point(1183, 105)
        Me.txtFreight.Name = "txtFreight"
        Me.txtFreight.ReadOnly = True
        Me.txtFreight.Size = New System.Drawing.Size(187, 30)
        Me.txtFreight.TabIndex = 478
        Me.txtFreight.Tag = "pur_delivery_fee"
        '
        'txtWeight_pur
        '
        Me.txtWeight_pur.Location = New System.Drawing.Point(1183, 13)
        Me.txtWeight_pur.Name = "txtWeight_pur"
        Me.txtWeight_pur.Size = New System.Drawing.Size(187, 30)
        Me.txtWeight_pur.TabIndex = 468
        Me.txtWeight_pur.Tag = "pur_quantity"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(1082, 111)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(95, 19)
        Me.Label31.TabIndex = 477
        Me.Label31.Text = "運    費"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(1083, 65)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(94, 19)
        Me.Label35.TabIndex = 471
        Me.Label35.Text = "總 金 額"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(779, 65)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(95, 19)
        Me.Label37.TabIndex = 469
        Me.Label37.Text = "單    價"
        '
        'txtId_pur
        '
        Me.txtId_pur.Location = New System.Drawing.Point(114, 13)
        Me.txtId_pur.Name = "txtId_pur"
        Me.txtId_pur.ReadOnly = True
        Me.txtId_pur.Size = New System.Drawing.Size(170, 30)
        Me.txtId_pur.TabIndex = 476
        Me.txtId_pur.Tag = "pur_id"
        '
        'txtUnitPrice_pur
        '
        Me.txtUnitPrice_pur.Location = New System.Drawing.Point(880, 59)
        Me.txtUnitPrice_pur.Name = "txtUnitPrice_pur"
        Me.txtUnitPrice_pur.Size = New System.Drawing.Size(124, 30)
        Me.txtUnitPrice_pur.TabIndex = 470
        Me.txtUnitPrice_pur.Tag = "pur_unit_price"
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(13, 19)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(95, 19)
        Me.Label38.TabIndex = 475
        Me.Label38.Text = "編    號"
        '
        'cmbCompany_pur
        '
        Me.cmbCompany_pur.FormattingEnabled = True
        Me.cmbCompany_pur.Location = New System.Drawing.Point(417, 15)
        Me.cmbCompany_pur.Name = "cmbCompany_pur"
        Me.cmbCompany_pur.Size = New System.Drawing.Size(330, 27)
        Me.cmbCompany_pur.TabIndex = 474
        Me.cmbCompany_pur.Tag = "pur_comp_id"
        '
        'lblCompany_pur
        '
        Me.lblCompany_pur.AutoSize = True
        Me.lblCompany_pur.Location = New System.Drawing.Point(316, 18)
        Me.lblCompany_pur.Name = "lblCompany_pur"
        Me.lblCompany_pur.Size = New System.Drawing.Size(95, 19)
        Me.lblCompany_pur.TabIndex = 473
        Me.lblCompany_pur.Text = "公    司"
        '
        'dtpDate_pur
        '
        Me.dtpDate_pur.CustomFormat = "yyyy/MM/dd"
        Me.dtpDate_pur.Location = New System.Drawing.Point(114, 59)
        Me.dtpDate_pur.Name = "dtpDate_pur"
        Me.dtpDate_pur.Size = New System.Drawing.Size(170, 30)
        Me.dtpDate_pur.TabIndex = 466
        Me.dtpDate_pur.Tag = "pur_date"
        '
        'Label92
        '
        Me.Label92.AutoSize = True
        Me.Label92.Location = New System.Drawing.Point(13, 65)
        Me.Label92.Name = "Label92"
        Me.Label92.Size = New System.Drawing.Size(95, 19)
        Me.Label92.TabIndex = 465
        Me.Label92.Text = "日    期"
        '
        'lblGasVendor_pur
        '
        Me.lblGasVendor_pur.AutoSize = True
        Me.lblGasVendor_pur.Location = New System.Drawing.Point(319, 65)
        Me.lblGasVendor_pur.Name = "lblGasVendor_pur"
        Me.lblGasVendor_pur.Size = New System.Drawing.Size(93, 19)
        Me.lblGasVendor_pur.TabIndex = 464
        Me.lblGasVendor_pur.Text = "大氣廠商"
        '
        'txtMemo_pur
        '
        Me.txtMemo_pur.Location = New System.Drawing.Point(114, 105)
        Me.txtMemo_pur.Name = "txtMemo_pur"
        Me.txtMemo_pur.Size = New System.Drawing.Size(170, 30)
        Me.txtMemo_pur.TabIndex = 463
        Me.txtMemo_pur.Tag = "pur_Memo"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 111)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 19)
        Me.Label3.TabIndex = 462
        Me.Label3.Text = "備    註"
        '
        'dgvPurchase
        '
        Me.dgvPurchase.AllowUserToAddRows = False
        Me.dgvPurchase.AllowUserToDeleteRows = False
        Me.dgvPurchase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPurchase.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvPurchase.Location = New System.Drawing.Point(10, 459)
        Me.dgvPurchase.Name = "dgvPurchase"
        Me.dgvPurchase.ReadOnly = True
        Me.dgvPurchase.RowTemplate.Height = 24
        Me.dgvPurchase.Size = New System.Drawing.Size(1862, 480)
        Me.dgvPurchase.TabIndex = 461
        '
        'btnSearch
        '
        Me.btnSearch.BackColor = System.Drawing.Color.Lime
        Me.btnSearch.Location = New System.Drawing.Point(693, 383)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(154, 44)
        Me.btnSearch.TabIndex = 460
        Me.btnSearch.Text = "查  詢"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel.Location = New System.Drawing.Point(523, 383)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(154, 44)
        Me.btnCancel.TabIndex = 459
        Me.btnCancel.Text = "取  消"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete.Location = New System.Drawing.Point(353, 383)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(154, 44)
        Me.btnDelete.TabIndex = 458
        Me.btnDelete.Text = "刪  除"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit.Location = New System.Drawing.Point(183, 383)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(154, 44)
        Me.btnEdit.TabIndex = 457
        Me.btnEdit.Text = "修  改"
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnAdd.Location = New System.Drawing.Point(13, 383)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(154, 44)
        Me.btnAdd.TabIndex = 456
        Me.btnAdd.Tag = ""
        Me.btnAdd.Text = "新  增"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'dgvGasUnpaid
        '
        Me.dgvGasUnpaid.AllowUserToAddRows = False
        Me.dgvGasUnpaid.AllowUserToDeleteRows = False
        Me.dgvGasUnpaid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvGasUnpaid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvGasUnpaid.Location = New System.Drawing.Point(3, 26)
        Me.dgvGasUnpaid.Name = "dgvGasUnpaid"
        Me.dgvGasUnpaid.ReadOnly = True
        Me.dgvGasUnpaid.RowTemplate.Height = 24
        Me.dgvGasUnpaid.Size = New System.Drawing.Size(1044, 207)
        Me.dgvGasUnpaid.TabIndex = 494
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.dgvGasUnpaid)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 141)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1050, 236)
        Me.GroupBox1.TabIndex = 495
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "大氣應付未付"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.dgvFreightUnpaid)
        Me.GroupBox2.Location = New System.Drawing.Point(1069, 141)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(800, 236)
        Me.GroupBox2.TabIndex = 496
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "運費應付未付"
        '
        'dgvFreightUnpaid
        '
        Me.dgvFreightUnpaid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFreightUnpaid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvFreightUnpaid.Location = New System.Drawing.Point(3, 26)
        Me.dgvFreightUnpaid.Name = "dgvFreightUnpaid"
        Me.dgvFreightUnpaid.ReadOnly = True
        Me.dgvFreightUnpaid.RowTemplate.Height = 24
        Me.dgvFreightUnpaid.Size = New System.Drawing.Size(794, 207)
        Me.dgvFreightUnpaid.TabIndex = 494
        '
        'GasPurchaseUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.cmbDriveCmp)
        Me.Controls.Add(Me.Label136)
        Me.Controls.Add(Me.chkSp)
        Me.Controls.Add(Me.chkSpecial)
        Me.Controls.Add(Me.Label308)
        Me.Controls.Add(Me.Label307)
        Me.Controls.Add(Me.Label305)
        Me.Controls.Add(Me.txtDeliUnitPrice)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbGasVendor)
        Me.Controls.Add(Me.cmbProduct)
        Me.Controls.Add(Me.lblProduct)
        Me.Controls.Add(Me.Label28)
        Me.Controls.Add(Me.txtSum_pur)
        Me.Controls.Add(Me.txtFreight)
        Me.Controls.Add(Me.txtWeight_pur)
        Me.Controls.Add(Me.Label31)
        Me.Controls.Add(Me.Label35)
        Me.Controls.Add(Me.Label37)
        Me.Controls.Add(Me.txtId_pur)
        Me.Controls.Add(Me.txtUnitPrice_pur)
        Me.Controls.Add(Me.Label38)
        Me.Controls.Add(Me.cmbCompany_pur)
        Me.Controls.Add(Me.lblCompany_pur)
        Me.Controls.Add(Me.dtpDate_pur)
        Me.Controls.Add(Me.Label92)
        Me.Controls.Add(Me.lblGasVendor_pur)
        Me.Controls.Add(Me.txtMemo_pur)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.dgvPurchase)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnAdd)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "GasPurchaseUserControl"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.Size = New System.Drawing.Size(1882, 949)
        CType(Me.dgvPurchase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvGasUnpaid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.dgvFreightUnpaid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnPrint As Button
    Friend WithEvents cmbDriveCmp As ComboBox
    Friend WithEvents Label136 As Label
    Friend WithEvents chkSp As CheckBox
    Friend WithEvents chkSpecial As CheckBox
    Friend WithEvents Label308 As Label
    Friend WithEvents Label307 As Label
    Friend WithEvents Label305 As Label
    Friend WithEvents txtDeliUnitPrice As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents cmbGasVendor As ComboBox
    Friend WithEvents cmbProduct As ComboBox
    Friend WithEvents lblProduct As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents txtSum_pur As TextBox
    Friend WithEvents txtFreight As TextBox
    Friend WithEvents txtWeight_pur As TextBox
    Friend WithEvents Label31 As Label
    Friend WithEvents Label35 As Label
    Friend WithEvents Label37 As Label
    Friend WithEvents txtId_pur As TextBox
    Friend WithEvents txtUnitPrice_pur As TextBox
    Friend WithEvents Label38 As Label
    Friend WithEvents cmbCompany_pur As ComboBox
    Friend WithEvents lblCompany_pur As Label
    Friend WithEvents dtpDate_pur As DateTimePicker
    Friend WithEvents Label92 As Label
    Friend WithEvents lblGasVendor_pur As Label
    Friend WithEvents txtMemo_pur As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents dgvPurchase As DataGridView
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents dgvGasUnpaid As DataGridView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents dgvFreightUnpaid As DataGridView
End Class
