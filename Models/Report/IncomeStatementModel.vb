''' <summary>
''' 損益表資料
''' </summary>
Public Class IncomeStatementModel
    Public Property CompanyName As String

    Public Property DateRange As String

    ''' <summary>
    ''' 營業收入
    ''' </summary>
    Public Property OperatingIncome As Integer

    ''' <summary>
    ''' 氣款收入
    ''' </summary>
    ''' <returns></returns>
    Public Property GasIncome As Integer

    ''' <summary>
    ''' 銷貨折讓
    ''' </summary>
    ''' <returns></returns>
    Public Property SalesDiscount As Integer

    ''' <summary>
    ''' 進貨
    ''' </summary>
    ''' <returns></returns>
    Public Property Income As Integer

    ''' <summary>
    ''' 營業外收益集合
    ''' </summary>
    ''' <returns></returns>
    Public Property CollectionsList As List(Of IncomeStatementItem)

    ''' <summary>
    ''' 營業費用集合
    ''' </summary>
    ''' <returns></returns>
    Public Property PaymentList As List(Of IncomeStatementItem)
End Class

''' <summary>
''' 損益項目
''' </summary>
Public Class IncomeStatementItem
    Public Property Subject As String
    Public Property Amount As Single
End Class