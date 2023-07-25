using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NoFishingQuests;

internal class AnglerShopUI : UIState
{
	// Main._textDisplayCache
	private static object TextDisplayCache => typeof(Main).GetField("_textDisplayCache", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Main.instance);
	private bool focused; // true if the shop button is hovered

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (Main.npcChatText == string.Empty)
			return;
		
		base.Draw(spriteBatch);

		// Source: Main.DrawNPCChatButtons

		Vector2 scale = new(0.9f); // scale of the button
		string text = Language.GetTextValue("LegacyInterface.28"); // "Shop"
		// How long the npc text is (varies with language)
		int numLines = (int)TextDisplayCache.GetType().GetProperty("AmountOfLines", BindingFlags.Instance | BindingFlags.Public).GetValue(TextDisplayCache);
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
			baseColor: !focused ? new Color(228, 206, 114, Main.mouseTextColor / 2) : new Color(255, 231, 69), // color of the button text
			rotation: 0f,
			origin: stringSize * 0.5f,
			baseScale: scale);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);

		// if the shop button is clicked
		if (focused && Main.mouseLeft) {
			OpenShop();
		}
	}

	// copied from Main.OpenShop (except the line with the comment)
	internal static void OpenShop()
	{
		Main.playerInventory = true;
		Main.stackSplit = 9999;
		Main.npcChatText = "";
		Main.SetNPCShopIndex(1);
		//Main.instance.shop[Main.npcShop].SetupShop(shopIndex);
		Main.instance.shop[Main.npcShop].SetupShop("Terraria/Angler/Shop", Main.LocalPlayer.TalkNPC); // Setup Angler shop (gets filled in ModifyShops.cs) 
		SoundEngine.PlaySound(SoundID.MenuTick);
	}
}
