<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Search_ChequePay
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
        Me.cmbCompany = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkStatus = New System.Windows.Forms.CheckBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpStart = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmbCompany
        '
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.Items.AddRange(New Object() {"已代收", "已兌現"})
        Me.cmbCompany.Location = New System.Drawing.Point(76, 96)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(337, 24)
        Me.cmbCompany.TabIndex = 21
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(14, 100)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 16)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "公司"
        '
        'chkStatus
        '
        Me.chkStatus.AutoSize = True
        Me.chkStatus.Location = New System.Drawing.Point(216, 58)
        Me.chkStatus.Name = "chkStatus"
        Me.chkStatus.Size = New System.Drawing.Size(122, 20)
        Me.chkStatus.TabIndex = 19
        Me.chkStatus.Text = "使用狀態查詢"
        Me.chkStatus.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.BackColor = System.Drawing.Color.Lime
        Me.btnOK.Location = New System.Drawing.Point(474, 95)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(67, 25)
        Me.btnOK.TabIndex = 18
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'cmbStatus
        '
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Items.AddRange(New Object() {"已代收", "已兌現"})
        Me.cmbStatus.Location = New System.Drawing.Point(76, 56)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(134, 24)
        Me.cmbStatus.TabIndex = 17
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 60)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 16)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "狀態"
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Location = New System.Drawing.Point(419, 16)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(122, 20)
        Me.chkDate.TabIndex = 15
        Me.chkDate.Text = "使用日期查詢"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'dtpEnd
        '
        Me.dtpEnd.Location = New System.Drawing.Point(279, 13)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(134, 27)
        Me.dtpEnd.TabIndex = 14
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(217, 18)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 16)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "日期迄"
        '
        'dtpStart
        '
        Me.dtpStart.Location = New System.Drawing.Point(76, 13)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(134, 27)
        Me.dtpStart.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 18)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 16)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "日期起"
        '
        'Search_ChequePay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(556, 137)
        Me.Controls.Add(Me.cmbCompany)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.chkStatus)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.cmbStatus)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.chkDate)
        Me.Controls.Add(Me.dtpEnd)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtpStart)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("標楷體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.Name = "Search_ChequePay"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.ShowIcon = False
        Me.Text = "應付支票管理查詢"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbCompany As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents chkStatus As CheckBox
    Friend WithEvents btnOK As Button
    Friend WithEvents cmbStatus As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents chkDate As CheckBox
    Friend WithEvents dtpEnd As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents dtpStart As DateTimePicker
    Friend WithEvents Label1 As Label
End Class
