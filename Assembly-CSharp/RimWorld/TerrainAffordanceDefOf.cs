using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EC5 RID: 7877
	[DefOf]
	public static class TerrainAffordanceDefOf
	{
		// Token: 0x0600CA80 RID: 51840 RVA: 0x004A6142 File Offset: 0x004A4342
		static TerrainAffordanceDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
		}

		// Token: 0x04007F32 RID: 32562
		public static TerrainAffordanceDef Light;

		// Token: 0x04007F33 RID: 32563
		public static TerrainAffordanceDef Medium;

		// Token: 0x04007F34 RID: 32564
		public static TerrainAffordanceDef Heavy;

		// Token: 0x04007F35 RID: 32565
		public static TerrainAffordanceDef SmoothableStone;

		// Token: 0x04007F36 RID: 32566
		public static TerrainAffordanceDef MovingFluid;

		// Token: 0x04007F37 RID: 32567
		public static TerrainAffordanceDef Bridgeable;

		// Token: 0x04007F38 RID: 32568
		public static TerrainAffordanceDef Walkable;
	}
}
