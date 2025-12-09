Public Class CollectionService
    Implements ICollectionService

    Private ReadOnly _uowFactory As IUnitOfWorkFactory
    Private ReadOnly _bmbService As IBankMonthlyBalanceService
    Private ReadOnly _aeSer As IAccountingEntryService
    Private ReadOnly _ocmSer As IOrderCollectionMappingService

    Public Sub New(uowFactory As IUnitOfWorkFactory,
                   bmbService As IBankMonthlyBalanceService,
                   aeSer As IAccountingEntryService,
                   ocmSer As IOrderCollectionMappingService)
        _uowFactory = uowFactory
        _bmbService = bmbService
        _aeSer = aeSer
        _ocmSer = ocmSer
    End Sub

    Public Async Function DeleteAsync(collectionId As Integer) As Task Implements ICollectionService.DeleteAsync
        Using uow = _uowFactory.Create()
            Try
                uow.BeginTransaction()

                Dim orgCol = Await uow.CollectionRepository.GetByIdAsync(collectionId)
                If orgCol Is Nothing Then Throw New Exception("找不到要更新的收款資料，可能已被刪除")

                Dim payType = orgCol.col_Type
                Dim bankId = orgCol.col_bank_Id
                Dim accountMonth = orgCol.col_AccountMonth
                Dim amount = orgCol.col_Amount

                ' 銷帳
                _ocmSer.DeleteCollection(orgCol.col_Id)

                If payType = "應收票據" Then
                    Dim cheque = Await uow.ChequeRepository.GetByNumberAsync(orgCol.col_Cheque)
                    If cheque IsNot Nothing Then
                        Await uow.ChequeRepository.DeleteAsync(cheque.che_Id)
                    End If

                ElseIf payType = "銀行存款" Then
                    ' 使用增量更新：減少一筆收入（負數）
                    Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                        uow.BankMonthlyBalancesRepository,
                        uow.BankRepository,
                        bankId,
                        accountMonth,
                        creditDelta:=0,
                        debitDelta:=-amount
                    )

                    ' 更新支票資訊
                    Dim cheque = Await uow.ChequeRepository.GetByNumberAsync(orgCol.col_Cheque)

                    If cheque IsNot Nothing Then
                        cheque.che_CashingDate = Nothing
                        cheque.chu_State = "已代收"
                    End If
                End If

                ' 科目為銀行存款時也需回沖（對應新增時的增量增加）
                Dim subject = Await uow.SubjectRepository.GetByIdAsync(orgCol.col_s_Id)
                If subject IsNot Nothing AndAlso subject.s_name = "銀行存款" Then
                    Await _bmbService.UpdateMonthBalanceIncrementalAsync(
                        uow.BankMonthlyBalancesRepository,
                        uow.BankRepository,
                        bankId,
                        accountMonth,
                        creditDelta:=0,
                        debitDelta:=-amount
                    )
                End If

                '刪除資料
                _aeSer.DeleteEntries("收款作業", orgCol.col_Id)
                Await uow.CollectionRepository.DeleteAsync(orgCol)

                Await uow.SaveChangesAsync()
                uow.Commit()
            Catch
                uow.Rollback()
                Throw
            End Try
        End Using
    End Function
End Class

