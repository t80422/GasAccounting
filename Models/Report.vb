Imports System.IO

Public Class Report
    Private _cusRep As ICustomerRepository = New CustomerRepository()

    Public Sub PrintCustomersGasDetailByDay(d As Date)
        Try
            '蒐集資料
            Dim datas = _cusRep.CustomersGasDetailByDay(d)

            '套版
            Dim filePath = Path.Combine(Application.StartupPath, "Report", "報表範本檔.xlsx")

            Using xml As New CloseXML_Excel(filePath)
                xml.SelectWorksheet("日氣量氣款收付明細表")

                With xml
                    For i As Integer = 0 To datas.Count
                        Dim rowIndex = 3 + i

                        .WriteToCell(rowIndex, 1, datas(i).客戶名稱)
                        .WriteToCell(rowIndex, 2, datas(i).存氣)
                        .WriteToCell(rowIndex, 3, datas(i).本日提量)
                        .WriteToCell(rowIndex, 4, datas(i).當月累計提量)
                        .WriteToCell(rowIndex, 5, datas(i).本日氣款)
                        .WriteToCell(rowIndex, 6, datas(i).本日收款)
                        .WriteToCell(rowIndex, 7, datas(i).結欠)
                    Next

                    .SaveAs("日氣量氣款收付明細表")
                End With
            End Using
        Catch ex As Exception
            MsgBox("列印失敗:" + ex.Message)
        End Try

    End Sub
End Class
