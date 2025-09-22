Imports System.IO

''' <summary>
''' 大氣採購
''' </summary>
Public Class PurchasePresenter

    Private ReadOnly _view As IPurchaseView
    Private ReadOnly _purRep As IPurchaseRep
    Private ReadOnly _compRep As ICompanyRep
    Private ReadOnly _manuRep As IManufacturerRep
    Private ReadOnly _subRep As ISubjectRep
    Private ReadOnly _gmbSer As IGasMonthlyBalanceService
    Private ReadOnly _aeSer As IAccountingEntryService
    Private ReadOnly _printerSer As IPrinterService
    Private currentData As purchase

    Public ReadOnly Property View As IPurchaseView
        Get
            Return _view
        End Get
    End Property

    Public Sub New(view As IPurchaseView, purRep As IPurchaseRep, compRep As ICompanyRep, manuRep As IManufacturerRep, subRep As ISubjectRep, gmbSer As IGasMonthlyBalanceService,
                   aeSer As IAccountingEntryService, printerSer As IPrinterService)
        _view = view
        _purRep = purRep
        _compRep = compRep
        _manuRep = manuRep
        _subRep = subRep
        _gmbSer = gmbSer
        _aeSer = aeSer
        _printerSer = printerSer

        AddHandler _view.AddClicked, AddressOf Add
        AddHandler _view.EditClicked, AddressOf Edit
        AddHandler _view.DeleteClicked, AddressOf Delete
        AddHandler _view.CancelClicked, AddressOf Initialize
        AddHandler _view.SerchClicked, AddressOf Search
        AddHandler _view.PrintClicked, AddressOf Print
        AddHandler _view.GasVenderSelected, AddressOf GetDefaultPrice
        AddHandler _view.RowSelected, AddressOf SelectRowAsync
    End Sub

    Private Sub Initialize()
        Try
            _view.ClearInput()
            SetCompanyCmb()
            SetGasVendorCmb()
            SetDriveCompanyCmb()
            currentData = Nothing
            LoadList()
            _view.SetButton(False)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub GetDefaultPrice(sender As Object, data As Tuple(Of Object, Object))
        Try
            Dim result = _purRep.GetDefaultPricesAsync(data.Item1, data.Item2).Result
            _view.SetDefaultPrice(result.Item1, result.Item2)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadList(Optional criteria As PurchaseCondition = Nothing)
        Try
            Dim purchases = _purRep.SearchPurchasesAsync(criteria).Result
            Dim datas = purchases.Select(Function(x) New PurchaseVM(x)).ToList
            _view.ShowList(datas)
        Catch ex As Exception
            MsgBox(ex.StackTrace)
        End Try
    End Sub

    Private Sub Add()
        Using transaction = _purRep.BeginTransaction
            Try
                Dim data = _view.GetInput()
                Dim insert = _purRep.AddAsync(data).Result

                _gmbSer.UpdateOrAdd(data.pur_date, data.pur_comp_id)

                Dim entries = New List(Of accounting_entry) From {
                    New accounting_entry With {
                        .ae_TransactionId = insert.pur_id,
                        .ae_Date = Now,
                        .ae_TransactionType = "大氣進貨",
                        .ae_s_Id = 1,
                        .ae_Debit = insert.pur_price,
                        .ae_Credit = 0
                    },
                    New accounting_entry With {
                        .ae_TransactionId = insert.pur_id,
                        .ae_Date = Now,
                        .ae_TransactionType = "大氣進貨",
                        .ae_s_Id = 2,
                        .ae_Debit = insert.pur_delivery_fee,
                        .ae_Credit = 0
                    },
                    New accounting_entry With {
                        .ae_TransactionId = insert.pur_id,
                        .ae_Date = Now,
                        .ae_TransactionType = "大氣進貨",
                        .ae_s_Id = 3,
                        .ae_Debit = 0,
                        .ae_Credit = insert.pur_price + insert.pur_delivery_fee
                    }
                }

                _aeSer.AddEntries(entries)

                _purRep.SaveChangesAsync()
                transaction.Commit()
                Initialize()
                MsgBox("新增成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Private Sub Edit()
        Using transaction = _purRep.BeginTransaction
            Try
                Dim data = _view.GetInput()
                _gmbSer.UpdateOrAdd(data.pur_date, data.pur_comp_id)

                Dim entries = New List(Of accounting_entry) From {
                    New accounting_entry With {
                        .ae_TransactionId = data.pur_id,
                        .ae_Date = Now,
                        .ae_TransactionType = "大氣進貨",
                        .ae_s_Id = 1,
                        .ae_Debit = data.pur_price,
                        .ae_Credit = 0
                    },
                    New accounting_entry With {
                        .ae_TransactionId = data.pur_id,
                        .ae_Date = Now,
                        .ae_TransactionType = "大氣進貨",
                        .ae_s_Id = 2,
                        .ae_Debit = data.pur_delivery_fee,
                        .ae_Credit = 0
                    },
                    New accounting_entry With {
                        .ae_TransactionId = data.pur_id,
                        .ae_Date = Now,
                        .ae_TransactionType = "大氣進貨",
                        .ae_s_Id = 3,
                        .ae_Debit = 0,
                        .ae_Credit = data.pur_price + data.pur_delivery_fee
                    }
                }

                _aeSer.UpdateEntries(entries)
                _purRep.UpdateAsync(currentData, data)
                _purRep.SaveChangesAsync()

                transaction.Commit()
                Initialize()
                MsgBox("修改成功")
            Catch ex As Exception
                transaction.Rollback()
                MsgBox(ex.Message)
            End Try
        End Using
    End Sub

    Public Sub Delete()
        Using transaction = _purRep.BeginTransaction
            Try
                _aeSer.DeleteEntries("大氣進貨", currentData.pur_id)

                _purRep.DeleteAsync(currentData)

                _gmbSer.UpdateOrAdd(currentData.pur_date, currentData.pur_comp_id)

                _purRep.SaveChangesAsync()

                transaction.Commit()
                Initialize()
                MessageBox.Show("刪除成功")
            Catch ex As Exception
                transaction.Rollback()
                MessageBox.Show(ex.Message)
            End Try
        End Using
    End Sub

    Private Sub SelectRowAsync(sender As Object, id As Integer)
        Try
            Dim data = _purRep.GetByIdAsync(id).Result
            If data IsNot Nothing Then
                _view.ClearInput()
                currentData = data
                _view.SetDataToControls(data)
                _view.SetButton(True)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Search()
        Try
            Dim data = _view.GetSearchCondition
            LoadList(data)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Print(sender As Object, datas As List(Of PurchaseVM))
        Try
            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "大氣採購範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")

                    Dim rowIndex = 3

                    For Each item In datas
                        .WriteToCell(rowIndex, 1, item.廠商)
                        .WriteToCell(rowIndex, 2, item.日期.ToString("yyyy年MM月dd日"))
                        .WriteToCell(rowIndex, 3, item.產品)
                        .WriteToCell(rowIndex, 4, item.重量.ToString)
                        .WriteToCell(rowIndex, 5, item.單價.ToString)
                        .WriteToCell(rowIndex, 6, item.金額.ToString)
                        .WriteToCell(rowIndex, 7, item.公司)
                        .WriteToCell(rowIndex, 8, item.運費單價.ToString)
                        .WriteToCell(rowIndex, 9, item.運費.ToString)
                        .WriteToCell(rowIndex, 10, item.特殊單價.ToString)
                        .WriteToCell(rowIndex, 11, item.特殊運費.ToString)

                        rowIndex += 1
                    Next

                    '存檔
                    .SaveExcel("大氣採購")
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SetCompanyCmb()
        Try
            Dim companies = _compRep.GetCompanyDropdownAsync.Result
            _view.SetCompanyCmb(companies)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SetGasVendorCmb()
        Try
            Dim data = _manuRep.GetGasVendorCmbDataAsync.Result
            _view.SetGasVendorCmb(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SetDriveCompanyCmb()
        Try
            Dim data = _manuRep.GetVendorCmbWithoutGasAsync.Result
            _view.SetDriveVendorCmb(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class