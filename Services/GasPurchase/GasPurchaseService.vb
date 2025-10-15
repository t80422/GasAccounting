Public Class GasPurchaseService
    Implements IGasPurchaseService

    Private ReadOnly _paymentRep As IPaymentRep

    Public Sub New(paymentRep As IPaymentRep)
        _paymentRep = paymentRep
    End Sub

    Public Function GetPurchaseTradeSummary(datas As List(Of purchase), criteria As PurchaseCondition) As Tuple(Of List(Of PurchaseGasVendorTradeSummaryListVM), List(Of PurchaseFreightTradeSummaryListVM)) Implements IGasPurchaseService.GetPurchaseTradeSummary
        Try
            Dim paymentCriteria = New PaymentSearchCriteria With {
                .IsSearchDate = criteria.IsDateSearch,
                .StartDate = criteria.StartDate,
                .EndDate = criteria.EndDate
            }

            ' 先取得有效的採購資料
            Dim validPurchases = datas.Where(Function(x) x.pur_manu_id.HasValue AndAlso x.pur_date.HasValue AndAlso x.manufacturer IsNot Nothing).ToList()

            ' 手動分組處理：按年月+廠商分組
            Dim gasVendorGroups As New Dictionary(Of String, List(Of purchase))
            For Each item In validPurchases
                Dim groupKey = $"{item.pur_date.Value.Year:0000}-{item.pur_date.Value.Month:00}-{item.pur_manu_id.Value}"
                If Not gasVendorGroups.ContainsKey(groupKey) Then
                    gasVendorGroups(groupKey) = New List(Of purchase)
                End If
                gasVendorGroups(groupKey).Add(item)
            Next

            ' 建立月份總結資料
            Dim gasVendorMonthlyData As New List(Of Object)
            For Each kvp In gasVendorGroups
                Dim groupData = kvp.Value
                Dim firstItem = groupData.First()

                gasVendorMonthlyData.Add(New With {
                    .Year = firstItem.pur_date.Value.Year,
                    .Month = firstItem.pur_date.Value.Month,
                    .VendorId = firstItem.pur_manu_id.Value,
                    .vendorName = firstItem.manufacturer.manu_name,
                    .monthString = firstItem.pur_date.Value.ToString("yyyy/MM"),
                    .gasKg = groupData.Where(Function(x) x.pur_product = "普氣").Sum(Function(x) If(x.pur_quantity, 0)),
                    .gasAmount = groupData.Where(Function(x) x.pur_product = "普氣").Sum(Function(x) If(x.pur_price, 0)),
                    .gas_cKg = groupData.Where(Function(x) x.pur_product = "丙氣").Sum(Function(x) If(x.pur_quantity, 0)),
                    .gas_cAmount = groupData.Where(Function(x) x.pur_product = "丙氣").Sum(Function(x) If(x.pur_price, 0))
                })
            Next

            ' 取得所有涉及的大氣廠商ID
            Dim gasVendorIds = gasVendorMonthlyData.Select(Function(p) Integer.Parse(p.VendorId)).Distinct().ToList()

            ' 查詢大氣付款資料並手動分組
            Dim gasVendorPayment = _paymentRep.GetByCriteriaAndVendors(paymentCriteria, gasVendorIds)
            Dim validPayments = gasVendorPayment.Where(Function(x) x.p_m_Id.HasValue).ToList()

            ' 手動分組處理：按年月+廠商分組付款資料
            Dim gasPaymentGroups As New Dictionary(Of String, Integer)
            For Each payment In validPayments
                Dim groupKey = $"{payment.p_Date.Year:0000}-{payment.p_Date.Month:00}-{payment.p_m_Id.Value}"
                If Not gasPaymentGroups.ContainsKey(groupKey) Then
                    gasPaymentGroups(groupKey) = 0
                End If
                gasPaymentGroups(groupKey) += payment.p_Amount
            Next

            ' 建立大氣廠商月份總結（數據已按廠商+月份分組）
            Dim gasVendorResult As New List(Of PurchaseGasVendorTradeSummaryListVM)
            For Each monthData In gasVendorMonthlyData
                Dim paymentKey = $"{monthData.Year:0000}-{monthData.Month:00}-{monthData.VendorId}"
                Dim paidAmount = If(gasPaymentGroups.ContainsKey(paymentKey), gasPaymentGroups(paymentKey), 0)
                Dim totalAmount = monthData.gasAmount + monthData.gas_cAmount

                gasVendorResult.Add(New PurchaseGasVendorTradeSummaryListVM With {
                    .廠商名稱 = monthData.vendorName,
                    .帳款月份 = monthData.monthString,
                    .普氣Kg = monthData.gasKg,
                    .普氣金額 = monthData.gasAmount,
                    .丙氣Kg = monthData.gas_cKg,
                    .丙氣金額 = monthData.gas_cAmount,
                    .總計 = totalAmount,
                    .已付金額 = paidAmount,
                    .尚欠金額 = totalAmount - paidAmount
                })
            Next

            ' 添加大氣廠商總計
            Dim gasVendorTotals = gasVendorResult.GroupBy(Function(x) x.廠商名稱).
                                                  Select(Function(g) New PurchaseGasVendorTradeSummaryListVM With {
                                                      .廠商名稱 = g.Key,
                                                      .帳款月份 = "總計",
                                                      .普氣Kg = g.Sum(Function(x) x.普氣Kg),
                                                      .普氣金額 = g.Sum(Function(x) x.普氣金額),
                                                      .丙氣Kg = g.Sum(Function(x) x.丙氣Kg),
                                                      .丙氣金額 = g.Sum(Function(x) x.丙氣金額),
                                                      .總計 = g.Sum(Function(x) x.總計),
                                                      .已付金額 = g.Sum(Function(x) x.已付金額),
                                                      .尚欠金額 = g.Sum(Function(x) x.尚欠金額)
                                                  }).ToList

            gasVendorResult.AddRange(gasVendorTotals)

            ' 先取得有效的運輸資料
            Dim validTransportation = datas.Where(Function(x) x.pur_DriveCmpId.HasValue AndAlso x.manufacturer1 IsNot Nothing AndAlso x.pur_date.HasValue).ToList()

            ' 手動分組處理：按年月+運輸廠商分組
            Dim transportationGroups As New Dictionary(Of String, List(Of purchase))
            For Each item In validTransportation
                Dim groupKey = $"{item.pur_date.Value.Year:0000}-{item.pur_date.Value.Month:00}-{item.pur_DriveCmpId.Value}"
                If Not transportationGroups.ContainsKey(groupKey) Then
                    transportationGroups(groupKey) = New List(Of purchase)
                End If
                transportationGroups(groupKey).Add(item)
            Next

            ' 建立運輸月份總結資料
            Dim transportationMonthlyData As New List(Of Object)
            For Each kvp In transportationGroups
                Dim groupData = kvp.Value
                Dim firstItem = groupData.First()

                transportationMonthlyData.Add(New With {
                    .Year = firstItem.pur_date.Value.Year,
                    .Month = firstItem.pur_date.Value.Month,
                    .vendorId = firstItem.pur_DriveCmpId.Value,
                    .vendorName = firstItem.manufacturer1.manu_name,
                    .monthString = firstItem.pur_date.Value.ToString("yyyy/MM"),
                    .Amount = groupData.Sum(Function(x) If(x.pur_delivery_fee, 0))
                })
            Next

            ' 取得所有涉及的運輸廠商ID
            Dim transportationIds = transportationMonthlyData.Select(Function(x) Integer.Parse(x.vendorId)).Distinct().ToList

            ' 查詢運費付款資料並手動分組
            Dim transportationPayment = _paymentRep.GetByCriteriaAndVendors(paymentCriteria, transportationIds)
            Dim validTransportationPayments = transportationPayment.Where(Function(x) x.p_m_Id.HasValue).ToList()

            ' 手動分組處理：按年月+運輸廠商分組付款資料
            Dim transportationPaymentGroups As New Dictionary(Of String, Integer)
            For Each payment In validTransportationPayments
                Dim groupKey = $"{payment.p_Date.Year:0000}-{payment.p_Date.Month:00}-{payment.p_m_Id.Value}"
                If Not transportationPaymentGroups.ContainsKey(groupKey) Then
                    transportationPaymentGroups(groupKey) = 0
                End If
                transportationPaymentGroups(groupKey) += payment.p_Amount
            Next

            ' 建立運費月份總結（數據已按廠商+月份分組）
            Dim freightResult As New List(Of PurchaseFreightTradeSummaryListVM)
            For Each monthData In transportationMonthlyData
                Dim paymentKey = $"{monthData.Year:0000}-{monthData.Month:00}-{monthData.vendorId}"
                Dim paidAmount = If(transportationPaymentGroups.ContainsKey(paymentKey), transportationPaymentGroups(paymentKey), 0)

                freightResult.Add(New PurchaseFreightTradeSummaryListVM With {
                    .運輸廠商 = monthData.vendorName,
                    .帳款月份 = monthData.monthString,
                    .運費 = monthData.Amount,
                    .已付金額 = paidAmount,
                    .尚欠金額 = monthData.Amount - paidAmount
                })
            Next

            ' 添加運費廠商總計
            Dim freightTotals = freightResult.GroupBy(Function(x) x.運輸廠商).
                                              Select(Function(g) New PurchaseFreightTradeSummaryListVM With {
                                                  .運輸廠商 = g.Key,
                                                  .帳款月份 = "總計",
                                                  .運費 = g.Sum(Function(x) x.運費),
                                                  .已付金額 = g.Sum(Function(x) x.已付金額),
                                                  .尚欠金額 = g.Sum(Function(x) x.尚欠金額)
                                              }).ToList

            freightResult.AddRange(freightTotals)

            Return Tuple.Create(gasVendorResult, freightResult)

        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
