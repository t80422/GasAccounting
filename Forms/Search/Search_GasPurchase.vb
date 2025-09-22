Public Class Search_GasPurchase
    Private ReadOnly _companyRep As ICompanyRep
    Private ReadOnly _vendorRep As IManufacturerRep

    Public Property Criteria As New PurchaseCondition

    Public Sub New(companyRep As ICompanyRep, vendorRep As IManufacturerRep)
        InitializeComponent()

        _companyRep = companyRep
        _vendorRep = vendorRep
    End Sub

    Private Sub Search_GasPurchase_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetComboBox(cmbCompany, _companyRep.GetCompanyDropdownAsync.Result)
        SetComboBox(cmbGasVendor, _vendorRep.GetGasVendorCmbDataAsync.Result)
        SetComboBox(cmbTransportation, _vendorRep.GetVendorCmbWithoutGasAsync.Result)
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        With Criteria
            .StartDate = dtpStart.Value.Date
            .EndDate = dtpEnd.Value.Date
            .IsDateSearch = chkDate.Checked
            .CompanyId = cmbCompany.SelectedValue
            .Product = cmbProduct.SelectedItem
            .ManufacturerId = cmbGasVendor.SelectedValue
            .TransportationId = cmbTransportation.SelectedValue
        End With

        DialogResult = DialogResult.OK
    End Sub
End Class