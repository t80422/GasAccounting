Public Interface IJournalService
    Sub Add(entry As journal)

    Sub Edit(entry As journal)

    Sub Delete(subpoenaNo As Integer)

    ''' <summary>
    ''' 取得傳票編號
    ''' </summary>
    ''' <returns></returns>
    Function GetSubpoenaNo() As Integer
End Interface
