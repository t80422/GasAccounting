Public Interface IReportService
    Sub GeneratorCashSubpoena(day As Date, datas As List(Of CashSubpoenaDTO), isCollection As Boolean)
    Sub GeneratorTransferSubpoena(day As Date, datas As List(Of TransferSubpoenaDTO), isCollection As Boolean)
End Interface
