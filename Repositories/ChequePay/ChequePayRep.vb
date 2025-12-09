Imports System.Data.Entity

Public Class ChequePayRep
    Inherits Repository(Of chque_pay)
    Implements IChequePayRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetByChequeNumber(chequeNumber As String) As chque_pay Implements IChequePayRep.GetByChequeNumber
        Try
            Return _dbSet.FirstOrDefault(Function(x) x.cp_Number = chequeNumber)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetList(Optional criteria As ChequeSC = Nothing) As List(Of ChequePayVM) Implements IChequePayRep.GetList
        Try
            Dim query = _dbSet.Join(_context.payments, Function(cp) cp.cp_Id, Function(p) p.p_cp_Id, Function(cp, p) New With {cp, p}).
                AsNoTracking().
                AsQueryable()

            If criteria IsNot Nothing Then
                If criteria.IsDate Then query = query.Where(Function(x) x.cp.cp_Date >= criteria.StartDate AndAlso x.cp.cp_Date <= criteria.EndDate)
                If Not String.IsNullOrEmpty(criteria.Status) Then
                    If criteria.Status = "已兌現" Then
                        query = query.Where(Function(x) x.cp.cp_IsCashing.HasValue AndAlso x.cp.cp_IsCashing.Value)
                    End If
                End If
                If criteria.CompanyId.HasValue Then query = query.Where(Function(x) x.p.p_comp_Id = criteria.CompanyId.Value)
                If Not String.IsNullOrEmpty(criteria.ChequeNumber) Then query = query.Where(Function(x) x.cp.cp_Number = criteria.ChequeNumber)
            End If

            Dim result = query.OrderByDescending(Function(x) x.cp.cp_Id).ToList

            Return result.Select(Function(x) New ChequePayVM With {
                .備註 = x.cp.cp_Memo,
                .對方銀行 = x.p.manufacturer?.manu_account,
                .廠商名稱 = x.p.manufacturer?.manu_name,
                .銀行帳號 = x.p.bank?.bank_Account,
                .日期 = x.cp.cp_Date,
                .編號 = x.cp.cp_Id,
                .金額 = x.cp.cp_Amount,
                .支票號碼 = x.cp.cp_Number,
                .銀行兌現日期 = x.cp.cp_BankCashing?.ToString("yyyy/MM/dd"),
                .支票兌現日期 = x.cp.cp_CashingDate?.ToString("yyyy/MM/dd"),
                .是否兌現 = If(x.cp.cp_IsCashing.HasValue, x.cp.cp_IsCashing.Value.ToString, "")
            }).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
