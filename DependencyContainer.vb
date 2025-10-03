Imports Unity
Imports Unity.Lifetime

Public Class DependencyContainer
    Private Shared _container As IUnityContainer

    Public Shared Sub Initialize()
        _container = New UnityContainer()

        With _container
            ' 註冊 DbContext 為 Singleton（給舊的 Presenter 使用）
            .RegisterType(Of gas_accounting_systemEntities)(New ContainerControlledLifetimeManager())

            ' 註冊 UnitOfWork（每次創建新的 Transient DbContext）
            .RegisterType(Of IUnitOfWork, UnitOfWork)(New TransientLifetimeManager())

            ' 註冊 Repository
            .RegisterType(Of IAccountingEntryRep, AccountingEntryRep)()
            .RegisterType(Of IBankRep, BankRep)()
            .RegisterType(Of IBasicPriceRep, BasicPriceRep)()
            .RegisterType(Of IBankMonthlyBalancesRep, BankMonthlyBalancesRep)()
            .RegisterType(Of IBarrelMonthlyBalancesRep, BarrelMonthlyBalancesRep)()
            .RegisterType(Of IBasicSetRep, BasicSetRep)()
            .RegisterType(Of ICarRep, CarRep)()
            .RegisterType(Of IChequeRep, ChequeRep)()
            .RegisterType(Of IClosingEntryRep, ClosingEntryRep)()
            .RegisterType(Of ICollectionRep, CollectionRep)()
            .RegisterType(Of ICompanyRep, CompanyRep)()
            .RegisterType(Of ICustomerRep, CustomerRep)()
            .RegisterType(Of IGasBarrelRep, GasBarrelRep)()
            .RegisterType(Of IManufacturerRep, ManufacturerRep)()
            .RegisterType(Of IGasMonthlyBalanceRep, GasMonthlyBalanceRep)
            .RegisterType(Of IOrderRep, OrderRep)()
            .RegisterType(Of IOrderCollectionMappingRep, OrderCollectionMappingRep)()
            .RegisterType(Of IPaymentRep, PaymentRep)()
            .RegisterType(Of IPrinterService, PrinterService)()
            .RegisterType(Of IPurchaseBarrelRep, PurchaseBarrelRep)()
            .RegisterType(Of IPurchaseRep, PurchaseRep)
            .RegisterType(Of IPricePlanRep, PricePlanRep)
            .RegisterType(Of IReportRep, ReportRep)()
            .RegisterType(Of IScrapBarrelRep, ScrapBarrelRep)()
            .RegisterType(Of IScrapBarrelDetailRep, ScrapBarrelDetailRep)()
            .RegisterType(Of ISurplusGasRep, SurplusGasRep)()
            .RegisterType(Of ISubjectRep, SubjectRep)()
            .RegisterType(Of IChequePayRep, ChequePayRep)()

            ' 註冊 Service
            .RegisterType(Of IAccountingEntryService, AccountingEntryService)()
            .RegisterType(Of IBankMonthlyBalanceService, BankMonthlyBalanceService)()
            .RegisterType(Of IBarrelMonthlyBalanceService, BarrelMonthlyBalanceService)()
            .RegisterType(Of IMonthlyAccountService, MonthlyAccountService)
            .RegisterType(Of IPriceCalculationService, PriceCalculationService)()
            .RegisterType(Of IPrinterService, PrinterService)()
            .RegisterType(Of IOrderCollectionMappingService, OrderCollectionMappingService)()
            .RegisterType(Of IReportService, ReportService)()
            .RegisterType(Of IManufacturerService, ManufacturerService)
            .RegisterType(Of IGasMonthlyBalanceService, GasMonthlyBalanceService)
            .RegisterType(Of IGasPurchaseService, GasPurchaseService)

            ' 註冊 View
            .RegisterType(Of IChequePayView, ChequePayUserControl)
            .RegisterType(Of ICustomerView, CustomerUserControl)
            .RegisterType(Of IGasCheckoutView, GasCheckoutUserControl)
            .RegisterType(Of IPurchaseView, GasPurchaseUserControl)
            .RegisterType(Of IPurchaseBarrelView, PurchaseBarrelUserControl)()
            .RegisterType(Of IPaymentView, PaymentUserControl)()
            .RegisterType(Of IOrderView, OrderUserControl)

            ' 註冊 Presenter
            .RegisterType(Of ChequePayPresenter)
            .RegisterType(Of CustomerPresenter)
            .RegisterType(Of GasCheckoutPresenter)
            .RegisterType(Of PurchasePresenter)
            .RegisterType(Of PurBarrelPresenter)()
            .RegisterType(Of ClosingEntryPresenter)()
            .RegisterType(Of ReportPresenter)()
            .RegisterType(Of OrderPresenter)()
            .RegisterType(Of PaymentPresenter)()
            .RegisterType(Of CollectionPresenter)()
            .RegisterType(Of ScrapBarrelPresenter)
            .RegisterType(Of ScrapBarrelDetailPresenter)
            .RegisterType(Of ChequePresenter)()
        End With
    End Sub

    Public Shared Function Resolve(Of T)() As T
        Return _container.Resolve(Of T)()
    End Function

    Public Shared Sub Dispose()
        _container?.Dispose()
    End Sub
End Class
