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

    <TestMethod("轉帳收入傳票-一借多貸，借方 A/B/C 合併儲存格")>
    Public Sub GeneratorTransferSubpoena_Income_MergeDebitCells()
        ' Arrange：一借多貸，一組兩筆貸方明細，借方合併
        Dim groups As New List(Of TransferSubpoenaGroup) From {
            New TransferSubpoenaGroup With {
                .SubjectName = "測試借方科目1",
                .Summary = "正常借方摘要",
                .Amount = 100,
                .Details = New List(Of TransferSubpoenaDetail) From {
                    New TransferSubpoenaDetail With {.SubjectName = "測試貸方科目1", .Summary = "正常貸方摘要", .Amount = 100}
                }
            },
            New TransferSubpoenaGroup With {
                .SubjectName = "測試借方科目2",
                .Summary = "合併借方摘要",
                .Amount = 300,
                .Details = New List(Of TransferSubpoenaDetail) From {
                    New TransferSubpoenaDetail With {.SubjectName = "測試貸方科目2", .Summary = "合併貸方摘要", .Amount = 100},
                    New TransferSubpoenaDetail With {.SubjectName = "測試貸方科目3", .Summary = "合併貸方摘要", .Amount = 200}
                }
            }
        }

        ' Act
        _reportService.GeneratorTransferSubpoena(New TransferSubpoenaReportRequest With {
            .Day = Date.Now,
            .Groups = groups,
            .VoucherType = TransferVoucherType.Income
        })
    End Sub

    <TestMethod("轉帳支出傳票-一貸多借，貸方 D/E/F 合併儲存格")>
    Public Sub GeneratorTransferSubpoena_Expense_MergeCreditCells()
        ' Arrange：一貸多借，一組兩筆借方明細，貸方合併
        Dim groups As New List(Of TransferSubpoenaGroup) From {
            New TransferSubpoenaGroup With {
                .SubjectName = "銀行存款",
                .Summary = "合併貸方",
                .Amount = 300,
                .Details = New List(Of TransferSubpoenaDetail) From {
                    New TransferSubpoenaDetail With {.SubjectName = "借方科目A", .Summary = "摘要A", .Amount = 100},
                    New TransferSubpoenaDetail With {.SubjectName = "借方科目B", .Summary = "摘要B", .Amount = 200}
                }
            }
        }

        ' Act
        _reportService.GeneratorTransferSubpoena(New TransferSubpoenaReportRequest With {
            .Day = Date.Now,
            .Groups = groups,
            .VoucherType = TransferVoucherType.Expense
        })
    End Sub
End Class
