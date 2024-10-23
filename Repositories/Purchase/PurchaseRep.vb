Imports System.Data.Entity

Public Class PurchaseRep
    Inherits Repository(Of purchase)
    Implements IPurchaseRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function SearchPurchasesAsync(conditions As PurchaseCondition) As Task(Of IEnumerable(Of purchase)) Implements IPurchaseRep.SearchPurchasesAsync
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If conditions.CompanyId <> 0 Then query = query.Where(Function(x) x.pur_comp_id = conditions.CompanyId)
            If conditions.ManufacturerId <> 0 Then query = query.Where(Function(x) x.pur_manu_id = conditions.ManufacturerId)
            If Not String.IsNullOrEmpty(conditions.Product) Then query = query.Where(Function(x) x.pur_product = conditions.Product)
            If conditions.IsDateSearch Then query = query.Where(Function(x) x.pur_date >= conditions.StartDate AndAlso x.pur_date <= conditions.EndDate)

            Return Await query.ToListAsync()
        Catch ex As Exception
            Throw New Exception("取得purchase列表時發生錯誤", ex)
        End Try
    End Function

    Public Async Function GetDefaultPricesAsync(manuId As Integer, productName As String) As Task(Of Tuple(Of Single, Single)) Implements IPurchaseRep.GetDefaultPricesAsync
        Try
            Dim unitPriceQuery = Await _dbSet.Where(Function(x) x.pur_manu_id = manuId AndAlso x.pur_product = productName AndAlso Not x.pur_SpecialUP).
                                              OrderByDescending(Function(x) x.pur_date).
                                              ThenByDescending(Function(x) x.pur_id).
                                              FirstOrDefaultAsync
            Dim deliveryUnitPriceQuery = Await _dbSet.Where(Function(x) x.pur_manu_id = manuId AndAlso x.pur_product = productName AndAlso Not x.pur_SpecialDUP).
                                                    OrderByDescending(Function(x) x.pur_date).
                                                    ThenByDescending(Function(x) x.pur_id).
                                                    FirstOrDefaultAsync

            Dim unitPrice As Double = If(unitPriceQuery IsNot Nothing, unitPriceQuery.pur_unit_price, 0)
            Dim deliveryUnitPrice As Double = If(deliveryUnitPriceQuery IsNot Nothing, deliveryUnitPriceQuery.pur_deli_unit_price, 0)

            Return New Tuple(Of Single, Single)(Math.Round(unitPrice, 1), Math.Round(deliveryUnitPrice, 1))
        Catch ex As Exception
            Throw New Exception("取得預設價格時發生錯誤", ex)
        End Try
    End Function

    Public Async Function GetPurchasesWithoutCheckoutAsync(input As PurchaseCondition) As Task(Of IEnumerable(Of purchase)) Implements IPurchaseRep.GetPurchasesWithoutCheckoutAsync
        Try
            Dim query = _dbSet.Where(Function(x) Not x.pur_Checkout)
            If input.IsDateSearch Then query = query.Where(Function(x) x.pur_date >= input.StartDate AndAlso x.pur_date < input.EndDate)
            If input.ManufacturerId <> 0 Then query = query.Where(Function(x) x.pur_manu_id = input.ManufacturerId)
            Return Await query.ToListAsync
        Catch ex As Exception
            Throw New Exception("取得未結帳訂貨資訊錯誤", ex)
        End Try
    End Function

    Public Async Function UpdateCheckoutStatusAsync(ids As List(Of Integer)) As Task(Of Integer) Implements IPurchaseRep.UpdateCheckoutStatusAsync
        Try
            Dim purchases = Await _dbSet.Where(Function(x) ids.Contains(x.pur_id)).ToListAsync
            purchases.ForEach(Sub(x) x.pur_Checkout = True)
            Return Await _context.SaveChangesAsync
        Catch ex As Exception
            Throw New Exception("更新結帳狀態時發生錯誤", ex)
        End Try
    End Function
End Class