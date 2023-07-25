using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoFishingQuests;

public class ModifyShops : GlobalNPC
{
	public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
	{
		if (npc.type != NPCID.Angler) {
			return;
		}

		for (int i = 0; i < Chest.maxItems; i++) {
			items[i] = GetAnglerShopItems()[i];
		}
	}

	private static Item[] GetAnglerShopItems()
	{
		// all items being sold:
		(short id, int price)[] items = {
			(ItemID.SonarPotion, 2),
			(ItemID.FishingPotion, 2),
			(ItemID.CratePotion, 2),
			(ItemID.ApprenticeBait, 3),
			(ItemID.JourneymanBait, 10),
			(ItemID.MasterBait, 15),
			(ItemID.FuzzyCarrot, 10),
			(ItemID.AnglerHat, 8),
			(ItemID.AnglerVest, 8),
			(ItemID.AnglerPants, 8),
			(ItemID.FishHook, 30),
			(ItemID.GoldenFishingRod, 50),
			(ItemID.GoldenBugNet, 50),
			(ItemID.HighTestFishingLine, 8),
			(ItemID.AnglerEarring, 8),
			(ItemID.TackleBox, 8),
			(ItemID.FishermansGuide, 15),
			(ItemID.WeatherRadio, 15),
			(ItemID.Sextant, 15),
			(ItemID.SeashellHairpin, 4),
			(ItemID.MermaidAdornment, 4),
			(ItemID.MermaidTail, 4),
			(ItemID.FishCostumeMask, 4),
			(ItemID.FishCostumeShirt, 4),
			(ItemID.FishCostumeFinskirt, 4),
			(ItemID.BunnyfishTrophy, 1),
			(ItemID.GoldfishTrophy, 1),
			(ItemID.SharkteethTrophy, 1),
			(ItemID.TreasureMap, 1),
			(ItemID.SeaweedPlanter, 1),
			(ItemID.PillaginMePixels, 1),
			(ItemID.CompassRose, 1),
			(ItemID.ShipsWheel, 1),
			(ItemID.ShipInABottle, 1),
			(ItemID.LifePreserver, 1),
			(ItemID.WallAnchor, 1),
			(ItemID.BottomlessBucket, 40),
			(ItemID.SuperAbsorbantSponge, 40),
			(ItemID.FinWings, 40),
			(ItemID.HotlineFishingHook, 40)
		};

		// some items are only sold if WoF has been defeated
		if (!Main.hardMode) {
			items[^1].id = ItemID.None;
			items[^2].id = ItemID.None;
		}

		// add items to the list
		Item[] itemsArr = new Item[40];
		for (int i = 0; i < items.Length; i++) {
			itemsArr[i] = new Item(items[i].id) {
				isAShopItem = true,
				shopCustomPrice = items[i].price
			};

			if (Config.Instance.useCustomCurrency) {
				itemsArr[i].shopSpecialCurrency = AnglerCoin.Id;
			}
			else {
				itemsArr[i].shopCustomPrice *= 6700;
			}
		}

		return itemsArr;
	}
}