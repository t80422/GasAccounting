Public Class CarPresenter
    Private ReadOnly _view As ICarView
    Private ReadOnly _cusRep As ICustomerRep
    Private ReadOnly _carRep As ICarRep
    Private _current As car

    Public Sub New(view As ICarView, cusRep As ICustomerRep, carRep As ICarRep)
        _view = view
        _cusRep = cusRep
        _carRep = carRep
    End Sub

    Public Sub LoadCusByCusCode(cusCode As String)
        Try
            Dim cus = _cusRep.GetByCusCode(cusCode)

            If cus Is Nothing Then Throw New Exception("查無此客戶")

            _view.DisplayCustomer(cus.cus_id, cus.cus_code, cus.cus_name)

        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub LoadList()
        Try
            Dim criteria = _view.GetSearchCriteria
            Dim datas = _carRep.Search(criteria)

            _view.DisplayList(datas.Select(Function(x) New CarVM(x)).ToList)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Add()
        Try
            Dim input = _view.GetUserInput
            Validate(input)
            _carRep.AddAsync(input)
            _view.ClearInput()
            LoadList()
            MsgBox("新增成功")
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub LoadDetail(id As Integer)
        Try
            Dim data = Await _carRep.GetByIdAsync(id)
            _current = data
            _view.ClearInput()
            _view.DisplayDetail(data)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Update()
        Try
            Dim update = _view.GetUserInput
            Validate(update)
            _carRep.UpdateAsync(_current, update)
            _view.ClearInput()
            LoadList()
            MsgBox("修改成功")
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Delete()
        Try
            _carRep.DeleteAsync(_current)
            _view.ClearInput()
            LoadList()
            MsgBox("修改成功")
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Validate(entity As car)
        If String.IsNullOrEmpty(entity.c_no) Then Throw New Exception("請輸入車號")
        If entity.c_cus_id = 0 Then Throw New Exception("請輸入客戶")
    End Sub
End Class
