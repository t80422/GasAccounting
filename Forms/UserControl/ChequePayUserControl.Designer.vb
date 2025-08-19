<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ChequePayUserControl
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
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.txtCheId = New System.Windows.Forms.TextBox()
        Me.Label123 = New System.Windows.Forms.Label()
        Me.dtpCashingDate = New System.Windows.Forms.DateTimePicker()
        Me.lblCashingDate = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.Label148 = New System.Windows.Forms.Label()
        Me.txtNumber = New System.Windows.Forms.TextBox()
        Me.Label147 = New System.Windows.Forms.Label()
        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
        Me.Label146 = New System.Windows.Forms.Label()
        Me.dgvCheque = New System.Windows.Forms.DataGridView()
        Me.chkIsCashing = New System.Windows.Forms.CheckBox()
        CType(Me.dgvCheque, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnSearch
        '
        Me.btnSearch.BackColor = System.Drawing.Color.Lime
        Me.btnSearch.Location = New System.Drawing.Point(159, 39)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(140, 44)
        Me.btnSearch.TabIndex = 492
        Me.btnSearch.Text = "查  詢"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'txtCheId
        '
        Me.txtCheId.Location = New System.Drawing.Point(106, 3)
        Me.txtCheId.Name = "txtCheId"
        Me.txtCheId.ReadOnly = True
        Me.txtCheId.Size = New System.Drawing.Size(166, 30)
        Me.txtCheId.TabIndex = 487
        Me.txtCheId.Tag = "cp_Id"
        '
        'Label123
        '
        Me.Label123.AutoSize = True
        Me.Label123.Location = New System.Drawing.Point(3, 9)
        Me.Label123.Name = "Label123"
        Me.Label123.Size = New System.Drawing.Size(95, 19)
        Me.Label123.TabIndex = 486
        Me.Label123.Text = "編    號"
        '
        'dtpCashingDate
        '
        Me.dtpCashingDate.Location = New System.Drawing.Point(1208, 3)
        Me.dtpCashingDate.Name = "dtpCashingDate"
        Me.dtpCashingDate.Size = New System.Drawing.Size(166, 30)
        Me.dtpCashingDate.TabIndex = 474
        Me.dtpCashingDate.Tag = "cp_CashingDate"
        Me.dtpCashingDate.Visible = False
        '
        'lblCashingDate
        '
        Me.lblCashingDate.AutoSize = True
        Me.lblCashingDate.Location = New System.Drawing.Point(1107, 9)
        Me.lblCashingDate.Name = "lblCashingDate"
        Me.lblCashingDate.Size = New System.Drawing.Size(93, 19)
        Me.lblCashingDate.TabIndex = 473
        Me.lblCashingDate.Text = "兌現日期"
        Me.lblCashingDate.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel.Location = New System.Drawing.Point(3, 39)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel.TabIndex = 471
        Me.btnCancel.Text = "取  消"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'txtAmount
        '
        Me.txtAmount.Location = New System.Drawing.Point(933, 3)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.ReadOnly = True
        Me.txtAmount.Size = New System.Drawing.Size(166, 30)
        Me.txtAmount.TabIndex = 470
        Me.txtAmount.Tag = "che_Amount"
        '
        'Label148
        '
        Me.Label148.AutoSize = True
        Me.Label148.Location = New System.Drawing.Point(830, 9)
        Me.Label148.Name = "Label148"
        Me.Label148.Size = New System.Drawing.Size(95, 19)
        Me.Label148.TabIndex = 469
        Me.Label148.Text = "金    額"
        '
        'txtNumber
        '
        Me.txtNumber.Location = New System.Drawing.Point(656, 3)
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.ReadOnly = True
        Me.txtNumber.Size = New System.Drawing.Size(166, 30)
        Me.txtNumber.TabIndex = 468
        Me.txtNumber.Tag = "cp_Number"
        '
        'Label147
        '
        Me.Label147.AutoSize = True
        Me.Label147.Location = New System.Drawing.Point(555, 9)
        Me.Label147.Name = "Label147"
        Me.Label147.Size = New System.Drawing.Size(93, 19)
        Me.Label147.TabIndex = 467
        Me.Label147.Text = "支票號碼"
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(381, 3)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(166, 30)
        Me.dtpDate.TabIndex = 466
        Me.dtpDate.Tag = "cp_Date"
        '
        'Label146
        '
        Me.Label146.AutoSize = True
        Me.Label146.Location = New System.Drawing.Point(280, 9)
        Me.Label146.Name = "Label146"
        Me.Label146.Size = New System.Drawing.Size(93, 19)
        Me.Label146.TabIndex = 465
        Me.Label146.Text = "給票日期"
        '
        'dgvCheque
        '
        Me.dgvCheque.AllowUserToAddRows = False
        Me.dgvCheque.AllowUserToDeleteRows = False
        Me.dgvCheque.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCheque.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvCheque.Location = New System.Drawing.Point(0, 109)
        Me.dgvCheque.Name = "dgvCheque"
        Me.dgvCheque.RowTemplate.Height = 24
        Me.dgvCheque.Size = New System.Drawing.Size(1882, 840)
        Me.dgvCheque.TabIndex = 472
        '
        'chkIsCashing
        '
        Me.chkIsCashing.AutoSize = True
        Me.chkIsCashing.Location = New System.Drawing.Point(1382, 7)
        Me.chkIsCashing.Name = "chkIsCashing"
        Me.chkIsCashing.Size = New System.Drawing.Size(91, 23)
        Me.chkIsCashing.TabIndex = 494
        Me.chkIsCashing.Text = "已兌現"
        Me.chkIsCashing.UseVisualStyleBackColor = True
        '
        'ChequePayUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.chkIsCashing)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtCheId)
        Me.Controls.Add(Me.Label123)
        Me.Controls.Add(Me.dtpCashingDate)
        Me.Controls.Add(Me.lblCashingDate)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.Label148)
        Me.Controls.Add(Me.txtNumber)
        Me.Controls.Add(Me.Label147)
        Me.Controls.Add(Me.dtpDate)
        Me.Controls.Add(Me.Label146)
        Me.Controls.Add(Me.dgvCheque)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "ChequePayUserControl"
        Me.Size = New System.Drawing.Size(1882, 949)
        CType(Me.dgvCheque, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSearch As Button
    Friend WithEvents txtCheId As TextBox
    Friend WithEvents Label123 As Label
    Friend WithEvents dtpCashingDate As DateTimePicker
    Friend WithEvents lblCashingDate As Label
    Friend WithEvents btnCancel As Button
    Friend WithEvents txtAmount As TextBox
    Friend WithEvents Label148 As Label
    Friend WithEvents txtNumber As TextBox
    Friend WithEvents Label147 As Label
    Friend WithEvents dtpDate As DateTimePicker
    Friend WithEvents Label146 As Label
    Friend WithEvents dgvCheque As DataGridView
    Friend WithEvents chkIsCashing As CheckBox
End Class
