Public Class frmWriteOff
    Private ReadOnly _colId As Integer
    Private _cusId As Integer
    Private _monthlyAccountService As IMonthlyAccountService

    Public Sub New(colId As Integer)
        InitializeComponent()

        _colId = colId
        _monthlyAccountService = New MonthlyAccountService()
    End Sub

    Private Sub frmWriteOff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCollectionData()
        SetupDGV()
        LoadUnpaidMonths()

        Using db As New gas_accounting_systemEntities
            If db.monthly_account.Any Then btnInit.Visible = False
        End Using
    End Sub

    Private Sub LoadCollectionData()
        Using db As New gas_accounting_systemEntities
            Dim col = db.collections.Find(_colId)
            _cusId = col.col_cus_Id

            txtAmount.Text = col.col_Amount
            txtCusName.Text = col.customer.cus_name
            txtDate.Text = col.col_Date.ToString("yyyy/MM/dd")
            txtType.Text = col.col_Type
            txtUnmatched.Text = col.col_UnmatchedAmount
        End Using
    End Sub

    Private Sub SetupDGV()
        With dgvMonth
            .Columns.Clear()

            .Columns.Add("MonthlyId", "編號")
            .Columns.Add("MonthText", "年月")
            .Columns.Add("TotalAmount", "金額")
            .Columns.Add("UnpaidAmount", "未收金額")
            .Columns.Add("WriteOffAmount", "銷帳金額")

            ' 隱藏編號欄位
            .Columns("MonthlyId").Visible = False
            .Columns("MonthText").ReadOnly = True
            .Columns("TotalAmount").ReadOnly = True
            .Columns("UnpaidAmount").ReadOnly = True

            .Columns("TotalAmount").DefaultCellStyle.Format = "N0"
            .Columns("TotalAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("UnpaidAmount").DefaultCellStyle.Format = "N0"
            .Columns("UnpaidAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WriteOffAmount").DefaultCellStyle.Format = "N0"
            .Columns("WriteOffAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("MonthText").Width = 100
            .Columns("TotalAmount").Width = 100
            .Columns("UnpaidAmount").Width = 100
            .Columns("WriteOffAmount").Width = 100
        End With
    End Sub

    Private Sub LoadUnpaidMonths()
        ' 使用 MonthlyAccountService 取得未結案月度帳單
        Dim unpaidMonths = _monthlyAccountService.GetCustomerUnpaidMonths(_cusId)

        ' 將月度帳單資料加入到 DataGridView
        For Each month As monthly_account In unpaidMonths
            dgvMonth.Rows.Add(
                month.ma_id,
                $"{month.ma_year}/{month.ma_month}",
                month.ma_total_amount,
                month.ma_unpaid_amount,
                0
            )
        Next
    End Sub

    Private Sub dgvWriteOff_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvMonth.CellValidating
        ' 只驗證銷帳金額欄位
        If e.ColumnIndex = dgvMonth.Columns("WriteOffAmount").Index Then
            ' 檢查是否為有效的數字
            If Not Decimal.TryParse(e.FormattedValue.ToString(), Nothing) Then
                e.Cancel = True
                MessageBox.Show("請輸入有效的金額")
                Return
            End If

            ' 取得輸入的銷帳金額
            Dim writeOffAmount = Decimal.Parse(e.FormattedValue.ToString())
            ' 取得該筆月度帳單的未收金額
            Dim unpaidAmount = CDec(dgvMonth.Rows(e.RowIndex).Cells("UnpaidAmount").Value)
            ' 取得目前的未銷帳金額
            Dim unmatchedAmount = CDec(txtUnmatched.Text)

            ' 檢查銷帳金額是否超過未收金額或未銷帳金額
            If writeOffAmount > unpaidAmount Then
                e.Cancel = True
                MessageBox.Show("銷帳金額不能超過未收金額")
            ElseIf writeOffAmount > unmatchedAmount Then
                e.Cancel = True
                MessageBox.Show("銷帳金額不能超過未銷帳金額")
            End If
        End If
    End Sub

    Private Function ProcessMonthlyWriteOff() As Boolean
        ' 第一步：驗證是否有選擇要銷帳的月份
        Dim hasWriteOff As Boolean = False
        Dim totalWriteOffAmount As Decimal = 0

        ' 計算總銷帳金額並檢查是否有選擇銷帳項目
        For Each row As DataGridViewRow In dgvMonth.Rows
            Dim writeOffAmount As Decimal = If(row.Cells("WriteOffAmount").Value Is Nothing,
                                         0,
                                         CDec(row.Cells("WriteOffAmount").Value))
            If writeOffAmount > 0 Then
                hasWriteOff = True
                totalWriteOffAmount += writeOffAmount
            End If
        Next

        ' 如果沒有任何銷帳項目，顯示錯誤訊息並返回
        If Not hasWriteOff Then
            MessageBox.Show("請至少選擇一個月份進行銷帳", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' 第二步：確認銷帳總額不超過未銷帳金額
        Dim unmatchedAmount As Decimal = CDec(txtUnmatched.Text)
        If totalWriteOffAmount > unmatchedAmount Then
            MessageBox.Show("銷帳總金額不能超過未銷帳金額", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        ' 第三步：執行銷帳處理
        Try
            Using db As New gas_accounting_systemEntities
                ' 開始資料庫交易
                Using transaction = db.Database.BeginTransaction()
                    Try
                        ' 取得收款單資料
                        Dim collection = db.collections.Find(_colId)

                        ' 處理每一筆銷帳資料
                        For Each row As DataGridViewRow In dgvMonth.Rows
                            Dim writeOffAmount As Decimal = If(row.Cells("WriteOffAmount").Value Is Nothing,
                                                         0,
                                                         CDec(row.Cells("WriteOffAmount").Value))

                            If writeOffAmount > 0 Then
                                ' 解析月份文字取得年月
                                Dim monthlyId As Integer = CInt(row.Cells("MonthlyId").Value)
                                Dim monthlyData = db.monthly_account.Find(monthlyId)
                                Dim year = monthlyData.ma_year
                                Dim month = monthlyData.ma_month

                                ' 更新月度帳單資料
                                _monthlyAccountService.UpdateMonthlyAccountAfterWriteOff(_cusId, year, month, writeOffAmount)

                                ' 更新收款單未銷帳金額
                                collection.col_UnmatchedAmount -= writeOffAmount
                            End If
                        Next

                        ' 儲存變更
                        db.SaveChanges()

                        ' 確認交易
                        transaction.Commit()

                        ' 顯示成功訊息
                        MessageBox.Show("銷帳作業完成", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return True

                    Catch ex As Exception
                        ' 發生錯誤時回滾交易
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show($"銷帳作業發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        If ProcessMonthlyWriteOff() Then
            DialogResult = DialogResult.OK
            Close()
        End If
    End Sub

    Private Sub btnInit_Click(sender As Object, e As EventArgs) Handles btnInit.Click
        Using frm As New frmInitializeMonthlyAccounts
            frm.ShowDialog()
        End Using
    End Sub

    Private Sub dgvMonth_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvMonth.CellMouseClick
        LoadOrderDetails()
    End Sub

    Private Sub dgvMonth_SelectionChanged(sender As Object, e As EventArgs) Handles dgvMonth.SelectionChanged
        LoadOrderDetails()
    End Sub

    Private Sub LoadOrderDetails()
        Try
            ' 確保有選中行且行索引有效
            If dgvMonth.SelectedRows.Count = 0 AndAlso dgvMonth.SelectedCells.Count = 0 Then
                Return
            End If

            Dim rowIndex As Integer

            If dgvMonth.SelectedRows.Count > 0 Then
                rowIndex = dgvMonth.SelectedRows(0).Index
            Else
                rowIndex = dgvMonth.SelectedCells(0).RowIndex
            End If

            ' 獲取選中月份的月度帳單ID以及年月
            Dim monthlyId As Integer = dgvMonth.Rows(rowIndex).Cells("MonthlyId").Value
            Dim monthText As String = dgvMonth.Rows(rowIndex).Cells("MonthText").Value.ToString()
            Dim parts = monthText.Split("/"c)
            Dim year As Integer = parts(0)
            Dim month As Integer = parts(1)

            ' 設置dgvDetail
            SetupDetailDGV()

            ' 查詢該月份的訂單詳細
            Using db As New gas_accounting_systemEntities
                Dim orders = db.orders.Where(Function(o) _
                                o.o_cus_Id = _cusId AndAlso
                                o.o_date.HasValue AndAlso
                                o.o_date.Value.Year = year AndAlso
                                o.o_date.Value.Month = month).
                          OrderBy(Function(o) o.o_date).
                          ToList()

                ' 顯示訂單詳細
                dgvDetail.Rows.Clear()

                For Each order In orders
                    dgvDetail.Rows.Add(
                        order.o_date.Value.ToString("yyyy/MM/dd"),
                        order.o_total_amount
                    )
                Next
            End Using
        Catch ex As Exception
            MessageBox.Show($"載入訂單詳細時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetupDetailDGV()
        ' 如果已設置，則不重複設置
        If dgvDetail.Columns.Count > 0 Then Return

        With dgvDetail
            .Columns.Clear()

            .Columns.Add("OrderDate", "日期")
            .Columns.Add("TotalAmount", "總金額")

            .Columns("OrderDate").ReadOnly = True
            .Columns("TotalAmount").ReadOnly = True

            .Columns("TotalAmount").DefaultCellStyle.Format = "N0"
            .Columns("TotalAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("OrderDate").Width = 100
            .Columns("TotalAmount").Width = 100
        End With
    End Sub

    Private Sub btnAuto_Click(sender As Object, e As EventArgs) Handles btnAuto.Click
        Try
            ' 取得未銷帳金額
            Dim unmatchedAmount As Decimal = CDec(txtUnmatched.Text)
            If unmatchedAmount <= 0 Then
                MessageBox.Show("沒有未銷帳金額可分配", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' 清除現有的銷帳金額
            For Each row As DataGridViewRow In dgvMonth.Rows
                row.Cells("WriteOffAmount").Value = 0
            Next

            ' 按年月排序（從最早開始）
            Dim sortedRows = dgvMonth.Rows.Cast(Of DataGridViewRow)().
                OrderBy(Function(r) CInt(r.Cells("MonthText").Value.ToString().Split("/"c)(0))). ' 年份
                ThenBy(Function(r) CInt(r.Cells("MonthText").Value.ToString().Split("/"c)(1))). ' 月份
                ToList()

            ' 從最早月份開始分配金額
            Dim remainingAmount = unmatchedAmount
            For Each row In sortedRows
                If remainingAmount <= 0 Then Exit For

                Dim unpaidAmount = CDec(row.Cells("UnpaidAmount").Value)
                ' 分配金額不能超過未付金額
                Dim writeOffAmount = Math.Min(remainingAmount, unpaidAmount)

                row.Cells("WriteOffAmount").Value = writeOffAmount
                remainingAmount -= writeOffAmount
            Next

            ' 如果還有剩餘金額，顯示提示
            If remainingAmount > 0 Then
                MessageBox.Show($"尚有 {remainingAmount:N0} 元未分配", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show($"自動分配金額時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class