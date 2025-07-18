Public Interface IInspectionView
    Sub ShowCustomer(cus As customer)
    Sub Clear()
    Sub DisplayList(data As List(Of InspectionVM))
    Function GetInput() As inspection
    Sub ShowDetail(data As inspection)
End Interface
