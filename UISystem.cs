using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace NoFishingQuests;

internal class UISystem : ModSystem
{
	internal UserInterface UserInterface;
	private UIState UI;

	public override void Load()
	{
		if (!Main.dedServ)
		{
			UI = new NoFishingQuestsUI();
			UI.Activate();
			UserInterface = new UserInterface();
			UserInterface.SetState(UI);
		}
	}

	public override void Unload()
	{
		UI = null;
		UserInterface = null;
	}

	private GameTime _lastUpdateUiGameTime;
	public override void UpdateUI(GameTime gameTime)
	{
		_lastUpdateUiGameTime = gameTime;

		// if the player is talking to the Angler and the new shop isn't opened
		if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.Angler && Main.npcShop != 99)
		{
			UserInterface.Update(gameTime);
		}
	}

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
		if (mouseTextIndex != -1)
		{
			layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
				"NoFishingQuests: UI",
				delegate
				{
						// if the player is talking to the Angler and the new shop isn't opened
						if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCID.Angler && Main.npcShop != 99)
					{
						UserInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
					}
					return true;
				}, InterfaceScaleType.UI));
		}
	}
}
