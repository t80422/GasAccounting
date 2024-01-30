Public Class BasicPriceMV
    Public Property 編號 As Integer
    Public Property 年月份 As String
    Public Property 普氣進氣價格 As Double
    Public Property 丙氣進氣價格 As Double
    Public Property 普氣銷售價格 As Double
    Public Property 丙氣銷售價格 As Double

    Public Shared Function GetBPList(data As IEnumerable(Of basic_price)) As List(Of BasicPriceMV)
        'Dim list = data.ToList
        Dim carList = data.Select(Function(bp) New BasicPriceMV With {
            .編號 = bp.bp_id,
            .年月份 = bp.bp_date.ToString("yyyy/MM"),
            .普氣進氣價格 = bp.bp_normal_in,
            .丙氣進氣價格 = bp.bp_c_in,
            .普氣銷售價格 = bp.bp_normal_out,
            .丙氣銷售價格 = bp.bp_c_out
        })
        Return carList.ToList
    End Function
End Class
