using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001146 RID: 4422
	public static class HateChantDroneLevelUtility
	{
		// Token: 0x06006F08 RID: 28424 RVA: 0x0026E304 File Offset: 0x0026C504
		public static string GetLabel(this HateChantDroneLevel level)
		{
			switch (level)
			{
			case HateChantDroneLevel.None:
				return "HateChantDroneLevel_None".Translate();
			case HateChantDroneLevel.VeryLow:
				return "HateChantDroneLevel_VeryLow".Translate();
			case HateChantDroneLevel.Low:
				return "HateChantDroneLevel_Low".Translate();
			case HateChantDroneLevel.Medium:
				return "HateChantDroneLevel_Medium".Translate();
			case HateChantDroneLevel.High:
				return "HateChantDroneLevel_High".Translate();
			case HateChantDroneLevel.Extreme:
				return "HateChantDroneLevel_Extreme".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x06006F09 RID: 28425 RVA: 0x0026E396 File Offset: 0x0026C596
		public static string GetLabelCap(this HateChantDroneLevel level)
		{
			return level.GetLabel().CapitalizeFirst();
		}
	}
}
