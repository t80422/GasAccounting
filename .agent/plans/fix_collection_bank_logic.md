Step Id: 21
## Problem
經過檢查 `Presenters/CollectionPresenter.vb` 中的 `Edit` 方法，發現兩個主要問題：

1. **餘額調整方向錯誤**：
   當處理「科目」(`col_s_Id`) 為「銀行存款」時，程式錯誤地使用了 `debitDelta`（收入/借方）。
   作為科目（貸方/資金來源），銀行存款應該被視為「支出」或「資產減少」。
   - **新增**銀行科目：應增加支出（減少餘額） -> `creditDelta = newAmount`。
   - **移除**銀行科目：應減少支出（回補餘額） -> `creditDelta = -oldAmount`。

2. **觸發條件過於嚴苛 (Bug)**：
   `Edit` 方法中的判斷式 `If oldBankId.HasValue AndAlso col.col_bank_Id.HasValue Then` 使用了 `AndAlso`。
   這意味著如果「舊資料」或「新資料」其中一方沒有銀行 ID（例如從現金改為銀行轉帳），程式就會**跳過**餘額更新，導致資料不一致。

## Solution
修正 `Edit` 方法的觸發條件與 `AdjustBankMonthlyBalanceForBankSubjectAsync` 的內部邏輯。

具體規則：
1. **修正 Edit 觸發條件**：
   不應強制要求兩者都有值。應該判斷「舊科目是銀行」或「新科目是銀行」，並在呼叫 `Adjust...` 時處理 BankId 可能為空的情況（通常有科目為銀行時，Validate 應確保有 BankId，若無則視為 0 或跳過該邊）。
   
2. **修正 Adjust 邏輯**：
   將所有針對科目（貸方）的調整改為使用 `creditDelta`。
   - 同銀行同月份修改金額：`creditDelta = newAmount - oldAmount`。
   - 不同銀行/月份：
     - 舊銀行：`creditDelta = -oldAmount` (回補)。
     - 新銀行：`creditDelta = newAmount` (扣除)。

## Plan Steps
1. [ ] 修改 `Presenters/CollectionPresenter.vb` 中的 `Edit` 方法：
    - 移除 `If oldBankId.HasValue AndAlso col.col_bank_Id.HasValue` 限制。
    - 改為檢查 Subjects 是否為 "銀行存款"，並安全地取得 BankId (使用 GetValueOrDefault)。
    - 呼叫 `AdjustBankMonthlyBalanceForBankSubjectAsync`。
2. [ ] 修改 `Presenters/CollectionPresenter.vb` 中的 `AdjustBankMonthlyBalanceForBankSubjectAsync` 方法：
    - 將參數傳遞給 `UpdateMonthBalanceIncrementalAsync` 的邏輯從 `debitDelta` 改為 `creditDelta`。
    - 確保 `oldBankId` 和 `newBankId` 參數能正確接收（或在呼叫端處理好 0 的情況）。
