Imports System.Data.Entity

Public Class PaymentRep
    Inherits Repository(Of payment)
    Implements IPaymentRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function SearchPaymentAsync(Optional criteria As PaymentSearchCriteria = Nothing) As Task(Of IEnumerable(Of payment)) Implements IPaymentRep.SearchPaymentAsync
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If criteria IsNot Nothing Then
                If criteria.IsSearchDate Then query = query.Where(Function(x) x.p_Date >= criteria.StartDate AndAlso x.p_Date < criteria.EndDate)
                If criteria.CompanyId.HasValue Then query = query.Where(Function(x) x.p_comp_Id = criteria.CompanyId)
                If criteria.BankId.HasValue Then query = query.Where(Function(x) x.p_bank_Id = criteria.BankId)
                If Not String.IsNullOrEmpty(criteria.ChequeNo) Then query = query.Where(Function(x) x.chque_pay.cp_Number = criteria.ChequeNo)
                If criteria.SubjectId.HasValue Then query = query.Where(Function(x) x.p_s_Id = criteria.SubjectId)
                If criteria.VendorId.HasValue Then query = query.Where(Function(x) x.p_m_Id = criteria.VendorId)
                If Not String.IsNullOrEmpty(criteria.Cridit) Then query = query.Where(Function(x) x.p_Type = criteria.Cridit)
            End If

            Return Await query.OrderByDescending(Function(x) x.p_Date).ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetVendorAmountDue(vendorId As Integer) As List(Of AmountDueVM) Implements IPaymentRep.GetVendorAmountDue
        Try
            ' 取得該製造商的購買和付款數據
            Dim purchaseData = (
                From pur In _context.purchases.AsNoTracking().AsEnumerable
                Where pur.pur_manu_id = vendorId
                Group By Year = pur.pur_date.Value.Year, Month = pur.pur_date.Value.Month Into GroupTotal = Sum(pur.pur_price)
                Select New With {Year, Month, .TotalPurchase = GroupTotal}
                ).ToList()

            Dim paymentData = (
                From pay In _context.payments.AsNoTracking().AsEnumerable
                Where pay.p_m_Id = vendorId
                Group By Year = pay.p_Date.Year, Month = pay.p_Date.Month Into GroupTotal = Sum(pay.p_Amount)
                Select New With {Year, Month, .TotalPayment = GroupTotal}
                ).ToList()

            ' 未付帳款列表
            Dim amountDueList As New List(Of AmountDueVM)

            ' 計算每個月的未付帳款
            For Each pur In purchaseData
                Dim correspondingPayment = paymentData.
                    Where(Function(p) p.Year = pur.Year AndAlso p.Month = pur.Month).
                    Select(Function(p) p.TotalPayment).
                    DefaultIfEmpty(0).
                    FirstOrDefault()
                Dim unpaidAmount = pur.TotalPurchase - correspondingPayment

                amountDueList.Add(New AmountDueVM With {
                    .廠商 = (From manu In _context.manufacturers Where manu.manu_id = vendorId Select manu.manu_name).FirstOrDefault(),
                    .年月份 = New DateTime(pur.Year, pur.Month, 1).ToString("yyyy/MM"),
                    .未付帳款 = unpaidAmount
                })
            Next

            Return amountDueList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetBankPaymentsByDateRangeAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of IEnumerable(Of payment)) Implements IPaymentRep.GetBankPaymentsByDateRangeAsync
        Try
            Return Await _dbSet.AsNoTracking.
                Where(Function(x) x.p_Date >= startDate AndAlso
                                  x.p_Date < endDate AndAlso
                                  x.p_Type = "銀行存款" AndAlso
                                  x.p_bank_Id = bankId).
                ToListAsync()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetCashSubpoenaData(selectDate As Date) As List(Of CashSubpoenaDTO) Implements IPaymentRep.GetCashSubpoenaData
        Try
            Dim payments = _dbSet.Where(Function(x) x.p_Date = selectDate AndAlso x.p_Type = "現金").ToList()
            Dim result As New List(Of CashSubpoenaDTO)

            For Each x In payments
                ' 第一組：金額 1 -> subject1
                If x.p_debit_amount_1.GetValueOrDefault() > 0 Then
                    result.Add(New CashSubpoenaDTO With {
                        .SubjectName = x.subject1?.s_name,
                        .Amount = x.p_debit_amount_1.Value,
                        .Summary = x.p_Memo,
                        .Code = x.manufacturer?.manu_code
                    })
                End If

                ' 第二組：金額 2 -> subject2
                If x.p_debit_amount_2.GetValueOrDefault() > 0 Then
                    result.Add(New CashSubpoenaDTO With {
                        .SubjectName = x.subject2?.s_name,
                        .Amount = x.p_debit_amount_2.Value,
                        .Summary = x.p_Memo,
                        .Code = x.manufacturer?.manu_code
                    })
                End If

                ' 第三組：金額 3 -> subject
                If x.p_debit_amount_3.GetValueOrDefault() > 0 Then
                    result.Add(New CashSubpoenaDTO With {
                        .SubjectName = x.subject?.s_name,
                        .Amount = x.p_debit_amount_3.Value,
                        .Summary = x.p_Memo,
                        .Code = x.manufacturer?.manu_code
                    })
                End If
            Next

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetTransferSubpoenaData(day As Date) As List(Of TransferSubpoenaDTO) Implements IPaymentRep.GetTransferSubpoenaData
        Try
            ' 轉帳傳票範圍：銀行存款、應付票據 (不含轉帳折讓)
            Dim payments = _dbSet.Where(Function(x) x.p_Date = day AndAlso
                                          (x.p_Type = "銀行存款" OrElse x.p_Type = "應付票據")).ToList()

            Dim result As New List(Of TransferSubpoenaDTO)

            For Each x In payments
                ' 第一組：金額 1 -> subject1 (借) / p_Type (貸)
                If x.p_debit_amount_1.GetValueOrDefault() > 0 Then
                    result.Add(New TransferSubpoenaDTO With {
                        .DebitSubjectName = x.subject1?.s_name,
                        .DebitAmount = x.p_debit_amount_1.Value,
                        .DebitSummary = x.p_Memo,
                        .CreditSubjectName = x.p_Type,
                        .CreditAmount = x.p_Amount,
                        .CreditSummary = If(x.p_Type = "銀行存款", "", x.p_Memo),
                        .Id = x.p_Id
                    })
                End If

                ' 第二組：金額 2 -> subject2 (借) / p_Type (貸)
                If x.p_debit_amount_2.GetValueOrDefault() > 0 Then
                    result.Add(New TransferSubpoenaDTO With {
                        .DebitSubjectName = x.subject2?.s_name,
                        .DebitAmount = x.p_debit_amount_2.Value,
                        .DebitSummary = x.p_Memo,
                        .CreditSubjectName = x.p_Type,
                        .CreditAmount = x.p_Amount,
                        .CreditSummary = If(x.p_Type = "銀行存款", "", x.p_Memo),
                        .Id = x.p_Id
                    })
                End If

                ' 第三組：金額 3 -> subject (借) / p_Type (貸)
                If x.p_debit_amount_3.GetValueOrDefault() > 0 Then
                    result.Add(New TransferSubpoenaDTO With {
                        .DebitSubjectName = x.subject?.s_name,
                        .DebitAmount = x.p_debit_amount_3.Value,
                        .DebitSummary = x.p_Memo,
                        .CreditSubjectName = x.p_Type,
                        .CreditAmount = x.p_Amount,
                        .CreditSummary = If(x.p_Type = "銀行存款", "", x.p_Memo),
                        .Id = x.p_Id
                    })
                End If
            Next

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetByCriteriaAndVendors(criteria As PaymentSearchCriteria, vendorIds As List(Of Integer)) As List(Of payment) Implements IPaymentRep.GetByCriteriaAndVendors
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If criteria.IsSearchDate Then query = query.Where(Function(x) x.p_AccountMonth >= criteria.StartDate AndAlso x.p_AccountMonth < criteria.EndDate)
            If criteria.CompanyId.HasValue Then query = query.Where(Function(x) x.p_comp_Id = criteria.CompanyId)

            query = query.Where(Function(x) vendorIds.Contains(x.p_m_Id))

            Return query.ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetCashToBankTransfersByDateRangeAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of IEnumerable(Of payment)) Implements IPaymentRep.GetCashToBankTransfersByDateRangeAsync
        Try
            Return Await _dbSet.AsNoTracking.
                Where(Function(x) x.p_Date >= startDate AndAlso
                                  x.p_Date < endDate AndAlso
                                  x.p_Type = "現金" AndAlso
                                  x.p_bank_Id = bankId AndAlso
                                  x.subject.s_name = "銀行存款").
                ToListAsync()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetBankAccount(bankId As Integer) As IEnumerable(Of payment) Implements IPaymentRep.GetBankAccount
        Try
            Return _dbSet.Where(Function(x) (x.p_bank_Id = bankId AndAlso x.p_Type = "銀行存款") Or
                                            (x.p_bank_Id = bankId AndAlso x.subject1.s_name = "銀行存款") Or
                                            (x.p_debit_bank_id_2 = bankId AndAlso x.subject2.s_name = "銀行存款") Or
                                            (x.p_debit_bank_id_3 = bankId AndAlso x.subject.s_name = "銀行存款"))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetBankMainWithdrawalSumAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of Integer) Implements IPaymentRep.GetBankMainWithdrawalSumAsync
        Try
            Dim total = Await _dbSet.AsNoTracking.
                Where(Function(x) x.p_Date >= startDate AndAlso
                                  x.p_Date < endDate AndAlso
                                  x.p_Type = "銀行存款" AndAlso
                                  x.p_bank_Id = bankId).
                SumAsync(Function(x) x.p_Amount)
            Return total
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Async Function GetBankSideDepositSumAsync(bankId As Integer, startDate As Date, endDate As Date) As Task(Of Integer) Implements IPaymentRep.GetBankSideDepositSumAsync
        Try
            Dim sum1 = Await _dbSet.AsNoTracking.
                Where(Function(x) x.p_Date >= startDate AndAlso
                                  x.p_Date < endDate AndAlso
                                  x.p_bank_Id = bankId AndAlso
                                  x.subject1.s_name = "銀行存款").
                SumAsync(Function(x) CType(x.p_debit_amount_1, Integer?))

            Dim sum2 = Await _dbSet.AsNoTracking.
                Where(Function(x) x.p_Date >= startDate AndAlso
                                  x.p_Date < endDate AndAlso
                                  x.p_debit_bank_id_2 = bankId AndAlso
                                  x.subject2.s_name = "銀行存款").
                SumAsync(Function(x) CType(x.p_debit_amount_2, Integer?))

            Dim sum3 = Await _dbSet.AsNoTracking.
                Where(Function(x) x.p_Date >= startDate AndAlso
                                  x.p_Date < endDate AndAlso
                                  x.p_debit_bank_id_3 = bankId AndAlso
                                  x.subject.s_name = "銀行存款").
                SumAsync(Function(x) CType(x.p_debit_amount_3, Integer?))

            Return sum1.GetValueOrDefault() + sum2.GetValueOrDefault() + sum3.GetValueOrDefault()
        Catch ex As Exception
            Return 0
        End Try
    End Function
End Class
