''' <summary>
''' 轉帳傳票報表產生請求。
''' </summary>
Public Class TransferSubpoenaReportRequest
    ''' <summary>傳票日期</summary>
    Public Property Day As Date
    ''' <summary>轉帳傳票群組資料（每群組對應一筆 payment/collection 的合併側與明細）</summary>
    Public Property Groups As List(Of TransferSubpoenaGroup)
    ''' <summary>傳票類型（收入／支出），決定合併規則</summary>
    Public Property VoucherType As TransferVoucherType
End Class
