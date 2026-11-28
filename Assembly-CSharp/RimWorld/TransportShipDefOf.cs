using System;

namespace RimWorld
{
	// Token: 0x02001EDA RID: 7898
	[DefOf]
	public static class TransportShipDefOf
	{
		// Token: 0x0600CA94 RID: 51860 RVA: 0x004A6296 File Offset: 0x004A4496
		static TransportShipDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TransportShipDefOf));
		}

		// Token: 0x04008063 RID: 32867
		[MayRequireRoyalty]
		public static TransportShipDef Ship_Shuttle;

		// Token: 0x04008064 RID: 32868
		[MayRequireBiotech]
		public static TransportShipDef Ship_ShuttleCrashing;

		// Token: 0x04008065 RID: 32869
		[MayRequireBiotech]
		public static TransportShipDef Ship_ShuttleCrashing_Mechanitor;
	}
}
