<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQueryCustomer
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
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
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnQuery = New System.Windows.Forms.Button()
        Me.txtCusPhone = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txtCusContactPerson = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtCusName = New System.Windows.Forms.TextBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.txtCusCode = New System.Windows.Forms.TextBox()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.dgvCustomer = New System.Windows.Forms.DataGridView()
        CType(Me.dgvCustomer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnClear
        '
        Me.btnClear.AutoSize = True
        Me.btnClear.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnClear.Location = New System.Drawing.Point(388, 171)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(60, 30)
        Me.btnClear.TabIndex = 358
        Me.btnClear.Text = "清除"
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'btnOK
        '
        Me.btnOK.AutoSize = True
        Me.btnOK.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(456, 171)
        Me.btnOK.Margin = New System.Windows.Forms.Padding(4)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(60, 30)
        Me.btnOK.TabIndex = 357
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'btnQuery
        '
        Me.btnQuery.AutoSize = True
        Me.btnQuery.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnQuery.Location = New System.Drawing.Point(320, 171)
        Me.btnQuery.Margin = New System.Windows.Forms.Padding(4)
        Me.btnQuery.Name = "btnQuery"
        Me.btnQuery.Size = New System.Drawing.Size(60, 30)
        Me.btnQuery.TabIndex = 356
        Me.btnQuery.Text = "查詢"
        Me.btnQuery.UseVisualStyleBackColor = False
        '
        'txtCusPhone
        '
        Me.txtCusPhone.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtCusPhone.Location = New System.Drawing.Point(395, 134)
        Me.txtCusPhone.Margin = New System.Windows.Forms.Padding(0)
        Me.txtCusPhone.Name = "txtCusPhone"
        Me.txtCusPhone.Size = New System.Drawing.Size(121, 27)
        Me.txtCusPhone.TabIndex = 354
        Me.txtCusPhone.Tag = "cus_phone1"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label26.Location = New System.Drawing.Point(317, 135)
        Me.Label26.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(51, 19)
        Me.Label26.TabIndex = 355
        Me.Label26.Text = "電話"
        '
        'txtCusContactPerson
        '
        Me.txtCusContactPerson.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtCusContactPerson.Location = New System.Drawing.Point(395, 91)
        Me.txtCusContactPerson.Margin = New System.Windows.Forms.Padding(0)
        Me.txtCusContactPerson.Name = "txtCusContactPerson"
        Me.txtCusContactPerson.Size = New System.Drawing.Size(121, 27)
        Me.txtCusContactPerson.TabIndex = 352
        Me.txtCusContactPerson.Tag = "cus_contact_person"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label20.Location = New System.Drawing.Point(317, 92)
        Me.Label20.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(72, 19)
        Me.Label20.TabIndex = 353
        Me.Label20.Text = "聯絡人"
        '
        'txtCusName
        '
        Me.txtCusName.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtCusName.Location = New System.Drawing.Point(395, 48)
        Me.txtCusName.Margin = New System.Windows.Forms.Padding(0)
        Me.txtCusName.Name = "txtCusName"
        Me.txtCusName.Size = New System.Drawing.Size(121, 27)
        Me.txtCusName.TabIndex = 350
        Me.txtCusName.Tag = "cus_name"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label27.Location = New System.Drawing.Point(317, 49)
        Me.Label27.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(51, 19)
        Me.Label27.TabIndex = 351
        Me.Label27.Text = "名稱"
        '
        'txtCusCode
        '
        Me.txtCusCode.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtCusCode.Location = New System.Drawing.Point(395, 5)
        Me.txtCusCode.Margin = New System.Windows.Forms.Padding(0)
        Me.txtCusCode.Name = "txtCusCode"
        Me.txtCusCode.Size = New System.Drawing.Size(121, 27)
        Me.txtCusCode.TabIndex = 348
        Me.txtCusCode.Tag = "cus_code"
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label55.Location = New System.Drawing.Point(317, 6)
        Me.Label55.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(51, 19)
        Me.Label55.TabIndex = 349
        Me.Label55.Text = "代號"
        '
        'dgvCustomer
        '
        Me.dgvCustomer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCustomer.Dock = System.Windows.Forms.DockStyle.Left
        Me.dgvCustomer.Location = New System.Drawing.Point(5, 5)
        Me.dgvCustomer.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.dgvCustomer.Name = "dgvCustomer"
        Me.dgvCustomer.RowTemplate.Height = 24
        Me.dgvCustomer.Size = New System.Drawing.Size(300, 206)
        Me.dgvCustomer.TabIndex = 347
        '
        'frmQueryCustomer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(526, 216)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnQuery)
        Me.Controls.Add(Me.txtCusPhone)
        Me.Controls.Add(Me.Label26)
        Me.Controls.Add(Me.txtCusContactPerson)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.txtCusName)
        Me.Controls.Add(Me.Label27)
        Me.Controls.Add(Me.txtCusCode)
        Me.Controls.Add(Me.Label55)
        Me.Controls.Add(Me.dgvCustomer)
        Me.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmQueryCustomer"
        Me.Padding = New System.Windows.Forms.Padding(5)
        Me.Text = "客戶查詢"
        CType(Me.dgvCustomer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnClear As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents btnQuery As Button
    Friend WithEvents txtCusPhone As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents txtCusContactPerson As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents txtCusName As TextBox
    Friend WithEvents Label27 As Label
    Friend WithEvents txtCusCode As TextBox
    Friend WithEvents Label55 As Label
    Friend WithEvents dgvCustomer As DataGridView
End Class
