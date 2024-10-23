Public Class PurBarrelPresenter
    Private ReadOnly _view As IPurchaseBarrelView
    Private ReadOnly _pbRep As IPurchaseBarrelRep
    Private ReadOnly _vendorRep As IManufacturerRep
    Private ReadOnly _barmbSer As IBarrelMonthlyBalanceService
    Private ReadOnly _compRep As ICompanyRep
    Private ReadOnly _aeSer As IAccountingEntryService

    Public Sub New(view As IPurchaseBarrelView, pbRep As IPurchaseBarrelRep, vendorRep As IManufacturerRep, barmbSer As IBarrelMonthlyBalanceService, compRep As ICompanyRep,
                   aeSer As IAccountingEntryService)
        _view = view
        _pbRep = pbRep
        _vendorRep = vendorRep
        _barmbSer = barmbSer
        _compRep = compRep
        _aeSer = aeSer
    End Sub

    Public Async Sub Initialize()
        Await LoadVendor()
        Await LoadCompany()
        _view.ClearInput()
        LoadList()
    End Sub

    Public Async Sub LoadList()
        Try
            Dim criteria = _view.GetSearchCriteria
            Dim datas = Await _pbRep.SearchAsync(criteria)
            _view.DisplayList(datas.Select(Function(x) New PurchaseBarrelVM(x)).ToList)
            _view.ClearInput()
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Add()
        Using transactions = _pbRep.BeginTransaction
            Try
                Dim input = _view.GetUserInput
                Validate(input)

                Dim insert = Await _pbRep.AddAsync(input)
                Await _barmbSer.UpdateOrAddAsync(input.pb_Date)

                Dim entries = New List(Of accounting_entry) From {
                    New accounting_entry With {
                        .ae_TransactionId = insert.pb_Id,
                        .ae_Date = Now,
                        .ae_TransactionType = "新瓶採購",
                        .ae_s_Id = 4,
                        .ae_Debit = insert.pb_Amount,
                        .ae_Credit = 0
                    },
                    New accounting_entry With {
                        .ae_TransactionId = insert.pb_Id,
                        .ae_Date = Now,
                        .ae_TransactionType = "新瓶採購",
                        .ae_s_Id = 3,
                        .ae_Debit = 0,
                        .ae_Credit = insert.pb_Amount
                    }
                }

                _aeSer.AddEntries(entries)

                transactions.Commit()
                _view.ClearInput()
                LoadList()
                MsgBox("新增成功")
            Catch ex As Exception
                transactions.Rollback()
                Console.WriteLine(ex.StackTrace)
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub LoadDetail(id As Integer)
        Try
            Dim data = Await _pbRep.GetByIdAsync(id)
            _view.ClearInput()
            _view.DisplayDetail(data)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Update()
        Using transactions = _pbRep.BeginTransaction
            Try
                Dim input = _view.GetUserInput
                Validate(input)

                Await _pbRep.UpdateAsync(input.pb_Id, input)
                Await _barmbSer.UpdateOrAddAsync(input.pb_Date)
                Dim entries = New List(Of accounting_entry) From {
                    New accounting_entry With {
                        .ae_TransactionId = input.pb_Id,
                        .ae_Date = Now,
                        .ae_TransactionType = "新瓶採購",
                        .ae_s_Id = 4,
                        .ae_Debit = input.pb_Amount,
                        .ae_Credit = 0
                    },
                    New accounting_entry With {
                        .ae_TransactionId = input.pb_Id,
                        .ae_Date = Now,
                        .ae_TransactionType = "新瓶採購",
                        .ae_s_Id = 3,
                        .ae_Debit = 0,
                        .ae_Credit = input.pb_Amount
                    }
                }

                _aeSer.UpdateEntries(entries)

                transactions.Commit()
                _view.ClearInput()
                LoadList()
                MsgBox("修改成功")
            Catch ex As Exception
                transactions.Rollback()
                Console.WriteLine(ex.StackTrace)
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub Delete()
        Using transactions = _pbRep.BeginTransaction
            Try
                Dim input = _view.GetUserInput

                Await _pbRep.DeleteAsync(input.pb_Id)
                Await _barmbSer.UpdateOrAddAsync(input.pb_Date)
                _aeSer.DeleteEntries("新瓶採購", input.pb_Id)
                Await _pbRep.SaveChangesAsync()
                transactions.Commit()
                _view.ClearInput()
                LoadList()
                MsgBox("刪除成功")
            Catch ex As Exception
                transactions.Rollback()
                Console.WriteLine(ex.StackTrace)
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Private Sub Validate(input As purchase_barrel)
        If input.pb_manu_Id = 0 Then Throw New Exception("請選擇廠商")
    End Sub

    Private Async Function LoadVendor() As Task
        Try
            Dim datas = Await _vendorRep.GetVendorDropdownAsync
            _view.SetVendorCmb(datas.ToList)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Function

    Private Async Function LoadCompany() As Task
        Try
            Dim datas = Await _compRep.GetCompanyDropdownAsync
            _view.SetCompanyCmb(datas.ToList)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Function
End Class