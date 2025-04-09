Public Class frmDatePicker
    Public Property SelectedDate As Date = Nothing
    Public Property isMonth As Boolean

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        SelectedDate = MonthCalendar1.SelectionStart
        isMonth = False
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnConfirmMonth_Click(sender As Object, e As EventArgs) Handles btnConfirmMonth.Click
        SelectedDate = MonthCalendar1.SelectionStart
        isMonth = True
        DialogResult = DialogResult.OK
        Close()
    End Sub
End Class