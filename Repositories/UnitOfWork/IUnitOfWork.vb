Imports System.Data.Entity

''' <summary>
''' Unit of Work 介面，用於管理 DbContext 生命週期和交易
''' </summary>
Public Interface IUnitOfWork
    Inherits IDisposable

    ''' <summary>
    ''' 取得 Purchase Repository
    ''' </summary>
    ReadOnly Property PurchaseRepository As IPurchaseRep

    ''' <summary>
    ''' 取得 Company Repository
    ''' </summary>
    ReadOnly Property CompanyRepository As ICompanyRep

    ''' <summary>
    ''' 取得 Manufacturer Repository
    ''' </summary>
    ReadOnly Property ManufacturerRepository As IManufacturerRep

    ''' <summary>
    ''' 取得 Subject Repository
    ''' </summary>
    ReadOnly Property SubjectRepository As ISubjectRep

    ''' <summary>
    ''' 取得 AccountingEntry Repository
    ''' </summary>
    ReadOnly Property AccountingEntryRepository As IAccountingEntryRep

    ''' <summary>
    ''' 取得 GasMonthlyBalance Repository
    ''' </summary>
    ReadOnly Property GasMonthlyBalanceRepository As IGasMonthlyBalanceRep

    ''' <summary>
    ''' 取得 Order Repository
    ''' </summary>
    ReadOnly Property OrderRepository As IOrderRep

    ''' <summary>
    ''' 取得 BasicSet Repository
    ''' </summary>
    ReadOnly Property BasicSetRepository As IBasicSetRep

    ''' <summary>
    ''' 取得 Payment Repository
    ''' </summary>
    ReadOnly Property PaymentRepository As IPaymentRep

    ''' <summary>
    ''' 開始資料庫交易
    ''' </summary>
    Function BeginTransaction() As DbContextTransaction

    ''' <summary>
    ''' 保存所有變更
    ''' </summary>
    Function SaveChangesAsync() As Task(Of Integer)

    ''' <summary>
    ''' 取得 DbContext 實例（給 Service 層使用，如有需要）
    ''' </summary>
    ReadOnly Property Context As gas_accounting_systemEntities
End Interface

