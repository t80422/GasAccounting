Imports System.Threading.Tasks
Imports GasAccounting
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Moq

<TestClass>
Public Class TestBankMonthlyBalanceService
    Private _mockBmbRep As Mock(Of IBankMonthlyBalancesRep)
    Private _mockBankRep As Mock(Of IBankRep)
    Private _mockPaymentRep As Mock(Of IPaymentRep)
    Private _mockCollectionRep As Mock(Of ICollectionRep)
    Private _service As BankMonthlyBalanceService

    <TestInitialize>
    Public Sub Setup()
        _mockBmbRep = New Mock(Of IBankMonthlyBalancesRep)()
        _mockBankRep = New Mock(Of IBankRep)()
        _mockPaymentRep = New Mock(Of IPaymentRep)()
        _mockCollectionRep = New Mock(Of ICollectionRep)()
        _service = New BankMonthlyBalanceService()
    End Sub

    <TestMethod>
    Public Async Function UpdateMonthBalanceAsync_ShouldUsePreciseSummation() As Task
        ' Arrange
        Dim bankId As Integer = 1
        Dim inputMonth As Date = New Date(2024, 1, 1)
        Dim monthStart As Date = inputMonth
        Dim monthEnd As Date = monthStart.AddMonths(1)

        ' Setup mocked returns for NEW summation methods
        ' 模擬: 
        ' 1. 付款轉入銀行 (借方/現金存入 + 拆分存入) -> PaymentRep.GetBankSideDepositSumAsync = 1000
        ' 2. 收款存入銀行 (借方/銀行存款) -> CollectionRep.GetBankMainDepositSumAsync = 2000
        ' 3. 收款轉出銀行 (貸方/銀行領現 + 拆分領出) -> CollectionRep.GetBankSideWithdrawalSumAsync = 500
        ' 4. 付款轉出銀行 (貸方/銀行存款) -> PaymentRep.GetBankMainWithdrawalSumAsync = 300
        
        _mockPaymentRep.Setup(Function(x) x.GetBankSideDepositSumAsync(bankId, monthStart, monthEnd)).ReturnsAsync(1000)
        _mockCollectionRep.Setup(Function(x) x.GetBankMainDepositSumAsync(bankId, monthStart, monthEnd)).ReturnsAsync(2000)
        
        _mockCollectionRep.Setup(Function(x) x.GetBankSideWithdrawalSumAsync(bankId, monthStart, monthEnd)).ReturnsAsync(500)
        _mockPaymentRep.Setup(Function(x) x.GetBankMainWithdrawalSumAsync(bankId, monthStart, monthEnd)).ReturnsAsync(300)

        ' Setup Initial Balance
        _mockBmbRep.Setup(Function(x) x.GetLastBalanceBeforeMonthAsync(monthStart, bankId)).ReturnsAsync(CType(Nothing, bank_monthly_balances))
        _mockBankRep.Setup(Function(x) x.GetByIdAsync(bankId)).ReturnsAsync(New bank With {.bank_InitialBalance = 5000})
        _mockBmbRep.Setup(Function(x) x.UpdateBankMonthlyBalancesAsync(It.IsAny(Of bank_monthly_balances))).Returns(Task.CompletedTask)

        ' Act
        Await _service.UpdateMonthBalanceAsync(_mockBmbRep.Object, _mockBankRep.Object, _mockPaymentRep.Object, _mockCollectionRep.Object, bankId, inputMonth)

        ' Assert
        ' 驗證是否呼叫了新的精確加總方法
        _mockPaymentRep.Verify(Function(x) x.GetBankSideDepositSumAsync(bankId, monthStart, monthEnd), Times.Once)
        _mockCollectionRep.Verify(Function(x) x.GetBankMainDepositSumAsync(bankId, monthStart, monthEnd), Times.Once)
        _mockCollectionRep.Verify(Function(x) x.GetBankSideWithdrawalSumAsync(bankId, monthStart, monthEnd), Times.Once)
        _mockPaymentRep.Verify(Function(x) x.GetBankMainWithdrawalSumAsync(bankId, monthStart, monthEnd), Times.Once)

        ' 驗證計算正確性
        ' Opening = 5000
        ' Debit = 1000 + 2000 = 3000
        ' Credit = 500 + 300 = 800
        ' Closing = 5000 + 3000 - 800 = 7200
        _mockBmbRep.Verify(Sub(x) x.UpdateBankMonthlyBalancesAsync(It.Is(Function(b As bank_monthly_balances) _
            b.bm_OpeningBalance = 5000 AndAlso
            b.bm_TotalDebit = 3000 AndAlso
            b.bm_TotalCredit = 800 AndAlso
            b.bm_ClosingBalance = 7200)), Times.Once)
    End Function

End Class
