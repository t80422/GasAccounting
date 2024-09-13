''' <summary>
''' 基礎價格
''' </summary>
Public Class BasicPricePresenter
    Private ReadOnly _view As IBasicPriceView
    Private ReadOnly _bpRep As IBasicPriceRep

    Public Sub New(view As IBasicPriceView, bpRep As IBasicPriceRep)
        _view = view
        _bpRep = bpRep
    End Sub

    Public Async Function SearchAsync(isSearch As Boolean) As Task
        Try
            Dim criteria As basic_price = Nothing

            If isSearch Then criteria = _view.GetUserInput

            Dim datas = Await _bpRep.SearchAsync(criteria)
            _view.DisplayList(datas.OrderByDescending(Function(x) x.bp_date).Select(Function(x) New BasicPriceMV(x)).ToList)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Function AddAsync() As Task
        Try
            Dim data = _view.GetUserInput

            If Not Validation(data) Then
                MsgBox("請填寫必填項目")
                Return
            End If

            Dim checkDuplicate = Await _bpRep.CheckDuplicateMonthAsync(data.bp_date)

            If Not checkDuplicate Then
                Await _bpRep.AddAsync(data)
                MsgBox("新增成功")
                _view.ClearInput()
                Await SearchAsync(False)
            Else
                MsgBox("重複月份")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Function LoadDetail(id As Integer) As Task
        Try
            Dim data = Await _bpRep.GetByIdAsync(id)
            _view.ClearInput()
            _view.DisplayDetail(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Async Function UpdateAsync() As Task
        Try
            Dim data = _view.GetUserInput

            If Not Validation(data) Then
                MsgBox("請填寫必填項目")
                Return
            End If

            Await _bpRep.UpdateAsync(data.bp_id, data)
            MsgBox("修改成功")
            _view.ClearInput()
            Await SearchAsync(False)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Function Validation(data As basic_price) As Boolean
        If data.bp_c_out = 0 Then Return False
        If data.bp_c_in = 0 Then Return False
        If data.bp_Delivery_C = 0 Then Return False
        If data.bp_Delivery_Normal = 0 Then Return False
        If data.bp_normal_in = 0 Then Return False
        If data.bp_normal_out = 0 Then Return False

        Return True
    End Function
End Class
