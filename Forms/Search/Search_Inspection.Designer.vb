<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Search_Inspection
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
        Me.btnOK = New System.Windows.Forms.Button()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCusCode = New System.Windows.Forms.TextBox()
        Me.txtCusName = New System.Windows.Forms.TextBox()
        Me.btnCus = New System.Windows.Forms.Button()
        Me.Label102 = New System.Windows.Forms.Label()
        Me.Label116 = New System.Windows.Forms.Label()
        Me.txtCusId = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.BackColor = System.Drawing.Color.Lime
        Me.btnOK.Location = New System.Drawing.Point(502, 79)
        Me.btnOK.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(138, 40)
        Me.btnOK.TabIndex = 15
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Location = New System.Drawing.Point(247, 18)
        Me.chkDate.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(154, 23)
        Me.chkDate.TabIndex = 12
        Me.chkDate.Text = "使用日期查詢"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'dtpDate
        '
        Me.dtpDate.CustomFormat = "yyyy/MM"
        Me.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDate.Location = New System.Drawing.Point(115, 14)
        Me.dtpDate.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(120, 30)
        Me.dtpDate.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 20)
        Me.Label1.Margin = New System.Windows.Forms.Padding(7, 0, 7, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 19)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "日期起"
        '
        'txtCusCode
        '
        Me.txtCusCode.Location = New System.Drawing.Point(115, 51)
        Me.txtCusCode.Name = "txtCusCode"
        Me.txtCusCode.Size = New System.Drawing.Size(165, 30)
        Me.txtCusCode.TabIndex = 455
        Me.txtCusCode.Tag = "cus_code"
        '
        'txtCusName
        '
        Me.txtCusName.Location = New System.Drawing.Point(115, 89)
        Me.txtCusName.Name = "txtCusName"
        Me.txtCusName.ReadOnly = True
        Me.txtCusName.Size = New System.Drawing.Size(378, 30)
        Me.txtCusName.TabIndex = 453
        Me.txtCusName.Tag = "cus_name"
        '
        'btnCus
        '
        Me.btnCus.AutoSize = True
        Me.btnCus.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnCus.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnCus.Location = New System.Drawing.Point(286, 53)
        Me.btnCus.Name = "btnCus"
        Me.btnCus.Size = New System.Drawing.Size(82, 26)
        Me.btnCus.TabIndex = 454
        Me.btnCus.Text = "搜尋客戶"
        Me.btnCus.UseVisualStyleBackColor = False
        '
        'Label102
        '
        Me.Label102.AutoSize = True
        Me.Label102.Location = New System.Drawing.Point(16, 95)
        Me.Label102.Name = "Label102"
        Me.Label102.Size = New System.Drawing.Size(93, 19)
        Me.Label102.TabIndex = 452
        Me.Label102.Text = "客戶名稱"
        '
        'Label116
        '
        Me.Label116.AutoSize = True
        Me.Label116.Location = New System.Drawing.Point(16, 57)
        Me.Label116.Name = "Label116"
        Me.Label116.Size = New System.Drawing.Size(93, 19)
        Me.Label116.TabIndex = 451
        Me.Label116.Text = "客戶代號"
        '
        'txtCusId
        '
        Me.txtCusId.Location = New System.Drawing.Point(410, 11)
        Me.txtCusId.Name = "txtCusId"
        Me.txtCusId.ReadOnly = True
        Me.txtCusId.Size = New System.Drawing.Size(165, 30)
        Me.txtCusId.TabIndex = 456
        Me.txtCusId.Tag = "cus_code"
        Me.txtCusId.Visible = False
        '
        'Search_Inspection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(650, 128)
        Me.Controls.Add(Me.txtCusId)
        Me.Controls.Add(Me.txtCusCode)
        Me.Controls.Add(Me.txtCusName)
        Me.Controls.Add(Me.btnCus)
        Me.Controls.Add(Me.Label102)
        Me.Controls.Add(Me.Label116)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.chkDate)
        Me.Controls.Add(Me.dtpDate)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.MaximizeBox = False
        Me.Name = "Search_Inspection"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "檢驗費查詢"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnOK As Button
    Friend WithEvents chkDate As CheckBox
    Friend WithEvents dtpDate As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents txtCusCode As TextBox
    Friend WithEvents txtCusName As TextBox
    Friend WithEvents btnCus As Button
    Friend WithEvents Label102 As Label
    Friend WithEvents Label116 As Label
    Friend WithEvents txtCusId As TextBox
End Class
