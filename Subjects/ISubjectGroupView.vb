Public Interface ISubjectGroupView
    'todo 可以改成繼承ICommonView
    ''' <summary>
    ''' 顯示科目列表到 UI
    ''' </summary>
    ''' <param name="subjects"></param>
    Sub DisplaySubjects(subjects As List(Of SubjectGroupVM))

    ''' <summary>
    ''' 獲取用戶輸入的科目資料
    ''' </summary>
    ''' <returns></returns>
    Function GetUserInput() As subjects_group

    ''' <summary>
    ''' 清除或重置輸入界面
    ''' </summary>
    Sub ClearInputs()

    ''' <summary>
    ''' 將資料傳送至控制項
    ''' </summary>
    ''' <param name="subject"></param>
    Sub SetSubject(subject As subjects_group)
End Interface
