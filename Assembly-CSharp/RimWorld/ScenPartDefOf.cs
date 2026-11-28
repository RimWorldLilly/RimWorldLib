using System;

namespace RimWorld
{
	// Token: 0x02001E76 RID: 7798
	[DefOf]
	public static class ScenPartDefOf
	{
		// Token: 0x0600CA31 RID: 51761 RVA: 0x004A5C03 File Offset: 0x004A3E03
		static ScenPartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenPartDefOf));
		}

		// Token: 0x0400779A RID: 30618
		public static ScenPartDef PlayerFaction;

		// Token: 0x0400779B RID: 30619
		public static ScenPartDef ConfigPage_ConfigureStartingPawns;

		// Token: 0x0400779C RID: 30620
		public static ScenPartDef PlayerPawnsArriveMethod;

		// Token: 0x0400779D RID: 30621
		public static ScenPartDef PlanetLayerFixed;
	}
}
