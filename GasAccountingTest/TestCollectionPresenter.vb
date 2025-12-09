Imports GasAccounting
Imports Moq

<TestClass>
Public Class TestCollectionPresenter
    Private _mockBmbService As Mock(Of IBankMonthlyBalanceService)
    Private _mockAeService As Mock(Of IAccountingEntryService)
    Private _mockOcmService As Mock(Of IOrderCollectionMappingService)
    Private _mockReportService As Mock(Of IReportService)
    Private _mockCollectionService As Mock(Of ICollectionService)

    <TestInitialize>
    Public Sub Setup()
        _mockBmbService = New Mock(Of IBankMonthlyBalanceService)(MockBehavior.Strict)
        _mockAeService = New Mock(Of IAccountingEntryService)(MockBehavior.Strict)
        _mockOcmService = New Mock(Of IOrderCollectionMappingService)(MockBehavior.Strict)
        _mockReportService = New Mock(Of IReportService)(MockBehavior.Strict)
        _mockCollectionService = New Mock(Of ICollectionService)(MockBehavior.Strict)
    End Sub

    Private Function CreatePresenter() As CollectionPresenter
        Return New CollectionPresenter(
            _mockBmbService.Object,
            _mockAeService.Object,
            _mockOcmService.Object,
            _mockReportService.Object,
            _mockCollectionService.Object
        )
    End Function

    Private Shared Function CreateMockUow() As Mock(Of IUnitOfWork)
        Dim mockBmbRep = New Mock(Of IBankMonthlyBalancesRep)
        Dim mockBankRep = New Mock(Of IBankRep)
        Dim mockUow = New Mock(Of IUnitOfWork)

        mockUow.SetupGet(Function(x) x.BankMonthlyBalancesRepository).Returns(mockBmbRep.Object)
        mockUow.SetupGet(Function(x) x.BankRepository).Returns(mockBankRep.Object)

        Return mockUow
    End Function

    Private Shared Async Function InvokeAdjustAsync(presenter As CollectionPresenter,
                                                    uow As IUnitOfWork,
                                                    oldIsBank As Boolean,
                                                    newIsBank As Boolean,
                                                    oldBankId As Integer,
                                                    oldMonth As Date,
                                                    oldAmount As Decimal,
                                                    newBankId As Integer,
                                                    newMonth As Date,
                                                    newAmount As Decimal) As Task
        Dim po = New PrivateObject(presenter)
        Dim task = CType(
            po.Invoke(
                "AdjustBankMonthlyBalanceForBankSubjectAsync",
                uow,
                oldIsBank,
                newIsBank,
                oldBankId,
                oldMonth,
                oldAmount,
                newBankId,
                newMonth,
                newAmount),
            Task)
        Await task
    End Function

    <TestMethod("科目由非銀行轉為銀行：應增加新銀行當月金額")>
    Public Async Function AdjustBankBalance_NonBankToBank() As Task
        ' Arrange
        Dim presenter = CreatePresenter()
        Dim mockUow = CreateMockUow()
        Dim targetMonth = New Date(2024, 5, 1)
        Dim newAmount As Decimal = 500

        _mockBmbService.Setup(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                mockUow.Object.BankMonthlyBalancesRepository,
                mockUow.Object.BankRepository,
                2,
                targetMonth,
                0,
                newAmount)
        ).Returns(Task.CompletedTask).Verifiable()

        ' Act
        Await InvokeAdjustAsync(presenter, mockUow.Object,
                                oldIsBank:=False,
                                newIsBank:=True,
                                oldBankId:=0,
                                oldMonth:=targetMonth,
                                oldAmount:=0,
                                newBankId:=2,
                                newMonth:=targetMonth,
                                newAmount:=newAmount)

        ' Assert
        _mockBmbService.Verify()
    End Function

    <TestMethod("科目由銀行轉為非銀行：應回沖舊銀行金額")>
    Public Async Function AdjustBankBalance_BankToNonBank() As Task
        ' Arrange
        Dim presenter = CreatePresenter()
        Dim mockUow = CreateMockUow()
        Dim oldMonth = New Date(2024, 6, 1)
        Dim oldAmount As Decimal = 300

        _mockBmbService.Setup(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                mockUow.Object.BankMonthlyBalancesRepository,
                mockUow.Object.BankRepository,
                1,
                oldMonth,
                0,
                -oldAmount)
        ).Returns(Task.CompletedTask).Verifiable()

        ' Act
        Await InvokeAdjustAsync(presenter, mockUow.Object,
                                oldIsBank:=True,
                                newIsBank:=False,
                                oldBankId:=1,
                                oldMonth:=oldMonth,
                                oldAmount:=oldAmount,
                                newBankId:=0,
                                newMonth:=oldMonth,
                                newAmount:=0)

        ' Assert
        _mockBmbService.Verify()
    End Function

    <TestMethod("銀行科目同銀行同月份：只調整差額")>
    Public Async Function AdjustBankBalance_SameBankSameMonth() As Task
        ' Arrange
        Dim presenter = CreatePresenter()
        Dim mockUow = CreateMockUow()
        Dim month = New Date(2024, 7, 1)
        Dim oldAmount As Decimal = 200
        Dim newAmount As Decimal = 350
        Dim delta = newAmount - oldAmount

        _mockBmbService.Setup(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                mockUow.Object.BankMonthlyBalancesRepository,
                mockUow.Object.BankRepository,
                3,
                month,
                0,
                delta)
        ).Returns(Task.CompletedTask).Verifiable()

        ' Act
        Await InvokeAdjustAsync(presenter, mockUow.Object,
                                oldIsBank:=True,
                                newIsBank:=True,
                                oldBankId:=3,
                                oldMonth:=month,
                                oldAmount:=oldAmount,
                                newBankId:=3,
                                newMonth:=month,
                                newAmount:=newAmount)

        ' Assert
        _mockBmbService.Verify()
    End Function

    <TestMethod("銀行科目跨銀行或跨月份：舊減新加各一筆")>
    Public Async Function AdjustBankBalance_CrossBankOrMonth() As Task
        ' Arrange
        Dim presenter = CreatePresenter()
        Dim mockUow = CreateMockUow()
        Dim oldMonth = New Date(2024, 8, 1)
        Dim newMonth = New Date(2024, 9, 1)
        Dim oldAmount As Decimal = 400
        Dim newAmount As Decimal = 250

        _mockBmbService.Setup(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                mockUow.Object.BankMonthlyBalancesRepository,
                mockUow.Object.BankRepository,
                4,
                oldMonth,
                0,
                -oldAmount)
        ).Returns(Task.CompletedTask).Verifiable()

        _mockBmbService.Setup(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                mockUow.Object.BankMonthlyBalancesRepository,
                mockUow.Object.BankRepository,
                5,
                newMonth,
                0,
                newAmount)
        ).Returns(Task.CompletedTask).Verifiable()

        ' Act
        Await InvokeAdjustAsync(presenter, mockUow.Object,
                                oldIsBank:=True,
                                newIsBank:=True,
                                oldBankId:=4,
                                oldMonth:=oldMonth,
                                oldAmount:=oldAmount,
                                newBankId:=5,
                                newMonth:=newMonth,
                                newAmount:=newAmount)

        ' Assert
        _mockBmbService.Verify(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                mockUow.Object.BankMonthlyBalancesRepository,
                mockUow.Object.BankRepository,
                4,
                oldMonth,
                0,
                -oldAmount),
            Times.Once)

        _mockBmbService.Verify(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                mockUow.Object.BankMonthlyBalancesRepository,
                mockUow.Object.BankRepository,
                5,
                newMonth,
                0,
                newAmount),
            Times.Once)
    End Function
End Class

