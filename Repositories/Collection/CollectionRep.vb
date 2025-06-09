Imports System.Data.Entity

Public Class CollectionRep
    Inherits Repository(Of collection)
    Implements ICollectionRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Function GetList(Optional criteria As CollectionSearchCriteria = Nothing) As List(Of CollectionVM) Implements ICollectionRep.GetList
        Try
            Dim query = _dbSet.AsNoTracking.AsQueryable

            If criteria IsNot Nothing Then
                ' 日期範圍
                If criteria.IsDate Then
                    Dim startDate = criteria.StartDate.Value.Date
                    Dim endDate = criteria.EndDate.Value.Date.AddDays(1)
                    query = query.Where(Function(x) x.col_Date >= startDate AndAlso x.col_Date < endDate)
                End If

                ' 客戶編號
                If criteria.CusId <> 0 Then query = query.Where(Function(x) x.col_cus_Id = criteria.CusId)

                ' 科目編號
                If criteria.SubjectId.HasValue Then query = query.Where(Function(x) x.col_s_Id = criteria.SubjectId)

                ' 收款類型
                If Not String.IsNullOrEmpty(criteria.Type) Then query = query.Where(Function(x) x.col_Type = criteria.Type)

                ' 支票號碼
                If Not String.IsNullOrEmpty(criteria.ChequeNum) Then query = query.Where(Function(x) x.col_Cheque = criteria.ChequeNum)
            End If

            Return query.OrderByDescending(Function(x) x.col_Date).
                         ThenByDescending(Function(x) x.col_Id).ToList.
                         Select(Function(x) New CollectionVM(x)).ToList
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Async Function GetByBankAndMonthAsync(bankId As Integer, month As Date) As Task(Of IEnumerable(Of collection)) Implements ICollectionRep.GetByBankAndMonthAsync
        Try
            Return Await _dbSet.AsNoTracking.Where(Function(x) x.col_AccountMonth.Year = month.Year AndAlso x.col_AccountMonth.Month = month.Month AndAlso x.col_bank_Id = bankId).
                                             ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetSubpoenaData(day As Date, Optional isCash As Boolean = False) As List(Of SubpoenaDTO) Implements ICollectionRep.GetSubpoenaData
        Try
            Dim query = _dbSet.Where(Function(x) x.col_Date = day)

            Dim result = If(isCash, query.Where(Function(x) x.col_Type = "現金"), query.Where(Function(x) x.col_Type = "銀行" Or x.col_Type = "支票")).
                Select(Function(x) New SubpoenaDTO With {
                    .SubjectName = x.subject.s_name,
                    .Amount = x.col_Amount,
                    .Summary = x.col_Memo,
                    .Code = x.customer.cus_code
                }).ToList

            Return result
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class