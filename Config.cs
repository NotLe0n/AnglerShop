using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace NoFishingQuests;

public class Config : ModConfig
{
	public static Config Instance;
	
	public override ConfigScope Mode => ConfigScope.ServerSide;

	[Label("Use custom currency")]
	[Tooltip("Toggles if the angler's shop should use a custom currency or normal coins.")]
	[DefaultValue(false)]
	[ReloadRequired]
	public bool useCustomCurrency;

	public override void OnLoaded()
	{
		Instance = this;
		base.OnLoaded();
	}
}