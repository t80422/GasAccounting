Imports GasAccounting
Imports Moq

<TestClass>
Public Class TestReportService
    Private _mockContext As New Mock(Of gas_accounting_systemEntities)
    Private _reportService As IReportService

    <TestInitialize>
    Public Sub Setup()
        _reportService = New ReportService()
    End Sub

    <TestMethod("轉帳收入傳票-一借多貸，借方要合併儲存格")>
    Public Sub GeneratorTransferSubpoena_MergeCell()
        ' Arrange
        Dim datas As New List(Of TransferSubpoenaDTO) From {
            New TransferSubpoenaDTO With {
                .DebitSubjectName = "測試借方科目1",
                .DebitAmount = 100,
                .DebitSummary = "正常借方摘要",
                .CreditSubjectName = "測試貸方科目1",
                .CreditAmount = 100,
                .CreditSummary = "正常貸方摘要",
                .Id = 1
            },
            New TransferSubpoenaDTO With {
                .DebitSubjectName = "測試借方科目2",
                .DebitAmount = 300,
                .DebitSummary = "合併借方摘要",
                .CreditSubjectName = "測試貸方科目2",
                .CreditAmount = 100,
                .CreditSummary = "合併貸方摘要",
                .Id = 2
            },
            New TransferSubpoenaDTO With {
                .DebitSubjectName = "測試借方科目2",
                .DebitAmount = 300,
                .DebitSummary = "合併借方摘要",
                .CreditSubjectName = "測試貸方科目3",
                .CreditAmount = 200,
                .CreditSummary = "合併貸方摘要",
                .Id = 2
            }
        }

        ' Act
        _reportService.GeneratorTransferSubpoena(Date.Now, datas, True)
    End Sub
End Class
