# SuppliesToSpinTheWheelofChance – Original vs V2

A comparison between the **original** script and the **V2** upgrade. V2 is a drop‑in replacement with significant improvements, bug fixes, and new features.

---

## 📊 Comparison Table

| Feature | Original (v1) | V2 |
|---------|---------------|-----|
| **"All" option for SuppliesReward** | ❌ Bugged – only farms the first non‑maxed item, then stops. | ✅ Fixed – cycles through every non‑maxed reward until all are maxed. |
| **"All" option for SwindlesReturnItem** | ❌ Bugged – only farms the first non‑maxed item, then stops. | ✅ Fixed – re‑evaluates and moves to the next non‑maxed reward after each completion. |
| **Merge Buying from Swindle's Ripoff Emporium** | ❌ Not available. | ✅ New – automatically buys Tainted Gem, Dark Crystal Shard, Gem of Nulgath, Blood Gem, Totem, or Receipt when Unid10 is maxed. |
| **Auto‑buy Receipt of Swindle** | ❌ Not available. | ✅ New – if enabled, buys Receipts (300k gold each) when needed to craft the selected merge item. |
| **Voucher Handling** | ❌ Confusing – `KeepVoucher` applies to both member and non‑member vouchers. | ✅ Clear – `KeepMemberVoucher` explicitly controls **member** vouchers; non‑member vouchers are never sold. |
| **Voucher Item Quest** | ✅ Available – `VoucherItemQuestDuring` option. | ✅ Same – uses non‑member vouchers for Totem/Gem conversion. |
| **The Assistant Quest** | ✅ Available – `AssistantDuring` option. | ✅ Same – runs concurrently when enabled. |
| **UltraAlteon Target** | ✅ Available – `UltraAlteon` option. | ✅ Same – switches to Ultra Chaos Alteon. |
| **Buy Quantity Control** | ❌ Not available. | ✅ Added – `BuyQuantity` slider (1–100) controls how many merge items to buy per attempt. |
| **Requirements Checking** | ❌ Only checks Unidentified 10. | ✅ Full check – reads shop requirements (Unid10, Receipts, Gold) before buying. |
| **Shop Navigation** | ✅ Joins map and loads shop. | ✅ Same – improves reliability with explicit navigation. |
| **Error Handling** | ⚠️ Basic – stops on errors. | ✅ Improved – graceful fallbacks and clear logging. |
| **Code Quality** | ⚠️ Some unused methods. | ✅ Cleaner – removed redundancies, better structure. |
| **Configuration File** | Uses `SuppliesOptions.json`. | Uses `SuppliesOptionsV2.json` – separate from original, no conflicts. |

---

## 🆕 What's New in V2

### 1. Fixed "All" Mode
- In the original, selecting **All** for either quest would only farm the **first** non‑maxed item and then stop.
- In V2, the script **cycles** through every non‑maxed reward, farming each until maxed, then moving to the next.

### 2. Merge Shop Integration
- V2 automatically buys items from **Swindle's Ripoff Emporium** (Shop ID: 1951) when:
  - You have **1000 Unidentified 10**.
  - The selected merge item is not already maxed.
- Supported items: Tainted Gem, Dark Crystal Shard, Gem of Nulgath, Blood Gem of the Archfiend, Totem of Nulgath, Receipt of Swindle.

### 3. Auto‑Buy Receipt of Swindle
- Many merge items require **Receipt of Swindle**.
- V2 can automatically purchase missing Receipts using gold (300k each) if enabled.

### 4. Clear Voucher Controls
- Original: `KeepVoucher` was ambiguous – did it apply to member, non‑member, or both?
- V2: `KeepMemberVoucher` explicitly controls **member** vouchers. Non‑member vouchers are **never sold** – they are only used for the Totem conversion quest.

### 5. Better Requirements Checking
- V2 reads the **full shop item requirements** (Unid10, Receipts, Gold, etc.) before attempting to buy.
- It logs exactly what you have and what you're missing, so you know why a purchase failed.

---

## ⚙️ Migration from Original to V2

1. **Download** `SuppliesToSpinTheWheelofChanceV2.cs`.
2. **Place** it in your `Scripts` folder alongside the original (they can coexist).
3. **Reconfigure** your options – V2 uses a separate config file (`SuppliesOptionsV2.json`), so your original settings are preserved.
4. **Run** V2 and enjoy the new features.

> 💡 **Tip:** You can keep both scripts. The original works as before; V2 adds the extra functionality without overwriting anything.

---

## ❓ Which Version Should I Use?

| If you want... | Use... |
|----------------|--------|
| Simple farming of Supplies and Swindles only | Original |
| Farming with "All" mode working correctly | ✅ V2 |
| Automatic merge buying | ✅ V2 |
| Auto‑buy Receipt of Swindle | ✅ V2 |
| Clearer voucher controls | ✅ V2 |
| Better error logging and reliability | ✅ V2 |

**Recommendation:** Switch to V2 – it's a drop‑in replacement with all the fixes and features you need, with no downsides.

---

## ⚠️ Disclaimer

**Use at your own risk.** Automation may violate the Terms of Service of the game you are using this with. This script is provided for **educational and informational purposes only**. The author is not responsible for any account actions taken by the game's publisher, including but not limited to warnings, suspensions, or bans. By using this script, you accept full responsibility for your actions.

---

> Built with AI, driven by vibes. Choose V2 – it just works. 🎡
