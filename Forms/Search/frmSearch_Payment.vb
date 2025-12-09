Public Class frmSearch_Payment
    Public Criteria As New PaymentSearchCriteria

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        With Criteria
            .IsSearchDate = chkDate.Checked
            .StartDate = dtpStart.Value.Date
            .EndDate = dtpEnd.Value.Date.AddDays(1)
            .VendorId = If(cmbVendor.SelectedValue, Nothing)
            .Cridit = cmbCredit.SelectedItem
        End With

        DialogResult = DialogResult.OK
    End Sub

    Private Sub frmSearch_Payment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Using db As New gas_accounting_systemEntities
            Dim manuRep As New ManufacturerRep(db)
            SetComboBox(cmbVendor, manuRep.GetVendorDropdownAsync().Result)
        End Using
    End Sub
End Class