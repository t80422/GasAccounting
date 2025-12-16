<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSearch_Collection
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbSubject = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbType = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtpStart = New System.Windows.Forms.DateTimePicker()
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtChequeNum = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCusCode = New System.Windows.Forms.TextBox()
        Me.txtCusName = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.txtCusId = New System.Windows.Forms.TextBox()
        Me.cmbBank = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.dtpAccountMonth = New System.Windows.Forms.DateTimePicker()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.chkAccountMonth = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(661, 20)
        Me.Label2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "科    目"
        '
        'cmbSubject
        '
        Me.cmbSubject.FormattingEnabled = True
        Me.cmbSubject.Location = New System.Drawing.Point(772, 16)
        Me.cmbSubject.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.cmbSubject.Name = "cmbSubject"
        Me.cmbSubject.Size = New System.Drawing.Size(198, 27)
        Me.cmbSubject.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(338, 112)
        Me.Label3.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(93, 19)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "收款類型"
        '
        'cmbType
        '
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Items.AddRange(New Object() {"現金", "銀行存款", "應收票據"})
        Me.cmbType.Location = New System.Drawing.Point(447, 108)
        Me.cmbType.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(198, 27)
        Me.cmbType.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 66)
        Me.Label4.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 19)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "日期起"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(338, 66)
        Me.Label5.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 19)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "日期迄"
        '
        'dtpStart
        '
        Me.dtpStart.Location = New System.Drawing.Point(124, 60)
        Me.dtpStart.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(198, 30)
        Me.dtpStart.TabIndex = 8
        '
        'dtpEnd
        '
        Me.dtpEnd.Location = New System.Drawing.Point(447, 60)
        Me.dtpEnd.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(198, 30)
        Me.dtpEnd.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(15, 112)
        Me.Label6.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(93, 19)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "支票號碼"
        '
        'txtChequeNum
        '
        Me.txtChequeNum.Location = New System.Drawing.Point(124, 106)
        Me.txtChequeNum.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtChequeNum.Name = "txtChequeNum"
        Me.txtChequeNum.Size = New System.Drawing.Size(198, 30)
        Me.txtChequeNum.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 20)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "客戶代號"
        '
        'txtCusCode
        '
        Me.txtCusCode.Location = New System.Drawing.Point(124, 14)
        Me.txtCusCode.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCusCode.Name = "txtCusCode"
        Me.txtCusCode.Size = New System.Drawing.Size(198, 30)
        Me.txtCusCode.TabIndex = 1
        '
        'txtCusName
        '
        Me.txtCusName.Location = New System.Drawing.Point(447, 14)
        Me.txtCusName.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCusName.Name = "txtCusName"
        Me.txtCusName.ReadOnly = True
        Me.txtCusName.Size = New System.Drawing.Size(198, 30)
        Me.txtCusName.TabIndex = 13
        Me.txtCusName.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(338, 20)
        Me.Label7.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(93, 19)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "客戶名稱"
        '
        'btnOk
        '
        Me.btnOk.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnOk.Location = New System.Drawing.Point(832, 151)
        Me.btnOk.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(138, 36)
        Me.btnOk.TabIndex = 14
        Me.btnOk.Text = "搜尋"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Location = New System.Drawing.Point(661, 64)
        Me.chkDate.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(154, 23)
        Me.chkDate.TabIndex = 15
        Me.chkDate.Text = "使用日期查詢"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'txtCusId
        '
        Me.txtCusId.Location = New System.Drawing.Point(827, 55)
        Me.txtCusId.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtCusId.Name = "txtCusId"
        Me.txtCusId.Size = New System.Drawing.Size(180, 30)
        Me.txtCusId.TabIndex = 16
        Me.txtCusId.Visible = False
        '
        'cmbBank
        '
        Me.cmbBank.FormattingEnabled = True
        Me.cmbBank.Items.AddRange(New Object() {"現金", "銀行存款", "應收票據"})
        Me.cmbBank.Location = New System.Drawing.Point(772, 108)
        Me.cmbBank.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.cmbBank.Name = "cmbBank"
        Me.cmbBank.Size = New System.Drawing.Size(198, 27)
        Me.cmbBank.TabIndex = 18
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(661, 112)
        Me.Label8.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(51, 19)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "銀行"
        '
        'dtpAccountMonth
        '
        Me.dtpAccountMonth.CustomFormat = "yyyy年MM月"
        Me.dtpAccountMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpAccountMonth.Location = New System.Drawing.Point(124, 152)
        Me.dtpAccountMonth.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.dtpAccountMonth.Name = "dtpAccountMonth"
        Me.dtpAccountMonth.Size = New System.Drawing.Size(198, 30)
        Me.dtpAccountMonth.TabIndex = 20
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(15, 158)
        Me.Label9.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(93, 19)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "帳款月份"
        '
        'chkAccountMonth
        '
        Me.chkAccountMonth.AutoSize = True
        Me.chkAccountMonth.Location = New System.Drawing.Point(338, 156)
        Me.chkAccountMonth.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.chkAccountMonth.Name = "chkAccountMonth"
        Me.chkAccountMonth.Size = New System.Drawing.Size(196, 23)
        Me.chkAccountMonth.TabIndex = 21
        Me.chkAccountMonth.Text = "使用帳款月份查詢"
        Me.chkAccountMonth.UseVisualStyleBackColor = True
        '
        'frmSearch_Collection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(988, 196)
        Me.Controls.Add(Me.chkAccountMonth)
        Me.Controls.Add(Me.dtpAccountMonth)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.cmbBank)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtCusId)
        Me.Controls.Add(Me.chkDate)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.txtCusName)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtChequeNum)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.dtpEnd)
        Me.Controls.Add(Me.dtpStart)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbType)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbSubject)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtCusCode)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.MaximizeBox = False
        Me.Name = "frmSearch_Collection"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "收入管理查詢"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label2 As Label
    Friend WithEvents cmbSubject As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbType As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents dtpStart As DateTimePicker
    Friend WithEvents dtpEnd As DateTimePicker
    Friend WithEvents Label6 As Label
    Friend WithEvents txtChequeNum As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtCusCode As TextBox
    Friend WithEvents txtCusName As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents btnOk As Button
    Friend WithEvents chkDate As CheckBox
    Friend WithEvents txtCusId As TextBox
    Friend WithEvents cmbBank As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents dtpAccountMonth As DateTimePicker
    Friend WithEvents Label9 As Label
    Friend WithEvents chkAccountMonth As CheckBox
End Class
