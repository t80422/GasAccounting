Public Class InvoiceSearchCriteria
    Private _Month As Date

    Public Property CusId As Integer

    Public Property Month As Date
        Get
            Return _Month.Date
        End Get
        Set
            _Month = Value
        End Set
    End Property

    Public Property Number As String

    Public Property IsSearchMonth As Boolean
End Class
