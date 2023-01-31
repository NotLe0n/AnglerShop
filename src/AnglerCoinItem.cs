using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace NoFishingQuests;

public class AnglerCoinItem : ModItem
{
	public override void SetStaticDefaults()
	{
		Tooltip.SetDefault("The currency to buy items from the Angler's Shop");
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
	}

	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;

		Item.rare = -11;
		Item.maxStack = 999;
		Item.value = Item.buyPrice(gold: 1);
	}

	public override void Load()
	{
		On.Terraria.UI.ItemSlot.PickItemMovementAction += AllowCoinSlotPlacement;
	}

	private int AllowCoinSlotPlacement(On.Terraria.UI.ItemSlot.orig_PickItemMovementAction orig, Item[] inv, int context, int slot, Item checkItem)
	{
		if (context == 1 && checkItem.type == ModContent.ItemType<AnglerCoinItem>()) {
			return 0;
		}

		return orig(inv, context, slot, checkItem);
	}
}