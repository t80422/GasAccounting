Public Class ManufacturerPresenter
    Inherits BasePresenter(Of manufacturer, ManufacturerVM, IManufacturerView)
    Implements IPresenter(Of manufacturer, ManufacturerVM)

    Public Sub New(view As IManufacturerView)
        MyBase.New(view)
        _presenter = Me
    End Sub

    Public Function SetSearchConditions(query As IQueryable(Of manufacturer), conditions As Object) As IQueryable(Of manufacturer) Implements IPresenter(Of manufacturer, ManufacturerVM).SetSearchConditions
        Dim c As manufacturer = conditions

        query = query.Where(Function(x) x.manu_code.Contains(c.manu_code))
        query = query.Where(Function(x) x.manu_name.Contains(c.manu_name))
        query = query.Where(Function(x) x.manu_phone1.Contains(c.manu_phone1))

        Return query
    End Function

    Public Function SetListViewModel(query As IQueryable(Of manufacturer)) As List(Of ManufacturerVM) Implements IPresenter(Of manufacturer, ManufacturerVM).SetListViewModel
        Return query.Select(Function(x) New ManufacturerVM With {
                    .編號 = x.manu_id,
                    .代號 = x.manu_code,
                    .名稱 = x.manu_name,
                    .負責人 = x.manu_principal,
                    .聯絡人 = x.manu_contact_person,
                    .電話1 = x.manu_phone1,
                    .電話2 = x.manu_phone2,
                    .地址 = x.manu_address,
                    .統編 = x.manu_tax_id,
                    .傳真 = x.manu_fax,
                    .是否為大氣公司 = x.manu_GasVendor,
                    .銀行 = x.manu_bank,
                    .分行 = x.manu_branches,
                    .銀行代號 = x.manu_bank_code,
                    .戶名 = x.manu_account_name,
                    .帳號 = x.manu_account,
                    .備註 = x.manu_memo
                }).ToList
    End Function

    'Public Sub LoadList(Optional conditions As ManufacturerVM = Nothing)
    '    Try
    '        Using db As New gas_accounting_systemEntities
    '            Dim query = db.manufacturers.AsQueryable

    '            If conditions IsNot Nothing Then
    '                query = query.Where(Function(x) x.manu_code.Contains(conditions.manu_code))
    '                query = query.Where(Function(x) x.manu_name.Contains(conditions.manu_name))
    '                query = query.Where(Function(x) x.manu_contact_person.Contains(conditions.manu_contact_person))
    '                query = query.Where(Function(x) x.manu_phone1.Contains(conditions.manu_phone1))
    '            End If

    '            _view.ShowList(query.ToList)
    '        End Using

    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

    'Public Sub Add()
    '    Dim data = _view.GetUserInput

    '    If data IsNot Nothing Then
    '        Try
    '            Using db As New gas_accounting_systemEntities
    '                db.manufacturers.Add(data)
    '                db.SaveChanges()
    '                LoadList()
    '                _view.ClearInput()
    '                MsgBox("新增成功")
    '            End Using

    '        Catch ex As Exception
    '            MsgBox(ex.Message)
    '        End Try
    '    End If
    'End Sub

    'Public Sub Edit()
    '    Try
    '        Using db As New gas_accounting_systemEntities
    '            Dim data = _view.GetUserInput

    '            If data IsNot Nothing Then
    '                Dim existingSubject = db.manufacturers.Find(data.manu_id)

    '                If existingSubject IsNot Nothing Then
    '                    ' 使用反射來更新屬性
    '                    For Each prop In GetType(ManufacturerVM).GetProperties()
    '                        If prop.CanRead AndAlso prop.CanWrite Then
    '                            Dim newValue = prop.GetValue(data)
    '                            prop.SetValue(existingSubject, newValue)
    '                        End If
    '                    Next

    '                    db.SaveChanges()
    '                    LoadList()
    '                    _view.ClearInput()
    '                    MsgBox("修改成功。")
    '                Else
    '                    MsgBox("未找到指定的對象。")
    '                End If
    '            End If

    '        End Using

    '    Catch ex As Exception
    '        MsgBox("修改時發生錯誤: " & ex.Message)
    '    End Try
    'End Sub

    'Public Sub Delete(id As Integer)
    '    Try
    '        Using db As New gas_accounting_systemEntities
    '            Dim data = db.manufacturers.Find(id)
    '            If data IsNot Nothing Then
    '                db.manufacturers.Remove(data)
    '                db.SaveChanges()
    '                LoadList()
    '                _view.ClearInput()
    '                MsgBox("刪除成功。")

    '            Else
    '                MsgBox("未找到要刪除的對象。")
    '            End If
    '        End Using

    '    Catch ex As Exception
    '        MsgBox("刪除時發生錯誤: " & ex.Message)
    '    End Try
    'End Sub

    'Public Sub SelectRow(id As Integer)
    '    Try
    '        Using db As New gas_accounting_systemEntities
    '            Dim data = db.manufacturers.Find(id)
    '            If data IsNot Nothing Then
    '                _view.ClearInput()
    '                _view.SetDataToControl(data)
    '            End If
    '        End Using

    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub
End Class
