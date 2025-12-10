# 豐原瓦斯會計軟體系統

## 專案簡介

瓦斯會計軟體系統，使用 WinForms 開發的桌面應用程式，採用 MVP 架構設計。

## 技術架構

- 開發環境：.NET Framework 4.7.2
- UI 框架：Windows Forms
- 設計模式：MVP (Model-View-Presenter)
- ORM：Entity Framework 6.5.1
- 資料庫：MySQL 8.0
- 相依性注入：Unity 5.11.10

## 目錄結構

```plaintext
├── Forms/          # WinForm 視圖檔案
├── Presenters/     # Presenter 類別
├── Models/         # 資料模型
├── Services/       # 業務邏輯服務
├── Repositories/   # 資料存取層
├── Utilities/      # 共用工具類別
└── Data/           # DbContext 和資料庫相關設定
```

## 開發規範

### MVP 實作規範

1. 視圖 (View)
   - 命名規範：`frm<功能名稱>` (例: frmLogin)
   - 必須實作對應的介面 (例: ILoginView)
   - 不包含業務邏輯
   - 只負責 UI 顯示和使用者輸入

2. Presenter
   - 命名規範：`<功能名稱>Presenter` (例: LoginPresenter)
   - 處理所有業務邏輯
   - 通過介面與視圖溝通
   - 負責資料驗證和錯誤處理

3. Model
   - 命名規範：`<實體名稱>Model` (例: UserModel)
   - 純資料類別
   - 不包含業務邏輯
   - 可以包含資料驗證

### Entity Framework 規範

1. 資料庫存取
   - 使用 Repository Pattern
   - 實作 Unit of Work Pattern
   - 所有 Entity 類別放在 Models 資料夾
   - DbContext 類別放在 Data 資料夾

2. 命名規範
   - DbContext: `GasAccountingContext`
   - Repository: `I<Entity>Repository` 和 `<Entity>Repository`
   - Entity 類別: 單數形式，例如 `Customer`，而不是 `Customers`

3. 查詢規範
   - 使用非同步方法 (async/await)
   - 大量資料查詢需實作分頁
   - 避免 N+1 查詢問題
   - 需要時使用 Include 進行預載入

4. 資料驗證
   - 使用 Data Annotations
   - 複雜驗證邏輯放在 Model 類別中
   - 必要時實作 IValidatableObject

### 程式碼規範

1. 錯誤處理
   - 使用 Try-Catch 處理所有可能的異常
   - 統一的錯誤訊息格式
   - 記錄異常到日誌系統

2. 註解規範
   - 所有公開方法必須加上註解
   - 複雜的業務邏輯需要詳細註解
   - 使用 XML 文件註解格式

3. 程式碼風格
   - 使用 4 個空格進行縮排
   - 最大行寬：120 字元
   - 使用 VB.NET 官方建議的程式碼風格指南

## 開發環境需求

- Visual Studio 2019 或更新版本
- .NET Framework 4.7.2 SDK
- MySQL 8.0
- Git

## 建置說明

1. Clone 專案
2. 還原 NuGet 套件
3. 更新資料庫連線字串
4. 執行資料庫遷移
5. 編譯並執行

## 注意事項

- 提交前必須通過所有單元測試
- 遵循 Git Flow 工作流程
- 定期更新 NuGet 套件
