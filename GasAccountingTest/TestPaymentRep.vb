Imports GasAccounting
Imports Moq

<TestClass>
Public Class TestPaymentRep
    Private _mockContext As New Mock(Of gas_accounting_systemEntities)()
    Private _repository As New PaymentRep(_mockContext.Object)

    <TestMethod("借方總金額")>
    Public Sub GetBankSideDepositSumAsync_Currect()
        ' Arrange
        Dim bankId As Integer = 1
        Dim startDate = Now.Date
        Dim endDate = startDate.AddDays(1)

        Dim subject = New subject With {
            .s_id = 1,
            .s_name = "銀行存款"
        }

        Dim payments = New List(Of payment) From {
            New payment With {
                .p_Id = 1,
                .p_Date = Now.Date,
                .p_bank_Id = bankId,
                .p_s_Id = subject.s_id,
                .subject = subject
            }
        }

        ' Act
        Dim result = _repository.GetBankSideDepositSumAsync(bankId, startDate, endDate).Result
    End Sub
End Class
