using System;

namespace RimWorld.Planet
{
	// Token: 0x02002358 RID: 9048
	public static class LandmarkDensityUtility
	{
		// Token: 0x17002768 RID: 10088
		// (get) Token: 0x0600E043 RID: 57411 RVA: 0x00536AF6 File Offset: 0x00534CF6
		public static int EnumValuesCount
		{
			get
			{
				if (LandmarkDensityUtility.cachedEnumValuesCount < 0)
				{
					LandmarkDensityUtility.cachedEnumValuesCount = Enum.GetNames(typeof(LandmarkDensity)).Length;
				}
				return LandmarkDensityUtility.cachedEnumValuesCount;
			}
		}

		// Token: 0x0600E044 RID: 57412 RVA: 0x00536B1C File Offset: 0x00534D1C
		public static float GetScaleFactor(this LandmarkDensity density)
		{
			switch (density)
			{
			case LandmarkDensity.Sparse:
				return 0.05f;
			case LandmarkDensity.SlightlyMoreSparse:
				return 0.33f;
			case LandmarkDensity.SlightlySparse:
				return 0.66f;
			case LandmarkDensity.Normal:
				return 1f;
			case LandmarkDensity.SlightlyCrowded:
				return 1.5f;
			case LandmarkDensity.SlightlyMoreCrowded:
				return 2f;
			case LandmarkDensity.Crowded:
				return 3f;
			default:
				return 1f;
			}
		}

		// Token: 0x04008FED RID: 36845
		private static int cachedEnumValuesCount = -1;

		// Token: 0x04008FEE RID: 36846
		private const float ScaleFactor_Sparse = 0.05f;

		// Token: 0x04008FEF RID: 36847
		private const float ScaleFactor_SlightlyMoreSparse = 0.33f;

		// Token: 0x04008FF0 RID: 36848
		private const float ScaleFactor_SlightlySparse = 0.66f;

		// Token: 0x04008FF1 RID: 36849
		private const float ScaleFactor_Normal = 1f;

		// Token: 0x04008FF2 RID: 36850
		private const float ScaleFactor_SlightlyCrowded = 1.5f;

		// Token: 0x04008FF3 RID: 36851
		private const float ScaleFactor_SlightlyMoreCrowded = 2f;

		// Token: 0x04008FF4 RID: 36852
		private const float ScaleFactor_Crowded = 3f;
	}
}
