<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SurplusGasUserControl
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
        Me.dgvPurchaseBarrel = New System.Windows.Forms.DataGridView()
        Me.btnQuery_pb = New System.Windows.Forms.Button()
        Me.btnCancel_pb = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnDelete_emp = New System.Windows.Forms.Button()
        Me.btnEdit_emp = New System.Windows.Forms.Button()
        Me.btnAdd_emp = New System.Windows.Forms.Button()
        CType(Me.dgvPurchaseBarrel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvPurchaseBarrel
        '
        Me.dgvPurchaseBarrel.AllowUserToAddRows = False
        Me.dgvPurchaseBarrel.AllowUserToDeleteRows = False
        Me.dgvPurchaseBarrel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPurchaseBarrel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvPurchaseBarrel.Location = New System.Drawing.Point(10, 292)
        Me.dgvPurchaseBarrel.Name = "dgvPurchaseBarrel"
        Me.dgvPurchaseBarrel.ReadOnly = True
        Me.dgvPurchaseBarrel.RowTemplate.Height = 24
        Me.dgvPurchaseBarrel.Size = New System.Drawing.Size(1862, 647)
        Me.dgvPurchaseBarrel.TabIndex = 440
        '
        'btnQuery_pb
        '
        Me.btnQuery_pb.BackColor = System.Drawing.Color.Lime
        Me.btnQuery_pb.Location = New System.Drawing.Point(639, 51)
        Me.btnQuery_pb.Name = "btnQuery_pb"
        Me.btnQuery_pb.Size = New System.Drawing.Size(140, 44)
        Me.btnQuery_pb.TabIndex = 439
        Me.btnQuery_pb.Text = "查  詢"
        Me.btnQuery_pb.UseVisualStyleBackColor = False
        '
        'btnCancel_pb
        '
        Me.btnCancel_pb.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel_pb.Location = New System.Drawing.Point(483, 51)
        Me.btnCancel_pb.Name = "btnCancel_pb"
        Me.btnCancel_pb.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel_pb.TabIndex = 438
        Me.btnCancel_pb.Text = "取  消"
        Me.btnCancel_pb.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 19)
        Me.Label1.TabIndex = 441
        Me.Label1.Text = "台普"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(70, 13)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 30)
        Me.TextBox1.TabIndex = 442
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(233, 13)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(100, 30)
        Me.TextBox2.TabIndex = 444
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(176, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 19)
        Me.Label2.TabIndex = 443
        Me.Label2.Text = "台丙"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(559, 13)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(100, 30)
        Me.TextBox3.TabIndex = 448
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(502, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 19)
        Me.Label3.TabIndex = 447
        Me.Label3.Text = "槽丙"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(396, 13)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(100, 30)
        Me.TextBox4.TabIndex = 446
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(339, 19)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 19)
        Me.Label4.TabIndex = 445
        Me.Label4.Text = "槽普"
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(885, 13)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(100, 30)
        Me.TextBox5.TabIndex = 452
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(828, 19)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 19)
        Me.Label5.TabIndex = 451
        Me.Label5.Text = "車丙"
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(722, 13)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(100, 30)
        Me.TextBox6.TabIndex = 450
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(665, 19)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(51, 19)
        Me.Label6.TabIndex = 449
        Me.Label6.Text = "車普"
        '
        'TextBox7
        '
        Me.TextBox7.Location = New System.Drawing.Point(1048, 13)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(100, 30)
        Me.TextBox7.TabIndex = 454
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(991, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(51, 19)
        Me.Label7.TabIndex = 453
        Me.Label7.Text = "現銷"
        '
        'btnDelete_emp
        '
        Me.btnDelete_emp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete_emp.Location = New System.Drawing.Point(327, 51)
        Me.btnDelete_emp.Margin = New System.Windows.Forms.Padding(5)
        Me.btnDelete_emp.Name = "btnDelete_emp"
        Me.btnDelete_emp.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete_emp.TabIndex = 457
        Me.btnDelete_emp.Text = "刪  除"
        Me.btnDelete_emp.UseVisualStyleBackColor = False
        '
        'btnEdit_emp
        '
        Me.btnEdit_emp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit_emp.Location = New System.Drawing.Point(171, 51)
        Me.btnEdit_emp.Margin = New System.Windows.Forms.Padding(5)
        Me.btnEdit_emp.Name = "btnEdit_emp"
        Me.btnEdit_emp.Size = New System.Drawing.Size(140, 44)
        Me.btnEdit_emp.TabIndex = 456
        Me.btnEdit_emp.Text = "修  改"
        Me.btnEdit_emp.UseVisualStyleBackColor = False
        '
        'btnAdd_emp
        '
        Me.btnAdd_emp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnAdd_emp.Location = New System.Drawing.Point(15, 51)
        Me.btnAdd_emp.Margin = New System.Windows.Forms.Padding(5)
        Me.btnAdd_emp.Name = "btnAdd_emp"
        Me.btnAdd_emp.Size = New System.Drawing.Size(140, 44)
        Me.btnAdd_emp.TabIndex = 455
        Me.btnAdd_emp.Tag = ""
        Me.btnAdd_emp.Text = "新  增"
        Me.btnAdd_emp.UseVisualStyleBackColor = False
        '
        'SurplusGasUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnDelete_emp)
        Me.Controls.Add(Me.btnEdit_emp)
        Me.Controls.Add(Me.btnAdd_emp)
        Me.Controls.Add(Me.TextBox7)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextBox6)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dgvPurchaseBarrel)
        Me.Controls.Add(Me.btnQuery_pb)
        Me.Controls.Add(Me.btnCancel_pb)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "SurplusGasUserControl"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.Size = New System.Drawing.Size(1882, 949)
        CType(Me.dgvPurchaseBarrel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvPurchaseBarrel As DataGridView
    Friend WithEvents btnQuery_pb As Button
    Friend WithEvents btnCancel_pb As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents TextBox7 As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents btnDelete_emp As Button
    Friend WithEvents btnEdit_emp As Button
    Friend WithEvents btnAdd_emp As Button
End Class
