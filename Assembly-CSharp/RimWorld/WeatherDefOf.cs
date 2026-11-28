using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EA8 RID: 7848
	[DefOf]
	public static class WeatherDefOf
	{
		// Token: 0x0600CA63 RID: 51811 RVA: 0x004A5F55 File Offset: 0x004A4155
		static WeatherDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeatherDefOf));
		}

		// Token: 0x04007E3E RID: 32318
		public static WeatherDef Clear;

		// Token: 0x04007E3F RID: 32319
		public static WeatherDef Fog;

		// Token: 0x04007E40 RID: 32320
		public static WeatherDef FoggyRain;

		// Token: 0x04007E41 RID: 32321
		[MayRequireAnomaly]
		public static WeatherDef GrayPall;

		// Token: 0x04007E42 RID: 32322
		[MayRequireAnomaly]
		public static WeatherDef UnnaturalDarkness_Stage1;

		// Token: 0x04007E43 RID: 32323
		[MayRequireAnomaly]
		public static WeatherDef UnnaturalDarkness_Stage2;
	}
}
