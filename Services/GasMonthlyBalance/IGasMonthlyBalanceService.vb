Public Interface IGasMonthlyBalanceService
    ' 舊方法（使用構造函數注入的 Repository）
    Sub UpdateOrAdd(month As Date, companyId As Integer)

    ' 新方法（接受 Repository 參數，給 UnitOfWork 使用）
    Sub UpdateOrAdd(gmbRep As IGasMonthlyBalanceRep, ordRep As IOrderRep, compRep As ICompanyRep, bsRep As IBasicSetRep, month As Date, companyId As Integer)
End Interface
