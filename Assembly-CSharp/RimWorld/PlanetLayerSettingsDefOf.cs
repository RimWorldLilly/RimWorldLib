using System;

namespace RimWorld
{
	// Token: 0x02001F05 RID: 7941
	[DefOf]
	public static class PlanetLayerSettingsDefOf
	{
		// Token: 0x0600CABD RID: 51901 RVA: 0x004A654F File Offset: 0x004A474F
		static PlanetLayerSettingsDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PlanetLayerSettingsDefOf));
		}

		// Token: 0x0400812D RID: 33069
		public static PlanetLayerSettingsDef Surface;

		// Token: 0x0400812E RID: 33070
		[MayRequireOdyssey]
		public static PlanetLayerSettingsDef Orbit;
	}
}
