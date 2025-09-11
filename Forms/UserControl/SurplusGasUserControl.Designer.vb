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
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtPlatform = New System.Windows.Forms.TextBox()
        Me.txtPlatform_C = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSlot_C = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtSlot = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtCar_C = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtCar = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSell = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.dtpStart = New System.Windows.Forms.DateTimePicker()
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.dtpMonth = New System.Windows.Forms.DateTimePicker()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtTotal = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtId = New System.Windows.Forms.TextBox()
        Me.btnPrint = New System.Windows.Forms.Button()
        CType(Me.dgvPurchaseBarrel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvPurchaseBarrel
        '
        Me.dgvPurchaseBarrel.AllowUserToAddRows = False
        Me.dgvPurchaseBarrel.AllowUserToDeleteRows = False
        Me.dgvPurchaseBarrel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPurchaseBarrel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvPurchaseBarrel.Location = New System.Drawing.Point(10, 179)
        Me.dgvPurchaseBarrel.Name = "dgvPurchaseBarrel"
        Me.dgvPurchaseBarrel.ReadOnly = True
        Me.dgvPurchaseBarrel.RowTemplate.Height = 24
        Me.dgvPurchaseBarrel.Size = New System.Drawing.Size(1862, 760)
        Me.dgvPurchaseBarrel.TabIndex = 440
        '
        'btnQuery_pb
        '
        Me.btnQuery_pb.BackColor = System.Drawing.Color.Lime
        Me.btnQuery_pb.Location = New System.Drawing.Point(639, 105)
        Me.btnQuery_pb.Name = "btnQuery_pb"
        Me.btnQuery_pb.Size = New System.Drawing.Size(140, 44)
        Me.btnQuery_pb.TabIndex = 439
        Me.btnQuery_pb.Text = "查  詢"
        Me.btnQuery_pb.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel.Location = New System.Drawing.Point(483, 105)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel.TabIndex = 438
        Me.btnCancel.Text = "取  消"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 19)
        Me.Label1.TabIndex = 441
        Me.Label1.Text = "台普"
        '
        'txtPlatform
        '
        Me.txtPlatform.Location = New System.Drawing.Point(71, 59)
        Me.txtPlatform.Name = "txtPlatform"
        Me.txtPlatform.Size = New System.Drawing.Size(100, 30)
        Me.txtPlatform.TabIndex = 442
        Me.txtPlatform.Tag = "sg_Platform"
        '
        'txtPlatform_C
        '
        Me.txtPlatform_C.Location = New System.Drawing.Point(234, 59)
        Me.txtPlatform_C.Name = "txtPlatform_C"
        Me.txtPlatform_C.Size = New System.Drawing.Size(100, 30)
        Me.txtPlatform_C.TabIndex = 444
        Me.txtPlatform_C.Tag = "sg_Platform_C"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(177, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 19)
        Me.Label2.TabIndex = 443
        Me.Label2.Text = "台丙"
        '
        'txtSlot_C
        '
        Me.txtSlot_C.Location = New System.Drawing.Point(560, 59)
        Me.txtSlot_C.Name = "txtSlot_C"
        Me.txtSlot_C.Size = New System.Drawing.Size(100, 30)
        Me.txtSlot_C.TabIndex = 448
        Me.txtSlot_C.Tag = "sg_Slot_C"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(503, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 19)
        Me.Label3.TabIndex = 447
        Me.Label3.Text = "槽丙"
        '
        'txtSlot
        '
        Me.txtSlot.Location = New System.Drawing.Point(397, 59)
        Me.txtSlot.Name = "txtSlot"
        Me.txtSlot.Size = New System.Drawing.Size(100, 30)
        Me.txtSlot.TabIndex = 446
        Me.txtSlot.Tag = "sg_Slot"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(340, 65)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 19)
        Me.Label4.TabIndex = 445
        Me.Label4.Text = "槽普"
        '
        'txtCar_C
        '
        Me.txtCar_C.Location = New System.Drawing.Point(886, 59)
        Me.txtCar_C.Name = "txtCar_C"
        Me.txtCar_C.Size = New System.Drawing.Size(100, 30)
        Me.txtCar_C.TabIndex = 452
        Me.txtCar_C.Tag = "sg_Car_C"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(829, 65)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 19)
        Me.Label5.TabIndex = 451
        Me.Label5.Text = "車丙"
        '
        'txtCar
        '
        Me.txtCar.Location = New System.Drawing.Point(723, 59)
        Me.txtCar.Name = "txtCar"
        Me.txtCar.Size = New System.Drawing.Size(100, 30)
        Me.txtCar.TabIndex = 450
        Me.txtCar.Tag = "sg_Car"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(666, 65)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(51, 19)
        Me.Label6.TabIndex = 449
        Me.Label6.Text = "車普"
        '
        'txtSell
        '
        Me.txtSell.Location = New System.Drawing.Point(1049, 59)
        Me.txtSell.Name = "txtSell"
        Me.txtSell.Size = New System.Drawing.Size(100, 30)
        Me.txtSell.TabIndex = 454
        Me.txtSell.Tag = "sg_Sell"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(992, 65)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(51, 19)
        Me.Label7.TabIndex = 453
        Me.Label7.Text = "現銷"
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete.Location = New System.Drawing.Point(327, 105)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(5)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete.TabIndex = 457
        Me.btnDelete.Text = "刪  除"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit.Location = New System.Drawing.Point(171, 105)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(5)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(140, 44)
        Me.btnEdit.TabIndex = 456
        Me.btnEdit.Text = "修  改"
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnAdd.Location = New System.Drawing.Point(15, 105)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(5)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(140, 44)
        Me.btnAdd.TabIndex = 455
        Me.btnAdd.Tag = ""
        Me.btnAdd.Text = "新  增"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(13, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(72, 19)
        Me.Label8.TabIndex = 458
        Me.Label8.Text = "日期起"
        '
        'dtpStart
        '
        Me.dtpStart.Location = New System.Drawing.Point(91, 13)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(190, 30)
        Me.dtpStart.TabIndex = 459
        Me.dtpStart.Tag = "sg_StartDate"
        '
        'dtpEnd
        '
        Me.dtpEnd.Location = New System.Drawing.Point(365, 13)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(190, 30)
        Me.dtpEnd.TabIndex = 461
        Me.dtpEnd.Tag = "sg_EndDate"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(287, 19)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(72, 19)
        Me.Label9.TabIndex = 460
        Me.Label9.Text = "日期迄"
        '
        'dtpMonth
        '
        Me.dtpMonth.CustomFormat = "yyyy年MM月"
        Me.dtpMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpMonth.Location = New System.Drawing.Point(618, 13)
        Me.dtpMonth.Name = "dtpMonth"
        Me.dtpMonth.Size = New System.Drawing.Size(190, 30)
        Me.dtpMonth.TabIndex = 463
        Me.dtpMonth.Tag = "sg_Moth"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(561, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(51, 19)
        Me.Label10.TabIndex = 462
        Me.Label10.Text = "月份"
        '
        'txtTotal
        '
        Me.txtTotal.Location = New System.Drawing.Point(1233, 59)
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.ReadOnly = True
        Me.txtTotal.Size = New System.Drawing.Size(100, 30)
        Me.txtTotal.TabIndex = 465
        Me.txtTotal.Tag = "sg_Total"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(1155, 65)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(72, 19)
        Me.Label11.TabIndex = 464
        Me.Label11.Text = "總庫存"
        '
        'txtId
        '
        Me.txtId.Location = New System.Drawing.Point(941, 115)
        Me.txtId.Name = "txtId"
        Me.txtId.ReadOnly = True
        Me.txtId.Size = New System.Drawing.Size(100, 30)
        Me.txtId.TabIndex = 466
        Me.txtId.Tag = "sg_Id"
        Me.txtId.Visible = False
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.Cyan
        Me.btnPrint.Location = New System.Drawing.Point(795, 105)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(140, 44)
        Me.btnPrint.TabIndex = 467
        Me.btnPrint.Text = "列  印"
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'SurplusGasUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.txtId)
        Me.Controls.Add(Me.txtTotal)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.dtpMonth)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.dtpEnd)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.dtpStart)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.txtSell)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtCar_C)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtCar)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtSlot_C)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtSlot)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtPlatform_C)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtPlatform)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dgvPurchaseBarrel)
        Me.Controls.Add(Me.btnQuery_pb)
        Me.Controls.Add(Me.btnCancel)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(10)
        Me.Name = "SurplusGasUserControl"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.Size = New System.Drawing.Size(1882, 949)
        CType(Me.dgvPurchaseBarrel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvPurchaseBarrel As DataGridView
    Friend WithEvents btnQuery_pb As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtPlatform As TextBox
    Friend WithEvents txtPlatform_C As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtSlot_C As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtSlot As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtCar_C As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtCar As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtSell As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents dtpStart As DateTimePicker
    Friend WithEvents dtpEnd As DateTimePicker
    Friend WithEvents Label9 As Label
    Friend WithEvents dtpMonth As DateTimePicker
    Friend WithEvents Label10 As Label
    Friend WithEvents txtTotal As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtId As TextBox
    Friend WithEvents btnPrint As Button
End Class
