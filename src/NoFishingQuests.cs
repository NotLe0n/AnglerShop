using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.ID;

namespace NoFishingQuests;

internal class NoFishingQuests : Mod
{
	public override void Load()
	{
		AnglerCoin.Id = CustomCurrencyManager.RegisterCurrency(new AnglerCoin(ModContent.ItemType<AnglerCoinItem>(), 999));

		Terraria.On_Player.GetAnglerReward += AddAnglerCoinToQuestReward;
		
		base.Load();
	}

	private void AddAnglerCoinToQuestReward(On_Player.orig_GetAnglerReward orig, Player self, NPC angler, int questItemType)
	{
		orig(self, angler, questItemType);
			
		if (!Config.Instance.useCustomCurrency) {
			return;
		}

		int type = ModContent.ItemType<AnglerCoinItem>();
		int stack = Main.rand.Next(1, 5);
		int index = Item.NewItem(new EntitySource_Gift(angler), (int)self.position.X, (int)self.position.Y,
			self.width, self.height, type, stack, false, 0, true);

		if (Main.netMode == NetmodeID.MultiplayerClient) {
			NetMessage.SendData(MessageID.SyncItem, -1, -1, null, index, 1f);
		}
	}
}