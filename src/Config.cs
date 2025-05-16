using System.ComponentModel;
using Terraria.ModLoader.Config;

// ReSharper disable UnassignedField.Global

namespace NoFishingQuests;

public class Config : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ServerSide;
	
	[DefaultValue(false)]
	[ReloadRequired]
	public bool useCustomCurrency;

	[DefaultValue(1)]
	[Slider]
	[Range(1, 10)]
	public int minAnglerCoins;

	[DefaultValue(5)]
	[Slider]
	[Range(1, 10)]
	public int maxAnglerCoins;
	
	[DefaultValue(2)]
	[Slider]
	[Range(0.5f, 10f)]
	[ReloadRequired]
	public float goldMultiplier;

	public override void OnChanged()
	{
		if (minAnglerCoins > maxAnglerCoins) {
			minAnglerCoins = maxAnglerCoins;
		}
		base.OnChanged();
	}
}