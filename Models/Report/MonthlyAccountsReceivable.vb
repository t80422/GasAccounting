''' <summary>
''' 月應收帳明細
''' </summary>
Public Class MonthlyAccountsReceivable
    Public Property Month As String
    Public Property List As List(Of MonthlyAccountsReceivableList)
End Class

Public Class MonthlyAccountsReceivableList
    Public Property CusCode As String

    ''' <summary>
    ''' 應收帳
    ''' </summary>
    ''' <returns></returns>
    Public Property AccountsReceivable As Single

    ''' <summary>
    ''' 已收帳
    ''' </summary>
    ''' <returns></returns>
    Public Property AccountsReceived As Single

    Public Property Discount As Single
End Class
