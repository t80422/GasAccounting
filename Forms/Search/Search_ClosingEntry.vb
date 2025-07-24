Public Class Search_ClosingEntry
    Public Criteria As New ClosingEntrySC
    Private _subjectRep As New SubjectRep(New gas_accounting_systemEntities)
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        With Criteria
            .StartDate = dtpStart.Value.Date
            .EndDate = dtpEnd.Value.AddDays(1).Date
            .IsDate = chkDate.Checked
            .SubjectId = cmbSuject.SelectedValue
        End With
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Search_ClosingEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim subjects = _subjectRep.GetAllAsync().Result.Select(Function(s) New SelectListItem With {
            .Value = s.s_id,
            .Display = s.s_name
        }).ToList()
        cmbSuject.DataSource = subjects
        cmbSuject.DisplayMember = "Display"
        cmbSuject.ValueMember = "Value"
        cmbSuject.SelectedIndex = -1
    End Sub
End Class