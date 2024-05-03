Public Class OrderPresenter
    Private _view As IOrderView
    Private _ordRep As IOrderRepository = New OrderRepository
    Private _cusRep As ICustomerRepository = New CustomerRepository
    Private _carRep As ICarRepository = New CarRepository

    Public Property StockValues As New Dictionary(Of String, Object) '儲存客戶瓦斯桶庫存
    Public Property DepositStockValues As New Dictionary(Of String, Object) '儲存司機寄瓶庫存
    Public Property GasValues As New Dictionary(Of String, Integer) '用於存儲 txtGas_、txtGas_c_ 開頭的 TextBox 的初始值
    Public Property DepositValues As New Dictionary(Of String, Object) '用於存儲TextBox.Tag = o_deposit開頭的初始值

    Public Sub New(view As IOrderView)
        _view = view
    End Sub

    Public Sub LoadList()
        Dim condition = _view.GetQueryCondition
        Dim list = _ordRep.QueryOrders(condition, False)
        _view.ShowList(list.Select(Function(x) New OrderVM(x)).ToList)
    End Sub

    ''' <summary>
    ''' 取得客戶庫存
    ''' </summary>
    ''' <param name="cusId"></param>
    Public Sub GetCusStk(cusId As Integer)
        Dim data = _cusRep.GetCustomerById(cusId)

        If data IsNot Nothing Then
            StockValues = GetEntityFieldsByPrefix(data, "cus_gas")
            _view.ShowCusStk(data)
        End If
    End Sub

    ''' <summary>
    ''' 載入車號選單
    ''' </summary>
    ''' <param name="cusId"></param>
    Public Sub LoadCmbCar(cusId As Integer)
        Dim data = _carRep.GetCmbByCusId(cusId)
        If data IsNot Nothing Then _view.SetCmbCar(data)
    End Sub

    Public Function LoadCar(carId As Integer) As car
        Dim car = _carRep.GetCarById(carId)

        If car IsNot Nothing Then
            DepositStockValues = GetEntityFieldsByPrefix(car, "c_deposit_")
            Return car
        End If

        Return Nothing
    End Function

    Public Sub Add(container As Control)
        Try
            Dim ord As New order
            Dim car As New car
            Dim cus As New customer

            If GetData(ord, car, cus, container) Then
                _ordRep.Add(ord, car, cus)
                _view.Reset()
                LoadList()
                MsgBox("新增成功")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub GetDetail(id As Integer)
        Try
            _view.ShowDetails(_ordRep.GetOrderById(id))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Edit(container As Control)
        Dim ord As New order
        Dim car As New car
        Dim cus As New customer

        Try
            If GetData(ord, car, cus, container) Then
                _ordRep.Edit(ord, car, cus)
                _view.Reset()
                LoadList()
                MsgBox("修改成功")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Delete(orderId As Integer, orderName As String)
        Dim ord = _ordRep.GetOrderById(orderId)

        If ord Is Nothing Then Exit Sub

        Dim carId = ord.o_c_id
        Dim car = _carRep.GetCarById(carId)
        Dim cusId = car.c_cus_id
        Dim cus = _cusRep.GetCustomerById(cusId)

        If orderName = "進場單" Then
            cus.cus_gas_50 -= GetOrderValue(ord, "50", True)
            cus.cus_gas_20 -= GetOrderValue(ord, "20", True)
            cus.cus_gas_16 -= GetOrderValue(ord, "16", True)
            cus.cus_gas_10 -= GetOrderValue(ord, "10", True)
            cus.cus_gas_4 -= GetOrderValue(ord, "4", True)
            cus.cus_gas_15 -= GetOrderValue(ord, "15", True)
            cus.cus_gas_14 -= GetOrderValue(ord, "14", True)
            cus.cus_gas_5 -= GetOrderValue(ord, "5", True)
            cus.cus_gas_2 -= GetOrderValue(ord, "2", True)

            '取得寄瓶庫存
            car.c_deposit_50 -= GetDepositValue(ord, "50", True)
            car.c_deposit_20 -= GetDepositValue(ord, "20", True)
            car.c_deposit_16 -= GetDepositValue(ord, "16", True)
            car.c_deposit_10 -= GetDepositValue(ord, "10", True)
            car.c_deposit_4 -= GetDepositValue(ord, "4", True)
            car.c_deposit_15 -= GetDepositValue(ord, "15", True)
            car.c_deposit_14 -= GetDepositValue(ord, "14", True)
            car.c_deposit_5 -= GetDepositValue(ord, "5", True)
            car.c_deposit_2 -= GetDepositValue(ord, "2", True)
        Else
            cus.cus_gas_50 += GetOrderValue(ord, "50", False)
            cus.cus_gas_20 += GetOrderValue(ord, "20", False)
            cus.cus_gas_16 += GetOrderValue(ord, "16", False)
            cus.cus_gas_10 += GetOrderValue(ord, "10", False)
            cus.cus_gas_4 += GetOrderValue(ord, "4", False)
            cus.cus_gas_15 += GetOrderValue(ord, "15", False)
            cus.cus_gas_14 += GetOrderValue(ord, "14", False)
            cus.cus_gas_5 += GetOrderValue(ord, "5", False)
            cus.cus_gas_2 += GetOrderValue(ord, "2", False)

            '取得寄瓶庫存
            car.c_deposit_50 -= GetDepositValue(ord, "50", False)
            car.c_deposit_20 -= GetDepositValue(ord, "20", False)
            car.c_deposit_16 -= GetDepositValue(ord, "16", False)
            car.c_deposit_10 -= GetDepositValue(ord, "10", False)
            car.c_deposit_4 -= GetDepositValue(ord, "4", False)
            car.c_deposit_15 -= GetDepositValue(ord, "15", False)
            car.c_deposit_14 -= GetDepositValue(ord, "14", False)
            car.c_deposit_5 -= GetDepositValue(ord, "5", False)
            car.c_deposit_2 -= GetDepositValue(ord, "2", False)
        End If

        Try
            _ordRep.Delete(ord.o_id, car, cus)
            _view.Reset()
            LoadList()
            MsgBox("刪除成功")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function GetCusDataByCusCode(cusCode As String) As customer
        Return _cusRep.GetCusByCusCode(cusCode)
    End Function

    Private Function GetData(ByRef ord As order, ByRef car As car, ByRef cus As customer, container As Control) As Boolean
        ord = _view.GetUserInput_ord()
        If ord Is Nothing Then Return False

        car = _carRep.GetCarById(ord.o_c_id)
        _view.GetUserInput_car(car, container)
        cus = _cusRep.GetCustomerById(car.c_cus_id)
        _view.GetUserInput_cus(cus, container)

        Return True
    End Function

    ''' <summary>
    ''' 取得訂單明細
    ''' </summary>
    ''' <param name="ord"></param>
    ''' <param name="group"></param>
    ''' <param name="isIn"></param>
    ''' <returns></returns>
    Private Function GetOrderValue(ord As order, group As String, isIn As Boolean) As Integer
        If isIn Then
            Return ord.GetType.GetProperties.
                Where(Function(p) (p.Name.StartsWith("o_in_") Or p.Name.StartsWith("o_new_in_") Or p.Name.StartsWith("o_inspect_")) And p.Name.EndsWith(group)).
                Sum(Function(x) x.GetValue(ord))
        Else
            Return ord.GetType.GetProperties.
                Where(Function(p) (p.Name.StartsWith("o_gas_") Or p.Name.StartsWith("o_gas_c_") Or p.Name.StartsWith("o_empty_")) And p.Name.EndsWith(group)).
                Sum(Function(x) x.GetValue(ord))
        End If
    End Function

    ''' <summary>
    ''' 取得寄瓶明細
    ''' </summary>
    ''' <param name="ord"></param>
    ''' <param name="group"></param>
    ''' <param name="isIn"></param>
    ''' <returns></returns>
    Private Function GetDepositValue(ord As order, group As String, isIn As Boolean) As Integer
        If isIn Then
            Return ord.GetType.GetProperties.
                Where(Function(p) p.Name.StartsWith("o_deposit_in_") And p.Name.EndsWith(group)).
                Sum(Function(x) x.GetValue(ord))
        Else
            Return ord.GetType.GetProperties.
                Where(Function(p) p.Name.StartsWith("o_deposit_out_") And p.Name.EndsWith(group)).
                Sum(Function(x) x.GetValue(ord))
        End If
    End Function
End Class
