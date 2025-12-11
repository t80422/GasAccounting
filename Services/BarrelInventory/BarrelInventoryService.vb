Imports System.Data.Entity
Imports System.Linq

Public Class BarrelInventoryService
    Implements IBarrelInventoryService

    Public Sub New()
    End Sub

    Public Async Function ApplyPurchaseAsync(gasBarrelRep As IGasBarrelRep, purchase As purchase_barrel) As Task Implements IBarrelInventoryService.ApplyPurchaseAsync
        If gasBarrelRep Is Nothing Then Throw New ArgumentNullException(NameOf(gasBarrelRep))
        If purchase Is Nothing Then Throw New ArgumentNullException(NameOf(purchase))

        Dim delta = BuildQtyDictionary(purchase)
        Await ApplyDeltaAsync(gasBarrelRep, delta)
    End Function

    Public Async Function ApplyUpdateAsync(gasBarrelRep As IGasBarrelRep, originalPurchase As purchase_barrel, updatedPurchase As purchase_barrel) As Task Implements IBarrelInventoryService.ApplyUpdateAsync
        If gasBarrelRep Is Nothing Then Throw New ArgumentNullException(NameOf(gasBarrelRep))
        If originalPurchase Is Nothing Then Throw New ArgumentNullException(NameOf(originalPurchase))
        If updatedPurchase Is Nothing Then Throw New ArgumentNullException(NameOf(updatedPurchase))

        Dim origin = BuildQtyDictionary(originalPurchase)
        Dim updated = BuildQtyDictionary(updatedPurchase)
        Dim delta = origin.ToDictionary(Function(x) x.Key, Function(x) updated(x.Key) - x.Value)

        Await ApplyDeltaAsync(gasBarrelRep, delta)
    End Function

    Public Async Function ApplyDeleteAsync(gasBarrelRep As IGasBarrelRep, purchase As purchase_barrel) As Task Implements IBarrelInventoryService.ApplyDeleteAsync
        If gasBarrelRep Is Nothing Then Throw New ArgumentNullException(NameOf(gasBarrelRep))
        If purchase Is Nothing Then Throw New ArgumentNullException(NameOf(purchase))

        Dim delta = BuildQtyDictionary(purchase).ToDictionary(Function(x) x.Key, Function(x) -x.Value)
        Await ApplyDeltaAsync(gasBarrelRep, delta)
    End Function

    Public Async Function ApplyOrderIssueAsync(gasBarrelRep As IGasBarrelRep, ord As order) As Task Implements IBarrelInventoryService.ApplyOrderIssueAsync
        If gasBarrelRep Is Nothing Then Throw New ArgumentNullException(NameOf(gasBarrelRep))
        If ord Is Nothing Then Throw New ArgumentNullException(NameOf(ord))

        Dim delta = BuildOrderNewOutDictionary(ord).ToDictionary(Function(x) x.Key, Function(x) -x.Value)
        Await ApplyDeltaAsync(gasBarrelRep, delta)
    End Function

    Public Async Function ApplyOrderIssueUpdateAsync(gasBarrelRep As IGasBarrelRep, originalOrder As order, updatedOrder As order) As Task Implements IBarrelInventoryService.ApplyOrderIssueUpdateAsync
        If gasBarrelRep Is Nothing Then Throw New ArgumentNullException(NameOf(gasBarrelRep))
        If originalOrder Is Nothing Then Throw New ArgumentNullException(NameOf(originalOrder))
        If updatedOrder Is Nothing Then Throw New ArgumentNullException(NameOf(updatedOrder))

        Dim origin = BuildOrderNewOutDictionary(originalOrder)
        Dim updated = BuildOrderNewOutDictionary(updatedOrder)
        Dim delta = origin.ToDictionary(Function(x) x.Key, Function(x) -(updated(x.Key) - x.Value))

        Await ApplyDeltaAsync(gasBarrelRep, delta)
    End Function

    Public Async Function ApplyOrderIssueDeleteAsync(gasBarrelRep As IGasBarrelRep, ord As order) As Task Implements IBarrelInventoryService.ApplyOrderIssueDeleteAsync
        If gasBarrelRep Is Nothing Then Throw New ArgumentNullException(NameOf(gasBarrelRep))
        If ord Is Nothing Then Throw New ArgumentNullException(NameOf(ord))

        Dim delta = BuildOrderNewOutDictionary(ord) ' 刪除時把先前扣掉的量加回去
        Await ApplyDeltaAsync(gasBarrelRep, delta)
    End Function

    Private Shared Function BuildQtyDictionary(purchase As purchase_barrel) As Dictionary(Of String, Integer)
        Return New Dictionary(Of String, Integer) From {
            {"50", SafeQty(purchase.pb_Qty_50)},
            {"20", SafeQty(purchase.pb_Qty_20)},
            {"16", SafeQty(purchase.pb_Qty_16)},
            {"10", SafeQty(purchase.pb_Qty_10)},
            {"4", SafeQty(purchase.pb_Qty_4)},
            {"18", SafeQty(purchase.pb_Qty_18)},
            {"14", SafeQty(purchase.pb_Qty_14)},
            {"5", SafeQty(purchase.pb_Qty_5)},
            {"2", SafeQty(purchase.pb_Qty_2)}
        }
    End Function

    Private Shared Function BuildOrderNewOutDictionary(ord As order) As Dictionary(Of String, Integer)
        Return New Dictionary(Of String, Integer) From {
            {"50", SafeQty(ord.o_new_in_50)},
            {"20", SafeQty(ord.o_new_in_20)},
            {"16", SafeQty(ord.o_new_in_16)},
            {"10", SafeQty(ord.o_new_in_10)},
            {"4", SafeQty(ord.o_new_in_4)},
            {"18", SafeQty(ord.o_new_in_18)},
            {"14", SafeQty(ord.o_new_in_14)},
            {"5", SafeQty(ord.o_new_in_5)},
            {"2", SafeQty(ord.o_new_in_2)}
        }
    End Function

    Private Shared Async Function ApplyDeltaAsync(gasBarrelRep As IGasBarrelRep, delta As Dictionary(Of String, Integer)) As Task
        Dim targetNames = delta.Where(Function(x) x.Value <> 0).Select(Function(x) x.Key).ToList()
        If Not targetNames.Any() Then Return

        Dim ctx = gasBarrelRep.Context.Set(Of gas_barrel)()
        Dim barrels = Await ctx.Where(Function(x) targetNames.Contains(x.gb_Name)).ToListAsync()
        Dim missing = targetNames.Except(barrels.Select(Function(x) x.gb_Name)).ToList()
        If missing.Any() Then Throw New Exception($"找不到瓦斯桶類型：{String.Join(", ", missing)}")

        For Each kv In delta
            If kv.Value = 0 Then Continue For
            Dim entity = barrels.First(Function(x) x.gb_Name = kv.Key)
            entity.gb_Inventory += kv.Value
        Next

        Await gasBarrelRep.SaveChangesAsync()
    End Function

    Private Shared Function SafeQty(value As Integer?) As Integer
        If value.HasValue Then
            Return value.Value
        End If

        Return 0
    End Function
End Class

