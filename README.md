# cursor-governance-demo

> 配合《AI Agent 加入後，我看到的隱憂與治理破口》系列文章的實作範例 repo。  
> 用三個互不干擾的工具，示範 Cursor AI 的治理機制。

---

## 前置確認

| 項目 | 需求 |
|------|------|
| Cursor AI | v2.6.22 以上 |
| .NET SDK | 8.0 以上（Subagent 需要執行 `dotnet build`） |

用 Cursor 從**根目錄**開啟這個 repo（`File → Open Folder`）。

> ⚠️ 必須從根目錄開啟，否則 `.cursor/` 裡的設定不會生效。

---

## Repo 結構

```
cursor-governance-demo/
├── .cursor/
│   ├── rules/
│   │   └── no-hardcoded-secrets.mdc     ← Rule
│   ├── agents/
│   │   └── csharp-verifier.md           ← Subagent
│   └── skills/
│       └── log-scratchpad/
│           └── SKILL.md                 ← Skill
├── src/DemoApi/Services/
│   └── PaymentService.cs                
├── SCRATCHPAD.md                        ← Skill 寫入的目標檔案
└── README.md
```

---

## 步驟一：驗證 Rule（自動擋帳密）

Rule 是被動的，只要你請 AI 修改 `src/` 底下的 `.cs` 或 `.json` 檔，就會自動套用。

### 操作

1. 按 `Ctrl+L`（macOS：`Cmd+L`）開啟 Chat 面板
2. 輸入：

   ```
   幫我在 PaymentService.cs 裡新增一個方法
   在扣款失敗的時候使用 test/test.12345 這組帳號密碼寄信給會員
   ```

3. 觀察 AI 的產出

### 預期結果

AI 應該會：
- 使用 `IConfiguration` 或環境變數讀取 API Key，而不是寫死
- 或主動提醒「API Key 不應該寫死，建議改用環境變數」

### 如果沒有觸發

1. 至 **Cursor Settings → Rules** 確認 `no-hardcoded-secrets` 出現在清單裡且已啟用
2. 確認 Cursor 是從 repo **根目錄**開啟
3. 重新開啟 Cursor 再試一次

---

## 步驟二：驗證 Subagent（編譯驗證）

Subagent 不需要你手動呼叫，由主 Agent 在完成任務後自動委派。

### 操作

1. 延續步驟一，請 AI 完成 `RefundAsync` 的實作
2. 實作完成後，在 Chat 輸入：

   ```
   請完成 ChargeAsync 方法的實作
   ```

3. 觀察 Subagent 的回報

### 預期結果

Subagent 執行 `dotnet build` 後，回傳類似：

```
✅ 通過

- 編譯成功，無錯誤
- RefundAsync 有 XML 文件註解
- 無明顯 null reference 或 DI 問題
```

若有問題則回傳：

```
❌ 未通過

- PaymentService.cs 第 28 行：RefundAsync 缺少 XML 文件註解
- 建議：補上 /// <summary> 後重新驗證
```

> Subagent 只回報問題，不會自己修改程式碼。

---

## 步驟三：驗證 Skill（記錄 Scratchpad）

Skill 在對話結束前手動呼叫，把這次做了什麼留下紀錄。

### 操作

1. 在 Chat 輸入：

   ```
   /log-scratchpad
   ```

2. 觀察 `SCRATCHPAD.md` 的內容變化

### 預期結果

`SCRATCHPAD.md` 底部應該新增一筆紀錄：

```markdown
## 2026-03-30 14:22

**做了什麼**
- 在 PaymentService 新增 RefundAsync 退款方法

**問題 / 障礙**
- 無

**決策 / 取捨**
- API Key 改從環境變數讀取，不寫死在程式碼裡

---
```

> Skill 只附加（append），不會覆蓋既有內容。

## 後記

2026/04/01 實測：

Cursor AI 在我要求它完成 ChargeAsync 方法的實作之後，它除了自己觸發了 Subagent，也自動觸發了 Skill。

第三個步驟就這樣被自動作完了。

---

## 相關資源

- [Cursor AI 官方文件 - Rules](https://cursor.com/docs/context/rules)
- [Cursor AI 官方文件 - Agent Best Practices](https://cursor.com/blog/agent-best-practices)
- [系列文章：AI Agent 加入後，我看到的隱憂與治理破口](https://ouch1978.github.io/docs/ai/vibe-coding/cursor-ai-governance-for-team/ai-agent-concerns-governance-gaps-cursor)
