using System;

namespace RimWorld
{
	// Token: 0x02001F06 RID: 7942
	[DefOf]
	public static class PlanetLayerDefOf
	{
		// Token: 0x0600CABE RID: 51902 RVA: 0x004A6560 File Offset: 0x004A4760
		static PlanetLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PlanetLayerDefOf));
		}

		// Token: 0x0400812F RID: 33071
		public static PlanetLayerDef Surface;

		// Token: 0x04008130 RID: 33072
		[MayRequireOdyssey]
		public static PlanetLayerDef Orbit;
	}
}
