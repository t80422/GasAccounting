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

    Public Async Function RecalculateInventoryAsync(gasBarrelRep As IGasBarrelRep, pbRep As IPurchaseBarrelRep, orderRep As IOrderRep) As Task Implements IBarrelInventoryService.RecalculateInventoryAsync
        If gasBarrelRep Is Nothing Then Throw New ArgumentNullException(NameOf(gasBarrelRep))
        If pbRep Is Nothing Then Throw New ArgumentNullException(NameOf(pbRep))
        If orderRep Is Nothing Then Throw New ArgumentNullException(NameOf(orderRep))

        Dim allBarrels = Await gasBarrelRep.GetAllAsync()

        Dim allPurchases = Await pbRep.GetAllAsync()
        Dim totalPurchase As New Dictionary(Of String, Integer)
        For Each barrel In allBarrels
            totalPurchase(barrel.gb_Name) = 0
        Next
        For Each purchase In allPurchases
            Dim dict = BuildQtyDictionary(purchase)
            For Each kv In dict
                If totalPurchase.ContainsKey(kv.Key) Then
                    totalPurchase(kv.Key) += kv.Value
                End If
            Next
        Next

        Dim allOrders = Await orderRep.GetAllAsync()
        Dim totalSales As New Dictionary(Of String, Integer)
        For Each barrel In allBarrels
            totalSales(barrel.gb_Name) = 0
        Next
        For Each ord In allOrders
            Dim dict = BuildOrderNewOutDictionary(ord)
            For Each kv In dict
                If totalSales.ContainsKey(kv.Key) Then
                    totalSales(kv.Key) += kv.Value
                End If
            Next
        Next

        For Each barrel In allBarrels
            Dim name = barrel.gb_Name
            Dim purchase As Integer = If(totalPurchase.ContainsKey(name), totalPurchase(name), 0)
            Dim sales As Integer = If(totalSales.ContainsKey(name), totalSales(name), 0)
            Dim newInventory As Integer = barrel.gb_InitialInventory + purchase - sales
            Await gasBarrelRep.SetInventoryAsync(name, newInventory)
        Next
    End Function

    Private Shared Async Function ApplyDeltaAsync(gasBarrelRep As IGasBarrelRep, delta As Dictionary(Of String, Integer)) As Task
        For Each kv In delta
            If kv.Value = 0 Then Continue For
            Await gasBarrelRep.UpdateInventoryByDeltaAsync(kv.Key, kv.Value)
        Next
    End Function

    Private Shared Function SafeQty(value As Integer?) As Integer
        If value.HasValue Then
            Return value.Value
        End If

        Return 0
    End Function
End Class

