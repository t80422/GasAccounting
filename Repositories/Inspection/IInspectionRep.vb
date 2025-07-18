Public Interface IInspectionRep
    Inherits IRepository(Of inspection)

    Function Search(criteria As InspectionSC) As List(Of InspectionVM)
End Interface
