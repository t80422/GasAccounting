Imports System.IO

''' <summary>
''' 大氣採購
''' </summary>
Public Class PurchasePresenter

    Private ReadOnly _view As IPurchaseView
    Private ReadOnly _gmbSer As IGasMonthlyBalanceService
    Private ReadOnly _aeSer As IAccountingEntryService
    Private ReadOnly _printerSer As IPrinterService
    Private ReadOnly _purchaseSer As IGasPurchaseService
    Private currentData As purchase

    Public ReadOnly Property View As IPurchaseView
        Get
            Return _view
        End Get
    End Property

    Public Sub New(view As IPurchaseView, gmbSer As IGasMonthlyBalanceService, aeSer As IAccountingEntryService, printerSer As IPrinterService, purchaseSer As IGasPurchaseService)
        _view = view
        _gmbSer = gmbSer
        _aeSer = aeSer
        _printerSer = printerSer
        _purchaseSer = purchaseSer

        AddHandler _view.CreateRequest, AddressOf Add
        AddHandler _view.UpdateRequest, AddressOf Edit
        AddHandler _view.DeleteRequest, AddressOf Delete
        AddHandler _view.CancelRequest, AddressOf Initialize
        AddHandler _view.SearchRequest, AddressOf Search
        AddHandler _view.PrintClicked, AddressOf Print
        AddHandler _view.GasVenderSelected, AddressOf GetDefaultPrice
        AddHandler _view.DataSelectedRequest, AddressOf SelectRowAsync
    End Sub

    Private Async Sub Initialize()
        Try
            _view.ClearInput()
            Await SetGasVendorCmbAsync()
            Await SetCompanyCmbAsync()
            Await SetDriveCompanyCmbAsync()
            currentData = Nothing
            Await LoadListAsync()
            _view.ButtonStatus(False)
        Catch ex As Exception
            Dim errorMsg = $"【PurchasePresenter.Initialize】初始化大氣採購畫面時發生錯誤" & vbCrLf &
                          $"錯誤訊息: {ex.Message}" & vbCrLf &
                          If(ex.InnerException IsNot Nothing, $"內部錯誤: {ex.InnerException.Message}" & vbCrLf, "")
            MsgBox(errorMsg, MsgBoxStyle.Critical, "初始化錯誤")
        End Try
    End Sub

    Private Async Sub GetDefaultPrice(sender As Object, data As Tuple(Of Object, Object))
        Try
            Using uow As IUnitOfWork = DependencyContainer.Resolve(Of IUnitOfWork)()
                Dim result = Await uow.PurchaseRepository.GetDefaultPricesAsync(data.Item1, data.Item2)
                _view.SetDefaultPrice(result.Item1, result.Item2)
            End Using
        Catch ex As Exception
            Dim errorMsg = $"【PurchasePresenter.GetDefaultPrice】取得預設價格時發生錯誤" & vbCrLf &
                          $"廠商ID: {data.Item1}, 產品: {data.Item2}" & vbCrLf &
                          $"錯誤訊息: {ex.Message}" & vbCrLf &
                          If(ex.InnerException IsNot Nothing, $"內部錯誤: {ex.InnerException.Message}" & vbCrLf, "")
            MsgBox(errorMsg, MsgBoxStyle.Exclamation, "取得預設價格錯誤")
        End Try
    End Sub

    Private Async Function LoadListAsync(Optional criteria As PurchaseCondition = Nothing) As Task
        Try
            Using uow As IUnitOfWork = DependencyContainer.Resolve(Of IUnitOfWork)()
                Dim purchases = Await uow.PurchaseRepository.SearchPurchasesAsync(criteria)

                If criteria IsNot Nothing Then
                    Dim summary = _purchaseSer.GetPurchaseTradeSummary(purchases.ToList, criteria)
                    _view.ShowGasUnpaidSummary(summary.Item1)
                    _view.ShowTransportationSummary(summary.Item2)
                End If

                Dim datas = purchases.Select(Function(x) New PurchaseVM(x)).ToList
                _view.ShowList(datas)
            End Using
        Catch ex As Exception
            Dim errorMsg = $"【PurchasePresenter.LoadListAsync】載入大氣採購列表時發生錯誤" & vbCrLf &
                          $"查詢條件: {If(criteria IsNot Nothing, $"CompanyId={criteria.CompanyId}, ManufacturerId={criteria.ManufacturerId}", "無條件")}" & vbCrLf &
                          $"錯誤訊息: {ex.Message}" & vbCrLf &
                          If(ex.InnerException IsNot Nothing, $"內部錯誤: {ex.InnerException.Message}" & vbCrLf, "") &
                          $"堆疊追蹤: {ex.StackTrace}"
            MsgBox(errorMsg, MsgBoxStyle.Critical, "載入列表錯誤")
        End Try
    End Function

    Private Async Sub Add()
        Using uow As IUnitOfWork = DependencyContainer.Resolve(Of IUnitOfWork)()
            uow.BeginTransaction()
            Try
                Dim data As New purchase
                _view.GetInput(data)
                Dim insert = Await uow.PurchaseRepository.AddAsync(data)

                ' 使用新版本的 Service 方法（傳入 Repository）
                _gmbSer.UpdateOrAdd(uow.GasMonthlyBalanceRepository, uow.OrderRepository,
                                   uow.CompanyRepository, uow.BasicSetRepository,
                                   data.pur_date, data.pur_comp_id)

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
                        .ae_Credit = insert.pur_price.Value + insert.pur_delivery_fee.Value
                    }
                }

                ' 使用新版本的 Service 方法（傳入 Repository）
                _aeSer.AddEntries(uow.AccountingEntryRepository, entries)

                ' 統一由 UnitOfWork 保存
                Await uow.SaveChangesAsync()
                uow.Commit()
                Initialize()
                MessageBox.Show("新增成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                uow.Rollback()
                Dim errorMsg = $"【PurchasePresenter.Add】新增大氣採購時發生錯誤" & vbCrLf &
                              $"錯誤訊息: {ex.Message}" & vbCrLf &
                              If(ex.InnerException IsNot Nothing, $"內部錯誤: {ex.InnerException.Message}" & vbCrLf, "") &
                              $"堆疊追蹤: {ex.StackTrace}"
                MessageBox.Show(errorMsg, "新增錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Async Sub Edit()
        Using uow As IUnitOfWork = DependencyContainer.Resolve(Of IUnitOfWork)()
            uow.BeginTransaction()
            Try
                _view.GetInput(currentData)

                ' 使用新版本的 Service 方法（傳入 Repository）
                _gmbSer.UpdateOrAdd(uow.GasMonthlyBalanceRepository, uow.OrderRepository,
                                       uow.CompanyRepository, uow.BasicSetRepository,
                                       currentData.pur_date, currentData.pur_comp_id)

                Dim entries = New List(Of accounting_entry) From {
                        New accounting_entry With {
                            .ae_TransactionId = currentData.pur_id,
                            .ae_Date = Now,
                            .ae_TransactionType = "大氣進貨",
                            .ae_s_Id = 1,
                            .ae_Debit = currentData.pur_price,
                            .ae_Credit = 0
                        },
                        New accounting_entry With {
                            .ae_TransactionId = currentData.pur_id,
                            .ae_Date = Now,
                            .ae_TransactionType = "大氣進貨",
                            .ae_s_Id = 2,
                            .ae_Debit = currentData.pur_delivery_fee,
                            .ae_Credit = 0
                        },
                        New accounting_entry With {
                            .ae_TransactionId = currentData.pur_id,
                            .ae_Date = Now,
                            .ae_TransactionType = "大氣進貨",
                            .ae_s_Id = 3,
                            .ae_Debit = 0,
                            .ae_Credit = currentData.pur_price + currentData.pur_delivery_fee
                        }
                    }

                ' 使用新版本的 Service 方法（傳入 Repository）
                _aeSer.UpdateEntries(uow.AccountingEntryRepository, entries)
                Await uow.PurchaseRepository.UpdateAsync(currentData.pur_id, currentData)

                ' 統一由 UnitOfWork 保存
                Await uow.SaveChangesAsync()

                uow.Commit()
                Initialize()
                MsgBox("修改成功", MsgBoxStyle.Information, "成功")
            Catch ex As Exception
                uow.Rollback()
                Dim errorMsg = $"【PurchasePresenter.Edit】修改大氣採購時發生錯誤" & vbCrLf &
                                  $"採購單ID: {If(currentData?.pur_id, 0)}" & vbCrLf &
                                  $"錯誤訊息: {ex.Message}" & vbCrLf &
                                  If(ex.InnerException IsNot Nothing, $"內部錯誤: {ex.InnerException.Message}" & vbCrLf, "") &
                                  $"堆疊追蹤: {ex.StackTrace}"
                MsgBox(errorMsg, MsgBoxStyle.Critical, "修改錯誤")
            End Try
        End Using
    End Sub

    Public Async Sub Delete()
        Using uow As IUnitOfWork = DependencyContainer.Resolve(Of IUnitOfWork)()
            uow.BeginTransaction()
            Try
                ' 使用新版本的 Service 方法（傳入 Repository）
                _aeSer.DeleteEntries(uow.AccountingEntryRepository, "大氣進貨", currentData.pur_id)

                Await uow.PurchaseRepository.DeleteAsync(currentData)

                ' 使用新版本的 Service 方法（傳入 Repository）
                _gmbSer.UpdateOrAdd(uow.GasMonthlyBalanceRepository, uow.OrderRepository,
                                       uow.CompanyRepository, uow.BasicSetRepository,
                                       currentData.pur_date, currentData.pur_comp_id)

                ' 統一由 UnitOfWork 保存
                Await uow.SaveChangesAsync()

                uow.Commit()
                Initialize()
                MessageBox.Show("刪除成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                uow.Rollback()
                Dim errorMsg = $"【PurchasePresenter.Delete】刪除大氣採購時發生錯誤" & vbCrLf &
                                  $"採購單ID: {If(currentData?.pur_id, 0)}, 日期: {If(currentData?.pur_date, Date.MinValue):yyyy/MM/dd}" & vbCrLf &
                                  $"錯誤訊息: {ex.Message}" & vbCrLf &
                                  If(ex.InnerException IsNot Nothing, $"內部錯誤: {ex.InnerException.Message}" & vbCrLf, "") &
                                  $"堆疊追蹤: {ex.StackTrace}"
                MessageBox.Show(errorMsg, "刪除錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Async Sub SelectRowAsync(sender As Object, id As Integer)
        Try
            Using uow As IUnitOfWork = DependencyContainer.Resolve(Of IUnitOfWork)()
                Dim data = Await uow.PurchaseRepository.GetByIdAsync(id)
                If data IsNot Nothing Then
                    _view.ClearInput()
                    currentData = data
                    _view.ShowDetail(data)
                    _view.ButtonStatus(True)
                End If
            End Using
        Catch ex As Exception
            Dim errorMsg = $"【PurchasePresenter.SelectRowAsync】選取大氣採購資料時發生錯誤" & vbCrLf &
                          $"採購單ID: {id}" & vbCrLf &
                          $"錯誤訊息: {ex.Message}" & vbCrLf &
                          If(ex.InnerException IsNot Nothing, $"內部錯誤: {ex.InnerException.Message}" & vbCrLf, "")
            MsgBox(errorMsg, MsgBoxStyle.Exclamation, "選取資料錯誤")
        End Try
    End Sub

    Private Async Sub Search()
        Try
            Dim criteria = _view.GetSearchCondition
            Await LoadListAsync(criteria)
        Catch ex As Exception
            Dim errorMsg = $"【PurchasePresenter.Search】搜尋大氣採購時發生錯誤" & vbCrLf &
                          $"錯誤訊息: {ex.Message}" & vbCrLf &
                          If(ex.InnerException IsNot Nothing, $"內部錯誤: {ex.InnerException.Message}" & vbCrLf, "")
            MessageBox.Show(errorMsg, "搜尋錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

    Private Async Function SetCompanyCmbAsync() As Task
        Try
            Using uow As IUnitOfWork = DependencyContainer.Resolve(Of IUnitOfWork)()
                Dim companies = Await uow.CompanyRepository.GetCompanyDropdownAsync
                _view.SetCompanyCmb(companies)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Async Function SetGasVendorCmbAsync() As Task
        Try
            Using uow As IUnitOfWork = DependencyContainer.Resolve(Of IUnitOfWork)()
                Dim data = Await uow.ManufacturerRepository.GetGasVendorCmbDataAsync
                _view.SetGasVendorCmb(data)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Async Function SetDriveCompanyCmbAsync() As Task
        Try
            Using uow As IUnitOfWork = DependencyContainer.Resolve(Of IUnitOfWork)()
                Dim data = Await uow.ManufacturerRepository.GetVendorCmbWithoutGasAsync
                _view.SetDriveVendorCmb(data)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function
End Class