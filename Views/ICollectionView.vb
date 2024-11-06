Public Interface ICollectionView
    Inherits IBaseView(Of collection, CollectionVM)

    Sub SetSubjectCmb(datas As List(Of SelectListItem))
    Sub SetCompanyCmb(datas As List(Of SelectListItem))
    Sub SetBankCmb(datas As List(Of SelectListItem))
    Function GetSearchCriteria() As CollectionSearchCriteria
    Function GetChequeInput() As cheque
End Interface