Imports Unity
Imports Unity.Lifetime

Public Class DependencyContainer
    Private Shared _container As IUnityContainer

    Public Shared Sub Initialize()
        _container = New UnityContainer()

        ' 註冊 DbContext 為 Singleton（整個應用程式共用一個實例）
        _container.RegisterType(Of gas_accounting_systemEntities)(New ContainerControlledLifetimeManager())

        ' 註冊 Repository
        _container.RegisterType(Of IClosingEntryRep, ClosingEntryRep)()
        _container.RegisterType(Of ISubjectRep, SubjectRep)()

        ' 註冊 Presenter - 讓 Unity 自動注入依賴
        _container.RegisterType(Of ClosingEntryPresenter)()

        ' 註冊 UserControl - 讓 Unity 可以動態建立
        _container.RegisterType(Of ucClosingEntry)()
    End Sub

    Public Shared Function Resolve(Of T)() As T
        Return _container.Resolve(Of T)()
    End Function

    Public Shared Sub Dispose()
        _container?.Dispose()
    End Sub
End Class
