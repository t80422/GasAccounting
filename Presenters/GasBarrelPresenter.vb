Public Class GasBarrelPresenter
    Private ReadOnly _view As IGasBarrelView
    Private ReadOnly _gbRep As IGasBarrelRep

    Public Sub New(view As IGasBarrelView, gbRep As IGasBarrelRep)
        _view = view
        _gbRep = gbRep
    End Sub

    Public Async Sub LoadList()
        Try
            Dim datas = Await _gbRep.GetAllAsync
            _view.ClearInput()
            _view.DisplayList(datas.Select(Function(x) New GasBarrelVM(x)).ToList)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub LoadDetail(id As Integer)
        Try
            Dim data = Await _gbRep.GetByIdAsync(id)
            _view.DisplayDetail(data)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Update()
        Try
            Dim input = _view.GetUserInput()
            Dim id = input.gb_Id
            Dim data = Await _gbRep.GetByIdAsync(id)

            If data.gb_InitialInventory <> input.gb_InitialInventory Then
                Dim diff = input.gb_InitialInventory - data.gb_InitialInventory
                input.gb_Inventory += diff
            End If

            Await _gbRep.UpdateAsync(id, input)
            _view.ClearInput()
            LoadList()
            MsgBox("修改成功")
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub
End Class