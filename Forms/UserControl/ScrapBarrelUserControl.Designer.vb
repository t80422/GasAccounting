<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScrapBarrelUserControl
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
        Me.grpSBD = New System.Windows.Forms.GroupBox()
        Me.dgvSBD = New System.Windows.Forms.DataGridView()
        Me.btnCreate_sbd = New System.Windows.Forms.Button()
        Me.btnEdit_sbd = New System.Windows.Forms.Button()
        Me.btnDelete_sbd = New System.Windows.Forms.Button()
        Me.btnCancel_sbd = New System.Windows.Forms.Button()
        Me.grpPrice_sbd = New System.Windows.Forms.GroupBox()
        Me.txtAcquisitions10_sbd = New System.Windows.Forms.TextBox()
        Me.txtAcquisitions16_sbd = New System.Windows.Forms.TextBox()
        Me.txtAcquisitions4_sbd = New System.Windows.Forms.TextBox()
        Me.txtAcquisitions20_sbd = New System.Windows.Forms.TextBox()
        Me.txtAcquisitions50_sbd = New System.Windows.Forms.TextBox()
        Me.Label328 = New System.Windows.Forms.Label()
        Me.txtBuy10_sbd = New System.Windows.Forms.TextBox()
        Me.txtBuy16_sbd = New System.Windows.Forms.TextBox()
        Me.txtQty4_sbd = New System.Windows.Forms.TextBox()
        Me.txtQty10_sbd = New System.Windows.Forms.TextBox()
        Me.txtQty16_sbd = New System.Windows.Forms.TextBox()
        Me.txtBuy4_sbd = New System.Windows.Forms.TextBox()
        Me.txtBuy20_sbd = New System.Windows.Forms.TextBox()
        Me.txtBuy50_sbd = New System.Windows.Forms.TextBox()
        Me.txtQty20_sbd = New System.Windows.Forms.TextBox()
        Me.txtQty50_sbd = New System.Windows.Forms.TextBox()
        Me.Label275 = New System.Windows.Forms.Label()
        Me.Label276 = New System.Windows.Forms.Label()
        Me.Label279 = New System.Windows.Forms.Label()
        Me.Label301 = New System.Windows.Forms.Label()
        Me.Label302 = New System.Windows.Forms.Label()
        Me.Label319 = New System.Windows.Forms.Label()
        Me.Label326 = New System.Windows.Forms.Label()
        Me.Label327 = New System.Windows.Forms.Label()
        Me.Label324 = New System.Windows.Forms.Label()
        Me.Label323 = New System.Windows.Forms.Label()
        Me.txtCusId_sbd = New System.Windows.Forms.TextBox()
        Me.btnSearchCus_sbd = New System.Windows.Forms.Button()
        Me.txtCusCode_sbd = New System.Windows.Forms.TextBox()
        Me.txtCusName_sbd = New System.Windows.Forms.TextBox()
        Me.grpSB = New System.Windows.Forms.GroupBox()
        Me.txtSBId = New System.Windows.Forms.TextBox()
        Me.Label325 = New System.Windows.Forms.Label()
        Me.dtpSC = New System.Windows.Forms.DateTimePicker()
        Me.grpPrice_sb = New System.Windows.Forms.GroupBox()
        Me.txtBuy10 = New System.Windows.Forms.TextBox()
        Me.txtBuy16 = New System.Windows.Forms.TextBox()
        Me.txtAcquisitions4 = New System.Windows.Forms.TextBox()
        Me.txtAcquisitions10 = New System.Windows.Forms.TextBox()
        Me.txtAcquisitions16 = New System.Windows.Forms.TextBox()
        Me.txtBuy4 = New System.Windows.Forms.TextBox()
        Me.txtBuy20 = New System.Windows.Forms.TextBox()
        Me.txtBuy50 = New System.Windows.Forms.TextBox()
        Me.txtAcquisitions20 = New System.Windows.Forms.TextBox()
        Me.txtAcquisitions50 = New System.Windows.Forms.TextBox()
        Me.Label303 = New System.Windows.Forms.Label()
        Me.Label315 = New System.Windows.Forms.Label()
        Me.Label316 = New System.Windows.Forms.Label()
        Me.Label317 = New System.Windows.Forms.Label()
        Me.Label318 = New System.Windows.Forms.Label()
        Me.Label320 = New System.Windows.Forms.Label()
        Me.Label321 = New System.Windows.Forms.Label()
        Me.Label322 = New System.Windows.Forms.Label()
        Me.dgvSB = New System.Windows.Forms.DataGridView()
        Me.btnCreate_sb = New System.Windows.Forms.Button()
        Me.btnPrint_sb = New System.Windows.Forms.Button()
        Me.BtnEdit_sb = New System.Windows.Forms.Button()
        Me.btnDelete_sb = New System.Windows.Forms.Button()
        Me.btnCancel_sb = New System.Windows.Forms.Button()
        Me.chkIsMonthlyStatement = New System.Windows.Forms.CheckBox()
        Me.grpSBD.SuspendLayout()
        CType(Me.dgvSBD, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpPrice_sbd.SuspendLayout()
        Me.grpSB.SuspendLayout()
        Me.grpPrice_sb.SuspendLayout()
        CType(Me.dgvSB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpSBD
        '
        Me.grpSBD.Controls.Add(Me.chkIsMonthlyStatement)
        Me.grpSBD.Controls.Add(Me.dgvSBD)
        Me.grpSBD.Controls.Add(Me.btnCreate_sbd)
        Me.grpSBD.Controls.Add(Me.btnEdit_sbd)
        Me.grpSBD.Controls.Add(Me.btnDelete_sbd)
        Me.grpSBD.Controls.Add(Me.btnCancel_sbd)
        Me.grpSBD.Controls.Add(Me.grpPrice_sbd)
        Me.grpSBD.Controls.Add(Me.Label324)
        Me.grpSBD.Controls.Add(Me.Label323)
        Me.grpSBD.Controls.Add(Me.txtCusId_sbd)
        Me.grpSBD.Controls.Add(Me.btnSearchCus_sbd)
        Me.grpSBD.Controls.Add(Me.txtCusCode_sbd)
        Me.grpSBD.Controls.Add(Me.txtCusName_sbd)
        Me.grpSBD.Enabled = False
        Me.grpSBD.Location = New System.Drawing.Point(786, 3)
        Me.grpSBD.Name = "grpSBD"
        Me.grpSBD.Size = New System.Drawing.Size(1087, 937)
        Me.grpSBD.TabIndex = 485
        Me.grpSBD.TabStop = False
        Me.grpSBD.Text = "客戶設定"
        '
        'dgvSBD
        '
        Me.dgvSBD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSBD.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvSBD.Location = New System.Drawing.Point(3, 264)
        Me.dgvSBD.Name = "dgvSBD"
        Me.dgvSBD.ReadOnly = True
        Me.dgvSBD.RowTemplate.Height = 24
        Me.dgvSBD.Size = New System.Drawing.Size(1081, 670)
        Me.dgvSBD.TabIndex = 487
        '
        'btnCreate_sbd
        '
        Me.btnCreate_sbd.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCreate_sbd.Location = New System.Drawing.Point(6, 199)
        Me.btnCreate_sbd.Name = "btnCreate_sbd"
        Me.btnCreate_sbd.Size = New System.Drawing.Size(140, 44)
        Me.btnCreate_sbd.TabIndex = 483
        Me.btnCreate_sbd.Tag = ""
        Me.btnCreate_sbd.Text = "新  增"
        Me.btnCreate_sbd.UseVisualStyleBackColor = False
        '
        'btnEdit_sbd
        '
        Me.btnEdit_sbd.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit_sbd.Location = New System.Drawing.Point(162, 199)
        Me.btnEdit_sbd.Name = "btnEdit_sbd"
        Me.btnEdit_sbd.Size = New System.Drawing.Size(140, 44)
        Me.btnEdit_sbd.TabIndex = 484
        Me.btnEdit_sbd.Text = "修  改"
        Me.btnEdit_sbd.UseVisualStyleBackColor = False
        '
        'btnDelete_sbd
        '
        Me.btnDelete_sbd.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete_sbd.Location = New System.Drawing.Point(318, 199)
        Me.btnDelete_sbd.Name = "btnDelete_sbd"
        Me.btnDelete_sbd.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete_sbd.TabIndex = 485
        Me.btnDelete_sbd.Text = "刪  除"
        Me.btnDelete_sbd.UseVisualStyleBackColor = False
        '
        'btnCancel_sbd
        '
        Me.btnCancel_sbd.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel_sbd.Location = New System.Drawing.Point(474, 199)
        Me.btnCancel_sbd.Name = "btnCancel_sbd"
        Me.btnCancel_sbd.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel_sbd.TabIndex = 486
        Me.btnCancel_sbd.Text = "取  消"
        Me.btnCancel_sbd.UseVisualStyleBackColor = False
        '
        'grpPrice_sbd
        '
        Me.grpPrice_sbd.Controls.Add(Me.txtAcquisitions10_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtAcquisitions16_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtAcquisitions4_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtAcquisitions20_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtAcquisitions50_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.Label328)
        Me.grpPrice_sbd.Controls.Add(Me.txtBuy10_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtBuy16_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtQty4_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtQty10_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtQty16_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtBuy4_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtBuy20_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtBuy50_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtQty20_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.txtQty50_sbd)
        Me.grpPrice_sbd.Controls.Add(Me.Label275)
        Me.grpPrice_sbd.Controls.Add(Me.Label276)
        Me.grpPrice_sbd.Controls.Add(Me.Label279)
        Me.grpPrice_sbd.Controls.Add(Me.Label301)
        Me.grpPrice_sbd.Controls.Add(Me.Label302)
        Me.grpPrice_sbd.Controls.Add(Me.Label319)
        Me.grpPrice_sbd.Controls.Add(Me.Label326)
        Me.grpPrice_sbd.Controls.Add(Me.Label327)
        Me.grpPrice_sbd.Location = New System.Drawing.Point(489, 29)
        Me.grpPrice_sbd.Name = "grpPrice_sbd"
        Me.grpPrice_sbd.Size = New System.Drawing.Size(398, 164)
        Me.grpPrice_sbd.TabIndex = 482
        Me.grpPrice_sbd.TabStop = False
        Me.grpPrice_sbd.Text = "價格"
        '
        'txtAcquisitions10_sbd
        '
        Me.txtAcquisitions10_sbd.Location = New System.Drawing.Point(279, 124)
        Me.txtAcquisitions10_sbd.Name = "txtAcquisitions10_sbd"
        Me.txtAcquisitions10_sbd.ReadOnly = True
        Me.txtAcquisitions10_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtAcquisitions10_sbd.TabIndex = 34
        '
        'txtAcquisitions16_sbd
        '
        Me.txtAcquisitions16_sbd.Location = New System.Drawing.Point(221, 124)
        Me.txtAcquisitions16_sbd.Name = "txtAcquisitions16_sbd"
        Me.txtAcquisitions16_sbd.ReadOnly = True
        Me.txtAcquisitions16_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtAcquisitions16_sbd.TabIndex = 33
        '
        'txtAcquisitions4_sbd
        '
        Me.txtAcquisitions4_sbd.Location = New System.Drawing.Point(337, 124)
        Me.txtAcquisitions4_sbd.Name = "txtAcquisitions4_sbd"
        Me.txtAcquisitions4_sbd.ReadOnly = True
        Me.txtAcquisitions4_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtAcquisitions4_sbd.TabIndex = 32
        '
        'txtAcquisitions20_sbd
        '
        Me.txtAcquisitions20_sbd.Location = New System.Drawing.Point(163, 124)
        Me.txtAcquisitions20_sbd.Name = "txtAcquisitions20_sbd"
        Me.txtAcquisitions20_sbd.ReadOnly = True
        Me.txtAcquisitions20_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtAcquisitions20_sbd.TabIndex = 31
        '
        'txtAcquisitions50_sbd
        '
        Me.txtAcquisitions50_sbd.Location = New System.Drawing.Point(105, 124)
        Me.txtAcquisitions50_sbd.Name = "txtAcquisitions50_sbd"
        Me.txtAcquisitions50_sbd.ReadOnly = True
        Me.txtAcquisitions50_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtAcquisitions50_sbd.TabIndex = 30
        '
        'Label328
        '
        Me.Label328.AutoSize = True
        Me.Label328.Location = New System.Drawing.Point(6, 130)
        Me.Label328.Name = "Label328"
        Me.Label328.Size = New System.Drawing.Size(93, 19)
        Me.Label328.TabIndex = 29
        Me.Label328.Text = "收購金額"
        '
        'txtBuy10_sbd
        '
        Me.txtBuy10_sbd.Location = New System.Drawing.Point(279, 86)
        Me.txtBuy10_sbd.Name = "txtBuy10_sbd"
        Me.txtBuy10_sbd.ReadOnly = True
        Me.txtBuy10_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtBuy10_sbd.TabIndex = 28
        '
        'txtBuy16_sbd
        '
        Me.txtBuy16_sbd.Location = New System.Drawing.Point(221, 86)
        Me.txtBuy16_sbd.Name = "txtBuy16_sbd"
        Me.txtBuy16_sbd.ReadOnly = True
        Me.txtBuy16_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtBuy16_sbd.TabIndex = 27
        '
        'txtQty4_sbd
        '
        Me.txtQty4_sbd.Location = New System.Drawing.Point(337, 48)
        Me.txtQty4_sbd.Name = "txtQty4_sbd"
        Me.txtQty4_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtQty4_sbd.TabIndex = 24
        '
        'txtQty10_sbd
        '
        Me.txtQty10_sbd.Location = New System.Drawing.Point(279, 48)
        Me.txtQty10_sbd.Name = "txtQty10_sbd"
        Me.txtQty10_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtQty10_sbd.TabIndex = 23
        '
        'txtQty16_sbd
        '
        Me.txtQty16_sbd.Location = New System.Drawing.Point(221, 48)
        Me.txtQty16_sbd.Name = "txtQty16_sbd"
        Me.txtQty16_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtQty16_sbd.TabIndex = 22
        '
        'txtBuy4_sbd
        '
        Me.txtBuy4_sbd.Location = New System.Drawing.Point(337, 86)
        Me.txtBuy4_sbd.Name = "txtBuy4_sbd"
        Me.txtBuy4_sbd.ReadOnly = True
        Me.txtBuy4_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtBuy4_sbd.TabIndex = 19
        '
        'txtBuy20_sbd
        '
        Me.txtBuy20_sbd.Location = New System.Drawing.Point(163, 86)
        Me.txtBuy20_sbd.Name = "txtBuy20_sbd"
        Me.txtBuy20_sbd.ReadOnly = True
        Me.txtBuy20_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtBuy20_sbd.TabIndex = 18
        '
        'txtBuy50_sbd
        '
        Me.txtBuy50_sbd.Location = New System.Drawing.Point(105, 86)
        Me.txtBuy50_sbd.Name = "txtBuy50_sbd"
        Me.txtBuy50_sbd.ReadOnly = True
        Me.txtBuy50_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtBuy50_sbd.TabIndex = 17
        '
        'txtQty20_sbd
        '
        Me.txtQty20_sbd.Location = New System.Drawing.Point(163, 48)
        Me.txtQty20_sbd.Name = "txtQty20_sbd"
        Me.txtQty20_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtQty20_sbd.TabIndex = 14
        '
        'txtQty50_sbd
        '
        Me.txtQty50_sbd.Location = New System.Drawing.Point(105, 48)
        Me.txtQty50_sbd.Name = "txtQty50_sbd"
        Me.txtQty50_sbd.Size = New System.Drawing.Size(50, 30)
        Me.txtQty50_sbd.TabIndex = 13
        '
        'Label275
        '
        Me.Label275.AutoSize = True
        Me.Label275.Location = New System.Drawing.Point(352, 26)
        Me.Label275.Name = "Label275"
        Me.Label275.Size = New System.Drawing.Size(20, 19)
        Me.Label275.TabIndex = 8
        Me.Label275.Text = "4"
        '
        'Label276
        '
        Me.Label276.AutoSize = True
        Me.Label276.Location = New System.Drawing.Point(289, 26)
        Me.Label276.Name = "Label276"
        Me.Label276.Size = New System.Drawing.Size(31, 19)
        Me.Label276.TabIndex = 7
        Me.Label276.Text = "10"
        '
        'Label279
        '
        Me.Label279.AutoSize = True
        Me.Label279.Location = New System.Drawing.Point(231, 26)
        Me.Label279.Name = "Label279"
        Me.Label279.Size = New System.Drawing.Size(31, 19)
        Me.Label279.TabIndex = 6
        Me.Label279.Text = "16"
        '
        'Label301
        '
        Me.Label301.AutoSize = True
        Me.Label301.Location = New System.Drawing.Point(173, 26)
        Me.Label301.Name = "Label301"
        Me.Label301.Size = New System.Drawing.Size(31, 19)
        Me.Label301.TabIndex = 5
        Me.Label301.Text = "20"
        '
        'Label302
        '
        Me.Label302.AutoSize = True
        Me.Label302.Location = New System.Drawing.Point(115, 26)
        Me.Label302.Name = "Label302"
        Me.Label302.Size = New System.Drawing.Size(31, 19)
        Me.Label302.TabIndex = 4
        Me.Label302.Text = "50"
        '
        'Label319
        '
        Me.Label319.AutoSize = True
        Me.Label319.Location = New System.Drawing.Point(6, 92)
        Me.Label319.Name = "Label319"
        Me.Label319.Size = New System.Drawing.Size(93, 19)
        Me.Label319.TabIndex = 2
        Me.Label319.Text = "買價金額"
        '
        'Label326
        '
        Me.Label326.AutoSize = True
        Me.Label326.Location = New System.Drawing.Point(6, 54)
        Me.Label326.Name = "Label326"
        Me.Label326.Size = New System.Drawing.Size(51, 19)
        Me.Label326.TabIndex = 1
        Me.Label326.Text = "數量"
        '
        'Label327
        '
        Me.Label327.AutoSize = True
        Me.Label327.Location = New System.Drawing.Point(6, 26)
        Me.Label327.Name = "Label327"
        Me.Label327.Size = New System.Drawing.Size(51, 19)
        Me.Label327.TabIndex = 0
        Me.Label327.Text = "公斤"
        '
        'Label324
        '
        Me.Label324.AutoSize = True
        Me.Label324.Location = New System.Drawing.Point(6, 34)
        Me.Label324.Name = "Label324"
        Me.Label324.Size = New System.Drawing.Size(93, 19)
        Me.Label324.TabIndex = 468
        Me.Label324.Text = "客戶代號"
        '
        'Label323
        '
        Me.Label323.AutoSize = True
        Me.Label323.Location = New System.Drawing.Point(6, 73)
        Me.Label323.Name = "Label323"
        Me.Label323.Size = New System.Drawing.Size(93, 19)
        Me.Label323.TabIndex = 469
        Me.Label323.Text = "客戶名稱"
        '
        'txtCusId_sbd
        '
        Me.txtCusId_sbd.Location = New System.Drawing.Point(893, 35)
        Me.txtCusId_sbd.Name = "txtCusId_sbd"
        Me.txtCusId_sbd.ReadOnly = True
        Me.txtCusId_sbd.Size = New System.Drawing.Size(165, 30)
        Me.txtCusId_sbd.TabIndex = 481
        Me.txtCusId_sbd.Tag = "cus_code"
        Me.txtCusId_sbd.Visible = False
        '
        'btnSearchCus_sbd
        '
        Me.btnSearchCus_sbd.AutoSize = True
        Me.btnSearchCus_sbd.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnSearchCus_sbd.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnSearchCus_sbd.Location = New System.Drawing.Point(276, 29)
        Me.btnSearchCus_sbd.Name = "btnSearchCus_sbd"
        Me.btnSearchCus_sbd.Size = New System.Drawing.Size(82, 26)
        Me.btnSearchCus_sbd.TabIndex = 471
        Me.btnSearchCus_sbd.Text = "搜尋客戶"
        Me.btnSearchCus_sbd.UseVisualStyleBackColor = False
        '
        'txtCusCode_sbd
        '
        Me.txtCusCode_sbd.Location = New System.Drawing.Point(105, 29)
        Me.txtCusCode_sbd.Name = "txtCusCode_sbd"
        Me.txtCusCode_sbd.Size = New System.Drawing.Size(165, 30)
        Me.txtCusCode_sbd.TabIndex = 472
        Me.txtCusCode_sbd.Tag = "cus_code"
        '
        'txtCusName_sbd
        '
        Me.txtCusName_sbd.Location = New System.Drawing.Point(105, 67)
        Me.txtCusName_sbd.Name = "txtCusName_sbd"
        Me.txtCusName_sbd.ReadOnly = True
        Me.txtCusName_sbd.Size = New System.Drawing.Size(378, 30)
        Me.txtCusName_sbd.TabIndex = 470
        Me.txtCusName_sbd.Tag = "cus_name"
        '
        'grpSB
        '
        Me.grpSB.Controls.Add(Me.txtSBId)
        Me.grpSB.Controls.Add(Me.Label325)
        Me.grpSB.Controls.Add(Me.dtpSC)
        Me.grpSB.Controls.Add(Me.grpPrice_sb)
        Me.grpSB.Controls.Add(Me.dgvSB)
        Me.grpSB.Controls.Add(Me.btnCreate_sb)
        Me.grpSB.Controls.Add(Me.btnPrint_sb)
        Me.grpSB.Controls.Add(Me.BtnEdit_sb)
        Me.grpSB.Controls.Add(Me.btnDelete_sb)
        Me.grpSB.Controls.Add(Me.btnCancel_sb)
        Me.grpSB.Location = New System.Drawing.Point(3, 3)
        Me.grpSB.Name = "grpSB"
        Me.grpSB.Size = New System.Drawing.Size(780, 937)
        Me.grpSB.TabIndex = 484
        Me.grpSB.TabStop = False
        Me.grpSB.Text = "月價格設定"
        '
        'txtSBId
        '
        Me.txtSBId.Location = New System.Drawing.Point(570, 121)
        Me.txtSBId.Name = "txtSBId"
        Me.txtSBId.ReadOnly = True
        Me.txtSBId.Size = New System.Drawing.Size(165, 30)
        Me.txtSBId.TabIndex = 482
        Me.txtSBId.Tag = "cus_code"
        Me.txtSBId.Visible = False
        '
        'Label325
        '
        Me.Label325.AutoSize = True
        Me.Label325.Location = New System.Drawing.Point(6, 35)
        Me.Label325.Name = "Label325"
        Me.Label325.Size = New System.Drawing.Size(51, 19)
        Me.Label325.TabIndex = 466
        Me.Label325.Text = "月份"
        '
        'dtpSC
        '
        Me.dtpSC.CustomFormat = "yyyy/MM"
        Me.dtpSC.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpSC.Location = New System.Drawing.Point(63, 29)
        Me.dtpSC.Name = "dtpSC"
        Me.dtpSC.Size = New System.Drawing.Size(120, 30)
        Me.dtpSC.TabIndex = 467
        '
        'grpPrice_sb
        '
        Me.grpPrice_sb.Controls.Add(Me.txtBuy10)
        Me.grpPrice_sb.Controls.Add(Me.txtBuy16)
        Me.grpPrice_sb.Controls.Add(Me.txtAcquisitions4)
        Me.grpPrice_sb.Controls.Add(Me.txtAcquisitions10)
        Me.grpPrice_sb.Controls.Add(Me.txtAcquisitions16)
        Me.grpPrice_sb.Controls.Add(Me.txtBuy4)
        Me.grpPrice_sb.Controls.Add(Me.txtBuy20)
        Me.grpPrice_sb.Controls.Add(Me.txtBuy50)
        Me.grpPrice_sb.Controls.Add(Me.txtAcquisitions20)
        Me.grpPrice_sb.Controls.Add(Me.txtAcquisitions50)
        Me.grpPrice_sb.Controls.Add(Me.Label303)
        Me.grpPrice_sb.Controls.Add(Me.Label315)
        Me.grpPrice_sb.Controls.Add(Me.Label316)
        Me.grpPrice_sb.Controls.Add(Me.Label317)
        Me.grpPrice_sb.Controls.Add(Me.Label318)
        Me.grpPrice_sb.Controls.Add(Me.Label320)
        Me.grpPrice_sb.Controls.Add(Me.Label321)
        Me.grpPrice_sb.Controls.Add(Me.Label322)
        Me.grpPrice_sb.Location = New System.Drawing.Point(189, 29)
        Me.grpPrice_sb.Name = "grpPrice_sb"
        Me.grpPrice_sb.Size = New System.Drawing.Size(375, 126)
        Me.grpPrice_sb.TabIndex = 473
        Me.grpPrice_sb.TabStop = False
        Me.grpPrice_sb.Text = "價格"
        '
        'txtBuy10
        '
        Me.txtBuy10.Location = New System.Drawing.Point(258, 86)
        Me.txtBuy10.Name = "txtBuy10"
        Me.txtBuy10.Size = New System.Drawing.Size(50, 30)
        Me.txtBuy10.TabIndex = 28
        '
        'txtBuy16
        '
        Me.txtBuy16.Location = New System.Drawing.Point(200, 86)
        Me.txtBuy16.Name = "txtBuy16"
        Me.txtBuy16.Size = New System.Drawing.Size(50, 30)
        Me.txtBuy16.TabIndex = 27
        '
        'txtAcquisitions4
        '
        Me.txtAcquisitions4.Location = New System.Drawing.Point(316, 48)
        Me.txtAcquisitions4.Name = "txtAcquisitions4"
        Me.txtAcquisitions4.Size = New System.Drawing.Size(50, 30)
        Me.txtAcquisitions4.TabIndex = 24
        '
        'txtAcquisitions10
        '
        Me.txtAcquisitions10.Location = New System.Drawing.Point(258, 48)
        Me.txtAcquisitions10.Name = "txtAcquisitions10"
        Me.txtAcquisitions10.Size = New System.Drawing.Size(50, 30)
        Me.txtAcquisitions10.TabIndex = 23
        '
        'txtAcquisitions16
        '
        Me.txtAcquisitions16.Location = New System.Drawing.Point(200, 48)
        Me.txtAcquisitions16.Name = "txtAcquisitions16"
        Me.txtAcquisitions16.Size = New System.Drawing.Size(50, 30)
        Me.txtAcquisitions16.TabIndex = 22
        '
        'txtBuy4
        '
        Me.txtBuy4.Location = New System.Drawing.Point(316, 86)
        Me.txtBuy4.Name = "txtBuy4"
        Me.txtBuy4.Size = New System.Drawing.Size(50, 30)
        Me.txtBuy4.TabIndex = 19
        '
        'txtBuy20
        '
        Me.txtBuy20.Location = New System.Drawing.Point(142, 86)
        Me.txtBuy20.Name = "txtBuy20"
        Me.txtBuy20.Size = New System.Drawing.Size(50, 30)
        Me.txtBuy20.TabIndex = 18
        '
        'txtBuy50
        '
        Me.txtBuy50.Location = New System.Drawing.Point(84, 86)
        Me.txtBuy50.Name = "txtBuy50"
        Me.txtBuy50.Size = New System.Drawing.Size(50, 30)
        Me.txtBuy50.TabIndex = 17
        '
        'txtAcquisitions20
        '
        Me.txtAcquisitions20.Location = New System.Drawing.Point(142, 48)
        Me.txtAcquisitions20.Name = "txtAcquisitions20"
        Me.txtAcquisitions20.Size = New System.Drawing.Size(50, 30)
        Me.txtAcquisitions20.TabIndex = 14
        '
        'txtAcquisitions50
        '
        Me.txtAcquisitions50.Location = New System.Drawing.Point(84, 48)
        Me.txtAcquisitions50.Name = "txtAcquisitions50"
        Me.txtAcquisitions50.Size = New System.Drawing.Size(50, 30)
        Me.txtAcquisitions50.TabIndex = 13
        '
        'Label303
        '
        Me.Label303.AutoSize = True
        Me.Label303.Location = New System.Drawing.Point(331, 26)
        Me.Label303.Name = "Label303"
        Me.Label303.Size = New System.Drawing.Size(20, 19)
        Me.Label303.TabIndex = 8
        Me.Label303.Text = "4"
        '
        'Label315
        '
        Me.Label315.AutoSize = True
        Me.Label315.Location = New System.Drawing.Point(268, 26)
        Me.Label315.Name = "Label315"
        Me.Label315.Size = New System.Drawing.Size(31, 19)
        Me.Label315.TabIndex = 7
        Me.Label315.Text = "10"
        '
        'Label316
        '
        Me.Label316.AutoSize = True
        Me.Label316.Location = New System.Drawing.Point(210, 26)
        Me.Label316.Name = "Label316"
        Me.Label316.Size = New System.Drawing.Size(31, 19)
        Me.Label316.TabIndex = 6
        Me.Label316.Text = "16"
        '
        'Label317
        '
        Me.Label317.AutoSize = True
        Me.Label317.Location = New System.Drawing.Point(152, 26)
        Me.Label317.Name = "Label317"
        Me.Label317.Size = New System.Drawing.Size(31, 19)
        Me.Label317.TabIndex = 5
        Me.Label317.Text = "20"
        '
        'Label318
        '
        Me.Label318.AutoSize = True
        Me.Label318.Location = New System.Drawing.Point(94, 26)
        Me.Label318.Name = "Label318"
        Me.Label318.Size = New System.Drawing.Size(31, 19)
        Me.Label318.TabIndex = 4
        Me.Label318.Text = "50"
        '
        'Label320
        '
        Me.Label320.AutoSize = True
        Me.Label320.Location = New System.Drawing.Point(6, 90)
        Me.Label320.Name = "Label320"
        Me.Label320.Size = New System.Drawing.Size(51, 19)
        Me.Label320.TabIndex = 2
        Me.Label320.Text = "買價"
        '
        'Label321
        '
        Me.Label321.AutoSize = True
        Me.Label321.Location = New System.Drawing.Point(6, 54)
        Me.Label321.Name = "Label321"
        Me.Label321.Size = New System.Drawing.Size(72, 19)
        Me.Label321.TabIndex = 1
        Me.Label321.Text = "收購價"
        '
        'Label322
        '
        Me.Label322.AutoSize = True
        Me.Label322.Location = New System.Drawing.Point(6, 26)
        Me.Label322.Name = "Label322"
        Me.Label322.Size = New System.Drawing.Size(51, 19)
        Me.Label322.TabIndex = 0
        Me.Label322.Text = "公斤"
        '
        'dgvSB
        '
        Me.dgvSB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSB.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvSB.Location = New System.Drawing.Point(3, 224)
        Me.dgvSB.Name = "dgvSB"
        Me.dgvSB.ReadOnly = True
        Me.dgvSB.RowTemplate.Height = 24
        Me.dgvSB.Size = New System.Drawing.Size(774, 710)
        Me.dgvSB.TabIndex = 480
        '
        'btnCreate_sb
        '
        Me.btnCreate_sb.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCreate_sb.Location = New System.Drawing.Point(6, 161)
        Me.btnCreate_sb.Name = "btnCreate_sb"
        Me.btnCreate_sb.Size = New System.Drawing.Size(140, 44)
        Me.btnCreate_sb.TabIndex = 474
        Me.btnCreate_sb.Tag = ""
        Me.btnCreate_sb.Text = "新  增"
        Me.btnCreate_sb.UseVisualStyleBackColor = False
        '
        'btnPrint_sb
        '
        Me.btnPrint_sb.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnPrint_sb.Location = New System.Drawing.Point(630, 161)
        Me.btnPrint_sb.Name = "btnPrint_sb"
        Me.btnPrint_sb.Size = New System.Drawing.Size(140, 44)
        Me.btnPrint_sb.TabIndex = 479
        Me.btnPrint_sb.Text = "列   印"
        Me.btnPrint_sb.UseVisualStyleBackColor = False
        '
        'BtnEdit_sb
        '
        Me.BtnEdit_sb.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.BtnEdit_sb.Location = New System.Drawing.Point(162, 161)
        Me.BtnEdit_sb.Name = "BtnEdit_sb"
        Me.BtnEdit_sb.Size = New System.Drawing.Size(140, 44)
        Me.BtnEdit_sb.TabIndex = 475
        Me.BtnEdit_sb.Text = "修  改"
        Me.BtnEdit_sb.UseVisualStyleBackColor = False
        '
        'btnDelete_sb
        '
        Me.btnDelete_sb.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete_sb.Location = New System.Drawing.Point(318, 161)
        Me.btnDelete_sb.Name = "btnDelete_sb"
        Me.btnDelete_sb.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete_sb.TabIndex = 476
        Me.btnDelete_sb.Text = "刪  除"
        Me.btnDelete_sb.UseVisualStyleBackColor = False
        '
        'btnCancel_sb
        '
        Me.btnCancel_sb.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel_sb.Location = New System.Drawing.Point(474, 161)
        Me.btnCancel_sb.Name = "btnCancel_sb"
        Me.btnCancel_sb.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel_sb.TabIndex = 477
        Me.btnCancel_sb.Text = "取  消"
        Me.btnCancel_sb.UseVisualStyleBackColor = False
        '
        'chkIsMonthlyStatement
        '
        Me.chkIsMonthlyStatement.AutoSize = True
        Me.chkIsMonthlyStatement.Location = New System.Drawing.Point(10, 108)
        Me.chkIsMonthlyStatement.Name = "chkIsMonthlyStatement"
        Me.chkIsMonthlyStatement.Size = New System.Drawing.Size(133, 23)
        Me.chkIsMonthlyStatement.TabIndex = 488
        Me.chkIsMonthlyStatement.Text = "紀錄對帳單"
        Me.chkIsMonthlyStatement.UseVisualStyleBackColor = True
        '
        'ScrapBarrelUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpSBD)
        Me.Controls.Add(Me.grpSB)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "ScrapBarrelUserControl"
        Me.Size = New System.Drawing.Size(1882, 949)
        Me.grpSBD.ResumeLayout(False)
        Me.grpSBD.PerformLayout()
        CType(Me.dgvSBD, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpPrice_sbd.ResumeLayout(False)
        Me.grpPrice_sbd.PerformLayout()
        Me.grpSB.ResumeLayout(False)
        Me.grpSB.PerformLayout()
        Me.grpPrice_sb.ResumeLayout(False)
        Me.grpPrice_sb.PerformLayout()
        CType(Me.dgvSB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpSBD As GroupBox
    Friend WithEvents dgvSBD As DataGridView
    Friend WithEvents btnCreate_sbd As Button
    Friend WithEvents btnEdit_sbd As Button
    Friend WithEvents btnDelete_sbd As Button
    Friend WithEvents btnCancel_sbd As Button
    Friend WithEvents grpPrice_sbd As GroupBox
    Friend WithEvents txtAcquisitions10_sbd As TextBox
    Friend WithEvents txtAcquisitions16_sbd As TextBox
    Friend WithEvents txtAcquisitions4_sbd As TextBox
    Friend WithEvents txtAcquisitions20_sbd As TextBox
    Friend WithEvents txtAcquisitions50_sbd As TextBox
    Friend WithEvents Label328 As Label
    Friend WithEvents txtBuy10_sbd As TextBox
    Friend WithEvents txtBuy16_sbd As TextBox
    Friend WithEvents txtQty4_sbd As TextBox
    Friend WithEvents txtQty10_sbd As TextBox
    Friend WithEvents txtQty16_sbd As TextBox
    Friend WithEvents txtBuy4_sbd As TextBox
    Friend WithEvents txtBuy20_sbd As TextBox
    Friend WithEvents txtBuy50_sbd As TextBox
    Friend WithEvents txtQty20_sbd As TextBox
    Friend WithEvents txtQty50_sbd As TextBox
    Friend WithEvents Label275 As Label
    Friend WithEvents Label276 As Label
    Friend WithEvents Label279 As Label
    Friend WithEvents Label301 As Label
    Friend WithEvents Label302 As Label
    Friend WithEvents Label319 As Label
    Friend WithEvents Label326 As Label
    Friend WithEvents Label327 As Label
    Friend WithEvents Label324 As Label
    Friend WithEvents Label323 As Label
    Friend WithEvents txtCusId_sbd As TextBox
    Friend WithEvents btnSearchCus_sbd As Button
    Friend WithEvents txtCusCode_sbd As TextBox
    Friend WithEvents txtCusName_sbd As TextBox
    Friend WithEvents grpSB As GroupBox
    Friend WithEvents txtSBId As TextBox
    Friend WithEvents Label325 As Label
    Friend WithEvents dtpSC As DateTimePicker
    Friend WithEvents grpPrice_sb As GroupBox
    Friend WithEvents txtBuy10 As TextBox
    Friend WithEvents txtBuy16 As TextBox
    Friend WithEvents txtAcquisitions4 As TextBox
    Friend WithEvents txtAcquisitions10 As TextBox
    Friend WithEvents txtAcquisitions16 As TextBox
    Friend WithEvents txtBuy4 As TextBox
    Friend WithEvents txtBuy20 As TextBox
    Friend WithEvents txtBuy50 As TextBox
    Friend WithEvents txtAcquisitions20 As TextBox
    Friend WithEvents txtAcquisitions50 As TextBox
    Friend WithEvents Label303 As Label
    Friend WithEvents Label315 As Label
    Friend WithEvents Label316 As Label
    Friend WithEvents Label317 As Label
    Friend WithEvents Label318 As Label
    Friend WithEvents Label320 As Label
    Friend WithEvents Label321 As Label
    Friend WithEvents Label322 As Label
    Friend WithEvents dgvSB As DataGridView
    Friend WithEvents btnCreate_sb As Button
    Friend WithEvents btnPrint_sb As Button
    Friend WithEvents BtnEdit_sb As Button
    Friend WithEvents btnDelete_sb As Button
    Friend WithEvents btnCancel_sb As Button
    Friend WithEvents chkIsMonthlyStatement As CheckBox
End Class
