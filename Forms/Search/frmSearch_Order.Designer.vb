<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSearch_Order
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpStart = New System.Windows.Forms.DateTimePicker()
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkIsDate = New System.Windows.Forms.CheckBox()
        Me.txtCusCode = New System.Windows.Forms.TextBox()
        Me.btnSearchCus = New System.Windows.Forms.Button()
        Me.lblCusCode = New System.Windows.Forms.Label()
        Me.txtCusName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCusID = New System.Windows.Forms.TextBox()
        Me.chkIn = New System.Windows.Forms.CheckBox()
        Me.chkOut = New System.Windows.Forms.CheckBox()
        Me.btnSubmit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "日期起"
        '
        'dtpStart
        '
        Me.dtpStart.Location = New System.Drawing.Point(112, 13)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(165, 30)
        Me.dtpStart.TabIndex = 1
        '
        'dtpEnd
        '
        Me.dtpEnd.Location = New System.Drawing.Point(361, 13)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(165, 30)
        Me.dtpEnd.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(283, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "日期迄"
        '
        'chkIsDate
        '
        Me.chkIsDate.AutoSize = True
        Me.chkIsDate.Location = New System.Drawing.Point(532, 17)
        Me.chkIsDate.Name = "chkIsDate"
        Me.chkIsDate.Size = New System.Drawing.Size(154, 23)
        Me.chkIsDate.TabIndex = 4
        Me.chkIsDate.Text = "使用日期查詢"
        Me.chkIsDate.UseVisualStyleBackColor = True
        '
        'txtCusCode
        '
        Me.txtCusCode.Location = New System.Drawing.Point(112, 59)
        Me.txtCusCode.Name = "txtCusCode"
        Me.txtCusCode.Size = New System.Drawing.Size(77, 30)
        Me.txtCusCode.TabIndex = 515
        Me.txtCusCode.Tag = "cus_code"
        '
        'btnSearchCus
        '
        Me.btnSearchCus.AutoSize = True
        Me.btnSearchCus.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnSearchCus.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnSearchCus.Location = New System.Drawing.Point(195, 61)
        Me.btnSearchCus.Name = "btnSearchCus"
        Me.btnSearchCus.Size = New System.Drawing.Size(82, 26)
        Me.btnSearchCus.TabIndex = 514
        Me.btnSearchCus.Text = "搜尋客戶"
        Me.btnSearchCus.UseVisualStyleBackColor = False
        '
        'lblCusCode
        '
        Me.lblCusCode.AutoSize = True
        Me.lblCusCode.Location = New System.Drawing.Point(13, 65)
        Me.lblCusCode.Name = "lblCusCode"
        Me.lblCusCode.Size = New System.Drawing.Size(93, 19)
        Me.lblCusCode.TabIndex = 513
        Me.lblCusCode.Text = "客戶代號"
        '
        'txtCusName
        '
        Me.txtCusName.Location = New System.Drawing.Point(112, 105)
        Me.txtCusName.Name = "txtCusName"
        Me.txtCusName.ReadOnly = True
        Me.txtCusName.Size = New System.Drawing.Size(574, 30)
        Me.txtCusName.TabIndex = 517
        Me.txtCusName.Tag = "cus_name"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 111)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(93, 19)
        Me.Label3.TabIndex = 516
        Me.Label3.Text = "客戶名稱"
        '
        'txtCusID
        '
        Me.txtCusID.Location = New System.Drawing.Point(292, 59)
        Me.txtCusID.Name = "txtCusID"
        Me.txtCusID.ReadOnly = True
        Me.txtCusID.Size = New System.Drawing.Size(100, 30)
        Me.txtCusID.TabIndex = 518
        Me.txtCusID.Tag = "o_cus_Id"
        Me.txtCusID.Visible = False
        '
        'chkIn
        '
        Me.chkIn.AutoSize = True
        Me.chkIn.Checked = True
        Me.chkIn.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIn.Location = New System.Drawing.Point(17, 151)
        Me.chkIn.Name = "chkIn"
        Me.chkIn.Size = New System.Drawing.Size(91, 23)
        Me.chkIn.TabIndex = 519
        Me.chkIn.Text = "進場單"
        Me.chkIn.UseVisualStyleBackColor = True
        '
        'chkOut
        '
        Me.chkOut.AutoSize = True
        Me.chkOut.Checked = True
        Me.chkOut.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOut.Location = New System.Drawing.Point(112, 151)
        Me.chkOut.Name = "chkOut"
        Me.chkOut.Size = New System.Drawing.Size(91, 23)
        Me.chkOut.TabIndex = 520
        Me.chkOut.Text = "出場單"
        Me.chkOut.UseVisualStyleBackColor = True
        '
        'btnSubmit
        '
        Me.btnSubmit.AutoSize = True
        Me.btnSubmit.BackColor = System.Drawing.Color.Lime
        Me.btnSubmit.Location = New System.Drawing.Point(604, 145)
        Me.btnSubmit.Name = "btnSubmit"
        Me.btnSubmit.Size = New System.Drawing.Size(75, 35)
        Me.btnSubmit.TabIndex = 521
        Me.btnSubmit.Text = "確認"
        Me.btnSubmit.UseVisualStyleBackColor = False
        '
        'frmSearch_Order
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(692, 189)
        Me.Controls.Add(Me.btnSubmit)
        Me.Controls.Add(Me.chkOut)
        Me.Controls.Add(Me.chkIn)
        Me.Controls.Add(Me.txtCusID)
        Me.Controls.Add(Me.txtCusName)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtCusCode)
        Me.Controls.Add(Me.btnSearchCus)
        Me.Controls.Add(Me.lblCusCode)
        Me.Controls.Add(Me.chkIsDate)
        Me.Controls.Add(Me.dtpEnd)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtpStart)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSearch_Order"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "查詢"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents dtpStart As DateTimePicker
    Friend WithEvents dtpEnd As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents chkIsDate As CheckBox
    Friend WithEvents txtCusCode As TextBox
    Friend WithEvents btnSearchCus As Button
    Friend WithEvents lblCusCode As Label
    Friend WithEvents txtCusName As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtCusID As TextBox
    Friend WithEvents chkIn As CheckBox
    Friend WithEvents chkOut As CheckBox
    Friend WithEvents btnSubmit As Button
End Class
