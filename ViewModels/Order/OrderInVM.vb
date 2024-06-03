Public Class OrderInVM
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
        退普氣 = data.o_return
        退丙氣 = data.o_return_c
        折讓 = data.o_sales_allowance
        總金額 = data.o_total_amount
        備註 = data.o_memo
        操作人員 = data.employee.emp_name
    End Sub
End Class
