<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CompanyUserControl
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
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label232 = New System.Windows.Forms.Label()
        Me.Label312 = New System.Windows.Forms.Label()
        Me.Label311 = New System.Windows.Forms.Label()
        Me.Label310 = New System.Windows.Forms.Label()
        Me.btnCancel_comp = New System.Windows.Forms.Button()
        Me.btnDelete_comp = New System.Windows.Forms.Button()
        Me.btnEdit_comp = New System.Windows.Forms.Button()
        Me.btnAdd_comp = New System.Windows.Forms.Button()
        Me.dgvCompany = New System.Windows.Forms.DataGridView()
        Me.txtMemo_comp = New System.Windows.Forms.TextBox()
        Me.Label68 = New System.Windows.Forms.Label()
        Me.txtTaxID = New System.Windows.Forms.TextBox()
        Me.Label67 = New System.Windows.Forms.Label()
        Me.txtShortName = New System.Windows.Forms.TextBox()
        Me.Label66 = New System.Windows.Forms.Label()
        Me.txtName_comp = New System.Windows.Forms.TextBox()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.txtID_comp = New System.Windows.Forms.TextBox()
        CType(Me.dgvCompany, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(844, 13)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(150, 30)
        Me.TextBox2.TabIndex = 366
        Me.TextBox2.Tag = "comp_Phone"
        '
        'Label232
        '
        Me.Label232.AutoSize = True
        Me.Label232.Location = New System.Drawing.Point(741, 19)
        Me.Label232.Name = "Label232"
        Me.Label232.Size = New System.Drawing.Size(95, 19)
        Me.Label232.TabIndex = 365
        Me.Label232.Text = "電    話"
        '
        'Label312
        '
        Me.Label312.AutoSize = True
        Me.Label312.ForeColor = System.Drawing.Color.Red
        Me.Label312.Location = New System.Drawing.Point(1002, 19)
        Me.Label312.Name = "Label312"
        Me.Label312.Size = New System.Drawing.Size(20, 19)
        Me.Label312.TabIndex = 361
        Me.Label312.Text = "*"
        Me.Label312.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label311
        '
        Me.Label311.AutoSize = True
        Me.Label311.ForeColor = System.Drawing.Color.Red
        Me.Label311.Location = New System.Drawing.Point(452, 19)
        Me.Label311.Name = "Label311"
        Me.Label311.Size = New System.Drawing.Size(20, 19)
        Me.Label311.TabIndex = 360
        Me.Label311.Text = "*"
        Me.Label311.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label310
        '
        Me.Label310.AutoSize = True
        Me.Label310.ForeColor = System.Drawing.Color.Red
        Me.Label310.Location = New System.Drawing.Point(13, 19)
        Me.Label310.Name = "Label310"
        Me.Label310.Size = New System.Drawing.Size(20, 19)
        Me.Label310.TabIndex = 359
        Me.Label310.Text = "*"
        Me.Label310.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnCancel_comp
        '
        Me.btnCancel_comp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel_comp.Location = New System.Drawing.Point(481, 49)
        Me.btnCancel_comp.Name = "btnCancel_comp"
        Me.btnCancel_comp.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel_comp.TabIndex = 358
        Me.btnCancel_comp.Text = "取  消"
        Me.btnCancel_comp.UseVisualStyleBackColor = False
        '
        'btnDelete_comp
        '
        Me.btnDelete_comp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete_comp.Location = New System.Drawing.Point(325, 49)
        Me.btnDelete_comp.Name = "btnDelete_comp"
        Me.btnDelete_comp.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete_comp.TabIndex = 357
        Me.btnDelete_comp.Text = "刪  除"
        Me.btnDelete_comp.UseVisualStyleBackColor = False
        '
        'btnEdit_comp
        '
        Me.btnEdit_comp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit_comp.Location = New System.Drawing.Point(169, 49)
        Me.btnEdit_comp.Name = "btnEdit_comp"
        Me.btnEdit_comp.Size = New System.Drawing.Size(140, 44)
        Me.btnEdit_comp.TabIndex = 356
        Me.btnEdit_comp.Text = "修  改"
        Me.btnEdit_comp.UseVisualStyleBackColor = False
        '
        'btnAdd_comp
        '
        Me.btnAdd_comp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnAdd_comp.Location = New System.Drawing.Point(13, 49)
        Me.btnAdd_comp.Name = "btnAdd_comp"
        Me.btnAdd_comp.Size = New System.Drawing.Size(140, 44)
        Me.btnAdd_comp.TabIndex = 355
        Me.btnAdd_comp.Tag = ""
        Me.btnAdd_comp.Text = "新  增"
        Me.btnAdd_comp.UseVisualStyleBackColor = False
        '
        'dgvCompany
        '
        Me.dgvCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCompany.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvCompany.Location = New System.Drawing.Point(10, 119)
        Me.dgvCompany.Name = "dgvCompany"
        Me.dgvCompany.ReadOnly = True
        Me.dgvCompany.RowTemplate.Height = 24
        Me.dgvCompany.Size = New System.Drawing.Size(1862, 820)
        Me.dgvCompany.TabIndex = 354
        '
        'txtMemo_comp
        '
        Me.txtMemo_comp.Location = New System.Drawing.Point(1394, 13)
        Me.txtMemo_comp.Name = "txtMemo_comp"
        Me.txtMemo_comp.Size = New System.Drawing.Size(150, 30)
        Me.txtMemo_comp.TabIndex = 353
        Me.txtMemo_comp.Tag = "comp_memo"
        '
        'Label68
        '
        Me.Label68.AutoSize = True
        Me.Label68.Location = New System.Drawing.Point(1291, 19)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(95, 19)
        Me.Label68.TabIndex = 352
        Me.Label68.Text = "備    註"
        '
        'txtTaxID
        '
        Me.txtTaxID.Location = New System.Drawing.Point(1133, 13)
        Me.txtTaxID.Name = "txtTaxID"
        Me.txtTaxID.Size = New System.Drawing.Size(150, 30)
        Me.txtTaxID.TabIndex = 351
        Me.txtTaxID.Tag = "comp_tax_id"
        '
        'Label67
        '
        Me.Label67.AutoSize = True
        Me.Label67.Location = New System.Drawing.Point(1030, 19)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(95, 19)
        Me.Label67.TabIndex = 350
        Me.Label67.Text = "統    編"
        '
        'txtShortName
        '
        Me.txtShortName.Location = New System.Drawing.Point(583, 13)
        Me.txtShortName.Name = "txtShortName"
        Me.txtShortName.Size = New System.Drawing.Size(150, 30)
        Me.txtShortName.TabIndex = 349
        Me.txtShortName.Tag = "comp_short"
        '
        'Label66
        '
        Me.Label66.AutoSize = True
        Me.Label66.Location = New System.Drawing.Point(480, 19)
        Me.Label66.Name = "Label66"
        Me.Label66.Size = New System.Drawing.Size(95, 19)
        Me.Label66.TabIndex = 348
        Me.Label66.Text = "簡    稱"
        '
        'txtName_comp
        '
        Me.txtName_comp.Location = New System.Drawing.Point(144, 13)
        Me.txtName_comp.Name = "txtName_comp"
        Me.txtName_comp.Size = New System.Drawing.Size(300, 30)
        Me.txtName_comp.TabIndex = 347
        Me.txtName_comp.Tag = "comp_name"
        '
        'Label62
        '
        Me.Label62.AutoSize = True
        Me.Label62.Location = New System.Drawing.Point(41, 19)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(95, 19)
        Me.Label62.TabIndex = 346
        Me.Label62.Text = "名    稱"
        '
        'txtID_comp
        '
        Me.txtID_comp.Location = New System.Drawing.Point(627, 59)
        Me.txtID_comp.Name = "txtID_comp"
        Me.txtID_comp.ReadOnly = True
        Me.txtID_comp.Size = New System.Drawing.Size(150, 30)
        Me.txtID_comp.TabIndex = 345
        Me.txtID_comp.Tag = "comp_id"
        Me.txtID_comp.Visible = False
        '
        'CompanyUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Label232)
        Me.Controls.Add(Me.Label312)
        Me.Controls.Add(Me.Label311)
        Me.Controls.Add(Me.Label310)
        Me.Controls.Add(Me.btnCancel_comp)
        Me.Controls.Add(Me.btnDelete_comp)
        Me.Controls.Add(Me.btnEdit_comp)
        Me.Controls.Add(Me.btnAdd_comp)
        Me.Controls.Add(Me.dgvCompany)
        Me.Controls.Add(Me.txtMemo_comp)
        Me.Controls.Add(Me.Label68)
        Me.Controls.Add(Me.txtTaxID)
        Me.Controls.Add(Me.Label67)
        Me.Controls.Add(Me.txtShortName)
        Me.Controls.Add(Me.Label66)
        Me.Controls.Add(Me.txtName_comp)
        Me.Controls.Add(Me.Label62)
        Me.Controls.Add(Me.txtID_comp)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "CompanyUserControl"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.Size = New System.Drawing.Size(1882, 949)
        CType(Me.dgvCompany, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label232 As Label
    Friend WithEvents Label312 As Label
    Friend WithEvents Label311 As Label
    Friend WithEvents Label310 As Label
    Friend WithEvents btnCancel_comp As Button
    Friend WithEvents btnDelete_comp As Button
    Friend WithEvents btnEdit_comp As Button
    Friend WithEvents btnAdd_comp As Button
    Friend WithEvents dgvCompany As DataGridView
    Friend WithEvents txtMemo_comp As TextBox
    Friend WithEvents Label68 As Label
    Friend WithEvents txtTaxID As TextBox
    Friend WithEvents Label67 As Label
    Friend WithEvents txtShortName As TextBox
    Friend WithEvents Label66 As Label
    Friend WithEvents txtName_comp As TextBox
    Friend WithEvents Label62 As Label
    Friend WithEvents txtID_comp As TextBox
End Class
