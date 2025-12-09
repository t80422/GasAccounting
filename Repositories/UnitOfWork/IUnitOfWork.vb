''' <summary>
''' Unit of Work 介面，用於管理 DbContext 生命週期和交易
''' </summary>
Public Interface IUnitOfWork
    Inherits IDisposable

    ReadOnly Property PurchaseRepository As IPurchaseRep
    ReadOnly Property CompanyRepository As ICompanyRep
    ReadOnly Property ManufacturerRepository As IManufacturerRep
    ReadOnly Property SubjectRepository As ISubjectRep
    ReadOnly Property AccountingEntryRepository As IAccountingEntryRep
    ReadOnly Property GasMonthlyBalanceRepository As IGasMonthlyBalanceRep
    ReadOnly Property OrderRepository As IOrderRep
    ReadOnly Property BasicSetRepository As IBasicSetRep
    ReadOnly Property PaymentRepository As IPaymentRep
    ReadOnly Property ChequePayRepository As IChequePayRep
    ReadOnly Property ReportRepository As IReportRep
    ReadOnly Property BankRepository As IBankRep
    ReadOnly Property BankMonthlyBalancesRepository As IBankMonthlyBalancesRep
    ReadOnly Property CollectionRepository As ICollectionRep
    ReadOnly Property CustomerRepository As ICustomerRep
    ReadOnly Property ChequeRepository As IChequeRep
    ReadOnly Property PaymentChequeRepository As IPaymentChequeRep

    Sub BeginTransaction()
    Sub Commit()
    Sub Rollback()
    Function SaveChangesAsync() As Task(Of Integer)

    ReadOnly Property Context As gas_accounting_systemEntities
End Interface

