<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInitializeMonthlyAccounts
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
        Me.btnInitialize = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnInitialize
        '
        Me.btnInitialize.Location = New System.Drawing.Point(143, 83)
        Me.btnInitialize.Name = "btnInitialize"
        Me.btnInitialize.Size = New System.Drawing.Size(119, 33)
        Me.btnInitialize.TabIndex = 0
        Me.btnInitialize.Text = "開始初始化"
        Me.btnInitialize.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(268, 83)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(119, 33)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "關閉"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(28, 134)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(89, 12)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "等待初始化操作"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(28, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(474, 47)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "此工具將從現有的訂單資料中，建立月度帳單資料。這將清空現有的月度帳單資料，並重新從訂單資料中建立。" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "初始化過程可能需要一些時間，請耐心等待。"
        '
        'frmInitializeMonthlyAccounts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(533, 165)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnInitialize)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInitializeMonthlyAccounts"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "初始化月度帳單資料"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnInitialize As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents Label1 As Label
End Class 