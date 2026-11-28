using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C1 RID: 5057
	public static class ZonePresetNames
	{
		// Token: 0x06007D59 RID: 32089 RVA: 0x002CD6B0 File Offset: 0x002CB8B0
		public static string PresetName(this StorageSettingsPreset preset)
		{
			if (preset == StorageSettingsPreset.DumpingStockpile)
			{
				return "DumpingStockpile".Translate();
			}
			if (preset == StorageSettingsPreset.DefaultStockpile)
			{
				return "Stockpile".Translate();
			}
			if (preset == StorageSettingsPreset.CorpseStockpile)
			{
				return "CorpseStockpile".Translate();
			}
			return "Zone".Translate();
		}
	}
}
