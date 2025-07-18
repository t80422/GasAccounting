Imports System.IO

Public Class InspectionPresenter
    Private ReadOnly _view As IInspectionView
    Private ReadOnly _cusRep As ICustomerRep
    Private ReadOnly _insRep As IInspectionRep
    Private _currentData As inspection

    Public Sub New(view As IInspectionView, cusRep As ICustomerRep, insRep As IInspectionRep)
        _view = view
        _cusRep = cusRep
        _insRep = insRep
    End Sub

    ''' <summary>
    ''' 根據客戶代碼獲取客戶信息
    ''' </summary>
    ''' <param name="cusCode">客戶代碼</param>
    ''' <remarks></remarks>
    Public Sub GetCustomerByCusCode(cusCode As String)
        Try
            Dim cus = _cusRep.GetByCusCode(cusCode)
            If cus IsNot Nothing Then
                _view.ShowCustomer(cus)
            Else
                MsgBox("找不到該客戶")
            End If
        Catch ex As Exception
            MsgBox("發生錯誤: " & ex.Message)
        End Try
    End Sub

    Public Sub Reset()
        Try
            _view.Clear()
            LoadList(New InspectionSC)
            _currentData = Nothing
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub LoadList(Optional criteria As InspectionSC = Nothing)
        Try
            Dim data = _insRep.Search(criteria)
            _view.DisplayList(data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Add()
        Try
            Dim data = _view.GetInput()
            _insRep.AddAsync(data)
            Reset()
            LoadList()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Async Sub LoadDetail(id As Integer)
        Try
            Dim data = Await _insRep.GetByIdAsync(id)
            If data IsNot Nothing Then
                _view.Clear()
                _currentData = data
                _view.ShowDetail(data)
            Else
                MsgBox("找不到該檢查記錄")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Update()
        Try
            If _currentData Is Nothing Then
                MsgBox("請先選擇一條記錄")
                Return
            End If

            Dim updatedData = _view.GetInput()
            updatedData.in_Id = _currentData.in_Id
            _insRep.UpdateAsync(_currentData.in_Id, updatedData)
            Reset()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Delete()
        Try
            If _currentData Is Nothing Then
                MsgBox("請先選擇一條記錄")
                Return
            End If
            Dim result = MsgBox("確定要刪除這條記錄嗎？", MsgBoxStyle.YesNo)
            If result = MsgBoxResult.Yes Then
                _insRep.DeleteAsync(_currentData.in_Id)
                Reset()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub Print()
        Try
            If _currentData Is Nothing Then
                MsgBox("請先選擇一條記錄")
                Return
            End If

            '取得資料
            Dim data = _insRep.GetByIdAsync(_currentData.in_Id).Result

            '取得範本檔
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "每月檢驗費範本檔.xlsx")

            '套版
            Using xml As New CloseXML_Excel(filePath)
                With xml
                    .SelectWorksheet("Sheet1")
                    .WriteToCell("A1", data.customer.cus_name)

                    .WriteToCell("B4", data.in_Qty50.ToString)
                    .WriteToCell("C4", data.in_Price50.ToString)
                    .WriteToCell("B5", data.in_Qty20.ToString)
                    .WriteToCell("C5", data.in_Price20.ToString)
                    .WriteToCell("B6", data.in_Qty16.ToString)
                    .WriteToCell("C6", data.in_Price16.ToString)
                    .WriteToCell("B7", data.in_Qty10.ToString)
                    .WriteToCell("C7", data.in_Price10.ToString)
                    .WriteToCell("B8", data.in_Qty4.ToString)
                    .WriteToCell("C8", data.in_Price4.ToString)
                    .WriteToCell("B9", data.in_QtySwitch.ToString)
                    .WriteToCell("C9", data.in_PriceSwitch.ToString)
                    .WriteToCell("B10", data.in_QtyFreight.ToString)
                    .WriteToCell("C10", data.in_PriceFreight.ToString)
                    .WriteToCell("B11", data.in_QtyRustProof.ToString)
                    .WriteToCell("C11", data.in_PriceRustProof.ToString)
                    .WriteToCell("B12", data.in_QtySpraying.ToString)
                    .WriteToCell("C12", data.in_PriceSpraying.ToString)
                    .WriteToCell("B13", data.in_QtyTotal.ToString)
                    .WriteToCell("C13", data.in_PriceTotal.ToString)

                    '存檔
                    Dim title = $"{data.customer.cus_name} {data.in_Month.Value:yyyy年MM}月檢驗費.xlsx"
                    .SaveExcel(title)
                End With
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
