<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SurplusGasUserControl
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
        Me.dgvPurchaseBarrel = New System.Windows.Forms.DataGridView()
        Me.btnQuery_pb = New System.Windows.Forms.Button()
        Me.btnCancel_pb = New System.Windows.Forms.Button()
        CType(Me.dgvPurchaseBarrel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvPurchaseBarrel
        '
        Me.dgvPurchaseBarrel.AllowUserToAddRows = False
        Me.dgvPurchaseBarrel.AllowUserToDeleteRows = False
        Me.dgvPurchaseBarrel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPurchaseBarrel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvPurchaseBarrel.Location = New System.Drawing.Point(5, 74)
        Me.dgvPurchaseBarrel.Name = "dgvPurchaseBarrel"
        Me.dgvPurchaseBarrel.ReadOnly = True
        Me.dgvPurchaseBarrel.RowTemplate.Height = 24
        Me.dgvPurchaseBarrel.Size = New System.Drawing.Size(1872, 870)
        Me.dgvPurchaseBarrel.TabIndex = 440
        '
        'btnQuery_pb
        '
        Me.btnQuery_pb.BackColor = System.Drawing.Color.Lime
        Me.btnQuery_pb.Location = New System.Drawing.Point(164, 8)
        Me.btnQuery_pb.Name = "btnQuery_pb"
        Me.btnQuery_pb.Size = New System.Drawing.Size(140, 44)
        Me.btnQuery_pb.TabIndex = 439
        Me.btnQuery_pb.Text = "查  詢"
        Me.btnQuery_pb.UseVisualStyleBackColor = False
        '
        'btnCancel_pb
        '
        Me.btnCancel_pb.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCancel_pb.Location = New System.Drawing.Point(8, 8)
        Me.btnCancel_pb.Name = "btnCancel_pb"
        Me.btnCancel_pb.Size = New System.Drawing.Size(140, 44)
        Me.btnCancel_pb.TabIndex = 438
        Me.btnCancel_pb.Text = "取  消"
        Me.btnCancel_pb.UseVisualStyleBackColor = False
        '
        'SurplusGasUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dgvPurchaseBarrel)
        Me.Controls.Add(Me.btnQuery_pb)
        Me.Controls.Add(Me.btnCancel_pb)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "SurplusGasUserControl"
        Me.Padding = New System.Windows.Forms.Padding(5)
        Me.Size = New System.Drawing.Size(1882, 949)
        CType(Me.dgvPurchaseBarrel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvPurchaseBarrel As DataGridView
    Friend WithEvents btnQuery_pb As Button
    Friend WithEvents btnCancel_pb As Button
End Class
