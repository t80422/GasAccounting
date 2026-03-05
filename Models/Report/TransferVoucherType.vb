''' <summary>
''' 轉帳傳票類型：決定一借多貸（收入）或一貸多借（支出）的合併與呈現規則。
''' </summary>
Public Enum TransferVoucherType
    ''' <summary>收入：一借多貸，借方合併儲存格</summary>
    Income = 0
    ''' <summary>支出：一貸多借，貸方合併儲存格</summary>
    Expense = 1
End Enum
