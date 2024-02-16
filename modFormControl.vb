Imports System.IO

Module modFormControl
    ''' <summary>
    ''' 清空指定控制項內其他控制項
    ''' </summary>
    ''' <param name="container">控制項的集合</param>
    Public Sub ClearControls(container As Control, Optional exception As List(Of String) = Nothing)
        For Each ctrl As Control In container.Controls
            ' 檢查是否為例外列表中的控件
            If exception IsNot Nothing AndAlso (exception.Contains(ctrl.Name) OrElse exception.Contains(ctrl.Text)) Then
                Continue For
            End If

            ' 遞迴調用對於容器類型控件
            If TypeOf ctrl Is GroupBox OrElse TypeOf ctrl Is FlowLayoutPanel OrElse TypeOf ctrl Is Panel Then
                ClearControls(ctrl, exception)
            ElseIf TypeOf ctrl Is TabControl Then
                Dim tabControl As TabControl = CType(ctrl, TabControl)
                For Each tabPage As TabPage In tabControl.TabPages
                    ClearControls(tabPage, exception) ' 正確地對 TabPages 進行遞迴
                Next
            End If

            ' 清除特定類型控件的內容
            If TypeOf ctrl Is TextBox Then
                Dim txt As TextBox = ctrl
                txt.Text = String.Empty

                If txt.ReadOnly Then
                    txt.BackColor = SystemColors.Control
                End If

            ElseIf TypeOf ctrl Is CheckBox Then
                CType(ctrl, CheckBox).Checked = False
            ElseIf TypeOf ctrl Is RadioButton Then
                CType(ctrl, RadioButton).Checked = False
            ElseIf TypeOf ctrl Is ComboBox Then
                CType(ctrl, ComboBox).SelectedIndex = -1
            End If
        Next
    End Sub

    ''' <summary>
    ''' 將取得的資料傳至各控制項(控制項的Tag必須寫上表格欄位名稱)
    ''' </summary>
    ''' <param name="ctrls">父容器</param>
    ''' <param name="row"></param>
    Public Sub GetDataToControls(ctrls As Control, row As Object)
        For Each ctrl In ctrls.Controls.Cast(Of Control).Where(Function(c) Not String.IsNullOrEmpty(c.Tag))
            Dim value = GetCellData(row, ctrl.Tag.ToString)
            Select Case ctrl.GetType.Name
                Case "TextBox"
                    ctrl.Text = value
                Case "DateTimePicker"
                    Dim dtp As DateTimePicker = ctrl
                    dtp.Value = value
                Case "ComboBox"
                    Dim cmb As ComboBox = ctrl
                    cmb.SelectedIndex = cmb.FindStringExact(value)
                Case "GroupBox"
                    Dim grp As GroupBox = ctrl
                    For Each c In grp.Controls
                        If TypeOf c Is CheckBox Then
                            Dim chk As CheckBox = c
                            Dim b As Boolean
                            If Boolean.TryParse(value, b) Then
                                chk.Checked = value
                            Else
                                chk.Checked = value.Contains(chk.Text)
                            End If
                        ElseIf TypeOf c Is RadioButton Then
                            Dim rdo As RadioButton = c
                            rdo.Checked = rdo.Text = value
                        End If
                    Next
                    GetDataToControls(ctrl, row)
                Case "CheckBox"
                    Dim chk As CheckBox = ctrl
                    If Boolean.Parse(value) Then
                        chk.Checked = value
                    Else
                        chk.Checked = value.Contains(chk.Text)
                    End If
                Case Else
            End Select
        Next
    End Sub

    ''' <summary>
    ''' 取得儲存格的內容
    ''' </summary>
    ''' <param name="row">DataRow、DataGridViewRow</param>
    ''' <param name="colName"></param>
    ''' <returns></returns>
    Private Function GetCellData(row As Object, colName As String) As String
        Select Case row.GetType.Name
            Case "DataRow"
                Dim r As DataRow = row
                Return r(colName).ToString
            Case "DataGridViewRow"
                Dim r As DataGridViewRow = row
                Return r.Cells(colName).Value.ToString
            Case Else
                Return ""
        End Select
    End Function

    ''' <summary>
    ''' 將資料放到DataGridView
    ''' </summary>
    ''' <param name="sql"></param>
    ''' <param name="dgv"></param>
    Public Sub GetDataToDgv(sql As String, dgv As DataGridView)
        'With dgv
        '    .DataSource = SelectTable(sql)
        '    Dim lstTableNames = GetTableNamesFromQuery(sql)
        '    '條件式
        '    Dim conditions = String.Join(" OR ", lstTableNames.Select(Function(x) $"Table_name = '{x}'"))
        '    '用table欄位的備註將dgv的欄位改名
        '    Dim tableCol = SelectTable($"SELECT COLUMN_NAME, COLUMN_COMMENT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'gas_accounting_system' AND {conditions}")
        '    For Each col As DataGridViewColumn In .Columns
        '        Dim row = tableCol.AsEnumerable().FirstOrDefault(Function(x) x("COLUMN_NAME").ToString() = col.Name)
        '        If row IsNot Nothing Then
        '            col.HeaderText = row("COLUMN_COMMENT").ToString()
        '        End If
        '    Next
        '    .AutoResizeColumnHeadersHeight()
        'End With
    End Sub

    ''' <summary>
    ''' 取得指定控制項內所有的目標控制項
    ''' </summary>
    ''' <typeparam name="T">目標控制項</typeparam>
    ''' <param name="parent">父控制項</param>
    ''' <returns></returns>
    Public Function GetControlInParent(Of T As Control)(parent As Control) As List(Of T)
        Dim lst As New List(Of T)
        If parent.Controls.Count > 0 Then
            For Each ctrl In parent.Controls
                If TypeOf ctrl Is T Then lst.Add(ctrl)
                lst.AddRange(GetControlInParent(Of T)(ctrl))
            Next
        End If
        Return lst
    End Function

    ''' <summary>
    ''' 檢查TextBox裡是否為正整數
    ''' </summary>
    ''' <param name="dic"></param>
    ''' <returns></returns>
    Public Function CheckPositiveInteger(dic As Dictionary(Of String, Object)) As Boolean
        For Each kvp In dic
            If Not IsNumeric(kvp.Value.Text) Then
                MsgBox(kvp.Key + " 不為數字!")
                kvp.Value.Focus()
                Return False
            End If
            If Val(kvp.Value.Text) < 0 Then
                MsgBox(kvp.Key + " 不能為負數!")
                kvp.Value.Focus()
                Return False
            End If
        Next
        Return True
    End Function

    Public Sub SaveDataGridWidth(sender As Object, e As EventArgs)
        Dim dgv As DataGridView = sender
        With dgv
            Dim lst As New List(Of String)
            For Each col As DataGridViewColumn In .Columns
                lst.Add(col.Width)
            Next

            Dim filePath = Path.Combine(Application.StartupPath, "DGVWidth.set")
            Dim lines As List(Of String) = If(File.Exists(filePath), File.ReadAllLines(filePath).ToList, New List(Of String))
            Dim bReplace As Boolean = False
            For i As Integer = 0 To lines.Count - 1
                Dim parts = lines(i).Split(":")
                If parts(0) = .Name Then
                    parts(1) = String.Join(",", lst)
                    lines(i) = String.Join(":", parts)
                    bReplace = True
                    Exit For
                End If
            Next

            If Not bReplace Then
                lines.Add($"{ .Name}:{String.Join(",", lst)}")
            End If

            File.WriteAllLines(filePath, lines)
        End With
    End Sub

    ''' <summary>
    ''' 讀取並設定欄寬,要輸入資料後才能讀取
    ''' </summary>
    ''' <param name="dgvs"></param>
    Sub ReadDataGridWidth(dgvs As List(Of DataGridView))
        Try
            Dim lines = File.ReadAllLines(Application.StartupPath + "\DGVWidth.set").ToList
            For Each dgv In dgvs
                Dim line = lines.FirstOrDefault(Function(l) l.StartsWith(dgv.Name))
                If line IsNot Nothing Then
                    Dim widths = line.Split(":")(1).Split(",")
                    For i As Integer = 0 To widths.Length - 1
                        dgv.Columns(i).Width = Integer.Parse(widths(i))
                    Next
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' 設定DataGridView的樣式屬性
    ''' </summary>
    ''' <param name="container">父容器</param>
    Public Sub SetDataGridViewStyle(container As Control)
        For Each dgv In GetControlInParent(Of DataGridView)(container)
            With dgv
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .MultiSelect = False
                .ColumnHeadersDefaultCellStyle.Font = New Font("標楷體", 12, FontStyle.Bold)
                .DefaultCellStyle.Font = New Font("標楷體", 12, FontStyle.Bold)
                .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(224, 224, 224)
                .EnableHeadersVisualStyles = False
                .ColumnHeadersDefaultCellStyle.BackColor = Color.MediumTurquoise
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .ReadOnly = True
                .AllowUserToResizeColumns = True
            End With
        Next
    End Sub

    ''' <summary>
    ''' 動態設定搜尋欄按下Enter即搜尋事件
    ''' </summary>
    ''' <param name="parent"></param>
    Public Sub SetQueryEnterEven(parent As Control)
        GetControlInParent(Of TextBox)(parent).Where(Function(txt) txt.Name.Contains("txtQuery")).ToList.ForEach(Sub(t)
                                                                                                                     AddHandler t.KeyPress, AddressOf txtQuery_KeyPress
                                                                                                                 End Sub)
    End Sub

    '搜尋欄位按下"Enter"即可搜尋
    Private Sub txtQuery_KeyPress(txt As TextBox, e As KeyPressEventArgs)
        If e.KeyChar = vbCr Then
            Dim btn As Button = txt.Parent.Controls.OfType(Of Button).FirstOrDefault(Function(x) x.Name.Contains("Query"))
            btn.PerformClick()
        End If
    End Sub

    ''' <summary>
    ''' 檢查必填欄位
    ''' </summary>
    ''' <param name="ctrls"></param>
    ''' <returns></returns>
    Public Function CheckRequired(ctrls As List(Of Control)) As Boolean
        For Each ctrl As Control In ctrls
            If (TypeOf ctrl Is TextBox Or TypeOf ctrl Is ComboBox) AndAlso String.IsNullOrEmpty(ctrl.Text) Then
                MsgBox($"請填寫必填欄位", Title:="資料驗證")
                ctrl.Focus()
                Return False
            End If
        Next

        Return True
    End Function

    Public Sub TextBox_KeyPress_Int(sender As Object, e As KeyPressEventArgs)
        ' 檢查按下的鍵是否為數字或控制鍵（如退格鍵）
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' 阻止字符輸入
        End If
    End Sub

    ''' <summary>
    ''' 設置按鈕的啟用狀態
    ''' </summary>
    ''' <param name="sender">觸發事件的控件</param>
    ''' <param name="enableCreate">是否啟用新增按鈕</param>
    Public Sub SetButtonState(sender As Control, enableCreate As Boolean)
        Dim parent = sender.Parent
        Dim btns = parent.Controls.OfType(Of Button)
        btns.First(Function(x) x.Text = "新  增").Enabled = enableCreate
        btns.Where(Function(x) x.Text = "修  改" Or x.Text = "刪  除").ToList.ForEach(Sub(y) y.Enabled = Not enableCreate)
    End Sub

    Public Function PositiveIntegerOnly_TextBox(sender As Object, e As KeyPressEventArgs) As Boolean
        ' 檢查按鍵是否為數字或控制鍵（如退格鍵）
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            ' 如果不是數字或控制鍵，取消事件，不允許字符被輸入
            e.Handled = True
            Return False
        End If

        Return True
    End Function

    Public Sub SetQueryControls(btn As Button, ctrls As List(Of Control))
        Dim c As Color
        If btn.Text = "查  詢" Then
            btn.Text = "確  認"
            c = Color.LightGreen
        Else
            btn.Text = "查  詢"
            c = SystemColors.Control
        End If

        For Each ctrl In ctrls
            ctrl.BackColor = c
        Next
    End Sub

    ''' <summary>
    ''' 自定義TabPage索引標籤、文字顏色
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub TabControl_DrawItem(sender As TabControl, e As DrawItemEventArgs)
        '檢查當前索引標籤是否為選中狀態
        Dim isSelected As Boolean = (e.State And DrawItemState.Selected) = DrawItemState.Selected
        '繪製索引標籤的背景
        Dim backColor As Color = If(isSelected, Color.LightBlue, Color.WhiteSmoke)
        e.Graphics.FillRectangle(New SolidBrush(backColor), e.Bounds)
        '繪製索引標籤的文字
        Dim text As String = sender.TabPages(e.Index).Text
        Dim textColor As Color = Color.Black
        Dim font As Font = sender.Font
        e.Graphics.DrawString(text, font, New SolidBrush(textColor), e.Bounds.Location)
    End Sub

    Public Sub PositiveFloatOnly_TextBox(sender As Object, e As KeyPressEventArgs)
        ' 檢查輸入的是不是數字或控制鍵，如果不是，則取消事件
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso (e.KeyChar <> ".") Then
            e.Handled = True
        End If

        ' 只允許一個小數點
        If (e.KeyChar = ".") AndAlso DirectCast(sender, TextBox).Text.IndexOf(".") > -1 Then
            e.Handled = True
        End If

        ' 如果第一個字元是小數點，則在前面加上0
        If (e.KeyChar = ".") AndAlso DirectCast(sender, TextBox).Text.Length = 0 Then
            DirectCast(sender, TextBox).Text = "0."
            DirectCast(sender, TextBox).Select(2, 0)
            e.Handled = True
        End If
    End Sub

    Public Sub FloatOnly_TextBox(sender As Object, e As KeyPressEventArgs)
        ' 允許數字、控制字符、負號和小數點
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso
       (e.KeyChar <> "."c) AndAlso (e.KeyChar <> "-"c) Then
            e.Handled = True ' 拒絕字符
        End If

        ' 只允許一個小數點和在第一位的單個負號
        Dim textBox As TextBox = DirectCast(sender, TextBox)
        If (e.KeyChar = "."c AndAlso textBox.Text.IndexOf("."c) > -1) OrElse
       (e.KeyChar = "-"c AndAlso textBox.Text.Length > 0 AndAlso textBox.Text.IndexOf("-"c) = 0) Then
            e.Handled = True ' 拒絕字符
        End If

        ' 如果第一個字符是小數點，自動在前面加上0
        If (e.KeyChar = "."c) AndAlso (textBox.Text.Length = 0 OrElse textBox.SelectionStart = 0) Then
            textBox.Text = "0" & textBox.Text
            textBox.SelectionStart = 1
        End If

        ' 如果第一個字符是負號，確保它是唯一的且在文本的開始位置
        If (e.KeyChar = "-"c) AndAlso (textBox.SelectionStart <> 0 OrElse textBox.Text.IndexOf("-"c) > -1) Then
            e.Handled = True
        End If
    End Sub

    Public Sub SetComboBox(cmb As ComboBox, data As List(Of ComboBoxItems))
        With cmb
            .DataSource = data
            .ValueMember = "Value"
            .DisplayMember = "Display"
            .SelectedIndex = -1
        End With
    End Sub

    Public Function DGV_SelectionChanged(sender As Object) As Integer
        Dim ctrl As DataGridView = sender
        If Not ctrl.Focused Then Return 0

        SetButtonState(ctrl, False)

        Return ctrl.SelectedRows(0).Cells(0).Value
    End Function
End Module
