'------------------------------------------------------------------------------
' <auto-generated>
'     這個程式碼是由範本產生。
'
'     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
'     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class customer
    Public Property cus_id As Integer
    Public Property cus_code As String
    Public Property cus_name As String
    Public Property cus_principal As String
    Public Property cus_contact_person As String
    Public Property cus_phone1 As String
    Public Property cus_phone2 As String
    Public Property cus_fax As String
    Public Property cus_address As String
    Public Property cus_tax_id As String
    Public Property cus_memo As String
    Public Property cus_insurance As String
    Public Property cus_gas_50 As Integer
    Public Property cus_gas_20 As Integer
    Public Property cus_gas_16 As Integer
    Public Property cus_gas_10 As Integer
    Public Property cus_gas_4 As Integer
    Public Property cus_gas_15 As Integer
    Public Property cus_gas_14 As Integer
    Public Property cus_gas_5 As Integer
    Public Property cus_gas_2 As Integer
    Public Property cus_pp_Id As Nullable(Of Integer)

    Public Overridable Property cars As ICollection(Of car) = New HashSet(Of car)
    Public Overridable Property priceplan As priceplan

End Class
