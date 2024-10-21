''' <summary>
''' 銷項
''' </summary>
Public Class OutInvoice
    Public Property Companies As List(Of CompanyInvoiceData)
End Class

Public Class CompanyInvoiceData
    Public Property CompanyName As String
    Public Property MonthlyData As List(Of MonthInvoiceData)
End Class

Public Class MonthInvoiceData
    Public Property Month As Integer
    Public Property RegularInvoices As List(Of InvoiceGroup)
    Public Property SpecialInvoices As SpecialInvoices
End Class

Public Class InvoiceGroup
    Public Property GroupNumber As Integer
    Public Property Qty As Integer
    Public Property Amount As Single
    Public Property TaxAmount As Single
End Class

Public Class SpecialInvoices
    Public Property TwoPartMachine As InvoiceGroup
    Public Property ThreePartHandwritten As InvoiceGroup
    Public Property TwoPartHandwritten As InvoiceGroup
End Class