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

Partial Public Class payment
    Public Property p_Id As Integer
    Public Property p_Date As Date
    Public Property p_Amount As Integer
    Public Property p_Type As String
    Public Property p_m_Id As Nullable(Of Integer)
    Public Property p_Memo As String
    Public Property p_Cheque As String
    Public Property p_bank_Id As Nullable(Of Integer)
    Public Property p_s_Id As Nullable(Of Integer)
    Public Property p_AccountMonth As Nullable(Of Date)
    Public Property p_comp_Id As Nullable(Of Integer)
    Public Property p_SubpoenaNo As Integer

    Public Overridable Property bank As bank
    Public Overridable Property company As company
    Public Overridable Property manufacturer As manufacturer
    Public Overridable Property subject As subject

End Class
