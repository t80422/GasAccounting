<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDatePicker
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
        Me.MonthCalendar1 = New System.Windows.Forms.MonthCalendar()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'MonthCalendar1
        '
        Me.MonthCalendar1.Dock = System.Windows.Forms.DockStyle.Top
        Me.MonthCalendar1.Location = New System.Drawing.Point(0, 0)
        Me.MonthCalendar1.MaxSelectionCount = 1
        Me.MonthCalendar1.Name = "MonthCalendar1"
        Me.MonthCalendar1.TabIndex = 0
        '
        'btnOk
        '
        Me.btnOk.AutoSize = True
        Me.btnOk.BackColor = System.Drawing.Color.Lime
        Me.btnOk.Font = New System.Drawing.Font("新細明體", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnOk.Location = New System.Drawing.Point(87, 174)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(50, 30)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Text = "確認"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'frmDatePicker
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(220, 215)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.MonthCalendar1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDatePicker"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "請選擇日期"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MonthCalendar1 As MonthCalendar
    Friend WithEvents btnOk As Button
End Class
