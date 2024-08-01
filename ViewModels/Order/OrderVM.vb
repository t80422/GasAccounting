Public Class OrderVM
    Public Property 編號 As Integer
    Public Property 時間 As Date
    Public Property 客戶名稱 As String
    Public Property 車號 As String
    Public Property 進出場 As String
    Public Property 正常桶進50 As Integer
    Public Property 正常桶進20 As Integer
    Public Property 正常桶進16 As Integer
    Public Property 正常桶進10 As Integer
    Public Property 正常桶進4 As Integer
    Public Property 正常桶進15 As Integer
    Public Property 正常桶進14 As Integer
    Public Property 正常桶進5 As Integer
    Public Property 正常桶進2 As Integer
    Public Property 新桶進50 As Integer
    Public Property 新桶進20 As Integer
    Public Property 新桶進16 As Integer
    Public Property 新桶進10 As Integer
    Public Property 新桶進4 As Integer
    Public Property 新桶進15 As Integer
    Public Property 新桶進14 As Integer
    Public Property 新桶進5 As Integer
    Public Property 新桶進2 As Integer
    Public Property 檢驗桶進50 As Integer
    Public Property 檢驗桶進20 As Integer
    Public Property 檢驗桶進16 As Integer
    Public Property 檢驗桶進10 As Integer
    Public Property 檢驗桶進4 As Integer
    Public Property 檢驗桶進15 As Integer
    Public Property 檢驗桶進14 As Integer
    Public Property 檢驗桶進5 As Integer
    Public Property 檢驗桶進2 As Integer
    Public Property 寄桶進50 As Integer
    Public Property 寄桶進20 As Integer
    Public Property 寄桶進16 As Integer
    Public Property 寄桶進10 As Integer
    Public Property 寄桶進4 As Integer
    Public Property 寄桶進15 As Integer
    Public Property 寄桶進14 As Integer
    Public Property 寄桶進5 As Integer
    Public Property 寄桶進2 As Integer
    Public Property 普氣50 As Integer
    Public Property 普氣20 As Integer
    Public Property 普氣16 As Integer
    Public Property 普氣10 As Integer
    Public Property 普氣4 As Integer
    Public Property 普氣15 As Integer
    Public Property 普氣14 As Integer
    Public Property 普氣5 As Integer
    Public Property 普氣2 As Integer
    Public Property 丙氣50 As Integer
    Public Property 丙氣20 As Integer
    Public Property 丙氣16 As Integer
    Public Property 丙氣10 As Integer
    Public Property 丙氣4 As Integer
    Public Property 丙氣15 As Integer
    Public Property 丙氣14 As Integer
    Public Property 丙氣5 As Integer
    Public Property 丙氣2 As Integer
    Public Property 空瓶出場50 As Integer
    Public Property 空瓶出場20 As Integer
    Public Property 空瓶出場16 As Integer
    Public Property 空瓶出場10 As Integer
    Public Property 空瓶出場4 As Integer
    Public Property 空瓶出場15 As Integer
    Public Property 空瓶出場14 As Integer
    Public Property 空瓶出場5 As Integer
    Public Property 空瓶出場2 As Integer
    Public Property 寄桶出50 As Integer
    Public Property 寄桶出20 As Integer
    Public Property 寄桶出16 As Integer
    Public Property 寄桶出10 As Integer
    Public Property 寄桶出4 As Integer
    Public Property 寄桶出15 As Integer
    Public Property 寄桶出14 As Integer
    Public Property 寄桶出5 As Integer
    Public Property 寄桶出2 As Integer
    Public Property 寄桶車號 As String
    Public Property 退普氣 As Integer
    Public Property 退丙氣 As Integer
    Public Property 折讓 As Integer
    Public Property 總金額 As Integer
    Public Property 備註 As String
    Public Property 操作人員 As String

    Public Sub New(data As order)
        編號 = data.o_id
        時間 = data.o_date
        客戶名稱 = data.car.customer.cus_name
        車號 = data.car.c_no
        進出場 = data.o_in_out
        正常桶進50 = data.o_in_50
        正常桶進20 = data.o_in_20
        正常桶進16 = data.o_in_16
        正常桶進10 = data.o_in_10
        正常桶進4 = data.o_in_4
        正常桶進15 = data.o_in_15
        正常桶進14 = data.o_in_14
        正常桶進5 = data.o_in_5
        正常桶進2 = data.o_in_2
        新桶進50 = data.o_new_in_50
        新桶進20 = data.o_new_in_20
        新桶進16 = data.o_new_in_16
        新桶進10 = data.o_new_in_10
        新桶進4 = data.o_new_in_4
        新桶進15 = data.o_new_in_15
        新桶進14 = data.o_new_in_14
        新桶進5 = data.o_new_in_5
        新桶進2 = data.o_new_in_2
        檢驗桶進50 = data.o_inspect_50
        檢驗桶進20 = data.o_inspect_20
        檢驗桶進16 = data.o_inspect_16
        檢驗桶進10 = data.o_inspect_10
        檢驗桶進4 = data.o_inspect_4
        檢驗桶進15 = data.o_inspect_15
        檢驗桶進14 = data.o_inspect_14
        檢驗桶進5 = data.o_inspect_5
        檢驗桶進2 = data.o_inspect_2
        寄桶進50 = data.o_deposit_in_50
        寄桶進20 = data.o_deposit_in_20
        寄桶進16 = data.o_deposit_in_16
        寄桶進10 = data.o_deposit_in_10
        寄桶進4 = data.o_deposit_in_4
        寄桶進15 = data.o_deposit_in_15
        寄桶進14 = data.o_deposit_in_14
        寄桶進5 = data.o_deposit_in_5
        寄桶進2 = data.o_deposit_in_2
        普氣50 = data.o_gas_50
        普氣20 = data.o_gas_20
        普氣16 = data.o_gas_16
        普氣10 = data.o_gas_10
        普氣4 = data.o_gas_4
        普氣15 = data.o_gas_15
        普氣14 = data.o_gas_14
        普氣5 = data.o_gas_5
        普氣2 = data.o_gas_2
        丙氣50 = data.o_gas_c_50
        丙氣20 = data.o_gas_c_20
        丙氣16 = data.o_gas_c_16
        丙氣10 = data.o_gas_c_10
        丙氣4 = data.o_gas_c_4
        丙氣15 = data.o_gas_c_15
        丙氣14 = data.o_gas_c_14
        丙氣5 = data.o_gas_c_5
        丙氣2 = data.o_gas_c_2
        空瓶出場50 = data.o_empty_50
        空瓶出場20 = data.o_empty_20
        空瓶出場16 = data.o_empty_16
        空瓶出場10 = data.o_empty_10
        空瓶出場4 = data.o_empty_4
        空瓶出場15 = data.o_empty_15
        空瓶出場14 = data.o_empty_14
        空瓶出場5 = data.o_empty_5
        空瓶出場2 = data.o_empty_2
        寄桶出50 = data.o_deposit_out_50
        寄桶出20 = data.o_deposit_out_20
        寄桶出16 = data.o_deposit_out_16
        寄桶出10 = data.o_deposit_out_10
        寄桶出4 = data.o_deposit_out_4
        寄桶出15 = data.o_deposit_out_15
        寄桶出14 = data.o_deposit_out_14
        寄桶出5 = data.o_deposit_out_5
        寄桶出2 = data.o_deposit_out_2
        寄桶車號 = data.car1?.c_no
        退普氣 = data.o_return
        退丙氣 = data.o_return_c
        折讓 = data.o_sales_allowance
        總金額 = data.o_total_amount
        備註 = data.o_memo
        操作人員 = data.employee?.emp_name
    End Sub
End Class