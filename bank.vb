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

Partial Public Class bank
    Public Property bank_id As Integer
    Public Property bank_name As String
    Public Property bank_InitialBalance As Integer
    Public Property bank_CurrentBalance As Integer

    Public Overridable Property payments As ICollection(Of payment) = New HashSet(Of payment)

End Class
