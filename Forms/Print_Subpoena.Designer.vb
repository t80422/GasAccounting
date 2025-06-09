<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Print_Subpoena
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
        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
        Me.btnCash = New System.Windows.Forms.Button()
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 17)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "日期"
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(59, 12)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(150, 27)
        Me.dtpDate.TabIndex = 1
        '
        'btnCash
        '
        Me.btnCash.AutoSize = True
        Me.btnCash.BackColor = System.Drawing.Color.Lime
        Me.btnCash.Location = New System.Drawing.Point(16, 45)
        Me.btnCash.Name = "btnCash"
        Me.btnCash.Padding = New System.Windows.Forms.Padding(5)
        Me.btnCash.Size = New System.Drawing.Size(123, 36)
        Me.btnCash.TabIndex = 2
        Me.btnCash.Text = "列印現金傳票"
        Me.btnCash.UseVisualStyleBackColor = False
        '
        'btnTransfer
        '
        Me.btnTransfer.AutoSize = True
        Me.btnTransfer.BackColor = System.Drawing.Color.Lime
        Me.btnTransfer.Location = New System.Drawing.Point(145, 45)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Padding = New System.Windows.Forms.Padding(5)
        Me.btnTransfer.Size = New System.Drawing.Size(123, 36)
        Me.btnTransfer.TabIndex = 3
        Me.btnTransfer.Text = "列印轉帳傳票"
        Me.btnTransfer.UseVisualStyleBackColor = False
        '
        'Print_Subpoena
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(280, 91)
        Me.Controls.Add(Me.btnTransfer)
        Me.Controls.Add(Me.btnCash)
        Me.Controls.Add(Me.dtpDate)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Print_Subpoena"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "列印傳票"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents dtpDate As DateTimePicker
    Friend WithEvents btnCash As Button
    Friend WithEvents btnTransfer As Button
End Class
