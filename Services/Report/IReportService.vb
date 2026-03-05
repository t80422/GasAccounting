Public Interface IReportService
    Sub GeneratorCashSubpoena(day As Date, datas As List(Of CashSubpoenaDTO), isCollection As Boolean)
    Sub GeneratorTransferSubpoena(request As TransferSubpoenaReportRequest)
End Interface
