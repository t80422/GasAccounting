Imports System.Data.SqlClient
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports MySql.Data.MySqlClient

Module modUtility
    Public Sheets As String() = {"員工管理", "權限管理", "廠商管理", "客戶管理", "公司管理", "基礎價格", "銀行帳戶管理", "採購作業", "付款作業", "應付明細帳", "銷售作業", "收款作業",
    "月結帳單", "應收帳款", "氣款應收帳", "能源局", "提氣量清冊", "保險", "發票機", "財務", "新桶明細", "氣量氣款收付明細", "現金帳", "支票管理", "銀行帳", "應收票據", "應納稅額"}
    Private PropertyCache As New Dictionary(Of Type, Dictionary(Of String, PropertyInfo))

    ''' <summary>
    ''' 取得控制項到資料表實體
    ''' </summary>
    ''' <param name="entity">資料表實體</param>
    ''' <param name="container"></param>
    Public Sub AutoMapControlsToEntity(entity As Object, container As Control)
        Dim entityType = entity.GetType()

        For Each control As Control In container.Controls.OfType(Of Control).Where(Function(x) Not String.IsNullOrEmpty(x.Tag))
            Dim propValue As String = Nothing
            Dim propName As String = Nothing

            If TypeOf control Is TextBox Then
                Dim txt As TextBox = control
                propName = txt.Tag
                propValue = txt.Text

            ElseIf TypeOf control Is ComboBox Then
                Dim cmb As ComboBox = control

                If cmb.SelectedIndex <> -1 Then
                    propName = cmb.Tag

                    If cmb.DataSource Is Nothing Then
                        propValue = cmb.Text
                    Else
                        propValue = cmb.SelectedItem.Key
                    End If
                End If

            ElseIf TypeOf control Is DateTimePicker Then
                Dim dtp As DateTimePicker = control
                propName = dtp.Tag
                propValue = dtp.Value

            ElseIf TypeOf control Is GroupBox Then
                Dim grp As GroupBox = control
                propName = grp.Tag
                propValue = grp.Controls.OfType(Of RadioButton).FirstOrDefault(Function(rdo) rdo.Checked).Text

            ElseIf TypeOf control Is TabControl Then
                Dim tc As TabControl = control
                propName = tc.Tag
                propValue = tc.SelectedTab.Text
            Else
                Continue For
            End If

            If Not String.IsNullOrEmpty(propName) Then
                Dim prop = entityType.GetProperty(propName)

                If prop IsNot Nothing Then
                    Try
                        ' 獲取屬性的基礎類型（處理可空類型）
                        Dim propType = If(Nullable.GetUnderlyingType(prop.PropertyType), prop.PropertyType)
                        If propType.Name = "Int32" Or propType.Name = "Single" Then
                            If String.IsNullOrEmpty(propValue) Then
                                propValue = 0
                            End If
                        End If
                        ' 根據屬性類型轉換值
                        Dim convertedValue = Convert.ChangeType(propValue, propType)
                        prop.SetValue(entity, convertedValue)

                    Catch ex As Exception
                        Throw
                    End Try
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' 取得資料表實體將資料傳到控制項 (控件名稱格式為 "txt[屬性名稱]" 或 "cmb[屬性名稱]" 等)
    ''' </summary>
    ''' <param name="entity">資料表實體</param>
    ''' <param name="container"></param>
    Public Sub AutoMapEntityToControls(entity As Object, container As Control)
        Dim entityType = entity.GetType()

        If Not PropertyCache.ContainsKey(entityType) Then
            PropertyCache(entityType) = entityType.GetProperties().ToDictionary(Function(p) p.Name, Function(p) p)
        End If

        Dim properties = PropertyCache(entityType)

        For Each control As Control In container.Controls.OfType(Of Control).Where(Function(x) x.Tag IsNot Nothing)
            Dim controlName = control.Tag
            Dim prop = entityType.GetProperty(controlName)

            If prop IsNot Nothing Then
                Dim value = prop.GetValue(entity)
                If String.IsNullOrEmpty(value) Then Continue For

                If TypeOf control Is TextBox Then
                    CType(control, TextBox).Text = value?.ToString()

                ElseIf TypeOf control Is ComboBox Then
                    Dim cmb As ComboBox = control
                    If cmb.DataSource Is Nothing Then
                        cmb.SelectedIndex = cmb.FindStringExact(value)
                    Else
                        cmb.SelectedValue = value
                    End If

                ElseIf TypeOf control Is DateTimePicker Then
                    CType(control, DateTimePicker).Value = value

                ElseIf TypeOf control Is GroupBox Then
                    Dim grp As GroupBox = control
                    Dim rdos = grp.Controls.OfType(Of RadioButton)
                    If rdos.Count > 0 Then
                        rdos.FirstOrDefault(Function(x) x.Text = value).Checked = True
                    End If
                Else

                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' 取得資料表 prefix 開頭的欄位資料
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <param name="prefix"></param>
    ''' <returns></returns>
    Public Function GetEntityFieldsByPrefix(entity As Object, prefix As String) As Dictionary(Of String, Object)
        Dim result As New Dictionary(Of String, Object)
        Dim entityType As Type = entity.GetType()

        ' 使用反射獲取所有屬性
        Dim properties As PropertyInfo() = entityType.GetProperties()
        For Each prop As PropertyInfo In properties
            ' 檢查屬性名稱是否以指定的前綴開頭
            If prop.Name.StartsWith(prefix) Then
                ' 將屬性名稱和值添加到結果字典中
                result.Add(prop.Name, prop.GetValue(entity))
            End If
        Next

        Return result
    End Function

    ''' <summary>
    ''' 將資料庫欄位的備註設定到dgv的header
    ''' </summary>
    ''' <param name="tableName"></param>
    ''' <param name="dgv"></param>
    Public Sub SetColumnHeaders(tableName As String, dgv As DataGridView)
        Dim connectionString As String

        Using context As New gas_accounting_systemEntities()
            connectionString = context.Database.Connection.ConnectionString
        End Using

        ' SQL 查詢以獲取欄位備註
        Dim query As String = $"SELECT COLUMN_NAME, COLUMN_COMMENT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'"

        ' 創建 SQL 連接
        Using connection As New MySqlConnection(connectionString)
            ' 創建 SQL 命令
            Using command As New MySqlCommand(query, connection)
                ' 打開連接
                connection.Open()

                ' 執行命令並讀取結果
                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim columnName As String = reader("COLUMN_NAME").ToString()
                        Dim columnComment As String = reader("COLUMN_COMMENT").ToString()

                        ' 為 DataGridView 的對應列設置標題
                        For Each column As DataGridViewColumn In dgv.Columns
                            If column.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase) Then
                                column.HeaderText = columnComment
                                Exit For
                            End If
                        Next
                    End While
                End Using
            End Using
        End Using
    End Sub
End Module