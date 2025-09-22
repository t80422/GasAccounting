<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PurchaseBarrelUserControl
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
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.cmbCompany = New System.Windows.Forms.ComboBox()
        Me.Label248 = New System.Windows.Forms.Label()
        Me.dtpDate_pb = New System.Windows.Forms.DateTimePicker()
        Me.Label178 = New System.Windows.Forms.Label()
        Me.dgvPurchaseBarrel = New System.Windows.Forms.DataGridView()
        Me.btnQuery_pb = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.txtAmount_pb = New System.Windows.Forms.TextBox()
        Me.Label177 = New System.Windows.Forms.Label()
        Me.grpBarrel = New System.Windows.Forms.GroupBox()
        Me.txtUnitPrice_2 = New System.Windows.Forms.TextBox()
        Me.txtUnitPrice_5 = New System.Windows.Forms.TextBox()
        Me.txtUnitPrice_14 = New System.Windows.Forms.TextBox()
        Me.txtUnitPrice_15 = New System.Windows.Forms.TextBox()
        Me.txtUnitPrice_4 = New System.Windows.Forms.TextBox()
        Me.txtUnitPrice_10 = New System.Windows.Forms.TextBox()
        Me.txtUnitPrice_16 = New System.Windows.Forms.TextBox()
        Me.txtUnitPrice_20 = New System.Windows.Forms.TextBox()
        Me.txtUnitPrice_50 = New System.Windows.Forms.TextBox()
        Me.txtQty_2 = New System.Windows.Forms.TextBox()
        Me.txtQty_5 = New System.Windows.Forms.TextBox()
        Me.txtQty_14 = New System.Windows.Forms.TextBox()
        Me.txtQty_15 = New System.Windows.Forms.TextBox()
        Me.txtQty_4 = New System.Windows.Forms.TextBox()
        Me.txtQty_10 = New System.Windows.Forms.TextBox()
        Me.txtQty_16 = New System.Windows.Forms.TextBox()
        Me.txtQty_20 = New System.Windows.Forms.TextBox()
        Me.txtQty_50 = New System.Windows.Forms.TextBox()
        Me.Label176 = New System.Windows.Forms.Label()
        Me.Label175 = New System.Windows.Forms.Label()
        Me.Label174 = New System.Windows.Forms.Label()
        Me.Label173 = New System.Windows.Forms.Label()
        Me.Label172 = New System.Windows.Forms.Label()
        Me.Label171 = New System.Windows.Forms.Label()
        Me.Label170 = New System.Windows.Forms.Label()
        Me.Label169 = New System.Windows.Forms.Label()
        Me.Label168 = New System.Windows.Forms.Label()
        Me.Label167 = New System.Windows.Forms.Label()
        Me.Label163 = New System.Windows.Forms.Label()
        Me.cmbVendor = New System.Windows.Forms.ComboBox()
        Me.txtId_pb = New System.Windows.Forms.TextBox()
        Me.lblVendor_pb = New System.Windows.Forms.Label()
        CType(Me.dgvPurchaseBarrel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpBarrel.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.Cyan
        Me.btnPrint.Location = New System.Drawing.Point(793, 160)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(140, 44)
        Me.btnPrint.TabIndex = 446
        Me.btnPrint.Text = "列  印"
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'cmbCompany
        '
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.Location = New System.Drawing.Point(114, 56)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(485, 27)
        Me.cmbCompany.TabIndex = 445
        Me.cmbCompany.Tag = "pb_comp_Id"
        '
        'Label248
        '
        Me.Label248.AutoSize = True
        Me.Label248.Location = New System.Drawing.Point(13, 60)
        Me.Label248.Name = "Label248"
        Me.Label248.Size = New System.Drawing.Size(95, 19)
        Me.Label248.TabIndex = 444
        Me.Label248.Text = "公    司"
        '
        'dtpDate_pb
        '
        Me.dtpDate_pb.Location = New System.Drawing.Point(114, 99)
        Me.dtpDate_pb.Name = "dtpDate_pb"
        Me.dtpDate_pb.Size = New System.Drawing.Size(189, 30)
        Me.dtpDate_pb.TabIndex = 442
        Me.dtpDate_pb.Tag = "pb_Date"
        '
        'Label178
        '
        Me.Label178.AutoSize = True
        Me.Label178.Location = New System.Drawing.Point(13, 105)
        Me.Label178.Name = "Label178"
        Me.Label178.Size = New System.Drawing.Size(95, 19)
        Me.Label178.TabIndex = 441
        Me.Label178.Text = "日    期"
        '
        'dgvPurchaseBarrel
        '
        Me.dgvPurchaseBarrel.AllowUserToAddRows = False
        Me.dgvPurchaseBarrel.AllowUserToDeleteRows = False
        Me.dgvPurchaseBarrel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPurchaseBarrel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvPurchaseBarrel.Location = New System.Drawing.Point(10, 229)
        Me.dgvPurchaseBarrel.Name = "dgvPurchaseBarrel"
        Me.dgvPurchaseBarrel.ReadOnly = True
        Me.dgvPurchaseBarrel.RowTemplate.Height = 24
        Me.dgvPurchaseBarrel.Size = New System.Drawing.Size(1862, 710)
        Me.dgvPurchaseBarrel.TabIndex = 440
        '
        'btnQuery_pb
        '
        Me.btnQuery_pb.BackColor = System.Drawing.Color.Lime
        Me.btnQuery_pb.Location = New System.Drawing.Point(637, 160)
        Me.btnQuery_pb.Name = "btnQuery_pb"
        Me.btnQuery_pb.Size = New System.Drawing.Size(140, 44)
        Me.btnQuery_pb.TabIndex = 439
        Me.btnQuery_pb.Text = "查  詢"
        Me.btnQuery_pb.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel.Location = New System.Drawing.Point(481, 160)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel.TabIndex = 438
        Me.btnCancel.Text = "取  消"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete.Location = New System.Drawing.Point(325, 160)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete.TabIndex = 437
        Me.btnDelete.Text = "刪  除"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit.Location = New System.Drawing.Point(169, 160)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(140, 44)
        Me.btnEdit.TabIndex = 436
        Me.btnEdit.Text = "修  改"
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnAdd.Location = New System.Drawing.Point(13, 160)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(140, 44)
        Me.btnAdd.TabIndex = 435
        Me.btnAdd.Tag = ""
        Me.btnAdd.Text = "新  增"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'txtAmount_pb
        '
        Me.txtAmount_pb.Location = New System.Drawing.Point(410, 99)
        Me.txtAmount_pb.Name = "txtAmount_pb"
        Me.txtAmount_pb.ReadOnly = True
        Me.txtAmount_pb.Size = New System.Drawing.Size(189, 30)
        Me.txtAmount_pb.TabIndex = 434
        Me.txtAmount_pb.Tag = "pb_Amount"
        '
        'Label177
        '
        Me.Label177.AutoSize = True
        Me.Label177.Location = New System.Drawing.Point(309, 105)
        Me.Label177.Name = "Label177"
        Me.Label177.Size = New System.Drawing.Size(95, 19)
        Me.Label177.TabIndex = 433
        Me.Label177.Text = "總    計"
        '
        'grpBarrel
        '
        Me.grpBarrel.Controls.Add(Me.txtUnitPrice_2)
        Me.grpBarrel.Controls.Add(Me.txtUnitPrice_5)
        Me.grpBarrel.Controls.Add(Me.txtUnitPrice_14)
        Me.grpBarrel.Controls.Add(Me.txtUnitPrice_15)
        Me.grpBarrel.Controls.Add(Me.txtUnitPrice_4)
        Me.grpBarrel.Controls.Add(Me.txtUnitPrice_10)
        Me.grpBarrel.Controls.Add(Me.txtUnitPrice_16)
        Me.grpBarrel.Controls.Add(Me.txtUnitPrice_20)
        Me.grpBarrel.Controls.Add(Me.txtUnitPrice_50)
        Me.grpBarrel.Controls.Add(Me.txtQty_2)
        Me.grpBarrel.Controls.Add(Me.txtQty_5)
        Me.grpBarrel.Controls.Add(Me.txtQty_14)
        Me.grpBarrel.Controls.Add(Me.txtQty_15)
        Me.grpBarrel.Controls.Add(Me.txtQty_4)
        Me.grpBarrel.Controls.Add(Me.txtQty_10)
        Me.grpBarrel.Controls.Add(Me.txtQty_16)
        Me.grpBarrel.Controls.Add(Me.txtQty_20)
        Me.grpBarrel.Controls.Add(Me.txtQty_50)
        Me.grpBarrel.Controls.Add(Me.Label176)
        Me.grpBarrel.Controls.Add(Me.Label175)
        Me.grpBarrel.Controls.Add(Me.Label174)
        Me.grpBarrel.Controls.Add(Me.Label173)
        Me.grpBarrel.Controls.Add(Me.Label172)
        Me.grpBarrel.Controls.Add(Me.Label171)
        Me.grpBarrel.Controls.Add(Me.Label170)
        Me.grpBarrel.Controls.Add(Me.Label169)
        Me.grpBarrel.Controls.Add(Me.Label168)
        Me.grpBarrel.Controls.Add(Me.Label167)
        Me.grpBarrel.Controls.Add(Me.Label163)
        Me.grpBarrel.Location = New System.Drawing.Point(607, 15)
        Me.grpBarrel.Margin = New System.Windows.Forms.Padding(5)
        Me.grpBarrel.Name = "grpBarrel"
        Me.grpBarrel.Padding = New System.Windows.Forms.Padding(5)
        Me.grpBarrel.Size = New System.Drawing.Size(657, 137)
        Me.grpBarrel.TabIndex = 432
        Me.grpBarrel.TabStop = False
        Me.grpBarrel.Text = "瓦斯桶"
        '
        'txtUnitPrice_2
        '
        Me.txtUnitPrice_2.Location = New System.Drawing.Point(585, 96)
        Me.txtUnitPrice_2.Name = "txtUnitPrice_2"
        Me.txtUnitPrice_2.Size = New System.Drawing.Size(49, 30)
        Me.txtUnitPrice_2.TabIndex = 29
        Me.txtUnitPrice_2.Tag = "pb_UnitPrice_2"
        '
        'txtUnitPrice_5
        '
        Me.txtUnitPrice_5.Location = New System.Drawing.Point(520, 96)
        Me.txtUnitPrice_5.Name = "txtUnitPrice_5"
        Me.txtUnitPrice_5.Size = New System.Drawing.Size(49, 30)
        Me.txtUnitPrice_5.TabIndex = 28
        Me.txtUnitPrice_5.Tag = "pb_UnitPrice_5"
        '
        'txtUnitPrice_14
        '
        Me.txtUnitPrice_14.Location = New System.Drawing.Point(455, 96)
        Me.txtUnitPrice_14.Name = "txtUnitPrice_14"
        Me.txtUnitPrice_14.Size = New System.Drawing.Size(49, 30)
        Me.txtUnitPrice_14.TabIndex = 27
        Me.txtUnitPrice_14.Tag = "pb_UnitPrice_14"
        '
        'txtUnitPrice_15
        '
        Me.txtUnitPrice_15.Location = New System.Drawing.Point(390, 96)
        Me.txtUnitPrice_15.Name = "txtUnitPrice_15"
        Me.txtUnitPrice_15.Size = New System.Drawing.Size(49, 30)
        Me.txtUnitPrice_15.TabIndex = 26
        Me.txtUnitPrice_15.Tag = "pb_UnitPrice_15"
        '
        'txtUnitPrice_4
        '
        Me.txtUnitPrice_4.Location = New System.Drawing.Point(325, 96)
        Me.txtUnitPrice_4.Name = "txtUnitPrice_4"
        Me.txtUnitPrice_4.Size = New System.Drawing.Size(49, 30)
        Me.txtUnitPrice_4.TabIndex = 25
        Me.txtUnitPrice_4.Tag = "pb_UnitPrice_4"
        '
        'txtUnitPrice_10
        '
        Me.txtUnitPrice_10.Location = New System.Drawing.Point(260, 96)
        Me.txtUnitPrice_10.Name = "txtUnitPrice_10"
        Me.txtUnitPrice_10.Size = New System.Drawing.Size(49, 30)
        Me.txtUnitPrice_10.TabIndex = 24
        Me.txtUnitPrice_10.Tag = "pb_UnitPrice_10"
        '
        'txtUnitPrice_16
        '
        Me.txtUnitPrice_16.Location = New System.Drawing.Point(195, 96)
        Me.txtUnitPrice_16.Name = "txtUnitPrice_16"
        Me.txtUnitPrice_16.Size = New System.Drawing.Size(49, 30)
        Me.txtUnitPrice_16.TabIndex = 23
        Me.txtUnitPrice_16.Tag = "pb_UnitPrice_16"
        '
        'txtUnitPrice_20
        '
        Me.txtUnitPrice_20.Location = New System.Drawing.Point(130, 96)
        Me.txtUnitPrice_20.Name = "txtUnitPrice_20"
        Me.txtUnitPrice_20.Size = New System.Drawing.Size(49, 30)
        Me.txtUnitPrice_20.TabIndex = 22
        Me.txtUnitPrice_20.Tag = "pb_UnitPrice_20"
        '
        'txtUnitPrice_50
        '
        Me.txtUnitPrice_50.Location = New System.Drawing.Point(65, 96)
        Me.txtUnitPrice_50.Name = "txtUnitPrice_50"
        Me.txtUnitPrice_50.Size = New System.Drawing.Size(49, 30)
        Me.txtUnitPrice_50.TabIndex = 21
        Me.txtUnitPrice_50.Tag = "pb_UnitPrice_50"
        '
        'txtQty_2
        '
        Me.txtQty_2.Location = New System.Drawing.Point(585, 50)
        Me.txtQty_2.Name = "txtQty_2"
        Me.txtQty_2.Size = New System.Drawing.Size(49, 30)
        Me.txtQty_2.TabIndex = 20
        Me.txtQty_2.Tag = "pb_Qty_2"
        '
        'txtQty_5
        '
        Me.txtQty_5.Location = New System.Drawing.Point(520, 50)
        Me.txtQty_5.Name = "txtQty_5"
        Me.txtQty_5.Size = New System.Drawing.Size(49, 30)
        Me.txtQty_5.TabIndex = 19
        Me.txtQty_5.Tag = "pb_Qty_5"
        '
        'txtQty_14
        '
        Me.txtQty_14.Location = New System.Drawing.Point(455, 50)
        Me.txtQty_14.Name = "txtQty_14"
        Me.txtQty_14.Size = New System.Drawing.Size(49, 30)
        Me.txtQty_14.TabIndex = 18
        Me.txtQty_14.Tag = "pb_Qty_14"
        '
        'txtQty_15
        '
        Me.txtQty_15.Location = New System.Drawing.Point(390, 50)
        Me.txtQty_15.Name = "txtQty_15"
        Me.txtQty_15.Size = New System.Drawing.Size(49, 30)
        Me.txtQty_15.TabIndex = 17
        Me.txtQty_15.Tag = "pb_Qty_15"
        '
        'txtQty_4
        '
        Me.txtQty_4.Location = New System.Drawing.Point(325, 50)
        Me.txtQty_4.Name = "txtQty_4"
        Me.txtQty_4.Size = New System.Drawing.Size(49, 30)
        Me.txtQty_4.TabIndex = 16
        Me.txtQty_4.Tag = "pb_Qty_4"
        '
        'txtQty_10
        '
        Me.txtQty_10.Location = New System.Drawing.Point(260, 50)
        Me.txtQty_10.Name = "txtQty_10"
        Me.txtQty_10.Size = New System.Drawing.Size(49, 30)
        Me.txtQty_10.TabIndex = 15
        Me.txtQty_10.Tag = "pb_Qty_10"
        '
        'txtQty_16
        '
        Me.txtQty_16.Location = New System.Drawing.Point(195, 50)
        Me.txtQty_16.Name = "txtQty_16"
        Me.txtQty_16.Size = New System.Drawing.Size(49, 30)
        Me.txtQty_16.TabIndex = 14
        Me.txtQty_16.Tag = "pb_Qty_16"
        '
        'txtQty_20
        '
        Me.txtQty_20.Location = New System.Drawing.Point(130, 50)
        Me.txtQty_20.Name = "txtQty_20"
        Me.txtQty_20.Size = New System.Drawing.Size(49, 30)
        Me.txtQty_20.TabIndex = 13
        Me.txtQty_20.Tag = "pb_Qty_20"
        '
        'txtQty_50
        '
        Me.txtQty_50.Location = New System.Drawing.Point(65, 50)
        Me.txtQty_50.Name = "txtQty_50"
        Me.txtQty_50.Size = New System.Drawing.Size(49, 30)
        Me.txtQty_50.TabIndex = 12
        Me.txtQty_50.Tag = "pb_Qty_50"
        '
        'Label176
        '
        Me.Label176.Location = New System.Drawing.Point(581, 28)
        Me.Label176.Name = "Label176"
        Me.Label176.Size = New System.Drawing.Size(53, 19)
        Me.Label176.TabIndex = 11
        Me.Label176.Text = "2Kg"
        Me.Label176.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label175
        '
        Me.Label175.Location = New System.Drawing.Point(516, 28)
        Me.Label175.Name = "Label175"
        Me.Label175.Size = New System.Drawing.Size(53, 19)
        Me.Label175.TabIndex = 10
        Me.Label175.Text = "5Kg"
        Me.Label175.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label174
        '
        Me.Label174.Location = New System.Drawing.Point(451, 28)
        Me.Label174.Name = "Label174"
        Me.Label174.Size = New System.Drawing.Size(53, 19)
        Me.Label174.TabIndex = 9
        Me.Label174.Text = "14Kg"
        Me.Label174.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label173
        '
        Me.Label173.Location = New System.Drawing.Point(386, 28)
        Me.Label173.Name = "Label173"
        Me.Label173.Size = New System.Drawing.Size(53, 19)
        Me.Label173.TabIndex = 8
        Me.Label173.Text = "18Kg"
        Me.Label173.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label172
        '
        Me.Label172.Location = New System.Drawing.Point(321, 28)
        Me.Label172.Name = "Label172"
        Me.Label172.Size = New System.Drawing.Size(53, 19)
        Me.Label172.TabIndex = 7
        Me.Label172.Text = "4Kg"
        Me.Label172.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label171
        '
        Me.Label171.Location = New System.Drawing.Point(256, 28)
        Me.Label171.Name = "Label171"
        Me.Label171.Size = New System.Drawing.Size(53, 19)
        Me.Label171.TabIndex = 6
        Me.Label171.Text = "10Kg"
        Me.Label171.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label170
        '
        Me.Label170.Location = New System.Drawing.Point(191, 28)
        Me.Label170.Name = "Label170"
        Me.Label170.Size = New System.Drawing.Size(53, 19)
        Me.Label170.TabIndex = 5
        Me.Label170.Text = "16Kg"
        Me.Label170.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label169
        '
        Me.Label169.Location = New System.Drawing.Point(126, 28)
        Me.Label169.Name = "Label169"
        Me.Label169.Size = New System.Drawing.Size(53, 19)
        Me.Label169.TabIndex = 4
        Me.Label169.Text = "20Kg"
        Me.Label169.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label168
        '
        Me.Label168.Location = New System.Drawing.Point(61, 28)
        Me.Label168.Name = "Label168"
        Me.Label168.Size = New System.Drawing.Size(53, 19)
        Me.Label168.TabIndex = 3
        Me.Label168.Text = "50Kg"
        Me.Label168.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label167
        '
        Me.Label167.AutoSize = True
        Me.Label167.Location = New System.Drawing.Point(8, 99)
        Me.Label167.Name = "Label167"
        Me.Label167.Size = New System.Drawing.Size(51, 19)
        Me.Label167.TabIndex = 2
        Me.Label167.Text = "單價"
        '
        'Label163
        '
        Me.Label163.AutoSize = True
        Me.Label163.Location = New System.Drawing.Point(8, 53)
        Me.Label163.Name = "Label163"
        Me.Label163.Size = New System.Drawing.Size(51, 19)
        Me.Label163.TabIndex = 1
        Me.Label163.Text = "數量"
        '
        'cmbVendor
        '
        Me.cmbVendor.FormattingEnabled = True
        Me.cmbVendor.Location = New System.Drawing.Point(114, 13)
        Me.cmbVendor.Name = "cmbVendor"
        Me.cmbVendor.Size = New System.Drawing.Size(485, 27)
        Me.cmbVendor.TabIndex = 431
        Me.cmbVendor.Tag = "pb_manu_Id"
        '
        'txtId_pb
        '
        Me.txtId_pb.Location = New System.Drawing.Point(1272, 122)
        Me.txtId_pb.Name = "txtId_pb"
        Me.txtId_pb.ReadOnly = True
        Me.txtId_pb.Size = New System.Drawing.Size(121, 30)
        Me.txtId_pb.TabIndex = 430
        Me.txtId_pb.Tag = "pb_Id"
        Me.txtId_pb.Visible = False
        '
        'lblVendor_pb
        '
        Me.lblVendor_pb.AutoSize = True
        Me.lblVendor_pb.Location = New System.Drawing.Point(13, 17)
        Me.lblVendor_pb.Name = "lblVendor_pb"
        Me.lblVendor_pb.Size = New System.Drawing.Size(95, 19)
        Me.lblVendor_pb.TabIndex = 429
        Me.lblVendor_pb.Text = "廠    商"
        '
        'PurchaseBarrelUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.cmbCompany)
        Me.Controls.Add(Me.Label248)
        Me.Controls.Add(Me.dtpDate_pb)
        Me.Controls.Add(Me.Label178)
        Me.Controls.Add(Me.dgvPurchaseBarrel)
        Me.Controls.Add(Me.btnQuery_pb)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.txtAmount_pb)
        Me.Controls.Add(Me.Label177)
        Me.Controls.Add(Me.grpBarrel)
        Me.Controls.Add(Me.cmbVendor)
        Me.Controls.Add(Me.txtId_pb)
        Me.Controls.Add(Me.lblVendor_pb)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "PurchaseBarrelUserControl"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.Size = New System.Drawing.Size(1882, 949)
        CType(Me.dgvPurchaseBarrel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpBarrel.ResumeLayout(False)
        Me.grpBarrel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnPrint As Button
    Friend WithEvents cmbCompany As ComboBox
    Friend WithEvents Label248 As Label
    Friend WithEvents dtpDate_pb As DateTimePicker
    Friend WithEvents Label178 As Label
    Friend WithEvents dgvPurchaseBarrel As DataGridView
    Friend WithEvents btnQuery_pb As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents txtAmount_pb As TextBox
    Friend WithEvents Label177 As Label
    Friend WithEvents grpBarrel As GroupBox
    Friend WithEvents txtUnitPrice_2 As TextBox
    Friend WithEvents txtUnitPrice_5 As TextBox
    Friend WithEvents txtUnitPrice_14 As TextBox
    Friend WithEvents txtUnitPrice_15 As TextBox
    Friend WithEvents txtUnitPrice_4 As TextBox
    Friend WithEvents txtUnitPrice_10 As TextBox
    Friend WithEvents txtUnitPrice_16 As TextBox
    Friend WithEvents txtUnitPrice_20 As TextBox
    Friend WithEvents txtUnitPrice_50 As TextBox
    Friend WithEvents txtQty_2 As TextBox
    Friend WithEvents txtQty_5 As TextBox
    Friend WithEvents txtQty_14 As TextBox
    Friend WithEvents txtQty_15 As TextBox
    Friend WithEvents txtQty_4 As TextBox
    Friend WithEvents txtQty_10 As TextBox
    Friend WithEvents txtQty_16 As TextBox
    Friend WithEvents txtQty_20 As TextBox
    Friend WithEvents txtQty_50 As TextBox
    Friend WithEvents Label176 As Label
    Friend WithEvents Label175 As Label
    Friend WithEvents Label174 As Label
    Friend WithEvents Label173 As Label
    Friend WithEvents Label172 As Label
    Friend WithEvents Label171 As Label
    Friend WithEvents Label170 As Label
    Friend WithEvents Label169 As Label
    Friend WithEvents Label168 As Label
    Friend WithEvents Label167 As Label
    Friend WithEvents Label163 As Label
    Friend WithEvents cmbVendor As ComboBox
    Friend WithEvents txtId_pb As TextBox
    Friend WithEvents lblVendor_pb As Label
End Class
