Imports System.Data.Entity.Core.Mapping

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

            ' 取得大氣廠商分組總結
            Dim gasVendorSummary = datas.GroupBy(Function(x) x.pur_manu_id).
                                         Select(Function(g) New With {
                                            .vendorId = g.Key,
                                            .vendorName = g.First.manufacturer.manu_name,
                                            .gasKg = g.Where(Function(x) x.pur_product = "普氣").Sum(Function(x) If(x.pur_quantity, 0)),
                                            .gasAmount = g.Where(Function(x) x.pur_product = "普氣").Sum(Function(x) If(x.pur_price, 0)),
                                            .gas_cKg = g.Where(Function(x) x.pur_product = "丙氣").Sum(Function(x) If(x.pur_quantity, 0)),
                                            .gas_cAmount = g.Where(Function(x) x.pur_product = "丙氣").Sum(Function(x) If(x.pur_price, 0))
                                         }).ToList
            ' 取得涉及的大氣廠商ID
            Dim gasVendorIds = gasVendorSummary.Select(Function(p) p.vendorId).ToList()

            ' 查詢大氣付款資料
            Dim gasVendorPayment = _paymentRep.GetByCriteriaAndVendors(paymentCriteria, gasVendorIds)
            Dim gasPaymentSummary = gasVendorPayment.GroupBy(Function(x) x.p_m_Id).
                                                     Select(Function(g) New With {
                                                        .vendorId = g.Key,
                                                        .paid = g.Sum(Function(x) x.p_Amount)
                                                     }).ToDictionary(Function(x) x.vendorId, Function(x) x.paid)
            ' 建立大氣廠商總結
            Dim gasVendorResult = gasVendorSummary.Select(Function(x) New PurchaseGasVendorTradeSummaryListVM With {
                .廠商名稱 = x.vendorName,
                .普氣Kg = x.gasKg,
                .普氣金額 = x.gasAmount,
                .丙氣Kg = x.gas_cKg,
                .丙氣金額 = x.gas_cAmount,
                .總計 = x.gasAmount + x.gas_cAmount,
                .已付金額 = If(gasPaymentSummary.ContainsKey(x.vendorId), gasPaymentSummary(x.vendorId), 0),
                .尚欠金額 = CInt(x.gasAmount + x.gas_cAmount) - If(gasPaymentSummary.ContainsKey(x.vendorId), gasPaymentSummary(x.vendorId), 0)
            }).ToList

            ' 取得運輸廠商分組總結
            Dim transportationSummary = datas.Where(Function(x) x.pur_DriveCmpId IsNot Nothing).
                                              GroupBy(Function(x) x.pur_DriveCmpId).
                                              Select(Function(g) New With {
                                                .vendorId = g.Key,
                                                .vendorName = g.First.manufacturer1.manu_name,
                                                .Amount = g.Sum(Function(x) x.pur_delivery_fee)
                                              }).ToList
            ' 取得涉及的運輸廠商ID
            Dim transportationIds = transportationSummary.Select(Function(x) x.vendorId.Value).ToList

            ' 查詢運費付款資料
            Dim transportationPayment = _paymentRep.GetByCriteriaAndVendors(paymentCriteria, transportationIds)
            Dim transportationPaymentSummary = transportationPayment.GroupBy(Function(x) x.p_m_Id).
                                                                     Select(Function(g) New With {
                                                                        .vendorId = g.Key,
                                                                        .paid = g.Sum(Function(x) x.p_Amount)
                                                                     }).ToDictionary(Function(x) x.vendorId, Function(x) x.paid)
            ' 建立運費總結
            Dim freightResult = transportationSummary.Select(Function(x) New PurchaseFreightTradeSummaryListVM With {
                .運輸廠商 = x.vendorName,
                .運費 = x.Amount,
                .已付金額 = If(transportationPaymentSummary.ContainsKey(x.vendorId), transportationPaymentSummary(x.vendorId), 0),
                .尚欠金額 = x.Amount - If(transportationPaymentSummary.ContainsKey(x.vendorId), transportationPaymentSummary(x.vendorId), 0)
            }).ToList

            Return Tuple.Create(gasVendorResult, freightResult)

        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
