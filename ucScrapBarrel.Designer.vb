<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucScrapBarrel
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
        Me.txtCusCode_ins = New System.Windows.Forms.TextBox()
        Me.txtCusName_ins = New System.Windows.Forms.TextBox()
        Me.btnCus_ins = New System.Windows.Forms.Button()
        Me.Label102 = New System.Windows.Forms.Label()
        Me.Label116 = New System.Windows.Forms.Label()
        Me.dtpIns = New System.Windows.Forms.DateTimePicker()
        Me.Label94 = New System.Windows.Forms.Label()
        Me.grpIns = New System.Windows.Forms.GroupBox()
        Me.txtAmount20 = New System.Windows.Forms.TextBox()
        Me.txtAmount50 = New System.Windows.Forms.TextBox()
        Me.txtPriceTotal = New System.Windows.Forms.TextBox()
        Me.Label265 = New System.Windows.Forms.Label()
        Me.txtAmountTotal = New System.Windows.Forms.TextBox()
        Me.txtAmountSpraying = New System.Windows.Forms.TextBox()
        Me.txtAmountRustProof = New System.Windows.Forms.TextBox()
        Me.txtAmountFreight = New System.Windows.Forms.TextBox()
        Me.txtAmountSwitch = New System.Windows.Forms.TextBox()
        Me.txtAmount4 = New System.Windows.Forms.TextBox()
        Me.txtAmount10 = New System.Windows.Forms.TextBox()
        Me.txtAmount16 = New System.Windows.Forms.TextBox()
        Me.txtQtyTotal = New System.Windows.Forms.TextBox()
        Me.txtPriceSpraying = New System.Windows.Forms.TextBox()
        Me.txtPriceFreight = New System.Windows.Forms.TextBox()
        Me.txtPrice10 = New System.Windows.Forms.TextBox()
        Me.txtPrice16 = New System.Windows.Forms.TextBox()
        Me.txtQtyFreight = New System.Windows.Forms.TextBox()
        Me.txtQtySwitch = New System.Windows.Forms.TextBox()
        Me.txtQty4 = New System.Windows.Forms.TextBox()
        Me.txtQty10 = New System.Windows.Forms.TextBox()
        Me.txtQty16 = New System.Windows.Forms.TextBox()
        Me.txtPriceRustProof = New System.Windows.Forms.TextBox()
        Me.txtPriceSwitch = New System.Windows.Forms.TextBox()
        Me.txtPrice4 = New System.Windows.Forms.TextBox()
        Me.txtPrice20 = New System.Windows.Forms.TextBox()
        Me.txtPrice50 = New System.Windows.Forms.TextBox()
        Me.txtQtySpraying = New System.Windows.Forms.TextBox()
        Me.txtQtyRustProof = New System.Windows.Forms.TextBox()
        Me.txtQty20 = New System.Windows.Forms.TextBox()
        Me.txtQty50 = New System.Windows.Forms.TextBox()
        Me.Label263 = New System.Windows.Forms.Label()
        Me.Label262 = New System.Windows.Forms.Label()
        Me.Label261 = New System.Windows.Forms.Label()
        Me.Label260 = New System.Windows.Forms.Label()
        Me.Label259 = New System.Windows.Forms.Label()
        Me.Label258 = New System.Windows.Forms.Label()
        Me.Label257 = New System.Windows.Forms.Label()
        Me.Label256 = New System.Windows.Forms.Label()
        Me.Label255 = New System.Windows.Forms.Label()
        Me.Label252 = New System.Windows.Forms.Label()
        Me.Label249 = New System.Windows.Forms.Label()
        Me.Label162 = New System.Windows.Forms.Label()
        Me.Label161 = New System.Windows.Forms.Label()
        Me.txtCusId_ins = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.grpIns.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtCusCode_ins
        '
        Me.txtCusCode_ins.Location = New System.Drawing.Point(298, 5)
        Me.txtCusCode_ins.Margin = New System.Windows.Forms.Padding(5)
        Me.txtCusCode_ins.Name = "txtCusCode_ins"
        Me.txtCusCode_ins.Size = New System.Drawing.Size(121, 30)
        Me.txtCusCode_ins.TabIndex = 457
        Me.txtCusCode_ins.Tag = "cus_code"
        '
        'txtCusName_ins
        '
        Me.txtCusName_ins.Location = New System.Drawing.Point(532, 5)
        Me.txtCusName_ins.Margin = New System.Windows.Forms.Padding(5)
        Me.txtCusName_ins.Name = "txtCusName_ins"
        Me.txtCusName_ins.ReadOnly = True
        Me.txtCusName_ins.Size = New System.Drawing.Size(295, 30)
        Me.txtCusName_ins.TabIndex = 455
        Me.txtCusName_ins.Tag = "cus_name"
        '
        'btnCus_ins
        '
        Me.btnCus_ins.AutoSize = True
        Me.btnCus_ins.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnCus_ins.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnCus_ins.Location = New System.Drawing.Point(728, 45)
        Me.btnCus_ins.Margin = New System.Windows.Forms.Padding(5)
        Me.btnCus_ins.Name = "btnCus_ins"
        Me.btnCus_ins.Size = New System.Drawing.Size(99, 31)
        Me.btnCus_ins.TabIndex = 456
        Me.btnCus_ins.Text = "搜尋客戶"
        Me.btnCus_ins.UseVisualStyleBackColor = False
        '
        'Label102
        '
        Me.Label102.AutoSize = True
        Me.Label102.Location = New System.Drawing.Point(429, 11)
        Me.Label102.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label102.Name = "Label102"
        Me.Label102.Size = New System.Drawing.Size(93, 19)
        Me.Label102.TabIndex = 454
        Me.Label102.Text = "客戶名稱"
        '
        'Label116
        '
        Me.Label116.AutoSize = True
        Me.Label116.Location = New System.Drawing.Point(197, 11)
        Me.Label116.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label116.Name = "Label116"
        Me.Label116.Size = New System.Drawing.Size(93, 19)
        Me.Label116.TabIndex = 453
        Me.Label116.Text = "客戶代號"
        '
        'dtpIns
        '
        Me.dtpIns.CustomFormat = "yyyy/MM"
        Me.dtpIns.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpIns.Location = New System.Drawing.Point(65, 5)
        Me.dtpIns.Margin = New System.Windows.Forms.Padding(5)
        Me.dtpIns.Name = "dtpIns"
        Me.dtpIns.Size = New System.Drawing.Size(121, 30)
        Me.dtpIns.TabIndex = 452
        '
        'Label94
        '
        Me.Label94.AutoSize = True
        Me.Label94.Location = New System.Drawing.Point(5, 11)
        Me.Label94.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label94.Name = "Label94"
        Me.Label94.Size = New System.Drawing.Size(51, 19)
        Me.Label94.TabIndex = 451
        Me.Label94.Text = "月份"
        '
        'grpIns
        '
        Me.grpIns.Controls.Add(Me.txtAmount20)
        Me.grpIns.Controls.Add(Me.txtAmount50)
        Me.grpIns.Controls.Add(Me.txtPriceTotal)
        Me.grpIns.Controls.Add(Me.Label265)
        Me.grpIns.Controls.Add(Me.txtAmountTotal)
        Me.grpIns.Controls.Add(Me.txtAmountSpraying)
        Me.grpIns.Controls.Add(Me.txtAmountRustProof)
        Me.grpIns.Controls.Add(Me.txtAmountFreight)
        Me.grpIns.Controls.Add(Me.txtAmountSwitch)
        Me.grpIns.Controls.Add(Me.txtAmount4)
        Me.grpIns.Controls.Add(Me.txtAmount10)
        Me.grpIns.Controls.Add(Me.txtAmount16)
        Me.grpIns.Controls.Add(Me.txtQtyTotal)
        Me.grpIns.Controls.Add(Me.txtPriceSpraying)
        Me.grpIns.Controls.Add(Me.txtPriceFreight)
        Me.grpIns.Controls.Add(Me.txtPrice10)
        Me.grpIns.Controls.Add(Me.txtPrice16)
        Me.grpIns.Controls.Add(Me.txtQtyFreight)
        Me.grpIns.Controls.Add(Me.txtQtySwitch)
        Me.grpIns.Controls.Add(Me.txtQty4)
        Me.grpIns.Controls.Add(Me.txtQty10)
        Me.grpIns.Controls.Add(Me.txtQty16)
        Me.grpIns.Controls.Add(Me.txtPriceRustProof)
        Me.grpIns.Controls.Add(Me.txtPriceSwitch)
        Me.grpIns.Controls.Add(Me.txtPrice4)
        Me.grpIns.Controls.Add(Me.txtPrice20)
        Me.grpIns.Controls.Add(Me.txtPrice50)
        Me.grpIns.Controls.Add(Me.txtQtySpraying)
        Me.grpIns.Controls.Add(Me.txtQtyRustProof)
        Me.grpIns.Controls.Add(Me.txtQty20)
        Me.grpIns.Controls.Add(Me.txtQty50)
        Me.grpIns.Controls.Add(Me.Label263)
        Me.grpIns.Controls.Add(Me.Label262)
        Me.grpIns.Controls.Add(Me.Label261)
        Me.grpIns.Controls.Add(Me.Label260)
        Me.grpIns.Controls.Add(Me.Label259)
        Me.grpIns.Controls.Add(Me.Label258)
        Me.grpIns.Controls.Add(Me.Label257)
        Me.grpIns.Controls.Add(Me.Label256)
        Me.grpIns.Controls.Add(Me.Label255)
        Me.grpIns.Controls.Add(Me.Label252)
        Me.grpIns.Controls.Add(Me.Label249)
        Me.grpIns.Controls.Add(Me.Label162)
        Me.grpIns.Controls.Add(Me.Label161)
        Me.grpIns.Location = New System.Drawing.Point(842, 5)
        Me.grpIns.Margin = New System.Windows.Forms.Padding(5)
        Me.grpIns.Name = "grpIns"
        Me.grpIns.Padding = New System.Windows.Forms.Padding(5)
        Me.grpIns.Size = New System.Drawing.Size(1035, 200)
        Me.grpIns.TabIndex = 458
        Me.grpIns.TabStop = False
        '
        'txtAmount20
        '
        Me.txtAmount20.Location = New System.Drawing.Point(168, 155)
        Me.txtAmount20.Margin = New System.Windows.Forms.Padding(5)
        Me.txtAmount20.Name = "txtAmount20"
        Me.txtAmount20.ReadOnly = True
        Me.txtAmount20.Size = New System.Drawing.Size(88, 30)
        Me.txtAmount20.TabIndex = 43
        '
        'txtAmount50
        '
        Me.txtAmount50.Location = New System.Drawing.Point(72, 155)
        Me.txtAmount50.Margin = New System.Windows.Forms.Padding(5)
        Me.txtAmount50.Name = "txtAmount50"
        Me.txtAmount50.ReadOnly = True
        Me.txtAmount50.Size = New System.Drawing.Size(88, 30)
        Me.txtAmount50.TabIndex = 42
        '
        'txtPriceTotal
        '
        Me.txtPriceTotal.Location = New System.Drawing.Point(936, 109)
        Me.txtPriceTotal.Margin = New System.Windows.Forms.Padding(5)
        Me.txtPriceTotal.Name = "txtPriceTotal"
        Me.txtPriceTotal.ReadOnly = True
        Me.txtPriceTotal.Size = New System.Drawing.Size(88, 30)
        Me.txtPriceTotal.TabIndex = 41
        '
        'Label265
        '
        Me.Label265.AutoSize = True
        Me.Label265.Location = New System.Drawing.Point(955, 28)
        Me.Label265.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label265.Name = "Label265"
        Me.Label265.Size = New System.Drawing.Size(51, 19)
        Me.Label265.TabIndex = 40
        Me.Label265.Text = "合計"
        '
        'txtAmountTotal
        '
        Me.txtAmountTotal.Location = New System.Drawing.Point(936, 155)
        Me.txtAmountTotal.Margin = New System.Windows.Forms.Padding(5)
        Me.txtAmountTotal.Name = "txtAmountTotal"
        Me.txtAmountTotal.ReadOnly = True
        Me.txtAmountTotal.Size = New System.Drawing.Size(88, 30)
        Me.txtAmountTotal.TabIndex = 39
        '
        'txtAmountSpraying
        '
        Me.txtAmountSpraying.Location = New System.Drawing.Point(840, 155)
        Me.txtAmountSpraying.Margin = New System.Windows.Forms.Padding(5)
        Me.txtAmountSpraying.Name = "txtAmountSpraying"
        Me.txtAmountSpraying.ReadOnly = True
        Me.txtAmountSpraying.Size = New System.Drawing.Size(88, 30)
        Me.txtAmountSpraying.TabIndex = 38
        '
        'txtAmountRustProof
        '
        Me.txtAmountRustProof.Location = New System.Drawing.Point(744, 155)
        Me.txtAmountRustProof.Margin = New System.Windows.Forms.Padding(5)
        Me.txtAmountRustProof.Name = "txtAmountRustProof"
        Me.txtAmountRustProof.ReadOnly = True
        Me.txtAmountRustProof.Size = New System.Drawing.Size(88, 30)
        Me.txtAmountRustProof.TabIndex = 37
        '
        'txtAmountFreight
        '
        Me.txtAmountFreight.Location = New System.Drawing.Point(648, 155)
        Me.txtAmountFreight.Margin = New System.Windows.Forms.Padding(5)
        Me.txtAmountFreight.Name = "txtAmountFreight"
        Me.txtAmountFreight.ReadOnly = True
        Me.txtAmountFreight.Size = New System.Drawing.Size(88, 30)
        Me.txtAmountFreight.TabIndex = 36
        '
        'txtAmountSwitch
        '
        Me.txtAmountSwitch.Location = New System.Drawing.Point(552, 155)
        Me.txtAmountSwitch.Margin = New System.Windows.Forms.Padding(5)
        Me.txtAmountSwitch.Name = "txtAmountSwitch"
        Me.txtAmountSwitch.ReadOnly = True
        Me.txtAmountSwitch.Size = New System.Drawing.Size(88, 30)
        Me.txtAmountSwitch.TabIndex = 35
        '
        'txtAmount4
        '
        Me.txtAmount4.Location = New System.Drawing.Point(456, 155)
        Me.txtAmount4.Margin = New System.Windows.Forms.Padding(5)
        Me.txtAmount4.Name = "txtAmount4"
        Me.txtAmount4.ReadOnly = True
        Me.txtAmount4.Size = New System.Drawing.Size(88, 30)
        Me.txtAmount4.TabIndex = 34
        '
        'txtAmount10
        '
        Me.txtAmount10.Location = New System.Drawing.Point(360, 155)
        Me.txtAmount10.Margin = New System.Windows.Forms.Padding(5)
        Me.txtAmount10.Name = "txtAmount10"
        Me.txtAmount10.ReadOnly = True
        Me.txtAmount10.Size = New System.Drawing.Size(88, 30)
        Me.txtAmount10.TabIndex = 33
        '
        'txtAmount16
        '
        Me.txtAmount16.Location = New System.Drawing.Point(264, 155)
        Me.txtAmount16.Margin = New System.Windows.Forms.Padding(5)
        Me.txtAmount16.Name = "txtAmount16"
        Me.txtAmount16.ReadOnly = True
        Me.txtAmount16.Size = New System.Drawing.Size(88, 30)
        Me.txtAmount16.TabIndex = 32
        '
        'txtQtyTotal
        '
        Me.txtQtyTotal.Location = New System.Drawing.Point(936, 63)
        Me.txtQtyTotal.Margin = New System.Windows.Forms.Padding(5)
        Me.txtQtyTotal.Name = "txtQtyTotal"
        Me.txtQtyTotal.ReadOnly = True
        Me.txtQtyTotal.Size = New System.Drawing.Size(88, 30)
        Me.txtQtyTotal.TabIndex = 31
        '
        'txtPriceSpraying
        '
        Me.txtPriceSpraying.Location = New System.Drawing.Point(840, 109)
        Me.txtPriceSpraying.Margin = New System.Windows.Forms.Padding(5)
        Me.txtPriceSpraying.Name = "txtPriceSpraying"
        Me.txtPriceSpraying.Size = New System.Drawing.Size(88, 30)
        Me.txtPriceSpraying.TabIndex = 30
        '
        'txtPriceFreight
        '
        Me.txtPriceFreight.Location = New System.Drawing.Point(648, 109)
        Me.txtPriceFreight.Margin = New System.Windows.Forms.Padding(5)
        Me.txtPriceFreight.Name = "txtPriceFreight"
        Me.txtPriceFreight.Size = New System.Drawing.Size(88, 30)
        Me.txtPriceFreight.TabIndex = 29
        '
        'txtPrice10
        '
        Me.txtPrice10.Location = New System.Drawing.Point(360, 109)
        Me.txtPrice10.Margin = New System.Windows.Forms.Padding(5)
        Me.txtPrice10.Name = "txtPrice10"
        Me.txtPrice10.Size = New System.Drawing.Size(88, 30)
        Me.txtPrice10.TabIndex = 28
        '
        'txtPrice16
        '
        Me.txtPrice16.Location = New System.Drawing.Point(264, 109)
        Me.txtPrice16.Margin = New System.Windows.Forms.Padding(5)
        Me.txtPrice16.Name = "txtPrice16"
        Me.txtPrice16.Size = New System.Drawing.Size(88, 30)
        Me.txtPrice16.TabIndex = 27
        '
        'txtQtyFreight
        '
        Me.txtQtyFreight.Location = New System.Drawing.Point(648, 63)
        Me.txtQtyFreight.Margin = New System.Windows.Forms.Padding(5)
        Me.txtQtyFreight.Name = "txtQtyFreight"
        Me.txtQtyFreight.Size = New System.Drawing.Size(88, 30)
        Me.txtQtyFreight.TabIndex = 26
        '
        'txtQtySwitch
        '
        Me.txtQtySwitch.Location = New System.Drawing.Point(552, 63)
        Me.txtQtySwitch.Margin = New System.Windows.Forms.Padding(5)
        Me.txtQtySwitch.Name = "txtQtySwitch"
        Me.txtQtySwitch.Size = New System.Drawing.Size(88, 30)
        Me.txtQtySwitch.TabIndex = 25
        '
        'txtQty4
        '
        Me.txtQty4.Location = New System.Drawing.Point(456, 63)
        Me.txtQty4.Margin = New System.Windows.Forms.Padding(5)
        Me.txtQty4.Name = "txtQty4"
        Me.txtQty4.Size = New System.Drawing.Size(88, 30)
        Me.txtQty4.TabIndex = 24
        '
        'txtQty10
        '
        Me.txtQty10.Location = New System.Drawing.Point(360, 63)
        Me.txtQty10.Margin = New System.Windows.Forms.Padding(5)
        Me.txtQty10.Name = "txtQty10"
        Me.txtQty10.Size = New System.Drawing.Size(88, 30)
        Me.txtQty10.TabIndex = 23
        '
        'txtQty16
        '
        Me.txtQty16.Location = New System.Drawing.Point(264, 63)
        Me.txtQty16.Margin = New System.Windows.Forms.Padding(5)
        Me.txtQty16.Name = "txtQty16"
        Me.txtQty16.Size = New System.Drawing.Size(88, 30)
        Me.txtQty16.TabIndex = 22
        '
        'txtPriceRustProof
        '
        Me.txtPriceRustProof.Location = New System.Drawing.Point(744, 109)
        Me.txtPriceRustProof.Margin = New System.Windows.Forms.Padding(5)
        Me.txtPriceRustProof.Name = "txtPriceRustProof"
        Me.txtPriceRustProof.Size = New System.Drawing.Size(88, 30)
        Me.txtPriceRustProof.TabIndex = 21
        '
        'txtPriceSwitch
        '
        Me.txtPriceSwitch.Location = New System.Drawing.Point(552, 109)
        Me.txtPriceSwitch.Margin = New System.Windows.Forms.Padding(5)
        Me.txtPriceSwitch.Name = "txtPriceSwitch"
        Me.txtPriceSwitch.Size = New System.Drawing.Size(88, 30)
        Me.txtPriceSwitch.TabIndex = 20
        '
        'txtPrice4
        '
        Me.txtPrice4.Location = New System.Drawing.Point(456, 109)
        Me.txtPrice4.Margin = New System.Windows.Forms.Padding(5)
        Me.txtPrice4.Name = "txtPrice4"
        Me.txtPrice4.Size = New System.Drawing.Size(88, 30)
        Me.txtPrice4.TabIndex = 19
        '
        'txtPrice20
        '
        Me.txtPrice20.Location = New System.Drawing.Point(168, 109)
        Me.txtPrice20.Margin = New System.Windows.Forms.Padding(5)
        Me.txtPrice20.Name = "txtPrice20"
        Me.txtPrice20.Size = New System.Drawing.Size(88, 30)
        Me.txtPrice20.TabIndex = 18
        '
        'txtPrice50
        '
        Me.txtPrice50.Location = New System.Drawing.Point(72, 109)
        Me.txtPrice50.Margin = New System.Windows.Forms.Padding(5)
        Me.txtPrice50.Name = "txtPrice50"
        Me.txtPrice50.Size = New System.Drawing.Size(88, 30)
        Me.txtPrice50.TabIndex = 17
        '
        'txtQtySpraying
        '
        Me.txtQtySpraying.Location = New System.Drawing.Point(840, 63)
        Me.txtQtySpraying.Margin = New System.Windows.Forms.Padding(5)
        Me.txtQtySpraying.Name = "txtQtySpraying"
        Me.txtQtySpraying.Size = New System.Drawing.Size(88, 30)
        Me.txtQtySpraying.TabIndex = 16
        '
        'txtQtyRustProof
        '
        Me.txtQtyRustProof.Location = New System.Drawing.Point(744, 63)
        Me.txtQtyRustProof.Margin = New System.Windows.Forms.Padding(5)
        Me.txtQtyRustProof.Name = "txtQtyRustProof"
        Me.txtQtyRustProof.Size = New System.Drawing.Size(88, 30)
        Me.txtQtyRustProof.TabIndex = 15
        '
        'txtQty20
        '
        Me.txtQty20.Location = New System.Drawing.Point(168, 63)
        Me.txtQty20.Margin = New System.Windows.Forms.Padding(5)
        Me.txtQty20.Name = "txtQty20"
        Me.txtQty20.Size = New System.Drawing.Size(88, 30)
        Me.txtQty20.TabIndex = 14
        '
        'txtQty50
        '
        Me.txtQty50.Location = New System.Drawing.Point(72, 63)
        Me.txtQty50.Margin = New System.Windows.Forms.Padding(5)
        Me.txtQty50.Name = "txtQty50"
        Me.txtQty50.Size = New System.Drawing.Size(88, 30)
        Me.txtQty50.TabIndex = 13
        '
        'Label263
        '
        Me.Label263.AutoSize = True
        Me.Label263.Location = New System.Drawing.Point(859, 28)
        Me.Label263.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label263.Name = "Label263"
        Me.Label263.Size = New System.Drawing.Size(51, 19)
        Me.Label263.TabIndex = 12
        Me.Label263.Text = "噴字"
        '
        'Label262
        '
        Me.Label262.AutoSize = True
        Me.Label262.Location = New System.Drawing.Point(763, 28)
        Me.Label262.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label262.Name = "Label262"
        Me.Label262.Size = New System.Drawing.Size(51, 19)
        Me.Label262.TabIndex = 11
        Me.Label262.Text = "防鏽"
        '
        'Label261
        '
        Me.Label261.AutoSize = True
        Me.Label261.Location = New System.Drawing.Point(667, 28)
        Me.Label261.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label261.Name = "Label261"
        Me.Label261.Size = New System.Drawing.Size(51, 19)
        Me.Label261.TabIndex = 10
        Me.Label261.Text = "運費"
        '
        'Label260
        '
        Me.Label260.AutoSize = True
        Me.Label260.Location = New System.Drawing.Point(560, 28)
        Me.Label260.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label260.Name = "Label260"
        Me.Label260.Size = New System.Drawing.Size(72, 19)
        Me.Label260.TabIndex = 9
        Me.Label260.Text = "開關頭"
        '
        'Label259
        '
        Me.Label259.AutoSize = True
        Me.Label259.Location = New System.Drawing.Point(490, 28)
        Me.Label259.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label259.Name = "Label259"
        Me.Label259.Size = New System.Drawing.Size(20, 19)
        Me.Label259.TabIndex = 8
        Me.Label259.Text = "4"
        '
        'Label258
        '
        Me.Label258.AutoSize = True
        Me.Label258.Location = New System.Drawing.Point(389, 28)
        Me.Label258.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label258.Name = "Label258"
        Me.Label258.Size = New System.Drawing.Size(31, 19)
        Me.Label258.TabIndex = 7
        Me.Label258.Text = "10"
        '
        'Label257
        '
        Me.Label257.AutoSize = True
        Me.Label257.Location = New System.Drawing.Point(293, 28)
        Me.Label257.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label257.Name = "Label257"
        Me.Label257.Size = New System.Drawing.Size(31, 19)
        Me.Label257.TabIndex = 6
        Me.Label257.Text = "16"
        '
        'Label256
        '
        Me.Label256.AutoSize = True
        Me.Label256.Location = New System.Drawing.Point(197, 28)
        Me.Label256.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label256.Name = "Label256"
        Me.Label256.Size = New System.Drawing.Size(31, 19)
        Me.Label256.TabIndex = 5
        Me.Label256.Text = "20"
        '
        'Label255
        '
        Me.Label255.AutoSize = True
        Me.Label255.Location = New System.Drawing.Point(101, 28)
        Me.Label255.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label255.Name = "Label255"
        Me.Label255.Size = New System.Drawing.Size(31, 19)
        Me.Label255.TabIndex = 4
        Me.Label255.Text = "50"
        '
        'Label252
        '
        Me.Label252.AutoSize = True
        Me.Label252.Location = New System.Drawing.Point(11, 161)
        Me.Label252.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label252.Name = "Label252"
        Me.Label252.Size = New System.Drawing.Size(51, 19)
        Me.Label252.TabIndex = 3
        Me.Label252.Text = "金額"
        '
        'Label249
        '
        Me.Label249.AutoSize = True
        Me.Label249.Location = New System.Drawing.Point(11, 115)
        Me.Label249.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label249.Name = "Label249"
        Me.Label249.Size = New System.Drawing.Size(51, 19)
        Me.Label249.TabIndex = 2
        Me.Label249.Text = "價格"
        '
        'Label162
        '
        Me.Label162.AutoSize = True
        Me.Label162.Location = New System.Drawing.Point(11, 69)
        Me.Label162.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label162.Name = "Label162"
        Me.Label162.Size = New System.Drawing.Size(51, 19)
        Me.Label162.TabIndex = 1
        Me.Label162.Text = "數量"
        '
        'Label161
        '
        Me.Label161.AutoSize = True
        Me.Label161.Location = New System.Drawing.Point(10, 28)
        Me.Label161.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label161.Name = "Label161"
        Me.Label161.Size = New System.Drawing.Size(51, 19)
        Me.Label161.TabIndex = 0
        Me.Label161.Text = "公斤"
        '
        'txtCusId_ins
        '
        Me.txtCusId_ins.Location = New System.Drawing.Point(490, 93)
        Me.txtCusId_ins.Margin = New System.Windows.Forms.Padding(5)
        Me.txtCusId_ins.Name = "txtCusId_ins"
        Me.txtCusId_ins.ReadOnly = True
        Me.txtCusId_ins.Size = New System.Drawing.Size(300, 30)
        Me.txtCusId_ins.TabIndex = 472
        Me.txtCusId_ins.Tag = "cus_code"
        Me.txtCusId_ins.Visible = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Button2.Location = New System.Drawing.Point(161, 107)
        Me.Button2.Margin = New System.Windows.Forms.Padding(5)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(140, 44)
        Me.Button2.TabIndex = 471
        Me.Button2.Text = "列   印"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.Lime
        Me.Button3.Location = New System.Drawing.Point(5, 107)
        Me.Button3.Margin = New System.Windows.Forms.Padding(5)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(140, 44)
        Me.Button3.TabIndex = 470
        Me.Button3.Text = "查  詢"
        Me.Button3.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Button4.Location = New System.Drawing.Point(473, 161)
        Me.Button4.Margin = New System.Windows.Forms.Padding(5)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(140, 44)
        Me.Button4.TabIndex = 469
        Me.Button4.Text = "取  消"
        Me.Button4.UseVisualStyleBackColor = False
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Button5.Location = New System.Drawing.Point(317, 161)
        Me.Button5.Margin = New System.Windows.Forms.Padding(5)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(140, 44)
        Me.Button5.TabIndex = 468
        Me.Button5.Text = "刪  除"
        Me.Button5.UseVisualStyleBackColor = False
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Button6.Location = New System.Drawing.Point(161, 161)
        Me.Button6.Margin = New System.Windows.Forms.Padding(5)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(140, 44)
        Me.Button6.TabIndex = 467
        Me.Button6.Text = "修  改"
        Me.Button6.UseVisualStyleBackColor = False
        '
        'Button7
        '
        Me.Button7.AutoSize = True
        Me.Button7.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Button7.Location = New System.Drawing.Point(5, 161)
        Me.Button7.Margin = New System.Windows.Forms.Padding(5)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(140, 44)
        Me.Button7.TabIndex = 466
        Me.Button7.Tag = ""
        Me.Button7.Text = "新  增"
        Me.Button7.UseVisualStyleBackColor = False
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.DataGridView1.Location = New System.Drawing.Point(0, 214)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(4)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(1882, 735)
        Me.DataGridView1.TabIndex = 473
        '
        'ucInspection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.txtCusId_ins)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.grpIns)
        Me.Controls.Add(Me.txtCusCode_ins)
        Me.Controls.Add(Me.txtCusName_ins)
        Me.Controls.Add(Me.btnCus_ins)
        Me.Controls.Add(Me.Label102)
        Me.Controls.Add(Me.Label116)
        Me.Controls.Add(Me.dtpIns)
        Me.Controls.Add(Me.Label94)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.Name = "ucInspection"
        Me.Size = New System.Drawing.Size(1882, 949)
        Me.grpIns.ResumeLayout(False)
        Me.grpIns.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtCusCode_ins As TextBox
    Friend WithEvents txtCusName_ins As TextBox
    Friend WithEvents btnCus_ins As Button
    Friend WithEvents Label102 As Label
    Friend WithEvents Label116 As Label
    Friend WithEvents dtpIns As DateTimePicker
    Friend WithEvents Label94 As Label
    Friend WithEvents grpIns As GroupBox
    Friend WithEvents txtAmount20 As TextBox
    Friend WithEvents txtAmount50 As TextBox
    Friend WithEvents txtPriceTotal As TextBox
    Friend WithEvents Label265 As Label
    Friend WithEvents txtAmountTotal As TextBox
    Friend WithEvents txtAmountSpraying As TextBox
    Friend WithEvents txtAmountRustProof As TextBox
    Friend WithEvents txtAmountFreight As TextBox
    Friend WithEvents txtAmountSwitch As TextBox
    Friend WithEvents txtAmount4 As TextBox
    Friend WithEvents txtAmount10 As TextBox
    Friend WithEvents txtAmount16 As TextBox
    Friend WithEvents txtQtyTotal As TextBox
    Friend WithEvents txtPriceSpraying As TextBox
    Friend WithEvents txtPriceFreight As TextBox
    Friend WithEvents txtPrice10 As TextBox
    Friend WithEvents txtPrice16 As TextBox
    Friend WithEvents txtQtyFreight As TextBox
    Friend WithEvents txtQtySwitch As TextBox
    Friend WithEvents txtQty4 As TextBox
    Friend WithEvents txtQty10 As TextBox
    Friend WithEvents txtQty16 As TextBox
    Friend WithEvents txtPriceRustProof As TextBox
    Friend WithEvents txtPriceSwitch As TextBox
    Friend WithEvents txtPrice4 As TextBox
    Friend WithEvents txtPrice20 As TextBox
    Friend WithEvents txtPrice50 As TextBox
    Friend WithEvents txtQtySpraying As TextBox
    Friend WithEvents txtQtyRustProof As TextBox
    Friend WithEvents txtQty20 As TextBox
    Friend WithEvents txtQty50 As TextBox
    Friend WithEvents Label263 As Label
    Friend WithEvents Label262 As Label
    Friend WithEvents Label261 As Label
    Friend WithEvents Label260 As Label
    Friend WithEvents Label259 As Label
    Friend WithEvents Label258 As Label
    Friend WithEvents Label257 As Label
    Friend WithEvents Label256 As Label
    Friend WithEvents Label255 As Label
    Friend WithEvents Label252 As Label
    Friend WithEvents Label249 As Label
    Friend WithEvents Label162 As Label
    Friend WithEvents Label161 As Label
    Friend WithEvents txtCusId_ins As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents DataGridView1 As DataGridView
End Class
