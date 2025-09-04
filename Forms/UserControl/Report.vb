Public Class Report
    Inherits UserControl
    Implements IReportView

    Private _presenter As ReportPresenter

    Public Sub New(presenter As ReportPresenter)
        InitializeComponent()
        _presenter = presenter
    End Sub

    Public Sub SetGasVendorCmb(item As List(Of SelectListItem)) Implements IReportView.SetGasVendorCmb
        SetComboBox(cmbManu, item)
    End Sub

    Public Sub SetBankAccountCmb(items As List(Of SelectListItem)) Implements IReportView.SetBankAccountCmb
        SetComboBox(cmbBankAccount_BankAccount, items)
    End Sub

    Public Sub SetCompanyCmb(items As List(Of SelectListItem)) Implements IReportView.SetCompanyCmb
        SetComboBox(cmbCompany_ITD, items)
        SetComboBox(cmbCompany_insurance, items)
        SetComboBox(cmbCompany_IS, items)
    End Sub

    Private Sub Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _presenter.SetView(Me)
        btnRefresh.PerformClick()
    End Sub

    ' 大氣進貨明細
    Private Sub btnGasPayableDetail_Click(sender As Object, e As EventArgs) Handles btnGasPayableDetail.Click
        If Not (cmbManu.SelectedIndex = -1 And String.IsNullOrEmpty(cmbManu.Text)) Then
            _presenter.GenerateGasPayableDetail(Now.Date, cmbManu.SelectedValue)
        End If
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        _presenter.LoadBankAccount()
        _presenter.LoadCompany()
        _presenter.GetManuCmb()
    End Sub

    ' 進項銷項
    Private Sub btnInOut_Click(sender As Object, e As EventArgs) Handles btnInOut.Click
        _presenter.GenerateInOut(dtpYear_InOut.Value, cmbMonth_InOut.SelectedItem)
    End Sub

    ' 銀行帳
    Private Sub btnBankAccount_Click(sender As Object, e As EventArgs) Handles btnBankAccount.Click
        _presenter.GenerateBankAccount(dtpMonth_BankAccount.Value.Date, cmbBankAccount_BankAccount.SelectedItem.Value)
    End Sub

    ' 單一客戶每月的應收帳明細表
    Private Sub btnMonthlyCusReceivable_Click(sender As Object, e As EventArgs) Handles btnMonthlyCusReceivable.Click
        _presenter.GenerateMonthlyCustomerReceivable(Now.Date, txtCusCode_dcr.Text)
    End Sub

    ' 提氣支數統計
    Private Sub btnGasUsageCylinderCount_Click(sender As Object, e As EventArgs) Handles btnGasUsageCylinderCount.Click
        _presenter.GenerateGasUsageAndCylinderCount(dtpDate_gucc.Value.Date)
    End Sub

    ' 現金帳
    Private Sub btnCashAccount_Click(sender As Object, e As EventArgs) Handles btnCashAccount.Click
        _presenter.GenerateCashAccount(dtpStart_ca.Value.Date, dtpEnd_ca.Value.Date)
    End Sub

    ' 客戶寄桶結存瓶
    Private Sub btnCGCI_Click(sender As Object, e As EventArgs) Handles btnCGCI.Click
        _presenter.GenerateCustomerGasCylinderInventory(txtCusId_cgci.Text)
    End Sub

    ' 客戶寄桶結存瓶-客戶搜尋
    Private Sub btnSearchCus_cgci_Click(sender As Object, e As EventArgs) Handles btnSearchCus_cgci.Click
        Using searchForm As New frmQueryCustomer
            If searchForm.ShowDialog = DialogResult.OK Then
                txtCusId_cgci.Text = searchForm.CusId
                txtCusName_cgci.Text = searchForm.CusName
                txtCusCode_cgci.Text = searchForm.CusCode
            End If
        End Using
    End Sub

    ' 新桶明細
    Private Sub btnNewBarrel_Click(sender As Object, e As EventArgs) Handles btnNewBarrel.Click
        _presenter.GenerateNewBarrelDetails(dtpMonth_newBarrel.Value)
    End Sub

    ' 每日科目匯總表
    Private Sub btnDSS_Click(sender As Object, e As EventArgs) Handles btnDSS.Click
        _presenter.GenerateDailySubjectSummary(dtpDate_DSS.Value)
    End Sub

    ' 進銷存明細
    Private Sub btnITD_Click(sender As Object, e As EventArgs) Handles btnITD.Click
        _presenter.GenerateInventoryTransactionDetail(dtpYear_ITD.Value, cmbCompany_ITD.SelectedItem.Value, Nothing)
    End Sub

    ' 發票明細
    Private Sub btnTax_Click(sender As Object, e As EventArgs) Handles btnTax.Click
        _presenter.GenerateTax(dtpMonth_tax.Value)
    End Sub

    ' 能源局
    Private Sub btnEnergyBureau_Click(sender As Object, e As EventArgs) Handles btnEnergyBureau.Click
        _presenter.GenerateEnergyBureau(dtpMonth_EB.Value)
    End Sub

    ' 月結帳單
    Private Sub btnMonthlyStatement_Click(sender As Object, e As EventArgs) Handles btnMonthlyStatement.Click
        _presenter.GenerateMonthlyStatement(txtCusCode_MS.Text, dtpMonth_MS.Value)
    End Sub

    ' 保險
    Private Sub btnInsurance_Click(sender As Object, e As EventArgs) Handles btnInsurance.Click
        _presenter.GenerateInsurance(cmbCompany_insurance.SelectedItem.Value, dtpMonth_insurance.Value)
    End Sub

    ' 損益表
    Private Sub btnIncomeStatement_Click(sender As Object, e As EventArgs) Handles btnIncomeStatement.Click
        _presenter.GenerateIncomeStatement(dtpStart_IS.Value, dtpEnd_IS.Value, cmbCompany_IS.SelectedItem.Value)
    End Sub

    ' 月應收帳明細
    Private Sub btnMAR_Click(sender As Object, e As EventArgs) Handles btnMAR.Click
        _presenter.GenerateMonthlyAccountsReceivable(dtpMonth_MAR.Value)
    End Sub

    ' 客戶發票明細
    Private Sub btnGenerate_RI_Click(sender As Object, e As EventArgs) Handles btnGenerate_RI.Click
        _presenter.GenerateInvoice(dtpMonth_RI.Value)
    End Sub

    ' 科目平衡表
    Private Sub btnAccountBalance_Click(sender As Object, e As EventArgs) Handles btnAccountBalance.Click
        _presenter.GenerateAccountBalance(dtpAccountBalance.Value.Date)
    End Sub
End Class