''' <summary>
''' 轉帳傳票群組：對應「一」側（收入=借方，支出=貸方）的科目名稱、摘要、金額，以及多筆明細。
''' </summary>
Public Class TransferSubpoenaGroup
    ''' <summary>合併側科目名稱</summary>
    Public Property SubjectName As String
    ''' <summary>合併側摘要</summary>
    Public Property Summary As String
    ''' <summary>合併側金額</summary>
    Public Property Amount As Integer
    ''' <summary>展開側明細（1~3 筆）</summary>
    Public Property Details As List(Of TransferSubpoenaDetail)
End Class

''' <summary>
''' 轉帳傳票明細：對應「多」側的每一筆科目、摘要、金額。
''' </summary>
Public Class TransferSubpoenaDetail
    ''' <summary>科目名稱</summary>
    Public Property SubjectName As String
    ''' <summary>摘要</summary>
    Public Property Summary As String
    ''' <summary>金額</summary>
    Public Property Amount As Integer
End Class
