<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWriteOff
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
        Me.txtDate = New System.Windows.Forms.TextBox()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtType = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtUnmatched = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtCusName = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dgvMonth = New System.Windows.Forms.DataGridView()
        Me.btnConfirm = New System.Windows.Forms.Button()
        Me.dgvDetail = New System.Windows.Forms.DataGridView()
        Me.btnInit = New System.Windows.Forms.Button()
        Me.btnAuto = New System.Windows.Forms.Button()
        CType(Me.dgvMonth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "日    期"
        '
        'txtDate
        '
        Me.txtDate.Location = New System.Drawing.Point(59, 12)
        Me.txtDate.Name = "txtDate"
        Me.txtDate.ReadOnly = True
        Me.txtDate.Size = New System.Drawing.Size(100, 22)
        Me.txtDate.TabIndex = 1
        '
        'txtAmount
        '
        Me.txtAmount.Location = New System.Drawing.Point(59, 40)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.ReadOnly = True
        Me.txtAmount.Size = New System.Drawing.Size(100, 22)
        Me.txtAmount.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 12)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "金    額"
        '
        'txtType
        '
        Me.txtType.Location = New System.Drawing.Point(401, 12)
        Me.txtType.Name = "txtType"
        Me.txtType.ReadOnly = True
        Me.txtType.Size = New System.Drawing.Size(100, 22)
        Me.txtType.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(342, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 12)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "收款類型"
        '
        'txtUnmatched
        '
        Me.txtUnmatched.Location = New System.Drawing.Point(236, 40)
        Me.txtUnmatched.Name = "txtUnmatched"
        Me.txtUnmatched.ReadOnly = True
        Me.txtUnmatched.Size = New System.Drawing.Size(100, 22)
        Me.txtUnmatched.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(165, 43)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 12)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "未銷帳金額"
        '
        'txtCusName
        '
        Me.txtCusName.Location = New System.Drawing.Point(236, 12)
        Me.txtCusName.Name = "txtCusName"
        Me.txtCusName.ReadOnly = True
        Me.txtCusName.Size = New System.Drawing.Size(100, 22)
        Me.txtCusName.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(165, 17)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 12)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "客戶名稱"
        '
        'dgvMonth
        '
        Me.dgvMonth.AllowUserToAddRows = False
        Me.dgvMonth.AllowUserToDeleteRows = False
        Me.dgvMonth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMonth.Location = New System.Drawing.Point(12, 69)
        Me.dgvMonth.Name = "dgvMonth"
        Me.dgvMonth.RowTemplate.Height = 24
        Me.dgvMonth.Size = New System.Drawing.Size(445, 382)
        Me.dgvMonth.TabIndex = 10
        '
        'btnConfirm
        '
        Me.btnConfirm.Location = New System.Drawing.Point(423, 40)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(75, 23)
        Me.btnConfirm.TabIndex = 11
        Me.btnConfirm.Text = "確認"
        Me.btnConfirm.UseVisualStyleBackColor = True
        '
        'dgvDetail
        '
        Me.dgvDetail.AllowUserToAddRows = False
        Me.dgvDetail.AllowUserToDeleteRows = False
        Me.dgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetail.Location = New System.Drawing.Point(463, 69)
        Me.dgvDetail.Name = "dgvDetail"
        Me.dgvDetail.RowTemplate.Height = 24
        Me.dgvDetail.Size = New System.Drawing.Size(245, 382)
        Me.dgvDetail.TabIndex = 12
        '
        'btnInit
        '
        Me.btnInit.Location = New System.Drawing.Point(507, 12)
        Me.btnInit.Name = "btnInit"
        Me.btnInit.Size = New System.Drawing.Size(75, 23)
        Me.btnInit.TabIndex = 13
        Me.btnInit.Text = "初始化(暫)"
        Me.btnInit.UseVisualStyleBackColor = True
        '
        'btnAuto
        '
        Me.btnAuto.Location = New System.Drawing.Point(342, 40)
        Me.btnAuto.Name = "btnAuto"
        Me.btnAuto.Size = New System.Drawing.Size(75, 23)
        Me.btnAuto.TabIndex = 14
        Me.btnAuto.Text = "自動分配"
        Me.btnAuto.UseVisualStyleBackColor = True
        '
        'frmWriteOff
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(721, 463)
        Me.Controls.Add(Me.btnAuto)
        Me.Controls.Add(Me.btnInit)
        Me.Controls.Add(Me.dgvDetail)
        Me.Controls.Add(Me.btnConfirm)
        Me.Controls.Add(Me.dgvMonth)
        Me.Controls.Add(Me.txtCusName)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtUnmatched)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtType)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtDate)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWriteOff"
        Me.Text = "銷帳"
        Me.TopMost = True
        CType(Me.dgvMonth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents txtDate As TextBox
    Friend WithEvents txtAmount As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtType As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtUnmatched As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtCusName As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents dgvMonth As DataGridView
    Friend WithEvents btnConfirm As Button
    Friend WithEvents dgvDetail As DataGridView
    Friend WithEvents btnInit As Button
    Friend WithEvents btnAuto As Button
End Class
