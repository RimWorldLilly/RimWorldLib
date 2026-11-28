using System;

namespace RimWorld
{
	// Token: 0x02001E8B RID: 7819
	[DefOf]
	public static class BiomeDefOf
	{
		// Token: 0x0600CA46 RID: 51782 RVA: 0x004A5D68 File Offset: 0x004A3F68
		static BiomeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BiomeDefOf));
		}

		// Token: 0x04007AA2 RID: 31394
		public static BiomeDef IceSheet;

		// Token: 0x04007AA3 RID: 31395
		public static BiomeDef Tundra;

		// Token: 0x04007AA4 RID: 31396
		public static BiomeDef BorealForest;

		// Token: 0x04007AA5 RID: 31397
		public static BiomeDef TemperateForest;

		// Token: 0x04007AA6 RID: 31398
		public static BiomeDef Desert;

		// Token: 0x04007AA7 RID: 31399
		public static BiomeDef SeaIce;

		// Token: 0x04007AA8 RID: 31400
		public static BiomeDef Ocean;

		// Token: 0x04007AA9 RID: 31401
		public static BiomeDef Lake;

		// Token: 0x04007AAA RID: 31402
		public static BiomeDef ColdBog;

		// Token: 0x04007AAB RID: 31403
		public static BiomeDef TropicalRainforest;

		// Token: 0x04007AAC RID: 31404
		public static BiomeDef TropicalSwamp;

		// Token: 0x04007AAD RID: 31405
		[MayRequireAnomaly]
		public static BiomeDef Undercave;

		// Token: 0x04007AAE RID: 31406
		[MayRequireOdyssey]
		public static BiomeDef Scarlands;

		// Token: 0x04007AAF RID: 31407
		[MayRequireOdyssey]
		public static BiomeDef Space;

		// Token: 0x04007AB0 RID: 31408
		[MayRequireOdyssey]
		public static BiomeDef Orbit;

		// Token: 0x04007AB1 RID: 31409
		[MayRequireOdyssey]
		public static BiomeDef Glowforest;
	}
}
