using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NoFishingQuests;

internal class NoFishingQuestsUI : UIState
{
	// Main._textDisplayCache
	private static object TextDisplayCache => typeof(Main).GetField("_textDisplayCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(Main.instance);
	private bool focused; // true if the shop button is hovered

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		// Source: Main.DrawNPCChatButtons

		Vector2 scale = new(0.9f); // scale of the button
		string text = Language.GetTextValue("LegacyInterface.28"); // "Shop"
																   // How long the npc text is (varies with language)
		int numLines = (int)TextDisplayCache.GetType().GetProperty("AmountOfLines", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).GetValue(TextDisplayCache) + 1;
		Color col = new(228, 206, 114, Main.mouseTextColor / 2); // color of the button text
		Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, scale); // size of "Shop" with scale 0.9

		// vanilla did that, idk
		Vector2 value2 = new(1f);
		if (stringSize.X > 260f)
			value2.X *= 260f / stringSize.X;

		// button positions
		float posButton1 = 180 + (Main.screenWidth - 800) / 2; // position of the first button (Quest)
		float posButton2 = posButton1 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.64"), scale).X + 30f; // Position of the second button (Close)
		float posButton3 = posButton2 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.52"), scale).X + 30f; // Position of the third button (Happiness)
		float posButton4 = posButton3 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("UI.NPCCheckHappiness"), scale).X + 30f; // Position of the new button
		Vector2 position = new(posButton4, 130 + numLines * 30);

		// if the player is hovering over the button
		if (Main.MouseScreen.Between(position, position + stringSize * scale * value2.X) && !PlayerInput.IgnoreMouseInterface) {
			Main.LocalPlayer.mouseInterface = true;
			Main.LocalPlayer.releaseUseItem = false;
			scale *= 1.2f; // make button bigger

			if (!focused) {
				SoundEngine.PlaySound(SoundID.MenuTick);
			}

			focused = true;
		}
		else {
			if (focused) {
				SoundEngine.PlaySound(SoundID.MenuTick);
			}

			focused = false;
		}

		// draw button shadow
		ChatManager.DrawColorCodedStringShadow(spriteBatch: spriteBatch,
											   font: FontAssets.MouseText.Value,
											   text: text,
											   position: position + stringSize * value2 * 0.5f,
											   baseColor: (!focused) ? Color.Black : Color.Brown,
											   rotation: 0f,
											   origin: stringSize * 0.5f,
											   baseScale: scale * value2);

		// draw button text
		ChatManager.DrawColorCodedString(spriteBatch: spriteBatch,
										 font: FontAssets.MouseText.Value,
										 text: text,
										 position: position + stringSize * value2 * 0.5f,
										 baseColor: !focused ? col : Main.OurFavoriteColor,
										 rotation: 0f,
										 origin: stringSize * 0.5f,
										 baseScale: scale);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);

		// if the shop button is clicked
		if (focused && Main.mouseLeft) {
			OpenShop(99);
		}
	}

	// copied from Main.OpenShop (except the line with the comment)
	internal static void OpenShop(int shopIndex)
	{
		Main.playerInventory = true;
		Main.stackSplit = 9999;
		Main.npcChatText = "";
		Main.SetNPCShopIndex(shopIndex);
		//shop[npcShop].SetupShop(npcShop);
		SetupShop(Main.instance.shop[99]); // calling my own SetupShop method
		SoundEngine.PlaySound(SoundID.MenuTick);
	}

	private static void SetupShop(Chest shop)
	{
		// all items being sold:
		short[] itemids = { ItemID.SonarPotion, ItemID.FishingPotion, ItemID.CratePotion, ItemID.ApprenticeBait, ItemID.JourneymanBait, ItemID.MasterBait, ItemID.FuzzyCarrot, ItemID.AnglerHat, ItemID.AnglerVest, ItemID.AnglerPants,
								ItemID.GoldenFishingRod, ItemID.GoldenBugNet, ItemID.FishHook, ItemID.HighTestFishingLine, ItemID.AnglerEarring, ItemID.TackleBox, ItemID.FishermansGuide, ItemID.WeatherRadio, ItemID.Sextant, ItemID.SeashellHairpin,
								ItemID.MermaidAdornment, ItemID.MermaidTail, ItemID.FishCostumeMask, ItemID.FishCostumeShirt, ItemID.FishCostumeFinskirt, ItemID.BunnyfishTrophy, ItemID.GoldfishTrophy, ItemID.SharkteethTrophy, ItemID.TreasureMap,
								ItemID.SeaweedPlanter, ItemID.PillaginMePixels, ItemID.CompassRose, ItemID.ShipsWheel, ItemID.ShipInABottle, ItemID.LifePreserver, ItemID.WallAnchor, ItemID.None, ItemID.None, ItemID.None, ItemID.None };

		// some items are only sold if WoF has been defeated
		if (Main.hardMode) {
			itemids[^4] = ItemID.FinWings;
			itemids[^3] = ItemID.BottomlessBucket;
			itemids[^2] = ItemID.SuperAbsorbantSponge;
			itemids[^1] = ItemID.HotlineFishingHook;
		}

		// add items to the list
		for (int i = 0; i < itemids.Length; i++) {
			shop.item[i].SetDefaults(itemids[i]);
			shop.item[i].isAShopItem = true;

			if (itemids[i] is ItemID.ApprenticeBait or ItemID.JourneymanBait or ItemID.MasterBait) {
				shop.item[i].value *= 2;
			}
		}
	}
}
