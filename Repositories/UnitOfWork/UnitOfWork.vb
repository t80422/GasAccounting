Imports System.Data.Entity

''' <summary>
''' Unit of Work 實作，管理一個 Transient DbContext 實例
''' </summary>
Public Class UnitOfWork
    Implements IUnitOfWork

    Private ReadOnly _context As gas_accounting_systemEntities
    Private _transaction As DbContextTransaction
    Private _disposed As Boolean = False

    ' Repository 延遲初始化
    Private _purchaseRepository As IPurchaseRep
    Private _companyRepository As ICompanyRep
    Private _manufacturerRepository As IManufacturerRep
    Private _subjectRepository As ISubjectRep
    Private _accountingEntryRepository As IAccountingEntryRep
    Private _gasMonthlyBalanceRepository As IGasMonthlyBalanceRep
    Private _orderRepository As IOrderRep
    Private _basicSetRepository As IBasicSetRep
    Private _paymentRepository As IPaymentRep
    Private _chequePayRepository As IChequePayRep
    Private _reportRepository As IReportRep

    Public Sub New(context As gas_accounting_systemEntities)
        _context = context
    End Sub

    Public ReadOnly Property Context As gas_accounting_systemEntities Implements IUnitOfWork.Context
        Get
            Return _context
        End Get
    End Property

    Public ReadOnly Property PurchaseRepository As IPurchaseRep Implements IUnitOfWork.PurchaseRepository
        Get
            If _purchaseRepository Is Nothing Then
                _purchaseRepository = New PurchaseRep(_context)
            End If
            Return _purchaseRepository
        End Get
    End Property

    Public ReadOnly Property CompanyRepository As ICompanyRep Implements IUnitOfWork.CompanyRepository
        Get
            If _companyRepository Is Nothing Then
                _companyRepository = New CompanyRep(_context)
            End If
            Return _companyRepository
        End Get
    End Property

    Public ReadOnly Property ManufacturerRepository As IManufacturerRep Implements IUnitOfWork.ManufacturerRepository
        Get
            If _manufacturerRepository Is Nothing Then
                _manufacturerRepository = New ManufacturerRep(_context)
            End If
            Return _manufacturerRepository
        End Get
    End Property

    Public ReadOnly Property SubjectRepository As ISubjectRep Implements IUnitOfWork.SubjectRepository
        Get
            If _subjectRepository Is Nothing Then
                _subjectRepository = New SubjectRep(_context)
            End If
            Return _subjectRepository
        End Get
    End Property

    Public ReadOnly Property AccountingEntryRepository As IAccountingEntryRep Implements IUnitOfWork.AccountingEntryRepository
        Get
            If _accountingEntryRepository Is Nothing Then
                _accountingEntryRepository = New AccountingEntryRep(_context)
            End If
            Return _accountingEntryRepository
        End Get
    End Property

    Public ReadOnly Property GasMonthlyBalanceRepository As IGasMonthlyBalanceRep Implements IUnitOfWork.GasMonthlyBalanceRepository
        Get
            If _gasMonthlyBalanceRepository Is Nothing Then
                _gasMonthlyBalanceRepository = New GasMonthlyBalanceRep(_context)
            End If
            Return _gasMonthlyBalanceRepository
        End Get
    End Property

    Public ReadOnly Property OrderRepository As IOrderRep Implements IUnitOfWork.OrderRepository
        Get
            If _orderRepository Is Nothing Then
                _orderRepository = New OrderRep(_context)
            End If
            Return _orderRepository
        End Get
    End Property

    Public ReadOnly Property BasicSetRepository As IBasicSetRep Implements IUnitOfWork.BasicSetRepository
        Get
            If _basicSetRepository Is Nothing Then
                _basicSetRepository = New BasicSetRep(_context)
            End If
            Return _basicSetRepository
        End Get
    End Property

    Public ReadOnly Property PaymentRepository As IPaymentRep Implements IUnitOfWork.PaymentRepository
        Get
            If _paymentRepository Is Nothing Then
                _paymentRepository = New PaymentRep(_context)
            End If
            Return _paymentRepository
        End Get
    End Property

    Public ReadOnly Property ChequePayRepository As IChequePayRep Implements IUnitOfWork.ChequePayRepository
        Get
            If _chequePayRepository Is Nothing Then
                _chequePayRepository = New ChequePayRep(_context)
            End If
            Return _chequePayRepository
        End Get
    End Property

    Public ReadOnly Property ReportRepository As IReportRep Implements IUnitOfWork.ReportRepository
        Get
            If _reportRepository Is Nothing Then _reportRepository = New ReportRep(_context)
            Return _reportRepository
        End Get
    End Property

    Public Sub BeginTransaction() Implements IUnitOfWork.BeginTransaction
        _transaction = _context.Database.BeginTransaction()
    End Sub

    Public Sub Commit() Implements IUnitOfWork.Commit
        If _transaction IsNot Nothing Then _transaction.Commit()
    End Sub

    Public Sub Rollback() Implements IUnitOfWork.Rollback
        If _transaction IsNot Nothing Then _transaction.Rollback()
    End Sub

    Public Async Function SaveChangesAsync() As Task(Of Integer) Implements IUnitOfWork.SaveChangesAsync
        Try
            Return Await _context.SaveChangesAsync()
        Catch ex As Exception
            Throw New Exception("保存變更時發生錯誤", ex)
        End Try
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _disposed Then
            If disposing Then
                _context?.Dispose()
            End If
            _disposed = True
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
End Class

