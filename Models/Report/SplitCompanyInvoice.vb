''' <summary>
''' 分裝場進項銷項
''' </summary>
Public Class SplitCompanyInvoice
    Public Property FrontDate1 As String
    Public Property FrontTax1 As Single
    Public Property FrontAmount1 As Single
    Public Property FrontVendorTaxId1 As String

    Public Property FrontDate2 As String
    Public Property FrontTax2 As Single
    Public Property FrontAmount2 As Single
    Public Property FrontVendorTaxId2 As String

    Public Property EndDate1 As String
    Public Property EndTax1 As Single
    Public Property EndAmount1 As Single
    Public Property EndVendorTaxId1 As String

    Public Property EndDate2 As String
    Public Property EndTax2 As Single
    Public Property EndAmount2 As Single
    Public Property EndVendorTaxId2 As String

    Public Property InList As List(Of InDetail)

    Public Sub New()
        InList = New List(Of InDetail)
    End Sub
End Class

Public Class InDetail
    Public Property Day As String
    Public Property InvoiceNum As String
    Public Property Name As String
    Public Property Tax As Single
    Public Property Amount As Single
    Public Property VendorTaxId As String
End Class
