# SuppliesToSpinTheWheelofChance – Original vs V2

A comparison between the **original** script and the **V2** upgrade. V2 is a drop‑in replacement with significant improvements, bug fixes, and new features.

---

## 📊 Comparison Table

| Feature | Original (v1) | V2 |
|---------|---------------|-----|
| **"All" option for SuppliesReward** | ❌ Bugged – only farms the first non‑maxed item, then stops. | ✅ Fixed – cycles through every non‑maxed reward until all are maxed. |
| **"All" option for SwindlesReturnItem** | ❌ Bugged – only farms the first non‑maxed item, then stops. | ✅ Fixed – re‑evaluates and moves to the next non‑maxed reward after each completion. |
| **Merge Buying from Swindle's Ripoff Emporium** | ❌ Not available. | ✅ New – automatically buys Tainted Gem, Dark Crystal Shard, Gem of Nulgath, Blood Gem, Totem, or Receipt when Unid10 is maxed. |
| **Auto‑buy Receipt of Swindle** | ❌ Not available. | ✅ New – buys Receipts (300k gold each) as needed to maximize Unid10 spending. |
| **Voucher Handling** | ❌ Confusing – `KeepVoucher` applies to both member and non‑member vouchers. | ✅ Clear – `KeepMemberVoucher` explicitly controls **member** vouchers; non‑member vouchers are never sold. |
| **Voucher Item Quest** | ✅ Available – `VoucherItemQuestDuring` option. | ✅ Same – uses non‑member vouchers for Totem/Gem conversion. |
| **The Assistant Quest** | ✅ Available – `AssistantDuring` option. | ✅ Same – runs concurrently when enabled. |
| **UltraAlteon Target** | ✅ Available – `UltraAlteon` option. | ✅ Same – switches to Ultra Chaos Alteon. |
| **Configuration File** | Uses `SuppliesOptions.json`. | Uses `SuppliesOptionsV2.json` – separate from original, no conflicts. |

---

## 🆕 What's New in V2

### 1. Fixed "All" Mode
- In the original, selecting **All** for either quest would only farm the **first** non‑maxed item and then stop.
- In V2, the script **cycles** through every non‑maxed reward, farming each until maxed, then moving to the next.

### 2. Merge Shop Integration
- V2 automatically buys items from **Swindle's Ripoff Emporium** when:
  - You have **1000 Unidentified 10**.
  - The selected merge item is not already maxed.
- Supported items: Tainted Gem, Dark Crystal Shard, Gem of Nulgath, Blood Gem of the Archfiend, Totem of Nulgath, Receipt of Swindle.

### 3. Auto‑Buy Receipt of Swindle (The "Calculator")
- Many merge items require **Receipt of Swindle**.
- V2 calculates how many Receipts you need to spend all your Unidentified 10.
- If you don't have enough Receipts, it automatically buys the missing amount using gold (300k each).
- This maximizes your Unid10 spending without wasting resources.

### 4. Clear Voucher Controls
- Original: `KeepVoucher` was ambiguous – did it apply to member, non‑member, or both?
- V2: `KeepMemberVoucher` explicitly controls **member** vouchers. Non‑member vouchers are **never sold** – they are only used for the Totem conversion quest.

### 5. Better Error Logging
- V2 logs exactly what you're missing (Unid10, Receipts, Gold) so you know why a purchase failed.

---

## ⚙️ How Merge Buying Works in V2

1. When **Unidentified 10** reaches **1000**, the script triggers merge buying.
2. It checks how many of the selected item you already have.
3. It calculates how many more you can buy (up to max stack).
4. For each purchase, it checks if you have enough **Receipt of Swindle**.
5. If not, and `BuyReceiptsIfNeeded` is enabled, it buys the missing Receipts using gold.
6. It buys **one item at a time** and repeats until:
   - The item is maxed, or
   - You run out of Unidentified 10, or
   - You run out of gold (if receipts are needed), or
   - You run out of Receipts (if auto‑buy is off).

This ensures you use your resources efficiently without wasting gold or materials.

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

**Use at your own risk.** Botting may violate the Terms of Service of AdventureQuest Worlds (AQW). This script is provided for **educational and informational purposes only**. The author is not responsible for any account actions taken by Artix Entertainment, including but not limited to warnings, suspensions, or bans. By using this script, you accept full responsibility for your actions.

---

> Built with AI, driven by vibes. Choose V2 – it just works. 🎡
