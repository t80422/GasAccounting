Public Class OrderMV
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

    Public Shared Function GetOrderList(data As IEnumerable(Of order)) As List(Of OrderMV)
        Dim list = data.Select(Function(x) New OrderMV With {
            .編號 = x.o_id,
            .時間 = x.o_date,
            .客戶名稱 = x.car.customer.cus_name,
            .車號 = x.car.c_no,
            .進出場 = x.o_in_out,
            .正常桶進50 = x.o_in_50,
            .正常桶進20 = x.o_in_20,
            .正常桶進16 = x.o_in_16,
            .正常桶進10 = x.o_in_10,
            .正常桶進4 = x.o_in_4,
            .正常桶進15 = x.o_in_15,
            .正常桶進14 = x.o_in_14,
            .正常桶進5 = x.o_in_5,
            .正常桶進2 = x.o_in_2,
            .新桶進50 = x.o_new_in_50,
            .新桶進20 = x.o_new_in_20,
            .新桶進16 = x.o_new_in_16,
            .新桶進10 = x.o_new_in_10,
            .新桶進4 = x.o_new_in_4,
            .新桶進15 = x.o_new_in_15,
            .新桶進14 = x.o_new_in_14,
            .新桶進5 = x.o_new_in_5,
            .新桶進2 = x.o_new_in_2,
            .檢驗桶進50 = x.o_inspect_50,
            .檢驗桶進20 = x.o_inspect_20,
            .檢驗桶進16 = x.o_inspect_16,
            .檢驗桶進10 = x.o_inspect_10,
            .檢驗桶進4 = x.o_inspect_4,
            .檢驗桶進15 = x.o_inspect_15,
            .檢驗桶進14 = x.o_inspect_14,
            .檢驗桶進5 = x.o_inspect_5,
            .檢驗桶進2 = x.o_inspect_2,
            .寄桶進50 = x.o_deposit_in_50,
            .寄桶進20 = x.o_deposit_in_20,
            .寄桶進16 = x.o_deposit_in_16,
            .寄桶進10 = x.o_deposit_in_10,
            .寄桶進4 = x.o_deposit_in_4,
            .寄桶進15 = x.o_deposit_in_15,
            .寄桶進14 = x.o_deposit_in_14,
            .寄桶進5 = x.o_deposit_in_5,
            .寄桶進2 = x.o_deposit_in_2,
            .普氣50 = x.o_gas_50,
            .普氣20 = x.o_gas_20,
            .普氣16 = x.o_gas_16,
            .普氣10 = x.o_gas_10,
            .普氣4 = x.o_gas_4,
            .普氣15 = x.o_gas_15,
            .普氣14 = x.o_gas_14,
            .普氣5 = x.o_gas_5,
            .普氣2 = x.o_gas_2,
            .丙氣50 = x.o_gas_c_50,
            .丙氣20 = x.o_gas_c_20,
            .丙氣16 = x.o_gas_c_16,
            .丙氣10 = x.o_gas_c_10,
            .丙氣4 = x.o_gas_c_4,
            .丙氣15 = x.o_gas_c_15,
            .丙氣14 = x.o_gas_c_14,
            .丙氣5 = x.o_gas_c_5,
            .丙氣2 = x.o_gas_c_2,
            .空瓶出場50 = x.o_empty_50,
            .空瓶出場20 = x.o_empty_20,
            .空瓶出場16 = x.o_empty_16,
            .空瓶出場10 = x.o_empty_10,
            .空瓶出場4 = x.o_empty_4,
            .空瓶出場15 = x.o_empty_15,
            .空瓶出場14 = x.o_empty_14,
            .空瓶出場5 = x.o_empty_5,
            .空瓶出場2 = x.o_empty_2,
            .寄桶出50 = x.o_deposit_out_50,
            .寄桶出20 = x.o_deposit_out_20,
            .寄桶出16 = x.o_deposit_out_16,
            .寄桶出10 = x.o_deposit_out_10,
            .寄桶出4 = x.o_deposit_out_4,
            .寄桶出15 = x.o_deposit_out_15,
            .寄桶出14 = x.o_deposit_out_14,
            .寄桶出5 = x.o_deposit_out_5,
            .寄桶出2 = x.o_deposit_out_2,
            .寄桶車號 = x.car1.c_no,
            .退普氣 = x.o_return,
            .退丙氣 = x.o_return_c,
            .折讓 = x.o_sales_allowance,
            .總金額 = x.o_total_amount,
            .備註 = x.o_memo
        }).ToList

        Return list
    End Function
End Class
