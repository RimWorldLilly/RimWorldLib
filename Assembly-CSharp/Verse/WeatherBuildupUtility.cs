using System;

namespace Verse
{
	// Token: 0x02000313 RID: 787
	public static class WeatherBuildupUtility
	{
		// Token: 0x06001822 RID: 6178 RVA: 0x00090D33 File Offset: 0x0008EF33
		public static WeatherBuildupCategory GetBuildupCategory(float depth)
		{
			if (depth < 0.03f)
			{
				return WeatherBuildupCategory.None;
			}
			if (depth < 0.25f)
			{
				return WeatherBuildupCategory.Dusting;
			}
			if (depth < 0.5f)
			{
				return WeatherBuildupCategory.Thin;
			}
			if (depth < 0.75f)
			{
				return WeatherBuildupCategory.Medium;
			}
			return WeatherBuildupCategory.Thick;
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x00090D60 File Offset: 0x0008EF60
		public static string GetSnowDescription(WeatherBuildupCategory category)
		{
			switch (category)
			{
			case WeatherBuildupCategory.None:
				return "SnowNone".Translate();
			case WeatherBuildupCategory.Dusting:
				return "SnowDusting".Translate();
			case WeatherBuildupCategory.Thin:
				return "SnowThin".Translate();
			case WeatherBuildupCategory.Medium:
				return "SnowMedium".Translate();
			case WeatherBuildupCategory.Thick:
				return "SnowThick".Translate();
			default:
				return "Unknown snow";
			}
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x00090DE0 File Offset: 0x0008EFE0
		public static string GetSandDescription(WeatherBuildupCategory category)
		{
			switch (category)
			{
			case WeatherBuildupCategory.None:
				return "SandNone".Translate();
			case WeatherBuildupCategory.Dusting:
				return "SandDusting".Translate();
			case WeatherBuildupCategory.Thin:
				return "SandThin".Translate();
			case WeatherBuildupCategory.Medium:
				return "SandMedium".Translate();
			case WeatherBuildupCategory.Thick:
				return "SandThick".Translate();
			default:
				return "Unknown sand";
			}
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x00090E5E File Offset: 0x0008F05E
		public static int MovementTicksAddOn(WeatherBuildupCategory category)
		{
			switch (category)
			{
			case WeatherBuildupCategory.None:
				return 0;
			case WeatherBuildupCategory.Dusting:
				return 0;
			case WeatherBuildupCategory.Thin:
				return 4;
			case WeatherBuildupCategory.Medium:
				return 8;
			case WeatherBuildupCategory.Thick:
				return 12;
			default:
				return 0;
			}
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x00090E88 File Offset: 0x0008F088
		public static void AddSnowRadial(IntVec3 center, Map map, float radius, float depth)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					float lengthHorizontal = (center - intVec).LengthHorizontal;
					float num2 = 1f - lengthHorizontal / radius;
					map.snowGrid.AddDepth(intVec, num2 * depth);
				}
			}
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x00090EF0 File Offset: 0x0008F0F0
		public static void AddSandRadial(IntVec3 center, Map map, float radius, float depth)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					float lengthHorizontal = (center - intVec).LengthHorizontal;
					float num2 = 1f - lengthHorizontal / radius;
					map.sandGrid.AddDepth(intVec, num2 * depth);
				}
			}
		}
	}
}
