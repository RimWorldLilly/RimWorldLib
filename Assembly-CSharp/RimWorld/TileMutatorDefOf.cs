using System;

namespace RimWorld
{
	// Token: 0x02001F03 RID: 7939
	[DefOf]
	public static class TileMutatorDefOf
	{
		// Token: 0x0600CABB RID: 51899 RVA: 0x004A652D File Offset: 0x004A472D
		static TileMutatorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TileMutatorDefOf));
		}

		// Token: 0x0400811C RID: 33052
		public static TileMutatorDef Mountain;

		// Token: 0x0400811D RID: 33053
		public static TileMutatorDef Coast;

		// Token: 0x0400811E RID: 33054
		public static TileMutatorDef River;

		// Token: 0x0400811F RID: 33055
		public static TileMutatorDef Caves;

		// Token: 0x04008120 RID: 33056
		[MayRequireOdyssey]
		public static TileMutatorDef Lakeshore;

		// Token: 0x04008121 RID: 33057
		[MayRequireOdyssey]
		public static TileMutatorDef RiverConfluence;

		// Token: 0x04008122 RID: 33058
		[MayRequireOdyssey]
		public static TileMutatorDef Headwater;

		// Token: 0x04008123 RID: 33059
		[MayRequireOdyssey]
		public static TileMutatorDef RiverIsland;

		// Token: 0x04008124 RID: 33060
		[MayRequireOdyssey]
		public static TileMutatorDef RiverDelta;

		// Token: 0x04008125 RID: 33061
		[MayRequireOdyssey]
		public static TileMutatorDef Fjord;

		// Token: 0x04008126 RID: 33062
		[MayRequireOdyssey]
		public static TileMutatorDef AnimalHabitat;

		// Token: 0x04008127 RID: 33063
		[MayRequireOdyssey]
		public static TileMutatorDef Crevasse;

		// Token: 0x04008128 RID: 33064
		[MayRequireOdyssey]
		public static TileMutatorDef MixedBiome;
	}
}
