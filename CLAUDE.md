# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 專案概覽

**豐原瓦斯會計軟體系統** — VB.NET WinForms 桌面應用程式，採用 MVP 架構，管理瓦斯公司的收款、付款、會計分錄、應收支票等業務。

- 技術棧：VB.NET / .NET Framework 4.7.2 / WinForms
- 資料庫：MySQL 8.0，透過 Entity Framework 6.5.1 存取
- 相依性注入：Unity 5.11.10
- 日誌：NLog（輸出至 `bin/Logs/`，保留 30 天）

## 建置與執行

使用 Visual Studio 2022 開啟 `GasAccounting.sln`：

1. 還原 NuGet 套件（Tools → NuGet Package Manager → Restore）
2. 確認 `App.config` 中的連線字串指向正確的 MySQL 實例：
   ```
   server=localhost;user id=root;Password=;database=gas_accounting_system;charset=utf8mb4;SslMode=none
   ```
3. 編譯：`Ctrl+Shift+B`
4. 執行：`F5`（輸出至 `bin/Debug/GasAccounting.exe`）

執行測試（MSTest + Moq）：測試總管 → 執行全部，或對 `GasAccountingTest/` 專案右鍵 → Run Tests。

## 架構

採用 MVP + Repository + UnitOfWork 分層架構：

```
Forms (實作 IxxxView) 
    ↕ 介面溝通
Presenters（業務邏輯、資料驗證）
    ↕ 呼叫
Services（跨 Repository 的業務流程）
    ↕ 呼叫
Repositories（IXxxRep / XxxRep）
    ↕ EF DbContext
MySQL 資料庫（gas_accounting_system）
```

`DependencyContainer.vb` 初始化 Unity 容器，統一註冊所有 Repository 和 Service。DbContext 為 `gas_accounting_systemEntities`（EDMX 自動生成）。

## 命名規範

| 類型 | 規則 | 範例 |
|------|------|------|
| Form | `frm<功能名稱>` | `frmLogin` |
| View 介面 | `I<功能名稱>View` | `ILoginView` |
| Presenter | `<功能名稱>Presenter` | `LoginPresenter` |
| Repository 介面 | `I<Entity>Rep` | `IBankRep` |
| Repository 實作 | `<Entity>Rep` | `BankRep` |
| 類別/方法 | PascalCase | `GetAllCustomers` |
| 變數 | camelCase | `customerName` |

## 重要規則

**UI 修改限制**：不允許直接修改 WinForms UI 設計（`.Designer.vb` 及 Form 的拖拉控件配置），所有建議的 UI 修改須在聊天視窗中說明，由開發者手動操作 Visual Studio Designer。

**資料庫操作**：所有 DB 存取必須透過 Repository 層，禁止在 Presenter 或 Service 層直接使用 `DbContext`。

**業務邏輯位置**：業務邏輯集中在 Presenter/Service 層；View 層（Form）只做 UI 顯示和事件觸發，不含任何邏輯。

**查詢效能**：唯讀查詢使用 `AsNoTracking()`；避免迴圈內查詢資料庫；需要關聯資料時使用 `Include` 預載入。

## 新增功能的標準流程

新增一個功能模組（例如新的管理功能）需依序建立：
1. `Views/IXxxView.vb` — 介面定義屬性與事件
2. `Forms/frmXxx.vb` — 實作介面，不含業務邏輯
3. `Repositories/Xxx/IXxxRep.vb` + `XxxRep.vb` — 資料存取
4. `Services/Xxx/IXxxService.vb` + `XxxService.vb`（視複雜度決定是否需要）
5. `Presenters/XxxPresenter.vb` — 處理事件與業務邏輯
6. 在 `DependencyContainer.vb` 中註冊新的 Repository/Service
7. `GasAccountingTest/` 中加入對應的單元測試
