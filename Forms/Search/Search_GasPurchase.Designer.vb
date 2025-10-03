<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Search_GasPurchase
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
        Me.cmbCompany = New System.Windows.Forms.ComboBox()
        Me.cmbProduct = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbGasVendor = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbTransportation = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnSubmit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "日期起"
        '
        'dtpStart
        '
        Me.dtpStart.Location = New System.Drawing.Point(112, 13)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(190, 30)
        Me.dtpStart.TabIndex = 1
        '
        'dtpEnd
        '
        Me.dtpEnd.Location = New System.Drawing.Point(386, 13)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(190, 30)
        Me.dtpEnd.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(308, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "日期迄"
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Location = New System.Drawing.Point(582, 17)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(154, 23)
        Me.chkDate.TabIndex = 4
        Me.chkDate.Text = "使用日期搜尋"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 63)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 19)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "公司"
        '
        'cmbCompany
        '
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.Location = New System.Drawing.Point(112, 59)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(190, 27)
        Me.cmbCompany.TabIndex = 6
        '
        'cmbProduct
        '
        Me.cmbProduct.FormattingEnabled = True
        Me.cmbProduct.Items.AddRange(New Object() {"普氣", "丙氣"})
        Me.cmbProduct.Location = New System.Drawing.Point(386, 59)
        Me.cmbProduct.Name = "cmbProduct"
        Me.cmbProduct.Size = New System.Drawing.Size(190, 27)
        Me.cmbProduct.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(308, 63)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 19)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "產品"
        '
        'cmbGasVendor
        '
        Me.cmbGasVendor.FormattingEnabled = True
        Me.cmbGasVendor.Location = New System.Drawing.Point(112, 102)
        Me.cmbGasVendor.Name = "cmbGasVendor"
        Me.cmbGasVendor.Size = New System.Drawing.Size(464, 27)
        Me.cmbGasVendor.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(13, 106)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(93, 19)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "大氣廠商"
        '
        'cmbTransportation
        '
        Me.cmbTransportation.FormattingEnabled = True
        Me.cmbTransportation.Location = New System.Drawing.Point(112, 145)
        Me.cmbTransportation.Name = "cmbTransportation"
        Me.cmbTransportation.Size = New System.Drawing.Size(464, 27)
        Me.cmbTransportation.TabIndex = 12
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(13, 149)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(93, 19)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "運輸公司"
        '
        'btnSubmit
        '
        Me.btnSubmit.AutoSize = True
        Me.btnSubmit.BackColor = System.Drawing.Color.Lime
        Me.btnSubmit.Location = New System.Drawing.Point(651, 144)
        Me.btnSubmit.Name = "btnSubmit"
        Me.btnSubmit.Size = New System.Drawing.Size(75, 29)
        Me.btnSubmit.TabIndex = 13
        Me.btnSubmit.Text = "搜尋"
        Me.btnSubmit.UseVisualStyleBackColor = False
        '
        'Search_GasPurchase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(739, 190)
        Me.Controls.Add(Me.btnSubmit)
        Me.Controls.Add(Me.cmbTransportation)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cmbGasVendor)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbProduct)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbCompany)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.chkDate)
        Me.Controls.Add(Me.dtpEnd)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtpStart)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("標楷體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Search_GasPurchase"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "大氣進貨搜尋"
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
    Friend WithEvents cmbCompany As ComboBox
    Friend WithEvents cmbProduct As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents cmbGasVendor As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbTransportation As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents btnSubmit As Button
End Class
