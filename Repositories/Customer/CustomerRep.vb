Imports System.Data.Entity

Public Class CustomerRep
    Inherits Repository(Of customer)
    Implements ICustomerRep

    Public Sub New(context As gas_accounting_systemEntities)
        MyBase.New(context)
    End Sub

    Public Async Function SearchAsync(criteria As customer) As Task(Of List(Of customer)) Implements ICustomerRep.SearchAsync
        Try

            Dim query = _dbSet.AsNoTracking.AsQueryable

            If Not String.IsNullOrEmpty(criteria.cus_code) Then query = query.Where(Function(x) x.cus_code.Contains(criteria.cus_code))
            If Not String.IsNullOrEmpty(criteria.cus_name) Then query = query.Where(Function(x) x.cus_name.Contains(criteria.cus_name))
            If Not String.IsNullOrEmpty(criteria.cus_contact_person) Then query = query.Where(Function(x) x.cus_contact_person.Contains(criteria.cus_contact_person))
            If Not String.IsNullOrEmpty(criteria.cus_phone1) Then
                query = query.Where(Function(x) x.cus_phone1.Contains(criteria.cus_phone1))
                query = query.Where(Function(x) x.cus_phone2.Contains(criteria.cus_phone2))
            End If
            If criteria.cus_comp_Id IsNot Nothing Then query = query.Where(Function(x) x.cus_comp_Id = criteria.cus_comp_Id)

            Return Await query.ToListAsync
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetCustomerById(id As Integer) As customer Implements ICustomerRep.GetCustomerById
        Try
            Using db As New gas_accounting_systemEntities
                Return db.customers.Find(id)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetByCusCode(cusCode As String) As customer Implements ICustomerRep.GetByCusCode
        Try
            Return _dbSet.FirstOrDefault(Function(x) x.cus_code = cusCode)
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class