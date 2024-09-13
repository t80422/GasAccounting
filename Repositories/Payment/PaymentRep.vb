Imports System.Data.Entity

Public Class PaymentRep
    Inherits Repository(Of payment)
    Implements IPaymentRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function SearchPaymentAsync(criteria As PaymentSearchCriteria) As Task(Of IEnumerable(Of payment)) Implements IPaymentRep.SearchPaymentAsync
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If criteria.IsSearchDate Then query = query.Where(Function(x) x.p_Date >= criteria.StartDate AndAlso x.p_Date < criteria.EndDate)
            If criteria.CompanyId.HasValue Then query = query.Where(Function(x) x.p_comp_Id = criteria.CompanyId)
            If criteria.BankId.HasValue Then query = query.Where(Function(x) x.p_bank_Id = criteria.BankId)
            If Not String.IsNullOrEmpty(criteria.ChequeNo) Then query = query.Where(Function(x) x.p_Cheque = criteria.ChequeNo)
            If criteria.SubjectId.HasValue Then query = query.Where(Function(x) x.p_s_Id = criteria.SubjectId)
            If criteria.VendorId.HasValue Then query = query.Where(Function(x) x.p_m_Id = criteria.VendorId)

            Return Await query.ToListAsync
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
                Group By Year = pay.p_AccountMonth.Value.Year, Month = pay.p_AccountMonth.Value.Month Into GroupTotal = Sum(pay.p_Amount)
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

                If unpaidAmount > 0 Then
                    amountDueList.Add(New AmountDueVM With {
                        .廠商 = (From manu In _context.manufacturers Where manu.manu_id = vendorId Select manu.manu_name).FirstOrDefault(),
                        .年月份 = New DateTime(pur.Year, pur.Month, 1).ToString("yyyy/MM"),
                        .未付帳款 = unpaidAmount
                    })
                End If
            Next

            Return amountDueList

            '' 獲取廠商名稱
            'Dim vendorName = Await _context.manufacturers.
            '    Where(Function(m) m.manu_id = vendorId).
            '    Select(Function(m) m.manu_name).
            '    FirstOrDefaultAsync()

            '' 計算每月的購買和付款總額
            'Dim query = From purchase In _context.purchases.AsNoTracking()
            '            Where purchase.pur_manu_id = vendorId
            '            Group Join payment In _context.payments.AsNoTracking()
            '            On New With {
            '                Key .VendorId = purchase.pur_manu_id,
            '                Key .Year = purchase.pur_date.Value.Year,
            '                Key .Month = purchase.pur_date.Value.Month
            '            } Equals New With {
            '                Key .VendorId = CType(payment.p_m_Id, Integer),
            '                Key .Year = payment.p_AccountMonth.Value.Year,
            '                Key .Month = payment.p_AccountMonth.Value.Month
            '            } Into FullOuterJoin = Group
            '            Select New With {
            '            Key .Year = purchase.pur_date.Value.Year,
            '            Key .Month = purchase.pur_date.Value.Month,
            '            .TotalPurchase = purchase.pur_price,
            '            .TotalPayment = FullOuterJoin.Sum(Function(p) p.p_Amount)
            '        }

            'Dim result = Await query.ToListAsync()

            '' 處理結果並只返回未付款大於0的記錄
            'Return result.Select(Function(item)
            '                         Dim unpaidAmount = item.TotalPurchase - item.TotalPayment
            '                         Return New AmountDueVM With {
            '                         .廠商 = vendorName,
            '                         .年月份 = New DateTime(item.Year, item.Month, 1).ToString("yyyy/MM"),
            '                         .未付帳款 = If(unpaidAmount > 0, unpaidAmount, 0)
            '                     }
            '                     End Function).
            '                Where(Function(item) item.未付帳款 > 0).
            '                OrderBy(Function(item) item.年月份).
            '                ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetByBankAndMonthAsync(bankId As Integer, month As Date) As Task(Of IEnumerable(Of payment)) Implements IPaymentRep.GetByBankAndMonthAsync
        Try

            Return Await _dbSet.AsNoTracking.Where(Function(x) x.p_AccountMonth = month AndAlso x.p_bank_Id = bankId).ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
