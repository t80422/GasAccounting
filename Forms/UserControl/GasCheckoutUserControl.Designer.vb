<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GasCheckoutUserControl
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
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.cmbVendor = New System.Windows.Forms.ComboBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnCheckout = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dgvGasCheckout = New System.Windows.Forms.DataGridView()
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker()
        Me.Label139 = New System.Windows.Forms.Label()
        Me.dtpStart = New System.Windows.Forms.DateTimePicker()
        Me.Label138 = New System.Windows.Forms.Label()
        CType(Me.dgvGasCheckout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Location = New System.Drawing.Point(514, 17)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(112, 23)
        Me.chkDate.TabIndex = 432
        Me.chkDate.Text = "搜尋日期"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'cmbVendor
        '
        Me.cmbVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVendor.FormattingEnabled = True
        Me.cmbVendor.Items.AddRange(New Object() {"現金", "支票", "銀行"})
        Me.cmbVendor.Location = New System.Drawing.Point(733, 15)
        Me.cmbVendor.Name = "cmbVendor"
        Me.cmbVendor.Size = New System.Drawing.Size(250, 27)
        Me.cmbVendor.TabIndex = 431
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel.Location = New System.Drawing.Point(325, 49)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel.TabIndex = 430
        Me.btnCancel.Text = "取  消"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnCheckout
        '
        Me.btnCheckout.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCheckout.Location = New System.Drawing.Point(169, 49)
        Me.btnCheckout.Name = "btnCheckout"
        Me.btnCheckout.Size = New System.Drawing.Size(140, 44)
        Me.btnCheckout.TabIndex = 429
        Me.btnCheckout.Text = "結  帳"
        Me.btnCheckout.UseVisualStyleBackColor = False
        '
        'btnSearch
        '
        Me.btnSearch.BackColor = System.Drawing.Color.Lime
        Me.btnSearch.Location = New System.Drawing.Point(13, 49)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(140, 44)
        Me.btnSearch.TabIndex = 428
        Me.btnSearch.Text = "查  詢"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'dgvGasCheckout
        '
        Me.dgvGasCheckout.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvGasCheckout.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvGasCheckout.Location = New System.Drawing.Point(10, 119)
        Me.dgvGasCheckout.Name = "dgvGasCheckout"
        Me.dgvGasCheckout.ReadOnly = True
        Me.dgvGasCheckout.RowTemplate.Height = 24
        Me.dgvGasCheckout.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvGasCheckout.Size = New System.Drawing.Size(1862, 820)
        Me.dgvGasCheckout.TabIndex = 427
        '
        'dtpEnd
        '
        Me.dtpEnd.Location = New System.Drawing.Point(315, 13)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(193, 30)
        Me.dtpEnd.TabIndex = 424
        '
        'Label139
        '
        Me.Label139.AutoSize = True
        Me.Label139.Location = New System.Drawing.Point(632, 19)
        Me.Label139.Name = "Label139"
        Me.Label139.Size = New System.Drawing.Size(95, 19)
        Me.Label139.TabIndex = 423
        Me.Label139.Text = "廠    商"
        '
        'dtpStart
        '
        Me.dtpStart.Location = New System.Drawing.Point(114, 13)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(195, 30)
        Me.dtpStart.TabIndex = 422
        '
        'Label138
        '
        Me.Label138.AutoSize = True
        Me.Label138.Location = New System.Drawing.Point(13, 19)
        Me.Label138.Name = "Label138"
        Me.Label138.Size = New System.Drawing.Size(95, 19)
        Me.Label138.TabIndex = 421
        Me.Label138.Text = "日    期"
        '
        'GasCheckoutUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.chkDate)
        Me.Controls.Add(Me.cmbVendor)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnCheckout)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.dgvGasCheckout)
        Me.Controls.Add(Me.dtpEnd)
        Me.Controls.Add(Me.Label139)
        Me.Controls.Add(Me.dtpStart)
        Me.Controls.Add(Me.Label138)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "GasCheckoutUserControl"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.Size = New System.Drawing.Size(1882, 949)
        CType(Me.dgvGasCheckout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents chkDate As CheckBox
    Friend WithEvents cmbVendor As ComboBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnCheckout As Button
    Friend WithEvents btnSearch As Button
    Friend WithEvents dgvGasCheckout As DataGridView
    Friend WithEvents dtpEnd As DateTimePicker
    Friend WithEvents Label139 As Label
    Friend WithEvents dtpStart As DateTimePicker
    Friend WithEvents Label138 As Label
End Class
