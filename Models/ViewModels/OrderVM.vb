Public Class OrderVM
    Public Property 編號 As Integer
    Public Property 時間 As Date
    Public Property 客戶名稱 As String
    Public Property 車號 As String
    Public Property 進出場 As String
    Public Property 進空瓶50 As Integer
    Public Property 進空瓶20 As Integer
    Public Property 進空瓶16 As Integer
    Public Property 進空瓶10 As Integer
    Public Property 進空瓶4 As Integer
    Public Property 進空瓶18 As Integer
    Public Property 進空瓶14 As Integer
    Public Property 進空瓶5 As Integer
    Public Property 進空瓶2 As Integer
    Public Property 進新瓶50 As Integer
    Public Property 進新瓶20 As Integer
    Public Property 進新瓶16 As Integer
    Public Property 進新瓶10 As Integer
    Public Property 進新瓶4 As Integer
    Public Property 進新瓶18 As Integer
    Public Property 進新瓶14 As Integer
    Public Property 進新瓶5 As Integer
    Public Property 進新瓶2 As Integer
    Public Property 進檢驗50 As Integer
    Public Property 進檢驗20 As Integer
    Public Property 進檢驗16 As Integer
    Public Property 進檢驗10 As Integer
    Public Property 進檢驗4 As Integer
    Public Property 進檢驗18 As Integer
    Public Property 進檢驗14 As Integer
    Public Property 進檢驗5 As Integer
    Public Property 進檢驗2 As Integer
    Public Property 進寄桶50 As Integer
    Public Property 進寄桶20 As Integer
    Public Property 進寄桶16 As Integer
    Public Property 進寄桶10 As Integer
    Public Property 進寄桶4 As Integer
    Public Property 進寄桶18 As Integer
    Public Property 進寄桶14 As Integer
    Public Property 進寄桶5 As Integer
    Public Property 進寄桶2 As Integer
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
    Public Property 出空瓶50 As Integer
    Public Property 出空瓶20 As Integer
    Public Property 出空瓶16 As Integer
    Public Property 出空瓶10 As Integer
    Public Property 出空瓶4 As Integer
    Public Property 出空瓶18 As Integer
    Public Property 出空瓶14 As Integer
    Public Property 出空瓶5 As Integer
    Public Property 出空瓶2 As Integer
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
            時間 = data.o_date
            客戶名稱 = data.customer.cus_name
            車號 = data.car?.c_no
            進出場 = data.o_in_out
            進空瓶50 = data.o_in_50
            進空瓶20 = data.o_in_20
            進空瓶16 = data.o_in_16
            進空瓶10 = data.o_in_10
            進空瓶4 = data.o_in_4
            進空瓶18 = data.o_in_18
            進空瓶14 = data.o_in_14
            進空瓶5 = data.o_in_5
            進空瓶2 = data.o_in_2
            進新瓶50 = data.o_new_in_50
            進新瓶20 = data.o_new_in_20
            進新瓶16 = data.o_new_in_16
            進新瓶10 = data.o_new_in_10
            進新瓶4 = data.o_new_in_4
            進新瓶18 = data.o_new_in_18
            進新瓶14 = data.o_new_in_14
            進新瓶5 = data.o_new_in_5
            進新瓶2 = data.o_new_in_2
            進檢驗50 = data.o_inspect_50
            進檢驗20 = data.o_inspect_20
            進檢驗16 = data.o_inspect_16
            進檢驗10 = data.o_inspect_10
            進檢驗4 = data.o_inspect_4
            進檢驗18 = data.o_inspect_18
            進檢驗14 = data.o_inspect_14
            進檢驗5 = data.o_inspect_5
            進檢驗2 = data.o_inspect_2
            進寄桶50 = data.o_deposit_in_50
            進寄桶20 = data.o_deposit_in_20
            進寄桶16 = data.o_deposit_in_16
            進寄桶10 = data.o_deposit_in_10
            進寄桶4 = data.o_deposit_in_4
            進寄桶18 = data.o_deposit_in_18
            進寄桶14 = data.o_deposit_in_14
            進寄桶5 = data.o_deposit_in_5
            進寄桶2 = data.o_deposit_in_2
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
            出空瓶50 = data.o_empty_50
            出空瓶20 = data.o_empty_20
            出空瓶16 = data.o_empty_16
            出空瓶10 = data.o_empty_10
            出空瓶4 = data.o_empty_4
            出空瓶18 = data.o_empty_18
            出空瓶14 = data.o_empty_14
            出空瓶5 = data.o_empty_5
            出空瓶2 = data.o_empty_2
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
            Console.WriteLine(ex.StackTrace)
            Throw
        End Try
    End Sub
End Class