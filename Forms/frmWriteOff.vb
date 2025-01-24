Public Class frmWriteOff
    Private ReadOnly _colId As Integer
    Private _cusId As Integer

    Public Sub New(colId As Integer)
        InitializeComponent()

        _colId = colId
    End Sub

    Private Sub frmWriteOff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCollectionData()
        SetupDGV()
        LoadUnpaidOrders()
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
        With dgvWriteOff
            .Columns.Clear()

            .Columns.Add("OrderId", "編號")
            .Columns.Add("OrderDate", "日期")
            .Columns.Add("TotalAmount", "金額")
            .Columns.Add("UnpaidAmount", "未收金額")
            .Columns.Add("WriteOffAmount", "銷帳金額")

            .Columns("OrderId").ReadOnly = True
            .Columns("OrderDate").ReadOnly = True
            .Columns("TotalAmount").ReadOnly = True
            .Columns("UnpaidAmount").ReadOnly = True

            .Columns("TotalAmount").DefaultCellStyle.Format = "N0"
            .Columns("TotalAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("UnpaidAmount").DefaultCellStyle.Format = "N0"
            .Columns("UnpaidAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WriteOffAmount").DefaultCellStyle.Format = "N0"
            .Columns("WriteOffAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("OrderId").Width = 100
            .Columns("OrderDate").Width = 100
            .Columns("TotalAmount").Width = 100
            .Columns("UnpaidAmount").Width = 100
            .Columns("WriteOffAmount").Width = 100
        End With
    End Sub

    Private Sub LoadUnpaidOrders()
        Using db As New gas_accounting_systemEntities
            ' 取得該客戶的未結案訂單
            Dim unpaidOrders = db.orders.
                Where(Function(x) x.o_cus_Id = _cusId AndAlso
                                 x.o_UnpaidAmount > 0).
                OrderBy(Function(x) x.o_date).
                ToList()

            ' 將訂單資料加入到 DataGridView
            For Each order In unpaidOrders
                dgvWriteOff.Rows.Add(
                    order.o_id,
                    order.o_date.Value.ToString("yyyy/MM/dd"),
                    order.o_total_amount,
                    order.o_UnpaidAmount,
                    0
                )
            Next
        End Using
    End Sub

    Private Sub dgvWriteOff_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvWriteOff.CellValidating
        ' 只驗證銷帳金額欄位
        If e.ColumnIndex = dgvWriteOff.Columns("WriteOffAmount").Index Then
            ' 檢查是否為有效的數字
            If Not Decimal.TryParse(e.FormattedValue.ToString(), Nothing) Then
                e.Cancel = True
                MessageBox.Show("請輸入有效的金額")
                Return
            End If

            ' 取得輸入的銷帳金額
            Dim writeOffAmount = Decimal.Parse(e.FormattedValue.ToString())
            ' 取得該筆訂單的未收金額
            Dim unpaidAmount = CDec(dgvWriteOff.Rows(e.RowIndex).Cells("UnpaidAmount").Value)
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

    Private Function ProcessWriteOff() As Boolean
        ' 第一步：驗證是否有選擇要銷帳的訂單
        Dim hasWriteOff As Boolean = False
        Dim totalWriteOffAmount As Decimal = 0

        ' 計算總銷帳金額並檢查是否有選擇銷帳項目
        For Each row As DataGridViewRow In dgvWriteOff.Rows
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
            MessageBox.Show("請至少選擇一筆訂單進行銷帳", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
                        For Each row As DataGridViewRow In dgvWriteOff.Rows
                            Dim writeOffAmount As Decimal = If(row.Cells("WriteOffAmount").Value Is Nothing,
                                                         0,
                                                         CDec(row.Cells("WriteOffAmount").Value))

                            If writeOffAmount > 0 Then
                                ' 取得訂單編號
                                Dim orderId As Integer = row.Cells("OrderId").Value

                                ' 建立銷帳記錄
                                Dim mapping = New order_collection_mapping With {
                                    .ocm_o_Id = orderId,
                                    .ocm_col_Id = _colId,
                                    .ocm_Amount = writeOffAmount,
                                    .ocm_Date = Date.Today
                                }
                                db.order_collection_mapping.Add(mapping)

                                ' 更新訂單未收金額
                                Dim order = db.orders.Find(orderId)
                                order.o_UnpaidAmount -= writeOffAmount

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
        If ProcessWriteOff() Then
            DialogResult = DialogResult.OK
            Close()
        End If
    End Sub
End Class