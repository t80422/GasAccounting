Imports System.Data.Entity

''' <summary>
''' 銷帳
''' </summary>
Public Class WriteOffRep
    Public ReadOnly _db As gas_accounting_systemEntities
    Public ReadOnly _dbSet As DbSet(Of write_off)

    Public Sub New(Optional db As gas_accounting_systemEntities = Nothing)
        If db Is Nothing Then db = New gas_accounting_systemEntities()

        _db = db
        _dbSet = db.Set(Of write_off)
    End Sub

    ''' <summary>
    ''' 取得該月度帳單管理的銷帳總金額
    ''' </summary>
    ''' <param name="maId">月度帳單管理編號</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByMonthlyAccount(maId As Integer) As Integer
        Dim query = _dbSet.Where(Function(x) x.wo_ma_id = maId)
        Return If(query.Any(), query.Sum(Function(x) x.wo_amount), 0)
    End Function

    Public Function GetByMonthlyAccountAndCollection(maId As Integer, colId As Integer) As write_off
        Return _dbSet.FirstOrDefault(Function(x) x.wo_ma_id = maId And x.wo_col_id = colId)
    End Function

    Public Function GetTotalWriteOffByCollection(colId As Integer) As Integer
        Dim query = _dbSet.Where(Function(x) x.wo_col_id = colId)
        Return If(query.Any(), query.Sum(Function(x) x.wo_amount), 0)
    End Function
End Class
