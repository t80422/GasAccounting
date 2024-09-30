Public Class BillsReceivable
    Public Property CompanyName As String
    Public Property BankAccount As String
    Public Property List As List(Of BillsReceivableList)
End Class

Public Class BillsReceivableList
    ''' <summary>
    ''' 收票日期
    ''' </summary>
    ''' <returns></returns>
    Public Property ReceiveDate As Date

    Public Property CusCode As String

    Public Property ChequeNumber As String

    ''' <summary>
    ''' 發票人
    ''' </summary>
    ''' <returns></returns>
    Public Property IssuerName As String

    Public Property PayBankName As String

    ''' <summary>
    ''' 可兌換日期
    ''' </summary>
    ''' <returns></returns>
    Public Property AvailableDate As Date

    Public Property Amount As Integer

    ''' <summary>
    ''' 代收日期
    ''' </summary>
    ''' <returns></returns>
    Public Property CollectDate As Date?

    Public Property Memo As String
End Class
