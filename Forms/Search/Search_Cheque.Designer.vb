<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Search_Cheque
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
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.chkStatus = New System.Windows.Forms.CheckBox()
        Me.cmbBank = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dtpColDateEnd = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtpColDateStart = New System.Windows.Forms.DateTimePicker()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chkColDate = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 21)
        Me.Label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "日期起"
        '
        'dtpStart
        '
        Me.dtpStart.Location = New System.Drawing.Point(95, 15)
        Me.dtpStart.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(166, 30)
        Me.dtpStart.TabIndex = 1
        '
        'dtpEnd
        '
        Me.dtpEnd.Location = New System.Drawing.Point(349, 15)
        Me.dtpEnd.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(166, 30)
        Me.dtpEnd.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(271, 21)
        Me.Label2.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "日期迄"
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Location = New System.Drawing.Point(520, 19)
        Me.chkDate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(148, 23)
        Me.chkDate.TabIndex = 4
        Me.chkDate.Text = "使用日期查詢"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 122)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 19)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "狀態"
        '
        'cmbStatus
        '
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Items.AddRange(New Object() {"已代收", "已兌現"})
        Me.cmbStatus.Location = New System.Drawing.Point(95, 118)
        Me.cmbStatus.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(166, 27)
        Me.cmbStatus.TabIndex = 6
        '
        'btnOK
        '
        Me.btnOK.BackColor = System.Drawing.Color.Lime
        Me.btnOK.Location = New System.Drawing.Point(523, 162)
        Me.btnOK.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(84, 30)
        Me.btnOK.TabIndex = 7
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'chkStatus
        '
        Me.chkStatus.AutoSize = True
        Me.chkStatus.Location = New System.Drawing.Point(270, 120)
        Me.chkStatus.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkStatus.Name = "chkStatus"
        Me.chkStatus.Size = New System.Drawing.Size(148, 23)
        Me.chkStatus.TabIndex = 8
        Me.chkStatus.Text = "使用狀態查詢"
        Me.chkStatus.UseVisualStyleBackColor = True
        '
        'cmbBank
        '
        Me.cmbBank.FormattingEnabled = True
        Me.cmbBank.Items.AddRange(New Object() {"已代收", "已兌現"})
        Me.cmbBank.Location = New System.Drawing.Point(95, 165)
        Me.cmbBank.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmbBank.Name = "cmbBank"
        Me.cmbBank.Size = New System.Drawing.Size(420, 27)
        Me.cmbBank.TabIndex = 10
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(18, 170)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 19)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "銀行"
        '
        'dtpColDateEnd
        '
        Me.dtpColDateEnd.Location = New System.Drawing.Point(349, 66)
        Me.dtpColDateEnd.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dtpColDateEnd.Name = "dtpColDateEnd"
        Me.dtpColDateEnd.Size = New System.Drawing.Size(166, 30)
        Me.dtpColDateEnd.TabIndex = 14
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(271, 72)
        Me.Label5.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(69, 19)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "代收迄"
        '
        'dtpColDateStart
        '
        Me.dtpColDateStart.Location = New System.Drawing.Point(95, 66)
        Me.dtpColDateStart.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dtpColDateStart.Name = "dtpColDateStart"
        Me.dtpColDateStart.Size = New System.Drawing.Size(166, 30)
        Me.dtpColDateStart.TabIndex = 12
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(18, 72)
        Me.Label6.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(69, 19)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "代收起"
        '
        'chkColDate
        '
        Me.chkColDate.AutoSize = True
        Me.chkColDate.Location = New System.Drawing.Point(523, 70)
        Me.chkColDate.Margin = New System.Windows.Forms.Padding(4)
        Me.chkColDate.Name = "chkColDate"
        Me.chkColDate.Size = New System.Drawing.Size(148, 23)
        Me.chkColDate.TabIndex = 15
        Me.chkColDate.Text = "使用代收查詢"
        Me.chkColDate.UseVisualStyleBackColor = True
        '
        'Search_Cheque
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(684, 205)
        Me.Controls.Add(Me.chkColDate)
        Me.Controls.Add(Me.dtpColDateEnd)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.dtpColDateStart)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cmbBank)
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
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.MaximizeBox = False
        Me.Name = "Search_Cheque"
        Me.Padding = New System.Windows.Forms.Padding(12, 12, 12, 12)
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "支票管理查詢"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents dtpStart As DateTimePicker
    Friend WithEvents dtpEnd As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents chkDate As CheckBox
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbStatus As ComboBox
    Friend WithEvents btnOK As Button
    Friend WithEvents chkStatus As CheckBox
    Friend WithEvents cmbBank As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents dtpColDateEnd As DateTimePicker
    Friend WithEvents Label5 As Label
    Friend WithEvents dtpColDateStart As DateTimePicker
    Friend WithEvents Label6 As Label
    Friend WithEvents chkColDate As CheckBox
End Class
