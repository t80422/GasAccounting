Imports GasAccounting
Imports Moq

<TestClass>
Public Class TestBankMonthlyBalanceService
    Private _mockBmbRep As Mock(Of IBankMonthlyBalancesRep)
    Private _mockBankRep As Mock(Of IBankRep)
    Private _mockPaymentRep As Mock(Of IPaymentRep)
    Private _mockCollectionRep As Mock(Of ICollectionRep)
    Private _service As BankMonthlyBalanceService

    Private _mockContext As Mock(Of gas_accounting_systemEntities)


    <TestInitialize>
    Public Sub Setup()
        _mockBmbRep = New Mock(Of IBankMonthlyBalancesRep)()
        _mockBankRep = New Mock(Of IBankRep)()
        _mockPaymentRep = New Mock(Of IPaymentRep)()
        _mockCollectionRep = New Mock(Of ICollectionRep)()
        _service = New BankMonthlyBalanceService()

        _mockContext = New Mock(Of gas_accounting_systemEntities)()
    End Sub

    <TestMethod("測試 UpdateMonthBalanceAsync 是否正確統計並儲存 (使用假資料列表)")>
    Public Async Function UpdateMonthBalanceAsync_Currect() As Task
        ' Arrange
        Dim bankId As Integer = 1
        Dim inputDate = New Date(2026, 1, 1)
        Dim monthStart = New Date(inputDate.Year, inputDate.Month, 1)
        Dim monthEnd = monthStart.AddMonths(1)

        ' 定義假科目 (銀行存款)
        Dim bankSubject = New subject With {.s_id = 1, .s_name = "銀行存款"}

        ' 建立假資料列表 (讓測試更有真實感)
        Dim payments = New List(Of payment) From {
            New payment With {.p_bank_Id = 1, .p_Type = "銀行存款", .p_Amount = 400, .p_Date = monthStart}, ' 貸方 400
            New payment With {.p_bank_Id = 1, .subject1 = bankSubject, .p_debit_amount_1 = 100, .p_Date = monthStart}, ' 借方 100
            New payment With {.p_debit_bank_id_2 = 1, .subject2 = bankSubject, .p_debit_amount_2 = 200, .p_Date = monthStart}, ' 借方 200
            New payment With {.p_debit_bank_id_3 = 1, .subject = bankSubject, .p_debit_amount_3 = 300, .p_Date = monthStart} ' 借方 300
        }

        Dim collections = New List(Of collection) From {
            New collection With {.col_bank_Id = 1, .col_Type = "銀行存款", .col_Amount = 111, .col_Date = monthStart}, ' 借方 111
            New collection With {.col_credit_bank_id = 1, .subject = bankSubject, .col_credit_amount_1 = 222, .col_Date = monthStart}, ' 貸方 222
            New collection With {.col_credit_bank_id_2 = 1, .subject2 = bankSubject, .col_credit_amount_2 = 333, .col_Date = monthStart}, ' 貸方 333
            New collection With {.col_credit_bank_id_3 = 1, .subject1 = bankSubject, .col_credit_amount_3 = 333, .col_Date = monthStart} ' 貸方 333
        }

        ' --- 動態 Mock 設置 (這讓 Rep 表現得像是有跑邏輯一樣) ---
        
        ' 模擬 Payment 借方合計 logic
        _mockPaymentRep.Setup(Function(x) x.GetBankSideDepositSumAsync(bankId, monthStart, monthEnd)).ReturnsAsync(
            payments.Where(Function(p) (p.p_bank_Id = bankId AndAlso p.subject1?.s_name = "銀行存款") OrElse
                                      (p.p_debit_bank_id_2 = bankId AndAlso p.subject2?.s_name = "銀行存款") OrElse
                                      (p.p_debit_bank_id_3 = bankId AndAlso p.subject?.s_name = "銀行存款")) _
                    .Sum(Function(p) p.p_debit_amount_1.GetValueOrDefault() + p.p_debit_amount_2.GetValueOrDefault() + p.p_debit_amount_3.GetValueOrDefault())
        )

        ' 模擬 Payment 貸方合計 logic
        _mockPaymentRep.Setup(Function(x) x.GetBankMainWithdrawalSumAsync(bankId, monthStart, monthEnd)).ReturnsAsync(
            payments.Where(Function(p) p.p_bank_Id = bankId AndAlso p.p_Type = "銀行存款").Sum(Function(p) p.p_Amount)
        )

        ' 模擬 Collection 借方合計 logic
        _mockCollectionRep.Setup(Function(x) x.GetBankMainDepositSumAsync(bankId, monthStart, monthEnd)).ReturnsAsync(
            collections.Where(Function(c) c.col_bank_Id = bankId AndAlso c.col_Type = "銀行存款").Sum(Function(c) c.col_Amount)
        )

        ' 模擬 Collection 貸方合計 logic
        _mockCollectionRep.Setup(Function(x) x.GetBankSideWithdrawalSumAsync(bankId, monthStart, monthEnd)).ReturnsAsync(
            collections.Where(Function(c) (c.col_credit_bank_id = bankId AndAlso c.subject?.s_name = "銀行存款") OrElse
                                         (c.col_credit_bank_id_2 = bankId AndAlso c.subject2?.s_name = "銀行存款") OrElse
                                         (c.col_credit_bank_id_3 = bankId AndAlso c.subject1?.s_name = "銀行存款")) _
                       .Sum(Function(c) c.col_credit_amount_1.GetValueOrDefault() + c.col_credit_amount_2.GetValueOrDefault() + c.col_credit_amount_3.GetValueOrDefault())
        )

        ' 模擬上期餘額
        _mockBmbRep.Setup(Function(x) x.GetLastBalanceBeforeMonthAsync(monthStart, bankId)).ReturnsAsync(New bank_monthly_balances With {.bm_ClosingBalance = 1000})

        ' Act
        Await _service.UpdateMonthBalanceAsync(_mockBmbRep.Object, _mockBankRep.Object, _mockPaymentRep.Object, _mockCollectionRep.Object, bankId, inputDate)

        ' Assert (驗證最終計算結果)
        ' 期待值: 借方 (100+200+300) + 111 = 711, 貸方 400 + (222+333+333) = 1288
        _mockBmbRep.Verify(Sub(x) x.UpdateBankMonthlyBalancesAsync(It.Is(Of bank_monthly_balances)(
            Function(b) b.bm_TotalDebit = 711 AndAlso 
                        b.bm_TotalCredit = 1288 AndAlso 
                        b.bm_ClosingBalance = 423
        )), Times.Once())
    End Function
End Class
