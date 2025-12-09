<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSearch_Payment
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
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker()
        Me.dtpStart = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbVendor = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbCredit = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Location = New System.Drawing.Point(560, 25)
        Me.chkDate.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(154, 23)
        Me.chkDate.TabIndex = 32
        Me.chkDate.Text = "使用日期查詢"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnOk.Location = New System.Drawing.Point(703, 110)
        Me.btnOk.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(138, 36)
        Me.btnOk.TabIndex = 31
        Me.btnOk.Text = "搜尋"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'dtpEnd
        '
        Me.dtpEnd.Location = New System.Drawing.Point(372, 21)
        Me.dtpEnd.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(180, 30)
        Me.dtpEnd.TabIndex = 26
        '
        'dtpStart
        '
        Me.dtpStart.Location = New System.Drawing.Point(104, 21)
        Me.dtpStart.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(180, 30)
        Me.dtpStart.TabIndex = 25
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(292, 27)
        Me.Label5.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 19)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "日期迄"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(24, 27)
        Me.Label4.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 19)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "日期起"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 71)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 19)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "廠商"
        '
        'cmbVendor
        '
        Me.cmbVendor.FormattingEnabled = True
        Me.cmbVendor.Location = New System.Drawing.Point(104, 67)
        Me.cmbVendor.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.cmbVendor.Name = "cmbVendor"
        Me.cmbVendor.Size = New System.Drawing.Size(448, 27)
        Me.cmbVendor.TabIndex = 35
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(560, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 19)
        Me.Label2.TabIndex = 36
        Me.Label2.Text = "貸方科目"
        '
        'cmbCredit
        '
        Me.cmbCredit.FormattingEnabled = True
        Me.cmbCredit.Items.AddRange(New Object() {"現金", "銀行存款", "應付票據"})
        Me.cmbCredit.Location = New System.Drawing.Point(661, 67)
        Me.cmbCredit.Name = "cmbCredit"
        Me.cmbCredit.Size = New System.Drawing.Size(180, 27)
        Me.cmbCredit.TabIndex = 37
        '
        'frmSearch_Payment
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(860, 166)
        Me.Controls.Add(Me.cmbCredit)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbVendor)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkDate)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.dtpEnd)
        Me.Controls.Add(Me.dtpStart)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.MaximizeBox = False
        Me.Name = "frmSearch_Payment"
        Me.Padding = New System.Windows.Forms.Padding(18, 16, 18, 16)
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "付款作業查詢"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkDate As CheckBox
    Friend WithEvents btnOk As Button
    Friend WithEvents dtpEnd As DateTimePicker
    Friend WithEvents dtpStart As DateTimePicker
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbVendor As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbCredit As ComboBox
End Class
