<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucClosingEntry
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbDebit = New System.Windows.Forms.ComboBox()
        Me.cmbCredit = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtMemo_debit = New System.Windows.Forms.TextBox()
        Me.txtAmount_debit = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtAmount_credit = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtMemo_credit = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.dtpCE = New System.Windows.Forms.DateTimePicker()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.dgvClosingEntry = New System.Windows.Forms.DataGridView()
        Me.btnPrint = New System.Windows.Forms.Button()
        CType(Me.dgvClosingEntry, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(249, 14)
        Me.Label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "借方科目"
        '
        'cmbDebit
        '
        Me.cmbDebit.FormattingEnabled = True
        Me.cmbDebit.Location = New System.Drawing.Point(350, 10)
        Me.cmbDebit.Name = "cmbDebit"
        Me.cmbDebit.Size = New System.Drawing.Size(180, 27)
        Me.cmbDebit.TabIndex = 1
        '
        'cmbCredit
        '
        Me.cmbCredit.FormattingEnabled = True
        Me.cmbCredit.Location = New System.Drawing.Point(1133, 10)
        Me.cmbCredit.Name = "cmbCredit"
        Me.cmbCredit.Size = New System.Drawing.Size(180, 27)
        Me.cmbCredit.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(1032, 14)
        Me.Label2.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "貸方科目"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(538, 14)
        Me.Label3.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 19)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "備註"
        '
        'txtMemo_debit
        '
        Me.txtMemo_debit.Location = New System.Drawing.Point(597, 8)
        Me.txtMemo_debit.Name = "txtMemo_debit"
        Me.txtMemo_debit.Size = New System.Drawing.Size(180, 30)
        Me.txtMemo_debit.TabIndex = 5
        '
        'txtAmount_debit
        '
        Me.txtAmount_debit.Location = New System.Drawing.Point(844, 8)
        Me.txtAmount_debit.Name = "txtAmount_debit"
        Me.txtAmount_debit.Size = New System.Drawing.Size(180, 30)
        Me.txtAmount_debit.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(785, 14)
        Me.Label4.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 19)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "金額"
        '
        'txtAmount_credit
        '
        Me.txtAmount_credit.Location = New System.Drawing.Point(1627, 8)
        Me.txtAmount_credit.Name = "txtAmount_credit"
        Me.txtAmount_credit.Size = New System.Drawing.Size(180, 30)
        Me.txtAmount_credit.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(1568, 14)
        Me.Label5.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 19)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "金額"
        '
        'txtMemo_credit
        '
        Me.txtMemo_credit.Location = New System.Drawing.Point(1380, 8)
        Me.txtMemo_credit.Name = "txtMemo_credit"
        Me.txtMemo_credit.Size = New System.Drawing.Size(180, 30)
        Me.txtMemo_credit.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(1321, 14)
        Me.Label6.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(51, 19)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "備註"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(2, 14)
        Me.Label7.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(51, 19)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "日期"
        '
        'dtpCE
        '
        Me.dtpCE.Location = New System.Drawing.Point(61, 8)
        Me.dtpCE.Name = "dtpCE"
        Me.dtpCE.Size = New System.Drawing.Size(180, 30)
        Me.dtpCE.TabIndex = 13
        '
        'btnSearch
        '
        Me.btnSearch.BackColor = System.Drawing.Color.Lime
        Me.btnSearch.Location = New System.Drawing.Point(632, 44)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(140, 44)
        Me.btnSearch.TabIndex = 455
        Me.btnSearch.Text = "查  詢"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel.Location = New System.Drawing.Point(476, 44)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel.TabIndex = 454
        Me.btnCancel.Text = "取  消"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDelete.Location = New System.Drawing.Point(320, 44)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(140, 44)
        Me.btnDelete.TabIndex = 453
        Me.btnDelete.Text = "刪  除"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnEdit.Location = New System.Drawing.Point(164, 44)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(140, 44)
        Me.btnEdit.TabIndex = 452
        Me.btnEdit.Text = "修  改"
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnAdd.Location = New System.Drawing.Point(8, 44)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(140, 44)
        Me.btnAdd.TabIndex = 451
        Me.btnAdd.Tag = ""
        Me.btnAdd.Text = "新  增"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'dgvClosingEntry
        '
        Me.dgvClosingEntry.AllowUserToAddRows = False
        Me.dgvClosingEntry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvClosingEntry.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvClosingEntry.Location = New System.Drawing.Point(5, 114)
        Me.dgvClosingEntry.Name = "dgvClosingEntry"
        Me.dgvClosingEntry.ReadOnly = True
        Me.dgvClosingEntry.RowTemplate.Height = 24
        Me.dgvClosingEntry.Size = New System.Drawing.Size(1872, 830)
        Me.dgvClosingEntry.TabIndex = 456
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnPrint.Location = New System.Drawing.Point(788, 44)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(140, 44)
        Me.btnPrint.TabIndex = 480
        Me.btnPrint.Text = "列   印"
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'ucClosingEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.dgvClosingEntry)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.dtpCE)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtAmount_credit)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtMemo_credit)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtAmount_debit)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtMemo_debit)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbCredit)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbDebit)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(5)
        Me.Name = "ucClosingEntry"
        Me.Padding = New System.Windows.Forms.Padding(5)
        Me.Size = New System.Drawing.Size(1882, 949)
        CType(Me.dgvClosingEntry, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents cmbDebit As ComboBox
    Friend WithEvents cmbCredit As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtMemo_debit As TextBox
    Friend WithEvents txtAmount_debit As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtAmount_credit As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtMemo_credit As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents dtpCE As DateTimePicker
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents dgvClosingEntry As DataGridView
    Friend WithEvents btnPrint As Button
End Class
