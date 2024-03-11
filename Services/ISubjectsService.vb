Public Interface ISubjectsService
    ''' <summary>
    ''' 取得科目選項
    ''' </summary>
    ''' <param name="type">借、貸</param>
    ''' <returns></returns>
    Function GetSubjectsCmbItems(Optional type As String = Nothing) As List(Of ComboBoxItems)
End Interface
