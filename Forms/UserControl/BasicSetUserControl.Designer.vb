<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BasicSetUserControl
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
        Me.btnSave = New System.Windows.Forms.Button()
        Me.txtInitGas = New System.Windows.Forms.TextBox()
        Me.Label296 = New System.Windows.Forms.Label()
        Me.txtInitCash = New System.Windows.Forms.TextBox()
        Me.Label297 = New System.Windows.Forms.Label()
        Me.txtId = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnSave.Location = New System.Drawing.Point(13, 51)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(140, 44)
        Me.btnSave.TabIndex = 380
        Me.btnSave.Text = "儲  存"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'txtInitGas
        '
        Me.txtInitGas.Location = New System.Drawing.Point(420, 13)
        Me.txtInitGas.Name = "txtInitGas"
        Me.txtInitGas.Size = New System.Drawing.Size(150, 30)
        Me.txtInitGas.TabIndex = 378
        Me.txtInitGas.Tag = ""
        '
        'Label296
        '
        Me.Label296.AutoSize = True
        Me.Label296.Location = New System.Drawing.Point(279, 19)
        Me.Label296.Name = "Label296"
        Me.Label296.Size = New System.Drawing.Size(135, 19)
        Me.Label296.TabIndex = 377
        Me.Label296.Text = "初始瓦斯存量"
        '
        'txtInitCash
        '
        Me.txtInitCash.Location = New System.Drawing.Point(123, 13)
        Me.txtInitCash.Name = "txtInitCash"
        Me.txtInitCash.Size = New System.Drawing.Size(150, 30)
        Me.txtInitCash.TabIndex = 376
        Me.txtInitCash.Tag = ""
        '
        'Label297
        '
        Me.Label297.AutoSize = True
        Me.Label297.Location = New System.Drawing.Point(20, 19)
        Me.Label297.Margin = New System.Windows.Forms.Padding(10)
        Me.Label297.Name = "Label297"
        Me.Label297.Size = New System.Drawing.Size(93, 19)
        Me.Label297.TabIndex = 375
        Me.Label297.Text = "初始現金"
        '
        'txtId
        '
        Me.txtId.Location = New System.Drawing.Point(159, 61)
        Me.txtId.Name = "txtId"
        Me.txtId.ReadOnly = True
        Me.txtId.Size = New System.Drawing.Size(150, 30)
        Me.txtId.TabIndex = 381
        Me.txtId.Tag = ""
        Me.txtId.Visible = False
        '
        'BasicSetUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtId)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtInitGas)
        Me.Controls.Add(Me.Label296)
        Me.Controls.Add(Me.txtInitCash)
        Me.Controls.Add(Me.Label297)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "BasicSetUserControl"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.Size = New System.Drawing.Size(1882, 949)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSave As Button
    Friend WithEvents txtInitGas As TextBox
    Friend WithEvents Label296 As Label
    Friend WithEvents txtInitCash As TextBox
    Friend WithEvents Label297 As Label
    Friend WithEvents txtId As TextBox
End Class
