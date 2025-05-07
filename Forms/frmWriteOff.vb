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
            .Columns.Add("DiscountAmount", "折讓")
            .Columns.Add("Amount", "應收金額")
            .Columns.Add("PaidAmount", "已銷帳金額")
            .Columns.Add("UnpaidAmount", "未收金額")
            .Columns.Add("WriteOffAmount", "銷帳金額")

            .Columns("MonthlyId").Visible = False
            .Columns("MonthText").ReadOnly = True
            .Columns("TotalAmount").ReadOnly = True
            .Columns("UnpaidAmount").ReadOnly = True
            .Columns("Amount").ReadOnly = True
            .Columns("PaidAmount").ReadOnly = True

            .Columns("MonthText").DefaultCellStyle.BackColor = Color.LightGray
            .Columns("TotalAmount").DefaultCellStyle.Format = "N0"
            .Columns("TotalAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TotalAmount").DefaultCellStyle.BackColor = Color.LightGray
            .Columns("UnpaidAmount").DefaultCellStyle.Format = "N0"
            .Columns("UnpaidAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("UnpaidAmount").DefaultCellStyle.BackColor = Color.LightGray
            .Columns("WriteOffAmount").DefaultCellStyle.Format = "N0"
            .Columns("WriteOffAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DiscountAmount").DefaultCellStyle.Format = "N0"
            .Columns("DiscountAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("Amount").DefaultCellStyle.Format = "N0"
            .Columns("Amount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("Amount").DefaultCellStyle.BackColor = Color.LightGray
            .Columns("PaidAmount").DefaultCellStyle.Format = "N0"
            .Columns("PaidAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PaidAmount").DefaultCellStyle.BackColor = Color.LightGray

            .Columns("MonthText").Width = 80
            .Columns("TotalAmount").Width = 100
            .Columns("UnpaidAmount").Width = 100
            .Columns("WriteOffAmount").Width = 100
            .Columns("DiscountAmount").Width = 100
            .Columns("Amount").Width = 100
            .Columns("PaidAmount").Width = 100
        End With
    End Sub

    Private Sub LoadUnpaidMonths()
        ' 使用 MonthlyAccountService 取得未結案月度帳單
        Dim unpaidMonths = _monthlyAccountService.GetCustomerUnpaidMonths(_cusId)

        ' 將月度帳單資料加入到 DataGridView
        For Each ma As monthly_account In unpaidMonths
            Dim rowIndex As Integer = dgvMonth.Rows.Add() ' 新增一行並獲取行索引
            Dim row As DataGridViewRow = dgvMonth.Rows(rowIndex)
            Dim woRep As New WriteOffRep()
            Dim paidAmount = woRep.GetByMonthlyAccount(ma.ma_id)
            Dim writeOff = woRep.GetByMonthlyAccountAndCollection(ma.ma_id, _colId)

            ' 使用欄位名稱來對應資料
            row.Cells("MonthlyId").Value = ma.ma_id
            row.Cells("MonthText").Value = $"{ma.ma_year}/{ma.ma_month}"
            row.Cells("TotalAmount").Value = ma.ma_total_amount
            row.Cells("DiscountAmount").Value = If(ma.ma_discount, 0)
            row.Cells("WriteOffAmount").Value = If(writeOff IsNot Nothing, writeOff.wo_amount, 0)
            row.Cells("PaidAmount").Value = paidAmount

            Calculate(row) ' 計算應收金額和未收金額
        Next
    End Sub

    Private Sub dgvWriteOff_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvMonth.CellValidating
        ' 只驗證銷帳金額和折扣金額欄位
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

            ' 檢查銷帳金額是否超過未收金額或未銷帳金額
            If writeOffAmount > unpaidAmount Then
                e.Cancel = True
                MessageBox.Show("銷帳金額不能超過未收金額")
                Return
            End If
        ElseIf e.ColumnIndex = dgvMonth.Columns("DiscountAmount").Index Then
            Dim discountAmount As Integer

            ' 檢查是否為有效的數字
            If Not Integer.TryParse(e.FormattedValue.ToString(), discountAmount) Then
                e.Cancel = True
                MessageBox.Show("請輸入有效的整數折扣金額")
                Return
            End If

            ' 取得總金額
            Dim row As DataGridViewRow = dgvMonth.Rows(e.RowIndex)
            Dim totalAmount = CDec(row.Cells("TotalAmount").Value)

            ' 檢查折扣金額是否超過總金額
            If discountAmount > totalAmount Then
                e.Cancel = True
                MessageBox.Show("折扣金額不能超過總金額")
                Return
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
                            Dim writeOffAmount As Integer = row.Cells("WriteOffAmount").Value

                            If writeOffAmount > 0 Then
                                ' 處理銷帳明細
                                Dim monthlyId As Integer = row.Cells("MonthlyId").Value
                                Dim woRep As New WriteOffRep(db)
                                Dim writeOff = woRep.GetByMonthlyAccountAndCollection(monthlyId, _colId)
                                Dim resultWriteOff As Integer

                                If writeOff Is Nothing Then
                                    Dim writeOffData As New write_off With {
                                        .wo_col_id = _colId,
                                        .wo_amount = writeOffAmount,
                                        .wo_date = Date.Now,
                                        .wo_ma_id = monthlyId
                                    }
                                    db.write_off.Add(writeOffData)
                                    resultWriteOff = writeOffAmount
                                Else
                                    Dim orgWriteOffAmount As Integer = writeOff.wo_amount
                                    writeOff.wo_amount = writeOffAmount
                                    writeOff.wo_date = Date.Now
                                    resultWriteOff = writeOffAmount - orgWriteOffAmount
                                End If

                                db.SaveChanges()

                                ' 處理月度帳單
                                Dim ma = db.monthly_account.Find(monthlyId)
                                Dim discount As Integer = row.Cells("DiscountAmount").Value

                                ma.ma_discount = discount
                                ma.ma_paid_amount = woRep.GetByMonthlyAccount(monthlyId)
                                ma.ma_unpaid_amount = ma.ma_total_amount - ma.ma_paid_amount - discount
                                ma.ma_status = ma.ma_unpaid_amount <= 0
                                ma.ma_last_updated = Date.Now

                                ' 更新收款單未銷帳金額
                                collection.col_UnmatchedAmount -= resultWriteOff
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
        ' 確保點擊的行索引有效
        If e.RowIndex < 0 Then Return

        LoadOrderDetails()
    End Sub

    Private Sub dgvMonth_SelectionChanged(sender As Object, e As EventArgs) Handles dgvMonth.SelectionChanged
        ' 確保有選中行且行索引有效
        If dgvMonth.SelectedRows.Count = 0 AndAlso dgvMonth.SelectedCells.Count = 0 Then
            Return
        End If

        LoadOrderDetails()
    End Sub

    Private Sub LoadOrderDetails()
        Try
            ' 確保有選中行且行索引有效
            If dgvMonth.SelectedRows.Count = 0 Then
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
            Dim unmatchedAmount As Decimal = txtUnmatched.Text
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

    Private Sub Calculate(row As DataGridViewRow)
        row.Cells("Amount").Value = row.Cells("TotalAmount").Value - row.Cells("DiscountAmount").Value - row.Cells("WriteOffAmount").Value ' 計算應收金額
        row.Cells("UnpaidAmount").Value = row.Cells("Amount").Value - row.Cells("PaidAmount").Value ' 計算未收金額

    End Sub

    Private Sub dgvMonth_CellValidated(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMonth.CellValidated
        Calculate(dgvMonth.Rows(e.RowIndex))
        Dim totalWriteOff As Integer = 0

        ' 計算所有行的銷帳金額總和
        For Each row As DataGridViewRow In dgvMonth.Rows
            Dim writeOffAmount As Integer = If(row.Cells("WriteOffAmount").Value Is Nothing, 0, CDec(row.Cells("WriteOffAmount").Value))
            totalWriteOff += writeOffAmount
        Next

        If totalWriteOff > CInt(txtAmount.Text) Then MessageBox.Show("銷帳金額總和超過收款單金額", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
End Class