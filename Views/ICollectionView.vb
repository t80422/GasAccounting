Public Interface ICollectionView
    Inherits IBaseView(Of collection, CollectionVM)

    Sub SetSubjectCmb(datas As List(Of SelectListItem))
    Sub SetCompanyCmb(datas As List(Of SelectListItem))
    Sub SetBankCmb(datas As List(Of SelectListItem))
    Function GetChequeInput() As cheque
    Function GetChequeNumber() As String
End Interface