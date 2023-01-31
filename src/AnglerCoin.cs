using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria.GameContent.UI;

namespace NoFishingQuests;

public class AnglerCoin : CustomCurrencySingleCoin
{
	public static int Id;
	
	public AnglerCoin(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
	{
		CurrencyTextKey = "Angler Coin";
		CurrencyTextColor = Color.Orange;
	}
}