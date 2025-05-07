Public Interface IBankView
    Inherits ICommonView_old(Of bank, BankVM)

    Sub PopulateCompanyDropdown(data As List(Of SelectListItem))
End Interface
