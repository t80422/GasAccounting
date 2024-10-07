''' <summary>
''' 客戶提氣量憑單
''' </summary>
Public Class OrderVoucherVM
    Private ReadOnly _本日提量 As Integer

    Public Property 客戶名稱 As String
    Public Property 車號 As String
    Public Property 提氣時間 As Date
    Public Property 提單編號 As Integer
    Public Property 丙氣50kg As Integer
    Public Property 丙氣20kg As Integer
    Public Property 丙氣16kg As Integer
    Public Property 丙氣10kg As Integer
    Public Property 丙氣4kg As Integer
    Public Property 丙氣18kg As Integer
    Public Property 丙氣2kg As Integer
    Public Property 丙氣14kg As Integer
    Public Property 丙氣5kg As Integer

    Public ReadOnly Property 丙氣kg數 As Integer
        Get
            Return (丙氣10kg * 10) + (丙氣14kg * 14) + (丙氣18kg * 18) + (丙氣2kg * 2) + (丙氣16kg * 16) + (丙氣20kg * 20) + (丙氣4kg * 4) + (丙氣50kg * 50) + (丙氣5kg * 5)
        End Get
    End Property

    Public Property 普氣50kg As Integer
    Public Property 普氣20kg As Integer
    Public Property 普氣16kg As Integer
    Public Property 普氣10kg As Integer
    Public Property 普氣4kg As Integer
    Public Property 普氣18kg As Integer
    Public Property 普氣2kg As Integer
    Public Property 普氣14kg As Integer
    Public Property 普氣5kg As Integer

    Public ReadOnly Property 普氣kg數 As Integer
        Get
            Return (普氣10kg * 10) + (普氣14kg * 14) + (普氣18kg * 18) + (普氣2kg * 2) + (普氣16kg * 16) + (普氣20kg * 20) + (普氣4kg * 4) + (普氣50kg * 50) + (普氣5kg * 5)
        End Get
    End Property

    Public Property 檢驗50kg As Integer
    Public Property 檢驗20kg As Integer
    Public Property 檢驗16kg As Integer
    Public Property 檢驗10kg As Integer
    Public Property 檢驗4kg As Integer
    Public Property 檢驗18kg As Integer
    Public Property 檢驗2kg As Integer
    Public Property 檢驗14kg As Integer
    Public Property 檢驗5kg As Integer
    Public Property 新瓶50kg As Integer
    Public Property 新瓶20kg As Integer
    Public Property 新瓶16kg As Integer
    Public Property 新瓶10kg As Integer
    Public Property 新瓶4kg As Integer
    Public Property 新瓶18kg As Integer
    Public Property 新瓶2kg As Integer
    Public Property 新瓶14kg As Integer
    Public Property 新瓶5kg As Integer
    Public Property 退空瓶50kg As Integer
    Public Property 退空瓶20kg As Integer
    Public Property 退空瓶16kg As Integer
    Public Property 退空瓶10kg As Integer
    Public Property 退空瓶4kg As Integer
    Public Property 退空瓶18kg As Integer
    Public Property 退空瓶2kg As Integer
    Public Property 退空瓶14kg As Integer
    Public Property 退空瓶5kg As Integer
    Public Property 結存50kg As Integer
    Public Property 結存20kg As Integer
    Public Property 結存16kg As Integer
    Public Property 結存10kg As Integer
    Public Property 結存4kg As Integer
    Public Property 結存18kg As Integer
    Public Property 結存2kg As Integer
    Public Property 結存14kg As Integer
    Public Property 結存5kg As Integer
    Public Property 收空瓶50kg As Integer
    Public Property 收空瓶20kg As Integer
    Public Property 收空瓶16kg As Integer
    Public Property 收空瓶10kg As Integer
    Public Property 收空瓶4kg As Integer
    Public Property 收空瓶18kg As Integer
    Public Property 收空瓶2kg As Integer
    Public Property 收空瓶14kg As Integer
    Public Property 收空瓶5kg As Integer

    Public ReadOnly Property 本日提量 As Integer
        Get
            Return 丙氣kg數 + 普氣kg數
        End Get
    End Property

    Public Property 本日退氣 As Integer
    Public Property 本月累計實提量 As Integer
    Public Property 本月累計退氣 As Integer
    Public Property 尚欠氣款 As Single
    Public Property 已收氣款 As Single

    Public Sub New(order As order)
        If order Is Nothing Then Throw New ArgumentNullException("order 不能為 nothing")
        If order.car Is Nothing Then Throw New ArgumentNullException("order中的car不能為nothing")
        If order.car.customer Is Nothing Then Throw New ArgumentNullException("order中的car中的customer不能為nothing")

        客戶名稱 = order.car.customer.cus_name
        車號 = order.car.c_no
        提氣時間 = order.o_date
        提單編號 = order.o_id
        丙氣10kg = order.o_gas_c_10
        丙氣14kg = order.o_gas_c_14
        丙氣18kg = order.o_gas_c_18
        丙氣2kg = order.o_gas_c_2
        丙氣16kg = order.o_gas_c_16
        丙氣20kg = order.o_gas_c_20
        丙氣4kg = order.o_gas_c_4
        丙氣50kg = order.o_gas_c_50
        丙氣5kg = order.o_gas_c_5
        普氣10kg = order.o_gas_10
        普氣14kg = order.o_gas_14
        普氣18kg = order.o_gas_18
        普氣2kg = order.o_gas_2
        普氣16kg = order.o_gas_16
        普氣20kg = order.o_gas_20
        普氣4kg = order.o_gas_4
        普氣50kg = order.o_gas_50
        普氣5kg = order.o_gas_5
        檢驗10kg = order.o_inspect_10
        檢驗14kg = order.o_inspect_14
        檢驗18kg = order.o_inspect_18
        檢驗2kg = order.o_inspect_2
        檢驗16kg = order.o_inspect_16
        檢驗20kg = order.o_inspect_20
        檢驗4kg = order.o_inspect_4
        檢驗50kg = order.o_inspect_50
        檢驗5kg = order.o_inspect_5
        新瓶10kg = order.o_new_in_10
        新瓶14kg = order.o_new_in_14
        新瓶18kg = order.o_new_in_18
        新瓶2kg = order.o_new_in_2
        新瓶16kg = order.o_new_in_16
        新瓶20kg = order.o_new_in_20
        新瓶4kg = order.o_new_in_4
        新瓶50kg = order.o_new_in_50
        新瓶5kg = order.o_new_in_5
        收空瓶10kg = order.o_empty_10
        收空瓶14kg = order.o_empty_14
        收空瓶18kg = order.o_empty_18
        收空瓶16kg = order.o_empty_16
        收空瓶20kg = order.o_empty_20
        收空瓶2kg = order.o_empty_2
        收空瓶4kg = order.o_empty_4
        收空瓶50kg = order.o_empty_50
        收空瓶5kg = order.o_empty_5
        結存10kg = order.car.customer.cus_gas_10
        結存14kg = order.car.customer.cus_gas_14
        結存18kg = order.car.customer.cus_gas_18
        結存16kg = order.car.customer.cus_gas_16
        結存20kg = order.car.customer.cus_gas_20
        結存2kg = order.car.customer.cus_gas_2
        結存4kg = order.car.customer.cus_gas_4
        結存50kg = order.car.customer.cus_gas_50
        結存5kg = order.car.customer.cus_gas_5
        本日退氣 = order.o_return + order.o_return_c
    End Sub
End Class
