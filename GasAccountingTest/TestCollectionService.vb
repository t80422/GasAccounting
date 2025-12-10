Imports GasAccounting
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Moq

<TestClass>
Public Class TestCollectionService
    Private _mockUowFactory As Mock(Of IUnitOfWorkFactory)
    Private _mockUow As Mock(Of IUnitOfWork)
    Private _mockCollectionRep As Mock(Of ICollectionRep)
    Private _mockChequeRep As Mock(Of IChequeRep)
    Private _mockSubjectRep As Mock(Of ISubjectRep)
    Private _mockBmbRep As Mock(Of IBankMonthlyBalancesRep)
    Private _mockBankRep As Mock(Of IBankRep)
    Private _mockBmbService As Mock(Of IBankMonthlyBalanceService)
    Private _mockAeService As Mock(Of IAccountingEntryService)
    Private _mockOcmService As Mock(Of IOrderCollectionMappingService)

    <TestInitialize>
    Public Sub Setup()
        _mockUowFactory = New Mock(Of IUnitOfWorkFactory)
        _mockUow = New Mock(Of IUnitOfWork)
        _mockCollectionRep = New Mock(Of ICollectionRep)
        _mockChequeRep = New Mock(Of IChequeRep)
        _mockSubjectRep = New Mock(Of ISubjectRep)
        _mockBmbRep = New Mock(Of IBankMonthlyBalancesRep)
        _mockBankRep = New Mock(Of IBankRep)
        _mockBmbService = New Mock(Of IBankMonthlyBalanceService)(MockBehavior.Strict)
        _mockAeService = New Mock(Of IAccountingEntryService)(MockBehavior.Strict)
        _mockOcmService = New Mock(Of IOrderCollectionMappingService)(MockBehavior.Strict)

        _mockUowFactory.Setup(Function(x) x.Create()).Returns(_mockUow.Object)

        _mockUow.SetupGet(Function(x) x.CollectionRepository).Returns(_mockCollectionRep.Object)
        _mockUow.SetupGet(Function(x) x.ChequeRepository).Returns(_mockChequeRep.Object)
        _mockUow.SetupGet(Function(x) x.SubjectRepository).Returns(_mockSubjectRep.Object)
        _mockUow.SetupGet(Function(x) x.BankMonthlyBalancesRepository).Returns(_mockBmbRep.Object)
        _mockUow.SetupGet(Function(x) x.BankRepository).Returns(_mockBankRep.Object)

        _mockUow.Setup(Sub(x) x.BeginTransaction())
        _mockUow.Setup(Sub(x) x.Commit())
        _mockUow.Setup(Sub(x) x.Rollback())
        _mockUow.Setup(Function(x) x.SaveChangesAsync()).ReturnsAsync(1)
    End Sub

    Private Function CreateService() As CollectionService
        Return New CollectionService(
            _mockUowFactory.Object,
            _mockBmbService.Object,
            _mockAeService.Object,
            _mockOcmService.Object
        )
    End Function

    <TestMethod("刪除-銀行存款類型且科目為銀行：應呼叫增量回沖兩次")>
    Public Async Function Delete_BankTypeAndSubject_ShouldCallBmbTwice() As Task
        ' Arrange
        Dim orgCol = New collection With {
            .col_Id = 1,
            .col_Type = "銀行存款",
            .col_bank_Id = 1,
            .col_AccountMonth = New Date(2024, 5, 1),
            .col_Amount = 100D,
            .col_s_Id = 10,
            .col_Cheque = "CHK001"
        }

        Dim subject = New subject With {.s_id = 10, .s_name = "銀行存款"}
        Dim cheque = New cheque With {.che_Id = 5}

        _mockCollectionRep.Setup(Function(r) r.GetByIdAsync(orgCol.col_Id)).ReturnsAsync(orgCol)
        _mockSubjectRep.Setup(Function(r) r.GetByIdAsync(orgCol.col_s_Id)).ReturnsAsync(subject)
        _mockChequeRep.Setup(Function(r) r.GetByNumberAsync(orgCol.col_Cheque)).ReturnsAsync(cheque)
        _mockChequeRep.Setup(Function(r) r.DeleteAsync(cheque.che_Id)).Returns(Task.CompletedTask)

        _mockBmbService.Setup(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                _mockBmbRep.Object,
                _mockBankRep.Object,
                orgCol.col_bank_Id,
                orgCol.col_AccountMonth,
                0,
                -orgCol.col_Amount)
        ).Returns(Task.CompletedTask).Verifiable()

        _mockOcmService.Setup(Sub(s) s.DeleteCollection(orgCol.col_Id))
        _mockAeService.Setup(Sub(s) s.DeleteEntries("收款作業", orgCol.col_Id))
        _mockCollectionRep.Setup(Function(r) r.DeleteAsync(orgCol)).Returns(Task.CompletedTask)

        Dim service = CreateService()

        ' Act
        Await service.DeleteAsync(orgCol.col_Id)

        ' Assert
        _mockBmbService.Verify(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                _mockBmbRep.Object,
                _mockBankRep.Object,
                orgCol.col_bank_Id,
                orgCol.col_AccountMonth,
                0,
                -orgCol.col_Amount),
            Times.Exactly(2))
    End Function

    <TestMethod("刪除-現金但科目為銀行：應呼叫增量回沖一次")>
    Public Async Function Delete_CashTypeButBankSubject_ShouldCallBmbOnce() As Task
        ' Arrange
        Dim orgCol = New collection With {
            .col_Id = 2,
            .col_Type = "現金",
            .col_bank_Id = 2,
            .col_AccountMonth = New Date(2024, 6, 1),
            .col_Amount = 250D,
            .col_s_Id = 20
        }

        Dim subject = New subject With {.s_id = 20, .s_name = "銀行存款"}

        _mockCollectionRep.Setup(Function(r) r.GetByIdAsync(orgCol.col_Id)).ReturnsAsync(orgCol)
        _mockSubjectRep.Setup(Function(r) r.GetByIdAsync(orgCol.col_s_Id)).ReturnsAsync(subject)

        _mockBmbService.Setup(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                _mockBmbRep.Object,
                _mockBankRep.Object,
                orgCol.col_bank_Id,
                orgCol.col_AccountMonth,
                0,
                -orgCol.col_Amount)
        ).Returns(Task.CompletedTask).Verifiable()

        _mockOcmService.Setup(Sub(s) s.DeleteCollection(orgCol.col_Id))
        _mockAeService.Setup(Sub(s) s.DeleteEntries("收款作業", orgCol.col_Id))
        _mockCollectionRep.Setup(Function(r) r.DeleteAsync(orgCol)).Returns(Task.CompletedTask)

        Dim service = CreateService()

        ' Act
        Await service.DeleteAsync(orgCol.col_Id)

        ' Assert
        _mockBmbService.Verify(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                _mockBmbRep.Object,
                _mockBankRep.Object,
                orgCol.col_bank_Id,
                orgCol.col_AccountMonth,
                0,
                -orgCol.col_Amount),
            Times.Once)
    End Function

    <TestMethod("新增-借方現金貸方銀行存款：應記入銀行貸方")>
    Public Async Function Add_CashType_BankSubject_ShouldCreditBank() As Task
        ' Arrange
        Dim input = New collection With {
            .col_Id = 3,
            .col_Type = "現金",
            .col_bank_Id = 2,
            .col_AccountMonth = New Date(2024, 7, 1),
            .col_Amount = 800D,
            .col_s_Id = 30
        }

        Dim subject = New subject With {.s_id = 30, .s_name = "銀行存款"}

        _mockCollectionRep.Setup(Function(r) r.AddAsync(input)).ReturnsAsync(input)
        _mockSubjectRep.Setup(Function(r) r.GetByIdAsync(input.col_s_Id)).ReturnsAsync(subject)

        _mockBmbService.Setup(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                _mockBmbRep.Object,
                _mockBankRep.Object,
                input.col_bank_Id,
                input.col_AccountMonth,
                creditDelta:=input.col_Amount,
                debitDelta:=0)
        ).Returns(Task.CompletedTask).Verifiable()

        Dim service = CreateService()

        ' Act
        Await service.AddAsync(input)

        ' Assert
        _mockBmbService.Verify()
    End Function
End Class

