Imports System.IO

Public Class PurBarrelPresenter
    Private ReadOnly _view As IPurchaseBarrelView
    Private ReadOnly _pbRep As IPurchaseBarrelRep
    Private ReadOnly _vendorRep As IManufacturerRep
    Private ReadOnly _barmbSer As IBarrelMonthlyBalanceService
    Private ReadOnly _compRep As ICompanyRep
    Private ReadOnly _aeSer As IAccountingEntryService
    Private ReadOnly _barrelInvSer As IBarrelInventoryService

    Public ReadOnly Property View
        Get
            Return _view
        End Get
    End Property

    Public Sub New(view As IPurchaseBarrelView, pbRep As IPurchaseBarrelRep, vendorRep As IManufacturerRep, barmbSer As IBarrelMonthlyBalanceService, compRep As ICompanyRep,
                   aeSer As IAccountingEntryService, barrelInvSer As IBarrelInventoryService)
        _view = view
        _pbRep = pbRep
        _vendorRep = vendorRep
        _barmbSer = barmbSer
        _compRep = compRep
        _aeSer = aeSer
        _barrelInvSer = barrelInvSer

        ' 訂閱 View 的事件
        SubscribeToViewEvents()
    End Sub

    Private Sub SubscribeToViewEvents()
        AddHandler _view.CreateRequest, AddressOf Add
        AddHandler _view.UpdateRequest, AddressOf Update
        AddHandler _view.DeleteRequest, AddressOf Delete
        AddHandler _view.CancelRequest, AddressOf Reset
        AddHandler _view.DataSelectedRequest, AddressOf Detail
        AddHandler _view.PrintRequested, AddressOf Print
    End Sub

    Private Sub Reset()
        Initialize()
        _view.ButtonStatus(False)
    End Sub

    Private Sub Detail(sender As Object, id As Integer)
        LoadDetail(id)
        _view.ButtonStatus(True)
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
            _view.ShowList(datas.Select(Function(x) New PurchaseBarrelVM(x)).ToList)
            _view.ClearInput()
            GetBarrelInventory()
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Add()
        Using uow As New UnitOfWork
            uow.BeginTransaction()

            Try
                Dim input As New purchase_barrel
                _view.GetInput(input)
                Validate(input)

                Await uow.PurchaseBarrelRep.AddAsync(input)
                Dim barmSer As New BarrelMonthlyBalanceService(uow.BarrelMonthlyBalanceRep, uow.GasBarrelRepository, uow.PurchaseBarrelRep, uow.OrderRepository)
                Await barmSer.UpdateOrAddAsync(input.pb_Date)
                Await _barrelInvSer.ApplyPurchaseAsync(uow.GasBarrelRepository, input)

                'Dim entries = New List(Of accounting_entry) From {
                '    New accounting_entry With {
                '        .ae_TransactionId = insert.pb_Id,
                '        .ae_Date = Now,
                '        .ae_TransactionType = "新瓶採購",
                '        .ae_s_Id = 4,
                '        .ae_Debit = insert.pb_Amount,
                '        .ae_Credit = 0
                '    },
                '    New accounting_entry With {
                '        .ae_TransactionId = insert.pb_Id,
                '        .ae_Date = Now,
                '        .ae_TransactionType = "新瓶採購",
                '        .ae_s_Id = 3,
                '        .ae_Debit = 0,
                '        .ae_Credit = insert.pb_Amount
                '    }
                '}

                '_aeSer.AddEntries(entries)

                uow.Commit()
                _view.ClearInput()
                LoadList()
                MessageBox.Show("新增成功")
            Catch ex As Exception
                uow.Rollback()
                MessageBox.Show(ex.Message)
            End Try
        End Using
    End Sub

    Public Async Sub LoadDetail(id As Integer)
        Try
            Dim data = Await _pbRep.GetByIdAsync(id)
            _view.ClearInput()
            _view.ShowDetail(data)
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub Update()
        Using transactions = _pbRep.BeginTransaction
            Try
                Dim input As New purchase_barrel
                _view.GetInput(input)
                Validate(input)

                Dim existed = Await _pbRep.GetByIdAsync(input.pb_Id)
                If existed Is Nothing Then Throw New Exception("找不到要修改的資料")
                Dim existedQtySnapshot As New purchase_barrel With {
                    .pb_Qty_50 = existed.pb_Qty_50,
                    .pb_Qty_20 = existed.pb_Qty_20,
                    .pb_Qty_16 = existed.pb_Qty_16,
                    .pb_Qty_10 = existed.pb_Qty_10,
                    .pb_Qty_4 = existed.pb_Qty_4,
                    .pb_Qty_18 = existed.pb_Qty_18,
                    .pb_Qty_14 = existed.pb_Qty_14,
                    .pb_Qty_5 = existed.pb_Qty_5,
                    .pb_Qty_2 = existed.pb_Qty_2
                }

                Await _pbRep.UpdateAsync(input.pb_Id, input)
                Await _barmbSer.UpdateOrAddAsync(input.pb_Date)
                Dim gbRep As New GasBarrelRep(CType(_pbRep.Context, gas_accounting_systemEntities))
                Await _barrelInvSer.ApplyUpdateAsync(gbRep, existedQtySnapshot, input)
                'Dim entries = New List(Of accounting_entry) From {
                '    New accounting_entry With {
                '        .ae_TransactionId = input.pb_Id,
                '        .ae_Date = Now,
                '        .ae_TransactionType = "新瓶採購",
                '        .ae_s_Id = 4,
                '        .ae_Debit = input.pb_Amount,
                '        .ae_Credit = 0
                '    },
                '    New accounting_entry With {
                '        .ae_TransactionId = input.pb_Id,
                '        .ae_Date = Now,
                '        .ae_TransactionType = "新瓶採購",
                '        .ae_s_Id = 3,
                '        .ae_Debit = 0,
                '        .ae_Credit = input.pb_Amount
                '    }
                '}

                '_aeSer.UpdateEntries(entries)

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
                Dim input As New purchase_barrel
                _view.GetInput(input)

                Dim existed = Await _pbRep.GetByIdAsync(input.pb_Id)
                If existed Is Nothing Then Throw New Exception("找不到要刪除的資料")
                Dim existedQtySnapshot As New purchase_barrel With {
                    .pb_Qty_50 = existed.pb_Qty_50,
                    .pb_Qty_20 = existed.pb_Qty_20,
                    .pb_Qty_16 = existed.pb_Qty_16,
                    .pb_Qty_10 = existed.pb_Qty_10,
                    .pb_Qty_4 = existed.pb_Qty_4,
                    .pb_Qty_18 = existed.pb_Qty_18,
                    .pb_Qty_14 = existed.pb_Qty_14,
                    .pb_Qty_5 = existed.pb_Qty_5,
                    .pb_Qty_2 = existed.pb_Qty_2
                }

                Await _pbRep.DeleteAsync(input.pb_Id)
                Await _barmbSer.UpdateOrAddAsync(input.pb_Date)
                Dim gbRep As New GasBarrelRep(CType(_pbRep.Context, gas_accounting_systemEntities))
                Await _barrelInvSer.ApplyDeleteAsync(gbRep, existedQtySnapshot)
                '_aeSer.DeleteEntries("新瓶採購", input.pb_Id)
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

    Public Sub Print(sender As Object, datas As List(Of PurchaseBarrelVM))
        Try
            Dim tempPath = Path.Combine(Application.StartupPath, "Report", "新瓶採購範本檔.xlsx")

            Using xml As New CloseXML_Excel(tempPath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim rowIndex As Integer = 3

                    For i As Integer = 0 To datas.Count - 1
                        .WriteToCell(rowIndex + i, 1, datas(i).日期)
                        .WriteToCell(rowIndex + i, 2, datas(i).廠商名稱)
                        .WriteToCell(rowIndex + i, 3, datas(i).公司)
                        .WriteToCell(rowIndex + i, 4, datas(i).數量_50.ToString())
                        .WriteToCell(rowIndex + i, 5, datas(i).單價_50.ToString())
                        .WriteToCell(rowIndex + i, 6, datas(i).數量_20.ToString())
                        .WriteToCell(rowIndex + i, 7, datas(i).單價_20.ToString())
                        .WriteToCell(rowIndex + i, 8, datas(i).數量_16.ToString())
                        .WriteToCell(rowIndex + i, 9, datas(i).單價_16.ToString())
                        .WriteToCell(rowIndex + i, 10, datas(i).數量_10.ToString())
                        .WriteToCell(rowIndex + i, 11, datas(i).單價_10.ToString())
                        .WriteToCell(rowIndex + i, 12, datas(i).數量_4.ToString())
                        .WriteToCell(rowIndex + i, 13, datas(i).單價_4.ToString())
                        .WriteToCell(rowIndex + i, 14, datas(i).數量_18.ToString())
                        .WriteToCell(rowIndex + i, 15, datas(i).單價_18.ToString())
                        .WriteToCell(rowIndex + i, 16, datas(i).數量_14.ToString())
                        .WriteToCell(rowIndex + i, 17, datas(i).單價_14.ToString())
                        .WriteToCell(rowIndex + i, 18, datas(i).數量_5.ToString())
                        .WriteToCell(rowIndex + i, 19, datas(i).單價_5.ToString())
                        .WriteToCell(rowIndex + i, 20, datas(i).數量_2.ToString())
                        .WriteToCell(rowIndex + i, 21, datas(i).單價_2.ToString())
                    Next

                    .SaveExcel("新瓶採購")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
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

    Private Sub GetBarrelInventory()
        Try
            Using uow As New UnitOfWork
                Dim data = uow.GasBarrelRepository.GetAllAsync().Result.ToList
                Dim dto As New GasBarrelDTO
                data.ForEach(Sub(x)
                                 Select Case x.gb_Name
                                     Case "50"
                                         dto.Barrel_50 = x.gb_Inventory
                                     Case "20"
                                         dto.Barrel_20 = x.gb_Inventory
                                     Case "16"
                                         dto.Barrel_16 = x.gb_Inventory
                                     Case "10"
                                         dto.Barrel_10 = x.gb_Inventory
                                     Case "4"
                                         dto.Barrel_4 = x.gb_Inventory
                                     Case "18"
                                         dto.Barrel_18 = x.gb_Inventory
                                     Case "14"
                                         dto.Barrel_14 = x.gb_Inventory
                                     Case "5"
                                         dto.Barrel_5 = x.gb_Inventory
                                     Case "2"
                                         dto.Barrel_2 = x.gb_Inventory
                                 End Select
                             End Sub)
                _view.SetBarrelInventory(dto)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class