﻿'------------------------------------------------------------------------------
' <auto-generated>
'     這個程式碼是由範本產生。
'
'     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
'     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure

Partial Public Class gas_accounting_systemEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=gas_accounting_systemEntities")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Overridable Property banks() As DbSet(Of bank)
    Public Overridable Property basic_price() As DbSet(Of basic_price)
    Public Overridable Property cars() As DbSet(Of car)
    Public Overridable Property cheques() As DbSet(Of cheque)
    Public Overridable Property collections() As DbSet(Of collection)
    Public Overridable Property companies() As DbSet(Of company)
    Public Overridable Property customers() As DbSet(Of customer)
    Public Overridable Property employees() As DbSet(Of employee)
    Public Overridable Property journals() As DbSet(Of journal)
    Public Overridable Property manufacturers() As DbSet(Of manufacturer)
    Public Overridable Property orders() As DbSet(Of order)
    Public Overridable Property payments() As DbSet(Of payment)
    Public Overridable Property priceplans() As DbSet(Of priceplan)
    Public Overridable Property purchases() As DbSet(Of purchase)
    Public Overridable Property roles() As DbSet(Of role)
    Public Overridable Property stocks() As DbSet(Of stock)
    Public Overridable Property subjects() As DbSet(Of subject)

End Class
