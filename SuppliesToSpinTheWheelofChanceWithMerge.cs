/*
name: SuppliesToSpinTheWheelofChanceV2
description: Do "Supplies to Spin the Wheel" with merge buying from Swindle's Ripoff Emporium (v2).
tags: swindles return policy, supplies to spin the wheel, swindles bilk, the assistant, nulgath, nation, supplies, Ultra Chaos Alteon, escherion, merge, ripoff emporium
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/Nation/CoreNation.cs
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;
using Skua.Core.Models.Quests;
using Skua.Core.Options;

public class SuppliesToSpinTheWheelofChanceV2
{
    private IScriptInterface Bot => IScriptInterface.Instance;
    private static CoreBots Core => CoreBots.Instance;
    private static CoreNation Nation
    {
        get => _Nation ??= new CoreNation();
        set => _Nation = value;
    }
    private static CoreNation _Nation;

    public string OptionsStorage = "SuppliesOptionsV2";
    public bool DontPreconfigure = true;
    public List<IOption> Options = new()
    {
        CoreBots.Instance.SkipOptions,
        new Option<SwindlesReturnItem>(
            "SwindlesReturnItem",
            "SwindlesReturnItem",
            "pick the reward for the \"Swindles Return\" Quest",
            SwindlesReturnItem.All
        ),
        new Option<SuppliesReward>(
            "SuppliesReward",
            "SuppliesReward",
            "pick the reward for the \"Supplies to spin the wheel\" Quest",
            SuppliesReward.All
        ),
        new Option<bool>(
            "AssistantDuring",
            "Do: \"The Assistant\" during?",
            "Do the quest: [The Assistant], (requires alota gold, that you will get from the vouchers of nulgath (mem)) during this.",
            false
        ),
        new Option<bool>(
            "UltraAlteon",
            "Kill \"UltraAlteon\"",
            "Instead of \"Escherion\" or bamboozle, do \"Ultra Chaos Alteon\"?",
            false
        ),
        new Option<bool>(
            "VoucherItemQuestDuring",
            "Do `Voucher Item: Totem of Nulgath` During?",
            "Do Voucher Item: Totem of Nulgath During? (uses non‑member voucher)",
            false
        ),
        new Option<bool>(
            "KeepMemberVoucher",
            "Keep Member Voucher? (sell for gold if false)",
            "If true, keeps the member Voucher of Nulgath. If false, sells it for gold. (Non‑member voucher is never sold.)",
            false
        ),
        // Merge buying options
        new Option<RipoffEmporiumItem>(
            "BuyFromRipoffEmporium",
            "Buy from Swindle's Ripoff Emporium",
            "Buy this item from the merge shop when Unidentified 10 is maxed.",
            RipoffEmporiumItem.None
        ),
        new Option<bool>(
            "BuyReceiptsIfNeeded",
            "Buy Receipts if needed?",
            "If true, will buy Receipt of Swindle (300k gold each) if you don't have enough to craft the selected item.",
            true
        ),
    };

    public void ScriptMain(IScriptInterface bot)
    {
        Core.BankingBlackList.AddRange(Nation.SuppliesRewards.Concat(Nation.SwindlesReturnRewards));
        Core.SetOptions();

        DoSupplies();

        Core.SetOptions(false);
    }

    public void DoSupplies()
    {
        string? swindlesReturnItem = GetNormalizedConfigItem<SwindlesReturnItem>("SwindlesReturnItem", out bool maxSwindles);
        string? suppliesItem = GetNormalizedConfigItem<SuppliesReward>("SuppliesReward", out bool maxSupplies);
        RipoffEmporiumItem buyTarget = Bot.Config!.Get<RipoffEmporiumItem>("BuyFromRipoffEmporium");
        bool buyReceipts = Bot.Config!.Get<bool>("BuyReceiptsIfNeeded");

        // Read the renamed option
        bool keepMemberVoucher = Bot.Config!.Get<bool>("KeepMemberVoucher");

        Quest supplies = LoadQuestWithRetry(2857, "Supplies");
        Quest swindlesReturn = LoadQuestWithRetry(7551, "Swindle's Return");

        List<ItemBase> combinedRewards = BuildCombinedRewardsList(supplies, swindlesReturn);

        if (suppliesItem != null)
        {
            int maxStack = GetRewardMaxStack(supplies, suppliesItem);
            if (maxStack > 0 && !Core.CheckInventory(GetItemId(supplies, suppliesItem), maxStack))
            {
                Core.FarmingLogger(suppliesItem, maxStack);
                Nation.Supplies(
                    suppliesItem,
                    maxStack,
                    Bot.Config!.Get<bool>("UltraAlteon"),
                    keepMemberVoucher,
                    Bot.Config!.Get<bool>("AssistantDuring"),
                    swindlesReturnItem,
                    swindlesReturnItem != null,
                    Bot.Config!.Get<bool>("VoucherItemQuestDuring")
                );
            }
            if (buyTarget != RipoffEmporiumItem.None)
                BuyFromRipoffEmporium(buyTarget, buyReceipts);
            return;
        }

        foreach (ItemBase item in combinedRewards.Where(r => Nation.SuppliesRewards.Contains(r.Name)))
        {
            if (Core.CheckInventory(item.ID, item.MaxStack))
                continue;

            Core.FarmingLogger(item.Name, item.MaxStack);

            string? currentSwindles = swindlesReturnItem;
            if (currentSwindles == null && maxSwindles)
            {
                currentSwindles = GetNextNonMaxedReward(swindlesReturn, Nation.SwindlesReturnRewards);
            }

            Nation.Supplies(
                item.Name,
                item.MaxStack,
                Bot.Config!.Get<bool>("UltraAlteon"),
                keepMemberVoucher,
                Bot.Config!.Get<bool>("AssistantDuring"),
                currentSwindles,
                currentSwindles != null,
                Bot.Config!.Get<bool>("VoucherItemQuestDuring")
            );

            if (buyTarget != RipoffEmporiumItem.None)
                BuyFromRipoffEmporium(buyTarget, buyReceipts);
        }

        if (maxSwindles)
        {
            foreach (ItemBase item in combinedRewards.Where(r => Nation.SwindlesReturnRewards.Contains(r.Name)))
            {
                if (Core.CheckInventory(item.ID, item.MaxStack))
                    continue;

                Core.FarmingLogger(item.Name, item.MaxStack);
                string? dummySupplies = GetNextNonMaxedReward(supplies, Nation.SuppliesRewards);
                if (dummySupplies == null)
                {
                    Core.Logger("All Supplies items are maxed – cannot continue Swindles farming.");
                    break;
                }
                int dummyMaxStack = GetRewardMaxStack(supplies, dummySupplies);
                Nation.Supplies(
                    dummySupplies,
                    dummyMaxStack,
                    Bot.Config!.Get<bool>("UltraAlteon"),
                    keepMemberVoucher,
                    Bot.Config!.Get<bool>("AssistantDuring"),
                    item.Name,
                    true,
                    Bot.Config!.Get<bool>("VoucherItemQuestDuring")
                );
            }
        }
    }

    private string? GetNormalizedConfigItem<T>(string configKey, out bool isMaxAll) where T : Enum
    {
        string? item = Bot.Config!.Get<T>(configKey)?.ToString()?.Replace('_', ' ');
        isMaxAll = item == "All";
        return isMaxAll ? null : item;
    }

    private Quest LoadQuestWithRetry(int questId, string questName)
    {
        while (true)
        {
            Quest? quest = Core.InitializeWithRetries(() => Bot.Quests.EnsureLoad(questId));
            if (quest != null)
                return quest;

            Core.Logger($"Failed to load quest {questId} ({questName}). Retrying...");
            Core.Sleep();
        }
    }

    private List<ItemBase> BuildCombinedRewardsList(Quest supplies, Quest swindlesReturn)
    {
        List<ItemBase> combined = new();
        combined.AddRange(
            supplies.Rewards
                .Where(r => r != null &&
                            Nation.SuppliesRewards.Contains(r.Name) &&
                            !Core.CheckInventory(r.ID, r.MaxStack))
                .DistinctBy(r => r.ID)
        );
        combined.AddRange(
            swindlesReturn.Rewards
                .Where(r => r != null &&
                            Nation.SwindlesReturnRewards.Contains(r.Name) &&
                            !Core.CheckInventory(r.ID, r.MaxStack))
                .DistinctBy(r => r.ID)
        );
        return combined.DistinctBy(r => r.ID).ToList();
    }

    private string? GetNextNonMaxedReward(Quest quest, string[] validRewards)
    {
        if (quest?.Rewards == null)
            return null;

        var allItems = Bot.Inventory.Items.Concat(Bot.Bank.Items);

        return quest.Rewards
            .Where(r => r != null &&
                        validRewards.Contains(r.Name) &&
                        !allItems.Any(i => i.ID == r.ID && i.Quantity >= r.MaxStack))
            .Select(r => r.Name)
            .FirstOrDefault();
    }

    private int GetRewardMaxStack(Quest quest, string itemName)
    {
        ItemBase? reward = quest.Rewards?.FirstOrDefault(x => x != null && x.Name == itemName);
        return reward?.MaxStack ?? 0;
    }

    private int GetItemId(Quest quest, string itemName)
    {
        ItemBase? reward = quest.Rewards?.FirstOrDefault(x => x != null && x.Name == itemName);
        return reward?.ID ?? 0;
    }

    // --- UPDATED: Buys merge item, auto-buys Receipts if needed ---
    private void BuyFromRipoffEmporium(RipoffEmporiumItem target, bool buyReceipts)
    {
        if (target == RipoffEmporiumItem.None)
            return;

        // Get the item name and its max stack from shop
        string itemName = target.ToString().Replace('_', ' ');
        var shopItems = Core.GetShopItems("tercessuinotlim", 1951);
        var shopItem = shopItems.FirstOrDefault(x => x.Name == itemName);
        if (shopItem == null)
        {
            Core.Logger($"Item '{itemName}' not found in shop.");
            return;
        }
        int maxStack = shopItem.MaxStack;
        int currentQty = Bot.Inventory.GetQuantity(itemName);
        if (currentQty >= maxStack)
        {
            Core.Logger($"{itemName} is already at max stack ({maxStack}).");
            return;
        }
        int remainingCapacity = maxStack - currentQty;

        // Parse requirements
        int unid10PerItem = 0;
        int receiptPerItem = 0;
        foreach (var req in shopItem.Requirements)
        {
            if (req.Name == "Unidentified 10")
                unid10PerItem = req.Quantity;
            else if (req.Name == "Receipt of Swindle")
                receiptPerItem = req.Quantity;
        }

        // Current resources
        int unid10Have = Bot.Inventory.GetQuantity("Unidentified 10");
        int receiptHave = Bot.Inventory.GetQuantity("Receipt of Swindle");
        int goldHave = Bot.Player.Gold;

        // Max by Unid10
        int maxByUnid10 = unid10PerItem > 0 ? unid10Have / unid10PerItem : int.MaxValue;
        // Max by Receipts
        int maxByReceipt = receiptPerItem > 0 ? receiptHave / receiptPerItem : int.MaxValue;
        // Max by capacity
        int maxByCapacity = remainingCapacity;

        int maxPossible = Math.Min(maxByUnid10, Math.Min(maxByCapacity, maxByReceipt));

        // If we need Receipts and auto-buy is enabled, try to buy more Receipts to increase maxPossible
        if (receiptPerItem > 0 && buyReceipts)
        {
            int neededReceipts = maxPossible * receiptPerItem;
            int shortfall = neededReceipts - receiptHave;
            if (shortfall > 0)
            {
                int maxReceiptsToBuy = (int)(goldHave / 300000);
                int maxReceiptStack = 3;
                int currentReceipts = receiptHave;
                int canBuyReceipts = Math.Min(maxReceiptsToBuy, maxReceiptStack - currentReceipts);
                if (canBuyReceipts > 0)
                {
                    int buyAmount = Math.Min(canBuyReceipts, shortfall);
                    Core.Logger($"Buying {buyAmount} Receipt of Swindle (cost: {buyAmount * 300000} gold)...");
                    Core.BuyItem("tercessuinotlim", 1951, "Receipt of Swindle", buyAmount);
                    Core.Sleep(500);
                    // Recalculate after buying Receipts
                    receiptHave = Bot.Inventory.GetQuantity("Receipt of Swindle");
                    maxByReceipt = receiptPerItem > 0 ? receiptHave / receiptPerItem : int.MaxValue;
                    maxPossible = Math.Min(maxByUnid10, Math.Min(maxByCapacity, maxByReceipt));
                }
                else
                {
                    // Can't buy enough receipts, cap by current receipts
                    maxPossible = Math.Min(maxPossible, receiptHave / receiptPerItem);
                }
            }
        }
        else if (receiptPerItem > 0 && !buyReceipts)
        {
            maxPossible = Math.Min(maxPossible, receiptHave / receiptPerItem);
        }

        if (maxPossible <= 0)
        {
            Core.Logger($"Cannot buy any {itemName} – insufficient resources.");
            return;
        }

        // --- Bulk buy ---
        Core.Logger($"Buying {maxPossible}x {itemName} (requires {maxPossible * unid10PerItem} Unid10, {maxPossible * receiptPerItem} Receipts)...");
        int before = Bot.Inventory.GetQuantity(itemName);
        Core.BuyItem("tercessuinotlim", 1951, itemName, maxPossible);
        int after = Bot.Inventory.GetQuantity(itemName);
        if (after > before)
            Core.Logger($"Successfully bought {after - before}x {itemName}. Now have {after}/{maxStack}.");
        else
            Core.Logger("Purchase failed – quantity did not increase.");
}

    // ===== ENUMS (limited to merge items) =====

    public enum SwindlesReturnItem
    {
        All,
        Tainted_Gem,
        Dark_Crystal_Shard,
        Diamond_of_Nulgath,
        Gem_of_Nulgath,
        Blood_Gem_of_the_Archfiend,
        Receipt_of_Swindle,
    }

    public enum SuppliesReward
    {
        All,
        Tainted_Gem,
        Dark_Crystal_Shard,
        Diamond_of_Nulgath,
        Voucher_of_Nulgath,
        Voucher_of_Nulgath_NonMem,
        Gem_of_Nulgath,
        Unidentified_10,
        Essence_of_Nulgath,
    }

    // Only the items you want to buy from the merge
    public enum RipoffEmporiumItem
    {
        None,
        Tainted_Gem,
        Dark_Crystal_Shard,
        Gem_of_Nulgath,
        Blood_Gem_of_the_Archfiend,
        Totem_of_Nulgath,
        Receipt_of_Swindle,
    }
}