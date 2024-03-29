﻿Public Interface IPaymentView
    Inherits ICommonView(Of payment, PaymentVM)

    ''' <summary>
    ''' 設定廠商下拉選單
    ''' </summary>
    Sub SetManuCmb(data As List(Of ComboBoxItems))

    ''' <summary>
    ''' 設定銀行下拉選單
    ''' </summary>
    Sub SetBankCmb(data As List(Of ComboBoxItems))

    ''' <summary>
    ''' 設定科目下拉選單
    ''' </summary>
    Sub SetSubjectsCmb(data As List(Of ComboBoxItems))

    ''' <summary>
    ''' 設定公司下拉選單
    ''' </summary>
    ''' <param name="data"></param>
    Sub SetCompanyCmb(data As List(Of ComboBoxItems))

    ''' <summary>
    ''' 設定應付未付列表
    ''' </summary>
    ''' <param name="data"></param>
    Sub SetAmountDueDGV(data As List(Of AmountDueVM))

    ''' <summary>
    ''' 取得搜尋條件
    ''' </summary>
    ''' <returns></returns>
    Function GetQueryConditions() As PaymentQueryVM

    ''' <summary>
    ''' 取得支票資訊
    ''' </summary>
    Function GetChequeDatas() As cheque
End Interface
