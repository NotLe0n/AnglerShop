using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NoFishingQuests
{
    class NoFishingQuestsUI : UIState
    {
        private static object TextDisplayCache => typeof(Main).GetField("_textDisplayCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(Main.instance);
        private bool focused;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 scale = new Vector2(0.9f);
            string text = Language.GetTextValue("LegacyInterface.28");
            int numLines = (int)TextDisplayCache.GetType().GetProperty("AmountOfLines", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).GetValue(TextDisplayCache) + 1;
            Color col = new Color(228, 206, 114, Main.mouseTextColor / 2);
            Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, scale);

            Vector2 value2 = new Vector2(1f);
            if (stringSize.X > 260f)
                value2.X *= 260f / stringSize.X;

            float posButton1 = 180 + (Main.screenWidth - 800) / 2;
            float posButton2 = posButton1 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.64"), scale).X + 30f;
            float posButton3 = posButton2 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("LegacyInterface.52"), scale).X + 30f;
            float posButton4 = posButton3 + ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("UI.NPCCheckHappiness"), scale).X + 30f;
            Vector2 position = new Vector2(posButton4, 130 + numLines * 30);

            if (Main.MouseScreen.Between(position, position + stringSize * scale * value2.X) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.LocalPlayer.releaseUseItem = false;
                scale *= 1.2f;
                if (!focused)
                    SoundEngine.PlaySound(12);

                focused = true;
            }
            else
            {
                if (focused)
                    SoundEngine.PlaySound(12);

                focused = false;
            }

            ChatManager.DrawColorCodedStringShadow(spriteBatch: spriteBatch,
                                                   font: FontAssets.MouseText.Value,
                                                   text: text,
                                                   position: position + stringSize * value2 * 0.5f,
                                                   baseColor: (!focused) ? Color.Black : Color.Brown,
                                                   rotation: 0f,
                                                   origin: stringSize * 0.5f,
                                                   baseScale: scale * value2);

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

            if (focused)
            {
                if (Main.mouseLeft)
                {
                    OpenShop(99);
                }
            }
        }

        // copied from vanilla (except the line with the comment)
        private static void OpenShop(int shopIndex)
        {
            Main.playerInventory = true;
            Main.stackSplit = 9999;
            Main.npcChatText = "";
            Main.SetNPCShopIndex(shopIndex);
            /*shop[npcShop].SetupShop(npcShop);*/
            SetupShop(Main.instance.shop[99]);
            SoundEngine.PlaySound(12);
        }

        private static void SetupShop(Chest shop)
        {
            int nextSlot = 0;

            var itemids = new List<short> { ItemID.SonarPotion, ItemID.FishingPotion, ItemID.CratePotion, ItemID.ApprenticeBait, ItemID.JourneymanBait, ItemID.MasterBait, ItemID.FuzzyCarrot, ItemID.AnglerHat, ItemID.AnglerVest, ItemID.AnglerPants,
                                            ItemID.GoldenFishingRod, ItemID.GoldenBugNet, ItemID.FishHook, ItemID.HighTestFishingLine, ItemID.AnglerEarring, ItemID.TackleBox, ItemID.FishermansGuide, ItemID.WeatherRadio, ItemID.Sextant, ItemID.SeashellHairpin,
                                            ItemID.MermaidAdornment, ItemID.MermaidTail, ItemID.FishCostumeMask, ItemID.FishCostumeShirt, ItemID.FishCostumeFinskirt, ItemID.BunnyfishTrophy, ItemID.GoldfishTrophy, ItemID.SharkteethTrophy, ItemID.TreasureMap,
                                            ItemID.SeaweedPlanter, ItemID.PillaginMePixels, ItemID.CompassRose, ItemID.ShipsWheel, ItemID.ShipInABottle, ItemID.LifePreserver, ItemID.WallAnchor };
            if (Main.hardMode)
            {
                itemids.Add(ItemID.FinWings);
                itemids.Add(ItemID.BottomlessBucket);
                itemids.Add(ItemID.SuperAbsorbantSponge);
                itemids.Add(ItemID.HotlineFishingHook);
            }
            foreach (int element in itemids)
            {
                shop.item[nextSlot].SetDefaults(element);
                nextSlot++;
            }
        }
    }
}
