Public Class BankPresenter
    Inherits BasePresenter(Of bank, BankVM, IBankView)
    Implements IPresenter(Of bank, BankVM)

    Public Sub New(view As IBankView)
        MyBase.New(view)
        _presenter = Me
    End Sub

    Public Overrides Sub Add()
        If Not CheckRequired(_view.SetRequired()) Then Exit Sub

        Dim data = _view.GetUserInput

        If data IsNot Nothing Then
            Try
                Using db As New gas_accounting_systemEntities
                    UpdateCurrentBalance(data)
                    db.banks.Add(data)
                    db.SaveChanges()
                    LoadList()
                    _view.ClearInput()
                    MsgBox("新增成功")
                End Using

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Public Overrides Sub Edit(id As Integer)
        If Not CheckRequired(_view.SetRequired()) Then Exit Sub

        Dim data = _view.GetUserInput

        Try
            Using db As New gas_accounting_systemEntities
                If data IsNot Nothing Then
                    UpdateCurrentBalance(data)

                    Dim existingSubject = db.banks.Find(id)

                    If existingSubject IsNot Nothing Then
                        db.Entry(existingSubject).CurrentValues.SetValues(data)
                        db.SaveChanges()
                        LoadList()
                        MsgBox("修改成功。")
                    Else
                        MsgBox("未找到指定的對象。")
                    End If
                End If

            End Using

        Catch ex As Exception
            MsgBox("修改時發生錯誤: " & ex.Message)
        End Try
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of bank), conditions As Object) As IQueryable(Of bank) Implements IPresenter(Of bank, BankVM).SetSearchConditions
        Return Nothing
    End Function

    Public Function SetListViewModel(query As IQueryable(Of bank)) As List(Of BankVM) Implements IPresenter(Of bank, BankVM).SetListViewModel
        Return query.Select(Function(x) New BankVM With {
            .初始資金 = x.bank_InitialBalance,
            .名稱 = x.bank_name,
            .編號 = x.bank_id,
            .餘額 = x.bank_CurrentBalance
        }).ToList
    End Function

    Private Sub UpdateCurrentBalance(data As bank)
        Try
            If data.bank_id = 0 Then
                data.bank_CurrentBalance = data.bank_InitialBalance
                Return

            Else
                Using db As New gas_accounting_systemEntities
                    Dim bank = db.banks.Find(data.bank_id)
                    Dim orgInitial = bank.bank_InitialBalance
                    data.bank_CurrentBalance += (data.bank_InitialBalance - orgInitial)
                End Using

                Return
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
