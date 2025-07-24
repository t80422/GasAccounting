Imports Unity
Imports Unity.Lifetime

Public Class DependencyContainer
    Private Shared _container As IUnityContainer

    Public Shared Sub Initialize()
        _container = New UnityContainer()

        With _container
            ' 註冊 DbContext 為 Singleton（整個應用程式共用一個實例）
            .RegisterType(Of gas_accounting_systemEntities)(New ContainerControlledLifetimeManager())

            ' 註冊 Repository
            .RegisterType(Of IAccountingEntryRep, AccountingEntryRep)()
            .RegisterType(Of IBankRep, BankRep)()
            .RegisterType(Of IBasicPriceRep, BasicPriceRep)()
            .RegisterType(Of IBankMonthlyBalancesRep, BankMonthlyBalancesRep)()
            .RegisterType(Of IBarrelMonthlyBalancesRep, BarrelMonthlyBalancesRep)()
            .RegisterType(Of ICarRep, CarRep)()
            .RegisterType(Of IChequeRep, ChequeRep)()
            .RegisterType(Of IClosingEntryRep, ClosingEntryRep)()
            .RegisterType(Of ICollectionRep, CollectionRep)()
            .RegisterType(Of ICompanyRep, CompanyRep)()
            .RegisterType(Of ICustomerRep, CustomerRep)()
            .RegisterType(Of IGasBarrelRep, GasBarrelRep)()
            .RegisterType(Of IManufacturerRep, ManufacturerRep)()
            .RegisterType(Of IOrderRep, OrderRep)()
            .RegisterType(Of IOrderCollectionMappingRep, OrderCollectionMappingRep)()
            .RegisterType(Of IPaymentRep, PaymentRep)()
            .RegisterType(Of IPrinterService, PrinterService)()
            .RegisterType(Of IPurchaseBarrelRep, PurchaseBarrelRep)()
            .RegisterType(Of IReportRep, ReportRep)()
            .RegisterType(Of IScrapBarrelRep, ScrapBarrelRep)()
            .RegisterType(Of IScrapBarrelDetailRep, ScrapBarrelDetailRep)()
            .RegisterType(Of ISubjectRep, SubjectRep)()

            ' 註冊 Service
            .RegisterType(Of IAccountingEntryService, AccountingEntryService)()
            .RegisterType(Of IBankMonthlyBalanceService, BankMonthlyBalanceService)()
            .RegisterType(Of IBarrelMonthlyBalanceService, BarrelMonthlyBalanceService)()
            .RegisterType(Of IPriceCalculationService, PriceCalculationService)()
            .RegisterType(Of IPrinterService, PrinterService)()
            .RegisterType(Of IOrderCollectionMappingService, OrderCollectionMappingService)()
            .RegisterType(Of IReportService, ReportService)()

            ' 註冊 Presenter
            .RegisterType(Of ClosingEntryPresenter)()
            .RegisterType(Of ReportPresenter)()
            .RegisterType(Of OrderPresenter)()
            .RegisterType(Of PaymentPresenter)()
            .RegisterType(Of CollectionPresenter)()
            .RegisterType(Of ScrapBarrelPresenter)
            .RegisterType(Of ScrapBarrelDetailPresenter)
        End With
    End Sub

    Public Shared Function Resolve(Of T)() As T
        Return _container.Resolve(Of T)()
    End Function

    Public Shared Sub Dispose()
        _container?.Dispose()
    End Sub
End Class
