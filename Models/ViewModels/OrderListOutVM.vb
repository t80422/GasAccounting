Public Class OrderListOutVM
    Public Property 編號 As Integer
    Public Property 時間 As String
    Public Property 客戶名稱 As String
    Public Property 車號 As String
    Public Property 進出場 As String
    Public Property 出普氣50 As Integer
    Public Property 出普氣20 As Integer
    Public Property 出普氣16 As Integer
    Public Property 出普氣10 As Integer
    Public Property 出普氣4 As Integer
    Public Property 出普氣18 As Integer
    Public Property 出普氣14 As Integer
    Public Property 出普氣5 As Integer
    Public Property 出普氣2 As Integer
    Public Property 出丙氣50 As Integer
    Public Property 出丙氣20 As Integer
    Public Property 出丙氣16 As Integer
    Public Property 出丙氣10 As Integer
    Public Property 出丙氣4 As Integer
    Public Property 出丙氣18 As Integer
    Public Property 出丙氣14 As Integer
    Public Property 出丙氣5 As Integer
    Public Property 出丙氣2 As Integer
    Public Property 退空瓶50 As Integer
    Public Property 退空瓶20 As Integer
    Public Property 退空瓶16 As Integer
    Public Property 退空瓶10 As Integer
    Public Property 退空瓶4 As Integer
    Public Property 退空瓶18 As Integer
    Public Property 退空瓶14 As Integer
    Public Property 退空瓶5 As Integer
    Public Property 退空瓶2 As Integer
    Public Property 出寄桶50 As Integer
    Public Property 出寄桶20 As Integer
    Public Property 出寄桶16 As Integer
    Public Property 出寄桶10 As Integer
    Public Property 出寄桶4 As Integer
    Public Property 出寄桶18 As Integer
    Public Property 出寄桶14 As Integer
    Public Property 出寄桶5 As Integer
    Public Property 出寄桶2 As Integer
    Public Property 寄桶車號 As String
    Public Property 退普氣 As Integer
    Public Property 退丙氣 As Integer
    Public Property 折讓 As Integer
    Public Property 總金額 As Integer
    Public Property 保險金額 As Single
    Public Property 備註 As String
    Public Property 操作人員 As String

    Public Sub New(data As order)
        Try
            編號 = data.o_id
            時間 = data.o_date.Value.ToString("yyyy年MM月dd日 HH:mm")
            客戶名稱 = data.customer.cus_name
            車號 = data.car?.c_no
            進出場 = data.o_in_out
            出普氣50 = data.o_gas_50
            出普氣20 = data.o_gas_20
            出普氣16 = data.o_gas_16
            出普氣10 = data.o_gas_10
            出普氣4 = data.o_gas_4
            出普氣18 = data.o_gas_18
            出普氣14 = data.o_gas_14
            出普氣5 = data.o_gas_5
            出普氣2 = data.o_gas_2
            出丙氣50 = data.o_gas_c_50
            出丙氣20 = data.o_gas_c_20
            出丙氣16 = data.o_gas_c_16
            出丙氣10 = data.o_gas_c_10
            出丙氣4 = data.o_gas_c_4
            出丙氣18 = data.o_gas_c_18
            出丙氣14 = data.o_gas_c_14
            出丙氣5 = data.o_gas_c_5
            出丙氣2 = data.o_gas_c_2
            退空瓶50 = data.o_empty_50
            退空瓶20 = data.o_empty_20
            退空瓶16 = data.o_empty_16
            退空瓶10 = data.o_empty_10
            退空瓶4 = data.o_empty_4
            退空瓶18 = data.o_empty_18
            退空瓶14 = data.o_empty_14
            退空瓶5 = data.o_empty_5
            退空瓶2 = data.o_empty_2
            出寄桶50 = data.o_deposit_out_50
            出寄桶20 = data.o_deposit_out_20
            出寄桶16 = data.o_deposit_out_16
            出寄桶10 = data.o_deposit_out_10
            出寄桶4 = data.o_deposit_out_4
            出寄桶18 = data.o_deposit_out_18
            出寄桶14 = data.o_deposit_out_14
            出寄桶5 = data.o_deposit_out_5
            出寄桶2 = data.o_deposit_out_2
            寄桶車號 = data.car1?.c_no
            退普氣 = data.o_return
            退丙氣 = data.o_return_c
            折讓 = data.o_sales_allowance
            總金額 = data.o_total_amount
            備註 = data.o_memo
            操作人員 = data.employee?.emp_name
            保險金額 = data.o_Insurance
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class
