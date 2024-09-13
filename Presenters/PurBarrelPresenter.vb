Public Class PurBarrelPresenter
    Private ReadOnly _view As IPurchaseBarrelView
    Private ReadOnly _pbRep As IPurchaseBarrelRep
    Private ReadOnly _vendorRep As IManufacturerRep
    Private ReadOnly _barmbSer As IBarrelMonthlyBalanceService
    Private ReadOnly _gbRep As IGasBarrelRep

    Public Sub New(view As IPurchaseBarrelView, pbRep As IPurchaseBarrelRep, vendorRep As IManufacturerRep, barmbSer As IBarrelMonthlyBalanceService, gbRep As IGasBarrelRep)
        _view = view
        _pbRep = pbRep
        _vendorRep = vendorRep
        _barmbSer = barmbSer
        _gbRep = gbRep
    End Sub

    Public Async Sub Initialize()
        Await LoadVendor()
        _view.ClearInput()
        LoadList()
    End Sub

    Public Async Sub LoadList()
        Try
            Dim criteria = _view.GetSearchCriteria
            Dim datas = Await _pbRep.SearchAsync(criteria)
            _view.DisplayList(datas.Select(Function(x) New PurchaseBarrelVM(x)).ToList)
            _view.ClearInput()
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Add()
        Using transactions = _pbRep.BeginTransaction
            Try
                Dim input = _view.GetUserInput
                Validate(input)

                Await _pbRep.AddAsync(input)
                Await _barmbSer.UpdateOrAddAsync(input.pb_Date)

                transactions.Commit()
                _view.ClearInput()
                LoadList()
                MsgBox("新增成功")
            Catch ex As Exception
                transactions.Rollback()
                Console.WriteLine(ex.StackTrace)
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub LoadDetail(id As Integer)
        Try
            Dim data = Await _pbRep.GetByIdAsync(id)
            _view.ClearInput()
            _view.DisplayDetail(data)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Update()
        Using transactions = _pbRep.BeginTransaction
            Try
                Dim input = _view.GetUserInput
                Validate(input)

                Await _pbRep.UpdateAsync(input.pb_Id, input)
                Await _barmbSer.UpdateOrAddAsync(input.pb_Date)

                transactions.Commit()
                _view.ClearInput()
                LoadList()
                MsgBox("修改成功")
            Catch ex As Exception
                transactions.Rollback()
                Console.WriteLine(ex.StackTrace)
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub Delete()
        Using transactions = _pbRep.BeginTransaction
            Try
                Dim input = _view.GetUserInput

                Await _pbRep.DeleteAsync(input.pb_Id)
                Await _barmbSer.UpdateOrAddAsync(input.pb_Date)

                transactions.Commit()
                _view.ClearInput()
                LoadList()
                MsgBox("刪除成功")
            Catch ex As Exception
                transactions.Rollback()
                Console.WriteLine(ex.StackTrace)
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub
    Private Sub Validate(input As purchase_barrel)
        If input.pb_manu_Id = 0 Then Throw New Exception("請選擇廠商")
    End Sub

    Private Async Function LoadVendor() As Task
        Try
            Dim datas = Await _vendorRep.GetVendorDropdownAsync
            _view.SetVendorCmb(datas.ToList)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Function
End Class