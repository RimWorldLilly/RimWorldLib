using System;

namespace Verse
{
	// Token: 0x02000447 RID: 1095
	public static class HediffGrowthModeUtility
	{
		// Token: 0x06001F61 RID: 8033 RVA: 0x000BECE8 File Offset: 0x000BCEE8
		public static string GetLabel(this HediffGrowthMode m)
		{
			switch (m)
			{
			case HediffGrowthMode.Growing:
				return "HediffGrowthMode_Growing".Translate();
			case HediffGrowthMode.Stable:
				return "HediffGrowthMode_Stable".Translate();
			case HediffGrowthMode.Remission:
				return "HediffGrowthMode_Remission".Translate();
			default:
				throw new ArgumentException();
			}
		}
	}
}
