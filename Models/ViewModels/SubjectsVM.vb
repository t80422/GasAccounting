Public Class SubjectsVM
    Public Property 編號 As Integer
    Public Property 分類 As String
    Public Property 名稱 As String
    Public Property 備註 As String

    Public Sub New(subject As subject)
        編號 = subject.s_id
        分類 = subject.s_Type
        名稱 = subject.s_name
        備註 = subject.s_memo
    End Sub
End Class
