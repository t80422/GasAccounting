''' <summary>
''' 轉帳傳票
''' </summary>
Public Class TransferSubpoenaDTO
    ''' <summary>
    ''' 借方科目名稱
    ''' </summary>
    ''' <returns></returns>
    Public Property DebitSubjectName As String

    ''' <summary>
    ''' 借方摘要
    ''' </summary>
    ''' <returns></returns>
    Public Property DebitSummary As String

    ''' <summary>
    ''' 借方金額
    ''' </summary>
    ''' <returns></returns>
    Public Property DebitAmount As Integer

    ''' <summary>
    ''' 貸方科目名稱
    ''' </summary>
    ''' <returns></returns>
    Public Property CreditSubjectName As String

    ''' <summary>
    ''' 貸方摘要
    ''' </summary>
    ''' <returns></returns>
    Public Property CreditSummary As String

    ''' <summary>
    ''' 貸方金額
    ''' </summary>
    ''' <returns></returns>
    Public Property CreditAmount As Integer

    Public Property Id As Integer
End Class