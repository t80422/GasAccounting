<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Cheque_colUserControl
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
        Me.btnQuery_che = New System.Windows.Forms.Button()
        Me.btnSelectAll = New System.Windows.Forms.Button()
        Me.btnChange = New System.Windows.Forms.Button()
        Me.TextBox23 = New System.Windows.Forms.TextBox()
        Me.Label124 = New System.Windows.Forms.Label()
        Me.txtCheId = New System.Windows.Forms.TextBox()
        Me.Label123 = New System.Windows.Forms.Label()
        Me.Label122 = New System.Windows.Forms.Label()
        Me.Label121 = New System.Windows.Forms.Label()
        Me.TextBox21 = New System.Windows.Forms.TextBox()
        Me.Label118 = New System.Windows.Forms.Label()
        Me.TextBox8 = New System.Windows.Forms.TextBox()
        Me.Label117 = New System.Windows.Forms.Label()
        Me.dtpCollectionDate = New System.Windows.Forms.DateTimePicker()
        Me.lblCollectionDate = New System.Windows.Forms.Label()
        Me.btnPrint_cheque = New System.Windows.Forms.Button()
        Me.cmbState_Che = New System.Windows.Forms.ComboBox()
        Me.Label145 = New System.Windows.Forms.Label()
        Me.dtpCashingDate = New System.Windows.Forms.DateTimePicker()
        Me.lblCashingDate = New System.Windows.Forms.Label()
        Me.btnCancel_Che = New System.Windows.Forms.Button()
        Me.TextBox76 = New System.Windows.Forms.TextBox()
        Me.Label148 = New System.Windows.Forms.Label()
        Me.TextBox75 = New System.Windows.Forms.TextBox()
        Me.Label147 = New System.Windows.Forms.Label()
        Me.DateTimePicker7 = New System.Windows.Forms.DateTimePicker()
        Me.Label146 = New System.Windows.Forms.Label()
        Me.dgvCheque = New System.Windows.Forms.DataGridView()
        Me.btnRedeemed = New System.Windows.Forms.Button()
        CType(Me.dgvCheque, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnQuery_che
        '
        Me.btnQuery_che.BackColor = System.Drawing.Color.Lime
        Me.btnQuery_che.Location = New System.Drawing.Point(788, 83)
        Me.btnQuery_che.Name = "btnQuery_che"
        Me.btnQuery_che.Size = New System.Drawing.Size(140, 44)
        Me.btnQuery_che.TabIndex = 463
        Me.btnQuery_che.Text = "查  詢"
        Me.btnQuery_che.UseVisualStyleBackColor = False
        '
        'btnSelectAll
        '
        Me.btnSelectAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnSelectAll.Location = New System.Drawing.Point(8, 83)
        Me.btnSelectAll.Name = "btnSelectAll"
        Me.btnSelectAll.Size = New System.Drawing.Size(140, 44)
        Me.btnSelectAll.TabIndex = 462
        Me.btnSelectAll.Text = "全  選"
        Me.btnSelectAll.UseVisualStyleBackColor = False
        '
        'btnChange
        '
        Me.btnChange.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnChange.Location = New System.Drawing.Point(164, 83)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.Size = New System.Drawing.Size(140, 44)
        Me.btnChange.TabIndex = 461
        Me.btnChange.Text = "轉為已代收"
        Me.btnChange.UseVisualStyleBackColor = False
        '
        'TextBox23
        '
        Me.TextBox23.Location = New System.Drawing.Point(1245, 4)
        Me.TextBox23.Name = "TextBox23"
        Me.TextBox23.Size = New System.Drawing.Size(166, 30)
        Me.TextBox23.TabIndex = 460
        Me.TextBox23.Tag = "che_Memo"
        '
        'Label124
        '
        Me.Label124.AutoSize = True
        Me.Label124.Location = New System.Drawing.Point(1146, 10)
        Me.Label124.Name = "Label124"
        Me.Label124.Size = New System.Drawing.Size(95, 19)
        Me.Label124.TabIndex = 459
        Me.Label124.Text = "備    註"
        '
        'txtCheId
        '
        Me.txtCheId.Location = New System.Drawing.Point(135, 4)
        Me.txtCheId.Name = "txtCheId"
        Me.txtCheId.ReadOnly = True
        Me.txtCheId.Size = New System.Drawing.Size(166, 30)
        Me.txtCheId.TabIndex = 458
        Me.txtCheId.Tag = "che_Id"
        '
        'Label123
        '
        Me.Label123.AutoSize = True
        Me.Label123.Location = New System.Drawing.Point(34, 10)
        Me.Label123.Name = "Label123"
        Me.Label123.Size = New System.Drawing.Size(95, 19)
        Me.Label123.TabIndex = 457
        Me.Label123.Text = "編    號"
        '
        'Label122
        '
        Me.Label122.AutoSize = True
        Me.Label122.ForeColor = System.Drawing.Color.Red
        Me.Label122.Location = New System.Drawing.Point(8, 53)
        Me.Label122.Name = "Label122"
        Me.Label122.Size = New System.Drawing.Size(20, 19)
        Me.Label122.TabIndex = 456
        Me.Label122.Text = "*"
        '
        'Label121
        '
        Me.Label121.AutoSize = True
        Me.Label121.ForeColor = System.Drawing.Color.Red
        Me.Label121.Location = New System.Drawing.Point(849, 10)
        Me.Label121.Name = "Label121"
        Me.Label121.Size = New System.Drawing.Size(20, 19)
        Me.Label121.TabIndex = 455
        Me.Label121.Text = "*"
        '
        'TextBox21
        '
        Me.TextBox21.Location = New System.Drawing.Point(677, 4)
        Me.TextBox21.Name = "TextBox21"
        Me.TextBox21.Size = New System.Drawing.Size(166, 30)
        Me.TextBox21.TabIndex = 454
        Me.TextBox21.Tag = "che_AccountNumber"
        '
        'Label118
        '
        Me.Label118.AutoSize = True
        Me.Label118.Location = New System.Drawing.Point(578, 10)
        Me.Label118.Name = "Label118"
        Me.Label118.Size = New System.Drawing.Size(93, 19)
        Me.Label118.TabIndex = 453
        Me.Label118.Text = "銀行帳號"
        '
        'TextBox8
        '
        Me.TextBox8.Location = New System.Drawing.Point(1247, 47)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.Size = New System.Drawing.Size(166, 30)
        Me.TextBox8.TabIndex = 452
        Me.TextBox8.Tag = "che_IssuerName"
        '
        'Label117
        '
        Me.Label117.AutoSize = True
        Me.Label117.Location = New System.Drawing.Point(1147, 53)
        Me.Label117.Name = "Label117"
        Me.Label117.Size = New System.Drawing.Size(94, 19)
        Me.Label117.TabIndex = 451
        Me.Label117.Text = "發 票 人"
        '
        'dtpCollectionDate
        '
        Me.dtpCollectionDate.Location = New System.Drawing.Point(679, 47)
        Me.dtpCollectionDate.Name = "dtpCollectionDate"
        Me.dtpCollectionDate.Size = New System.Drawing.Size(166, 30)
        Me.dtpCollectionDate.TabIndex = 450
        Me.dtpCollectionDate.Tag = "che_CollectionDate"
        '
        'lblCollectionDate
        '
        Me.lblCollectionDate.AutoSize = True
        Me.lblCollectionDate.Location = New System.Drawing.Point(580, 53)
        Me.lblCollectionDate.Name = "lblCollectionDate"
        Me.lblCollectionDate.Size = New System.Drawing.Size(93, 19)
        Me.lblCollectionDate.TabIndex = 449
        Me.lblCollectionDate.Text = "代收日期"
        '
        'btnPrint_cheque
        '
        Me.btnPrint_cheque.BackColor = System.Drawing.Color.Cyan
        Me.btnPrint_cheque.Location = New System.Drawing.Point(632, 83)
        Me.btnPrint_cheque.Name = "btnPrint_cheque"
        Me.btnPrint_cheque.Size = New System.Drawing.Size(140, 44)
        Me.btnPrint_cheque.TabIndex = 448
        Me.btnPrint_cheque.Text = "列  印"
        Me.btnPrint_cheque.UseVisualStyleBackColor = False
        '
        'cmbState_Che
        '
        Me.cmbState_Che.FormattingEnabled = True
        Me.cmbState_Che.Items.AddRange(New Object() {"已兌現", "已代收", "失效"})
        Me.cmbState_Che.Location = New System.Drawing.Point(408, 49)
        Me.cmbState_Che.Name = "cmbState_Che"
        Me.cmbState_Che.Size = New System.Drawing.Size(166, 27)
        Me.cmbState_Che.TabIndex = 447
        Me.cmbState_Che.Tag = "chu_State"
        '
        'Label145
        '
        Me.Label145.AutoSize = True
        Me.Label145.Location = New System.Drawing.Point(307, 53)
        Me.Label145.Name = "Label145"
        Me.Label145.Size = New System.Drawing.Size(95, 19)
        Me.Label145.TabIndex = 446
        Me.Label145.Text = "狀    態"
        '
        'dtpCashingDate
        '
        Me.dtpCashingDate.Location = New System.Drawing.Point(974, 47)
        Me.dtpCashingDate.Name = "dtpCashingDate"
        Me.dtpCashingDate.Size = New System.Drawing.Size(166, 30)
        Me.dtpCashingDate.TabIndex = 445
        Me.dtpCashingDate.Tag = "che_CashingDate"
        Me.dtpCashingDate.Visible = False
        '
        'lblCashingDate
        '
        Me.lblCashingDate.AutoSize = True
        Me.lblCashingDate.Location = New System.Drawing.Point(875, 53)
        Me.lblCashingDate.Name = "lblCashingDate"
        Me.lblCashingDate.Size = New System.Drawing.Size(93, 19)
        Me.lblCashingDate.TabIndex = 444
        Me.lblCashingDate.Text = "兌現日期"
        Me.lblCashingDate.Visible = False
        '
        'btnCancel_Che
        '
        Me.btnCancel_Che.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel_Che.Location = New System.Drawing.Point(476, 83)
        Me.btnCancel_Che.Name = "btnCancel_Che"
        Me.btnCancel_Che.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel_Che.TabIndex = 442
        Me.btnCancel_Che.Text = "取  消"
        Me.btnCancel_Che.UseVisualStyleBackColor = False
        '
        'TextBox76
        '
        Me.TextBox76.Location = New System.Drawing.Point(135, 47)
        Me.TextBox76.Name = "TextBox76"
        Me.TextBox76.Size = New System.Drawing.Size(166, 30)
        Me.TextBox76.TabIndex = 441
        Me.TextBox76.Tag = "che_Amount"
        '
        'Label148
        '
        Me.Label148.AutoSize = True
        Me.Label148.Location = New System.Drawing.Point(34, 53)
        Me.Label148.Name = "Label148"
        Me.Label148.Size = New System.Drawing.Size(95, 19)
        Me.Label148.TabIndex = 440
        Me.Label148.Text = "金    額"
        '
        'TextBox75
        '
        Me.TextBox75.Location = New System.Drawing.Point(974, 4)
        Me.TextBox75.Name = "TextBox75"
        Me.TextBox75.Size = New System.Drawing.Size(166, 30)
        Me.TextBox75.TabIndex = 439
        Me.TextBox75.Tag = "che_Number"
        '
        'Label147
        '
        Me.Label147.AutoSize = True
        Me.Label147.Location = New System.Drawing.Point(875, 10)
        Me.Label147.Name = "Label147"
        Me.Label147.Size = New System.Drawing.Size(93, 19)
        Me.Label147.TabIndex = 438
        Me.Label147.Text = "支票號碼"
        '
        'DateTimePicker7
        '
        Me.DateTimePicker7.Location = New System.Drawing.Point(406, 4)
        Me.DateTimePicker7.Name = "DateTimePicker7"
        Me.DateTimePicker7.Size = New System.Drawing.Size(166, 30)
        Me.DateTimePicker7.TabIndex = 437
        Me.DateTimePicker7.Tag = "che_ReceivedDate"
        '
        'Label146
        '
        Me.Label146.AutoSize = True
        Me.Label146.Location = New System.Drawing.Point(307, 10)
        Me.Label146.Name = "Label146"
        Me.Label146.Size = New System.Drawing.Size(93, 19)
        Me.Label146.TabIndex = 436
        Me.Label146.Text = "收票日期"
        '
        'dgvCheque
        '
        Me.dgvCheque.AllowUserToAddRows = False
        Me.dgvCheque.AllowUserToDeleteRows = False
        Me.dgvCheque.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCheque.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvCheque.Location = New System.Drawing.Point(0, 149)
        Me.dgvCheque.Name = "dgvCheque"
        Me.dgvCheque.RowTemplate.Height = 24
        Me.dgvCheque.Size = New System.Drawing.Size(1882, 800)
        Me.dgvCheque.TabIndex = 443
        '
        'btnRedeemed
        '
        Me.btnRedeemed.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnRedeemed.Location = New System.Drawing.Point(320, 83)
        Me.btnRedeemed.Name = "btnRedeemed"
        Me.btnRedeemed.Size = New System.Drawing.Size(140, 44)
        Me.btnRedeemed.TabIndex = 464
        Me.btnRedeemed.Text = "轉為已兌現"
        Me.btnRedeemed.UseVisualStyleBackColor = False
        '
        'Cheque_colUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnRedeemed)
        Me.Controls.Add(Me.btnQuery_che)
        Me.Controls.Add(Me.btnSelectAll)
        Me.Controls.Add(Me.btnChange)
        Me.Controls.Add(Me.TextBox23)
        Me.Controls.Add(Me.Label124)
        Me.Controls.Add(Me.txtCheId)
        Me.Controls.Add(Me.Label123)
        Me.Controls.Add(Me.Label122)
        Me.Controls.Add(Me.Label121)
        Me.Controls.Add(Me.TextBox21)
        Me.Controls.Add(Me.Label118)
        Me.Controls.Add(Me.TextBox8)
        Me.Controls.Add(Me.Label117)
        Me.Controls.Add(Me.dtpCollectionDate)
        Me.Controls.Add(Me.lblCollectionDate)
        Me.Controls.Add(Me.btnPrint_cheque)
        Me.Controls.Add(Me.cmbState_Che)
        Me.Controls.Add(Me.Label145)
        Me.Controls.Add(Me.dtpCashingDate)
        Me.Controls.Add(Me.lblCashingDate)
        Me.Controls.Add(Me.btnCancel_Che)
        Me.Controls.Add(Me.TextBox76)
        Me.Controls.Add(Me.Label148)
        Me.Controls.Add(Me.TextBox75)
        Me.Controls.Add(Me.Label147)
        Me.Controls.Add(Me.DateTimePicker7)
        Me.Controls.Add(Me.Label146)
        Me.Controls.Add(Me.dgvCheque)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "Cheque_colUserControl"
        Me.Size = New System.Drawing.Size(1882, 949)
        CType(Me.dgvCheque, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnQuery_che As Button
    Friend WithEvents btnSelectAll As Button
    Friend WithEvents btnChange As Button
    Friend WithEvents TextBox23 As TextBox
    Friend WithEvents Label124 As Label
    Friend WithEvents txtCheId As TextBox
    Friend WithEvents Label123 As Label
    Friend WithEvents Label122 As Label
    Friend WithEvents Label121 As Label
    Friend WithEvents TextBox21 As TextBox
    Friend WithEvents Label118 As Label
    Friend WithEvents TextBox8 As TextBox
    Friend WithEvents Label117 As Label
    Friend WithEvents dtpCollectionDate As DateTimePicker
    Friend WithEvents lblCollectionDate As Label
    Friend WithEvents btnPrint_cheque As Button
    Friend WithEvents cmbState_Che As ComboBox
    Friend WithEvents Label145 As Label
    Friend WithEvents dtpCashingDate As DateTimePicker
    Friend WithEvents lblCashingDate As Label
    Friend WithEvents btnCancel_Che As Button
    Friend WithEvents TextBox76 As TextBox
    Friend WithEvents Label148 As Label
    Friend WithEvents TextBox75 As TextBox
    Friend WithEvents Label147 As Label
    Friend WithEvents DateTimePicker7 As DateTimePicker
    Friend WithEvents Label146 As Label
    Friend WithEvents dgvCheque As DataGridView
    Friend WithEvents btnRedeemed As Button
End Class
