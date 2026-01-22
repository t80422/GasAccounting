Imports System.Threading.Tasks
Imports GasAccounting
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Moq

<TestClass>
Public Class TestPaymentPresenter
    Private _mockView As Mock(Of IPaymentView)
    Private _mockBmbService As Mock(Of IBankMonthlyBalanceService)
    Private _mockAeSer As Mock(Of IAccountingEntryService)
    Private _mockReportSer As Mock(Of IReportService)
    Private _mockUowFactory As Mock(Of IUnitOfWorkFactory)

    <TestInitialize>
    Public Sub Setup()
        _mockView = New Mock(Of IPaymentView)()
        _mockBmbService = New Mock(Of IBankMonthlyBalanceService)()
        _mockAeSer = New Mock(Of IAccountingEntryService)()
        _mockReportSer = New Mock(Of IReportService)()
        _mockUowFactory = New Mock(Of IUnitOfWorkFactory)()
    End Sub

    Private Function CreatePresenter() As PaymentPresenter
        Return New PaymentPresenter(
            _mockView.Object,
            _mockBmbService.Object,
            _mockAeSer.Object,
            _mockReportSer.Object,
            _mockUowFactory.Object
        )
    End Function

    Private Shared Function CreateMockUow() As Mock(Of IUnitOfWork)
        Dim mockBmbRep = New Mock(Of IBankMonthlyBalancesRep)
        Dim mockBankRep = New Mock(Of IBankRep)
        Dim mockSubjectRep = New Mock(Of ISubjectRep)
        Dim mockUow = New Mock(Of IUnitOfWork)

        mockUow.SetupGet(Function(x) x.BankMonthlyBalancesRepository).Returns(mockBmbRep.Object)
        mockUow.SetupGet(Function(x) x.BankRepository).Returns(mockBankRep.Object)
        mockUow.SetupGet(Function(x) x.SubjectRepository).Returns(mockSubjectRep.Object)

        Return mockUow
    End Function

    Private Shared Async Function InvokeProcessDebitAsync(presenter As PaymentPresenter,
                                                        uow As IUnitOfWork,
                                                        oldSubjectId As Integer?,
                                                        newSubjectId As Integer?,
                                                        oldBankId As Integer?,
                                                        newBankId As Integer?,
                                                        oldAmount As Integer?,
                                                        newAmount As Integer?,
                                                        oldMonth As Date,
                                                        newMonth As Date) As Task
        Dim po = New PrivateObject(presenter)
        
        Dim task = CType(
            po.Invoke(
                "ProcessDebitBankSubjectAsync",
                uow,
                CType(Nothing, payment), 
                CType(Nothing, payment),
                oldSubjectId,
                newSubjectId,
                oldBankId,
                newBankId,
                oldAmount,
                newAmount,
                oldMonth,
                newMonth),
            Task)
        Await task
    End Function

    <TestMethod("借方科目由非銀行轉為銀行(存入)：應增加Debit(存款增加)")>
    Public Async Function ProcessDebit_NonBankToBank_IncreaseDebit() As Task
        ' Arrange
        Dim presenter = CreatePresenter()
        Dim mockUow = CreateMockUow()
        Dim targetMonth = New Date(2024, 5, 1)
        Dim newAmount As Integer = 1000
        Dim newBankId As Integer = 2
        Dim newSubjectId As Integer = 101 ' Bank Subject ID

        ' Setup Subject Repository to return "銀行存款" for ID 101
        Mock.Get(mockUow.Object.SubjectRepository).Setup(Function(x) x.GetByIdAsync(newSubjectId)).
            ReturnsAsync(New subject With {.s_id = newSubjectId, .s_name = "銀行存款"})

        ' Expect UpdateMonthBalanceIncrementalAsync call:
        ' debitDelta = +1000 (Increase Debit)
        _mockBmbService.Setup(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                mockUow.Object.BankMonthlyBalancesRepository,
                mockUow.Object.BankRepository,
                newBankId,
                targetMonth,
                0,      ' creditDelta
                newAmount ' debitDelta
            )
        ).Returns(Task.CompletedTask).Verifiable()

        ' Act
        Await InvokeProcessDebitAsync(presenter, mockUow.Object,
                                      oldSubjectId:=Nothing, ' Or non-bank ID
                                      newSubjectId:=newSubjectId,
                                      oldBankId:=Nothing,
                                      newBankId:=newBankId,
                                      oldAmount:=Nothing,
                                      newAmount:=newAmount,
                                      oldMonth:=DateTime.MinValue,
                                      newMonth:=targetMonth)

        ' Assert
        _mockBmbService.Verify()
    End Function

    <TestMethod("借方科目由銀行轉為非銀行(取消存入)：應減少Debit(存款減少)")>
    Public Async Function ProcessDebit_BankToNonBank_DecreaseDebit() As Task
        ' Arrange
        Dim presenter = CreatePresenter()
        Dim mockUow = CreateMockUow()
        Dim oldMonth = New Date(2024, 5, 1)
        Dim oldAmount As Integer = 1000
        Dim oldBankId As Integer = 2
        Dim oldSubjectId As Integer = 101

        ' Setup Subject Repository
        Mock.Get(mockUow.Object.SubjectRepository).Setup(Function(x) x.GetByIdAsync(oldSubjectId)).
            ReturnsAsync(New subject With {.s_id = oldSubjectId, .s_name = "銀行存款"})

        ' Expect debitDelta = -1000 (Decrease Debit)
        _mockBmbService.Setup(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                mockUow.Object.BankMonthlyBalancesRepository,
                mockUow.Object.BankRepository,
                oldBankId,
                oldMonth,
                0,        ' creditDelta
                -oldAmount ' debitDelta
            )
        ).Returns(Task.CompletedTask).Verifiable()

        ' Act
        Await InvokeProcessDebitAsync(presenter, mockUow.Object,
                                      oldSubjectId:=oldSubjectId,
                                      newSubjectId:=Nothing,
                                      oldBankId:=oldBankId,
                                      newBankId:=Nothing,
                                      oldAmount:=oldAmount,
                                      newAmount:=Nothing,
                                      oldMonth:=oldMonth,
                                      newMonth:=DateTime.MinValue)

        ' Assert
        _mockBmbService.Verify()
    End Function

     <TestMethod("同月份金額調整：應更新已差額")>
    Public Async Function ProcessDebit_AmountChange_IncrementalUpdate() As Task
        ' Arrange
        Dim presenter = CreatePresenter()
        Dim mockUow = CreateMockUow()
        Dim targetMonth = New Date(2024, 5, 1)
        Dim oldAmount As Integer = 1000
        Dim newAmount As Integer = 1500
        Dim bankId As Integer = 2
        Dim subjectId As Integer = 101

        ' Setup Subject Repository
        Mock.Get(mockUow.Object.SubjectRepository).Setup(Function(x) x.GetByIdAsync(subjectId)).
            ReturnsAsync(New subject With {.s_id = subjectId, .s_name = "銀行存款"})

        ' Expect debitDelta = +500
        _mockBmbService.Setup(
            Function(s) s.UpdateMonthBalanceIncrementalAsync(
                mockUow.Object.BankMonthlyBalancesRepository,
                mockUow.Object.BankRepository,
                bankId,
                targetMonth,
                0,        ' creditDelta
                500       ' debitDelta (1500 - 1000)
            )
        ).Returns(Task.CompletedTask).Verifiable()

        ' Act
        Await InvokeProcessDebitAsync(presenter, mockUow.Object,
                                      oldSubjectId:=subjectId,
                                      newSubjectId:=subjectId,
                                      oldBankId:=bankId,
                                      newBankId:=bankId,
                                      oldAmount:=oldAmount,
                                      newAmount:=newAmount,
                                      oldMonth:=targetMonth,
                                      newMonth:=targetMonth)

        ' Assert
        _mockBmbService.Verify()
    End Function
End Class
