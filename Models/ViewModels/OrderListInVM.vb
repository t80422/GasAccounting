Public Class OrderListInVM
    Public Property 編號 As Integer
    Public Property 時間 As String
    Public Property 客戶名稱 As String
    Public Property 車號 As String
    Public Property 進出場 As String
    Public Property 收空瓶50 As Integer
    Public Property 收空瓶20 As Integer
    Public Property 收空瓶16 As Integer
    Public Property 收空瓶10 As Integer
    Public Property 收空瓶4 As Integer
    Public Property 收空瓶18 As Integer
    Public Property 收空瓶14 As Integer
    Public Property 收空瓶5 As Integer
    Public Property 收空瓶2 As Integer
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
            收空瓶50 = data.o_in_50
            收空瓶20 = data.o_in_20
            收空瓶16 = data.o_in_16
            收空瓶10 = data.o_in_10
            收空瓶4 = data.o_in_4
            收空瓶18 = data.o_in_18
            收空瓶14 = data.o_in_14
            收空瓶5 = data.o_in_5
            收空瓶2 = data.o_in_2
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
