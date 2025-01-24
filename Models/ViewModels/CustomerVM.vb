Public Class CustomerVM
    Public Property 編號 As Integer
    Public Property 代號 As String
    Public Property 名稱 As String
    Public Property 負責人 As String
    Public Property 聯絡人 As String
    Public Property 電話1 As String
    Public Property 電話2 As String
    Public Property 傳真 As String
    Public Property 地址 As String
    Public Property 統編 As String
    Public Property 所屬公司 As String
    Public Property 備註 As String
    Public Property 保險 As String
    Public Property 價格方案 As String
    Public Property 瓦斯桶50Kg As Integer
    Public Property 瓦斯桶20Kg As Integer
    Public Property 瓦斯桶16Kg As Integer
    Public Property 瓦斯桶10Kg As Integer
    Public Property 瓦斯桶4Kg As Integer
    Public Property 瓦斯桶18Kg As Integer
    Public Property 瓦斯桶14Kg As Integer
    Public Property 瓦斯桶5Kg As Integer
    Public Property 瓦斯桶2Kg As Integer
    Public Property 普氣存氣 As Integer
    Public Property 丙氣存氣 As Integer

    Public Sub New(data As customer)
        編號 = data.cus_id
        代號 = data.cus_code
        名稱 = data.cus_name
        負責人 = data.cus_principal
        聯絡人 = data.cus_contact_person
        電話1 = data.cus_phone1
        電話2 = data.cus_phone2
        傳真 = data.cus_fax
        地址 = data.cus_address
        統編 = data.cus_tax_id
        備註 = data.cus_memo
        保險 = data.cus_IsInsurance
        價格方案 = data.priceplan?.pp_Name
        瓦斯桶50Kg = data.cus_gas_50
        瓦斯桶20Kg = data.cus_gas_20
        瓦斯桶16Kg = data.cus_gas_16
        瓦斯桶10Kg = data.cus_gas_10
        瓦斯桶4Kg = data.cus_gas_4
        瓦斯桶18Kg = data.cus_gas_18
        瓦斯桶14Kg =data.cus_gas_14
        瓦斯桶5Kg = data.cus_gas_5
        瓦斯桶2Kg = data.cus_gas_2
        普氣存氣 = data.cus_GasStock
        丙氣存氣 = data.cus_GasCStock
        所屬公司 = data.company?.comp_name
    End Sub
End Class
