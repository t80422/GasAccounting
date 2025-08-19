Public Class frmStartEndDatePicker
    Public Property StartDate As Date
    Public Property EndDate As Date

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        StartDate = dtpStart.Value.Date
        EndDate = dtpEnd.Value.Date
        DialogResult = DialogResult.OK
    End Sub
End Class