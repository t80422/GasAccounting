Public Class frmQueryCustomer
    Public Property ID As String
    Public Property CusName As String

    Private Sub frmQueryCustomer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnClear.PerformClick()
    End Sub

    Private Sub btnQuery_Click(sender As Object, e As EventArgs) Handles btnQuery.Click
        Try
            Using db As New gas_accounting_systemEntities
                Dim query = db.customers.AsQueryable

                If Not String.IsNullOrEmpty(txtCusCode.Text) Then
                    query = query.Where(Function(cus) cus.cus_code.Contains(txtCusCode.Text))
                End If

                If Not String.IsNullOrEmpty(txtCusName.Text) Then
                    query = query.Where(Function(cus) cus.cus_name.Contains(txtCusName.Text))
                End If

                If Not String.IsNullOrEmpty(txtCusContactPerson.Text) Then
                    query = query.Where(Function(cus) cus.cus_contact_person.Contains(txtCusContactPerson.Text))
                End If

                If Not String.IsNullOrEmpty(txtCusPhone.Text) Then
                    query = query.Where(Function(cus) cus.cus_phone1.Contains(txtCusPhone.Text) OrElse cus.cus_phone2.Contains(txtCusPhone.Text))
                End If

                dgvCustomer.DataSource = query.Select(Function(x) New With {.編號 = x.cus_id, .名稱 = x.cus_name}).ToList
                Dim lst = New List(Of DataGridView) From {dgvCustomer}
                ReadDataGridWidth(lst)
            End Using

        Catch ex As Exception
            MsgBox(ex.Message)
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If dgvCustomer.SelectedRows.Count = 0 Then
            ID = ""
            Name = ""
        Else
            ID = dgvCustomer.SelectedRows(0).Cells("編號").Value
            Name = dgvCustomer.SelectedRows(0).Cells("名稱").Value
        End If

    End Sub

    Private Sub dgvCustomer_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvCustomer.ColumnWidthChanged
        SaveDataGridWidth(sender, e)
    End Sub

    Private Sub dgvCustomer_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvCustomer.CellMouseClick
        Dim dgv = CType(sender, DataGridView)
        Dim selectRow = dgv.SelectedRows(0)
        Dim id As Integer = selectRow.Cells("編號").Value

        Try
            Using db As New gas_accounting_systemEntities
                Dim query = From cus In db.customers
                            Where cus.cus_id = id
                            Select cus
                Dim customer = query.FirstOrDefault
                AutoMapEntityToControls(customer, Me)
            End Using

        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearControls(Me)

        Try
            Using db As New gas_accounting_systemEntities
                Dim query = From customer In db.customers
                            Select New With {
                                .編號 = customer.cus_id,
                                .名稱 = customer.cus_name
                            }
                dgvCustomer.DataSource = query.ToList
            End Using

            SetDataGridViewStyle(Me)
            Dim lst = New List(Of DataGridView) From {dgvCustomer}
            ReadDataGridWidth(lst)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class