Public Class frmDatePicker
    Public Property SelectedDate As Date

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        SelectedDate = MonthCalendar1.SelectionStart
        DialogResult = DialogResult.OK
        Close()
    End Sub
End Class