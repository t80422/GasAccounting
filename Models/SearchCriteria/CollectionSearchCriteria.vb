Public Class CollectionSearchCriteria
    ''' <summary>
    ''' 客戶編號
    ''' </summary>
    ''' <returns></returns>
    Public Property CusId As Integer?

    ''' <summary>
    ''' 科目編號
    ''' </summary>
    ''' <returns></returns>
    Public Property SubjectId As Integer?

    ''' <summary>
    ''' 收款類型
    ''' </summary>
    ''' <returns></returns>
    Public Property Type As String

    ''' <summary>
    ''' 支票號碼
    ''' </summary>
    ''' <returns></returns>
    Public Property ChequeNum As String

    ''' <summary>
    ''' 是否日期範圍
    ''' </summary>
    ''' <returns></returns>
    Public Property IsDate As Boolean

    ''' <summary>
    ''' 開始日期
    ''' </summary>
    ''' <returns></returns>
    Public Property StartDate As Date?

    ''' <summary>
    ''' 結束日期
    ''' </summary>
    ''' <returns></returns>
    Public Property EndDate As Date?

    Public Property BankId As Integer?
End Class
