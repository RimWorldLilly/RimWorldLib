using System;

namespace RimWorld.Planet
{
	// Token: 0x020022FB RID: 8955
	public static class OverallPopulationUtility
	{
		// Token: 0x170026A5 RID: 9893
		// (get) Token: 0x0600DD4D RID: 56653 RVA: 0x00526420 File Offset: 0x00524620
		public static int EnumValuesCount
		{
			get
			{
				if (OverallPopulationUtility.cachedEnumValuesCount < 0)
				{
					OverallPopulationUtility.cachedEnumValuesCount = Enum.GetNames(typeof(OverallPopulation)).Length;
				}
				return OverallPopulationUtility.cachedEnumValuesCount;
			}
		}

		// Token: 0x0600DD4E RID: 56654 RVA: 0x00526448 File Offset: 0x00524648
		public static float GetScaleFactor(this OverallPopulation population)
		{
			switch (population)
			{
			case OverallPopulation.AlmostNone:
				return 0.1f;
			case OverallPopulation.Little:
				return 0.4f;
			case OverallPopulation.LittleBitLess:
				return 0.7f;
			case OverallPopulation.Normal:
				return 1f;
			case OverallPopulation.LittleBitMore:
				return 1.5f;
			case OverallPopulation.High:
				return 2f;
			case OverallPopulation.VeryHigh:
				return 2.75f;
			default:
				return 1f;
			}
		}

		// Token: 0x04008DE2 RID: 36322
		private static int cachedEnumValuesCount = -1;

		// Token: 0x04008DE3 RID: 36323
		private const float ScaleFactor_AlmostNone = 0.1f;

		// Token: 0x04008DE4 RID: 36324
		private const float ScaleFactor_Little = 0.4f;

		// Token: 0x04008DE5 RID: 36325
		private const float ScaleFactor_LittleBitLess = 0.7f;

		// Token: 0x04008DE6 RID: 36326
		private const float ScaleFactor_Normal = 1f;

		// Token: 0x04008DE7 RID: 36327
		private const float ScaleFactor_LittleBitMore = 1.5f;

		// Token: 0x04008DE8 RID: 36328
		private const float ScaleFactor_High = 2f;

		// Token: 0x04008DE9 RID: 36329
		private const float ScaleFactor_VeryHigh = 2.75f;
	}
}
